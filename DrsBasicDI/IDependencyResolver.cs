namespace DrsBasicDI;

/// <summary>
/// The <see cref="IDependencyResolver" /> interface defines the methods implemented by the
/// dependency resolver object.
/// </summary>
internal interface IDependencyResolver
{
    /// <summary>
    /// Retrieve the resolving object for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <returns>
    /// An instance of the resolving class type.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    T Resolve<T>() where T : class;
}