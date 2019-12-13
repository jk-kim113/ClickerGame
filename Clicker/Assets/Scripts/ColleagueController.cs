﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueController : MonoBehaviour
{
    public static ColleagueController Instance;

    private ColleagueData[] mDataArr;

    [SerializeField]
    private Colleague[] mPrefabArr;

    [SerializeField]
    private Transform mSpawnPos;

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

        mDataArr = new ColleagueData[3];

        mDataArr[0] = new ColleagueData();
        mDataArr[0].Name = "No.1";
        mDataArr[0].Level = 0;
        mDataArr[0].JobTime = 1.1f;
        mDataArr[0].CostCurrent = 100;

        mDataArr[1] = new ColleagueData();
        mDataArr[1].Name = "No.2";
        mDataArr[1].Level = 0;
        mDataArr[1].JobTime = 1f;
        mDataArr[1].CostCurrent = 200;

        mDataArr[2] = new ColleagueData();
        mDataArr[2].Name = "No.3";
        mDataArr[2].Level = 0;
        mDataArr[2].JobTime = 1.5f;
        mDataArr[2].CostCurrent = 300;
    }

    void Start()
    {
        
    }

    public void JobFinish(int id)
    {
        ColleagueData data = mDataArr[id];

        switch(data.JobType)
        {
            case eJobType.Gold:
                GameController.Instance.Gold += 1;
                break;
            case eJobType.Touch:
                GameController.Instance.Touch();
                break;
            default:
                break;
        }
    }

    public void TempInstantiate(int id)
    {
        AddLevel(id, 1);
    }

    public void AddLevel(int id, int amount)
    {
        Colleague newCol = Instantiate(mPrefabArr[id]);
        newCol.transform.position = mSpawnPos.position;
        newCol.Init(mDataArr[id].Name, id, mDataArr[id].JobTime);
    }
}

public class ColleagueData
{
    public string Name;
    public int Level;
    public float JobTime;
    public eJobType JobType;
    public double CostCurrent;
}

public enum eJobType
{
    Gold,
    Touch
}