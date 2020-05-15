using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenCannonMoving : MonoBehaviour
{

    Unit_info info;
    GameObject w1, w2;
    float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<Unit_info>();
        w1 = transform.Find("AxisAdjust").Find("wheel_left").gameObject;
        w2 = transform.Find("AxisAdjust").Find("wheel_right").gameObject;
        rotateSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (info.state == Unit_info.State.Moving)
        {
            w1.transform.RotateAround(w1.transform.position, w1.transform.forward, -rotateSpeed);
            w2.transform.RotateAround(w2.transform.position, w2.transform.forward, -rotateSpeed);

        }
    }
}
