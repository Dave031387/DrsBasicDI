namespace DrsBasicDI;

public class ScopeTests
{
    private readonly Mock<IContainerInternal> _mockContainer = new(MockBehavior.Strict);
    private readonly Mock<IDependencyResolver> _mockDependencyResolver = new(MockBehavior.Strict);
    private readonly Mock<IResolvingObjectsService> _mockResolvingObjectsService = new(MockBehavior.Strict);

    [Fact]
    public void CallDisposeMoreThanOnce_ShouldDisposeOfResolvingObjectsServiceOnlyOnce()
    {
        // Arrange
        Scope scope = GetScope();
        _mockResolvingObjectsService
            .Setup(o => o.Clear())
            .Verifiable(Times.Once);
        bool disposing = true;

        // Act
        scope.Dispose(disposing);
        scope.Dispose(disposing);

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        _mockResolvingObjectsService
            .VerifyAll();
        _mockContainer
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void CallDisposeWithNoParameters_ShouldDisposeOfResolvingObjectsService()
    {
        // Arrange
        Scope scope = GetScope();
        _mockResolvingObjectsService
            .Setup(o => o.Clear())
            .Verifiable(Times.Once);

        // Act
        scope.Dispose();

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        _mockResolvingObjectsService
            .VerifyAll();
        _mockContainer
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void CallDisposeWithParameterSetToFalse_ShouldNotDisposeOfResolvingObjectsService()
    {
        // Arrange
        Scope scope = GetScope();
        bool disposing = false;

        // Act
        scope.Dispose(disposing);

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        _mockResolvingObjectsService
            .VerifyAll();
        _mockContainer
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void CallDisposeWithParameterSetToTrue_ShouldDisposeOfResolvingObjectsService()
    {
        // Arrange
        Scope scope = GetScope();
        _mockResolvingObjectsService
            .Setup(o => o.Clear())
            .Verifiable(Times.Once);
        bool disposing = true;

        // Act
        scope.Dispose(disposing);

        // Assert
        scope._isDisposed
            .Should()
            .BeTrue();
        _mockResolvingObjectsService
            .VerifyAll();
        _mockContainer
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void ConstructScopeUsingNullContainer_ShouldThrowException()
    {
        // Arrange
        IContainerInternal container = null!;
        _mockResolvingObjectsService.Reset();
        _mockDependencyResolver.Reset();
        string parameterName = "container";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new Scope(container,
                                            _mockResolvingObjectsService.Object,
                                            _mockDependencyResolver.Object);

        // Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        _mockResolvingObjectsService
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void ConstructScopeUsingNullResolvingObjectsService_ShouldThrowException()
    {
        // Arrange
        IResolvingObjectsService resolvingObjectsService = null!;
        _mockContainer.Reset();
        _mockDependencyResolver.Reset();
        string parameterName = "resolvingObjectsService";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new Scope(_mockContainer.Object,
                                            resolvingObjectsService,
                                            _mockDependencyResolver.Object);

        // Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        _mockContainer
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void ConstructScopeUsingValidDependencies_ShouldCreateValidScope()
    {
        // Arrange
        Scope scope;
        _mockContainer.Reset();
        _mockResolvingObjectsService.Reset();
        _mockDependencyResolver.Reset();

        // Act
        scope = new Scope(_mockContainer.Object,
                          _mockResolvingObjectsService.Object,
                          _mockDependencyResolver.Object);

        // Assert
        scope
            .Should()
            .NotBeNull();
        scope._container
            .Should()
            .BeSameAs(_mockContainer.Object);
        scope._resolver
            .Should()
            .BeSameAs(_mockDependencyResolver.Object);
        scope._resolvingObjectsService
            .Should()
            .BeSameAs(_mockResolvingObjectsService.Object);
        _mockContainer
            .VerifyAll();
        _mockResolvingObjectsService
            .VerifyAll();
        _mockDependencyResolver
            .VerifyAll();
    }

    [Fact]
    public void GetDependency_ShouldCallDependencyResolver()
    {
        // Arrange
        Scope scope = GetScope();
        Class1 expected = new();
        _mockDependencyResolver
            .Setup(o => o.Resolve<IClass1>())
            .Returns(expected)
            .Verifiable(Times.Once);

        // Act
        IClass1 actual = scope.GetDependency<IClass1>();

        // Assert
        actual
            .Should()
            .NotBeNull();
        actual
            .Should()
            .BeSameAs(expected);
        _mockDependencyResolver
            .VerifyAll();
        _mockResolvingObjectsService
            .VerifyAll();
        _mockContainer
            .VerifyAll();
    }

    private Scope GetScope()
    {
        _mockContainer.Reset();
        _mockResolvingObjectsService.Reset();
        _mockDependencyResolver.Reset();
        return new(_mockContainer.Object,
                   _mockResolvingObjectsService.Object,
                   _mockDependencyResolver.Object);
    }
}