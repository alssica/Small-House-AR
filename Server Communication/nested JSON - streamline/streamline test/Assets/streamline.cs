using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class streamline : MonoBehaviour {

    private CFD_JSON data;
    public GameObject proto;
    List<ps>[] psL;
    bool start;

    void Start()
    {
        data = GetComponent<CFD_JSON>();
        //psL = data.ProcessData(data.cfd);
    }

    void Update()
    {
        start = (psL != null)? true: false;
        MoveParticle();
    }

    void MoveParticle()
    {
        List<GameObject> initialP = new List<GameObject>();

        if (start == true)
        {
            for (int id = 0; id < psL.Length; id++)                 //gives all ps with the same ID
            {
                foreach (ps point in psL[id])            //cycle through time
                {
                    if (point.time == 0)                //time++?
                    {
                       initialP.Add(CreateBall(proto, new Vector3(point.x, point.y, point.z))); 
                    }
                }
            }
        }

    }

    GameObject CreateBall (GameObject proto, Vector3 pos)
    {
        Transform parent = GameObject.Find("Particles").transform;
        GameObject ball = Instantiate(proto, pos, Quaternion.identity, parent);
        //ballList.Add(ball);
        return ball;
    }
}
