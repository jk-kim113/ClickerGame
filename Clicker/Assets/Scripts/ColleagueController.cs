﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueController : MonoBehaviour
{
    public static ColleagueController Instance;

    private ColleagueData[] mDataArr;

    [SerializeField]
    private Colleague[] mPrefabArr;
    private List<Colleague> mSpawnedList;

    [SerializeField]
    private Transform mSpawnPos;

    [SerializeField]
    private Sprite[] mIconArr;

    [SerializeField]
    private UIElement mElementPrefab;

    [SerializeField]
    private Transform mScrollTarget;

    private List<UIElement> mElementList;

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
        mDataArr[0].Contents = "<Color=#FF0000FF>{1}</Color>초 마다 <Color=#0000FFFF>{0}</Color>골드를 획득합니다.";
        mDataArr[0].JobTime = 1.1f;
        mDataArr[0].JobType = eJobType.Touch;
        mDataArr[0].ValueCurrent = 1;
        mDataArr[0].CostCurrent = 100;

        mDataArr[1] = new ColleagueData();
        mDataArr[1].Name = "No.2";
        mDataArr[1].Level = 0;
        mDataArr[1].Contents = "<Color=#FF0000FF>{1}</Color>초 마다 한번씩 터치룰 해줍니다";
        mDataArr[1].JobTime = 1f;
        mDataArr[1].JobType = eJobType.Touch;
        mDataArr[1].ValueCurrent = 0;
        mDataArr[1].CostCurrent = 200;

        mDataArr[2] = new ColleagueData();
        mDataArr[2].Name = "No.3";
        mDataArr[2].Level = 0;
        mDataArr[2].Contents = "<Color=#FF0000FF>{1}</Color>초 마다 <Color=#0000FFFF>{0}</Color>골드를 획득합니다.";
        mDataArr[2].JobTime = 1.5f;
        mDataArr[2].JobType = eJobType.Gold;
        mDataArr[2].ValueCurrent = 1;
        mDataArr[2].CostCurrent = 300;
    }

    void Start()
    {
        mElementList = new List<UIElement>();
        mSpawnedList = new List<Colleague>();

        for(int i = 0; i < mDataArr.Length; i++)
        {
            UIElement elem = Instantiate(mElementPrefab, mScrollTarget);

            elem.Init(
                null,
                i,
                mDataArr[i].Name,
                mDataArr[i].Contents,
                "구매",
                mDataArr[i].Level,
                mDataArr[i].ValueCurrent,
                mDataArr[i].CostCurrent,
                mDataArr[i].JobTime,
                AddLevel);

            mElementList.Add(elem);
        }
    }

    public void JobFinish(int id)
    {
        ColleagueData data = mDataArr[id];

        switch(data.JobType)
        {
            case eJobType.Gold:
                GameController.Instance.Gold += data.ValueCurrent;
                break;
            case eJobType.Touch:
                GameController.Instance.Touch();
                break;
            default:
                break;
        }
    }

    public void AddLevel(int id, int amount)
    {
        if(mDataArr[id].Level == 0)
        {
            Colleague newCol = Instantiate(mPrefabArr[id]);
            newCol.transform.position = mSpawnPos.position;
            newCol.Init(id, mDataArr[id].JobTime);
            mSpawnedList.Add(newCol);
        }

        mDataArr[id].Level += amount;
        mDataArr[id].ValueCurrent += mDataArr[id].Level;
        mDataArr[id].CostCurrent += mDataArr[id].Level;

        mElementList[id].Renew(
            mDataArr[id].Contents,
            "구매",
            mDataArr[id].Level,
            mDataArr[id].ValueCurrent,
            mDataArr[id].CostCurrent,
            mDataArr[id].JobTime);
    }
}

public class ColleagueData
{
    public string Name;
    public int Level;
    public string Contents;
    public float JobTime;
    public eJobType JobType;
    public double ValueCurrent;
    public double CostCurrent;
}

public enum eJobType
{
    Gold,
    Touch
}
