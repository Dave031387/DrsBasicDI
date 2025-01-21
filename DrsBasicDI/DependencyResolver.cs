namespace DrsBasicDI;

using System.Reflection;

internal class DependencyResolver
{
    /// <summary>
    /// A dictionary of <see cref="Dependency" /> objects whose keys are the corresponding
    /// dependency type specified by the <see cref="Dependency.DependencyType" /> property.
    /// </summary>
    private readonly Dictionary<Type, Dependency> _dependencies;

    /// <summary>
    /// This <see cref="ResolvedDependencies" /> instance contains all the non-scoped resolved
    /// dependencies.
    /// </summary>
    private readonly ResolvedDependencies _nonscoped;

    /// <summary>
    /// Save the <see cref="MethodInfo" /> details for the <see cref="Resolve{T}()" /> method so
    /// that we can dynamically invoke the method for different generic types.
    /// </summary>
    private readonly MethodInfo _resolveMethodInfo;

    /// <summary>
    /// This <see cref="ResolvedDependencies" /> instance contains all the scoped resolved
    /// dependencies for a specific dependency scope.
    /// </summary>
    private readonly ResolvedDependencies? _scoped;

    /// <summary>
    /// Create an instance of the <see cref="DependencyResolver" /> class.
    /// </summary>
    /// <param name="dependencies">
    /// A dictionary of dependency type-to-resolving type mappings.
    /// </param>
    /// <param name="nonscoped">
    /// A <see cref="ResolvedDependencies" /> object containing all of the resolved non-scoped
    /// dependency objects.
    /// </param>
    /// <param name="scoped">
    /// A <see cref="ResolvedDependencies" /> object containing all of the resolved scoped
    /// dependency objects.
    /// <para>
    /// This parameter is optional when only non-scoped dependencies are being resolved.
    /// </para>
    /// </param>
    internal DependencyResolver(Dictionary<Type, Dependency> dependencies,
                                ResolvedDependencies nonscoped,
                                ResolvedDependencies? scoped = null)
    {
        _dependencies = dependencies;
        _nonscoped = nonscoped;
        _scoped = scoped;
        _resolveMethodInfo = typeof(DependencyResolver).GetMethod(nameof(Resolve), BindingFlags.Public | BindingFlags.Instance)
            ?? throw new DependencyInjectionException(MsgResolverMethodInfoNotFound);
    }

    /// <summary>
    /// Retrieve the resolving object for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is to be resolved.
    /// </typeparam>
    /// <returns>
    /// An instance of the resolving class type.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    internal T Resolve<T>() where T : class
    {
        if (TryGetResolvedDependency(out T? resolvedDependency))
        {
            // The only way we will get to this point is if the resolvedDependency value is not
            // null.
            return resolvedDependency!;
        }

        if (TryGetFactoryValue(out T? factoryValue))
        {
            // The only way we will get to this point is if the factoryValue is not null.
            return factoryValue!;
        }

        return GetResolvingInstance<T>();
    }

    /// <summary>
    /// Construct the resolving instance for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type being resolved.
    /// </typeparam>
    /// <param name="constructorInfo">
    /// The constructor info for the resolving instance.
    /// </param>
    /// <param name="parameterValues">
    /// The constructor parameter values for the resolving instance.
    /// </param>
    /// <returns>
    /// An instance of the resolving object for the given dependency type <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    private T ConstructResolvingInstance<T>(ConstructorInfo constructorInfo, object[] parameterValues) where T : class
    {
        string typeName = typeof(T).GetFriendlyName();
        T resolvingObject;

        try
        {
            if (constructorInfo.Invoke(parameterValues) is not T resolvingInstance)
            {
                string msg = string.Format(MsgResolvingObjectNotCreated, typeName);
                throw new DependencyInjectionException(msg);
            }

            resolvingObject = resolvingInstance;
        }
        catch (Exception ex)
        {
            string msg = string.Format(MsgErrorDuringConstruction, typeName);
            throw new DependencyInjectionException(msg, ex);
        }

        resolvingObject = SaveResolvedDependency(resolvingObject);
        return resolvingObject;
    }

