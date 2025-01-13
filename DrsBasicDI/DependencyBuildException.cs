namespace DrsBasicDI;

[Serializable]
public class DependencyBuildException : Exception
{
    public DependencyBuildException()
    {
    }

    public DependencyBuildException(string message) : base(message)
    {
    }

    public DependencyBuildException(string message, Exception inner) : base(message, inner)
    {
    }
}