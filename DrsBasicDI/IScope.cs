namespace DrsBasicDI;

/// <summary>
/// The <see cref="IScope" /> interface defines the methods and properties for managing scoped
/// dependencies.
/// </summary>
public interface IScope : IDisposable
{
    /// <summary>
    /// Gets an instance of the resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" /> and resolving <paramref name="key" />.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The dependency type that is to be retrieved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// The resolving object for the given dependency type <typeparamref name="TDependency" /> and
    /// resolving <paramref name="key" />, or <see langword="null" /> if the resolving object can't
    /// be determined.
    /// </returns>
    public TDependency Resolve<TDependency>(string key = EmptyKey) where TDependency : class;
}