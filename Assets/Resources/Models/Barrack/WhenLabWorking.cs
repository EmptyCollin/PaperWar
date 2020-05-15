using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenLabWorking : MonoBehaviour
{
    // Start is called before the first frame update
    Unit_info info;
    GameObject g1, g2, g3;
    float rotateSpeed;
    void Start()
    {
        info = GetComponent<Unit_info>();
        g1 = transform.Find("Gear1").gameObject;
        g2 = transform.Find("Gear2").gameObject;
        g3 = transform.Find("Gear3").gameObject;
        rotateSpeed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (info.state == Unit_info.State.Working) {
            g1.transform.RotateAround(g1.transform.position, g1.transform.forward, rotateSpeed);
            g2.transform.RotateAround(g2.transform.position, g2.transform.forward, -rotateSpeed);
            g3.transform.RotateAround(g3.transform.position, g3.transform.forward, 2*rotateSpeed);
        }
    }
}
