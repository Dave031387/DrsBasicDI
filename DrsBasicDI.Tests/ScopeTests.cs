namespace DrsBasicDI;

public class ScopeTests
{
    [Fact]
    public void ConstructScopeUsingValidDependencies_ShouldCreateValidScope()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IResolvingObjectsService> mockResolvingObjectsService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        Mock<IDependencyResolver> mockDependencyResolver = mockServiceLocater.GetMock<IDependencyResolver>(Scoped);
        mockDependencyResolver
            .Setup(m => m.SetScopedResolver(mockResolvingObjectsService.Object))
            .Verifiable(Times.Once);

        // Act
        Action action = () => _ = new Scope(mockServiceLocater);

        // Assert
        action
            .Should()
            .NotThrow<Exception>();
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void Dispose_ShouldCallDisposeOnResolvingObjectsService()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IResolvingObjectsService> mockResolvingObjectsService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        mockResolvingObjectsService
            .Setup(m => m.Dispose())
            .Verifiable(Times.Once);
        Mock<IDependencyResolver> mockDependencyResolver = mockServiceLocater.GetMock<IDependencyResolver>(Scoped);
        mockDependencyResolver
            .Setup(m => m.SetScopedResolver(mockResolvingObjectsService.Object))
            .Verifiable(Times.Once);
        Scope scope = new(mockServiceLocater);

        // Act
        scope.Dispose();

        // Assert
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void GetDependency_ShouldCallResolveOnDependencyResolver()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IResolvingObjectsService> mockResolvingObjectsService = mockServiceLocater.GetMock<IResolvingObjectsService>(Scoped);
        Mock<IDependencyResolver> mockDependencyResolver = mockServiceLocater.GetMock<IDependencyResolver>(Scoped);
        mockDependencyResolver
            .Setup(m => m.SetScopedResolver(mockResolvingObjectsService.Object))
            .Verifiable(Times.Once);
        Class1 expected = new();
        string resolvingKey = "test";
        mockDependencyResolver
            .Setup(m => m.Resolve<IClass1>(resolvingKey))
            .Returns(expected)
            .Verifiable(Times.Once);
        Scope scope = new(mockServiceLocater);

        // Act
        IClass1 actual = scope.GetDependency<IClass1>(resolvingKey);

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }
}