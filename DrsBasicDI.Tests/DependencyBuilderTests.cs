namespace DrsBasicDI;

public class DependencyBuilderTests
{
    [Fact]
    public void DependencyLifetimeIsUndefined_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        Type resolvingType = typeof(Class1);
        string msg = string.Format(MsgUndefinedLifetime, nameof(IClass1));

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeAndResolvingTypeAreSame_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type type = typeof(Class2);
        DependencyLifetime lifetime = DependencyLifetime.Scoped;

        // Act
        IDependency dependency = builder
            .WithDependencyType(type)
            .WithResolvingType(type)
            .WithLifetime(lifetime)
            .Build();

        // Assert
        dependency.DependencyType
            .Should()
            .Be(type);
        dependency.ResolvingType
            .Should()
            .Be(type);
        dependency.Lifetime
            .Should()
            .Be(lifetime);
        dependency.Factory
            .Should()
            .BeNull();
    }

    [Fact]
    public void DependencyTypeIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type resolvingType = typeof(Class1);
        string msg = MsgUnspecifiedDependencyType;

        // Act
        void action() => builder
            .WithLifetime(DependencyLifetime.Transient)
            .WithResolvingType(resolvingType)
            .Build();

        // Assert
        AssertException(action, msg);
    }

    [Fact]
    public void DependencyTypeIsSubclassOfResolvingType_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        DependencyLifetime lifetime = DependencyLifetime.Transient;

        // Act
        IDependency dependency = builder
            .WithDependencyType<Class1>()
            .WithResolvingType<Class1A>()
            .WithLifetime(lifetime)
            .Build();

        // Assert
        dependency.DependencyType
            .Should()
            .Be<Class1>();
        dependency.ResolvingType
            .Should()
            .Be<Class1A>();
        dependency.Lifetime
            .Should()
            .Be(lifetime);
        dependency.Factory
            .Should()
            .BeNull();
    }

    [Fact]
    public void DependencyTypeIsUnboundGenericType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IGenericClass1<,>);
        string dependencyTypeName = "IGenericClass1<S, T>";
        Type resolvingType = typeof(GenericClass1<int, string>);
        string msg = string.Format(MsgGenericDependencyTypeIsOpen, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeIsValueType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type type = typeof(Struct1);
        string typeName = type.Name;
        string msg = string.Format(MsgInvalidDependencyType, typeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(type)
            .WithLifetime(DependencyLifetime.Transient)
            .WithResolvingType(type)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeSpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Singleton;
        string msg = string.Format(MsgDependencyTypeAlreadySpecified, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithDependencyType(dependencyType)
            .WithLifetime(lifetime)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void DependencyWithValidFactory_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        DependencyLifetime lifetime = DependencyLifetime.Singleton;
        string builtBy = "Factory";
        Class1A factory() => new()
        {
            BuiltBy = builtBy
        };

        // Act
        IDependency dependency = builder
            .WithDependencyType<IClass1>()
            .WithResolvingType<Class1A>()
            .WithLifetime(lifetime)
            .WithFactory(factory)
            .Build();

        // Assert
        dependency.DependencyType
            .Should()
            .Be<IClass1>();
        dependency.ResolvingType
            .Should()
            .Be<Class1A>();
        dependency.Lifetime
            .Should()
            .Be(lifetime);
        dependency.Factory
            .Should()
            .NotBeNull();
        ((Class1A)dependency.Factory!()).BuiltBy
            .Should()
            .Be(builtBy);
    }

    [Fact]
    public void FactorySpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = "N/A";
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Scoped;
        static Class1 factory() => new();
        string msg = string.Format(MsgFactoryAlreadySpecified, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithResolvingType(resolvingType)
            .WithFactory(factory)
            .WithLifetime(lifetime)
            .WithFactory(factory)
            .WithDependencyType(dependencyType)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void LifetimeSpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Transient;
        string msg = string.Format(MsgLifetimeAlreadySpecified, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithLifetime(lifetime)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingGenericClassTypeNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IGenericClass1<int, string>);
        string dependencyTypeName = "IGenericClass1<int, string>";
        Type resolvingType = typeof(GenericClass1<string, int>);
        string resolvingTypeName = "GenericClass1<string, int>";
        string msg = string.Format(MsgIncompatibleResolvingType, resolvingTypeName, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingNonGenericClassTypeNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class2);
        string resolvingTypeName = resolvingType.Name;
        string msg = string.Format(MsgIncompatibleResolvingType, resolvingTypeName, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsAbstractType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(AbstractClass1);
        string resolvingTypeName = resolvingType.Name;
        string msg = string.Format(MsgAbstractResolvingType, resolvingTypeName, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsInterfaceType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type type = typeof(IClass1);
        string typeName = type.Name;
        string msg = string.Format(MsgInvalidResolvingType, typeName, typeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(type)
            .WithResolvingType(type)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        string msg = string.Format(MsgUnspecifiedResolvingType, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsUnboundGenericType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IGenericClass1<int, string>);
        string dependencyTypeName = "IGenericClass1<int, string>";
        Type resolvingType = typeof(GenericClass1<,>);
        string resolvingTypeName = "GenericClass1<S, T>";
        string msg = string.Format(MsgResolvingGenericTypeIsOpen, resolvingTypeName, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsValueType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IStruct1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Struct1);
        string resolvingTypeName = resolvingType.Name;
        string msg = string.Format(MsgInvalidResolvingType, resolvingTypeName, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeSpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Transient;
        string msg = string.Format(MsgResolvingTypeAlreadySpecified, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .WithResolvingType(resolvingType)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void TypeReturnedFromFactoryNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class1);
        static Class2 factory() => new();
        string msg = string.Format(MsgIncompatibleFactory, nameof(Class2), dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithFactory(factory)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();

        // Assert
        AssertException(buildAction, msg);
    }

    [Theory]
    [InlineData(DependencyLifetime.Transient)]
    [InlineData(DependencyLifetime.Singleton)]
    [InlineData(DependencyLifetime.Scoped)]
    public void ValidDependencyAndResolvingTypes_ShouldAllowAnyValidLifetime(DependencyLifetime lifetime)
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;

        // Act
        IDependency dependency = builder
            .WithDependencyType<IClass1>()
            .WithResolvingType<Class1>()
            .WithLifetime(lifetime)
            .Build();

        // Assert
        dependency.DependencyType
            .Should()
            .Be<IClass1>();
        dependency.ResolvingType
            .Should()
            .Be<Class1>();
        dependency.Lifetime
            .Should()
            .Be(lifetime);
        dependency.Factory
            .Should()
            .BeNull();
    }

    [Fact]
    public void ValidGenericDependencyAndResolvingTypes_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IGenericClass1<int, string>);
        Type resolvingType = typeof(GenericClass1<int, string>);
        DependencyLifetime lifetime = DependencyLifetime.Singleton;

        // Act
        IDependency dependency = builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .Build();

        // Assert
        dependency.DependencyType
            .Should()
            .Be(dependencyType);
        dependency.ResolvingType
            .Should()
            .Be(resolvingType);
        dependency.Lifetime
            .Should()
            .Be(lifetime);
        dependency.Factory
            .Should()
            .BeNull();
    }

    private static void AssertException(Action action, string msg)
    {
        // Assert
        action
            .Should()
            .ThrowExactly<DependencyBuildException>()
            .WithMessage(msg);
    }
}