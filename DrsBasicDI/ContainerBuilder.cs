namespace DrsBasicDI;

public class ContainerBuilder
{
    private readonly List<Dependency> _dependencies = [];
    private readonly List<Type> _dependencyTypes = [];

    private ContainerBuilder()
    {
    }

    public static ContainerBuilder Empty => new();

    public ContainerBuilder AddDependency(Func<DependencyBuilder, DependencyBuilder> func)
    {
        Dependency dependency = func(DependencyBuilder.Empty).Build();

        if (_dependencyTypes.Contains(dependency.DependencyType))
        {
            string msg = string.Format(MsgDuplicateDependency, dependency.DependencyType.GetFriendlyName());
            throw new ContainerBuildException(msg);
        }

        _dependencyTypes.Add(dependency.DependencyType);
        _dependencies.Add(dependency);

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
}