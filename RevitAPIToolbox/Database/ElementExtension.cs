using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Techyard.Revit.Misc.SchemaDesigner;

namespace Techyard.Revit.Database
{
    public static class ElementExtension
    {
        public static ElementType GetElementType(this Element element)
        {
            return element.Document.GetElement(element.GetTypeId()) as ElementType;
        }

        public static T ReadData<T>(this Element element, T data) where T : SchemaBase
        {
            var schema = data.GetOrCreateSchema();
            var entity = element.GetEntity(schema);
            data.ExtractData(entity);
            return data;
        }

        public static T ReadData<T>(this Element element, Schema schema, string fieldName)
        {
            var field = schema.GetField(fieldName);
            var entity = element.GetEntity(schema);
            return entity.IsValid() ? entity.Get<T>(field) : default(T);
        }

        public static bool WriteData<T>(this Element element, T data, bool withTransaction = true) where T : SchemaBase
        {
            var action = new Action(() =>
            {
                var schema = data.GetOrCreateSchema();
                var entity = new Entity(schema);
                data.FillData(entity);
                element.SetEntity(entity);
            });
            try
            {
                if (withTransaction)
                    return element.Document.Modify(action, "Write entity");
                action();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool WriteData<T>(this Element element, Schema schema, string fieldName, T data,
            bool withTransaction = true)
            where T : class
        {
            var action = new Action(() =>
            {
                var entity = new Entity(schema);
                var field = schema.GetField(fieldName);
                entity.Set(field, data);
                element.SetEntity(entity);
            });
            try
            {
                if (withTransaction)
                    return element.Document.Modify(action, "Write entity");
                action();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}