namespace DrsBasicDI;

public class ScopeTests
{
    [Fact]
    public void CallDisposeMoreThanOnce_ShouldDisposeOfResolvingObjectsServiceOnlyOnce()
    {
        // Arrange
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        mockResolvingObjectsService
            .Setup(o => o.Clear())
            .Verifiable(Times.Once);
        Scope scope = new(mockContainer.Object,
                          mockResolvingObjectsService.Object,
                          mockDependencyResolver.Object);
        bool disposing = true;

        // Act
        scope.Dispose(disposing);
        scope.Dispose(disposing);

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        VerifyMocks(mockContainer, mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void CallDisposeWithNoParameters_ShouldDisposeOfResolvingObjectsService()
    {
        // Arrange
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        mockResolvingObjectsService
            .Setup(o => o.Clear())
            .Verifiable(Times.Once);
        Scope scope = new(mockContainer.Object,
                          mockResolvingObjectsService.Object,
                          mockDependencyResolver.Object);

        // Act
        scope.Dispose();

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        VerifyMocks(mockContainer, mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void CallDisposeWithParameterSetToFalse_ShouldNotDisposeOfResolvingObjectsService()
    {
        // Arrange
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        Scope scope = new(mockContainer.Object,
                          mockResolvingObjectsService.Object,
                          mockDependencyResolver.Object);
        bool disposing = false;

        // Act
        scope.Dispose(disposing);

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        VerifyMocks(mockContainer, mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void CallDisposeWithParameterSetToTrue_ShouldDisposeOfResolvingObjectsService()
    {
        // Arrange
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        mockResolvingObjectsService
            .Setup(o => o.Clear())
            .Verifiable(Times.Once);
        Scope scope = new(mockContainer.Object,
                          mockResolvingObjectsService.Object,
                          mockDependencyResolver.Object);
        bool disposing = true;

        // Act
        scope.Dispose(disposing);

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        VerifyMocks(mockContainer, mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void ConstructScopeUsingNullContainer_ShouldThrowException()
    {
        // Arrange
        IContainerInternal container = null!;
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        string parameterName = "container";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new Scope(container,
                                            mockResolvingObjectsService.Object,
                                            mockDependencyResolver.Object);

        // Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        VerifyMocks(null, mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void ConstructScopeUsingNullResolvingObjectsService_ShouldThrowException()
    {
        // Arrange
        IResolvingObjectsService resolvingObjectsService = null!;
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        string parameterName = "resolvingObjectsService";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new Scope(mockContainer.Object,
                                            resolvingObjectsService,
                                            mockDependencyResolver.Object);

        // Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        VerifyMocks(mockContainer, mockDependencyResolver);
    }

    [Fact]
    public void ConstructScopeUsingValidDependencies_ShouldCreateValidScope()
    {
        // Arrange
        Scope scope;
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);

        // Act
        scope = new Scope(mockContainer.Object,
                          mockResolvingObjectsService.Object,
                          mockDependencyResolver.Object);

        // Assert
        scope
            .Should()
            .NotBeNull();
        scope._container
            .Should()
            .BeSameAs(mockContainer.Object);
        scope._resolver
            .Should()
            .BeSameAs(mockDependencyResolver.Object);
        scope._resolvingObjectsService
            .Should()
            .BeSameAs(mockResolvingObjectsService.Object);
        VerifyMocks(mockContainer, mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void GetDependency_ShouldCallDependencyResolver()
    {
        // Arrange
        Mock<IContainerInternal> mockContainer = new(MockBehavior.Strict);
        Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        Class1 expected = new();
        mockDependencyResolver
            .Setup(o => o.Resolve<IClass1>())
            .Returns(expected)
            .Verifiable(Times.Once);
        Scope scope = new(mockContainer.Object,
                          mockResolvingObjectsService.Object,
                          mockDependencyResolver.Object);

        // Act
        IClass1 actual = scope.GetDependency<IClass1>();

        // Assert
        actual
            .Should()
            .NotBeNull();
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockContainer, mockDependencyResolver, mockResolvingObjectsService);
    }

    private static void VerifyMocks(Mock<IContainerInternal>? mockContainer = null,
                                    Mock<IDependencyResolver>? mockDependencyResolver = null,
                                    Mock<IResolvingObjectsService>? mockResolvingObjectsService = null)
    {
        if (mockContainer is not null)
        {
            if (mockContainer.Setups.Any())
            {
                mockContainer.VerifyAll();
            }

            mockContainer.VerifyNoOtherCalls();
        }

        if (mockDependencyResolver is not null)
        {
            if (mockDependencyResolver.Setups.Any())
            {
                mockDependencyResolver.VerifyAll();
            }
            mockDependencyResolver.VerifyNoOtherCalls();
        }

        if (mockResolvingObjectsService is not null)
        {
            if (mockResolvingObjectsService.Setups.Any())
            {
                mockResolvingObjectsService.VerifyAll();
            }
            mockResolvingObjectsService.VerifyNoOtherCalls();
        }
    }
}