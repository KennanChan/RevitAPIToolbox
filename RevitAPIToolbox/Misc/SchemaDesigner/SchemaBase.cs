using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB.ExtensibleStorage;
using Techyard.Revit.Attributes;
using Techyard.Revit.Common;
using Techyard.Revit.Exceptions;

namespace Techyard.Revit.Misc.SchemaDesigner
{
    public abstract class SchemaBase
    {
        private IDictionary<string, PropertyInfo> _properties;
        private IDictionary<string, FieldInfo> _fields;
        private SchemaAttribute _schema;

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
                    _schema = schemaDefinition ?? throw new SchemaNotDefinedWithAttributeException();
                }
                return _schema;
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
        public Schema GetOrCreate()
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

        internal void ExtractData(Entity entity)
        {
            Properties.Keys.ToList().ForEach(key =>
            {
                var property = Properties[key];
                var value = typeof(Entity).GetMethod("Get", new[] { typeof(string) })
                    ?.MakeGenericMethod(property.PropertyType)
                    .Invoke(entity, new object[] { key });
                property.SetValue(this, value, null);
            });

            Fields.Keys.ToList().ForEach(key =>
            {
                var field = Fields[key];
                var value = typeof(Entity).GetMethod("Get", new[] { typeof(string) })?.MakeGenericMethod(field.FieldType)
                    .Invoke(entity, new object[] { key });
                field.SetValue(this, value);
            });
        }

        internal void FillData(Entity entity)
        {
            Properties.Keys.ToList().ForEach(key =>
            {
                var property = Properties[key];
                var value = property.GetValue(this, null);
                var method = typeof(Entity).GetMethod("Set",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null, new[] { typeof(string) }, null);
                var genericMethod = method?.MakeGenericMethod(property.PropertyType);
                genericMethod?.Invoke(entity, new[] { key, value });
            });

            Fields.Keys.ToList().ForEach(key =>
            {
                var field = Fields[key];
                var value = field.GetValue(this);
                var method = typeof(Entity).GetMethod("Set",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null, new[] { typeof(string) }, null);
                var genericMethod = method?.MakeGenericMethod(field.FieldType);
                genericMethod?.Invoke(entity, new[] { key, value });
            });
        }
    }
}