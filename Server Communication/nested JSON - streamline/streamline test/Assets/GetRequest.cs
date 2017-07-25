using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetRequest : MonoBehaviour
{
    IEnumerator Start()
    {
        string url = "http://140.247.162.185:9000/point";
        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("error");
        }
        else
        {
            //JsonProcess(www.text);
            string jString = www.text;
        }
    }
}

[System.Serializable]
public class DataInfo
{
    public p0 point;

    public static DataInfo CreateFromJSON(string json)                             //not using static (all instance of datainfo will hsare the same json) 
    {                                                                       //cuz in the future might be multiple instances, each with its own web call/json file
        return JsonUtility.FromJson<DataInfo>(json);
    }
}

[System.Serializable]
public class px                                                         //for nested JSON structure...but maybe use a library to auto parse?
{
    public float x;
    public float vmeg;
}



