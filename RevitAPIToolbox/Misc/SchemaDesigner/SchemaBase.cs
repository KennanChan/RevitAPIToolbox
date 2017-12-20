using System;
using System.Linq;
using Autodesk.Revit.DB.ExtensibleStorage;
using Techyard.Revit.Attributes;
using Techyard.Revit.Exceptions;

namespace Techyard.Revit.Misc.SchemaDesigner
{
    public abstract class SchemaBase
    {
        /// <summary>
        ///     Get or create the schema from class definition
        /// </summary>
        /// <returns></returns>
        public virtual Schema GetOrCreate()
        {
            var schemaDefinition =
                GetType()
                    .GetCustomAttributes(typeof(SchemaAttribute), false)
                    .Cast<SchemaAttribute>()
                    .FirstOrDefault();
            if (null == schemaDefinition)
            {
                throw new SchemaNotDefinedWithAttributeException();
            }
            var guid = new Guid(schemaDefinition.Guid);
            var schema = Schema.Lookup(guid);
            if (null != schema)
                return schema;
            var builder = new SchemaBuilder(guid);
            builder.SetSchemaName(schemaDefinition.Name);

            //Get all attributed properties to build the schema
            GetType()
                .GetProperties()
                .ToDictionary(p => p,
                    p =>
                        p.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                            .Cast<SchemaFieldAttribute>()
                            .FirstOrDefault()).Where(pp => pp.Value != null).ToList().ForEach(pp =>
                {
                    builder.AddSimpleField(pp.Value.Name, pp.Key.PropertyType);
                });

            //Get all attributed fields to build the schema
            GetType()
                .GetFields()
                .ToDictionary(f => f,
                    f =>
                        f.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                            .Cast<SchemaFieldAttribute>()
                            .FirstOrDefault()).Where(fp => fp.Value != null).ToList().ForEach(fp =>
                {
                    builder.AddSimpleField(fp.Value.Name, fp.Key.FieldType);
                });
            return builder.Finish();
        }
    }
}
