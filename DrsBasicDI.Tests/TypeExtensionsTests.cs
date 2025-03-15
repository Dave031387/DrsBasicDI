namespace DrsBasicDI;

using System.Reflection;

public class TypeExtensionsTests
{
    [Theory]
    [MemberData(nameof(TestDataGenerator.GetArrayTypes), MemberType = typeof(TestDataGenerator))]
    public void GetFriendlyNameForArrayTypes_ShouldGenerateFriendlyName(Type type, string expected)
    {
        // Arrange/Act
        string actual = type.GetFriendlyName();

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetGenericTypes), MemberType = typeof(TestDataGenerator))]
    public void GetFriendlyNameForGenericTypes_ShouldGenerateFriendlyName(Type type, string expected)
    {
        // Arrange/Act
        string actual = type.GetFriendlyName();

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetNullableTypes), MemberType = typeof(TestDataGenerator))]
    public void GetFriendlyNameForNullableTypes_ShouldGenerateFriendlyName(Type type, string expected)
    {
        // Arrange/Act
        string actual = type.GetFriendlyName();

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator.GetPredefinedTypes), MemberType = typeof(TestDataGenerator))]
    public void GetFriendlyNameForPredefinedTypes_ShouldGenerateFriendlyName(Type type, string expected)
    {
        // Arrange/Act
        string actual = type.GetFriendlyName();

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void GetFriendlyNameForSimpleClassType_ShouldGenerateFriendlyName()
    {
        // Arrange
        Type type = typeof(Class1);
        string expected = nameof(Class1);

        // Act
        string actual = type.GetFriendlyName();

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void GetPrimaryConstructorInfoForClassType_ShouldReturnConstructorInfoWithMostParameters()
    {
        // Arrange
        Type type = typeof(Class1);
        ConstructorInfo expected = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, [typeof(int), typeof(string)])!;

        // Act
        ConstructorInfo actual = type.GetPrimaryConstructorInfo();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
    }

    [Fact]
    public void GetPrimaryConstructorInfoForValueType_ShouldThrowException()
    {
        // Arrange
        Type type = typeof(int);
        string typeName = "int";
        string expected = string.Format(MsgNoSuitableConstructors, typeName);

        // Act
        void action() => type.GetPrimaryConstructorInfo();

        // Assert
        TestHelper.AssertException<DependencyInjectionException>(action, expected);
    }
}