namespace DrsBasicDI;

public class ContainerTests
{
    private static readonly Type _class1DependencyType = typeof(IClass1);
    private static readonly DependencyLifetime _class1Lifetime = DependencyLifetime.Transient;
    private static readonly Type _class1ResolvingType = typeof(Class1);

    private static readonly Dependency _class1Dependency = new()
    {
        DependencyType = _class1DependencyType,
        ResolvingType = _class1ResolvingType,
        Lifetime = _class1Lifetime
    };

    private static readonly Type _class2DependencyType = typeof(IClass2);
    private static readonly DependencyLifetime _class2Lifetime = DependencyLifetime.Singleton;
    private static readonly Type _class2ResolvingType = typeof(Class2);

    private static readonly Dependency _class2Dependency = new()
    {
        DependencyType = _class2DependencyType,
        ResolvingType = _class2ResolvingType,
        Lifetime = _class2Lifetime
    };

    private readonly List<Dependency> _dependencies = [_class1Dependency, _class2Dependency];
    private readonly Mock<IDependencyResolver> _mockDependencyResolver = new(MockBehavior.Strict);
    private readonly Mock<IResolvingObjectsService> _mockResolvingObjectsService = new(MockBehavior.Strict);

    [Fact]
    public void ConstructContainerUsingNullDependenciesObject_ShouldThrowException()
    {
        // Arrange
        Container container;
        _mockDependencyResolver.Reset();
        _mockResolvingObjectsService.Reset();
        Action action = () => container = new(null!,
                                              _mockResolvingObjectsService.Object,
                                              _mockDependencyResolver.Object);
        string expected = "Value cannot be null. (Parameter 'dependencies')";

        // Act/Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        _mockDependencyResolver.VerifyAll();
        _mockResolvingObjectsService.VerifyAll();
    }

    [Fact]
    public void ConstructContainerUsingNullResolvingObjectsService_ShouldThrowException()
    {
        // Arrange
        Container container;
        _mockDependencyResolver.Reset();
        Action action = () => container = new([],
                                              null!,
                                              _mockDependencyResolver.Object);
        string expected = "Value cannot be null. (Parameter 'resolvingObjectsService')";

        // Act/Assert
        action
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage(expected);
        _mockDependencyResolver.VerifyAll();
    }

    [Fact]
    public void ConstructContainerUsingValidDependencies_ShouldConstructValidContainer()
    {
        // Arrange
        Container container;
        _mockDependencyResolver.Reset();
        _mockResolvingObjectsService.Reset();
        Type containerType = typeof(IContainer);
        Dependency containerDependency = new()
        {
            DependencyType = containerType,
            ResolvingType = typeof(Container),
            Lifetime = DependencyLifetime.Singleton
        };
        _mockResolvingObjectsService
            .Setup(x => x.Add<IContainer>(It.IsAny<Container>()))
            .Returns(It.IsAny<Container>())
            .Verifiable(Times.Once);

        // Act
        container = new(_dependencies,
                        _mockResolvingObjectsService.Object,
                        _mockDependencyResolver.Object);

        // Assert
        container.ResolvingObjectsService
            .Should()
            .BeSameAs(_mockResolvingObjectsService.Object);
        container.Dependencies
            .Should()
            .NotBeEmpty();
        container.Dependencies
            .Should()
            .ContainKey(containerType);
        container.Dependencies[containerType]
            .Should()
            .BeEquivalentTo(containerDependency);
        container.Dependencies
            .Should()
            .ContainKeys(_class1DependencyType, _class2DependencyType);
        container.Dependencies[_class1DependencyType]
            .Should()
            .BeEquivalentTo(_class1Dependency);
        container.Dependencies[_class2DependencyType]
            .Should()
            .BeEquivalentTo(_class2Dependency);
        _mockDependencyResolver.VerifyAll();
        _mockResolvingObjectsService.VerifyAll();
    }

    [Fact]
    public void CreateMultipleScopes_ShouldCreateUniqueScopes()
    {
        // Arrange
        Container container = GetTestContainer();

        // Act
        IScope scope1 = container.CreateScope();
        IScope scope2 = container.CreateScope();

        // Assert
        scope1
            .Should()
            .NotBeNull();
        ((Scope)scope1)._container
            .Should()
            .BeSameAs(container);
        scope2
            .Should()
            .NotBeNull();
        ((Scope)scope2)._container
            .Should()
            .BeSameAs(container);
        scope1
            .Should()
            .NotBeSameAs(scope2);
        ((Scope)scope1)._resolvingObjectsService
            .Should()
            .NotBeSameAs(((Scope)scope2)._resolvingObjectsService);
        ((Scope)scope1)._resolver
            .Should()
            .NotBeSameAs(((Scope)scope2)._resolver);
        _mockDependencyResolver.VerifyAll();
        _mockResolvingObjectsService.VerifyAll();
    }

    [Fact]
    public void CreateScope_ShouldCreateValidScope()
    {
        // Arrange
        Container container = GetTestContainer();

        // Act
        IScope scope = container.CreateScope();

        // Assert
        scope
            .Should()
            .NotBeNull();
        ((Scope)scope)._container
            .Should()
            .BeSameAs(container);
        ((Scope)scope)._resolvingObjectsService
            .Should()
            .NotBeNull();
        ((Scope)scope)._resolver
            .Should()
            .NotBeNull();
        _mockDependencyResolver.VerifyAll();
        _mockResolvingObjectsService.VerifyAll();
    }

    [Fact]
    public void GetDependency_ShouldCallDependencyResolver()
    {
        // Arrange
        Container container = GetTestContainer();
        Class1 expected = new();
        _mockDependencyResolver
            .Setup(x => x.Resolve<IClass1>())
            .Returns(expected)
            .Verifiable(Times.Once);

        // Act
        IClass1 actual = container.GetDependency<IClass1>();

        // Assert
        actual
            .Should()
            .NotBeNull();
        actual
            .Should()
            .BeSameAs(expected);
        _mockDependencyResolver.VerifyAll();
        _mockResolvingObjectsService.VerifyAll();
    }

    private Container GetTestContainer()
    {
        _mockDependencyResolver.Reset();
        _mockResolvingObjectsService.Reset();
        _mockResolvingObjectsService
            .Setup(x => x.Add<IContainer>(It.IsAny<Container>()))
            .Returns(It.IsAny<Container>())
            .Verifiable(Times.Once);
        return new(_dependencies,
                   _mockResolvingObjectsService.Object,
                   _mockDependencyResolver.Object);
    }
}