﻿namespace RichbetsResurrected.Entities.Crash;

public class CrashCashoutResult
{
    public bool IsSuccess { get; set; }
    public CrashError? Error { get; set; }
    public CrashPlayer? Player { get; set; }
}