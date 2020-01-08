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

public enum eSkillID
{
    Chain,
    OverWork
}

[Serializable]
public class PlayerSaveData
{
    public int Stage;
    public int GemID;
    public double Gold;
    public double Soul;
    public double GemHP;
    public int[] PlayerLevels;
    public float[] Cooltimes;
    public int[] ColleagueLevels;
}