﻿namespace DrsBasicDI;

using System.Reflection;

/// <summary>
/// The <see cref="ObjectConstructor" /> class is used to construct resolving objects for a given
/// dependency type.
/// </summary>
internal sealed class ObjectConstructor : IObjectConstructor
{
    /// <summary>
    /// Constructs a new instance of the <see cref="ObjectConstructor" /> class.
    /// </summary>
    public ObjectConstructor()
    {
    }

    /// <summary>
    /// Constructs an object of type <typeparamref name="T" /> using the specified
    /// <paramref name="constructorInfo" /> and <paramref name="parameterValues" />.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type to be constructed.
    /// </typeparam>
    /// <param name="constructorInfo">
    /// The constructor information for the resolving type that was mapped to the dependency type
    /// <typeparamref name="T" />.
    /// </param>
    /// <param name="parameterValues">
    /// The constructor parameter values to be used for constructing the resolving type.
    /// </param>
    /// <returns>
    /// The resolving object cast to the dependency type <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="DependencyInjectionException" />
    public T Construct<T>(ConstructorInfo constructorInfo, object[] parameterValues) where T : class
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

        return resolvingObject;
    }
}