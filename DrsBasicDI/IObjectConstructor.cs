namespace DrsBasicDI;

using System.Reflection;

/// <summary>
/// The <see cref="IObjectConstructor" /> defines a method for constructing resolving objects for a
/// given dependency type.
/// </summary>
internal interface IObjectConstructor
{
    /// <summary>
    /// Constructs an object of type <typeparamref name="T" /> using the specified
    /// <paramref name="constructorInfo" /> and <paramref name="parameterValues" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type to be constructed.
    /// </typeparam>
    /// <param name="constructorInfo">
    /// The constructor information for the resolving type that was mapped to the dependency type
    /// <typeparamref name="T" />.
    /// </param>
    /// <param name="parameterValues">
    /// The constructor parameter values to be used for constructing the resolving type.
    /// </param>
    /// <returns>
    /// The resolving object cast to the dependency type <typeparamref name="T" />.
    /// </returns>
    public T Construct<T>(ConstructorInfo constructorInfo, object[] parameterValues) where T : class;
}