namespace DrsBasicDI;

public class ContainerTests
{
    private static readonly Type _class1DependencyType = typeof(IClass1);
    private static readonly DependencyLifetime _class1Lifetime = DependencyLifetime.Transient;
    private static readonly Type _class1ResolvingType = typeof(Class1);
    private static readonly Type _class2DependencyType = typeof(IClass2);
    private static readonly DependencyLifetime _class2Lifetime = DependencyLifetime.Singleton;
    private static readonly Type _class2ResolvingType = typeof(Class2);

    //private static readonly IDependency _myClass1Dependency = new Dependency()
    //{
    //    DependencyType = _class1DependencyType,
    //    ResolvingType = _class1ResolvingType,
    //    Lifetime = _class1Lifetime
    //};

    //private static readonly IDependency _myClass2Dependency = new Dependency()
    //{
    //    DependencyType = _class2DependencyType,
    //    ResolvingType = _class2ResolvingType,
    //    Lifetime = _class2Lifetime
    //};

    //private readonly List<IDependency> _dependencyList = [_myClass1Dependency, _myClass2Dependency];

    [Fact]
    public void ConstructContainerUsingNullDependenciesObject_ShouldThrowException()
    {
        //// Arrange
        //Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        //Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        //Container container;
        //List<IDependency> dependencies = null!;
        //string parameterName = "dependencies";
        //string expected = string.Format(MsgInvalidNullArgument, parameterName);

        //// Act
        //Action action = () => container = new(dependencies,
        //                                      mockResolvingObjectsService.Object,
        //                                      mockDependencyResolver.Object);

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<ArgumentNullException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void ConstructContainerUsingNullResolvingObjectsService_ShouldThrowException()
    {
        //// Arrange
        //Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        //Container container;
        //List<IDependency> dependencies = [];
        //IResolvingObjectsService resolvingObjectsService = null!;
        //string parameterName = "resolvingObjectsService";
        //string expected = string.Format(MsgInvalidNullArgument, parameterName);

        //// Act
        //Action action = () => container = new(dependencies,
        //                                      resolvingObjectsService,
        //                                      mockDependencyResolver.Object);

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<ArgumentNullException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockDependencyResolver);
    }

    [Fact]
    public void ConstructContainerUsingValidDependencies_ShouldConstructValidContainer()
    {
        //// Arrange
        //Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        //Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        //IContainer? passedContainer = null;
        //mockResolvingObjectsService
        //    .Setup(x => x.Add<IContainer>(It.IsAny<Container>()))
        //    .Returns(It.IsAny<IContainer>())
        //    .Callback<IContainer>(x => passedContainer = x)
        //    .Verifiable(Times.Once);
        //Container container;
        //Type containerType = typeof(IContainer);
        //IDependency containerDependency = new Dependency()
        //{
        //    DependencyType = containerType,
        //    ResolvingType = typeof(Container),
        //    Lifetime = DependencyLifetime.Singleton
        //};

        //// Act
        //container = new(_dependencyList,
        //                mockResolvingObjectsService.Object,
        //                mockDependencyResolver.Object);

        //// Assert
        //container
        //    .Should()
        //    .NotBeNull();
        //passedContainer
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(container);
        //container.ResolvingObjectsService
        //    .Should()
        //    .BeSameAs(mockResolvingObjectsService.Object);
        //container.Dependencies
        //    .Should()
        //    .NotBeEmpty()
        //    .And
        //    .ContainKeys(containerType, _class1DependencyType, _class2DependencyType);
        //container.Dependencies[containerType]
        //    .Should()
        //    .BeEquivalentTo(containerDependency);
        //container.Dependencies[_class1DependencyType]
        //    .Should()
        //    .BeEquivalentTo(_myClass1Dependency);
        //container.Dependencies[_class2DependencyType]
        //    .Should()
        //    .BeEquivalentTo(_myClass2Dependency);
        //VerifyMocks(mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void CreateMultipleScopes_ShouldCreateUniqueScopes()
    {
        //// Arrange
        //Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        //Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        //Container container = GetTestContainer(mockResolvingObjectsService.Object, mockDependencyResolver.Object);

        //// Act
        //IScope scope1 = container.CreateScope();
        //IScope scope2 = container.CreateScope();

        //// Assert
        //scope1
        //    .Should()
        //    .NotBeNull();
        //((Scope)scope1)._container
        //    .Should()
        //    .BeSameAs(container);
        //scope2
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .NotBeSameAs(scope1);
        //((Scope)scope2)._container
        //    .Should()
        //    .BeSameAs(container);
        //((Scope)scope1).ResolvingObjectsService
        //    .Should()
        //    .NotBeSameAs(((Scope)scope2).ResolvingObjectsService);
        //((Scope)scope1).DependencyResolver
        //    .Should()
        //    .NotBeSameAs(((Scope)scope2).DependencyResolver);
        //VerifyMocks(mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void CreateScope_ShouldCreateValidScope()
    {
        //// Arrange
        //Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        //Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        //Container container = GetTestContainer(mockResolvingObjectsService.Object, mockDependencyResolver.Object);

        //// Act
        //IScope scope = container.CreateScope();

        //// Assert
        //scope
        //    .Should()
        //    .NotBeNull();
        //((Scope)scope)._container
        //    .Should()
        //    .BeSameAs(container);
        //((Scope)scope).ResolvingObjectsService
        //    .Should()
        //    .NotBeNull();
        //((Scope)scope).DependencyResolver
        //    .Should()
        //    .NotBeNull();
        //VerifyMocks(mockDependencyResolver, mockResolvingObjectsService);
    }

    [Fact]
    public void GetDependency_ShouldCallDependencyResolver()
    {
        //// Arrange
        //Mock<IDependencyResolver> mockDependencyResolver = new(MockBehavior.Strict);
        //Mock<IResolvingObjectsService> mockResolvingObjectsService = new(MockBehavior.Strict);
        //Container container = GetTestContainer(mockResolvingObjectsService.Object, mockDependencyResolver.Object);
        //Class1 expected = new();
        //mockDependencyResolver
        //    .Setup(x => x.Resolve<IClass1>())
        //    .Returns(expected)
        //    .Verifiable(Times.Once);

        //// Act
        //IClass1 actual = container.GetDependency<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockDependencyResolver, mockResolvingObjectsService);
    }

    private static void VerifyMocks(Mock<IDependencyResolver>? mockDependencyResolver = null, Mock<IResolvingObjectsService>? mockResolvingObjectsService = null)
    {
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

    //private Container GetTestContainer(IResolvingObjectsService resolvingObjectsService, IDependencyResolver dependencyResolver)
    //{
    //    Mock<IResolvingObjectsService> mockResolvingObjectsService = Mock.Get(resolvingObjectsService);
    //    mockResolvingObjectsService
    //        .Setup(x => x.Add<IContainer>(It.IsAny<Container>()))
    //        .Returns(It.IsAny<IContainer>())
    //        .Verifiable(Times.Once);
    //    return new(_dependencyList,
    //               resolvingObjectsService,
    //               dependencyResolver);
    //}
}