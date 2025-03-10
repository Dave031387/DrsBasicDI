namespace DrsBasicDI;

/// <summary>
/// The <see cref="IDependencyListConsumer" /> interface defines the properties and methods needed
/// for retrieving information from the dependency list.
/// </summary>
internal interface IDependencyListConsumer
{
    /// <summary>
    /// Get the <see cref="IDependency" /> object for the given dependency type
    /// <typeparamref name="T" /> and <paramref name="key" /> value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dependency to be retrieved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific <see cref="IDependency" /> object to be
    /// retrieved.
    /// </param>
    /// <returns>
    /// The <see cref="IDependency" /> instance corresponding to the given dependency type
    /// <typeparamref name="T" /> and <paramref name="key" /> value.
    /// </returns>
    public IDependency Get<T>(string key) where T : class;
}