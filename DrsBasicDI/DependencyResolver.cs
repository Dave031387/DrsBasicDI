namespace DrsBasicDI;

using System.Reflection;

/// <summary>
/// The <see cref="DependencyResolver" /> class is responsible for creating an instance of the
/// resolving object for a given dependency type.
/// </summary>
internal sealed class DependencyResolver : IDependencyResolver
{
    /// <summary>
    /// A lock object used to ensure thread safety when accessing or saving resolved dependencies.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Save the <see cref="MethodInfo" /> details for the <see cref="Resolve{T}(string)" /> method
    /// so that we can dynamically invoke the method for different generic types.
    /// </summary>
    private readonly MethodInfo _resolveMethodInfo = typeof(DependencyResolver).GetMethod(nameof(RecursiveResolve), BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new DependencyInjectionException(MsgResolveMethodInfoNotFound);

    /// <summary>
    /// Create a new instance of the <see cref="DependencyResolver" /> class.
    /// </summary>
    public DependencyResolver() : this(ServiceLocater.Instance)
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="serviceLocater">
    /// A service locater object that should provide mock instances of the requested dependencies.
    /// </param>
    internal DependencyResolver(IServiceLocater serviceLocater)
    {
        DependencyList = serviceLocater.Get<IDependencyListConsumer>();
        ObjectConstructor = serviceLocater.Get<IObjectConstructor>();
        NonscopedResolver = serviceLocater.Get<IResolvingObjectsService>(NonScoped);
    }

    /// <summary>
    /// Get a reference to the <see cref="IDependencyListConsumer" /> object.
    /// </summary>
    private IDependencyListConsumer DependencyList
    {
        get;
    }

    /// <summary>
    /// Get a reference to the <see cref="IResolvingObjectsService" /> instance used for managing
    /// non-scoped dependency objects.
    /// </summary>
    private IResolvingObjectsService NonscopedResolver
    {
        get;
    }

    /// <summary>
    /// Get a reference to the <see cref="IObjectConstructor" /> object.
    /// </summary>
    private IObjectConstructor ObjectConstructor
    {
        get;
    }

    /// <summary>
    /// Get a reference to the <see cref="IResolvingObjectsService" /> instance used for managing
    /// scoped dependency objects.
    /// </summary>
    private IResolvingObjectsService? ScopedResolver
    {
        get;
        set;
    }

    /// <summary>
    /// Call the <see cref="ResolvingObjectsService.Dispose()" /> method on the
    /// <see cref="NonscopedResolver" /> instance to dispose of any resources that may be owned by
    /// singleton or globally-scoped dependency objects that were created by the dependency
    /// injection container.
    /// </summary>
    public void Dispose() => NonscopedResolver.Dispose();

    /// <summary>
    /// Retrieve the resolving object for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// An instance of the resolving class type.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public T Resolve<T>(string key) where T : class
    {
        lock (_lock)
        {
            if (NonscopedResolver is null)
            {
                throw new DependencyInjectionException(MsgResolvingObjectServiceNotFound);
            }

            return RecursiveResolve<T>(key);
        }
    }

    /// <summary>
    /// Set the scoped <see cref="IResolvingObjectsService" /> to the specified
    /// <paramref name="resolvingObjectsService" /> instance.
    /// </summary>
    /// <param name="resolvingObjectsService">
    /// The scoped <see cref="IResolvingObjectsService" /> instance to be set.
    /// </param>
    public void SetScopedResolver(IResolvingObjectsService resolvingObjectsService) => ScopedResolver = resolvingObjectsService;

    /// <summary>
    /// Construct the resolving object for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type whose resolving object is being constructed.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be constructed.
    /// </param>
    /// <returns>
    /// An instance of the resolving object for the given dependency type <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    private T ConstructResolvingInstance<T>(string key) where T : class
    {
        IDependency dependency = DependencyList.Get<T>(key);
        Type resolvingType = dependency.ResolvingType;
        ConstructorInfo constructorInfo = resolvingType.GetPrimaryConstructorInfo();
        object[] resolvedParameters = ResolveNestedDependencies(constructorInfo);
        return SaveResolvedDependency(ObjectConstructor.Construct<T>(constructorInfo, resolvedParameters), key);
    }

    /// <summary>
    /// Retrieve the resolving object for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// An instance of the resolving class type.
    /// </returns>
    /// <remarks>
    /// This method will be called recursively until all nested dependency types have been resolved.
    /// </remarks>
    /// <exception cref="DependencyInjectionException" />
    private T RecursiveResolve<T>(string key) where T : class
    {
        if (TryGetResolvedDependency(out T? resolvedDependency, key))
        {
            // The only way we will get to this point is if the resolvedDependency value is not
            // null.
            return resolvedDependency!;
        }

        if (TryGetFactoryValue(out T? factoryValue, key))
        {
            // The only way we will get to this point is if the factoryValue is not null.
            return factoryValue!;
        }

        // If we get here the dependency hasn't been resolved yet, so go resolve it.
        return ConstructResolvingInstance<T>(key);
    }

    /// <summary>
    /// Gets the resolving objects corresponding to each of the parameters in the constructor passed
    /// in through the <paramref name="constructorInfo" /> parameter.
    /// </summary>
    /// <param name="constructorInfo">
    /// The <see cref="ConstructorInfo" /> of a resolving object for some dependency type.
    /// </param>
    /// <returns>
    /// An array of resolving objects corresponding to the parameters of the given
    /// <paramref name="constructorInfo" />.
    /// </returns>
    /// <remarks>
    /// This method makes recursive calls to <see cref="Resolve{T}" /> until all nested dependencies
    /// have been resolved.
    /// </remarks>
    /// <exception cref="DependencyInjectionException" />
    private object[] ResolveNestedDependencies(ConstructorInfo constructorInfo)
    {
        ParameterInfo[] parameters = constructorInfo.GetParameters();
        List<object> resolvedParameters = [];

        foreach (ParameterInfo parameter in parameters)
        {
            ResolvingKeyAttribute? attribute = parameter.GetCustomAttribute<ResolvingKeyAttribute>();
            string resolvingKey = attribute?.Key ?? EmptyKey;
            Type parameterType = parameter.ParameterType;
            string parameterTypeName = parameterType.GetFriendlyName();
            MethodInfo resolveMethodInfo;

            try
            {
                // Create a generic version of the RecursiveResolve<T>() method using the current
                // parameter type as the generic type parameter T.
                resolveMethodInfo = _resolveMethodInfo.MakeGenericMethod(parameterType);
            }
            catch (Exception ex)
            {
                string msg = string.Format(MsgUnableToMakeGenericResolveMethod, parameterTypeName);
                throw new DependencyInjectionException(msg, ex);
            }

            object? resolvedParameter;

            try
            {
                // Invoke the generic RecursiveResolve<T>() method for the current parameter type.
                resolvedParameter = resolveMethodInfo.Invoke(this, [resolvingKey]);
            }
            catch (Exception ex)
            {
                string msg = string.Format(MsgResolveMethodInvocationError, parameterTypeName);
                throw new DependencyInjectionException(msg, ex);
            }

            // We should not get a null value returned from the generic RecursiveResolve<T>()
            // method.
            if (resolvedParameter is null)
            {
                string msg = string.Format(MsgResolvingObjectNotCreated, parameterTypeName);
                throw new DependencyInjectionException(msg);
            }

            resolvedParameters.Add(resolvedParameter);
        }

        return [.. resolvedParameters];
    }

    /// <summary>
    /// Save the given resolved dependency object for later use if applicable.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the dependency that was resolved.
    /// </typeparam>
    /// <param name="resolvedDependency">
    /// An instance of the resolving type that was mapped to the dependency type
    /// <typeparamref name="T" />.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be saved.
    /// </param>
    /// <returns>
    /// The <paramref name="resolvedDependency" /> that was passed in, or the resolving instance
    /// that was previously saved by another application thread.
    /// </returns>
    /// <remarks>
    /// Only scoped and singleton dependencies are saved. Transient dependencies by definition are
    /// created new every time they're requested.
    /// </remarks>
    private T SaveResolvedDependency<T>(T resolvedDependency, string key) where T : class
    {
        IDependency dependency = DependencyList.Get<T>(key);

        if (dependency.Lifetime is DependencyLifetime.Scoped && ScopedResolver is not null)
        {
            return ScopedResolver.Add(resolvedDependency, key);
        }
        else if (dependency.Lifetime is DependencyLifetime.Scoped or DependencyLifetime.Singleton)
        {
            return NonscopedResolver!.Add(resolvedDependency, key);
        }

        return resolvedDependency;
    }

    /// <summary>
    /// Retrieve the resolving object from the factory method if one was defined for the given
    /// <see cref="Dependency" /> object.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the dependency that is being resolved.
    /// </typeparam>
    /// <param name="factoryValue">
    /// The resolving object that is returned from the <see cref="Dependency.Factory" /> method for
    /// the dependency type <typeparamref name="T" /> that is being resolved.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if a valid resolving object is returned from the
    /// <see cref="Dependency.Factory" /> method. Otherwise, returns <see langword="false" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    private bool TryGetFactoryValue<T>(out T? factoryValue, string key) where T : class
    {
        IDependency dependency = DependencyList.Get<T>(key);

        if (dependency.Factory is not null)
        {
            try
            {
                factoryValue = (T?)dependency.Factory();
            }
            catch (Exception ex)
            {
                string msg = string.Format(MsgFactoryInvocationError, typeof(T).GetFriendlyName());
                throw new DependencyInjectionException(msg, ex);
            }

            if (factoryValue is not null)
            {
                factoryValue = SaveResolvedDependency(factoryValue, key);
                return true;
            }
        }

        factoryValue = default;
        return false;
    }

    /// <summary>
    /// Try to retrieve the resolving object for the given dependency type if one was previously
    /// saved.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the dependency that is being resolved.
    /// </typeparam>
    /// <param name="resolvedDependency">
    /// The resolving object that is returned for the given dependency type
    /// <typeparamref name="T" /> if one was previously saved. Otherwise, <see langword="null" />.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if a valid resolving object is successfully retrieved. Otherwise,
    /// <see langword="false" />.
    /// </returns>
    private bool TryGetResolvedDependency<T>(out T? resolvedDependency, string key) where T : class
    {
        if (ScopedResolver is not null)
        {
            if (ScopedResolver.TryGetResolvingObject(out resolvedDependency, key))
            {
                return true;
            }

            IDependency dependency = DependencyList.Get<T>(key);

            if (dependency.Lifetime is DependencyLifetime.Scoped)
            {
                return false;
            }
        }

        if (NonscopedResolver!.TryGetResolvingObject(out resolvedDependency, key))
        {
            return true;
        }

        return false;
    }
}