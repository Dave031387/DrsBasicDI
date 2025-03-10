namespace DrsBasicDI;

/// <summary>
/// The <see cref="DependencyBuilder" /> class is used to construct a valid
/// <see cref="IDependency" /> object.
/// </summary>
public sealed class DependencyBuilder
{
    /// <summary>
    /// The dependency type for the <see cref="IDependency" /> object that is being built.
    /// </summary>
    private Type? _dependencyType;

    /// <summary>
    /// An optional factory method for creating instances of the resolving type for the
    /// <see cref="IDependency" /> object that is being built.
    /// </summary>
    private Func<object>? _factory;

    /// <summary>
    /// The lifetime of the <see cref="IDependency" /> object that is being built.
    /// </summary>
    private DependencyLifetime _lifetime = DependencyLifetime.Undefined;

    /// <summary>
    /// An optional key used to identify the dependency.
    /// </summary>
    private string? _resolvingKey;

    /// <summary>
    /// The type of the resolving object for the <see cref="IDependency" /> object that is being
    /// built.
    /// </summary>
    private Type? _resolvingType;

    /// <summary>
    /// Default constructor for the <see cref="DependencyBuilder" /> class.
    /// </summary>
    /// <remarks>
    /// This constructor is declared <see langword="private" />. Use the static <see cref="Empty" />
    /// property to create a new, empty <see cref="DependencyBuilder" /> object.
    /// </remarks>
    private DependencyBuilder()
    {
    }

    /// <summary>
    /// Gets a new instance of an empty <see cref="DependencyBuilder" /> object.
    /// </summary>
    public static DependencyBuilder Empty => new();

    /// <summary>
    /// Gets the friendly name of the dependency type, or "N/A" if the dependency type hasn't yet
    /// been given.
    /// </summary>
    private string DependencyTypeName => _dependencyType is null ? "N/A" : _dependencyType.GetFriendlyName();

    /// <summary>
    /// Gets the friendly name of the resolving type, or "N/A" if the resolving type hasn't yet been
    /// given.
    /// </summary>
    private string ResolvingTypeName => _resolvingType is null ? "N/A" : _resolvingType.GetFriendlyName();

    /// <summary>
    /// Build the <see cref="IDependency" /> object after verifying that the supplied information is
    /// valid.
    /// </summary>
    /// <returns>
    /// A new <see cref="IDependency" /> object whose properties are set according to the
    /// information that was passed into the <see cref="DependencyBuilder" /> object.
    /// </returns>
    /// <exception cref="DependencyBuildException" />
    internal IDependency Build()
    {
        Validate();

        string resolvingKey = _resolvingKey is null ? EmptyKey : _resolvingKey;
        return new Dependency(_dependencyType!,
                              _resolvingType!,
                              _lifetime,
                              _factory,
                              resolvingKey);
    }

    /// <summary>
    /// Specify the dependency type of the <see cref="IDependency" /> object that is to be built.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type.
    /// </typeparam>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the dependency
    /// type information.
    /// </returns>
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithDependencyType<T>() where T : class
        => WithDependencyType(typeof(T));

    /// <summary>
    /// Specify the dependency type of the <see cref="IDependency" /> object that is to be built.
    /// </summary>
    /// <param name="dependencyType">
    /// The dependency type.
    /// </param>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the dependency
    /// type information.
    /// </returns>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithDependencyType(Type dependencyType)
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

    /// <summary>
    /// Specify the factory delegate that will be used for constructing instances of the resolving
    /// type for the <see cref="IDependency" /> object that is being built.
    /// </summary>
    /// <param name="factory">
    /// A factory delegate that returns an instance of the resolving type for the
    /// <see cref="IDependency" /> object that is being built.
    /// </param>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the factory
    /// delegate information.
    /// </returns>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithFactory(Func<object> factory)
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

