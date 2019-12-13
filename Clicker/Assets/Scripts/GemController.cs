﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    public const int MAX_GEM_COUNT = 3;

    [SerializeField]
    private int mSheetCount = 5;

    [SerializeField]
    private SpriteRenderer mGem;
    [SerializeField]
    private Sprite[] mGemSprite;

    private double mCurrentHP, mMaxHP, mPhaseBoundary;

    private int mCurrentPhase, mStartIndex;

    [SerializeField]
    private MainUIController mMainUIController;

    void Awake()
    {
        mGemSprite = Resources.LoadAll<Sprite>("Gem");
    }

    public void GetNewGem(int id)
    {
        mStartIndex = id * mSheetCount;
        mGem.sprite = mGemSprite[mStartIndex];
        mCurrentPhase = 0;
        mCurrentHP = 0;
        mMaxHP = 100;
        mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);
    }

    public bool AddProgress(double value)
    {
        mCurrentHP += value;
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);
        
        if(mCurrentHP >= mPhaseBoundary)
        {
            mCurrentPhase++;

            if (mCurrentPhase > 4)
            {
                //Clear
                return true;
            }
            
            mGem.sprite = mGemSprite[mStartIndex + mCurrentPhase];
            mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        }
        return false;
    }
}
