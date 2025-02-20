namespace DrsBasicDI;

/// <summary>
/// The <see cref="Container" /> class implements a basic dependency injection container.
/// </summary>
public sealed class Container : IContainer, IContainerInternal
{
    /// <summary>
    /// An instance of <see cref="IDependencyResolver" /> used for resolving dependencies and
    /// returning the corresponding resolving object.
    /// </summary>
    private readonly IDependencyResolver _resolver;

    /// <summary>
    /// Create a new instance of the <see cref="Container" /> class.
    /// </summary>
    /// <param name="dependencies">
    /// A list of <see cref="IDependency" /> objects to be added to the container.
    /// </param>
    /// <param name="resolvingObjectsService">
    /// An empty <see cref="IResolvingObjectsService" /> instance that will be used for saving
    /// resolved scoped and singleton dependency objects.
    /// </param>
    /// <param name="resolver">
    /// An optional <see cref="DependencyResolver" /> object used only in unit tests.
    /// </param>
    /// <remarks>
    /// This constructor is declared <see langword="internal" /> to force the user to use the
    /// <see cref="ContainerBuilder" /> object to construct new <see cref="Container" /> objects.
    /// </remarks>
    /// <exception cref="ArgumentNullException" />
    internal Container(IEnumerable<IDependency> dependencies,
                       IResolvingObjectsService resolvingObjectsService,
                       IDependencyResolver? resolver = null)
    {
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));
        ArgumentNullException.ThrowIfNull(resolvingObjectsService, nameof(resolvingObjectsService));
        ResolvingObjectsService = resolvingObjectsService;
        Initialize();
        LoadDependencies(dependencies);
        _resolver = resolver is null
            ? new DependencyResolver(Dependencies!, ResolvingObjectsService)
            : resolver;
    }

    /// <summary>
    /// Get the dictionary of <see cref="IDependency" /> objects whose keys are the corresponding
    /// dependency type specified by the <see cref="IDependency.DependencyType" /> property.
    /// </summary>
    public Dictionary<Type, IDependency> Dependencies
    {
        get;
    } = [];

    /// <summary>
    /// Get the <see cref="IResolvingObjectsService" /> instance used to manage all the non-scoped
    /// resolved dependencies in this dependency injection container.
    /// </summary>
    public IResolvingObjectsService ResolvingObjectsService
    {
        get;
    }

    /// <summary>
    /// Create a new <see cref="IScope" /> object to be used in managing a dependency scope.
    /// </summary>
    /// <returns>
    /// A new <see cref="IScope" /> object.
    /// </returns>
    public IScope CreateScope() => new Scope(this, new ResolvingObjectsService());

    /// <summary>
    /// Get an instance of the resolving class that is mapped to the given dependency type
    /// <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <returns>
    /// An instance of the resolving object corresponding to the given dependency type
    /// <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public T GetDependency<T>() where T : class => _resolver.Resolve<T>();

    /// <summary>
    /// Initialize the container by adding a <see cref="IDependency" /> object that describes the
    /// <see cref="IContainer" /> dependency type and set the resolving object to this
    /// <see cref="Container" /> instance.
    /// </summary>
    private void Initialize()
    {
        Type containerDependencyType = typeof(IContainer);
        IDependency containerDependency = new Dependency()
        {
            DependencyType = containerDependencyType,
            ResolvingType = typeof(Container),
            Lifetime = DependencyLifetime.Singleton
        };
        Dependencies[containerDependencyType] = containerDependency;
        _ = ResolvingObjectsService.Add<IContainer>(this);
    }

    /// <summary>
    /// Load all of the dependencies into the container.
    /// </summary>
    /// <param name="dependencies">
    /// A list of <see cref="Dependency" /> objects.
    /// </param>
    private void LoadDependencies(IEnumerable<IDependency> dependencies)
    {
        foreach (IDependency dependency in dependencies)
        {
            // It is impossible for the DependencyType to be null because the Dependency object is
            // validated before being added to the container.
            Dependencies[dependency.DependencyType!] = dependency;
        }
    }
}