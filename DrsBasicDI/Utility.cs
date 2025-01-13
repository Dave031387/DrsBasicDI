namespace DrsBasicDI;

internal static class Utility
{
    private static readonly Dictionary<Type, string> _typeTranslationDictionary = new()
    {
        {typeof(bool), "bool"},
        {typeof(byte), "byte"},
        {typeof(char), "char"},
        {typeof(decimal), "decimal"},
        {typeof(double), "double"},
        {typeof(float), "float"},
        {typeof(int), "int"},
        {typeof(long), "long"},
        {typeof(object), "object"},
        {typeof(sbyte), "sbyte"},
        {typeof(short), "short"},
        {typeof(string), "string"},
        {typeof(uint), "uint"},
        {typeof(ulong), "ulong"},
        {typeof(ushort), "ushort"},
        {typeof(void), "void"}
    };

    internal static string GetFriendlyName(this Type type)
    {
        if (_typeTranslationDictionary.TryGetValue(type, out string? predefinedTypeName))
        {
            return predefinedTypeName;
        }
        else if (type.IsArray)
        {
            int rank = type.GetArrayRank();
            char comma = ',';
            string commas = rank > 1
                ? new string(comma, rank - 1)
                : string.Empty;
            Type arrayElementType = type.GetElementType()!;
            string arrayElementTypeName = arrayElementType.GetFriendlyName();
            return $"{arrayElementTypeName}[{commas}]";
        }
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            Type nullableType = type.GetGenericArguments()[0];
            string nullableTypeName = nullableType.GetFriendlyName();
            return $"{nullableTypeName}?";
        }
        else if (type.IsGenericType)
        {
            char genericSeparator = '`';
            string listSeparator = ", ";
            string genericTypeName = type.Name.Split(genericSeparator)[0];
            Type[] genericParameterTypes = type.GetGenericArguments();
            string genericParameterTypeNames = string.Join(listSeparator, genericParameterTypes.Select(GetFriendlyName));
            return $"{genericTypeName}<{genericParameterTypeNames}>";
        }
        else
        {
            return type.Name;
        }
    }
}
