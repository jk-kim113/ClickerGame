using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColleagueController : DataLoader
{
    public static ColleagueController Instance;

#pragma warning disable 0649
    [SerializeField]
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

    [SerializeField]
    private TextEffectPool mTextEffectPool;

    private List<UIElement> mElementList;
#pragma warning restore

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

        LoadJsonData(out mDataArr, AnimHash.COLLEAGUE_DATA_PATH);
    }

    void Start()
    {
        mElementList = new List<UIElement>();
        mSpawnedList = new List<Colleague>();

        for(int i = 0; i < mDataArr.Length; i++)
        {
            UIElement elem = Instantiate(mElementPrefab, mScrollTarget);

            elem.Init(
                mIconArr[i],
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

    public void JobFinish(int id, Vector3 pos)
    {
        ColleagueData data = mDataArr[id];

        switch(data.JobType)
        {
            case eJobType.Gold:
                GameController.Instance.Gold += data.ValueCurrent;

                TextEffect effect = mTextEffectPool.GetFromPool((int)eTextEffectType.ColleagueIncome);
                effect.ShowText(UnitBuilder.GetUnitStr(data.ValueCurrent));
                effect.transform.position = pos;
                
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
        #region Apply delegate example
        //GameController.Instance.GoldConsumeCallback = () =>
        //{
        //    if (mDataArr[id].Level == 0)
        //    {
        //        Colleague newCol = Instantiate(mPrefabArr[id]);
        //        newCol.transform.position = mSpawnPos.position;
        //        newCol.Init(id, mDataArr[id].JobTime);
        //        mSpawnedList.Add(newCol);
        //    }

        //    mDataArr[id].Level += amount;
        //    mDataArr[id].ValueCurrent = mDataArr[id].ValueBase * Math.Pow(mDataArr[id].ValueWeight, mDataArr[id].Level);
        //    mDataArr[id].CostCurrent = mDataArr[id].CostBase * Math.Pow(mDataArr[id].CostWeight, mDataArr[id].Level);

        //    mElementList[id].Renew(
        //        mDataArr[id].Contents,
        //        "구매",
        //        mDataArr[id].Level,
        //        mDataArr[id].ValueCurrent,
        //        mDataArr[id].CostCurrent,
        //        mDataArr[id].JobTime);

        //    
        //};
        #endregion

        GameController.Instance.GoldConsumeCallback = () => { ApplyLevel(id, amount); };

        GameController.Instance.Gold -= mDataArr[id].CostCurrent;
    }

    public void ApplyLevel(int id, int amount)
    {
        if (mDataArr[id].Level == 0)
        {
            Colleague newCol = Instantiate(mPrefabArr[id]);
            newCol.transform.position = mSpawnPos.position;
            newCol.Init(id, mDataArr[id].JobTime);
            mSpawnedList.Add(newCol);
        }

        mDataArr[id].Level += amount;
        mDataArr[id].ValueCurrent = mDataArr[id].ValueBase * Math.Pow(mDataArr[id].ValueWeight, mDataArr[id].Level);
        mDataArr[id].CostCurrent = mDataArr[id].CostBase * Math.Pow(mDataArr[id].CostWeight, mDataArr[id].Level);

        mElementList[id].Renew(
            mDataArr[id].Contents,
            "구매",
            mDataArr[id].Level,
            mDataArr[id].ValueCurrent,
            mDataArr[id].CostCurrent,
            mDataArr[id].JobTime);

        //UnityEngine.Random.Range(0,3);
    }
}

[Serializable]
public class ColleagueData
{
    public string Name;
    public int Level;
    public string Contents;
    public float JobTime;
    public eJobType JobType;

    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}

public enum eJobType
{
    Gold,
    Touch
}
