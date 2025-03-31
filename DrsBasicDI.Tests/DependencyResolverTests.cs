namespace DrsBasicDI;

using System.Reflection;

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
    public void DependencyFactoryReturnsNull_ShouldThrowException()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Class1? expected = null;
        IClass1 factory() => expected!;
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Transient, factory: factory);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(mockDependencyList, mockDependency.Object, EmptyKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);
        string dependencyName = GetDependencyName(typeof(IClass1).Name, EmptyKey);
        string msg = string.Format(MsgFactoryShouldNotReturnNull, dependencyName);

        // Act
        void action() => resolver.Resolve<IClass1>(EmptyKey);

        // Assert
        TestHelper.AssertException<DependencyInjectionException>(action, msg);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void DependencyFactoryThrowsException_ShouldThrowExceptionAgain()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        static IClass1 factory() => throw new Exception("Factory exception");
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Transient, factory: factory);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(mockDependencyList, mockDependency.Object, EmptyKey);
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
    public void ResolveScopedDependencyHavingConstructorDependency_ShouldReturnResolvingObjectAndSaveItToScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        IClass2 dependency = new Class2();
        Type dependencyType = typeof(Class2);
        ConstructorInfo class2ConstructorInfo = dependencyType.GetConstructor(Type.EmptyTypes)!;
        object[] class2ParameterValues = [];
        Mock<IDependency> mockClass2Dependency = GetMockDependency(lifetime: DependencyLifetime.Singleton,
                                                                   resolvingType: dependencyType,
                                                                   factory: null,
                                                                   checkFactory: true);
        IClass3 expected = new Class3(dependency);
        Type resolvingType = typeof(Class3);
        ConstructorInfo class3ConstructorInfo = resolvingType.GetConstructor([dependencyType])!;
        object[] class3ParameterValues = [dependency];
        Mock<IDependency> mockClass3Dependency = GetMockDependency(lifetime: DependencyLifetime.Scoped,
                                                                   resolvingType: resolvingType,
                                                                   factory: null,
                                                                   checkFactory: true);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass2>(mockDependencyList, mockClass2Dependency.Object, EmptyKey);
        SetupDependencyList<IClass3>(mockDependencyList, mockClass3Dependency.Object, EmptyKey);
        Mock<IObjectConstructor> mockObjectConstructor = mockServiceLocater.GetMock<IObjectConstructor>();
        SetupObjectConstructor(mockObjectConstructor, class2ConstructorInfo, class2ParameterValues, EmptyKey, dependency);
        SetupObjectConstructor(mockObjectConstructor, class3ConstructorInfo, class3ParameterValues, EmptyKey, expected);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        SetupResolvingObjectsService(mockScopedService, expected, EmptyKey, addObject: true);
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        SetupResolvingObjectsService(mockNonScopedService, dependency, EmptyKey, addObject: true);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass3 actual = resolver.Resolve<IClass3>(EmptyKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class3>()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ResolveScopedDependencyHavingParameterlessConstructorWithoutScopedService_ShouldReturnResolvingObjectAndSaveItToNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        IClass2 expected = new Class2();
        Type resolvingType = typeof(Class2);
        ConstructorInfo constructorInfo = resolvingType.GetConstructor(Type.EmptyTypes)!;
        object[] parameterValues = [];
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Scoped,
                                                             resolvingType: resolvingType,
                                                             factory: null,
                                                             checkFactory: true);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass2>(mockDependencyList, mockDependency.Object, EmptyKey);
        Mock<IObjectConstructor> mockObjectConstructor = mockServiceLocater.GetMock<IObjectConstructor>();
        SetupObjectConstructor(mockObjectConstructor, constructorInfo, parameterValues, EmptyKey, expected);
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        SetupResolvingObjectsService(mockNonScopedService, expected, EmptyKey, addObject: true);
        DependencyResolver resolver = new(mockServiceLocater);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>(EmptyKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class2>()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ResolveScopedDependencyHavingParameterlessConstructorWithScopedService_ShouldReturnResolvingObjectAndSaveItToScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        IClass2 expected = new Class2();
        Type resolvingType = typeof(Class2);
        ConstructorInfo constructorInfo = resolvingType.GetConstructor(Type.EmptyTypes)!;
        object[] parameterValues = [];
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Scoped,
                                                             resolvingType: resolvingType,
                                                             factory: null,
                                                             checkFactory: true);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass2>(mockDependencyList, mockDependency.Object, EmptyKey);
        Mock<IObjectConstructor> mockObjectConstructor = mockServiceLocater.GetMock<IObjectConstructor>();
        SetupObjectConstructor(mockObjectConstructor, constructorInfo, parameterValues, EmptyKey, expected);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        SetupResolvingObjectsService(mockScopedService, expected, EmptyKey, addObject: true);
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>(EmptyKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class2>()
            .And
            .BeSameAs(expected);
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
    public void ResolveSingletonDependencyHavingParameterlessConstructor_ShouldReturnResolvingObjectAndSaveItToNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        IClass2 expected = new Class2();
        Type resolvingType = typeof(Class2);
        ConstructorInfo constructorInfo = resolvingType.GetConstructor(Type.EmptyTypes)!;
        object[] parameterValues = [];
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Singleton,
                                                             resolvingType: resolvingType,
                                                             factory: null,
                                                             checkFactory: true);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass2>(mockDependencyList, mockDependency.Object, EmptyKey);
        Mock<IObjectConstructor> mockObjectConstructor = mockServiceLocater.GetMock<IObjectConstructor>();
        SetupObjectConstructor(mockObjectConstructor, constructorInfo, parameterValues, EmptyKey, expected);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        SetupResolvingObjectsService(mockNonScopedService, expected, EmptyKey, addObject: true);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>(EmptyKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class2>()
            .And
            .BeSameAs(expected);
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
    public void ResolveTransientDependencyHavingParameterlessConstructor_ShouldReturnResolvingObjectButNotSaveIt()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        IClass2 expected = new Class2();
        Type resolvingType = typeof(Class2);
        ConstructorInfo constructorInfo = resolvingType.GetConstructor(Type.EmptyTypes)!;
        object[] parameterValues = [];
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Transient,
                                                             resolvingType: resolvingType,
                                                             factory: null,
                                                             checkFactory: true);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass2>(mockDependencyList, mockDependency.Object, EmptyKey);
        Mock<IObjectConstructor> mockObjectConstructor = mockServiceLocater.GetMock<IObjectConstructor>();
        SetupObjectConstructor(mockObjectConstructor, constructorInfo, parameterValues, EmptyKey, expected);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>(EmptyKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class2>()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ScopedDependencyFactoryReturnsValidValueAndScopedServiceIsNotSet_ShouldReturnValueAndSaveItInNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        Class1 expected = new("Factory");
        IClass1 factory() => expected;
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Scoped, factory: factory);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(mockDependencyList, mockDependency.Object, resolvingKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        SetupResolvingObjectsService<IClass1>(mockNonScopedService, expected, resolvingKey, addObject: true);
        DependencyResolver resolver = new(mockServiceLocater);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>(resolvingKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class1>()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void ScopedDependencyFactoryReturnsValidValueAndScopedServiceIsSet_ShouldReturnValueAndSaveItInScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        Class1 expected = new("Factory");
        IClass1 factory() => expected;
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Scoped, factory: factory);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(mockDependencyList, mockDependency.Object, resolvingKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        SetupResolvingObjectsService<IClass1>(mockScopedService, expected, resolvingKey, addObject: true);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>(resolvingKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class1>()
            .And
            .BeSameAs(expected);
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

    [Fact]
    public void SingletonDependencyFactoryReturnsValidValue_ShouldReturnValueAndSaveItInNonScopedService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        Class1 expected = new("Factory");
        IClass1 factory() => expected;
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Singleton, factory: factory);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(mockDependencyList, mockDependency.Object, resolvingKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        Mock<IResolvingObjectsService> mockNonScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        SetupResolvingObjectsService<IClass1>(mockNonScopedService, expected, resolvingKey, addObject: true);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>(resolvingKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class1>()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void TransientDependencyFactoryReturnsValidValue_ShouldReturnValueButNotSaveIt()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Class1 expected = new("Factory");
        IClass1 factory() => expected;
        Mock<IDependency> mockDependency = GetMockDependency(lifetime: DependencyLifetime.Transient, factory: factory);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        SetupDependencyList<IClass1>(mockDependencyList, mockDependency.Object, EmptyKey);
        mockServiceLocater.CreateMock<IObjectConstructor>();
        mockServiceLocater.CreateMock<IResolvingObjectsService>(NonScoped);
        Mock<IResolvingObjectsService> mockScopedService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        DependencyResolver resolver = new(mockServiceLocater);
        resolver.SetScopedService(mockScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>(EmptyKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class1>()
            .And
            .BeSameAs(expected);
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
                .Returns(dependencyType)
                .Verifiable(Times.AtLeastOnce);
        }

        if (factory is not null)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(factory)
                .Verifiable(Times.AtLeastOnce);
        }
        else if (checkFactory)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(null!)
                .Verifiable(Times.AtLeastOnce);
        }

        if (lifetime is not null)
        {
            mock
                .SetupGet(m => m.Lifetime)
                .Returns((DependencyLifetime)lifetime)
                .Verifiable(Times.AtLeastOnce);
        }

        if (resolvingType is not null)
        {
            mock
                .SetupGet(m => m.ResolvingType)
                .Returns(resolvingType)
                .Verifiable(Times.AtLeastOnce);
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

    private static void SetupObjectConstructor<T>(Mock<IObjectConstructor> mock,
                                                                               ConstructorInfo constructorInfo,
                                                  object[] parameterValues,
                                                  string key,
                                                  T resolvingObject) where T : class
    {
        mock
            .Setup(m => m.Construct<T>(constructorInfo, parameterValues, key))
            .Returns(resolvingObject)
            .Verifiable(Times.Once);
    }

    private static void SetupResolvingObjectsService<T>(Mock<IResolvingObjectsService> mockResolvingObjectsService,
                                                        T? resolvingObject,
                                                        string resolvingKey,
                                                        bool addObject = false) where T : class
    {
        if (addObject)
        {
            T? nullObject = null;

            mockResolvingObjectsService
                .Setup(m => m.TryGetResolvingObject(out nullObject, resolvingKey))
                .Returns(false)
                .Verifiable(Times.Once);
            mockResolvingObjectsService
                .Setup(m => m.Add(resolvingObject!, resolvingKey))
                .Returns(resolvingObject!)
                .Verifiable(Times.Once);
        }
        else
        {
            mockResolvingObjectsService
                .Setup(m => m.TryGetResolvingObject(out resolvingObject, resolvingKey))
                .Returns(resolvingObject is not null)
                .Verifiable(Times.Once);
        }
    }
}