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
    /// This <see cref="IResolvingObjects" /> instance is used to manage all the scoped resolved
    /// dependencies in this dependency injection container.
    /// </summary>
    internal readonly IResolvingObjects _resolvingObjects;

    /// <summary>
    /// Flag to detect redundant calls to the <see cref="Dispose(bool)" /> method.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Create a new <see cref="Scope" /> object.
    /// </summary>
    /// <param name="container">
    /// A reference to the <see cref="Container" /> object that is creating this
    /// <see cref="Scope" /> object.
    /// </param>
    /// <param name="resolvingObjects">
    /// An empty <see cref="IResolvingObjects" /> instance that will be used for saving resolved
    /// scoped dependency objects.
    /// </param>
    /// <remarks>
    /// This constructor is marked <see langword="internal" />. Only the <see cref="Container" />
    /// object can be used to create new <see cref="Scope" /> objects.
    /// </remarks>
    internal Scope(IContainerInternal container, IResolvingObjects resolvingObjects)
    {
        _container = container;
        _resolvingObjects = resolvingObjects;
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
            _resolvingObjects.Clear();
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
    public T GetDependency<T>() where T : class
    {
        DependencyResolver resolver = new(_container.Dependencies,
                                          _container.ResolvingObjects,
                                          _resolvingObjects);
        return resolver.Resolve<T>();
    }
}