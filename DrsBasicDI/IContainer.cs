namespace DrsBasicDI;

/// <summary>
/// The <see cref="IContainer" /> interface defines the methods and properties for a simple
/// dependency injection container.
/// </summary>
public interface IContainer
{
    /// <summary>
    /// Create a new <see cref="IScope" /> object to be used in managing a dependency scope.
    /// </summary>
    /// <returns>
    /// A new <see cref="IScope" /> object.
    /// </returns>
    public IScope CreateScope();

    /// <summary>
    /// Get an instance of the resolving class that is mapped to the given dependency type
    /// <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving class object to be retrieved.
    /// </param>
    /// <returns>
    /// An instance of the resolving object corresponding to the given dependency type
    /// <typeparamref name="T" /> and resolving <paramref name="key" />.
    /// </returns>
    public T GetDependency<T>(string key = EmptyKey) where T : class;
}