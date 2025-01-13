namespace DrsBasicDI;

[Serializable]
public class ContainerBuildException : Exception
{
    public ContainerBuildException()
    {
    }

    public ContainerBuildException(string message) : base(message)
    {
    }

    public ContainerBuildException(string message, Exception inner) : base(message, inner)
    {
    }
}