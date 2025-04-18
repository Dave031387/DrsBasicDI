﻿namespace DrsBasicDI;

/// <summary>
/// The <see cref="Container" /> class implements a basic dependency injection container.
/// </summary>
internal sealed class Container : IContainer
{
    /// <summary>
    /// Create a new instance of the <see cref="Container" /> class.
    /// </summary>
    internal Container() : this(DrsBasicDI.ServiceLocater.Instance)
    {
    }

    /// <summary>
    /// Constructor for the <see cref="Container" /> class. Intended for unit testing only.
    /// </summary>
    /// <param name="serviceLocater">
    /// A service locater object that should provide mock instances of the requested dependencies.
    /// </param>
    internal Container(IServiceLocater serviceLocater)
    {
        ServiceLocater = serviceLocater;
        IResolvingObjectsService resolvingObjectsService = ServiceLocater.Get<IResolvingObjectsService>(NonScoped);
        _ = resolvingObjectsService.Add<IContainer>(this, EmptyKey);
        DependencyResolver = ServiceLocater.Get<IDependencyResolver>(NonScoped);
    }

    /// <summary>
    /// Get the <see cref="IDependencyResolver" /> instance used to resolve dependencies.
    /// </summary>
    private IDependencyResolver DependencyResolver
    {
        get;
    }

    /// <summary>
    /// Get a reference to the <see cref="IServiceLocater" /> object.
    /// </summary>
    private IServiceLocater ServiceLocater
    {
        get;
    }

    /// <summary>
    /// Create a new <see cref="IScope" /> object to be used in managing a dependency scope.
    /// </summary>
    /// <returns>
    /// A new <see cref="IScope" /> object.
    /// </returns>
    public IScope CreateScope() => ServiceLocater.Get<IScope>();

    /// <summary>
    /// Dispose of any resources held by any singleton or globally-scoped dependency objects that
    /// have been created by this dependency injection container.
    /// </summary>
    public void Dispose() => DependencyResolver.Dispose();

    /// <summary>
    /// Get an instance of the resolving class that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving class object to be retrieved.
    /// </param>
    /// <returns>
    /// An instance of the resolving object corresponding to the given dependency type
    /// <typeparamref name="TDependency" /> and resolving <paramref name="key" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public TDependency Resolve<TDependency>(string key = EmptyKey) where TDependency : class
        => DependencyResolver.Resolve<TDependency>(key);
}