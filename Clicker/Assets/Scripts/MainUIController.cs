using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public static MainUIController Instance;

    private static int mUImovehash = Animator.StringToHash("Move");

    [SerializeField]
    Animator[] mWindowAims;

    [SerializeField]
    private GaugeBar mProgressBar;

    [SerializeField]
    private Text mGoldText;

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
    }

    public void MoveWindow(int id)
    {
        mWindowAims[id].SetTrigger(mUImovehash);
    }

    public void ShowGold(double value)
    {
        mGoldText.text = UnitBuilder.GetUnitStr(value);
    }

    public void ShowProgress(double current, double max)
    {
        //TODO calc GuageBar progress float value
        float progress = (float)(current / max);
        //hack Build Gauge progress string
        //string progressString = progress.ToString("P0");

        string progressString = string.Format("{0} / {1}", UnitBuilder.GetUnitStr(current), UnitBuilder.GetUnitStr(max));

        mProgressBar.ShowGaugeBar(progress, progressString);
    }
}
