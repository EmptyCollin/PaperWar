using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUnit : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject gc;
    void Start()
    {
        gc = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G) && GetComponent<Unit_info>().isSelect) {
            GameObject prefab = Instantiate(Resources.Load("Models/Farmer/Farmer")) as GameObject;
            //Instantiate(prefab);
            prefab.transform.position =transform.position + new Vector3(3f, 0, -3f);
            prefab.GetComponent<Unit_info>().CreateFamer(gc.GetComponent<GameControl>().currentPlayer.playerID);

            // add to curPlayer controlled
            gc.GetComponent<GameControl>().currentPlayer.addControlled(prefab);
        }
    }

}
