namespace DrsBasicDI;

public class ContainerBuilder
{
    private readonly List<Dependency> _dependencies = [];
    private readonly List<Type> _dependencyTypes = [];

    private ContainerBuilder()
    {
    }

    public static ContainerBuilder Empty => new();

    public ContainerBuilder AddDependency(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .Build();
        Add(dependency);
        return this;
    }

    public ContainerBuilder AddScoped(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        Add(dependency);
        return this;
    }

    public ContainerBuilder AddScoped<T>(Func<DependencyBuilder, DependencyBuilder> builder) where T : class
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType(typeof(T))
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        Add(dependency);
        return this;
    }

    public ContainerBuilder AddSingleton(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        Add(dependency);
        return this;
    }

    public ContainerBuilder AddSingleton<T>(Func<DependencyBuilder, DependencyBuilder> builder) where T : class
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType(typeof(T))
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        Add(dependency);
        return this;
    }

    public ContainerBuilder AddTransient(Func<DependencyBuilder, DependencyBuilder> builder)
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        Add(dependency);
        return this;
    }

    public ContainerBuilder AddTransient<T>(Func<DependencyBuilder, DependencyBuilder> builder) where T : class
    {
        Dependency dependency = builder(DependencyBuilder.Empty)
            .WithDependencyType(typeof(T))
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        Add(dependency);
        return this;
    }

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