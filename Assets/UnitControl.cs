using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitControl : MonoBehaviour
{
    public GameObject selection;
    public GameObject healthBar = null;
    public int buildingSpeed = 100;
    public GameObject processingBar = null;

    // Start is called before the first frame update
    void Start()
    {
        selection = this.transform.Find("SelectionIndicator").gameObject;
        try
        {
            healthBar = this.transform.Find("HealthBar").gameObject;
        }
        catch { }

        try
        {
            processingBar = this.transform.Find("ProcessingBar").gameObject;
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Unit_info>().isSelect == true)
        {
            try
            {
                transform.Find("miniImage").gameObject.SetActive(true);
            }
            catch { }

            selection.SetActive(true);
        }
        else
        {
            try
            {
                transform.Find("miniImage").gameObject.SetActive(false);
            }
            catch { }
            selection.SetActive(false);
        }

        if (healthBar)
        {
            healthBar.GetComponent<HealthBar_FillControl>().Fill = GetComponent<Unit_info>().hp / GetComponent<Unit_info>().maxHp;
        }

        if (processingBar != null)
        {
            if (GetComponent<Unit_info>().currentItem != -999)
            {
                processingBar.SetActive(true);
                processingBar.GetComponent<ProcessingBar_FillControl>().Fill = GetComponent<Unit_info>().currentProcessing;
                processingBar.transform.Find("Item").GetComponent<RawImage>().texture = GetComponent<Unit_info>().itemText;
                processingBar.transform.Find("Number").GetComponent<TMP_Text>().text = GetComponent<Unit_info>().processingQueue.Count.ToString();
            }
            else
            {
                processingBar.SetActive(false);
            }
        }


        if (this.GetComponent<Unit_info>().goingToBe != -999)
        {

            this.GetComponent<Unit_info>().hp += buildingSpeed * Time.deltaTime;

            if (this.GetComponent<Unit_info>().hp >= this.GetComponent<Unit_info>().maxHp)
            {

                GameObject gc = GameObject.Find("GameController");

                // create tobe
                int tobe = this.GetComponent<Unit_info>().goingToBe;

                gc.GetComponent<GameControl>().CreateUnit(this.GetComponent<Unit_info>().belongTo, gc.GetComponent<GameControl>().players[this.GetComponent<Unit_info>().belongTo], this.transform.position, tobe);

                gc.GetComponent<GameControl>().currentPlayer.removeControlled(gameObject);
                if (gc.GetComponent<GameControl>().players[GetComponent<Unit_info>().belongTo].SelectedUnit.Contains(gameObject)) {
                    gc.GetComponent<GameControl>().players[GetComponent<Unit_info>().belongTo].removeSelected(gameObject);
                }
                
                Vector3 currPos = this.transform.position;
                Vector3 farmer_Pos = currPos + new Vector3(20, 1, 10);
                if (this.GetComponent<Unit_info>().goingToBe != 1)
                {
                    gc.GetComponent<GameControl>().CreateUnit(this.GetComponent<Unit_info>().belongTo, gc.GetComponent<GameControl>().players[this.GetComponent<Unit_info>().belongTo], farmer_Pos, 10);
                }

                Destroy(gameObject);

            }
        }


    }

}
