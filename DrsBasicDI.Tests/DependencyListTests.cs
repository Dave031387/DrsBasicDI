namespace DrsBasicDI;

public class DependencyListTests
{
    [Fact]
    public void AddNewDependencyToList_ShouldAddDependencyToList()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency = GetMockDependency<IClass1>(resolvingKey: EmptyKey);
        ServiceKey serviceKey = new(typeof(IClass1), EmptyKey);

        // Act
        dependencyList.Add(mockDependency.Object);

        // Assert
        dependencyList.Count
            .Should()
            .Be(2);
        dependencyList._dependencies
            .Should()
            .HaveCount(2)
            .And
            .ContainKey(serviceKey);
        dependencyList._dependencies[serviceKey]
            .Should()
            .BeSameAs(mockDependency.Object);
    }

    [Fact]
    public void AddTwoDependenciesWithSameTypeAndSameKey_ShouldThrowException()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency = GetMockDependency<IClass1>(resolvingKey: EmptyKey);
        ServiceKey serviceKey = new(typeof(IClass1), EmptyKey);
        dependencyList.Add(mockDependency.Object);
        string dependencyName = GetDependencyName(nameof(IClass1), EmptyKey);
        string msg = string.Format(MsgDuplicateDependency, dependencyName);

        // Act
        void action() => dependencyList.Add(mockDependency.Object);

        // Assert
        AssertException<ContainerBuildException>(action, msg);
        dependencyList.Count
            .Should()
            .Be(2);
        dependencyList._dependencies
            .Should()
            .HaveCount(2)
            .And
            .ContainKey(serviceKey);
        dependencyList._dependencies[serviceKey]
            .Should()
            .BeSameAs(mockDependency.Object);
    }

    [Fact]
    public void AddTwoDependenciesWithSameTypeButDifferentKeys_ShouldAddBothDependenciesToList()
    {
        // Arrange
        DependencyList dependencyList = new();
        string testKey = "test";
        Mock<IDependency> mockDependency1 = GetMockDependency<IClass1>(resolvingKey: EmptyKey);
        Mock<IDependency> mockDependency2 = GetMockDependency<IClass1>(resolvingKey: testKey);
        ServiceKey serviceKey1 = new(typeof(IClass1), EmptyKey);
        ServiceKey serviceKey2 = new(typeof(IClass1), testKey);

        // Act
        dependencyList.Add(mockDependency1.Object);
        dependencyList.Add(mockDependency2.Object);

        // Assert
        dependencyList.Count
            .Should()
            .Be(3);
        dependencyList._dependencies
            .Should()
            .HaveCount(3)
            .And
            .ContainKeys(serviceKey1, serviceKey2);
        dependencyList._dependencies[serviceKey1]
            .Should()
            .BeSameAs(mockDependency1.Object);
        dependencyList._dependencies[serviceKey2]
            .Should()
            .BeSameAs(mockDependency2.Object);
    }

    [Fact]
    public void ConstructNewDependencyList_ShouldContainContainerDependency()
    {
        // Arrange / Act
        DependencyList dependencyList = new();
        ServiceKey serviceKey = new(typeof(IContainer), EmptyKey);
        IDependency containerDependency = new Dependency(typeof(IContainer),
                                                         typeof(Container),
                                                         DependencyLifetime.Singleton,
                                                         null,
                                                         EmptyKey);

        // Assert
        dependencyList._dependencies
            .Should()
            .ContainSingle()
            .And
            .ContainKey(serviceKey);
        dependencyList._dependencies[serviceKey]
            .Should()
            .NotBeNull()
            .And
            .Be(containerDependency);
    }

    [Fact]
    public void GetAnExistingDependency_ShouldReturnDependency()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency = GetMockDependency<IClass1>(resolvingKey: EmptyKey);
        ServiceKey serviceKey = new(typeof(IClass1), EmptyKey);
        dependencyList._dependencies[serviceKey] = mockDependency.Object;

        // Act
        IDependency dependency = dependencyList.Get<IClass1>(EmptyKey);

        // Assert
        dependency
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(mockDependency.Object);
    }

    [Fact]
    public void GetAnExistingDependencyWithResolvingKey_ShouldReturnCorrectDependency()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency1 = GetMockDependency<IClass1>(resolvingKey: EmptyKey);
        ServiceKey serviceKey1 = new(typeof(IClass1), EmptyKey);
        dependencyList._dependencies[serviceKey1] = mockDependency1.Object;
        string resolvingKey = "test";
        Mock<IDependency> mockDependency2 = GetMockDependency<IClass1>(resolvingKey: resolvingKey);
        ServiceKey serviceKey2 = new(typeof(IClass1), resolvingKey);
        dependencyList._dependencies[serviceKey2] = mockDependency2.Object;

        // Act
        IDependency dependency = dependencyList.Get<IClass1>(resolvingKey);

        // Assert
        dependency
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(mockDependency2.Object);
    }

    [Fact]
    public void GetDependencyReturnsNull_ShouldThrowException()
    {
        // Arrange
        DependencyList dependencyList = new();
        ServiceKey serviceKey = new(typeof(IClass1), EmptyKey);
        dependencyList._dependencies[serviceKey] = null!;
        string msg = string.Format(MsgNullDependencyObject, GetDependencyName(nameof(IClass1), EmptyKey));

        // Act
        void action() => dependencyList.Get<IClass1>(EmptyKey);

        // Assert
        AssertException<DependencyInjectionException>(action, msg);
    }

    [Fact]
    public void GetDependencyThatDoesNotExist_ShouldThrowException()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency = GetMockDependency<IClass1>(resolvingKey: EmptyKey);
        ServiceKey serviceKey = new(typeof(IClass1), EmptyKey);
        dependencyList._dependencies[serviceKey] = mockDependency.Object;
        string resolvingKey = "test";
        string msg = string.Format(MsgDependencyMappingNotFound, GetDependencyName(nameof(IClass1), resolvingKey));

        // Act
        void action() => dependencyList.Get<IClass1>(resolvingKey);

        // Assert
        AssertException<DependencyInjectionException>(action, msg);
    }
}