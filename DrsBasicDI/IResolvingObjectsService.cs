namespace DrsBasicDI;

using System;

/// <summary>
/// The <see cref="IResolvingObjectsService" /> interface defines the methods used for maintaining the list
/// of resolved dependency objects.
/// </summary>
internal interface IResolvingObjectsService : IDisposable
{
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
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be added.
    /// </param>
    /// <returns>
    /// The <paramref name="resolvingObject" /> or the object retrieved from the list of resolving
    /// objects if one already exists for the given dependency type <typeparamref name="T" />.
    /// </returns>
    public T Add<T>(T resolvingObject, string key) where T : class;

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
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be retrieved.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if the given dependency type has been resolved, otherwise
    /// <see langword="false" />.
    /// </returns>
    public bool TryGetResolvingObject<T>(out T? resolvingObject, string key) where T : class;
}