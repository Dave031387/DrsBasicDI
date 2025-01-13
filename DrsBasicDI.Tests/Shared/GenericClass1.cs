namespace DrsBasicDI.Shared;

public class GenericClass1<S, T> : IGenericClass1<S, T>
{
    public string DoWork(S arg1, T arg2)
    {
        string typeS = typeof(S).GetFriendlyName();
        string typeT = typeof(T).GetFriendlyName();

        return $"GenericClass1<{typeS}, {typeT}>.DoWork\n  arg1={arg1}\n  arg2={arg2}";
    }
}