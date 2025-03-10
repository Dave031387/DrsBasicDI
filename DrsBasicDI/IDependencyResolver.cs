﻿namespace DrsBasicDI;

/// <summary>
/// The <see cref="IDependencyResolver" /> interface defines the methods implemented by the
/// dependency resolver object.
/// </summary>
internal interface IDependencyResolver
{
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
    public T Resolve<T>(string key) where T : class;

    /// <summary>
    /// Set the scoped <see cref="IResolvingObjectsService" /> to the specified
    /// <paramref name="resolvingObjectsService" /> instance.
    /// </summary>
    /// <param name="resolvingObjectsService">
    /// The scoped <see cref="IResolvingObjectsService" /> instance to be set.
    /// </param>
    public void SetScopedResolver(IResolvingObjectsService resolvingObjectsService);
}