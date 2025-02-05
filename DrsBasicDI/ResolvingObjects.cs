namespace DrsBasicDI;

/// <summary>
/// The <see cref="ResolvingObjects" /> class manages a dictionary of resolving objects for the
/// singleton and scoped dependencies contained in the dependency injection container.
/// </summary>
internal sealed class ResolvingObjects : IResolvingObjects
{
    /// <summary>
    /// A dictionary of resolving objects whose keys are the dependency types that have been
    /// resolved and whose values are instances of the corresponding resolving types.
    /// </summary>
    internal readonly Dictionary<Type, object> _resolvingObjects = [];

    /// <summary>
    /// A lock object used to ensure thread safety when accessing/modifying the
    /// <see cref="_resolvingObjects" /> field.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Create an instance of the <see cref="ResolvingObjects" /> class.
    /// </summary>
    internal ResolvingObjects()
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
    public T Add<T>(T resolvingObject) where T : class
    {
        if (TryGetResolvingObject(out T? value))
        {
            // If we get here then a resolving object for the given dependency type already been
            // added to the container and the resolving object that was added isn't null.
            return value!;
        }

        lock (_lock)
        {
            // If we get here then a resolving object hasn't yet been added to the container for the
            // given dependency type.
            _resolvingObjects[typeof(T)] = resolvingObject;
            return resolvingObject;
        }
    }

    /// <summary>
    /// Remove all objects from the list of resolved dependencies. Call Dispose on each object that
    /// implements the <see cref="IDisposable" /> interface.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            foreach (Type type in _resolvingObjects.Keys)
            {
                if (_resolvingObjects[type] is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _ = _resolvingObjects.Remove(type);
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
    public bool TryGetResolvingObject<T>(out T? resolvingObject) where T : class
    {
        lock (_lock)
        {
            if (_resolvingObjects.TryGetValue(typeof(T), out object? value))
            {
                if (value is not null)
                {
                    resolvingObject = (T)value;
                    return true;
                }
            }
        }

        resolvingObject = default;
        return false;
    }
}