using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataLoader : MonoBehaviour
{
    protected void LoadJsonData<T>(out T[] output, string location)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(location);

        if(textAsset != null)
        {
            string data = textAsset.text;

            if(!string.IsNullOrEmpty(data))
            {
                output = JsonConvert.DeserializeObject<T[]>(data); // try catch로 한번 더 체크가능하지만 쓰지 마셈
                return;
            }
            else
            {
                Debug.LogWarning("File is Empty : " + location);
            }
        }
        else
        {
            Debug.LogWarning("Wrong File Path : " + location);
        }

        output = null;
    }
}
