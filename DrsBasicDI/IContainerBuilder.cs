﻿namespace DrsBasicDI;

/// <summary>
/// The <see cref="IContainerBuilder" /> interface defines the methods that are required to build a
/// dependency injection container.
/// </summary>
public interface IContainerBuilder
{
    /// <summary>
    /// Construct a new <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddDependency(Func<DependencyBuilder, DependencyBuilder> builder);

    /// <summary>
    /// Construct a new <see cref="IDependency" /> object having the specified dependency type
    /// <typeparamref name="TDependency" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddDependency<TDependency>(Func<DependencyBuilder, DependencyBuilder> builder) where TDependency : class;

    /// <summary>
    /// Construct a new <see cref="IDependency" /> object having the specified dependency type
    /// <typeparamref name="TDependency" /> and resolving type <typeparamref name="TResolving" />
    /// and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the dependency type
    /// <typeparamref name="TDependency" />.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddDependency<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Construct a new scoped <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddScoped(Func<DependencyBuilder, DependencyBuilder> builder);

    /// <summary>
    /// Construct a new scoped <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddScoped<TDependency>(Func<DependencyBuilder, DependencyBuilder> builder) where TDependency : class;

    /// <summary>
    /// Construct a new scoped <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and resolving type
    /// <typeparamref name="TResolving" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />
    /// </typeparam>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddScoped<TDependency, TResolving>()
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Construct a new scoped <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and resolving type
    /// <typeparamref name="TResolving" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddScoped<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Construct a new singleton <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddSingleton(Func<DependencyBuilder, DependencyBuilder> builder);

    /// <summary>
    /// Construct a new singleton <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddSingleton<TDependency>(Func<DependencyBuilder, DependencyBuilder> builder) where TDependency : class;

    /// <summary>
    /// Construct a new singleton <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and resolving type
    /// <typeparamref name="TResolving" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />.
    /// </typeparam>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddSingleton<TDependency, TResolving>()
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Construct a new singleton <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and resolving type
    /// <typeparamref name="TResolving" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddSingleton<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Construct a new transient <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddTransient(Func<DependencyBuilder, DependencyBuilder> builder);

    /// <summary>
    /// Construct a new transient <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddTransient<TDependency>(Func<DependencyBuilder, DependencyBuilder> builder) where TDependency : class;

    /// <summary>
    /// Construct a new transient <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and resolving type
    /// <typeparamref name="TResolving" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />.
    /// </typeparam>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddTransient<TDependency, TResolving>()
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Construct a new transient <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="TDependency" /> and resolving type
    /// <typeparamref name="TResolving" /> and add it to the container.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>
    /// <typeparam name="TResolving">
    /// The resolving type that is mapped to the given dependency type
    /// <typeparamref name="TDependency" />.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="IContainerBuilder" /> object.
    /// </returns>
    public IContainerBuilder AddTransient<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)
        where TDependency : class
        where TResolving : class, TDependency;

    /// <summary>
    /// Build the <see cref="IContainer" /> object.
    /// </summary>
    /// <returns>
    /// A new <see cref="IContainer" /> object containing all the <see cref="IDependency" /> objects
    /// that were added.
    /// </returns>
    public IContainer Build();
}