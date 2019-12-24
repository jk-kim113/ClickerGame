using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInfoController : MonoBehaviour
{
    public static PlayerInfoController Instance;

    [SerializeField]
    private PlayerInfo[] mInfos;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class PlayerInfo
{
    public int ID;
    public string Name;
    public int Level;
    public string Contents;

    public int IconID;

    public ePlayerValueType ValueType;

    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public float CoolTime;
    public float CoolTimeCurrent;
    public float Duration;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}

public enum ePlayerValueType
{
    Exp,
    Numeric,
    Percent
}