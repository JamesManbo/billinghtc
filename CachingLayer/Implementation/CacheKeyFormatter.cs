using Global.Models.Filter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CachingLayer.Implementation
{
    public class CacheKeyFormatter
    {
        public static void AppendTypeName(StringBuilder stringBuilder, Type declaringType)
        {
            stringBuilder.Append(declaringType.FullName);
            if (declaringType.IsGenericType)
            {
                var genericArguments = declaringType.GetGenericArguments();
                AppendGenericArguments(stringBuilder, genericArguments);
            }
        }

        public static void AppendGenericArguments(StringBuilder stringBuilder, Type[] genericArguments)
        {
            stringBuilder.Append('<');
            for (var i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(genericArguments[i].Name);
            }
            stringBuilder.Append('>');
        }

        public static void AppendArguments(StringBuilder stringBuilder, object[] arguments)
        {
            stringBuilder.Append('(');
            for (var i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == null) continue;
                if (arguments[i].GetType().IsValueType)
                {
                    if (i > 0) stringBuilder.Append(",");
                    stringBuilder.Append(arguments[i]);
                }
                else
                {
                    var refernceTypeSerialization = ReferenceTypeFormat(arguments[i]);
                    if (!string.IsNullOrEmpty(refernceTypeSerialization))
                    {
                        if (i > 0) stringBuilder.Append(",");
                        stringBuilder.Append(refernceTypeSerialization);
                    }
                }
            }
            stringBuilder.Append(')');
        }

        private static string ReferenceTypeFormat(object obj)
        {
            var stringBuilder = new StringBuilder();
            if (obj.GetType().IsValueType)
            {
                stringBuilder.Append(obj);
            }
            else if (obj is string)
            {
                if (string.IsNullOrEmpty(obj?.ToString())) return "";
                stringBuilder.Append(obj);
            }
            else if (obj.GetType().IsArray)
            {
                var argAsArray = obj as Array;
                if (argAsArray == null || argAsArray.Length == 0)
                    return "";

                stringBuilder.Append("[");
                foreach (var item in argAsArray)
                {
                    stringBuilder.Append(ReferenceTypeFormat(item));
                }

                stringBuilder.Append("]");
            }
            else if (obj is IDictionary)
            {
                var argAsDictionary = (IDictionary)obj;
                if (argAsDictionary != null)
                {
                    stringBuilder.Append("[");
                    var keyValuePairs = new List<string>();
                    foreach (var key in argAsDictionary.Keys)
                    {
                        var keyValue = argAsDictionary[key];
                        if (keyValue == null) continue;
                        keyValuePairs.Add($"{key}_{ReferenceTypeFormat(keyValue)}");
                    }
                    stringBuilder.Append(string.Join(",", keyValuePairs));
                    stringBuilder.Append("]");
                }
            }
            else if (obj is IList)
            {
                var argAsList = (IList)obj;

                if (argAsList == null || argAsList.Count == 0)
                    return "";

                stringBuilder.Append("[");
                foreach (var item in argAsList)
                {
                    stringBuilder.Append(ReferenceTypeFormat(item));
                }
                stringBuilder.Append("]");
            }
            else
            {
                var objType = obj.GetType();
                var argProperties = objType.GetProperties()
                    .OrderBy(e => e.Name)
                    .ToList();

                stringBuilder.Append($"{objType.FullName}{{");
                for (int j = 0; j < argProperties.Count - 1; j++)
                {
                    var argumentVal = argProperties[j].GetValue(obj);

                    if (argumentVal == null) continue;
                    stringBuilder.Append(ReferenceTypeFormat(argumentVal));
                }
                stringBuilder.Append("}");
            }

            return stringBuilder.ToString();
        }
    }
}
