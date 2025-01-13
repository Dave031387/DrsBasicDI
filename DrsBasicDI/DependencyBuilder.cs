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
    {
        _dependencyType = typeof(T);
        return this;
    }

    public DependencyBuilder WithDependencyType(Type dependencyType)
    {
        ArgumentNullException.ThrowIfNull(dependencyType, nameof(dependencyType));
        _dependencyType = dependencyType;
        return this;
    }

    public DependencyBuilder WithFactory(Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));
        _factory = factory;
        return this;
    }

    public DependencyBuilder WithLifetime(DependencyLifetime lifetime)
    {
        _lifetime = lifetime;
        return this;
    }

    public DependencyBuilder WithResolvingType<T>() where T : class
    {
        _resolvingType = typeof(T);
        return this;
    }

    public DependencyBuilder WithResolvingType(Type resolvingType)
    {
        ArgumentNullException.ThrowIfNull(resolvingType, nameof(resolvingType));
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
            string msg = string.Format(MsgInvalidDependencyType, _dependencyType.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }

        if (_dependencyType.IsGenericType && !_dependencyType.IsConstructedGenericType)
        {
            string msg = string.Format(MsgGenericDependencyTypeIsOpen, _dependencyType.GetFriendlyName());
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
                string msg = string.Format(MsgIncompatibleFactory, _dependencyType!.GetFriendlyName());
                throw new DependencyBuildException(msg);
            }
        }
    }

    private void ValidateLifetime()
    {
        if (_lifetime is DependencyLifetime.Undefined)
        {
            string msg = string.Format(MsgUndefinedLifetime, _dependencyType!.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }
    }

    private void ValidateResolvingType()
    {
        if (_resolvingType is null)
        {
            string msg = string.Format(MsgUnspecifiedResolvingType, _dependencyType!.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }

        if (!_resolvingType.IsClass)
        {
            string msg = string.Format(MsgInvalidResolvingType, _resolvingType.GetFriendlyName(), _dependencyType!.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }

        if (_resolvingType.IsAbstract)
        {
            string msg = string.Format(MsgAbstractResolvingType, _resolvingType.GetFriendlyName(), _dependencyType!.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }

        if (_resolvingType.IsGenericType && !_resolvingType.IsConstructedGenericType)
        {
            string msg = string.Format(MsgResolvingGenericTypeIsOpen, _resolvingType.GetFriendlyName(), _dependencyType!.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }

        if (!_resolvingType.IsAssignableTo(_dependencyType))
        {
            string msg = string.Format(MsgIncompatibleResolvingType, _resolvingType.GetFriendlyName(), _dependencyType!.GetFriendlyName());
            throw new DependencyBuildException(msg);
        }
    }
}