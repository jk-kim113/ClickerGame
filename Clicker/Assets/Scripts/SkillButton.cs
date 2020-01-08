using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField]
    private Image mCoolTimeImg;
    [SerializeField]
    private Text mCooltimeTxt;

    public void ShowCoolTime(float CooltimeBase, float CooltimeCurrent)
    {
        mCoolTimeImg.fillAmount = CooltimeCurrent / CooltimeBase;

        float min = Mathf.Floor(CooltimeCurrent / 60);
        float sec = Mathf.Floor(CooltimeCurrent % 60);
        
        mCooltimeTxt.text = string.Format("{0} : {1}", min.ToString("00"), sec.ToString("00"));
    }

    public void SetVisible(bool visible)
    {
        mCoolTimeImg.gameObject.SetActive(visible);
    }
}
