﻿namespace DrsBasicDI;

public class ContainerBuilderTests
{
    [Fact]
    public void AddValidDependenciesToContainer_ShouldBuildContainer()
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

    [Fact]
    public void AddValidScopedDependencyToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type dependencyType = typeof(IClass2);
        Type resolvingType = typeof(Class2);

        // Act
        Container container = builder
            .AddScoped(b => b
                .WithDependencyType(dependencyType)
                .WithResolvingType(resolvingType))
            .Build();

        // Assert
        AssertValidContainer(container, DependencyLifetime.Scoped);
    }

    [Fact]
    public void AddValidSingletonDependencyToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type dependencyType = typeof(IClass2);
        Type resolvingType = typeof(Class2);

        // Act
        Container container = builder
            .AddSingleton(b => b
                .WithDependencyType(dependencyType)
                .WithResolvingType(resolvingType))
            .Build();

        // Assert
        AssertValidContainer(container, DependencyLifetime.Singleton);
    }

    [Fact]
    public void AddValidTransientDependencyToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type dependencyType = typeof(IClass2);
        Type resolvingType = typeof(Class2);

        // Act
        Container container = builder
            .AddTransient(b => b
                .WithDependencyType(dependencyType)
                .WithResolvingType(resolvingType))
            .Build();

        // Assert
        AssertValidContainer(container, DependencyLifetime.Transient);
    }

    [Fact]
    public void GenericAddValidScopedDependencyToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type resolvingType = typeof(Class2);

        // Act
        Container container = builder
            .AddScoped<IClass2>(b => b
                .WithResolvingType(resolvingType))
            .Build();

        // Assert
        AssertValidContainer(container, DependencyLifetime.Scoped);
    }

    [Fact]
    public void GenericAddValidSingletonDependencyToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type resolvingType = typeof(Class2);

        // Act
        Container container = builder
            .AddSingleton<IClass2>(b => b
                .WithResolvingType(resolvingType))
            .Build();

        // Assert
        AssertValidContainer(container, DependencyLifetime.Singleton);
    }

    [Fact]
    public void GenericAddValidTransientDependencyToContainer_ShouldBuildContainer()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type resolvingType = typeof(Class2);

        // Act
        Container container = builder
            .AddTransient<IClass2>(b => b
                .WithResolvingType(resolvingType))
            .Build();

        // Assert
        AssertValidContainer(container, DependencyLifetime.Transient);
    }

    [Fact]
    public void TryToAddDuplicateDependencyTypeToContainer_ShouldThrowException()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        Type resolvingType1 = typeof(Class1);
        string typeName = nameof(IClass1);
        void action() => builder
            .AddDependency(b => b
                .WithDependencyType(dependencyType)
                .WithResolvingType(resolvingType1)
                .WithLifetime(DependencyLifetime.Singleton))
            .AddDependency(b => b
                .WithDependencyType(dependencyType)
                .WithResolvingType<Class1A>()
                .WithLifetime(DependencyLifetime.Transient))
            .Build();
        string msg = string.Format(MsgDuplicateDependency, typeName);

        // Act/Assert
        AssertException(action, msg);
    }

    [Fact]
    public void TryToBuildEmptyContainer_ShouldThrowException()
    {
        // Arrange
        ContainerBuilder builder = ContainerBuilder.Empty;
        void action() => builder.Build();
        string msg = MsgContainerIsEmpty;

        // Act/Assert
        AssertException(action, msg);
    }

    private static void AssertException(Action action, string msg)
    {
        // Act/Assert
        action
            .Should()
            .ThrowExactly<ContainerBuildException>()
            .WithMessage(msg);
    }

    private static void AssertValidContainer(Container container, DependencyLifetime lifetime)
    {
        // Assert
        container
            .Should()
            .NotBeNull();
        container._dependencies
            .Should()
            .ContainSingle();
        container._dependencies
            .Should()
            .ContainKey(typeof(IClass2));
        Dependency dependency = container._dependencies[typeof(IClass2)];
        dependency.DependencyType
            .Should()
            .Be<IClass2>();
        dependency.ResolvingType
            .Should()
            .Be<Class2>();
        dependency.Lifetime
            .Should()
            .Be(lifetime);
        dependency.Factory
            .Should()
            .BeNull();
    }
}