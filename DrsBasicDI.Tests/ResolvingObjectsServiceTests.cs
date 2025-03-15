namespace DrsBasicDI;

public class ResolvingObjectsServiceTests
{
    [Fact]
    public void Dispose_ShouldCallDisposeOnDisposableObjectsAndRemoveAllObjectsFromList()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyListConsumer>();
        ServiceKey serviceKey1 = new(typeof(Class1), EmptyKey);
        ServiceKey serviceKey2 = new(typeof(CanDispose), "test");
        Mock<ICanDispose> mockDisposableObject = mockServiceLocater.GetMock<ICanDispose>();
        mockDisposableObject
            .Setup(m => m.Dispose())
            .Verifiable(Times.Once);
        ResolvingObjectsService resolvingObjectsService = new(mockServiceLocater);
        resolvingObjectsService._resolvingObjects[serviceKey1] = new Class1();
        resolvingObjectsService._resolvingObjects[serviceKey2] = mockDisposableObject.Object;

        // Act
        resolvingObjectsService.Dispose();

        // Assert
        resolvingObjectsService._resolvingObjects
            .Should()
            .BeEmpty();
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void TryToAddNewObjectWhenMatchingObjectAlreadyExists_ShouldReturnExistingObjectAndNotAddNewObject()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        Class1 newObject = new();
        Class1 existingObject = new();
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Singleton,
                                                null,
                                                resolvingKey);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        mockDependencyList
            .Setup(m => m.Get<IClass1>(resolvingKey))
            .Returns(dependency);
        ResolvingObjectsService resolvingObjectsService = new(mockServiceLocater);
        resolvingObjectsService._resolvingObjects[serviceKey] = existingObject;

        // Act
        IClass1 actual = resolvingObjectsService.Add<IClass1>(newObject, resolvingKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(existingObject)
            .And
            .NotBeSameAs(newObject);
        resolvingObjectsService._resolvingObjects
            .Should()
            .ContainSingle()
            .And
            .ContainKey(serviceKey);
        resolvingObjectsService._resolvingObjects[serviceKey]
            .Should()
            .BeSameAs(existingObject);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void TryToAddNewObjectWhenMatchingObjectDoesNotExist_ShouldAddNewObjectAndReturnNewObject()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        Class1 newObject = new();
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Singleton,
                                                null,
                                                resolvingKey);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        mockDependencyList
            .Setup(m => m.Get<IClass1>(resolvingKey))
            .Returns(dependency);
        ResolvingObjectsService resolvingObjectsService = new(mockServiceLocater);

        // Act
        IClass1 actual = resolvingObjectsService.Add<IClass1>(newObject, resolvingKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(newObject);
        resolvingObjectsService._resolvingObjects
            .Should()
            .ContainSingle()
            .And
            .ContainKey(serviceKey);
        resolvingObjectsService._resolvingObjects[serviceKey]
            .Should()
            .BeSameAs(newObject);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void TryToGetResolvingObjectThatDoesNotExist_ShouldReturnFalseAndSetObjectToNull()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Singleton,
                                                null,
                                                resolvingKey);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        mockDependencyList
            .Setup(m => m.Get<IClass1>(resolvingKey))
            .Returns(dependency);
        ResolvingObjectsService resolvingObjectsService = new(mockServiceLocater);

        // Act
        bool actual = resolvingObjectsService.TryGetResolvingObject(out IClass1? resolvingObject, resolvingKey);

        // Assert
        actual
            .Should()
            .BeFalse();
        resolvingObject
            .Should()
            .BeNull();
    }

    [Fact]
    public void TryToGetResolvingObjectThatExistsAndIsNotNull_ShouldReturnTrueAndSetObject()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        Class1 expected = new();
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Singleton,
                                                null,
                                                resolvingKey);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        mockDependencyList
            .Setup(m => m.Get<IClass1>(resolvingKey))
            .Returns(dependency);
        ResolvingObjectsService resolvingObjectsService = new(mockServiceLocater);
        resolvingObjectsService._resolvingObjects[serviceKey] = expected;

        // Act
        bool actual = resolvingObjectsService.TryGetResolvingObject(out IClass1? resolvingObject, resolvingKey);

        // Assert
        actual
            .Should()
            .BeTrue();
        resolvingObject
            .Should()
            .BeSameAs(expected);
    }

    [Fact]
    public void TryToGetResolvingObjectThatExistsAndIsNull_ShouldReturnFalseAndSetObjectToNull()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        string resolvingKey = "test";
        IDependency dependency = new Dependency(typeof(IClass1),
                                                typeof(Class1),
                                                DependencyLifetime.Singleton,
                                                null,
                                                resolvingKey);
        ServiceKey serviceKey = ServiceKey.GetServiceResolvingKey(dependency);
        Mock<IDependencyListConsumer> mockDependencyList = mockServiceLocater.GetMock<IDependencyListConsumer>();
        mockDependencyList
            .Setup(m => m.Get<IClass1>(resolvingKey))
            .Returns(dependency);
        ResolvingObjectsService resolvingObjectsService = new(mockServiceLocater);
        resolvingObjectsService._resolvingObjects[serviceKey] = null!;

        // Act
        bool actual = resolvingObjectsService.TryGetResolvingObject(out IClass1? resolvingObject, resolvingKey);

        // Assert
        actual
            .Should()
            .BeFalse();
        resolvingObject
            .Should()
            .BeNull();
    }
}