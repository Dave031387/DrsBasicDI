﻿namespace DrsBasicDI;

/// <summary>
/// The <see cref="IServiceLocater" /> interface defines a method for getting a service from a service locater.
/// </summary>
internal interface IServiceLocater
{
    /// <summary>
    /// Get an instance of the implementing class that is mapped to the given interface type
    /// <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    /// The interface type for which we want to retrieve the corresponding implementation class
    /// object.
    /// </typeparam>
    /// <param name="key">
    /// An optional key used to identify the specific implementation class object to be retrieved.
    /// </param>
    /// <returns>
    /// An instance of the implementation type that has been mapped to the given interface type
    /// <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="ServiceLocaterException" />
    public T Get<T>(string key = EmptyKey) where T : class;
}
