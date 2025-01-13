namespace DrsBasicDI;

public class ContainerBuilderTests
{
    [Fact]
    public void TryToAddDuplicateDependencyToContainer_ShouldThrowException()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type type = typeof(IClass1);
        string typeName = nameof(IClass1);
        Action action = () => builder
            .AddDependency(b => b
                .WithDependencyType(type)
                .WithResolvingType(typeof(Class1))
                .WithLifetime(DependencyLifetime.Singleton))
            .AddDependency(b => b
                .WithDependencyType(type)
                .WithResolvingType<Class1A>()
                .WithLifetime(DependencyLifetime.Transient))
            .Build();
        string msg = string.Format(MsgDuplicateDependency, typeName);

        // Act/Assert
        action
            .Should()
            .ThrowExactly<ContainerBuildException>()
            .WithMessage(msg);
    }

    [Fact]
    public void TryToBuildEmptyContainer_ShouldThrowException()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Action action = () => builder.Build();
        string msg = MsgContainerIsEmpty;

        // Act/Assert
        action
            .Should()
            .ThrowExactly<ContainerBuildException>()
            .WithMessage(msg);
    }

    [Fact]
    public void ValidDependenciesAddedToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type dependencyType1 = typeof(IClass1);
        Type resolvingType1 = typeof(Class1);
        DependencyLifetime lifetime1 = DependencyLifetime.Transient;
        Type dependencyType2 = typeof(IGenericClass1<int, string>);
        Type resolvingType2 = typeof(GenericClass1<int, string>);
        DependencyLifetime lifetime2 = DependencyLifetime.Scoped;
        static GenericClass1<int, string> factory() => new();
        string expected = "GenericClass1<int, string>.DoWork\n  arg1=5\n  arg2=test";

        // Act
        Container container = builder
            .AddDependency(b => b
                .WithDependencyType(dependencyType1)
                .WithResolvingType(resolvingType1)
                .WithLifetime(lifetime1))
            .AddDependency(b => b
                .WithDependencyType(dependencyType2)
                .WithResolvingType(resolvingType2)
                .WithLifetime(lifetime2)
                .WithFactory(factory))
            .Build();

        // Assert
        container
            .Should()
            .NotBeNull();
        container._dependencies
            .Should()
            .HaveCount(2);
        container._dependencies
            .Should()
            .ContainKeys(dependencyType1, dependencyType2);
        Dependency dependency1 = container._dependencies[dependencyType1];
        dependency1.DependencyType
            .Should()
            .Be(dependencyType1);
        dependency1.ResolvingType
            .Should()
            .Be(resolvingType1);
        dependency1.Lifetime
            .Should()
            .Be(lifetime1);
        dependency1.Factory
            .Should()
            .BeNull();
        Dependency dependency2 = container._dependencies[dependencyType2];
        dependency2.DependencyType
            .Should()
            .Be(dependencyType2);
        dependency2.ResolvingType
            .Should()
            .Be(resolvingType2);
        dependency2.Lifetime
            .Should()
            .Be(lifetime2);
        dependency2.Factory
            .Should()
            .NotBeNull();
        string actual = ((IGenericClass1<int, string>)dependency2.Factory!()).DoWork(5, "test");
        actual
            .Should()
            .Be(expected);
    }
}