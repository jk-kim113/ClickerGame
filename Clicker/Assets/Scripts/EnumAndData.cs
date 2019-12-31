using System.Collections;
using System.Collections.Generic;
using System;

public enum eEffectType
{
    Touch,
    PhaseShift
}

public enum eTextEffectType
{
    ColleagueIncome
}

public enum eValueType
{
    Exp,
    Numeric,
    Percent
}

[Serializable]
public class PlayerSaveData
{
    public int Stage;
    public int GemID;
    public double Gold;
    public double GemHP;
    public int[] PlayerLevels;
    public int[] ColleagueLevels;
}