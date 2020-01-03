using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField]
    private PlayerSaveData mPlayer;

    public AnimHash.VoidCallBack GoldConsumeCallback
    { get; set; }

    public double Gold
    {
        get
        {
            return mPlayer.Gold;
        }
        set
        {
            if(value >= 0)
            {
                if(mPlayer.Gold > value)
                {
                    GoldConsumeCallback?.Invoke();
                    GoldConsumeCallback = null;
                }

                mPlayer.Gold = value;
                MainUIController.Instance.ShowGold(mPlayer.Gold);
            }
            else
            {
                // not enough money
                Debug.Log("not enough money");
            }
        }
    }

    
    public int StageNumber
    {
        get
        {
            return mPlayer.Stage;
        }
    }

    [SerializeField]
    private GemController mGem;

    private double mTouchPower;
    public double TouchPower
    {
        get
        {
            return mTouchPower;
        }
        set
        {
            mTouchPower = value;
        }
    }

    private float mCriticalRate;
    public float CriticalRate
    {
        get
        {
            return mCriticalRate;
        }
        set
        {
            mCriticalRate = value;
        }
    }

    private float mCriticalValue;
    public float CriticalValue
    {
        get
        {
            return mCriticalValue;
        }
        set
        {
            mCriticalValue = value;
        }
    }

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
        MainUIController.Instance.ShowGold(mPlayer.Gold);

        PlayerPrefs.DeleteAll();

        Load();

        StartCoroutine(LoadGames());

        //mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
        //mGem.GetNewGem(mPlayer.GemID);
    }

    private IEnumerator LoadGames()
    {
        WaitForSeconds pointOne = new WaitForSeconds(.1f);

        while(!PlayerInfoController.Instance.Loaded || !ColleagueController.Instance.Loaded)
        {
            yield return pointOne;
        }

        if(mPlayer.GemID < 0)
        {
            mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
        }

        mGem.LoadGem(mPlayer.GemID, mPlayer.GemHP);
        PlayerInfoController.Instance.Load(mPlayer.PlayerLevels);
        ColleagueController.Instance.Load(mPlayer.ColleagueLevels);
    }

    public void Touch()
    {
        double touchPower = mTouchPower;

        float randVal = UnityEngine.Random.value;

        if(randVal <= mCriticalRate)
        {
            touchPower *= 1 + CriticalValue;
        }

        if(mGem.AddProgress(touchPower))
        {
            mPlayer.Stage++;
            mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
            mGem.GetNewGem(mPlayer.GemID);
        }
    }

    public void Save()
    {
        mPlayer.GemHP = mGem.CurrentHP;
        mPlayer.PlayerLevels = PlayerInfoController.Instance.LevelArr;
        mPlayer.ColleagueLevels = ColleagueController.Instance.LevelArr;

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        formatter.Serialize(stream, mPlayer);

        string data = Convert.ToBase64String(stream.GetBuffer());

        PlayerPrefs.SetString("Player", data);
        stream.Close();
    }

    public void Load()
    {
        string data = PlayerPrefs.GetString("Player");

        if(!string.IsNullOrEmpty(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(data));

            mPlayer = (PlayerSaveData)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            mPlayer = new PlayerSaveData();
            mPlayer.GemID = -1;

            mPlayer.PlayerLevels = new int[AnimHash.PLAYER_INFOS_LENGHTH];
            mPlayer.PlayerLevels[0] = 1;
            mPlayer.ColleagueLevels = new int[AnimHash.COLLEAGUE_INFOS_LENGTH];
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            Load();
            mGem.LoadGem(mPlayer.GemID, mPlayer.GemHP);
        }
    }
}
