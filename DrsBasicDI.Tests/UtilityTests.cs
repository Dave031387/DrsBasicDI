namespace DrsBasicDI;

public class UtilityTests
{
    [Theory]
    [MemberData(nameof(TestDataGenerator.GetArrayTypes), MemberType = typeof(TestDataGenerator))]
    public void ArrayTypes_ShouldGenerateFriendlyName(Type type, string expected)
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
    public void GenericTypes_ShouldGenerateFriendlyName(Type type, string expected)
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
    public void NullableTypes_ShouldGenerateFriendlyName(Type type, string expected)
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
    public void PredefinedTypes_ShouldGenerateFriendlyName(Type type, string expected)
    {
        // Arrange/Act
        string actual = type.GetFriendlyName();

        // Assert
        actual
            .Should()
            .Be(expected);
    }
}