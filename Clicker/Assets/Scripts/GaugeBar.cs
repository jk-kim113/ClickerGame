using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField]
    private Image mGaugeBarImg;

    [SerializeField]
    private Text mGaugeBarText;

    public void ShowGaugeBar(float progress, string text)
    {
        mGaugeBarImg.fillAmount = progress;
        mGaugeBarText.text = text;
    }
}
