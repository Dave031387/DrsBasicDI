namespace DrsBasicDI;

/// <summary>
/// The <see cref="Container" /> class implements a basic dependency injection container.
/// </summary>
public sealed class Container : IContainer
{
    /// <summary>
    /// A dictionary of <see cref="Dependency" /> objects whose keys are the corresponding
    /// dependency type specified by the <see cref="Dependency.DependencyType" /> property.
    /// </summary>
    internal readonly Dictionary<Type, Dependency> _dependencies = [];

    /// <summary>
    /// This <see cref="ResolvedDependencies" /> instance is used to manage all the non-scoped
    /// resolved dependencies in this dependency injection container.
    /// </summary>
    internal readonly ResolvedDependencies _resolvedDependencies = new();

    /// <summary>
    /// Default constructor for the <see cref="Container" /> class.
    /// </summary>
    /// <remarks>
    /// This constructor is declared <see langword="internal" /> to force the user to use the
    /// <see cref="ContainerBuilder" /> object to construct new <see cref="Container" /> objects.
    /// </remarks>
    internal Container()
    {
    }

    /// <summary>
    /// Create a new <see cref="IScope" /> object to be used in managing a dependency scope.
    /// </summary>
    /// <returns>
    /// A new <see cref="IScope" /> object.
    /// </returns>
    public IScope CreateScope() => new Scope(this);

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
                                          _resolvedDependencies);
        return resolver.Resolve<T>();
    }
}