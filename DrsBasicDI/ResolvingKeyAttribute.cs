﻿namespace DrsBasicDI;

/// <summary>
/// Attribute used to specify the key to use when resolving a dependency.
/// </summary>
/// <param name="key">
/// A <see langword="string" /> value that is used as the resolving key."
/// </param>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
public sealed class ResolvingKeyAttribute(string key) : Attribute
{
    /// <summary>
    /// Get the resolving key value.
    /// </summary>
    public string Key
    {
        get;
    } = key ?? EmptyKey;
}