using System;
using System.Linq;
using System.Reflection;

namespace Techyard.Revit.Common
{
    public static class TypeExtension
    {
        //public static MethodInfo GetMethod(this Type type, string name, params Type[] types)
        //{
        //    return type.GetMethod(
        //        name,
        //        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
        //        null,
        //        types,
        //        null);
        //}

        //public static MethodInfo GetGenericMethod(this Type type, string name, Type[] genericTypes, Type[] paramTypes)
        //{
        //    var resultMethod = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        //        .FirstOrDefault(method =>
        //        {
        //            if (method.Name != name) return false;
        //            if (!method.IsGenericMethod) return false;
        //            var genericArgs = method.GetGenericArguments();
        //            if (genericArgs.Length != genericTypes.Length)
        //                return false;
        //            for (var i = 0; i < genericTypes.Length; i++)
        //            {
        //                if (genericArgs[i] != genericTypes[i])
        //                    return false;
        //            }
        //            var parameters = method.GetParameters();
        //            if (parameters.Length != paramTypes.Length)
        //                return false;
        //            for (var i = 0; i < paramTypes.Length; i++)
        //            {
        //                if (paramTypes[i] != parameters[i].ParameterType)
        //                    return false;
        //            }
        //            return true;
        //        })?.MakeGenericMethod(genericTypes);
        //    if (null == resultMethod)
        //        throw new MissingMethodException();
        //    return resultMethod;
        //}
    }
}