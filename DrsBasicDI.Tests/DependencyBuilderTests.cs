namespace DrsBasicDI;

public class DependencyBuilderTests
{
    [Fact]
    public void DependencyLifetimeIsUndefined_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        Type resolvingType = typeof(Class1);
        string msg = string.Format(MsgUndefinedLifetime, nameof(IClass1));

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyLifetimeSetToUndefined_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        Type resolvingType = typeof(Class1);
        string msg = string.Format(MsgUndefinedLifetime, nameof(IClass1));

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(DependencyLifetime.Undefined)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeAndResolvingTypeAreSame_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        dependency.Key
            .Should()
            .Be(EmptyKey);
    }

    [Fact]
    public void DependencyTypeIsNotSpecified_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type resolvingType = typeof(Class1);
        string msg = MsgUnspecifiedDependencyType;

        // Act
        void buildAction() => builder
            .WithLifetime(DependencyLifetime.Transient)
            .WithResolvingType(resolvingType)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type resolvingType = typeof(Class1);
        string msg = MsgNullDependencyType;

        // Act
        void buildAction() => builder
            .WithDependencyType(null!)
            .WithLifetime(DependencyLifetime.Transient)
            .WithResolvingType(resolvingType)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeIsSubclassOfResolvingType_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        DependencyLifetime lifetime = DependencyLifetime.Transient;
        string resolvingKey = "test";

        // Act
        IDependency dependency = builder
            .WithDependencyType<Class1>()
            .WithResolvingType<Class1A>()
            .WithLifetime(lifetime)
            .WithResolvingKey(resolvingKey)
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
        dependency.Key
            .Should()
            .Be(resolvingKey);
    }

    [Fact]
    public void DependencyTypeIsUnboundGenericType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeIsValueType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyTypeSpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void DependencyWithValidFactory_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        dependency.Key
            .Should()
            .Be(EmptyKey);
        ((Class1A)dependency.Factory!()).BuiltBy
            .Should()
            .Be(builtBy);
    }

    [Fact]
    public void FactoryIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Scoped;
        string msg = string.Format(MsgNullFactory, nameof(IClass1));

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithFactory(null!)
            .WithLifetime(lifetime)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void FactorySpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void LifetimeSpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingGenericClassTypeNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingKeyIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Transient;
        string msg = string.Format(MsgNullResolvingKey, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .WithResolvingKey(null!)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingKeySpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        Type resolvingType = typeof(Class1);
        DependencyLifetime lifetime = DependencyLifetime.Transient;
        string resolvingKey = "test";
        string msg = string.Format(MsgResolvingKeyAlreadySpecified, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingKey(resolvingKey)
            .WithResolvingType(resolvingType)
            .WithLifetime(lifetime)
            .WithResolvingKey(resolvingKey)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingNonGenericClassTypeNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsAbstractType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsInterfaceType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsNotSpecified_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        string msg = string.Format(MsgUnspecifiedResolvingType, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsNull_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IClass1);
        string dependencyTypeName = dependencyType.Name;
        string msg = string.Format(MsgNullResolvingType, dependencyTypeName);

        // Act
        void buildAction() => builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(null!)
            .WithLifetime(DependencyLifetime.Singleton)
            .Build();

        // Assert
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsUnboundGenericType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeIsValueType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void ResolvingTypeSpecifiedMoreThanOnce_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Fact]
    public void TypeReturnedFromFactoryNotAssignableToDependencyType_ShouldThrowException()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
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
        TestHelper.AssertException<DependencyBuildException>(buildAction, msg);
    }

    [Theory]
    [InlineData(DependencyLifetime.Transient)]
    [InlineData(DependencyLifetime.Singleton)]
    [InlineData(DependencyLifetime.Scoped)]
    public void ValidDependencyAndResolvingTypes_ShouldAllowAnyValidLifetime(DependencyLifetime lifetime)
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;

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
        dependency.Key
            .Should()
            .Be(EmptyKey);
    }

    [Fact]
    public void ValidGenericDependencyAndResolvingTypes_ShouldBuildValidDependency()
    {
        // Arrange
        DependencyBuilder builder = DependencyBuilder.CreateNew;
        Type dependencyType = typeof(IGenericClass1<int, string>);
        Type resolvingType = typeof(GenericClass1<int, string>);
        DependencyLifetime lifetime = DependencyLifetime.Singleton;
        string resolvingKey = "test";

        // Act
        IDependency dependency = builder
            .WithDependencyType(dependencyType)
            .WithResolvingType(resolvingType)
            .WithResolvingKey(resolvingKey)
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
        dependency.Key
            .Should()
            .Be(resolvingKey);
    }
}