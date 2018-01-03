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
        private List<Tuple<string, PropertyInfo>> _properties;
        private List<Tuple<string, FieldInfo>> _fields;
        private SchemaAttribute _schema;
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

        /// <summary>
        ///     Get metadata of Autodesk.DB.ExtensibleStorage.Get<T>(string name) method
        /// </summary>
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

        /// <summary>
        ///     Get metadata of Autodesk.DB.ExtensibleStorage.Set<T>(string name,T value) method
        /// </summary>
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

        /// <summary>
        ///     Cache property metadata of user-difined schema
        /// </summary>
        private List<Tuple<string, PropertyInfo>> Properties =>
            _properties ??
            (_properties = GetType()
                .GetProperties()
                .Select(
                    p => new Tuple<string, PropertyInfo>(p.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                        .Cast<SchemaFieldAttribute>()
                        .FirstOrDefault()?.Name, p))
                .Where(pp => pp.Item1 != null)
                .ToList());

        private List<Tuple<string, FieldInfo>> Fields =>
            _fields ??
            (_fields = GetType()
                .GetFields()
                .Select(
                    f => new Tuple<string, FieldInfo>(f.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                        .Cast<SchemaFieldAttribute>()
                        .FirstOrDefault()?.Name, f))
                .Where(fp => fp.Item1 != null)
                .ToList());

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
                    builder.AddSimpleField(pp.Item1, pp.Item2.PropertyType);
                });

            //Get all attributed fields to build the schema
            Fields.ToList()
                .ForEach(fp =>
                {
                    builder.AddSimpleField(fp.Item1, fp.Item2.FieldType);
                });
            return builder.Finish();
        }

        /// <summary>
        ///     Extract data from the Entity object to current object
        /// </summary>
        /// <param name="entity"></param>
        internal void ExtractData(Entity entity)
        {
            Properties.ForEach(tuple =>
            {
                var name = tuple.Item1;
                var property = tuple.Item2;
                var genericMethod = EntityGet?.MakeGenericMethod(property.PropertyType);
                var value = genericMethod?.Invoke(entity, new object[] { name });
                property.SetValue(this, value, null);
            });

            Fields.ForEach(tuple =>
            {
                var name = tuple.Item1;
                var field = tuple.Item2;
                var genericMethod = EntityGet?.MakeGenericMethod(field.FieldType);
                var value = genericMethod?.Invoke(entity, new object[] { name });
                field.SetValue(this, value);
            });
        }

        /// <summary>
        ///     Fill data of current object to the Entity object
        /// </summary>
        /// <param name="entity"></param>
        internal void FillData(Entity entity)
        {
            Properties.ForEach(tuple =>
            {
                var name = tuple.Item1;
                var property = tuple.Item2;
                var value = property.GetValue(this, null);
                var genericMethod = EntitySet?.MakeGenericMethod(property.PropertyType);
                genericMethod?.Invoke(entity, new[] { name, value });
            });

            Fields.ForEach(tuple =>
            {
                var name = tuple.Item1;
                var field = tuple.Item2;
                var value = field.GetValue(this);
                var genericMethod = EntitySet?.MakeGenericMethod(field.FieldType);
                genericMethod?.Invoke(entity, new[] { name, value });
            });
        }
    }
}