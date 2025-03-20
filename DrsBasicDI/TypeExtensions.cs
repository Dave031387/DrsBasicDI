namespace DrsBasicDI;

using System.Reflection;

/// <summary>
/// The <see cref="TypeExtensions" /> class extends the <see cref="Type" /> class by adding a couple
/// methods that are used by the <see cref="DrsBasicDI" /> class library.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// The <see cref="BindingFlags" /> used to find constructors for the implementation types.
    /// </summary>
    private const BindingFlags ConstructorBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// A dictionary of predefined types whose key is the predefined type and whose value is the
    /// friendly name for the predefined type.
    /// </summary>
    private static readonly Dictionary<Type, string> _typeTranslationDictionary = new()
    {
        {typeof(bool), BoolFriendlyName},
        {typeof(byte), ByteFriendlyName},
        {typeof(char), CharFriendlyName},
        {typeof(decimal), DecimalFriendlyName},
        {typeof(double), DoubleFriendlyName},
        {typeof(float), FloatFriendlyName},
        {typeof(int), IntFriendlyName},
        {typeof(long), LongFriendlyName},
        {typeof(object), ObjectFriendlyName},
        {typeof(sbyte), SByteFriendlyName},
        {typeof(short), ShortFriendlyName},
        {typeof(string), StringFriendlyName},
        {typeof(uint), UIntFriendlyName},
        {typeof(ulong), ULongFriendlyName},
        {typeof(ushort), UShortFriendlyName},
        {typeof(void), VoidFriendlyName}
    };

    /// <summary>
    /// An extension method for the <see cref="Type" /> class that returns the constructor info for
    /// the given <see cref="Type" />. <br /> If there is more than one constructor for the given
    /// class type, then the info for the constructor having the most parameters will be returned.
    /// </summary>
    /// <param name="type">
    /// The class type for which we want to retrieve the constructor info.
    /// </param>
    /// <returns>
    /// The <see cref="ConstructorInfo" /> object for the given class type.
    /// </returns>
    /// <remarks>
    /// All parameters on the constructor must match dependency types that have been previously
    /// registered with the dependency injection container.
    /// </remarks>
    /// <exception cref="DependencyInjectionException" />
    internal static ConstructorInfo GetDIConstructorInfo(this Type type)
    {
        ConstructorInfo[] constructorInfo = type.GetConstructors(ConstructorBindingFlags);

        if (constructorInfo.Length < 1)
        {
            string resolvingName = GetResolvingName(type.GetFriendlyName());
            string msg = string.Format(MsgNoSuitableConstructors, resolvingName);
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
            string commas = rank > 1
                ? new string(Comma, rank - 1)
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
            string genericTypeName = type.Name.Split(GenericSeparator)[0];
            Type[] genericParameterTypes = type.GetGenericArguments();
            string genericParameterTypeNames = string.Join(ListSeparator, genericParameterTypes.Select(GetFriendlyName));
            return $"{genericTypeName}<{genericParameterTypeNames}>";
        }
        // Everything else is a simple class type.
        else
        {
            return type.Name;
        }
    }
}