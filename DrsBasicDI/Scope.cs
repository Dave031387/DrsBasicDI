namespace DrsBasicDI;

/// <summary>
/// The <see cref="Scope" /> class manages the creation and disposing of scoped dependencies.
/// </summary>
internal sealed class Scope : IScope
{
    /// <summary>
    /// Create a new instance of the <see cref="Scope" /> class.
    /// </summary>
    internal Scope() : this(ServiceLocater.Instance)
    {
    }

    /// <summary>
    /// Constructor for the <see cref="Scope" /> class. Intended for unit testing only.
    /// </summary>
    /// <param name="serviceLocater">
    /// A service locater object that should provide mock instances of the requested dependencies.
    /// </param>
    internal Scope(IServiceLocater serviceLocater)
    {
        DependencyResolver = serviceLocater.Get<IDependencyResolver>(Scoped);
        IResolvingObjectsService resolvingObjectsService = serviceLocater.Get<IResolvingObjectsService>(Scoped);
        DependencyResolver.SetScopedService(resolvingObjectsService);
    }

    /// <summary>
    /// Get a reference to the <see cref="IDependencyResolver" /> object.
    /// </summary>
    private IDependencyResolver DependencyResolver
    {
        get;
    }

    /// <summary>
    /// Dispose of any resources owned by any scoped dependency objects that have been built in this
    /// scope.
    /// </summary>
    public void Dispose() => DependencyResolver.Dispose();

    /// <summary>
    /// Gets an instance of the resolving type that is mapped to the given dependency type
    /// <typeparamref name="T" /> and resolving <paramref name="key" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be retrieved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// The resolving object for the given dependency type <typeparamref name="T" /> and resolving
    /// <paramref name="key" />, or <see langword="null" /> if the resolving object can't be
    /// determined.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public T GetDependency<T>(string key = EmptyKey) where T : class
        => DependencyResolver.Resolve<T>(key);
}