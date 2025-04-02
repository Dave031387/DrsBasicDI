namespace DrsBasicDI;

public static class TestHelper
{
    public static void AssertException<T>(Action action, string message) where T : Exception
    {
        action
            .Should()
            .ThrowExactly<T>()
            .WithMessage(message);
    }

    internal static Mock<IDependency> GetMockDependency(DependencyLifetime? lifetime = null,
                                                        Type? resolvingType = null,
                                                        Func<object>? factory = null,
                                                        Type? dependencyType = null,
                                                        bool checkFactory = false,
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
        else if (checkFactory)
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