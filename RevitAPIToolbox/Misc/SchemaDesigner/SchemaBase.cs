using System.Linq;
using Autodesk.Revit.DB.ExtensibleStorage;
using Techyard.Revit.Attributes;

namespace Techyard.Revit.Misc.SchemaDesigner
{
    public abstract class SchemaBase
    {
        public virtual Schema Create()
        {
            var schemaDefinition =
                GetType()
                    .GetCustomAttributes(typeof(SchemaAttribute), false)
                    .Cast<SchemaAttribute>()
                    .FirstOrDefault();
            if (null == schemaDefinition)
            {
                throw 
            }
            GetType()
                .GetProperties()
                .ToDictionary(p => p,
                    p =>
                        p.GetCustomAttributes(typeof(SchemaFieldAttribute), false)
                            .Cast<SchemaFieldAttribute>()
                            .FirstOrDefault()).Where(p=>p.Value!=null).Select(p=>);
        }
    }
}
