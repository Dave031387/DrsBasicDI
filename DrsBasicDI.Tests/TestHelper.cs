namespace DrsBasicDI;

/// <summary>
/// The <see cref="TestHelper" /> class provides helper methods for unit tests.
/// </summary>
public static class TestHelper
{
    /// <summary>
    /// Assert an action that should throw the given exception type with the given message.
    /// </summary>
    /// <typeparam name="T">
    /// The type of exception that should be thrown.
    /// </typeparam>
    /// <param name="action">
    /// The action that throws the exception.
    /// </param>
    /// <param name="message">
    /// The message that the exception should have.
    /// </param>
    public static void AssertException<T>(Action action, string message) where T : Exception
    {
        action
            .Should()
            .ThrowExactly<T>()
            .WithMessage(message);
    }

    /// <summary>
    /// Get a mock <see cref="IDependency" /> object with the given properties.
    /// </summary>
    /// <typeparam name="T">
    /// The dependency type.
    /// </typeparam>
    /// <param name="lifetime">
    /// The dependency lifetime.
    /// </param>
    /// <param name="resolvingType">
    /// The resolving type.
    /// </param>
    /// <param name="factory">
    /// The resolving factory method.
    /// </param>
    /// <param name="factoryReturnsNull">
    /// A boolean that should be set to <see langword="true" /> if the <paramref name="factory" />
    /// should be set up to return <see langword="null" />.
    /// </param>
    /// <param name="resolvingKey">
    /// The resolving key.
    /// </param>
    /// <returns>
    /// A <see cref="Mock{IDependency}" /> object having the given properties.
    /// </returns>
    internal static Mock<IDependency> GetMockDependency<T>(DependencyLifetime? lifetime = null,
                                                           Type? resolvingType = null,
                                                           Func<object>? factory = null,
                                                           bool factoryReturnsNull = false,
                                                           string? resolvingKey = null)
        => GetMockDependency(lifetime, resolvingType, factory, typeof(T), factoryReturnsNull, resolvingKey);

    /// <summary>
    /// Get a mock <see cref="IDependency" /> object with the given properties.
    /// </summary>
    /// <param name="lifetime">
    /// The dependency lifetime.
    /// </param>
    /// <param name="resolvingType">
    /// The resolving type.
    /// </param>
    /// <param name="factory">
    /// The resolving factory method.
    /// </param>
    /// <param name="dependencyType">
    /// The dependency type.
    /// </param>
    /// <param name="factoryReturnsNull">
    /// A boolean that should be set to <see langword="true" /> if the <paramref name="factory" />
    /// should be set up to return <see langword="null" />.
    /// </param>
    /// <param name="resolvingKey">
    /// The resolving key.
    /// </param>
    /// <returns>
    /// A <see cref="Mock{IDependency}" /> object having the given properties.
    /// </returns>
    internal static Mock<IDependency> GetMockDependency(DependencyLifetime? lifetime = null,
                                                        Type? resolvingType = null,
                                                        Func<object>? factory = null,
                                                        Type? dependencyType = null,
                                                        bool factoryReturnsNull = false,
                                                        string? resolvingKey = null)
    {
        Mock<IDependency> mock = new(MockBehavior.Strict);

        if (dependencyType is not null)
        {
            mock
                .SetupGet(m => m.DependencyType)
                .Returns(dependencyType)
                .Verifiable(Times.AtLeastOnce);
        }

        if (factory is not null)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(factory)
                .Verifiable(Times.AtLeastOnce);
        }
        else if (factoryReturnsNull)
        {
            mock
                .SetupGet(m => m.Factory)
                .Returns(null!)
                .Verifiable(Times.AtLeastOnce);
        }

        if (lifetime is not null)
        {
            mock
                .SetupGet(m => m.Lifetime)
                .Returns((DependencyLifetime)lifetime)
                .Verifiable(Times.AtLeastOnce);
        }

        if (resolvingType is not null)
        {
            mock
                .SetupGet(m => m.ResolvingType)
                .Returns(resolvingType)
                .Verifiable(Times.AtLeastOnce);
        }

        if (resolvingKey is not null)
        {
            mock
                .SetupGet(m => m.Key)
                .Returns(resolvingKey)
                .Verifiable(Times.AtLeastOnce);
        }

        return mock;
    }
}