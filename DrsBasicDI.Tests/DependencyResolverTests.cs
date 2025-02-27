namespace DrsBasicDI;

public class DependencyResolverTests
{
    [Fact]
    public void ConstructUsingNullDependencies_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        string parameterName = "dependencies";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new DependencyResolver(null!, mockNonScopedService.Object);

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
        Dictionary<Type, IDependency> dependencies = [];
        string parameterName = "nonScoped";
        string expected = string.Format(MsgInvalidNullArgument, parameterName);

        // Act
        Action action = () => _ = new DependencyResolver(dependencies, null!);

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
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        IClass2? expected = new Class2();
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService(expected, true);
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, mockScopedService, mockDependency);
    }

    [Fact]
    public void ResolveDependency_DependencyFoundInScopedService_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass2 expected = new Class2();
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService(expected, true);
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        Dictionary<Type, IDependency> dependencies = [];
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass2 actual = resolver.Resolve<IClass2>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, mockScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyMappingNotDefined1_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        Dictionary<Type, IDependency> dependencies = [];
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object);
        string expected = string.Format(MsgDependencyMappingNotFound, nameof(IClass2));

        // Act
        Action action = () => _ = resolver.Resolve<IClass2>();

        // Assert
        action
            .Should()
            .ThrowExactly<DependencyInjectionException>()
            .WithMessage(expected);
        VerifyMocks(mockNonScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyMappingNotDefined2_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        Dictionary<Type, IDependency> dependencies = [];
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);
        string expected = string.Format(MsgDependencyMappingNotFound, nameof(IClass2));

        // Act
        Action action = () => _ = resolver.Resolve<IClass2>();

        // Assert
        action
            .Should()
            .ThrowExactly<DependencyInjectionException>()
            .WithMessage(expected);
        VerifyMocks(mockNonScopedService, mockScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyObjectIsNull1_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), null!));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object);
        string expected = string.Format(MsgNullDependencyObject, nameof(IClass2));

        // Act
        Action action = () => _ = resolver.Resolve<IClass2>();

        // Assert
        action
            .Should()
            .ThrowExactly<DependencyInjectionException>()
            .WithMessage(expected);
        VerifyMocks(mockNonScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyObjectIsNull2_ShouldThrowException()
    {
        // Arrange
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), null!));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);
        string expected = string.Format(MsgNullDependencyObject, nameof(IClass2));

        // Act
        Action action = () => _ = resolver.Resolve<IClass2>();

        // Assert
        action
            .Should()
            .ThrowExactly<DependencyInjectionException>()
            .WithMessage(expected);
        VerifyMocks(mockNonScopedService, mockScopedService);
    }

    [Fact]
    public void ResolveDependency_FactoryThrowsException_ShouldThrowException()
    {
        // Arrange
        static object factory() => throw new InvalidOperationException();
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        Mock<IDependency> mockDependency = GetMockDependency(factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object);
        string expected = string.Format(MsgFactoryInvocationError, nameof(IClass2));

        // Act
        Action action = () => _ = resolver.Resolve<IClass2>();

        // Assert
        action
            .Should()
            .ThrowExactly<DependencyInjectionException>()
            .WithMessage(expected);
        VerifyMocks(mockNonScopedService, null, mockDependency);
    }

    [Fact]
    public void ResolveScopedDependency_FactoryMethodIsNotNull1_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass1 expected = new Class1("Factory");
        object factory() => expected;
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService(null, false, expected);
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Scoped, factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, null, mockDependency);
    }

    [Fact]
    public void ResolveScopedDependency_FactoryMethodIsNotNull2_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass1 expected = new Class1("Factory");
        object factory() => expected;
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService(null, false, expected);
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Scoped, factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, mockScopedService, mockDependency);
    }

    [Fact]
    public void ResolveSingletonDependency_FactoryMethodIsNotNull1_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass1 expected = new Class1("Factory");
        object factory() => expected;
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService(null, false, expected);
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Singleton, factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, null, mockDependency);
    }

    [Fact]
    public void ResolveSingletonDependency_FactoryMethodIsNotNull2_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass1 expected = new Class1("Factory");
        object factory() => expected;
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService(null, false, expected);
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass1>(null);
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Singleton, factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, mockScopedService, mockDependency);
    }

    [Fact]
    public void ResolveTransientDependency_FactoryMethodIsNotNull1_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass1 expected = new Class1("Factory");
        object factory() => expected;
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null);
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient, factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, null, mockDependency);
    }

    [Fact]
    public void ResolveTransientDependency_FactoryMethodIsNotNull2_ShouldReturnResolvingObject()
    {
        // Arrange
        IClass1 expected = new Class1("Factory");
        object factory() => expected;
        Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null);
        Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass1>(null);
        Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient, factory: factory);
        Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        DependencyResolver resolver = new(dependencies, mockNonScopedService.Object, mockScopedService.Object);

        // Act
        IClass1 actual = resolver.Resolve<IClass1>();

        // Assert
        actual
            .Should()
            .BeSameAs(expected);
        VerifyMocks(mockNonScopedService, null, mockDependency);
    }

    private static Dictionary<Type, IDependency> GetDependencies(params (Type, IDependency)[] dependencies)
    {
        Dictionary<Type, IDependency> result = [];

        foreach ((Type key, IDependency value) in dependencies)
        {
            result.Add(key, value);
        }

        return result;
    }

    private static Mock<IDependency> GetMockDependency(DependencyLifetime? lifetime = null,
                                                       Type? resolvingType = null,
                                                       Func<object>? factory = null,
                                                       Type? dependencyType = null)
    {
        Mock<IDependency> mock = new(MockBehavior.Strict);

        if (dependencyType is not null)
        {
            mock
                .SetupGet(m => m.DependencyType)
                .Returns(dependencyType);
        }

        if (factory is not null)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(factory);
        }

        if (lifetime is not null)
        {
            mock
                .SetupGet(m => m.Lifetime)
                .Returns((DependencyLifetime)lifetime);
        }

        if (resolvingType is not null)
        {
            mock
                .SetupGet(m => m.ResolvingType)
                .Returns(resolvingType);
        }

        return mock;
    }

    private static Mock<IResolvingObjectsService> GetMockResolvingObjectsService()
        => new(MockBehavior.Strict);

    private static Mock<IResolvingObjectsService> GetMockResolvingObjectsService<T>(T? getObject = null,
                                                                                    bool isFound = false,
                                                                                    T? addObject = null,
                                                                                    T? returnedObject = null) where T : class
    {
        Mock<IResolvingObjectsService> mock = GetMockResolvingObjectsService();

        SetupMockResolvingObjectsService(mock, getObject, isFound, addObject, returnedObject);

        return mock;
    }

    private static void SetupMockResolvingObjectsService<T>(Mock<IResolvingObjectsService> mock,
                                                            T? getObject,
                                                            bool isFound = false,
                                                            T? addObject = null,
                                                            T? returnedObject = null) where T : class
    {
        mock
            .Setup(m => m.TryGetResolvingObject(out getObject))
            .Returns(isFound)
            .Verifiable(Times.Once);

        if (addObject is not null)
        {
            if (returnedObject is not null)
            {
                mock
                    .Setup(m => m.Add(addObject))
                    .Returns(returnedObject)
                    .Verifiable(Times.Once);
            }
            else
            {
                mock
                    .Setup(m => m.Add(addObject))
                    .Returns(addObject)
                    .Verifiable(Times.Once);
            }
        }
    }

    private static void VerifyMocks(Mock<IResolvingObjectsService>? mockNonScopedService = null,
                                    Mock<IResolvingObjectsService>? mockScopedService = null,
                                    params Mock<IDependency>[] mockDependencies)
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

        foreach (Mock<IDependency> mockDependency in mockDependencies)
        {
            if (mockDependency.Setups.Any())
            {
                mockDependency.VerifyAll();
            }

            mockDependency.VerifyNoOtherCalls();
        }
    }
}