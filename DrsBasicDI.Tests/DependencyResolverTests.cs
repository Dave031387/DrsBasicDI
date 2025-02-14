namespace DrsBasicDI;

public class DependencyResolverTests
{
    [Fact]
    public void ConstructUsingNullDependencies_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = new(MockBehavior.Strict);
        Dictionary<Type, Dependency> dependencies = null!;
        string parameterName = "dependencies";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new DependencyResolver(dependencies, mockNonScopedService.Object);

        // Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        VerifyMocks(mockNonScopedService);
    }

    [Fact]
    public void ConstructUsingNullNonScopedService_ShouldThrowException()
    {
        // Arrange
        IResolvingObjectsService nonScoped = null!;
        Dictionary<Type, Dependency> dependencies = [];
        string parameterName = "nonScoped";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new DependencyResolver(dependencies, nonScoped);

        // Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
    }

    private static void VerifyMocks(Mock<IResolvingObjectsService>? mockNonScopedService = null, Mock<IResolvingObjectsService>? mockScopedService = null)
    {
        if (mockNonScopedService is not null)
        {
            if (mockNonScopedService.Setups.Any())
            {
                mockNonScopedService.VerifyAll();
            }

            mockNonScopedService.VerifyNoOtherCalls();
        }

        if (mockScopedService is not null)
        {
            if (mockScopedService.Setups.Any())
            {
                mockScopedService.VerifyAll();
            }

            mockScopedService.VerifyNoOtherCalls();
        }
    }
}
