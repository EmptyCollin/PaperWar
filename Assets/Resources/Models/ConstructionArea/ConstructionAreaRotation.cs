using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionAreaRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int signal1 = Mathf.Abs(Mathf.Sin(Time.time)) >= 0.5 ? 1 : 0;
        int signal2 = Mathf.Sin(Time.time) >= 0 ? 1 : -1;
        Quaternion r = Quaternion.AngleAxis(Time.deltaTime*signal1*signal2*45, new Vector3(0, 0, 1));

        transform.rotation *= r;
    }
}
