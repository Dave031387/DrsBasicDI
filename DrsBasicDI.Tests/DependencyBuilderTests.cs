namespace DrsBasicDI;

public class DependencyBuilderTests
{
    [Fact]
    public void DependencyLifetimeIsUndefined_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        void buildAction() => builder
            .WithDependencyType(typeof(IClass1))
            .WithResolvingType(typeof(Class1))
            .Build();
        string msg = string.Format(MsgUndefinedLifetime, nameof(IClass1));

        // Act/Assert
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
        Dependency dependency = builder
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
        void action() => builder
            .WithLifetime(DependencyLifetime.Transient)
            .WithResolvingType(typeof(Class1))
            .Build();
        string msg = MsgUnspecifiedDependencyType;

        // Act/Assert
        AssertException(action, msg);
    }

    [Fact]
    public void DependencyTypeIsSubclassOfResolvingType_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        DependencyLifetime lifetime = DependencyLifetime.Transient;

        // Act
        Dependency dependency = builder
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        string msg = string.Format(MsgGenericDependencyTypeIsOpen, dependencyTypeName);

        // Act/Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeIsValueType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type type = typeof(Struct1);
        string typeName = type.Name;
        void buildAction() => builder
            .WithDependencyType(type)
            .WithLifetime(DependencyLifetime.Transient)
            .WithResolvingType(type)
            .Build();
        string msg = string.Format(MsgInvalidDependencyType, typeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithDependencyType(dependencyType)
            .WithLifetime(lifetime)
            .Build();
        string msg = string.Format(MsgDependencyTypeAlreadySpecified, dependencyTypeName);

        // Act/Assert
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
        Dependency dependency = builder
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
        void buildAction() => builder
            .WithResolvingType(resolvingType)
            .WithFactory(factory)
            .WithLifetime(lifetime)
            .WithFactory(factory)
            .WithDependencyType(dependencyType)
            .Build();
        string msg = string.Format(MsgFactoryAlreadySpecified, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithLifetime(lifetime)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .Build();
        string msg = string.Format(MsgLifetimeAlreadySpecified, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        string msg = string.Format(MsgIncompatibleResolvingType, resolvingTypeName, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        string msg = string.Format(MsgIncompatibleResolvingType, resolvingTypeName, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        string msg = string.Format(MsgAbstractResolvingType, resolvingTypeName, dependencyTypeName);

        // Act/Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsInterfaceType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type type = typeof(IClass1);
        string typeName = type.Name;
        void buildAction() => builder
            .WithDependencyType(type)
            .WithResolvingType(type)
            .WithLifetime(DependencyLifetime.Scoped)
            .Build();
        string msg = string.Format(MsgInvalidResolvingType, typeName, typeName);

        // Act/Assert
        AssertException(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.Empty;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();
        string msg = string.Format(MsgUnspecifiedResolvingType, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        string msg = string.Format(MsgResolvingGenericTypeIsOpen, resolvingTypeName, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        string msg = string.Format(MsgInvalidResolvingType, resolvingTypeName, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .WithResolvingType(resolvingType)
            .Build();
        string msg = string.Format(MsgResolvingTypeAlreadySpecified, dependencyTypeName);

        // Act/Assert
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
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithFactory(factory)
            .WithLifetime(DependencyLifetime.Transient)
            .Build();
        string msg = string.Format(MsgIncompatibleFactory, dependencyTypeName);

        // Act/Assert
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
        Dependency dependency = builder
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
        Dependency dependency = builder
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
        // Act/Assert
        action
            .Should()
            .ThrowExactly<DependencyBuildException>()
            .WithMessage(msg);
    }
}