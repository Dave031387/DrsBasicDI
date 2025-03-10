namespace DrsBasicDI;

public class ContainerBuilderTests
{
    private readonly Type _containerType = typeof(IContainer);

    [Fact]
    public void AddValidDependenciesToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type dependencyType1 = typeof(IClass1);
        //Type resolvingType1 = typeof(Class1);
        //DependencyLifetime lifetime1 = DependencyLifetime.Transient;
        //Type dependencyType2 = typeof(IGenericClass1<int, string>);
        //Type resolvingType2 = typeof(GenericClass1<int, string>);
        //DependencyLifetime lifetime2 = DependencyLifetime.Scoped;
        //static GenericClass1<int, string> factory() => new();
        //string expected = "GenericClass1<int, string>.DoWork\n  arg1=5\n  arg2=test";

        //// Act
        //IContainer container = builder
        //    .AddDependency(b => b
        //        .WithDependencyType(dependencyType1)
        //        .WithResolvingType(resolvingType1)
        //        .WithLifetime(lifetime1))
        //    .AddDependency(b => b
        //        .WithDependencyType(dependencyType2)
        //        .WithResolvingType(resolvingType2)
        //        .WithLifetime(lifetime2)
        //        .WithFactory(factory))
        //    .Build();

        //// Assert
        //container
        //    .Should()
        //    .NotBeNull();
        //Dictionary<Type, IDependency> dependencies = GetDependencies(container);
        //dependencies
        //    .Should()
        //    .HaveCount(3)
        //    .And
        //    .ContainKeys(dependencyType1, dependencyType2, _containerType);
        //IDependency containerDependency = dependencies[_containerType];
        //containerDependency.DependencyType
        //    .Should()
        //    .Be(_containerType);
        //containerDependency.ResolvingType
        //    .Should()
        //    .Be<Container>();
        //containerDependency.Lifetime
        //    .Should()
        //    .Be(DependencyLifetime.Singleton);
        //containerDependency.Factory
        //    .Should()
        //    .BeNull();
        //Dictionary<Type, object> resolvedDependencies = GetResolvingObjects(container);
        //resolvedDependencies
        //    .Should()
        //    .ContainSingle()
        //    .And
        //    .ContainKey(_containerType);
        //resolvedDependencies[_containerType]
        //    .Should()
        //    .BeSameAs(container);
        //IDependency dependency1 = dependencies[dependencyType1];
        //dependency1.DependencyType
        //    .Should()
        //    .Be(dependencyType1);
        //dependency1.ResolvingType
        //    .Should()
        //    .Be(resolvingType1);
        //dependency1.Lifetime
        //    .Should()
        //    .Be(lifetime1);
        //dependency1.Factory
        //    .Should()
        //    .BeNull();
        //IDependency dependency2 = dependencies[dependencyType2];
        //dependency2.DependencyType
        //    .Should()
        //    .Be(dependencyType2);
        //dependency2.ResolvingType
        //    .Should()
        //    .Be(resolvingType2);
        //dependency2.Lifetime
        //    .Should()
        //    .Be(lifetime2);
        //dependency2.Factory
        //    .Should()
        //    .NotBeNull();
        //string actual = ((IGenericClass1<int, string>)dependency2.Factory!()).DoWork(5, "test");
        //actual
        //    .Should()
        //    .Be(expected);
    }

    [Fact]
    public void AddValidScopedDependencyToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type dependencyType = typeof(IClass2);
        //Type resolvingType = typeof(Class2);

        //// Act
        //IContainer container = builder
        //    .AddScoped(b => b
        //        .WithDependencyType(dependencyType)
        //        .WithResolvingType(resolvingType))
        //    .Build();

        //// Assert
        //AssertValidContainer(container, DependencyLifetime.Scoped);
    }

    [Fact]
    public void AddValidSingletonDependencyToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type dependencyType = typeof(IClass2);
        //Type resolvingType = typeof(Class2);

        //// Act
        //IContainer container = builder
        //    .AddSingleton(b => b
        //        .WithDependencyType(dependencyType)
        //        .WithResolvingType(resolvingType))
        //    .Build();

        //// Assert
        //AssertValidContainer(container, DependencyLifetime.Singleton);
    }

    [Fact]
    public void AddValidTransientDependencyToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type dependencyType = typeof(IClass2);
        //Type resolvingType = typeof(Class2);

        //// Act
        //IContainer container = builder
        //    .AddTransient(b => b
        //        .WithDependencyType(dependencyType)
        //        .WithResolvingType(resolvingType))
        //    .Build();

        //// Assert
        //AssertValidContainer(container, DependencyLifetime.Transient);
    }

    [Fact]
    public void GenericAddValidScopedDependencyToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type resolvingType = typeof(Class2);

        //// Act
        //IContainer container = builder
        //    .AddScoped<IClass2>(b => b
        //        .WithResolvingType(resolvingType))
        //    .Build();

        //// Assert
        //AssertValidContainer(container, DependencyLifetime.Scoped);
    }

    [Fact]
    public void GenericAddValidSingletonDependencyToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type resolvingType = typeof(Class2);

        //// Act
        //IContainer container = builder
        //    .AddSingleton<IClass2>(b => b
        //        .WithResolvingType(resolvingType))
        //    .Build();

        //// Assert
        //AssertValidContainer(container, DependencyLifetime.Singleton);
    }

    [Fact]
    public void GenericAddValidTransientDependencyToContainer_ShouldBuildContainer()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type resolvingType = typeof(Class2);

        //// Act
        //IContainer container = builder
        //    .AddTransient<IClass2>(b => b
        //        .WithResolvingType(resolvingType))
        //    .Build();

        //// Assert
        //AssertValidContainer(container, DependencyLifetime.Transient);
    }

    [Fact]
    public void GetContainerBuilderMoreThanOnce_ReturnsSameInstanceEachTime()
    {
        //// Arrange/Act
        //ContainerBuilder builder1 = ContainerBuilder.Current;
        //ContainerBuilder builder2 = ContainerBuilder.Current;
        //ContainerBuilder builder3 = ContainerBuilder.Current;

        //// Assert
        //builder1
        //    .Should()
        //    .BeSameAs(builder2)
        //    .And
        //    .BeSameAs(builder3);
    }

    [Fact]
    public void TryToAddDependencyAfterContainerBuilt_ShouldThrowException()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //_ = builder
        //    .AddTransient<IClass1>(b => b
        //        .WithResolvingType<Class1>())
        //    .Build();

        //// Act
        //void action() => builder
        //    .AddTransient<IClass2>(b => b
        //        .WithResolvingType<Class2>());

        //// Assert
        //AssertException(action, MsgCantAddToContainerAfterBuild);
    }

    [Fact]
    public void TryToAddDuplicateDependencyTypeToContainer_ShouldThrowException()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //Type dependencyType = typeof(IClass1);
        //Type resolvingType1 = typeof(Class1);
        //string typeName = nameof(IClass1);
        //string msg = string.Format(MsgDuplicateDependency, typeName);

        //// Act
        //void action() => builder
        //    .AddDependency(b => b
        //        .WithDependencyType(dependencyType)
        //        .WithResolvingType(resolvingType1)
        //        .WithLifetime(DependencyLifetime.Singleton))
        //    .AddDependency(b => b
        //        .WithDependencyType(dependencyType)
        //        .WithResolvingType<Class1A>()
        //        .WithLifetime(DependencyLifetime.Transient))
        //    .Build();

        //// Assert
        //AssertException(action, msg);
    }

    [Fact]
    public void TryToBuildContainerAfterContainerAlreadyBuilt_ShouldThrowException()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //_ = builder
        //    .AddTransient<IClass1>(b => b
        //        .WithResolvingType<Class1>())
        //    .Build();

        //// Act
        //void action() => builder
        //    .Build();

        //// Assert
        //AssertException(action, MsgContainerCantBeBuiltMoreThanOnce);
    }

    [Fact]
    public void TryToBuildEmptyContainer_ShouldThrowException()
    {
        //// Arrange
        //ContainerBuilder builder = ContainerBuilder.TestInstance;
        //string msg = MsgContainerIsEmpty;

        //// Act
        //void action() => builder.Build();

        //// Assert
        //AssertException(action, msg);
    }

    //private static void AssertException(Action action, string msg)
    //{
    //    // Assert
    //    action
    //        .Should()
    //        .ThrowExactly<ContainerBuildException>()
    //        .WithMessage(msg);
    //}

    //private static Dictionary<Type, IDependency> GetDependencies(IContainer container)
    //                                            => ((Container)container).Dependencies;

    //private static Dictionary<Type, object> GetResolvingObjects(IContainer container)
    //{
    //    Container theContainer = (Container)container;
    //    ResolvingObjectsService resolvingObjects = (ResolvingObjectsService)theContainer.ResolvingObjectsService;
    //    return resolvingObjects.ResolvingObjects;
    //}

    //private void AssertValidContainer(IContainer container, DependencyLifetime lifetime)
    //{
    //    // Assert
    //    container
    //        .Should()
    //        .NotBeNull();
    //    Dictionary<Type, IDependency> dependencies = GetDependencies(container);
    //    dependencies
    //        .Should()
    //        .HaveCount(2)
    //        .And
    //        .ContainKeys(typeof(IClass2), _containerType);
    //    IDependency dependency = dependencies[typeof(IClass2)];
    //    dependency.DependencyType
    //        .Should()
    //        .Be<IClass2>();
    //    dependency.ResolvingType
    //        .Should()
    //        .Be<Class2>();
    //    dependency.Lifetime
    //        .Should()
    //        .Be(lifetime);
    //    dependency.Factory
    //        .Should()
    //        .BeNull();
    //}
}