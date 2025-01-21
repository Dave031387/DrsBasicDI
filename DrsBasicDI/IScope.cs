namespace DrsBasicDI;

/// <summary>
/// The <see cref="IScope" /> interface defines the methods and properties for managing scoped
/// dependencies.
/// </summary>
public interface IScope : IDisposable
{
    /// <summary>
    /// Gets an instance of the resolving type that is mapped to the given dependency type
    /// <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be retrieved.
    /// </typeparam>
    /// <returns>
    /// The resolving object for the given dependency type <typeparamref name="T" />, or
    /// <see langword="null" /> if the resolving object can't be determined.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    T? GetDependency<T>() where T : class;
}