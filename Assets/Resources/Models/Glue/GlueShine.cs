using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueShine : MonoBehaviour
{
    public Material mat;
    private GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        mat.SetVector("Center", transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetVector("_CameraPos", mainCamera.transform.position);
        double malti = Math.Abs(Math.Sin(Time.realtimeSinceStartup/4))/2;
        mat.SetFloat("_Malti", (float)malti);
    }
}