    /// <summary>
    /// Gets the <see cref="Dependency" /> object for the given dependency type.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type that is being resolved.
    /// </typeparam>
    /// <returns>
    /// The <see cref="Dependency" /> object corresponding to the given dependency type
    /// <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    private Dependency GetDependency<T>() where T : class
    {
        Type dependencyType = typeof(T);

        if (_dependencies.TryGetValue(dependencyType, out Dependency? dependency))
        {
            if (dependency is null)
            {
                string msg = string.Format(MsgNullDependencyObject, dependencyType.GetFriendlyName());
                throw new DependencyInjectionException(msg);
            }

            return dependency;
        }
        else
        {
            string msg = string.Format(MsgDependencyMappingNotFound, dependencyType.GetFriendlyName());
            throw new DependencyInjectionException(msg);
        }
    }

    /// <summary>
    /// Gets an instance of the resolving object for the given dependency type
    /// <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type being resolved.
    /// </typeparam>
    /// <returns>
    /// An instance of the resolving object for the given dependency type <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    private T GetResolvingInstance<T>() where T : class
    {
        Type dependencyType = typeof(T);
        ConstructorInfo constructorInfo = dependencyType.GetConstructorInfo();
        object[] resolvingObjects = ResolveNestedDependencies(constructorInfo);
        return ConstructResolvingInstance<T>(constructorInfo, resolvingObjects);
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
        List<object> resolvingObjects = [];

        foreach (ParameterInfo parameter in parameters)
        {
            Type parameterType = parameter.ParameterType;
            string parameterTypeName = parameterType.GetFriendlyName();
            MethodInfo resolveMethodInfo;

            try
            {
                resolveMethodInfo = _resolveMethodInfo.MakeGenericMethod(parameterType);
            }
            catch (Exception ex)
            {
                string msg = string.Format(MsgUnableToMakeGenericResolveMethod, parameterTypeName);
                throw new DependencyInjectionException(msg, ex);
            }

            object? resolvingObject;

            try
            {
                resolvingObject = resolveMethodInfo.Invoke(this, []);
            }
            catch (Exception ex)
            {
                string msg = string.Format(MsgResolveMethodInvocationError, parameterTypeName);
                throw new DependencyInjectionException(msg, ex);
            }

            if (resolvingObject is null)
            {
                string msg = string.Format(MsgResolvingObjectNotCreated, parameterTypeName);
                throw new DependencyInjectionException(msg);
            }

            resolvingObjects.Add(resolvingObject);
        }

        return [.. resolvingObjects];
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
    /// <returns>
    /// The <paramref name="resolvedDependency" /> that was passed in, or the resolving instance
    /// that was saved by another application thread.
    /// </returns>
    /// <remarks>
    /// Only scoped and singleton dependencies are saved. Transient dependencies by definition are
    /// created new every time they're requested.
    /// </remarks>
    private T SaveResolvedDependency<T>(T resolvedDependency) where T : class
    {
        Dependency dependency = GetDependency<T>();

        if (dependency.Lifetime is DependencyLifetime.Scoped && _scoped is not null)
        {
            return _scoped.Add(resolvedDependency);
        }
        else if (dependency.Lifetime is DependencyLifetime.Scoped or DependencyLifetime.Singleton)
        {
            return _nonscoped.Add(resolvedDependency);
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
    /// <returns>
    /// <see langword="true" /> if a valid resolving object is returned from the
    /// <see cref="Dependency.Factory" /> method. Otherwise, returns <see langword="false" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    private bool TryGetFactoryValue<T>(out T? factoryValue) where T : class
    {
        Dependency dependency = GetDependency<T>();

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
                factoryValue = SaveResolvedDependency(factoryValue);
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
    /// <returns>
    /// <see langword="true" /> if a valid resolving object is successfully retrieved. Otherwise,
    /// <see langword="false" />.
    /// </returns>
    private bool TryGetResolvedDependency<T>(out T? resolvedDependency) where T : class
    {
        if (_scoped is not null)
        {
            if (_scoped.TryGetResolvedDependency(out resolvedDependency))
            {
                if (resolvedDependency is not null)
                {
                    return true;
                }
            }
        }
        else if (_nonscoped.TryGetResolvedDependency(out resolvedDependency))
        {
            if (resolvedDependency is not null)
            {
                return true;
            }
        }

        return false;
    }
}