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
        mockServiceLocater.CreateMock<IContainer>();
        string key = "test";
        IDependency dependency1 = new Dependency(typeof(IClass1),
                                                 typeof(Class1),
                                                 DependencyLifetime.Transient,
                                                 null,
                                                 EmptyKey);
        IDependency dependency2 = new Dependency(typeof(IGenericClass1<int, string>),
                                                 typeof(GenericClass1<int, string>),
                                                 DependencyLifetime.Scoped,
                                                 null,
                                                 key);
        IDependency dependency3 = new Dependency(typeof(IClass2),
                                                 typeof(Class2),
                                                 DependencyLifetime.Singleton,
                                                 null,
                                                 EmptyKey);
        SetupMockDependencyList(mockDependencyList, [dependency1, dependency2, dependency3]);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);

        // Act
        IContainer container = builder
            .AddDependency(b => b
                .WithDependencyType<IClass1>()
                .WithResolvingType<Class1>()
                .WithLifetime(DependencyLifetime.Transient))
            .AddDependency<IGenericClass1<int, string>>(b => b
                .WithResolvingType<GenericClass1<int, string>>()
                .WithLifetime(DependencyLifetime.Scoped)
                .WithResolvingKey(key))
            .AddDependency<IClass2, Class2>(b => b
                .WithLifetime(DependencyLifetime.Singleton))
            .Build();

        // Assert
        AssertValidContainer(container, mockServiceLocater);
    }

    [Fact]
    public void AddValidScopedDependenciesToContainer_ShouldBuildContainer()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        mockServiceLocater.CreateMock<IContainer>();
        string key = "test";
        IDependency dependency1 = new Dependency(typeof(IClass1),
                                                 typeof(Class1),
                                                 DependencyLifetime.Scoped,
                                                 null,
                                                 EmptyKey);
        IDependency dependency2 = new Dependency(typeof(IClass2),
                                                 typeof(Class2),
                                                 DependencyLifetime.Scoped,
                                                 null,
                                                 EmptyKey);
        IDependency dependency3 = new Dependency(typeof(IGenericClass1<int, string>),
                                                 typeof(GenericClass1<int, string>),
                                                 DependencyLifetime.Scoped,
                                                 null,
                                                 EmptyKey);
        IDependency dependency4 = new Dependency(typeof(IClass1),
                                                 typeof(Class1A),
                                                 DependencyLifetime.Scoped,
                                                 null,
                                                 key);
        SetupMockDependencyList(mockDependencyList, [dependency1, dependency2, dependency3, dependency4]);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);

        // Act
        IContainer container = builder
            .AddScoped(b => b
                .WithDependencyType<IClass1>()
                .WithResolvingType<Class1>())
            .AddScoped<IClass2>(b => b
                .WithResolvingType<Class2>())
            .AddScoped<IGenericClass1<int, string>, GenericClass1<int, string>>()
            .AddScoped<IClass1, Class1A>(b => b
                .WithResolvingKey(key))
            .Build();

        // Assert
        AssertValidContainer(container, mockServiceLocater);
    }

    [Fact]
    public void AddValidSingletonDependenciesToContainer_ShouldBuildContainer()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        mockServiceLocater.CreateMock<IContainer>();
        string key = "test";
        IDependency dependency1 = new Dependency(typeof(IClass1),
                                                 typeof(Class1),
                                                 DependencyLifetime.Singleton,
                                                 null,
                                                 EmptyKey);
        IDependency dependency2 = new Dependency(typeof(IClass2),
                                                 typeof(Class2),
                                                 DependencyLifetime.Singleton,
                                                 null,
                                                 EmptyKey);
        IDependency dependency3 = new Dependency(typeof(IGenericClass1<int, string>),
                                                 typeof(GenericClass1<int, string>),
                                                 DependencyLifetime.Singleton,
                                                 null,
                                                 EmptyKey);
        IDependency dependency4 = new Dependency(typeof(IClass1),
                                                 typeof(Class1A),
                                                 DependencyLifetime.Singleton,
                                                 null,
                                                 key);
        SetupMockDependencyList(mockDependencyList, [dependency1, dependency2, dependency3, dependency4]);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);

        // Act
        IContainer container = builder
            .AddSingleton(b => b
                .WithDependencyType<IClass1>()
                .WithResolvingType<Class1>())
            .AddSingleton<IClass2>(b => b
                .WithResolvingType<Class2>())
            .AddSingleton<IGenericClass1<int, string>, GenericClass1<int, string>>()
            .AddSingleton<IClass1, Class1A>(b => b
                .WithResolvingKey(key))
            .Build();

        // Assert
        AssertValidContainer(container, mockServiceLocater);
    }

    [Fact]
    public void AddValidTransientDependenciesToContainer_ShouldBuildContainer()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        mockServiceLocater.CreateMock<IContainer>();
        string key = "test";
        IDependency dependency1 = new Dependency(typeof(IClass1),
                                                 typeof(Class1),
                                                 DependencyLifetime.Transient,
                                                 null,
                                                 EmptyKey);
        IDependency dependency2 = new Dependency(typeof(IClass2),
                                                 typeof(Class2),
                                                 DependencyLifetime.Transient,
                                                 null,
                                                 EmptyKey);
        IDependency dependency3 = new Dependency(typeof(IGenericClass1<int, string>),
                                                 typeof(GenericClass1<int, string>),
                                                 DependencyLifetime.Transient,
                                                 null,
                                                 EmptyKey);
        IDependency dependency4 = new Dependency(typeof(IClass1),
                                                 typeof(Class1A),
                                                 DependencyLifetime.Transient,
                                                 null,
                                                 key);
        SetupMockDependencyList(mockDependencyList, [dependency1, dependency2, dependency3, dependency4]);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);

        // Act
        IContainer container = builder
            .AddTransient(b => b
                .WithDependencyType<IClass1>()
                .WithResolvingType<Class1>())
            .AddTransient<IClass2>(b => b
                .WithResolvingType<Class2>())
            .AddTransient<IGenericClass1<int, string>, GenericClass1<int, string>>()
            .AddTransient<IClass1, Class1A>(b => b
                .WithResolvingKey(key))
            .Build();

        // Assert
        AssertValidContainer(container, mockServiceLocater);
    }

    [Fact]
    public void GetContainerBuilderMoreThanOnce_ReturnsSameInstanceEachTime()
    {
        // Arrange/Act
        IContainerBuilder builder1 = ContainerBuilder.Current;
        IContainerBuilder builder2 = ContainerBuilder.Current;
        IContainerBuilder builder3 = ContainerBuilder.Current;

        // Assert
        builder1
            .Should()
            .BeSameAs(builder2)
            .And
            .BeSameAs(builder3);
    }

    [Fact]
    public void TryToAddDependencyAfterContainerBuilt_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        mockServiceLocater.CreateMock<IContainer>();
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Transient,
                                                null,
                                                EmptyKey);
        SetupMockDependencyList(mockDependencyList, [dependency]);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);
        _ = builder
            .AddTransient<IClass1>(b => b
                .WithResolvingType<Class1>())
            .Build();
        string msg = MsgCantAddToContainerAfterBuild;

        // Act
        void action() => builder
            .AddTransient<IClass2>(b => b
                .WithResolvingType<Class2>());

        // Assert
        AssertException<ContainerBuildException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void TryToBuildContainerAfterContainerAlreadyBuilt_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        mockServiceLocater.CreateMock<IContainer>();
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Transient,
                                                null,
                                                EmptyKey);
        SetupMockDependencyList(mockDependencyList, [dependency]);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);
        _ = builder
            .AddTransient<IClass1>(b => b
                .WithResolvingType<Class1>())
            .Build();
        string msg = MsgContainerCantBeBuiltMoreThanOnce;

        // Act
        void action() => builder
            .Build();

        // Assert
        AssertException<ContainerBuildException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void TryToBuildEmptyContainer_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyListBuilder> mockDependencyList = mockServiceLocater.GetMock<IDependencyListBuilder>();
        mockServiceLocater.CreateMock<IContainer>();
        mockDependencyList
            .SetupGet(m => m.Count)
            .Returns(0);
        IContainerBuilder builder = ContainerBuilder.GetTestInstance(mockServiceLocater);
        string msg = MsgContainerIsEmpty;

        // Act
        void action() => builder.Build();

        // Assert
        AssertException<ContainerBuildException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    private static void AssertValidContainer(IContainer container, MockServiceLocater mockServiceLocater)
    {
        container
            .Should()
            .NotBeNull();
        container
            .Should()
            .BeSameAs(mockServiceLocater.Get<IContainer>());
        mockServiceLocater.VerifyMocks();
    }

    private static void SetupMockDependencyList(Mock<IDependencyListBuilder> mockDependencyList, IDependency[] dependencies)
    {
        foreach (IDependency dependency in dependencies)
        {
            mockDependencyList
                .Setup(m => m.Add(dependency))
                .Verifiable(Times.Once);
        }

        mockDependencyList
            .SetupGet(m => m.Count)
            .Returns(dependencies.Length)
            .Verifiable(Times.Once);
    }
}