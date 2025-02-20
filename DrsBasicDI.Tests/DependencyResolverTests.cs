namespace DrsBasicDI;

public class DependencyResolverTests
{
    private readonly Dictionary<Type, IDependency> _dependencies = new()
    {
        { typeof(IClass1), new Dependency(typeof(IClass1), static () => new Class1("Factory"), DependencyLifetime.Transient, typeof(Class1)) },
        { typeof(IClass2), new Dependency(typeof(IClass2), null, DependencyLifetime.Singleton, typeof(Class2)) }
    };

    [Fact]
    public void ConstructUsingNullDependencies_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = new(MockBehavior.Strict);
        Dictionary<Type, IDependency> dependencies = null!;
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
        Dictionary<Type, IDependency> dependencies = [];
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

    [Fact]
    public void ResolveDependency_DependencyFoundInNonScopedService_ShouldReturnResolvingObject()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockScopedService = new(MockBehavior.Strict);
        IClass2? scopedClass2 = null;
        mockScopedService
            .Setup(m => m.TryGetResolvingObject(out scopedClass2))
            .Returns(false)
            .Verifiable(Times.Once);
        Mock<IResolvingObjectsService> mockNonScopedService = new(MockBehavior.Strict);
        IClass2? expected = new Class2();
        mockNonScopedService
            .Setup(m => m.TryGetResolvingObject(out expected))
            .Returns(true)
            .Verifiable(Times.Once);
        DependencyResolver resolver = new(_dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, mockScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyFoundInScopedService_ShouldReturnResolvingObject()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockScopedService = new(MockBehavior.Strict);
        IClass2? expected = new Class2();
        mockScopedService
            .Setup(m => m.TryGetResolvingObject(out expected))
            .Returns(true)
            .Verifiable(Times.Once);
        Mock<IResolvingObjectsService> mockNonScopedService = new(MockBehavior.Strict);
        DependencyResolver resolver = new(_dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, mockScopedService);
    }

    private static void VerifyMocks(Mock<IResolvingObjectsService>? mockNonScopedService = null,
                                    Mock<IResolvingObjectsService>? mockScopedService = null)
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
