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

        mTouchPower = 1;
    }

    private void Start()
    {
        MainUIController.Instance.ShowGold(mPlayer.Gold);

        Load();
        mGem.LoadGem(mPlayer.GemID, mPlayer.GemHP);

        //mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
        //mGem.GetNewGem(mPlayer.GemID);
    }

    public void Touch()
    {
        if(mGem.AddProgress(mTouchPower))
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
