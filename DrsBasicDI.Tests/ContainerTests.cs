namespace DrsBasicDI;

public class ContainerTests
{
    [Fact]
    public void ConstructContainerUsingValidDependencies_ShouldConstructValidContainer()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyResolver>(NonScoped);
        Mock<IResolvingObjectsService> mockResolvingObjectsService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        IContainer passedContainer = null!;
        mockResolvingObjectsService
            .Setup(m => m.Add<IContainer>(It.IsAny<Container>(), EmptyKey))
            .Callback<IContainer, string>((container, _) => passedContainer = container)
            .Returns(It.IsAny<IContainer>())
            .Verifiable(Times.Once);

        // Act
        IContainer container = new Container(mockServiceLocater);

        // Assert
        container
            .Should()
            .NotBeNull();
        passedContainer
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(container);
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void CreateScope_ShouldCreateValidScope()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        mockServiceLocater.CreateMock<IDependencyResolver>(NonScoped);
        mockServiceLocater.CreateMock<IScope>();
        Mock<IResolvingObjectsService> mockResolvingObjectsService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        mockResolvingObjectsService
            .Setup(m => m.Add<IContainer>(It.IsAny<Container>(), EmptyKey))
            .Returns(It.IsAny<IContainer>())
            .Verifiable(Times.Once);
        Container container = new(mockServiceLocater);

        // Act
        IScope scope = container.CreateScope();

        // Assert
        scope
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(mockServiceLocater.Get<IScope>());
        mockServiceLocater.VerifyMocks();
    }

    [Fact]
    public void GetDependency_ShouldCallDependencyResolver()
    {
        // Arrange
        MockServiceLocater mockServiceLocater = new();
        Mock<IDependencyResolver> mockDependencyResolver = mockServiceLocater.GetMock<IDependencyResolver>(NonScoped);
        Class1 expected = new();
        mockDependencyResolver
            .Setup(m => m.Resolve<IClass1>(EmptyKey))
            .Returns(expected)
            .Verifiable(Times.Once);
        mockServiceLocater.CreateMock<IScope>();
        Mock<IResolvingObjectsService> mockResolvingObjectsService = mockServiceLocater.GetMock<IResolvingObjectsService>(NonScoped);
        mockResolvingObjectsService
            .Setup(m => m.Add<IContainer>(It.IsAny<Container>(), EmptyKey))
            .Returns(It.IsAny<IContainer>())
            .Verifiable(Times.Once);
        Container container = new(mockServiceLocater);

        // Act
        IClass1 actual = container.GetDependency<IClass1>();

        // Assert
        actual
            .Should()
            .NotBeNull()
            .And
            .BeSameAs(expected);
        mockServiceLocater.VerifyMocks();
    }
}