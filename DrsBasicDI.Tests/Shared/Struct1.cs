﻿namespace DrsBasicDI.Shared;

public struct Struct1 : IStruct1
{
#pragma warning disable IDE0251 // Make member 'readonly'
    public string DoWork() => "Struct1.DoWork";
#pragma warning restore IDE0251 // Make member 'readonly'
}
