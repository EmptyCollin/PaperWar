using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public double targetSize;
    public float speed;
    public double dmg;

    // need game controller here to update resource
    public GameObject gc;

    void Start()
    {
        speed = 30f;
        //dmg = 0;
        targetSize = 6;

        gc = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
       
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        Colliorcheck();
    }

    public void Colliorcheck()
    {
        var dist = Vector3.Distance(target.transform.position, transform.position);
        if (dist < targetSize)
        {
            kill();

        }
    }

    public void kill()
    {
        // target decrase health here;
        
        

        target.GetComponent<Unit_info>().hp -= dmg;
        Destroy(gameObject);

    }
}