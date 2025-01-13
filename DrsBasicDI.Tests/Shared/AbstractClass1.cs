namespace DrsBasicDI.Shared;

public abstract class AbstractClass1 : IClass1
{
    public string BuiltBy
    {
        get;
        init;
    } = "Abstract";

    public abstract string DoWork();
}
