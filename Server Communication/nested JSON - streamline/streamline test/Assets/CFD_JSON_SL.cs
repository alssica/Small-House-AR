using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

//[System.Serializable]
public class CFD_JSON_SL : MonoBehaviour
{
    public List<ps> cfd = new List<ps>();                                   
    public Slider slider;
    public Text waiting;
    public Toggle section;
    public bool start;
    int N;
    //public bool showSection;

    IEnumerator Start()
    {
        slider = GetComponentInChildren<Slider>();
        section = GetComponentInChildren<Toggle>();
        section.isOn = false;
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
            cfd = ArrayJsonStreamline.streamlineJsonArray<ps>(www.text);
            Debug.Log(cfd[0]);
            //ProcessData(cfd);
            ParseID(cfd);
        }

        //CheckData();
    }

    public void ParseID(List<ps> psList)
    {
        if (psList != null)
        {
            List<float> IDlist = new List<float>();

            foreach (ps point in psList)
            {
                //IDlist.Add(point.);
                //Debug.Log(point.time);
                Debug.Log(point.sb);

            }
            //print(IDlist.Max());
            //Debug.Log(IDlist[IDlist.Count-1]);
            waiting.enabled = false;
        }
        else 
        {
            waiting.text = "loading";
            waiting.enabled = true;
            //yield return start = false;
        }
    }

    /*
    public List<ps>[] ProcessData(List<ps> list)
    {      
        if (list.Count != 0)
        {
            waiting.enabled = false;
            int[] ID = new int[list.Count];

            for (int i=0; i< list.Count; i++)
            {
                ID[i] = list[i].ID;
            }

            N = ID.Max();

            List<ps>[] ArrayofList = new List<ps>[N];

            for (var i=0; i < list.Count; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    if (list[i].ID == j)
                    {
                        ArrayofList[j].Add(list[i]);
                    }
                }
            }
            return ArrayofList;
        }
        else
        {
            waiting.text = "loading";
            waiting.enabled = true;
            //yield return start = false;
            return null;
        }
    }
    */

    public static float remap(float x, float dataMin, float dataMax, float targetMin, float targetMax)
    {
        return targetMin + (targetMax - targetMin) * (x - dataMin) / (dataMax - dataMin);
    }
}


//............................JSON Parsing..............................
[System.Serializable]
public class ArrayJsonStreamline
{
    public static List<ps> streamlineJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";                          //put a property wrapper around the original array
        var wrapper = JsonUtility.FromJson<Wrapper>(newJson);

        return wrapper.array;
     }

    [System.Serializable]
    private class Wrapper
    {
        public List<ps> array = new List<ps>();
    }
}

[System.Serializable]                                                       //should i keep these inside arrayjson or out???
public struct ps
{
    public float time;
    public float x;
    public float y;
    public float z;
    public float vx;
    public float vy;
    public float vz;
    public float dia;
    public float temp;
    public float den;
    public float sb;
}




