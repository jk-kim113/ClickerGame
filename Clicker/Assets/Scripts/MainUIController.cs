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

    public void ShowProgress(float progress)
    {
        mProgressBar.ShowGaugeBar(progress);
    }
}
