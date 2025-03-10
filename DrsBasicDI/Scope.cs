namespace DrsBasicDI;

/// <summary>
/// The <see cref="Scope" /> class manages the creation and disposing of scoped dependencies.
/// </summary>
internal sealed class Scope : IScope
{
    /// <summary>
    /// Flag to detect redundant calls to the <see cref="Dispose(bool)" /> method.
    /// </summary>
    internal bool _isDisposed;

    /// <summary>
    /// Create a new instance of the <see cref="Scope" /> class.
    /// </summary>
    public Scope() : this(ServiceLocater.Instance)
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
        ResolvingObjectsService = serviceLocater.Get<IResolvingObjectsService>(Scoped);
        DependencyResolver = serviceLocater.Get<IDependencyResolver>(Scoped);
        DependencyResolver.SetScopedResolver(ResolvingObjectsService);
    }

    /// <summary>
    /// Get a reference to the <see cref="IDependencyResolver" /> object.
    /// </summary>
    private IDependencyResolver DependencyResolver
    {
        get;
    }

    /// <summary>
    /// Get a reference to the <see cref="IResolvingObjectsService" /> object.
    /// </summary>
    private IResolvingObjectsService ResolvingObjectsService
    {
        get;
    }

    /// <summary>
    /// Dispose of the managed resources that are owned by this scope.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose of the managed resources that are owned by this scope and then set a flag to prevent
    /// redundant calls to this method.
    /// </summary>
    /// <param name="disposing">
    /// A boolean flag indicating whether or not managed resources should be disposed of.
    /// </param>
    public void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            ResolvingObjectsService.Clear();
        }

        // Unmanaged resources would be freed here if there were any.

        _isDisposed = true;
    }

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