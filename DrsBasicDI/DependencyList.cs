namespace DrsBasicDI;

internal sealed class DependencyList : IDependencyListBuilder, IDependencyListConsumer
{
    /// <summary>
    /// A list of <see cref="IDependency" /> objects that have been added to the dependency
    /// injection container.
    /// </summary>
    private readonly Dictionary<ServiceKey, IDependency> _dependencies = [];

    /// <summary>
    /// A lock object used to ensure thread safety when accessing or saving
    /// <see cref="IDependency" /> objects.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Create a new instance of the <see cref="DependencyList" /> class.
    /// </summary>
    internal DependencyList()
    {
    }

    /// <summary>
    /// Get the number of dependencies in the container.
    /// </summary>
    public int Count => _dependencies.Count;

    /// <summary>
    /// Add the given <paramref name="dependency" /> to the list of dependencies.
    /// </summary>
    /// <param name="dependency">
    /// The <see cref="IDependency" /> object to be added to the list of dependencies.
    /// </param>
    /// <exception cref="ContainerBuildException" />
    public void Add(IDependency dependency)
    {
        ServiceKey serviceKey = ServiceKey.GetServiceDependencyKey(dependency);

        lock (_lock)
        {
            if (_dependencies.ContainsKey(serviceKey))
            {
                string dependencyName = GetDependencyName(dependency.DependencyType.GetFriendlyName(), dependency.Key);
                string msg = string.Format(MsgDuplicateDependency, dependencyName);
                throw new ContainerBuildException(msg);
            }

            _dependencies[serviceKey] = dependency;
        }
    }

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
    /// <exception cref="DependencyInjectionException" />
    public IDependency Get<T>(string key) where T : class
        => Get(typeof(T), key);

    /// <summary>
    /// Get the <see cref="IDependency" /> object for the given <paramref name="dependencyType" />
    /// and <paramref name="key" /> value.
    /// </summary>
    /// <param name="dependencyType">
    /// The type of dependency to be retrieved.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific <see cref="IDependency" /> object to be
    /// retrieved.
    /// </param>
    /// <returns>
    /// The <see cref="IDependency" /> instance corresponding to the given
    /// <paramref name="dependencyType" /> and <paramref name="key" /> value.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public IDependency Get(Type dependencyType, string key)
    {
        ServiceKey serviceKey = ServiceKey.GetServiceKey(dependencyType, key);
        string dependencyName = GetDependencyName(dependencyType.GetFriendlyName(), key);

        lock (_lock)
        {
            if (_dependencies.TryGetValue(serviceKey, out IDependency? dependency))
            {
                if (dependency is null)
                {
                    string msg = string.Format(MsgNullDependencyObject, dependencyName);
                    throw new DependencyInjectionException(msg);
                }
                return dependency;
            }
            else
            {
                string msg = string.Format(MsgDependencyMappingNotFound, dependencyName);
                throw new DependencyInjectionException(msg);
            }
        }
    }
}