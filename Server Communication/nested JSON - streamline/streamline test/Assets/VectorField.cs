using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorField : MonoBehaviour {

    private List<p0> CFD = new List<p0>();
    static Material lineMaterial;
    public Toggle showVF;
    CFD_JSON c;
    public float r = 0.2f;
    public GameObject proto;
    List<Ball> balls = new List<Ball>();

    void Start()
    {
        c = GetComponent<CFD_JSON>();
        showVF = GameObject.Find("Vector Field").GetComponent<Toggle>();
    }

    void Update()
    {
        foreach (Ball b in balls)
        {
            MoveBall(b.CreateBall(proto, b.start), b.start, b.end, 10);
        }
    }

    public void OnRenderObject()
    {
        if (c.start == true && showVF.isOn == true)
        {
            //CheckData();
            c.waiting.enabled = false;

            CFD = (c.section.isOn == true) ? c.Section() : c.cfd;

            CreateLineMaterial();
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINES);
            //GL.LoadOrtho();
            //GL.LoadPixelMatrix();
           
            foreach (p0 p in CFD)
            {
                float a = CFD_JSON.remap(p.vmeg, c.vMin, c.vMax, 0, 1);
                Color vcolor = new Color(a, 1 - a, 0, 0.5f);
                GL.Color(vcolor);

                Vector3 start = new Vector3(p.x, p.z, p.y);
                Vector3 move = new Vector3(p.vx, p.vz, p.vy);
                Vector3 end = start + r * move;

                Ball ball = new Ball();
                ball.start = start;
                ball.end = end;
                balls.Add(ball);

                GL.Vertex3(start.x, start.y, start.z);
                GL.Vertex3(end.x, end.y, end.z);

                //MoveBall(ball.CreateBall(proto, start), start, end, 10);
            }

            GL.End();
            GL.PopMatrix();
        }
        
        else
        {
            c.waiting.text = "waiting to download data";
            c.waiting.enabled = true;
            //Debug.Log("waiting for connection");
        }
        
    }

    // functions...
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader lineShader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(lineShader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            //shader config: add alpha blending, turn off back face cull and depth writes
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void MoveBall(GameObject ball, Vector3 start, Vector3 end, int step)
    {
        float dist = end.magnitude - start.magnitude;
        float dMove = dist / step;
        Vector3 dir = end - start;

        for(int i =0; i <step; i++)
        {
            Vector3 dTrans = dMove * dir.normalized;
            ball.transform.Translate(dTrans * i);
        }
    }
}

public class Ball
{

    public Vector3 start;
    public Vector3 end;

    public GameObject CreateBall(GameObject proto, Vector3 pos)
    {
        Transform parent = GameObject.Find("Particles").transform;
        GameObject ball = GameObject.Instantiate(proto, pos, Quaternion.identity, parent);
        ball.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        //ballList.Add(ball);
        return ball;
    }
}