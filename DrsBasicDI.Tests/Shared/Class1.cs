namespace DrsBasicDI.Shared;

public class Class1 : IClass1
{
    public string BuiltBy { get; init; } = "Normal";

    public virtual string DoWork() => "Class1.DoWork";
}
