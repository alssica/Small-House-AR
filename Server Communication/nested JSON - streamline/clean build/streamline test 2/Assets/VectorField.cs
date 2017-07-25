﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorField : MonoBehaviour {

    private List<ps> CFD = new List<ps>();
    static Material lineMaterial;
    public Toggle showVF;
    CFD c;
    public float r=0.2f;

    void Start()
    {
        c = GetComponent<CFD>();
        showVF = GameObject.Find("Vector Field").GetComponent<Toggle>();
    }

    public void OnRenderObject()
    {
        if (c.start == true && showVF.isOn == true)
        {
            //CheckData();
            c.waiting.enabled = false;

            //CFD = (c.section.isOn == true) ? c.Section() : c.cfd;
            CFD = 

            CreateLineMaterial();
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINES);

            foreach (p0 p in CFD)
            {
                float a = CFD_JSON.remap(p.vmeg, c.vMin, c.vMax, 0, 1);
                Color vcolor = new Color(a, 1 - a, 0, 0.5f);
                GL.Color(vcolor);

                Vector3 start = new Vector3(p.x, p.z, p.y);
                Vector3 move = new Vector3(p.vx, p.vz, p.vy);
                Vector3 end = start + r * move;

                GL.Vertex3(start.x, start.y, start.z);
                GL.Vertex3(end.x, end.y, end.z);
            }

            GL.End();
            GL.PopMatrix();
        }

        /*
        else
        {
            c.waiting.text = "waiting to download data";
            c.waiting.enabled = true;
            //Debug.Log("waiting for connection");
        }
        */
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
}
