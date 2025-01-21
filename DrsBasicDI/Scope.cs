namespace DrsBasicDI;

/// <summary>
/// The <see cref="Scope" /> class manages the creation and disposing of scoped dependencies.
/// </summary>
internal class Scope : IScope
{
    /// <summary>
    /// Hold a reference to the dependency injection container.
    /// </summary>
    internal readonly Container _container;

    /// <summary>
    /// This <see cref="ResolvedDependencies" /> instance is used to manage all the non-scoped
    /// resolved dependencies in this dependency injection container.
    /// </summary>
    internal readonly ResolvedDependencies _resolvedDependencies = new();

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
    /// <remarks>
    /// This constructor is marked <see langword="internal" />. Only the <see cref="Container" />
    /// object can be used to create new <see cref="Scope" /> objects.
    /// </remarks>
    internal Scope(Container container) => _container = container;

    /// <summary>
    /// Dispose of the managed resources that are owned by this scope.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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
        DependencyResolver resolver = new(_container._dependencies,
                                          _container._resolvedDependencies,
                                          _resolvedDependencies);
        return resolver.Resolve<T>();
    }

    /// <summary>
    /// Dispose of the managed resources that are owned by this scope and then set a flag to prevent
    /// redundant calls to this method.
    /// </summary>
    /// <param name="disposing">
    /// A boolean flag indicating whether or not managed resources should be disposed of.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _resolvedDependencies.Clear();
        }

        // Unmanaged resources would be freed here if there were any.

        _isDisposed = true;
    }
}