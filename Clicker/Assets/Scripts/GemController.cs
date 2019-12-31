using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemController : MonoBehaviour
{
    public const int MAX_GEM_COUNT = 3;

    [SerializeField]
    private EffectPool mEffectPool;

    [SerializeField]
    private int mSheetCount = 5;

    [SerializeField]
    private SpriteRenderer mGem;
    [SerializeField]
    private Sprite[] mGemSprite;

    [SerializeField]
    private float mHPbase = 10, mHPweight = 1.4f, mRewardBase = 10, mRewardWeight = 1.05f;

    private double mCurrentHP, mMaxHP, mPhaseBoundary;
    public double CurrentHP
    {
        get
        {
            return mCurrentHP;
        }
    }

    private int mCurrentPhase, mStartIndex;

    [SerializeField]
    private MainUIController mMainUIController;

    void Awake()
    {
        mGemSprite = Resources.LoadAll<Sprite>("Gem");
    }

    public void LoadGem(int lastGemID, double currentHp)
    {
        mStartIndex = lastGemID * mSheetCount;
        mCurrentHP = currentHp;
        mMaxHP = mHPbase * Math.Pow(mHPweight, GameController.Instance.StageNumber);
        
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);

        mCurrentPhase = 0;

        while(mCurrentHP >= mPhaseBoundary)
        {
            mCurrentPhase++;
            mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        }

        mGem.sprite = mGemSprite[mStartIndex + mCurrentPhase];
    }

    public void GetNewGem(int id)
    {
        mStartIndex = id * mSheetCount;
        mGem.sprite = mGemSprite[mStartIndex];
        mCurrentPhase = 0;
        mCurrentHP = 0;
        mMaxHP = mHPbase * Math.Pow(mHPweight, GameController.Instance.StageNumber);
        mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);
    }

    //public void Save()
    //{
    //    PlayerPrefs.SetString("GemHP", mCurrentHP.ToString());
    //    string data = PlayerPrefs.GetString("GemHP");
    //}

    public bool AddProgress(double value)
    {
        mCurrentHP += value;
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);
        
        if(mCurrentHP >= mPhaseBoundary)
        {
            mCurrentPhase++;

            if (mCurrentPhase > 4)
            {
                GameController.Instance.Gold += mRewardBase * Math.Pow(mRewardWeight, GameController.Instance.StageNumber);

                Timer effect = mEffectPool.GetFromPool((int)eEffectType.PhaseShift);
                effect.transform.position = mGem.transform.position;

                return true;
            }

            mGem.sprite = mGemSprite[mStartIndex + mCurrentPhase];
            mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        }
        return false;
    }
}
