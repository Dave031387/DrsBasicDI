namespace DrsBasicDI;

using System.Reflection;

/// <summary>
/// The <see cref="Utility" /> class provides utility services for the rest of the classes in the
/// <see cref="DrsBasicDI" /> class library.
/// </summary>
internal static class Utility
{
    /// <summary>
    /// A dictionary of predefined types whose key is the predefined type and whose value is the
    /// friendly name for the predefined type.
    /// </summary>
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

    /// <summary>
    /// An extension method for the <see cref="Type" /> class that returns the constructor info for
    /// the <see cref="Type" />.
    /// </summary>
    /// <param name="type">
    /// The class type for which we want to retrieve the constructor info.
    /// </param>
    /// <returns>
    /// The <see cref="ConstructorInfo" /> object for the given class type.
    /// </returns>
    /// <remarks>
    /// If there is more than one constructor for the given class type, then the info for the
    /// constructor having the most parameters will be returned.
    /// </remarks>
    /// <exception cref="DependencyInjectionException" />
    internal static ConstructorInfo GetConstructorInfo(this Type type)
    {
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        ConstructorInfo[] constructorInfo = type.GetConstructors(bindingFlags);

        if (constructorInfo.Length < 1)
        {
            string msg = string.Format(Messages.MsgNoSuitableConstructors, type.GetFriendlyName());
            throw new DependencyInjectionException(msg);
        }

        int maxParameterCount = -1;
        int constructorIndex = -1;

        for (int i = 0; i < constructorInfo.Length; i++)
        {
            int parameterCount = constructorInfo[i].GetParameters().Length;

            if (parameterCount > maxParameterCount)
            {
                maxParameterCount = parameterCount;
                constructorIndex = i;
            }
        }

        return constructorInfo[constructorIndex];
    }

    /// <summary>
    /// An extension method for the <see cref="Type" /> class that returns a user-friendly name for
    /// the <see cref="Type" /> instance.
    /// </summary>
    /// <param name="type">
    /// The current instance of the <see cref="Type" /> class.
    /// </param>
    /// <returns>
    /// The friendly name for the current <see cref="Type" /> instance.
    /// </returns>
    /// <remarks>
    /// This method is recursive and is able to handle nested types.
    /// </remarks>
    internal static string GetFriendlyName(this Type type)
    {
        // Handle predefined types.
        if (_typeTranslationDictionary.TryGetValue(type, out string? predefinedTypeName))
        {
            return predefinedTypeName;
        }
        // Handle array types.
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
        // Handle nullable value types.
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            Type nullableType = type.GetGenericArguments()[0];
            string nullableTypeName = nullableType.GetFriendlyName();
            return $"{nullableTypeName}?";
        }
        // Handle generic types.
        else if (type.IsGenericType)
        {
            char genericSeparator = '`';
            string listSeparator = ", ";
            string genericTypeName = type.Name.Split(genericSeparator)[0];
            Type[] genericParameterTypes = type.GetGenericArguments();
            string genericParameterTypeNames = string.Join(listSeparator, genericParameterTypes.Select(GetFriendlyName));
            return $"{genericTypeName}<{genericParameterTypeNames}>";
        }
        // Everything else is a simple class type.
        else
        {
            return type.Name;
        }
    }
}