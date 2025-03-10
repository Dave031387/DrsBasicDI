namespace DrsBasicDI;

public class DependencyResolverTests
{
    [Fact]
    public void ConstructUsingNullDependencies_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        //string parameterName = "dependencies";
        //string expected = string.Format(MsgInvalidNullArgument, parameterName);

        //// Act
        //Action action = () => _ = new DependencyResolver(null!, mockObjectConstructor.Object, mockNonScopedService.Object);

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<ArgumentNullException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor, mockNonScopedService);
    }

    [Fact]
    public void ConstructUsingNullNonScopedService_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Dictionary<Type, IDependency> dependencies = [];
        //string parameterName = "nonScoped";
        //string expected = string.Format(MsgInvalidNullArgument, parameterName);

        //// Act
        //Action action = () => _ = new DependencyResolver(dependencies, mockObjectConstructor.Object, null!);

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<ArgumentNullException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor);
    }

    [Fact]
    public void ResolveDependency_DependencyFoundInNonScopedService_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //IClass2? expected = new Class2();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(expected, true);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass2 actual = resolver.Resolve<IClass2>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService,
        //            mockDependency);
    }

    [Fact]
    public void ResolveDependency_DependencyFoundInScopedService_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass2 expected = new Class2();
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(expected, true);
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        //Dictionary<Type, IDependency> dependencies = [];
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass2 actual = resolver.Resolve<IClass2>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyMappingNotDefined1_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Dictionary<Type, IDependency> dependencies = [];
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);
        //string expected = string.Format(MsgDependencyMappingNotFound, nameof(IClass2));

        //// Act
        //Action action = () => _ = resolver.Resolve<IClass2>();

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<DependencyInjectionException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor, mockNonScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyMappingNotDefined2_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Dictionary<Type, IDependency> dependencies = [];
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);
        //string expected = string.Format(MsgDependencyMappingNotFound, nameof(IClass2));

        //// Act
        //Action action = () => _ = resolver.Resolve<IClass2>();

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<DependencyInjectionException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyObjectIsNull1_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), null!));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);
        //string expected = string.Format(MsgNullDependencyObject, nameof(IClass2));

        //// Act
        //Action action = () => _ = resolver.Resolve<IClass2>();

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<DependencyInjectionException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor, mockNonScopedService);
    }

    [Fact]
    public void ResolveDependency_DependencyObjectIsNull2_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), null!));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);
        //string expected = string.Format(MsgNullDependencyObject, nameof(IClass2));

        //// Act
        //Action action = () => _ = resolver.Resolve<IClass2>();

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<DependencyInjectionException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService);
    }

    [Fact]
    public void ResolveDependency_FactoryThrowsException_ShouldThrowException()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //static object factory() => throw new InvalidOperationException();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);
        //string expected = string.Format(MsgFactoryInvocationError, nameof(IClass2));

        //// Act
        //Action action = () => _ = resolver.Resolve<IClass2>();

        //// Assert
        //action
        //    .Should()
        //    .ThrowExactly<DependencyInjectionException>()
        //    .WithMessage(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveScopedDependency_FactoryMethodIsNotNull1_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass1 expected = new Class1("Factory");
        //object factory() => expected;
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null, false, expected);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Scoped, factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);

        //// Act
        //IClass1 actual = resolver.Resolve<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveScopedDependency_FactoryMethodIsNotNull2_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass1 expected = new Class1("Factory");
        //object factory() => expected;
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass1>(null, false, expected);
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService();
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Scoped, factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass1 actual = resolver.Resolve<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService,
        //            mockDependency);
    }

    [Fact]
    public void ResolveSingletonDependency_FactoryMethodIsNotNull1_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass1 expected = new Class1("Factory");
        //object factory() => expected;
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null, false, expected);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Singleton, factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);

        //// Act
        //IClass1 actual = resolver.Resolve<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveSingletonDependency_FactoryMethodIsNotNull2_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass1 expected = new Class1("Factory");
        //object factory() => expected;
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null, false, expected);
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass1>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Singleton, factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass1 actual = resolver.Resolve<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService,
        //            mockDependency);
    }

    [Fact]
    public void ResolveTransientDependency_FactoryMethodIsNotNull1_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass1 expected = new Class1("Factory");
        //object factory() => expected;
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient, factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);

        //// Act
        //IClass1 actual = resolver.Resolve<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveTransientDependency_FactoryMethodIsNotNull2_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass1 expected = new Class1("Factory");
        //object factory() => expected;
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass1>(null);
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass1>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient, factory: factory);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass1), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass1 actual = resolver.Resolve<IClass1>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeSameAs(expected);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveTransientDependency_SimpleClassTypeWithParameterlessConstructor1_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient, typeof(Class2), checkFactory: true);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);

        //// Act
        //IClass2 actual = resolver.Resolve<IClass2>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeOfType<Class2>();
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveSingletonDependency_SimpleClassTypeWithParameterlessConstructor1_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass2 addedObject = new Class2();
        //IClass2 capturedObject = addedObject;
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null,
        //                                                                                              anyAddedObject: true,
        //                                                                                              capturedObject: capturedObject);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Singleton, typeof(Class2), checkFactory: true);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object);

        //// Act
        //IClass2 actual = resolver.Resolve<IClass2>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeOfType<Class2>()
        //    .And
        //    .BeSameAs(capturedObject)
        //    .And
        //    .NotBeSameAs(addedObject);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            null,
        //            mockDependency);
    }

    [Fact]
    public void ResolveTransientDependency_SimpleClassTypeWithParameterlessConstructor2_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Transient, typeof(Class2), checkFactory: true);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass2 actual = resolver.Resolve<IClass2>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeOfType<Class2>();
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService,
        //            mockDependency);
    }

    [Fact]
    public void ResolveSingletonDependency_SimpleClassTypeWithParameterlessConstructor2_ShouldReturnResolvingObject()
    {
        //// Arrange
        //Mock<IObjectConstructor> mockObjectConstructor = GetMockObjectConstructor();
        //IClass2 addedObject = new Class2();
        //Mock<IResolvingObjectsService> mockNonScopedService = GetMockResolvingObjectsService<IClass2>(null,
        //                                                                                              anyAddedObject: true,
        //                                                                                              capturedObject: addedObject);
        //Mock<IResolvingObjectsService> mockScopedService = GetMockResolvingObjectsService<IClass2>(null);
        //Mock<IDependency> mockDependency = GetMockDependency(DependencyLifetime.Singleton, typeof(Class2), checkFactory: true);
        //Dictionary<Type, IDependency> dependencies = GetDependencies((typeof(IClass2), mockDependency.Object));
        //DependencyResolver resolver = new(dependencies,
        //                                  mockObjectConstructor.Object,
        //                                  mockNonScopedService.Object,
        //                                  mockScopedService.Object);

        //// Act
        //IClass2 actual = resolver.Resolve<IClass2>();

        //// Assert
        //actual
        //    .Should()
        //    .NotBeNull()
        //    .And
        //    .BeOfType<Class2>()
        //    .And
        //    .BeSameAs(addedObject);
        //VerifyMocks(mockObjectConstructor,
        //            mockNonScopedService,
        //            mockScopedService,
        //            mockDependency);
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
                                                       Type? dependencyType = null,
                                                       bool checkFactory = false)
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
        else if (checkFactory)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(null!);
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

    private static Mock<IObjectConstructor> GetMockObjectConstructor()
        => new(MockBehavior.Strict);

    private static Mock<IResolvingObjectsService> GetMockResolvingObjectsService()
        => new(MockBehavior.Strict);

    private static Mock<IResolvingObjectsService> GetMockResolvingObjectsService<T>(T? getObject = null,
                                                                                    bool isFound = false,
                                                                                    T addObject = null!,
                                                                                    bool anyAddedObject = false,
                                                                                    T returnedObject = null!,
                                                                                    T capturedObject = null!) where T : class
    {
        Mock<IResolvingObjectsService> mock = GetMockResolvingObjectsService();

        SetupMockResolvingObjectsService(mock,
                                         getObject,
                                         isFound,
                                         addObject,
                                         anyAddedObject,
                                         returnedObject,
                                         capturedObject);

        return mock;
    }

    private static void SetupMockResolvingObjectsService<T>(Mock<IResolvingObjectsService> mock,
                                                            T? getObject,
                                                            bool isFound = false,
                                                            T addObject = null!,
                                                            bool anyAddedObject = false,
                                                            T returnedObject = null!,
                                                            T capturedObject = null!) where T : class
    {
        //mock
        //    .Setup(m => m.TryGetResolvingObject(out getObject))
        //    .Returns(isFound)
        //    .Verifiable(Times.Once);

        //if (addObject is not null)
        //{
        //    if (returnedObject is not null)
        //    {
        //        mock
        //            .Setup(m => m.Add(addObject))
        //            .Returns(returnedObject)
        //            .Verifiable(Times.Once);
        //    }
        //    else
        //    {
        //        mock
        //            .Setup(m => m.Add(addObject))
        //            .Returns(addObject)
        //            .Verifiable(Times.Once);
        //    }
        //}
        //else if (anyAddedObject)
        //{
        //    if (returnedObject is not null)
        //    {
        //        mock
        //            .Setup(m => m.Add(It.IsAny<T>()))
        //            .Returns(returnedObject)
        //            .Verifiable(Times.Once);
        //    }
        //    else
        //    {
        //        capturedObject
        //            .Should()
        //            .NotBeNull("capturedObject must not be null when anyAddedObject is true and returnedObject is null");
        //        mock
        //            .Setup(m => m.Add(It.IsAny<T>()))
        //            .Callback<T>(obj => capturedObject = obj)
        //            .Returns(() => capturedObject)
        //            .Verifiable(Times.Once);
        //    }
        //}
    }

    private static void VerifyMocks(Mock<IObjectConstructor>? mockObjectConstructor = null,
                                    Mock<IResolvingObjectsService>? mockNonScopedService = null,
                                    Mock<IResolvingObjectsService>? mockScopedService = null,
                                    params Mock<IDependency>[] mockDependencies)
    {
        if (mockObjectConstructor is not null)
        {
            if (mockObjectConstructor.Setups.Any())
            {
                mockObjectConstructor.VerifyAll();
            }

            mockObjectConstructor.VerifyNoOtherCalls();
        }

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