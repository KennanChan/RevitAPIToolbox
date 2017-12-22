using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB.ExtensibleStorage;
using Techyard.Revit.Attributes;
using Techyard.Revit.Exceptions;

namespace Techyard.Revit.Misc.SchemaDesigner
{
    public abstract class SchemaBase
    {
        private IDictionary<string, PropertyInfo> _properties;
        private IDictionary<string, FieldInfo> _fields;
        private SchemaAttribute _schema;
        private Type _fieldType;
        private MethodInfo _entityGet;
        private MethodInfo _entitySet;

        private SchemaAttribute SchemaDefinition
        {
            get
            {
                if (null == _schema)
                {
                    var schemaDefinition =
                        GetType()
                            .GetCustomAttributes(typeof(SchemaAttribute), false)
                            .Cast<SchemaAttribute>()
                            .FirstOrDefault();
                    _schema = schemaDefinition;
                    if (null == _schema)
                        throw new SchemaNotDefinedWithAttributeException();
                }
                return _schema;
            }
        }

        private Type FieldType => _fieldType ?? (_fieldType = typeof(Entity).Assembly.GetType("FieldType"));

        private MethodInfo EntityGet
        {
            get
            {
                if (null == _entityGet)
                {
                    var type = typeof(Entity);
                    _entityGet = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(method =>
                        {
                            if (method.Name != "Get") return false;
                            if (!method.IsGenericMethod) return false;
                            var parameters = method.GetParameters();
                            if (parameters.Length != 1)
                                return false;
                            return typeof(string) == parameters[0].ParameterType;
                        });
                }
                return _entityGet;
            }
        }

        private MethodInfo EntitySet
        {
            get
            {
                if (null == _entitySet)
                {
                    var type = typeof(Entity);
                    _entitySet = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(method =>
                        {
                            if (method.Name != "Set") return false;
                            if (!method.IsGenericMethod) return false;
                            var parameters = method.GetParameters();
                            if (parameters.Length != 2)
                                return false;
                            return typeof(string) == parameters[0].ParameterType;
                        });
                }
                return _entitySet;
            }
        }

        private Guid SchemaId => new Guid(SchemaDefinition.Guid);

        private string SchemaName => SchemaDefinition.Name;

        private IDictionary<string, PropertyInfo> Properties =>
            _properties ??
            (_properties = GetType()
                .GetProperties()
                .ToDictionary(p => p,
                    p => p.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                        .Cast<SchemaFieldAttribute>()
                        .FirstOrDefault()?.Name)
                .Where(pp => pp.Value != null)
                .ToDictionary(pp => pp.Value, pp => pp.Key));

        private IDictionary<string, FieldInfo> Fields =>
            _fields ??
            (_fields = GetType()
                .GetFields()
                .ToDictionary(f => f,
                    f => f.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                        .Cast<SchemaFieldAttribute>()
                        .FirstOrDefault()?.Name)
                .Where(fp => fp.Value != null)
                .ToDictionary(fp => fp.Value, fp => fp.Key));

        /// <summary>
        ///     Get or create the schema from class definition
        /// </summary>
        /// <returns></returns>
        public Schema GetOrCreateSchema()
        {
            var schema = Schema.Lookup(SchemaId);
            if (null != schema)
                return schema;
            var builder = new SchemaBuilder(SchemaId);
            builder.SetSchemaName(SchemaName);

            //Get all attributed properties to build the schema
            Properties.ToList()
                .ForEach(pp =>
                {
                    builder.AddSimpleField(pp.Key, pp.Value.PropertyType);
                });

            //Get all attributed fields to build the schema
            Fields.ToList()
                .ForEach(fp =>
                {
                    builder.AddSimpleField(fp.Key, fp.Value.FieldType);
                });
            return builder.Finish();
        }

        /// <summary>
        ///     Extract data from the Entity object to current object
        /// </summary>
        /// <param name="entity"></param>
        internal void ExtractData(Entity entity)
        {
            Properties.Keys.ToList().ForEach(key =>
            {
                var property = Properties[key];
                var genericMethod = EntityGet?.MakeGenericMethod(property.PropertyType);
                var value = genericMethod?.Invoke(entity, new object[] { key });
                property.SetValue(this, value, null);
            });

            Fields.Keys.ToList().ForEach(key =>
            {
                var field = Fields[key];
                var genericMethod = EntityGet?.MakeGenericMethod(field.FieldType);
                var value = genericMethod?.Invoke(entity, new object[] { key });
                field.SetValue(this, value);
            });
        }

        /// <summary>
        ///     Fill data of current object to the Entity object
        /// </summary>
        /// <param name="entity"></param>
        internal void FillData(Entity entity)
        {
            Properties.Keys.ToList().ForEach(key =>
            {
                var property = Properties[key];
                var value = property.GetValue(this, null);
                var genericMethod = EntitySet?.MakeGenericMethod(property.PropertyType);
                genericMethod?.Invoke(entity, new[] { key, value });
            });

            Fields.Keys.ToList().ForEach(key =>
            {
                var field = Fields[key];
                var value = field.GetValue(this);
                var genericMethod = EntitySet?.MakeGenericMethod(field.FieldType);
                genericMethod?.Invoke(entity, new[] { key, value });
            });
        }
    }
}