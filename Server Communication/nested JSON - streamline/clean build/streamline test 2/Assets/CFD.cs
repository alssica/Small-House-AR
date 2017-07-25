using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CFD : MonoBehaviour {

    public List<idtr> cfd = new List<idtr>();
    public Text waiting;
    public bool start;

    IEnumerator Start()
    {
        waiting = GetComponentInChildren<Text>();
        waiting.enabled = true;

        string url = "http://140.247.162.185:9000/streamline";
        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            //Debug.Log("error");
            waiting.text = "error communicating to server";
        }
        else
        {
            waiting.text = "downloading";
            //print(www.text);
            cfd = ArrayJson.getJsonArray<ps>(www.text);

            ParseID(cfd);
        }
    }

    public void ParseID(List<idtr> wrapper)
    {
        if (wrapper != null)
        {
            List<float> IDlist = new List<float>();

            foreach (idtr branch in wrapper)
            {

            }
            //print(wrapper.Count);
            waiting.enabled = false;
            int w = wrapper.Count - 1;
            for (int i=0; i< wrapper[w].data.Count; i++)
            {
                Debug.Log(wrapper[w].data[i].id);
            }


            //Debug.Log(orj.GetFields().Attributes());
            //Debug.Log(obj.name);

        }
        else
        {
            waiting.text = "loading";
            waiting.enabled = true;
        }
    }
}

[System.Serializable]
public class ArrayJson
{
    public static List<idtr> getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";                          //put a property wrapper around the original array
        var wrapper = JsonUtility.FromJson<Wrapper>(newJson);
        
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<idtr> array = new List<idtr>();
    }
}

[System.Serializable]
public class idtr
{
    public float id;
    public List<ps> data;
}

[System.Serializable]                                                     
public struct ps
{
    public float id;
    public float x;
    public float y;
    public float z;
    public float vx;
    public float vy;
    public float vz;
    public float dia;
    public float temp;
    public float time;
    public float den;

}