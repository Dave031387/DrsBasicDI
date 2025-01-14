using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DrsBasicDI.Tests")]

namespace DrsBasicDI;

public class DependencyBuilder
{
    private Type? _dependencyType;
    private Func<object>? _factory;
    private DependencyLifetime _lifetime = DependencyLifetime.Undefined;
    private Type? _resolvingType;

    private DependencyBuilder()
    {
    }

    public static DependencyBuilder Empty => new();

    public string DependencyTypeName => _dependencyType is null ? "N/A" : _dependencyType.GetFriendlyName();

    public string ResolvingTypeName => _resolvingType is null ? "N/A" : _resolvingType.GetFriendlyName();

    public Dependency Build()
    {
        Validate();

        return new()
        {
            DependencyType = _dependencyType!,
            Factory = _factory,
            Lifetime = _lifetime,
            ResolvingType = _resolvingType!
        };
    }

    public DependencyBuilder WithDependencyType<T>() where T : class
        => WithDependencyType(typeof(T));

    public DependencyBuilder WithDependencyType(Type dependencyType)
    {
        ArgumentNullException.ThrowIfNull(dependencyType, nameof(dependencyType));

        if (_dependencyType is not null)
        {
            string msg = string.Format(MsgDependencyTypeAlreadySpecified, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        _dependencyType = dependencyType;
        return this;
    }

    public DependencyBuilder WithFactory(Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        if (_factory is not null)
        {
            string msg = string.Format(MsgFactoryAlreadySpecified, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        _factory = factory;
        return this;
    }

    public DependencyBuilder WithLifetime(DependencyLifetime lifetime)
    {
        if (_lifetime is not DependencyLifetime.Undefined)
        {
            string msg = string.Format(MsgLifetimeAlreadySpecified, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        _lifetime = lifetime;
        return this;
    }

    public DependencyBuilder WithResolvingType<T>() where T : class
        => WithResolvingType(typeof(T));

    public DependencyBuilder WithResolvingType(Type resolvingType)
    {
        ArgumentNullException.ThrowIfNull(resolvingType, nameof(resolvingType));

        if (_resolvingType is not null)
        {
            string msg = string.Format(MsgResolvingTypeAlreadySpecified, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        _resolvingType = resolvingType;
        return this;
    }

    private void Validate()
    {
        ValidateDependencyType();
        ValidateResolvingType();
        ValidateFactory();
        ValidateLifetime();
    }

    private void ValidateDependencyType()
    {
        if (_dependencyType is null)
        {
            string msg = MsgUnspecifiedDependencyType;
            throw new DependencyBuildException(msg);
        }

        if (!_dependencyType.IsClass && !_dependencyType.IsInterface)
        {
            string msg = string.Format(MsgInvalidDependencyType, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        if (_dependencyType.IsGenericType && !_dependencyType.IsConstructedGenericType)
        {
            string msg = string.Format(MsgGenericDependencyTypeIsOpen, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }
    }

    private void ValidateFactory()
    {
        if (_factory is not null)
        {
            Type returnType = _factory.Method.ReturnType;

            if (!returnType.IsAssignableTo(_dependencyType))
            {
                string msg = string.Format(MsgIncompatibleFactory, DependencyTypeName);
                throw new DependencyBuildException(msg);
            }
        }
    }

    private void ValidateLifetime()
    {
        if (_lifetime is DependencyLifetime.Undefined)
        {
            string msg = string.Format(MsgUndefinedLifetime, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }
    }

    private void ValidateResolvingType()
    {
        if (_resolvingType is null)
        {
            string msg = string.Format(MsgUnspecifiedResolvingType, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        if (!_resolvingType.IsClass)
        {
            string msg = string.Format(MsgInvalidResolvingType, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        if (_resolvingType.IsAbstract)
        {
            string msg = string.Format(MsgAbstractResolvingType, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        if (_resolvingType.IsGenericType && !_resolvingType.IsConstructedGenericType)
        {
            string msg = string.Format(MsgResolvingGenericTypeIsOpen, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        if (!_resolvingType.IsAssignableTo(_dependencyType))
        {
            string msg = string.Format(MsgIncompatibleResolvingType, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }
    }
}