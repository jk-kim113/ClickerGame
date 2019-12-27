using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JasonGenerator<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    private ColleagueData[] mDataArr;

    [SerializeField]
    private PlayerInfo[] mPlayerInfos;

    #region CSV File Write & Read
    
    //[SerializeField]
    //private Dummy[] mSampleArr;

    //private void LoadSample()
    //{
    //    string data = Resources.Load<TextAsset>("JsonFiles/csvFile").text;

    //    string[] dataArr = data.Split('\n');

    //    Debug.Log(dataArr.Length);

    //    mSampleArr = new Dummy[data.Length - 1];
    //    for(int i = 0; i < mSampleArr.Length; i++)
    //    {
    //        string[] splited = dataArr[i + 1].Split(',');
    //        mSampleArr[i] = new Dummy();
    //        mSampleArr[i].id = int.Parse(splited[0]);
    //        mSampleArr[i].name = splited[1];
    //        mSampleArr[i].value = int.Parse(splited[2]);
    //    }
    //}
    
    #endregion

    public void GenerateJson(T[] Arr, string path)
    {
        string data = JsonConvert.SerializeObject(Arr, Formatting.Indented);

        StreamWriter writer = new StreamWriter(Application.dataPath + path);
        writer.Write(data);
        writer.Close();
    }

    public T LoadJson(string path)
    {
        string data = Resources.Load<TextAsset>(path).text;

        T Arr = JsonConvert.DeserializeObject<T>(data);

        return Arr;
    }

    public void GeneratePlayerInfo()
    {
        string data = JsonConvert.SerializeObject(mPlayerInfos, Formatting.Indented);

        Debug.Log(data);

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/JsonFiles/PlayerInfo.json");
        writer.Write(data);
        writer.Close();
    }

    public void LoadPlayerInfo()
    {
        string data = Resources.Load<TextAsset>("JsonFiles/PlayerInfo").text;

        mPlayerInfos = JsonConvert.DeserializeObject<PlayerInfo[]>(data);
    }

    public void GenerateColleague()
    {
        string data = JsonConvert.SerializeObject(mDataArr, Formatting.Indented);

        Debug.Log(data);

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/JsonFiles/Colleague.json");
        writer.Write(data);
        writer.Close();
    }

    public void LoadColleague()
    {
        string data = Resources.Load<TextAsset>("JasonFiles/Colleague").text;

        mDataArr = JsonConvert.DeserializeObject<ColleagueData[]>(data);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateColleague();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GeneratePlayerInfo();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadPlayerInfo();
        }
    }
}

public class Dummy
{
    public int id;
    public string name;
    public int value;
}