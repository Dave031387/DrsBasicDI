namespace DrsBasicDI;

public class DependencyResolverTests
{
    [Fact]
    public void ConstructUsingValidDependencies_ShouldConstructObject()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);

        // Act
        DependencyResolver actual = new(mockServiceLocater);

        // Assert
        actual
            .Should()
            .NotBeNull();
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void DependencyFactoryThrowsException_ShouldThrowExceptionAgain()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        static IClass1 factory() => throw new Exception("Factory exception");
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Transient, factory: factory);
        Mock<IDependencyListConsumer> dependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(dependencyList, mockDependency.Object, EmptyKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        DependencyResolver resolver = new(mockServiceLocater);
        string dependencyName = GetDependencyName(typeof(IClass1).Name, EmptyKey);
        string msg = string.Format(MsgFactoryInvocationError, dependencyName);

        // Act
        void action() => resolver.Resolve<IClass1>(EmptyKey);

        // Assert
        TestHelper.AssertException<DependencyInjectionException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void DisposeTwiceWhenScopedServiceIsNotNull_ShouldCallDisposeOnScopedServiceOnce()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        mockScopedService
            .Setup(m => m.Dispose())
            .Verifiable(Times.Once);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        resolver.Dispose();
        resolver.Dispose();

        // Assert
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void DisposeTwiceWhenScopedServiceIsNull_ShouldCallDisposeOnNonScopedServiceOnce()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        mockNonScopedService
            .Setup(m => m.Dispose())
            .Verifiable(Times.Once);
        DependencyResolver resolver = new(mockServiceLocater);

        // Act
        resolver.Dispose();
        resolver.Dispose();

        // Assert
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void DisposeWhenScopedServiceIsNotNull_ShouldCallDisposeOnScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        mockScopedService
            .Setup(m => m.Dispose())
            .Verifiable(Times.Once);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        resolver.Dispose();

        // Assert
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void DisposeWhenScopedServiceIsNull_ShouldCallDisposeOnNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        mockNonScopedService
            .Setup(m => m.Dispose())
            .Verifiable(Times.Once);
        DependencyResolver resolver = new(mockServiceLocater);

        // Act
        resolver.Dispose();

        // Assert
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ResolveScopedDependencyThatExistsInNonScopedService_ShouldReturnResolvingObjectFromNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependency> dependency = GetMockDependency(lifetime: DependencyLifetime.Scoped);
        Mock<IDependencyListConsumer> dependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        string resolvingKey = "test";
        SetupDependencyList<IClass1>(dependencyList, dependency.Object, EmptyKey);
        SetupDependencyList<IClass1>(dependencyList, dependency.Object, resolvingKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        IClass1 objectWithoutKey = new Class1();
        IClass1 objectWithKey = new Class1A();
        SetupResolvingObjectsService(mockNonScopedService, objectWithoutKey, EmptyKey);
        SetupResolvingObjectsService(mockNonScopedService, objectWithKey, resolvingKey);
        DependencyResolver resolver = new(mockServiceLocater);

        // Act
        IClass1 actualWithoutKey = resolver.Resolve<IClass1>(EmptyKey);
        IClass1 actualWithKey = resolver.Resolve<IClass1>(resolvingKey);

        // Assert
        actualWithoutKey
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(objectWithoutKey);
        actualWithKey
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(objectWithKey);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ResolveScopedDependencyThatExistsInScopedService_ShouldReturnResolvingObjectFromScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependency> dependency = GetMockDependency(lifetime: DependencyLifetime.Scoped);
        Mock<IDependencyListConsumer> dependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        string resolvingKey = "test";
        SetupDependencyList<IClass1>(dependencyList, dependency.Object, EmptyKey);
        SetupDependencyList<IClass1>(dependencyList, dependency.Object, resolvingKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        IClass1 objectWithoutKey = new Class1();
        IClass1 objectWithKey = new Class1A();
        SetupResolvingObjectsService(mockScopedService, objectWithoutKey, EmptyKey);
        SetupResolvingObjectsService(mockScopedService, objectWithKey, resolvingKey);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass1 actualWithoutKey = resolver.Resolve<IClass1>(EmptyKey);
        IClass1 actualWithKey = resolver.Resolve<IClass1>(resolvingKey);

        // Assert
        actualWithoutKey
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(objectWithoutKey);
        actualWithKey
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(objectWithKey);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ResolveSingletonDependencyThatExistsInNonScopedService_ShouldReturnResolvingObjectFromNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependency> dependency = GetMockDependency(lifetime: DependencyLifetime.Singleton);
        Mock<IDependencyListConsumer> dependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        string resolvingKey = "test";
        SetupDependencyList<IClass1>(dependencyList, dependency.Object, EmptyKey);
        SetupDependencyList<IClass1>(dependencyList, dependency.Object, resolvingKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        IClass1 objectWithoutKey = new Class1();
        IClass1 objectWithKey = new Class1A();
        SetupResolvingObjectsService(mockNonScopedService, objectWithoutKey, EmptyKey);
        SetupResolvingObjectsService(mockNonScopedService, objectWithKey, resolvingKey);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass1 actualWithoutKey = resolver.Resolve<IClass1>(EmptyKey);
        IClass1 actualWithKey = resolver.Resolve<IClass1>(resolvingKey);

        // Assert
        actualWithoutKey
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(objectWithoutKey);
        actualWithKey
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(objectWithKey);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void SetScopedService_ShouldSetScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        DependencyResolver resolver = new(mockServiceLocater);

        // Act
        resolver.SetScopedService(mockScopedService.Object);

        // Assert
        resolver.ScopedService
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(mockScopedService.Object);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void SetScopedServiceMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);
        string msg = MsgScopedServiceAlreadySet;

        // Act
        void action() => resolver.SetScopedService(mockScopedService.Object);

        // Assert
        TestHelper.AssertException<DependencyInjectionException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void SetScopedServiceSameAsNonScopedService_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        DependencyResolver resolver = new(mockServiceLocater);
        string msg = MsgScopedServiceSameAsNonScopedService;

        // Act
        void action() => resolver.SetScopedService(mockNonScopedService.Object);

        // Assert
        TestHelper.AssertException<DependencyInjectionException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void SetScopedServiceToNull_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        DependencyResolver resolver = new(mockServiceLocater);
        string msg = MsgScopedServiceIsNull;

        // Act
        void action() => resolver.SetScopedService(null!);

        // Assert
        TestHelper.AssertException<DependencyInjectionException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    private static Mock<IDependency> GetMockDependency(DependencyLifetime? lifetime = null,
                                                       Type? resolvingType = null,
                                                       Func<object>? factory = null,
                                                       Type? dependencyType = null,
                                                       bool checkFactory = false)
    {
        Mock<IDependency> mock = new(MockBehavior.Strict);

        if (dependencyType is not null)
        {
            mock
                .SetupGet(m => m.DependencyType)
                .Returns(dependencyType);
        }

        if (factory is not null)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(factory);
        }
        else if (checkFactory)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(null!);
        }

        if (lifetime is not null)
        {
            mock
                .SetupGet(m => m.Lifetime)
                .Returns((DependencyLifetime)lifetime);
        }

        if (resolvingType is not null)
        {
            mock
                .SetupGet(m => m.ResolvingType)
                .Returns(resolvingType);
        }

        return mock;
    }

    private static void SetupDependencyList<T>(Mock<IDependencyListConsumer> mockDependencyListConsumer,
                                               IDependency dependency,
                                               string resolvingKey) where T : class
    {
        mockDependencyListConsumer
            .Setup(m => m.Get<T>(resolvingKey))
            .Returns(dependency)
            .Verifiable(Times.AtLeastOnce);
    }

    private static void SetupResolvingObjectsService<T>(Mock<IResolvingObjectsService> mockResolvingObjectsService,
                                                        T? resolvingObject,
                                                        string resolvingKey) where T : class
    {
        mockResolvingObjectsService
            .Setup(m => m.TryGetResolvingObject(out resolvingObject, resolvingKey))
            .Returns(resolvingObject is not null)
            .Verifiable(Times.Once);
    }
}