using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInfoController : DataLoader
{
    public static PlayerInfoController Instance;

#pragma warning disable 0649
    [SerializeField]
    private PlayerInfo[] mInfos;
    
    [SerializeField]
    private UIElement mElementPrefab;

    [SerializeField]
    private Transform mScrollTarget;
    private List<UIElement> mElementList;
#pragma warning restore

    private bool mbLoaded;
    public bool Loaded
    {
        get
        {
            return mbLoaded;
        }
    }

    [SerializeField]
    private int[] mLevelArr;

    [SerializeField]
    private float[] mCoolTimeArr;

    [SerializeField]
    private SkillButton[] mSkillArr;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            mbLoaded = false;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadJsonData(out mInfos, AnimHash.PLAYER_DATA_PATH);
    }

    private void Start()
    {
        mElementList = new List<UIElement>();

        for(int i = 0; i < mInfos.Length; i++)
        {
            UIElement element = Instantiate(mElementPrefab, mScrollTarget);
            element.Init(
                null,
                i,
                mInfos[i].Name,
                mInfos[i].Contents,
                "Level Up",
                mInfos[i].Level,
                mInfos[i].ValueCurrent,
                mInfos[i].CostCurrent,
                mInfos[i].Duration,
                AddLevel,
                mInfos[i].ValueType);
            mElementList.Add(element);
        }

        mbLoaded = true;
    }

    public void Load(int[] levelArr, float[] coolTimeArr)
    {
        mLevelArr = levelArr;
        mCoolTimeArr = coolTimeArr;

        for(int i = 0; i < mInfos.Length; i++)
        {
            mInfos[i].Level = levelArr[i];
            CalcAndShowData(i);
            if(mInfos[i].CoolTimeIndex >= 0)
            {
                StartCoroutine(CoolTimeWorks(i));
            }
        }
    }

    public void AddLevel(int id, int amount)
    {
        GameController.Instance.GoldConsumeCallback = () => { ApplyLevelUp(id, amount); };
        GameController.Instance.Gold -= mInfos[id].CostCurrent;
    }

    public void ApplyLevelUp(int id, int amount)
    {
        mInfos[id].Level += amount;
        CalcAndShowData(id);
    }

    public void CalcAndShowData(int id)
    {
        mInfos[id].CostCurrent = mInfos[id].CostBase * Math.Pow(mInfos[id].CostWeight, mInfos[id].Level);

        switch (mInfos[id].ValueType)
        {
            case eValueType.Exp:
                mInfos[id].ValueCurrent = mInfos[id].ValueBase * Math.Pow(mInfos[id].ValueWeight, mInfos[id].Level);
                break;
            case eValueType.Numeric:
            case eValueType.Percent:
                mInfos[id].ValueCurrent = mInfos[id].ValueBase + mInfos[id].ValueWeight * mInfos[id].Level;
                break;
            default:
                Debug.LogError("Wrong value type : " + mInfos[id].ValueType);
                break;
        }

        mElementList[id].Renew(
            mInfos[id].Contents,
            "Level Up",
            mInfos[id].Level,
            mInfos[id].ValueCurrent,
            mInfos[id].CostCurrent,
            mInfos[id].Duration,
            mInfos[id].ValueType);

        if (id == 0)
        {
            GameController.Instance.TouchPower = mInfos[id].ValueCurrent;
        }
        else if (id == 1)
        {

        }
        else if (id == 2)
        {

        }
        else if (id == 3)
        {
            GameController.Instance.CriticalRate = (float)mInfos[id].ValueCurrent;
        }
        else if (id == 4)
        {
            GameController.Instance.CriticalValue = (float)mInfos[id].ValueCurrent;
        }
        else if (id == 5)
        {
            GameController.Instance.IncomeBonusWeight = mInfos[id].ValueCurrent;
        }
        else if (id == 6)
        {
            GameController.Instance.MaxHPWeight = mInfos[id].ValueCurrent;
        }
        else
        {
            Debug.LogError("Wrong Item ID" + id);
        }
    }

    public void ActiveSkill(int id)
    {
        mCoolTimeArr[mInfos[id].CoolTimeIndex] = mInfos[id].CoolTime;
        StartCoroutine(CoolTimeWorks(id));

        switch((eSkillID)mInfos[id].CoolTimeIndex)
        {
            case eSkillID.Chain:
                StartCoroutine(ChainFunction());
                break;
            case eSkillID.OverWork:

                int count = (int)Mathf.Ceil((float)mInfos[(int)eSkillID.OverWork].ValueCurrent);

                for(int i = 0; i < count; i++)
                {
                    ColleagueController.Instance.ForcedJobFinishAll();
                }

                break;
            default:
                break;
        }
    }

    private IEnumerator ChainFunction()
    {
        float duration = mInfos[(int)eSkillID.Chain].Duration;
        float OPS = mInfos[(int)eSkillID.Chain].Duration / (float)mInfos[(int)eSkillID.Chain].ValueCurrent;
        WaitForSeconds gap = new WaitForSeconds(OPS);
        
        while (duration > 0)
        {
            duration -= OPS;

            GameController.Instance.Touch();

            yield return gap;
        }
    }

    private IEnumerator CoolTimeWorks(int id)
    {
        WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

        int cooltimeID = mInfos[id].CoolTimeIndex;

        mSkillArr[cooltimeID].SetVisible(true);

        while(mCoolTimeArr[cooltimeID] > 0)
        {
            mCoolTimeArr[cooltimeID] -= Time.fixedDeltaTime;
            mSkillArr[cooltimeID].ShowCoolTime(mInfos[id].CoolTime, mCoolTimeArr[mInfos[id].CoolTimeIndex]);
            yield return fixedUpdate;
        }

        mSkillArr[id - 1].SetVisible(false);
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

    public eValueType ValueType;

    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public float CoolTime;
    public int CoolTimeIndex;
    public float Duration;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}

