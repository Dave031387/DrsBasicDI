﻿namespace DrsBasicDI;

/// <summary>
/// The <see cref="ContainerBuilder" /> class is used for constructing new <see cref="Container" />
/// objects.
/// </summary>
public sealed class ContainerBuilder
{
    /// <summary>
    /// A lazy initializer for the singleton instance of the <see cref="ContainerBuilder" /> object.
    /// </summary>
    private static readonly Lazy<ContainerBuilder> _lazy = new(() => new ContainerBuilder());

    /// <summary>
    /// A list of <see cref="IDependency" /> objects that have been added to this
    /// <see cref="Container" /> instance.
    /// </summary>
    private readonly List<IDependency> _dependencies = [];

    /// <summary>
    /// A list of dependency types taken from the <see cref="IDependency.DependencyType" /> property
    /// of the <see cref="IDependency" /> objects stored in the <see cref="_dependencies" /> list.
    /// </summary>
    private readonly List<Type> _dependencyTypes = [];

    /// <summary>
    /// A boolean flag that gets set to <see langword="true" /> once the <see cref="IContainer" />
    /// object has been built.
    /// </summary>
    private bool _containerHasBeenBuilt = false;

    /// <summary>
    /// Default constructor for the <see cref="ContainerBuilder" /> class.
    /// </summary>
    /// <remarks>
    /// This constructor is declared <see langword="private" />. Use the static
    /// <see cref="Current" /> property to create a new, empty <see cref="ContainerBuilder" />
    /// object.
    /// </remarks>
    private ContainerBuilder()
    {
    }

    /// <summary>
    /// Get the current <see cref="ContainerBuilder" /> instance.
    /// </summary>
    /// <remarks>
    /// This returns a thread safe singleton instance of the <see cref="ContainerBuilder" /> class.
    /// </remarks>
    public static ContainerBuilder Current => _lazy.Value;

    /// <summary>
    /// Get a new instance of the <see cref="ContainerBuilder" /> class.
    /// </summary>
    /// <remarks>
    /// This static property returns a new instance of the <see cref="ContainerBuilder" /> class
    /// every time it is called.
    /// </remarks>
    internal static ContainerBuilder TestInstance => new();

    /// <summary>
    /// Construct a new <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddDependency(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Construct a new scoped <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddScoped(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Construct a new scoped <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="T" /> and add it to the container.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddScoped<T>(Func<DependencyBuilder, DependencyBuilder> builder) where T : class
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType<T>()
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Construct a new singleton <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddSingleton(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Construct a new singleton <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="T" /> and add it to the container.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddSingleton<T>(Func<DependencyBuilder, DependencyBuilder> builder) where T : class
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType<T>()
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Construct a new transient <see cref="IDependency" /> object and add it to the container.
    /// </summary>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddTransient(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Construct a new transient <see cref="IDependency" /> object having the specified dependency
    /// type <typeparamref name="T" /> and add it to the container.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the dependency.
    /// </typeparam>
    /// <param name="builder">
    /// A builder function that takes a <see cref="DependencyBuilder" /> object as input and returns
    /// the updated <see cref="DependencyBuilder" /> object.
    /// </param>
    /// <returns>
    /// The updated <see cref="ContainerBuilder" /> object.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public ContainerBuilder AddTransient<T>(Func<DependencyBuilder, DependencyBuilder> builder) where T : class
    {
        CheckForContainerAlreadyBuilt();
        IDependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType<T>()
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Build the <see cref="IContainer" /> object.
    /// </summary>
    /// <returns>
    /// A new <see cref="IContainer" /> object containing all the <see cref="IDependency" /> objects
    /// that were added.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public IContainer Build()
    {
        CheckForContainerAlreadyBuilt(true);

        if (_dependencies.Count is 0)
        {
            string msg = MsgContainerIsEmpty;
            throw new ContainerBuildException(msg);
        }

        IContainer container = new Container(_dependencies, new ResolvingObjectsService());

        _containerHasBeenBuilt = true;
        return container;
    }

    /// <summary>
    /// Add the given <see cref="IDependency" /> object to the container after verifying that the
    /// specified dependency type wasn't already added to the container.
    /// </summary>
    /// <param name="dependency">
    /// The <see cref="IDependency" /> object to be added to the container.
    /// </param>
    /// <exception cref="ContainerBuildException" />
    private void Add(IDependency dependency)
    {
        if (dependency.DependencyType is null)
        {
            // This exception should never occur since the IDependency object is validated before
            // this method is called.
            throw new ContainerBuildException(MsgDependencyHasNullDependencyType);
        }
        else
        {
            if (_dependencyTypes.Contains(dependency.DependencyType))
            {
                string msg = string.Format(MsgDuplicateDependency, dependency.DependencyType.GetFriendlyName());
                throw new ContainerBuildException(msg);
            }
        }

        _dependencyTypes.Add(dependency.DependencyType);
        _dependencies.Add(dependency);
    }

    /// <summary>
    /// Check to see if the <see cref="IContainer" /> object has already been built. Throw an
    /// appropriate exception if it has.
    /// </summary>
    /// <param name="isBuildAction">
    /// A boolean flag indicating whether or not this method is being called from the
    /// <see cref="Build" /> method.
    /// </param>
    /// <exception cref="ContainerBuildException" />
    private void CheckForContainerAlreadyBuilt(bool isBuildAction = false)
    {
        if (_containerHasBeenBuilt)
        {
            if (isBuildAction)
            {
                throw new ContainerBuildException(MsgContainerCantBeBuiltMoreThanOnce);
            }
            else
            {
                throw new ContainerBuildException(MsgCantAddToContainerAfterBuild);
            }
        }
    }
}