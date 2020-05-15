using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildControl : MonoBehaviour
{
    // current player
    List<Player> players;

    GameObject gc;

    // userInput
    GameObject userIN;

    bool building = false;
    public int currentBuilding = -999;
    private GameObject ReplaceCursor;
    // record current doing building
    GameObject Farmer;
    GameObject Construction;
    public Vector3 Tar;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.Find("GameController").gameObject.GetComponent<GameControl>().players;

        userIN = GameObject.Find("Userinput").gameObject;
        ReplaceCursor = GameObject.Find("ReplaceCursor");
        gc = GameObject.Find("GameController");
        Tar = new Vector3(-999,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        // choose building position
        if (building == true)
        {
            if(GetComponent<Unit_info>().ID>=10)
            userIN.GetComponent<UserInput>().enabled = false;

            if (currentBuilding != -999)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    Physics.Raycast(ray, out hit);
                    Tar = hit.point;
                    userIN.GetComponent<UserInput>().resetStartPos();
                    userIN.GetComponent<UserInput>().enabled = true;
                    building = false;
                    ReplaceCursor.GetComponent<CursorReplace>().Restore();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    userIN.GetComponent<UserInput>().enabled = true;
                    building = false;
                    ReplaceCursor.GetComponent<CursorReplace>().Restore();

                }
            }
        }


        if (Tar.x != -999)
        {
            if (Vector3.Distance(this.transform.position, Tar) >= 7)
            {
                this.GetComponent<Movement>().MoveTo(Tar);
            }
            else
            {
                CreateConstruction(Tar, currentBuilding);
                Tar.x = -999;
                //building = false;
                //userIN.GetComponent<UserInput>().enabled = true;
                // always remove farmer when building
                players[GetComponent<Unit_info>().belongTo].removeSelected(gameObject);
                players[GetComponent<Unit_info>().belongTo].removeControlled(gameObject);

                players[GetComponent<Unit_info>().belongTo].RemoveFarmerTolist(gameObject);
                Destroy(gameObject);
            }
        }
        
        // construction upgrade
        else if (Construction != null) {
            CreateConstruction(Tar, currentBuilding);
            Tar.x = -999;
            building = false;
            userIN.GetComponent<UserInput>().enabled = true;
            players[GetComponent<Unit_info>().belongTo].clearSelect();
            players[GetComponent<Unit_info>().belongTo].removeControlled(Construction);
            Destroy(Construction);
            Construction = null;
        }
        
        
    }

    public void OnBuild(int who, int id)
    {
        Player p = gc.GetComponent<GameControl>().players[who];
        currentBuilding = id;
        building = true;
        
        // character build constructure
        if (p.SelectedUnit[0].GetComponent<Unit_info>().ID >= 10)
        {
            Farmer = p.SelectedUnit[0];
        }

        // construction upgrade
        else {
            Construction = p.SelectedUnit[0];
            Tar = p.SelectedUnit[0].transform.position;
        }
        
        

        
    }

    private void CreateConstruction(Vector3 pos, int ToBe)
    {
        
        GameObject prefab = Instantiate(Resources.Load("Models/ConstructionArea/ConstructionArea")) as GameObject;

        prefab.transform.position = pos;

        prefab.GetComponent<Unit_info>().CreateConstructionArea(players[GetComponent<Unit_info>().belongTo].playerID, ToBe);
        Definitions.ChangeColor(prefab, players[GetComponent<Unit_info>().belongTo].color);
        players[GetComponent<Unit_info>().belongTo].addControlled(prefab);
        gc.GetComponent<GameControl>().ResourceConsume(GetComponent<Unit_info>().belongTo, currentBuilding);
    }


}
