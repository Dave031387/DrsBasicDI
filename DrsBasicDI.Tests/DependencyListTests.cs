namespace DrsBasicDI;

public class DependencyListTests
{
    [Fact]
    public void AddNewDependencyToList_ShouldAddDependencyToList()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency = GetMockDependency(dependencyType: typeof(IClass1), resolvingKey: EmptyKey);
        ServiceKey serviceKey = new(typeof(IClass1), EmptyKey);

        // Act
        dependencyList.Add(mockDependency.Object);

        // Assert
        dependencyList.Count
            .Should()
            .Be(1);
        dependencyList._dependencies
            .Should()
            .ContainSingle()
            .And
            .ContainKey(serviceKey);
        dependencyList._dependencies[serviceKey]
            .Should()
            .Be(mockDependency.Object);
    }

    [Fact]
    public void AddTwoDependenciesWithSameTypeButDifferentKeys_ShouldAddBothDependenciesToList()
    {
        // Arrange
        DependencyList dependencyList = new();
        string testKey = "test";
        Mock<IDependency> mockDependency1 = GetMockDependency(dependencyType: typeof(IClass1), resolvingKey: EmptyKey);
        Mock<IDependency> mockDependency2 = GetMockDependency(dependencyType: typeof(IClass1), resolvingKey: testKey);
        ServiceKey serviceKey1 = new(typeof(IClass1), EmptyKey);
        ServiceKey serviceKey2 = new(typeof(IClass1), testKey);

        // Act
        dependencyList.Add(mockDependency1.Object);
        dependencyList.Add(mockDependency2.Object);

        // Assert
        dependencyList.Count
            .Should()
            .Be(2);
        dependencyList._dependencies
            .Should()
            .HaveCount(2)
            .And
            .ContainKeys(serviceKey1, serviceKey2);
        dependencyList._dependencies[serviceKey1]
            .Should()
            .Be(mockDependency1.Object);
        dependencyList._dependencies[serviceKey2]
            .Should()
            .Be(mockDependency2.Object);
    }

    [Fact]
    public void AddTwoDependenciesWithSameTypeAndSameKey_ShouldThrowException()
    {
        // Arrange
        DependencyList dependencyList = new();
        Mock<IDependency> mockDependency = GetMockDependency(dependencyType: typeof(IClass1), resolvingKey: EmptyKey);
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
            .Be(1);
        dependencyList._dependencies
            .Should()
            .ContainSingle()
            .And
            .ContainKey(serviceKey);
        dependencyList._dependencies[serviceKey]
            .Should()
            .Be(mockDependency.Object);
    }
}
