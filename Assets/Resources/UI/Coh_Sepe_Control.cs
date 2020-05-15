using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coh_Sepe_Control : MonoBehaviour
{
    private GameObject gc;

    public Sprite cohesion;
    public Sprite seperation;

    void Start()
    {
        gc = GameObject.Find("GameController");
    }

    public void Switch_CorS()
    {
        List<GameObject> selected = gc.GetComponent<GameControl>().currentPlayer.getSelected();

        // set all units in list to the first object in list's state
        bool setTo = selected[0].GetComponent<Movement>().isCohesion;

        // switch Icon
        if (setTo == true) // is cohesion
        {
            gameObject.GetComponent<Image>().sprite = seperation;
        }
        else               // is seperation
        {
            gameObject.GetComponent<Image>().sprite = cohesion;
        }



        foreach (GameObject unit in selected)
        {
            unit.GetComponent<Movement>().isCohesion = !setTo;
        }
        
    }

    
}
