namespace DrsBasicDI;

public class ResolvedDependenciesTests
{
    [Fact]
    public void AddNewObjectThatAlreadyExistsAndIsNotNull_ShouldReturnExistingObjectAndNotAddNewObject()
    {
        // Arrange
        Class1 myNewObject = new();
        Class1 myExistingObject = new();
        Type dependencyType = typeof(IClass1);
        ResolvedDependencies resolvedDependencies = new();
        resolvedDependencies._resolvedDependencies[dependencyType] = myExistingObject;

        // Act
        IClass1 actual = resolvedDependencies.Add<IClass1>(myNewObject);

        // Assert
        resolvedDependencies._resolvedDependencies
            .Should()
            .ContainSingle();
        resolvedDependencies._resolvedDependencies
            .Should()
            .ContainKey(dependencyType);
        resolvedDependencies._resolvedDependencies[dependencyType]
            .Should()
            .BeSameAs(myExistingObject);
        actual
            .Should()
            .BeSameAs(myExistingObject);
    }

    [Fact]
    public void AddNewObjectThatAlreadyExistsButIsNull_ShouldAddObjectAndReturnNewObject()
    {
        // Arrange
        Class1 myObject = new();
        Type dependencyType = typeof(IClass1);
        ResolvedDependencies resolvedDependencies = new();
        resolvedDependencies._resolvedDependencies[dependencyType] = null!;

        // Act
        IClass1 actual = resolvedDependencies.Add<IClass1>(myObject);

        // Assert
        resolvedDependencies._resolvedDependencies
            .Should()
            .ContainSingle();
        resolvedDependencies._resolvedDependencies
            .Should()
            .ContainKey(dependencyType);
        resolvedDependencies._resolvedDependencies[dependencyType]
            .Should()
            .BeSameAs(myObject);
        actual
            .Should()
            .BeSameAs(myObject);
    }

    [Fact]
    public void AddNewObjectThatDoesAlreadyExist_ShouldAddObjectAndReturnNewObject()
    {
        // Arrange
        Class1 myObject = new();
        Type dependencyType = typeof(IClass1);
        ResolvedDependencies resolvedDependencies = new();

        // Act
        IClass1 actual = resolvedDependencies.Add<IClass1>(myObject);

        // Assert
        resolvedDependencies._resolvedDependencies
            .Should()
            .ContainSingle();
        resolvedDependencies._resolvedDependencies
            .Should()
            .ContainKey(dependencyType);
        resolvedDependencies._resolvedDependencies[dependencyType]
            .Should()
            .BeSameAs(myObject);
        actual
            .Should()
            .BeSameAs(myObject);
    }

    [Fact]
    public void ClearTheResolvedDependencies_ShouldRemoveAllDependenciesFromTheList()
    {
        // Arrange
        ResolvedDependencies resolvedDependencies = new();
        resolvedDependencies._resolvedDependencies[typeof(IClass1)] = new Class1();
        resolvedDependencies._resolvedDependencies[typeof(ICanDispose)] = new CanDispose();

        // Act
        resolvedDependencies.Clear();

        // Assert
        resolvedDependencies._resolvedDependencies
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void TryToGetResolvingObjectThatDoesNotExist_ShouldReturnFalseAndSetObjectToNull()
    {
        // Arrange
        ResolvedDependencies resolvedDependencies = new();

        // Act
        bool actual = resolvedDependencies.TryGetResolvedDependency(out IClass1? resolvingObject);

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
        Class1 myObject = new();
        ResolvedDependencies resolvedDependencies = new();
        resolvedDependencies._resolvedDependencies[typeof(IClass1)] = myObject;

        // Act
        bool actual = resolvedDependencies.TryGetResolvedDependency(out IClass1? resolvingObject);

        // Assert
        actual
            .Should()
            .BeTrue();
        resolvingObject
            .Should()
            .BeSameAs(myObject);
    }

    [Fact]
    public void TryToGetResolvingObjectThatExistsAndIsNull_ShouldReturnFalseAndSetObjectToNull()
    {
        // Arrange
        ResolvedDependencies resolvedDependencies = new();
        resolvedDependencies._resolvedDependencies[typeof(IClass1)] = null!;

        // Act
        bool actual = resolvedDependencies.TryGetResolvedDependency(out IClass1? resolvingObject);

        // Assert
        actual
            .Should()
            .BeFalse();
        resolvingObject
            .Should()
            .BeNull();
    }
}