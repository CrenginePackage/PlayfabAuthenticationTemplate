using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionUtility
{
    private static List<string> properties;
    private static PropertyInfo[] propertyInfos;

    public static List<string> GetProperties<T>(T type)
    {
        properties = new List<string>();
        propertyInfos = typeof(UserInformation).GetProperties();

        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            properties.Add(propertyInfo.Name);
        }

        return properties;
    }
}
