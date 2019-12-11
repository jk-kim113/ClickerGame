using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField]
    private Image mGaugeBarImg;

    public void ShowGaugeBar(float progress)
    {
        mGaugeBarImg.fillAmount = progress;
    }
}
