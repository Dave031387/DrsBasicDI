namespace DrsBasicDI;

public class MessagesTests
{
    [Fact]
    public void FormatMessageWithDependencyTypeAndResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "This is a test message for {0}.";
        string expected = "This is a test message for dependency type IClass1 having resolving key \"test\".";

        // Act
        string actual = FormatMessage<IClass1>(msg, "test");

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithDependencyTypeButNoResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "This is a test message for {0}.";
        string expected = "This is a test message for dependency type IClass1.";

        // Act
        string actual = FormatMessage<IClass1>(msg);

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithForcedDependencyNameAndResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "This is a test message for {0}.";
        string expected = "This is a test message for dependency type N/A having resolving key \"test\".";

        // Act
        string actual = FormatMessage(msg, resolvingKey: "test", forceDependencyName: true);

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithForcedDependencyNameButNoResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "This is a test message for {0}.";
        string expected = "This is a test message for dependency type N/A.";

        // Act
        string actual = FormatMessage(msg, forceDependencyName: true);

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithoutParameters_ShouldReturnUnalteredMessage()
    {
        // Arrange
        string expected = "This is a test message.";

        // Act
        string actual = FormatMessage(expected);

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithResolvingTypeAndDependencyTypeAndResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "The {0} is mapped to {1}.";
        string expected = "The resolving type Class1 is mapped to dependency type IClass1 having resolving key \"test\".";

        // Act
        string actual = FormatMessage<IClass1>(msg, resolvingKey: "test", resolvingType: typeof(Class1));

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithResolvingTypeAndDependencyTypeButNoResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "The {0} is mapped to {1}.";
        string expected = "The resolving type Class1 is mapped to dependency type IClass1.";

        // Act
        string actual = FormatMessage<IClass1>(msg, resolvingType: typeof(Class1));

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithResolvingTypeAndForcedDependencyNameAndResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "The {0} is mapped to {1}.";
        string expected = "The resolving type Class1 is mapped to dependency type N/A having resolving key \"test\".";

        // Act
        string actual = FormatMessage(msg, resolvingKey: "test", resolvingType: typeof(Class1), forceDependencyName: true);

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithResolvingTypeAndForcedDependencyNameButNoResolvingKey_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "The {0} is mapped to {1}.";
        string expected = "The resolving type Class1 is mapped to dependency type N/A.";

        // Act
        string actual = FormatMessage(msg, resolvingType: typeof(Class1), forceDependencyName: true);

        // Assert
        actual
            .Should()
            .Be(expected);
    }

    [Fact]
    public void FormatMessageWithResolvingTypeButNoDependencyType_ShouldReturnFormattedMessage()
    {
        // Arrange
        string msg = "This is a test message for {0}.";
        string expected = "This is a test message for resolving type Class1.";

        // Act
        string actual = FormatMessage(msg, resolvingType: typeof(Class1));

        // Assert
        actual
            .Should()
            .Be(expected);
    }
}