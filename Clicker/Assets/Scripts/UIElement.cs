﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private Image mIcon;
    [SerializeField]
    private Text mNameText, mLevelText, mContentsText, mPurchaseText, mCostText;
    [SerializeField]
    private Button mPurchaseButton;

    private int mID;

    public void Init(Sprite icon, int id, string name, string contents, string puechaseText,
        int level, double value, double cost, double time, AnimHash.TwoIntParaCallBack callback)
    {
        mIcon.sprite = icon;
        mID = id;
        mNameText.text = name;
        mPurchaseButton.onClick.AddListener(() => { callback(mID, 1); });
        Renew(contents, puechaseText, level, value, cost, time);
    }

    public void Renew(string contents, string purchaseText, int level, double value, double cost, double time)
    {
        mContentsText.text = string.Format(contents, UnitBuilder.GetUnitStr(value), time.ToString("N1"));
        mCostText.text = UnitBuilder.GetUnitStr(cost);
        mPurchaseText.text = purchaseText;
        mLevelText.text = "LV." + level.ToString("N0");
    }
}
