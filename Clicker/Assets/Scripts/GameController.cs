using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public AnimHash.VoidCallBack GoldConsumeCallback
    { get; set; }

    [SerializeField]
    private double mGold;
    public double Gold
    {
        get
        {
            return mGold;
        }
        set
        {
            if(value >= 0)
            {
                if(mGold > value)
                {
                    GoldConsumeCallback?.Invoke();
                    GoldConsumeCallback = null;
                }

                mGold = value;
                MainUIController.Instance.ShowGold(mGold);
            }
            else
            {
                // not enough money
                Debug.Log("not enough money");
            }
        }
    }

    private int mStage;
    public int StageNumber
    {
        get
        {
            return mStage;
        }
    }

    [SerializeField]
    private GemController mGem;

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

    private void Start()
    {
        MainUIController.Instance.ShowGold(0);
        int id = Random.Range(0, GemController.MAX_GEM_COUNT);
        mGem.GetNewGem(id);
    }

    public void Touch()
    {
        if(mGem.AddProgress(1))
        {
            mStage++;
            int id = Random.Range(0, GemController.MAX_GEM_COUNT);
            mGem.GetNewGem(id);
        }
    }
}
