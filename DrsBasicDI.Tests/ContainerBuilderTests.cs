namespace DrsBasicDI;

public class ContainerBuilderTests
{
    private readonly IDependency _containerDependency = new Dependency(typeof(IContainer),
                                                                       typeof(Container),
                                                                       DependencyLifetime.Singleton,
                                                                       null,
                                                                       EmptyKey);

    [Fact]
    public void AddValidDependenciesToContainer_ShouldBuildContainer()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        Mock<IContainer> mockContainer = mockServiceLocater.GetMock<IContainer>();
        static GenericClass1<int, string> factory() => new();
        string key = "test";
        IDependency dependency1 = new Dependency(typeof(IClass1),
                                                 typeof(Class1),
                                                 DependencyLifetime.Transient,
                                                 null,
                                                 EmptyKey);
        IDependency dependency2 = new Dependency(typeof(IGenericClass1<int, string>),
                                                 typeof(GenericClass1<int, string>),
                                                 DependencyLifetime.Scoped,
                                                 factory,
                                                 key);
        mockDependencyList
            .Setup(m => m.Add(dependency1))
            .Verifiable(Times.Once);
        mockDependencyList
            .Setup(m => m.Add(dependency2))
            .Verifiable(Times.Once);
        mockDependencyList
            .Setup(m => m.Add(_containerDependency))
            .Verifiable(Times.Once);
        mockDependencyList
            .SetupGet(m => m.Count)
            .Returns(2)
            .Verifiable(Times.Once);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);

        // Act
        IContainer container = builder
            .AddDependency(b => b
                .WithDependencyType<IClass1>()
                .WithResolvingType<Class1>()
                .WithLifetime(DependencyLifetime.Transient))
            .AddDependency(b => b
                .WithDependencyType<IGenericClass1<int, string>>()
                .WithResolvingType<GenericClass1<int, string>>()
                .WithLifetime(DependencyLifetime.Scoped)
                .WithFactory(factory)
                .WithResolvingKey(key))
            .Build();

        // Assert
        container
            .Should()
            .NotBeNull();
        container
            .Should()
            .BeSameAs(mockContainer.Object);
        mockServiceLocater.VerifyMocks();
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