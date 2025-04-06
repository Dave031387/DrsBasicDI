namespace DrsBasicDI;

public class ServiceLocaterTests
{
    // Arrange
    private readonly IServiceLocater _serviceLocater = ServiceLocater.Instance;

    [Fact]
    public void GetIContainer_ShouldReturnSingletonInstance()
    {
        // Act
        IContainer container1 = _serviceLocater.Get<IContainer>();
        IContainer container2 = _serviceLocater.Get<IContainer>();

        // Assert
        container1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Container>()
            .And
            .BeSameAs(container2);
    }
}
