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

    [Fact]
    public void GetIDependencyListBuilder_ShouldReturnSingletonInstance()
    {
        // Act
        IDependencyListBuilder builder1 = _serviceLocater.Get<IDependencyListBuilder>();
        IDependencyListBuilder builder2 = _serviceLocater.Get<IDependencyListBuilder>();

        // Assert
        builder1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<DependencyList>()
            .And
            .BeSameAs(builder2);
    }

    [Fact]
    public void GetIDependencyListBuilderAndIDependencyListConsumer_ShouldReturnSameInstance()
    {
        // Act
        IDependencyListBuilder builder = _serviceLocater.Get<IDependencyListBuilder>();
        IDependencyListConsumer consumer = _serviceLocater.Get<IDependencyListConsumer>();

        // Assert
        builder
            .Should()
            .BeSameAs(consumer);
    }

    [Fact]
    public void GetIDependencyListConsumer_ShouldReturnSingletonInstance()
    {
        // Act
        IDependencyListConsumer consumer1 = _serviceLocater.Get<IDependencyListConsumer>();
        IDependencyListConsumer consumer2 = _serviceLocater.Get<IDependencyListConsumer>();

        // Assert
        consumer1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<DependencyList>()
            .And
            .BeSameAs(consumer2);
    }

    [Fact]
    public void GetIObjectConstructor_ShouldReturnSingletonInstance()
    {
        // Act
        IObjectConstructor constructor1 = _serviceLocater.Get<IObjectConstructor>();
        IObjectConstructor constructor2 = _serviceLocater.Get<IObjectConstructor>();

        // Assert
        constructor1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ObjectConstructor>()
            .And
            .BeSameAs(constructor2);
    }

    [Fact]
    public void GetIScope_ShouldReturnTransientInstance()
    {
        // Act
        IScope scope1 = _serviceLocater.Get<IScope>();
        IScope scope2 = _serviceLocater.Get<IScope>();

        // Assert
        scope1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<Scope>()
            .And
            .NotBeSameAs(scope2);
    }

    [Fact]
    public void GetNonScopedIDependencyResolver_ShouldReturnSingletonInstance()
    {
        // Act
        IDependencyResolver resolver1 = _serviceLocater.Get<IDependencyResolver>(NonScoped);
        IDependencyResolver resolver2 = _serviceLocater.Get<IDependencyResolver>(NonScoped);

        // Assert
        resolver1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<DependencyResolver>()
            .And
            .BeSameAs(resolver2);
    }

    [Fact]
    public void GetNonScopedIDependencyResolverAndScopedIDependencyResolver_ShouldReturnDifferentInstances()
    {
        // Act
        IDependencyResolver nonScopedResolver = _serviceLocater.Get<IDependencyResolver>(NonScoped);
        IDependencyResolver scopedResolver = _serviceLocater.Get<IDependencyResolver>(Scoped);

        // Assert
        nonScopedResolver
            .Should()
            .NotBeSameAs(scopedResolver);
    }

    [Fact]
    public void GetNonScopedIResolvingObjectsService_ShouldReturnSingletonInstance()
    {
        // Act
        IResolvingObjectsService service1 = _serviceLocater.Get<IResolvingObjectsService>(NonScoped);
        IResolvingObjectsService service2 = _serviceLocater.Get<IResolvingObjectsService>(NonScoped);

        // Assert
        service1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ResolvingObjectsService>()
            .And
            .BeSameAs(service2);
    }

    [Fact]
    public void GetNonScopedIResolvingObjectsServiceAndScopedIResolvingObjectsService_ShouldReturnDifferentInstances()
    {
        // Act
        IResolvingObjectsService nonScopedService = _serviceLocater.Get<IResolvingObjectsService>(NonScoped);
        IResolvingObjectsService scopedService = _serviceLocater.Get<IResolvingObjectsService>(Scoped);

        // Assert
        nonScopedService
            .Should()
            .NotBeSameAs(scopedService);
    }

    [Fact]
    public void GetScopedIDependencyResolver_ShouldReturnTransientInstance()
    {
        // Act
        IDependencyResolver resolver1 = _serviceLocater.Get<IDependencyResolver>(Scoped);
        IDependencyResolver resolver2 = _serviceLocater.Get<IDependencyResolver>(Scoped);

        // Assert
        resolver1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<DependencyResolver>()
            .And
            .NotBeSameAs(resolver2);
    }

    [Fact]
    public void GetScopedIResolvingObjectsService_ShouldReturnTransientInstance()
    {
        // Act
        IResolvingObjectsService service1 = _serviceLocater.Get<IResolvingObjectsService>(Scoped);
        IResolvingObjectsService service2 = _serviceLocater.Get<IResolvingObjectsService>(Scoped);

        // Assert
        service1
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ResolvingObjectsService>()
            .And
            .NotBeSameAs(service2);
    }

    [Fact]
    public void TryToGetInvalidDependency_ShouldThrowException()
    {
        // Arrange
        string dependencyName = GetDependencyName(nameof(IObjectConstructor), Scoped);
        string msg = string.Format(MsgServiceNotRegistered, dependencyName);

        // Act
        void action() => _serviceLocater.Get<IObjectConstructor>(Scoped);

        // Assert
        AssertException<ServiceLocaterException>(action, msg);
    }
}