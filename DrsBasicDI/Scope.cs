namespace DrsBasicDI;

/// <summary>
/// The <see cref="Scope" /> class manages the creation and disposing of scoped dependencies.
/// </summary>
internal sealed class Scope : IScope
{
    /// <summary>
    /// Hold a reference to the dependency injection container.
    /// </summary>
    internal readonly IContainerInternal _container;

    /// <summary>
    /// An instance of <see cref="IDependencyResolver" /> used for resolving dependencies and
    /// returning the corresponding resolving object.
    /// </summary>
    internal readonly IDependencyResolver _resolver;

    /// <summary>
    /// This <see cref="IResolvingObjectsService" /> instance is used to manage all the scoped
    /// resolved dependencies in this dependency injection container.
    /// </summary>
    internal readonly IResolvingObjectsService _resolvingObjectsService;

    /// <summary>
    /// Flag to detect redundant calls to the <see cref="Dispose(bool)" /> method.
    /// </summary>
    internal bool _isDisposed;

    /// <summary>
    /// Create a new <see cref="Scope" /> object.
    /// </summary>
    /// <param name="container">
    /// A reference to the <see cref="Container" /> object that is creating this
    /// <see cref="Scope" /> object.
    /// </param>
    /// <param name="resolvingObjectsService">
    /// An empty <see cref="IResolvingObjectsService" /> instance that will be used for saving
    /// resolved scoped dependency objects.
    /// </param>
    /// <param name="resolver">
    /// An optional <see cref="DependencyResolver" /> object used only in unit tests.
    /// </param>
    /// <remarks>
    /// This constructor is marked <see langword="internal" />. Only the <see cref="Container" />
    /// object can be used to create new <see cref="Scope" /> objects.
    /// </remarks>
    /// <exception cref="ArgumentNullException" />
    internal Scope(IContainerInternal container,
                   IResolvingObjectsService resolvingObjectsService,
                   IDependencyResolver? resolver = null)
    {
        ArgumentNullException.ThrowIfNull(container, nameof(container));
        ArgumentNullException.ThrowIfNull(resolvingObjectsService, nameof(resolvingObjectsService));
        _container = container;
        _resolvingObjectsService = resolvingObjectsService;
        _resolver = resolver is null
            ? new DependencyResolver(container.Dependencies, container.ResolvingObjectsService, resolvingObjectsService)
            : resolver;
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
            _resolvingObjectsService.Clear();
        }

        // Unmanaged resources would be freed here if there were any.

        _isDisposed = true;
    }

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
    public T GetDependency<T>() where T : class => _resolver.Resolve<T>();
}