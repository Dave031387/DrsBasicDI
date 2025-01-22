namespace DrsBasicDI;

/// <summary>
/// The <see cref="ResolvedDependencies" /> class manages a dictionary of
/// </summary>
internal sealed class ResolvedDependencies
{
    /// <summary>
    /// A lock object used to ensure thread safety when accessing/modifying the
    /// <see cref="_resolvedDependencies" /> field.
    /// </summary>
    internal readonly object _lock = new();

    /// <summary>
    /// A dictionary of resolved dependencies whose keys are the dependency types and whose values
    /// are an instance of the resolving type.
    /// </summary>
    internal readonly Dictionary<Type, object> _resolvedDependencies = [];

    /// <summary>
    /// Create an instance of the <see cref="ResolvedDependencies" /> class.
    /// </summary>
    internal ResolvedDependencies()
    {
    }

    /// <summary>
    /// Add the given <paramref name="resolvingObject" /> to the list of resolving objects if no
    /// object currently exists for the given dependency type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type being resolved.
    /// </typeparam>
    /// <param name="resolvingObject">
    /// The resolving object to be added for the given dependency type <typeparamref name="T" />.
    /// </param>
    /// <returns>
    /// The <paramref name="resolvingObject" /> or the object retrieved from the list of resolving
    /// objects if one already exists for the given dependency type <typeparamref name="T" />.
    /// </returns>
    internal T Add<T>(T resolvingObject) where T : class
    {
        lock (_lock)
        {
            if (TryGetResolvingObject(out T? dependency))
            {
                // If we get here then the dependency isn't null.
                return dependency!;
            }

            _resolvedDependencies[typeof(T)] = resolvingObject;
            return resolvingObject;
        }
    }

    /// <summary>
    /// Remove all objects from the list of resolved dependencies. Call Dispose on each object that
    /// implements the <see cref="IDisposable" /> interface.
    /// </summary>
    internal void Clear()
    {
        lock (_lock)
        {
            foreach (Type type in _resolvedDependencies.Keys)
            {
                if (_resolvedDependencies[type] is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _ = _resolvedDependencies.Remove(type);
            }
        }
    }

    /// <summary>
    /// Check to see if the specified dependency type has been resolved and, if it has, return the
    /// resolving object.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type whose resolving object is to be retrieved.
    /// </typeparam>
    /// <param name="resolvingObject">
    /// The resolved dependency object, or <see langword="null" /> if the dependency type hasn't yet
    /// been resolved.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if the given dependency type has been resolved, otherwise
    /// <see langword="false" />.
    /// </returns>
    internal bool TryGetResolvedDependency<T>(out T? resolvingObject) where T : class
    {
        lock (_lock)
        {
            return TryGetResolvingObject(out resolvingObject);
        }
    }

    /// <summary>
    /// Try to retrieve the resolving object for the given dependency type
    /// <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type whose resolving object we want to retrieve.
    /// </typeparam>
    /// <param name="resolvingObject">
    /// The retrieved resolving object, or <see langword="null" /> if no object exists.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if a resolving object is found, otherwise returns
    /// <see langword="false" />.
    /// </returns>
    private bool TryGetResolvingObject<T>(out T? resolvingObject) where T : class
    {
        if (_resolvedDependencies.TryGetValue(typeof(T), out object? resolvingInstance))
        {
            if (resolvingInstance is not null)
            {
                resolvingObject = (T)resolvingInstance;
                return true;
            }
        }

        resolvingObject = default;
        return false;
    }
}