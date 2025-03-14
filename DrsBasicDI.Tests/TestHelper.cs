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
}
