﻿namespace DrsBasicDI.Shared;

public class Class3(IClass2 class2) : IClass3
{
    private readonly IClass2 _class2 = class2;

    public string DoWork() => $"{_class2.DoWork()} called from Class3.DoWork";
}
