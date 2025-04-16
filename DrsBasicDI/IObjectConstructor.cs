﻿namespace DrsBasicDI;

using System.Reflection;

/// <summary>
/// The <see cref="IObjectConstructor" /> defines a method for constructing resolving objects for a
/// given dependency type.
/// </summary>
internal interface IObjectConstructor
{
    /// <summary>
    /// Constructs an object of type <typeparamref name="TDependency" /> using the specified
    /// <paramref name="constructorInfo" /> and <paramref name="parameterValues" />.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The dependency type to be constructed.
    /// </typeparam>
    /// <param name="constructorInfo">
    /// The constructor information for the resolving type that was mapped to the dependency type
    /// <typeparamref name="TDependency" />.
    /// </param>
    /// <param name="parameterValues">
    /// The constructor parameter values to be used for constructing the resolving type.
    /// </param>
    /// <param name="key">
    /// An optional key used to identify the specific resolving object to be constructed.
    /// </param>
    /// <returns>
    /// The resolving object cast to the dependency type <typeparamref name="TDependency" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public TDependency Construct<TDependency>(ConstructorInfo constructorInfo, object[] parameterValues, string key) where TDependency : class;
}