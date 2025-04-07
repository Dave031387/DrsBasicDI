namespace DrsBasicDI;

using System.Reflection;

public class ObjectConstructorTests
{
    [Fact]
    public void ConstructObjectHavingConstructorParameters_ShouldConstructObject()
    {
        // Arrange
        ConstructorInfo? constructorInfo = typeof(Class1).GetConstructor([typeof(string)]);
        ObjectConstructor objectConstructor = new();
        object[] parameterValues = ["ObjectConstructor"];

        // Act
        IClass1 result = objectConstructor.Construct<IClass1>(constructorInfo!, parameterValues, EmptyKey);

        // Assert
        result
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class1>()
            .And
            .Match<IClass1>(x => x.BuiltBy == "ObjectConstructor");
    }

    [Fact]
    public void ConstructObjectNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        ConstructorInfo? constructorInfo = typeof(Class2).GetConstructor([]);
        ObjectConstructor objectConstructor = new();
        object[] parameterValues = [];
        string dependencyName = GetDependencyName(nameof(IClass1), EmptyKey);
        string resolvingName = GetResolvingName(nameof(Class2));
        string msg1 = string.Format(MsgResolvingObjectNotCreated, resolvingName, dependencyName);
        string msg2 = string.Format(MsgErrorDuringConstruction, resolvingName, dependencyName);

        // Act
        Action action = () => objectConstructor.Construct<IClass1>(constructorInfo!, parameterValues, EmptyKey);

        // Assert
        action
            .Should()
            .Throw<DependencyInjectionException>()
            .WithMessage(msg2)
            .And
            .InnerException
            .Should()
            .NotBeNull()
            .And
            .BeOfType<DependencyInjectionException>()
            .And
            .Match<DependencyInjectionException>(x => x.Message == msg1);
    }

    [Fact]
    public void ConstructObjectWithParameterlessConstructor_ShouldConstructObject()
    {
        // Arrange
        ConstructorInfo? constructorInfo = typeof(Class2).GetConstructor([]);
        ObjectConstructor objectConstructor = new();
        object[] parameterValues = [];

        // Act
        IClass2 result = objectConstructor.Construct<IClass2>(constructorInfo!, parameterValues, EmptyKey);

        // Assert
        result
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Class2>();
    }
}