namespace DrsBasicDI;

/// <summary>
/// The <see cref="ResolvingObjectsService" /> class manages a dictionary of resolving objects for
/// the singleton and scoped dependencies contained in the dependency injection container.
/// </summary>
internal sealed class ResolvingObjectsService : IResolvingObjectsService
{
    /// <summary>
    /// A lock object used to ensure thread safety when accessing/modifying the
    /// <see cref="ResolvingObjects" /> field.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Create an instance of the <see cref="ResolvingObjectsService" /> class.
    /// </summary>
    public ResolvingObjectsService() : this(ServiceLocater.Instance)
    {
    }

    /// <summary>
    /// Constructor for the <see cref="ResolvingObjectsService" /> class. Intended for unit testing
    /// only.
    /// </summary>
    /// <param name="serviceLocater">
    /// A service locater object that should provide mock instances of the requested dependencies.
    /// </param>
    internal ResolvingObjectsService(IServiceLocater serviceLocater)
        => DependencyList = serviceLocater.Get<IDependencyListConsumer>();

    /// <summary>
    /// Get a reference to the <see cref="IDependencyListBuilder" /> object.
    /// </summary>
    private IDependencyListConsumer DependencyList
    {
        get;
    }

    /// <summary>
    /// Get the dictionary of resolving objects.
    /// </summary>
    public Dictionary<ServiceKey, object> ResolvingObjects { get; } = [];

    /// <summary>
    /// Add the given <paramref name="resolvingObject" /> to the list of resolving objects if no
    /// object currently exists for the given dependency type <typeparamref name="T" /> having the
    /// specified <paramref name="key" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type being resolved.
    /// </typeparam>
    /// <param name="resolvingObject">
    /// The resolving object to be added for the given dependency type <typeparamref name="T" />.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific <paramref name="resolvingObject" /> to be
    /// added.
    /// </param>
    /// <returns>
    /// The <paramref name="resolvingObject" /> or the object retrieved from the list of resolving
    /// objects if one already exists for the given dependency type <typeparamref name="T" /> and
    /// <paramref name="key" />.
    /// </returns>
    public T Add<T>(T resolvingObject, string key = EmptyKey) where T : class
    {
        IDependency dependency = DependencyList.Get<T>(key);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);

        if (TryGetResolvingObject(out T? value))
        {
            // If we get here then a resolving object for the given dependency type has already been
            // added to the container and the resolving object that was added isn't null.
            return value!;
        }

        lock (_lock)
        {
            // If we get here then a resolving object hasn't yet been added to the container for the
            // given dependency type.
            ResolvingObjects[serviceKey] = resolvingObject;
            return resolvingObject;
        }
    }

    /// <summary>
    /// Remove all objects from the list of resolved dependencies. Call Dispose on each object that
    /// implements the <see cref="IDisposable" /> interface.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            foreach (ServiceKey serviceKey in ResolvingObjects.Keys)
            {
                if (ResolvingObjects[serviceKey] is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _ = ResolvingObjects.Remove(serviceKey);
            }
        }
    }

    /// <summary>
    /// Check to see if the specified dependency type has been resolved and, if it has, return the
    /// resolving object.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type whose resolving object is to be retrieved.
    /// </typeparam>
    /// <param name="resolvingObject">
    /// The resolved dependency object, or <see langword="null" /> if the dependency type hasn't yet
    /// been resolved.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific <paramref name="resolvingObject" /> to be
    /// retrieved.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if the given dependency type has been resolved, otherwise
    /// <see langword="false" />.
    /// </returns>
    public bool TryGetResolvingObject<T>(out T? resolvingObject, string key = EmptyKey) where T : class
    {
        IDependency dependency = DependencyList.Get<T>(key);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);

        lock (_lock)
        {
            if (ResolvingObjects.TryGetValue(serviceKey, out object? value))
            {
                if (value is not null)
                {
                    resolvingObject = (T)value;
                    return true;
                }
            }
        }

        resolvingObject = default;
        return false;
    }
}