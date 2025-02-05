namespace DrsBasicDI;

/// <summary>
/// The <see cref="Container" /> class implements a basic dependency injection container.
/// </summary>
public sealed class Container : IContainer, IContainerInternal
{
    /// <summary>
    /// A dictionary of <see cref="Dependency" /> objects whose keys are the corresponding
    /// dependency type specified by the <see cref="Dependency.DependencyType" /> property.
    /// </summary>
    internal readonly Dictionary<Type, Dependency> _dependencies = [];

    /// <summary>
    /// This <see cref="IResolvingObjects" /> instance is used to manage all the non-scoped resolved
    /// dependencies in this dependency injection container.
    /// </summary>
    internal readonly IResolvingObjects _resolvingObjects;

    /// <summary>
    /// Create a new instance of the <see cref="Container" /> class.
    /// </summary>
    /// <param name="dependencies">
    /// A list of <see cref="Dependency" /> objects to be added to the container.
    /// </param>
    /// <param name="resolvingObjects">
    /// An empty <see cref="IResolvingObjects" /> instance that will be used for saving resolved
    /// scoped and singleton dependency objects.
    /// </param>
    /// <remarks>
    /// This constructor is declared <see langword="internal" /> to force the user to use the
    /// <see cref="ContainerBuilder" /> object to construct new <see cref="Container" /> objects.
    /// </remarks>
    /// <exception cref="ArgumentNullException" />
    internal Container(IEnumerable<Dependency> dependencies, IResolvingObjects resolvingObjects)
    {
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));
        ArgumentNullException.ThrowIfNull(resolvingObjects, nameof(resolvingObjects));
        _resolvingObjects = resolvingObjects;
        Initialize();
        LoadDependencies(dependencies);
    }

    /// <summary>
    /// Get the dictionary of <see cref="Dependency" /> objects.
    /// </summary>
    public Dictionary<Type, Dependency> Dependencies => _dependencies;

    /// <summary>
    /// Get the <see cref="IResolvingObjects" /> instance.
    /// </summary>
    public IResolvingObjects ResolvingObjects => _resolvingObjects;

    /// <summary>
    /// Create a new <see cref="IScope" /> object to be used in managing a dependency scope.
    /// </summary>
    /// <returns>
    /// A new <see cref="IScope" /> object.
    /// </returns>
    public IScope CreateScope() => new Scope(this, new ResolvingObjects());

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
    public T GetDependency<T>() where T : class
    {
        DependencyResolver resolver = new(_dependencies,
                                          _resolvingObjects);
        return resolver.Resolve<T>();
    }

    /// <summary>
    /// Initialize the container by adding a <see cref="Dependency" /> object that describes the
    /// <see cref="IContainer" /> dependency type and set the resolving object to this
    /// <see cref="Container" /> instance.
    /// </summary>
    private void Initialize()
    {
        Type containerDependencyType = typeof(IContainer);
        Dependency containerDependency = new()
        {
            DependencyType = containerDependencyType,
            ResolvingType = typeof(Container),
            Lifetime = DependencyLifetime.Singleton
        };
        _dependencies[containerDependencyType] = containerDependency;
        _ = _resolvingObjects.Add<IContainer>(this);
    }

    /// <summary>
    /// Load all of the dependencies into the container.
    /// </summary>
    /// <param name="dependencies">
    /// A list of <see cref="Dependency" /> objects.
    /// </param>
    private void LoadDependencies(IEnumerable<Dependency> dependencies)
    {
        foreach (Dependency dependency in dependencies)
        {
            _dependencies[dependency.DependencyType] = dependency;
        }
    }
}