namespace DrsBasicDI;

/// <summary>
/// The <see cref="ContainerBuilder" /> class is used for constructing new <see cref="Container" />
/// objects.
/// </summary>
public sealed class ContainerBuilder
{
    /// <summary>
    /// A list of <see cref="Dependency" /> objects that have been added to this
    /// <see cref="Container" /> instance.
    /// </summary>
    private readonly List<Dependency> _dependencies = [];

    /// <summary>
    /// A list of dependency types taken from the <see cref="Dependency.DependencyType" /> property
    /// of the <see cref="Dependency" /> objects stored in the <see cref="_dependencies" /> list.
    /// </summary>
    private readonly List<Type> _dependencyTypes = [];

    /// <summary>
    /// Default constructor for the <see cref="ContainerBuilder" /> class.
    /// </summary>
    /// <remarks>
    /// This constructor is declared <see langword="private" />. Use the static <see cref="Empty" />
    /// property to create a new, empty <see cref="ContainerBuilder" /> object.
    /// </remarks>
    private ContainerBuilder()
    {
    }

    /// <summary>
    /// Gets a new instance of an empty <see cref="ContainerBuilder" /> object.
    /// </summary>
    public static ContainerBuilder Empty => new();

    /// <summary>
    /// Add the specified <see cref="Dependency" /> object to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Add the specified scoped <see cref="Dependency" /> object to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Add the specified scoped <see cref="Dependency" /> object having the given dependency type
    /// to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType<T>()
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Add the specified singleton <see cref="Dependency" /> object to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Add the specified singleton <see cref="Dependency" /> object having the given dependency
    /// type to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType<T>()
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Add the specified transient <see cref="Dependency" /> object to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Add the specified transient <see cref="Dependency" /> object having the given dependency
    /// type to the container.
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
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType<T>()
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        Add(dependency);
        return this;
    }

    /// <summary>
    /// Build the <see cref="Container" /> object.
    /// </summary>
    /// <returns>
    /// A new <see cref="Container" /> object containing all the <see cref="Dependency" /> objects
    /// that were added.
    /// </returns>
    /// <exception cref="ContainerBuildException" />
    public Container Build()
    {
        if (_dependencies.Count is 0)
        {
            string msg = MsgContainerIsEmpty;
            throw new ContainerBuildException(msg);
        }

        Container container = new();

        foreach (Dependency dependency in _dependencies)
        {
            container._dependencies[dependency.DependencyType] = dependency;
        }

        return container;
    }

    /// <summary>
    /// Add the given <see cref="Dependency" /> object to the container after verifying that the
    /// dependency type wasn't already added to the container.
    /// </summary>
    /// <param name="dependency">
    /// The <see cref="Dependency" /> object to be added to the container.
    /// </param>
    /// <exception cref="ContainerBuildException" />
    private void Add(Dependency dependency)
    {
        if (_dependencyTypes.Contains(dependency.DependencyType))
        {
            string msg = string.Format(MsgDuplicateDependency, dependency.DependencyType.GetFriendlyName());
            throw new ContainerBuildException(msg);
        }

        _dependencyTypes.Add(dependency.DependencyType);
        _dependencies.Add(dependency);
    }
}