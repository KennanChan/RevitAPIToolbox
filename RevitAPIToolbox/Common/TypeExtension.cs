using System;
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
        //        types, null);
        //}

        //public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] types)
        //{
        //    foreach (var mi in type.GetMethods
        //        (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        //    {
        //        if (mi.Name != name) continue;
        //        if (!mi.IsGenericMethod) continue;
        //        if (mi.GetGenericArguments().Length != types.Length) continue;

        //        return mi.MakeGenericMethod(types);
        //    }

        //    throw new MissingMethodException();
        //}
    }
}