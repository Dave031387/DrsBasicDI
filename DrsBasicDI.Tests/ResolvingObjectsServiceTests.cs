namespace DrsBasicDI;

public class ResolvingObjectsServiceTests
{
    [Fact]
    public void AddNewObjectThatAlreadyExistsAndIsNotNull_ShouldReturnExistingObjectAndNotAddNewObject()
    {
        //// Arrange
        //Class1 myNewObject = new();
        //Class1 myExistingObject = new();
        //Type dependencyType = typeof(IClass1);
        //ResolvingObjectsService resolvingObjectsService = new();
        //resolvingObjectsService.ResolvingObjects[dependencyType] = myExistingObject;

        //// Act
        //IClass1 actual = resolvingObjectsService.Add<IClass1>(myNewObject);

        //// Assert
        //resolvingObjectsService.ResolvingObjects
        //    .Should()
        //    .ContainSingle()
        //    .And
        //    .ContainKey(dependencyType);
        //resolvingObjectsService.ResolvingObjects[dependencyType]
        //    .Should()
        //    .BeSameAs(myExistingObject);
        //actual
        //    .Should()
        //    .BeSameAs(myExistingObject);
    }

    [Fact]
    public void AddNewObjectThatAlreadyExistsButIsNull_ShouldAddObjectAndReturnNewObject()
    {
        //// Arrange
        //Class1 myObject = new();
        //Type dependencyType = typeof(IClass1);
        //ResolvingObjectsService resolvingObjectsService = new();
        //resolvingObjectsService.ResolvingObjects[dependencyType] = null!;

        //// Act
        //IClass1 actual = resolvingObjectsService.Add<IClass1>(myObject);

        //// Assert
        //resolvingObjectsService.ResolvingObjects
        //    .Should()
        //    .ContainSingle()
        //    .And
        //    .ContainKey(dependencyType);
        //resolvingObjectsService.ResolvingObjects[dependencyType]
        //    .Should()
        //    .BeSameAs(myObject);
        //actual
        //    .Should()
        //    .BeSameAs(myObject);
    }

    [Fact]
    public void AddNewObjectThatDoesAlreadyExist_ShouldAddObjectAndReturnNewObject()
    {
        //// Arrange
        //Class1 myObject = new();
        //Type dependencyType = typeof(IClass1);
        //ResolvingObjectsService resolvingObjectsService = new();

        //// Act
        //IClass1 actual = resolvingObjectsService.Add<IClass1>(myObject);

        //// Assert
        //resolvingObjectsService.ResolvingObjects
        //    .Should()
        //    .ContainSingle()
        //    .And
        //    .ContainKey(dependencyType);
        //resolvingObjectsService.ResolvingObjects[dependencyType]
        //    .Should()
        //    .BeSameAs(myObject);
        //actual
        //    .Should()
        //    .BeSameAs(myObject);
    }

    [Fact]
    public void ClearTheResolvedDependencies_ShouldRemoveAllDependenciesFromTheList()
    {
        //// Arrange
        //ResolvingObjectsService resolvingObjectsService = new();
        //resolvingObjectsService.ResolvingObjects[typeof(IClass1)] = new Class1();
        //resolvingObjectsService.ResolvingObjects[typeof(ICanDispose)] = new CanDispose();

        //// Act
        //resolvingObjectsService.Clear();

        //// Assert
        //resolvingObjectsService.ResolvingObjects
        //    .Should()
        //    .BeEmpty();
    }

    [Fact]
    public void TryToGetResolvingObjectThatDoesNotExist_ShouldReturnFalseAndSetObjectToNull()
    {
        //// Arrange
        //ResolvingObjectsService resolvingObjectsService = new();

        //// Act
        //bool actual = resolvingObjectsService.TryGetResolvingObject(out IClass1? resolvingObject);

        //// Assert
        //actual
        //    .Should()
        //    .BeFalse();
        //resolvingObject
        //    .Should()
        //    .BeNull();
    }

    [Fact]
    public void TryToGetResolvingObjectThatExistsAndIsNotNull_ShouldReturnTrueAndSetObject()
    {
        //// Arrange
        //Class1 myObject = new();
        //ResolvingObjectsService resolvingObjectsService = new();
        //resolvingObjectsService.ResolvingObjects[typeof(IClass1)] = myObject;

        //// Act
        //bool actual = resolvingObjectsService.TryGetResolvingObject(out IClass1? resolvingObject);

        //// Assert
        //actual
        //    .Should()
        //    .BeTrue();
        //resolvingObject
        //    .Should()
        //    .BeSameAs(myObject);
    }

    [Fact]
    public void TryToGetResolvingObjectThatExistsAndIsNull_ShouldReturnFalseAndSetObjectToNull()
    {
        //// Arrange
        //ResolvingObjectsService resolvingObjectsService = new();
        //resolvingObjectsService.ResolvingObjects[typeof(IClass1)] = null!;

        //// Act
        //bool actual = resolvingObjectsService.TryGetResolvingObject(out IClass1? resolvingObject);

        //// Assert
        //actual
        //    .Should()
        //    .BeFalse();
        //resolvingObject
        //    .Should()
        //    .BeNull();
    }
}