    /// <summary>
    /// Specify the lifetime of the <see cref="IDependency" /> object that is being built.
    /// </summary>
    /// <param name="lifetime">
    /// The <see cref="DependencyLifetime" /> enumeration value representing the lifetime of the
    /// <see cref="IDependency" /> object.
    /// </param>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the dependency
    /// lifetime information.
    /// </returns>
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithLifetime(DependencyLifetime lifetime)
    {
        if (lifetime is DependencyLifetime.Undefined)
        {
            string msg = string.Format(MsgUndefinedLifetime, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        if (_lifetime is not DependencyLifetime.Undefined)
        {
            string msg = string.Format(MsgLifetimeAlreadySpecified, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        _lifetime = lifetime;
        return this;
    }

    /// <summary>
    /// Specify the optional key that can be used to identify this <see cref="IDependency" />
    /// object.
    /// </summary>
    /// <param name="key">
    /// A <see langword="string" /> value to be used as the key.
    /// </param>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the resolving
    /// key information.
    /// </returns>
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithResolvingKey(string key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (_resolvingKey is not null)
        {
            string msg = string.Format(MsgResolvingKeyAlreadySpecified, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        _resolvingKey = key;
        return this;
    }

    /// <summary>
    /// Specify the resolving type of the <see cref="IDependency" /> object that is to be built.
    /// </summary>
    /// <typeparam name="T">
    /// The resolving type.
    /// </typeparam>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the resolving
    /// type information.
    /// </returns>
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithResolvingType<T>() where T : class
        => WithResolvingType(typeof(T));

    /// <summary>
    /// Specify the resolving type of the <see cref="IDependency" /> object that is to be built.
    /// </summary>
    /// <param name="resolvingType">
    /// The resolving type.
    /// </param>
    /// <returns>
    /// This <see cref="DependencyBuilder" /> instance after it has been updated with the resolving
    /// type information.
    /// </returns>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="DependencyBuildException" />
    internal DependencyBuilder WithResolvingType(Type resolvingType)
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

    /// <summary>
    /// Validate the state of the <see cref="DependencyBuilder" /> object to determine if the
    /// supplied information constitutes a valid <see cref="IDependency" /> object.
    /// </summary>
    /// <remarks>
    /// An exception is thrown if the supplied information isn't valid.
    /// </remarks>
    /// <exception cref="DependencyBuildException" />
    private void Validate()
    {
        ValidateDependencyType();
        ValidateResolvingType();
        ValidateFactory();
        ValidateLifetime();
    }

    /// <summary>
    /// Verify that the supplied dependency type information is valid.
    /// </summary>
    /// <exception cref="DependencyBuildException" />
    private void ValidateDependencyType()
    {
        // The dependency type must not be null.
        if (_dependencyType is null)
        {
            string msg = MsgUnspecifiedDependencyType;
            throw new DependencyBuildException(msg);
        }

        // The dependency type must be a class type or interface type.
        if (!_dependencyType.IsClass && !_dependencyType.IsInterface)
        {
            string msg = string.Format(MsgInvalidDependencyType, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        // If the dependency type is a generic type, then the generic type must be fully
        // constructed.
        if (_dependencyType.IsGenericType && !_dependencyType.IsConstructedGenericType)
        {
            string msg = string.Format(MsgGenericDependencyTypeIsOpen, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }
    }

    /// <summary>
    /// Verify that the supplied factory delegate information is valid.
    /// </summary>
    /// <exception cref="DependencyBuildException" />
    private void ValidateFactory()
    {
        if (_factory is not null)
        {
            Type returnType = _factory.Method.ReturnType;

            // The return type of the factory delegate must be a type that is assignable to the
            // specified dependency type.
            if (!returnType.IsAssignableTo(_dependencyType))
            {
                string msg = string.Format(MsgIncompatibleFactory, returnType.GetFriendlyName(), DependencyTypeName);
                throw new DependencyBuildException(msg);
            }
        }
    }

    /// <summary>
    /// Verify that the supplied dependency lifetime information is valid.
    /// </summary>
    /// <exception cref="DependencyBuildException" />
    private void ValidateLifetime()
    {
        // The lifetime must not be undefined.
        if (_lifetime is DependencyLifetime.Undefined)
        {
            string msg = string.Format(MsgUndefinedLifetime, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }
    }

    /// <summary>
    /// Verify that the supplied resolving type information is valid.
    /// </summary>
    /// <exception cref="DependencyBuildException" />
    private void ValidateResolvingType()
    {
        // The resolving type must not be null.
        if (_resolvingType is null)
        {
            string msg = string.Format(MsgUnspecifiedResolvingType, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        // The resolving type must be a class type.
        if (!_resolvingType.IsClass)
        {
            string msg = string.Format(MsgInvalidResolvingType, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        // The resolving type must not be an abstract type.
        if (_resolvingType.IsAbstract)
        {
            string msg = string.Format(MsgAbstractResolvingType, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        // If the resolving type is generic, then it must be a fully constructed generic type.
        if (_resolvingType.IsGenericType && !_resolvingType.IsConstructedGenericType)
        {
            string msg = string.Format(MsgResolvingGenericTypeIsOpen, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }

        // The resolving type must be assignable to the specified dependency type.
        if (!_resolvingType.IsAssignableTo(_dependencyType))
        {
            string msg = string.Format(MsgIncompatibleResolvingType, ResolvingTypeName, DependencyTypeName);
            throw new DependencyBuildException(msg);
        }
    }
}