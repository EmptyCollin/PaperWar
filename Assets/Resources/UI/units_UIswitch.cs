using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class units_UIswitch : MonoBehaviour
{
    // = true if player select one unit     =   UI of the unit
    // = false if player select more unit   =   UI of formation  
    bool JustOneUnitSelect;

    GameObject SingleUI;
    GameObject SingleImage;
    GameObject AttackNum;
    GameObject SpeedNum;
    GameObject AttackSpeed;
    private GameObject ReplaceCursor;
    GameObject UnitOutline;
    GameObject FormationUI;

    GameObject HealthBar;
    GameObject HealthBarText;
    GameObject UnitControlPanel;

    Player currPlayer;
    bool ifShowAbility = false;
    bool ifShowFormation = false;

    private GameObject gc;

    // Start is called before the first frame update
    void Start()
    {
        SingleUI = transform.Find("SingleUnitUI").gameObject;
        SingleImage = GameObject.Find("Unit_image");
        AttackNum = GameObject.Find("Attack_Number");
        SpeedNum = GameObject.Find("Speed_Number");
        AttackSpeed = GameObject.Find("AttackSpeed_Number");

        UnitOutline = transform.Find("Unit(s)_Outline").gameObject;

        FormationUI = transform.Find("FormationUI").gameObject;

        HealthBar = transform.Find("HealthBar").gameObject;
        HealthBarText = transform.Find("HealthBar_Text").gameObject;
        UnitControlPanel = transform.Find("UnitControlPanel").gameObject;

        currPlayer = GameObject.Find("GameController").GetComponent<GameControl>().currentPlayer;
        ReplaceCursor = GameObject.Find("ReplaceCursor");
        ifShowAbility = false;

        gc = GameObject.Find("GameController");

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currPlayer.getSelected().Count == 0)
        {
            FormationUI.SetActive(false);
            SingleUI.SetActive(false);
            UnitOutline.SetActive(false);
            HealthBar.SetActive(false);
            HealthBarText.SetActive(false);
            UnitControlPanel.SetActive(false);
            if (ifShowAbility)
            {
                HideAbilities();
            }
            if (ifShowFormation)
            {
                HideFormation();
            }

            return;
        }

        // single unit
        if (currPlayer.getSelected().Count == 1)
        {
            FormationUI.SetActive(false);
            SingleUI.SetActive(true);
            UnitOutline.SetActive(true);
            HealthBar.SetActive(true);
            HealthBarText.SetActive(true);
            UnitControlPanel.SetActive(true);

            AttackNum.GetComponent<TMP_Text>().text = currPlayer.getSelected()[0].GetComponent<Unit_info>().attDmg.ToString();
            SpeedNum.GetComponent<TMP_Text>().text = currPlayer.getSelected()[0].GetComponent<Unit_info>().moveSpeed.ToString();
            AttackSpeed.GetComponent<TMP_Text>().text = currPlayer.getSelected()[0].GetComponent<Unit_info>().attSpeed.ToString();
            string precent;
            precent = ((int)(currPlayer.getSelected()[0].GetComponent<Unit_info>().hp)).ToString() +"/"+ ((int)currPlayer.getSelected()[0].GetComponent<Unit_info>().maxHp).ToString();
            HealthBarText.GetComponent<TMP_Text>().text = precent;
            HealthBar.GetComponent<HealthBar_FillControl>().Fill = currPlayer.getSelected()[0].GetComponent<Unit_info>().hp/ currPlayer.getSelected()[0].GetComponent<Unit_info>().maxHp;

            if (!ifShowAbility)
            {
                ShowAbilities(currPlayer.getSelected()[0]);
            }
            if (ifShowFormation)
            {
                HideFormation();
            }
        }

        // multiple units
        else if (currPlayer.getSelected().Count > 1)
        {
            FormationUI.SetActive(true);
            SingleUI.SetActive(false);
            UnitOutline.SetActive(true);
            HealthBar.SetActive(false);
            HealthBarText.SetActive(false);
            UnitControlPanel.SetActive(true);
            if (ifShowAbility)
            {
                HideAbilities();
            }

            if (!ifShowFormation)
            {
                showFormation(currPlayer.getSelected());
            }
        }

        // none selection
        
    }

    public void showFormation(List<GameObject> selected)
    {
        ifShowFormation = true;

        int xoffset = 1;
        int yoffset = 1;
        float xgap = 100f;
        float ygap = -100f;
        int counter = 0;
        foreach (GameObject unit in selected)
        {
            counter += 1;

            GameObject prefab = Instantiate(Resources.Load("Icons/UnitesPrefab"), FormationUI.transform) as GameObject;
            
            prefab.transform.position = FormationUI.transform.position + new Vector3(-300 + xoffset * xgap, 200+yoffset*ygap, 0);
            string icon = gc.GetComponent<GameControl>().players[unit.GetComponent<Unit_info>().belongTo].FindUnitData(unit.GetComponent<Unit_info>().ID).iconPath;
            prefab.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(icon);

            if (counter % 5 == 0) { yoffset += 1; xoffset = 1; }
            else { xoffset += 1; }
            
        }
    }

    public void ShowAbilities(GameObject gameObject)
    {
        HideAbilities();
        ifShowAbility = true;

        List<int> canBuild = gameObject.GetComponent<Unit_info>().canBuild;
        List<int> canCreate = gameObject.GetComponent<Unit_info>().canCreate;
        List<int> canResearch = gameObject.GetComponent<Unit_info>().canResearch;

        int offset = 0;
        float gap = 105f;
        for (int i = 0; i < canBuild.Count; i++) {
            GameObject prefab = Instantiate(Resources.Load("UI/Ability"), UnitControlPanel.transform) as GameObject;
            prefab.transform.position = UnitControlPanel.transform.position + new Vector3(offset * gap, 0, 0);
            prefab.GetComponent<AbilitiesButton>().Record(gameObject.GetComponent<Unit_info>().belongTo,canBuild[i],gameObject.GetComponent<Unit_info>().ID);
            prefab.GetComponent<Button>().onClick.AddListener(delegate { ToBuild(prefab.GetComponent<AbilitiesButton>()); });
            offset++;
        }

        for (int i = 0; i < canCreate.Count; i++)
        {
            GameObject prefab = Instantiate(Resources.Load("UI/Ability"), UnitControlPanel.transform) as GameObject;
            prefab.transform.position = UnitControlPanel.transform.position + new Vector3(offset * gap, 0, 0);
            prefab.GetComponent<AbilitiesButton>().Record(gameObject.GetComponent<Unit_info>().belongTo, canCreate[i], gameObject.GetComponent<Unit_info>().ID);
            prefab.GetComponent<Button>().onClick.AddListener(delegate { ToCreate(prefab.GetComponent<AbilitiesButton>()); });
            offset++;
        }

        for (int i = 0; i < canResearch.Count; i++)
        {
            GameObject prefab = Instantiate(Resources.Load("UI/Ability"), UnitControlPanel.transform) as GameObject;
            prefab.transform.position = UnitControlPanel.transform.position + new Vector3(offset * gap, 0, 0);
            prefab.GetComponent<AbilitiesButton>().Record(gameObject.GetComponent<Unit_info>().belongTo, canResearch[i], gameObject.GetComponent<Unit_info>().ID);
            prefab.GetComponent<Button>().onClick.AddListener(delegate { ToResearch(prefab.GetComponent<AbilitiesButton>());});
            offset++;
        }

    }

    public void HideAbilities() {
        ifShowAbility = false;
        for (int a = UnitControlPanel.transform.childCount-1; a>=0;a--) {
            if (UnitControlPanel.transform.GetChild(a).tag == "AbilityButtom") {
                Destroy(UnitControlPanel.transform.GetChild(a).gameObject);
            }

        }
    }

    public void HideFormation()
    {
        ifShowFormation = false;
        for (int a = FormationUI.transform.childCount - 1; a >= 0; a--)
        {
            if (FormationUI.transform.GetChild(a).name != "Formation_Outline")
            {
                Destroy(FormationUI.transform.GetChild(a).gameObject);
            }

        }
    }

    public void ToBuild(AbilitiesButton abilitiesButton) {
        if (gc.GetComponent<GameControl>().CheckResources(abilitiesButton.who,abilitiesButton.target)) {
            gc.GetComponent<GameControl>().players[abilitiesButton.who].SelectedUnit[0].
                GetComponent<BuildControl>().OnBuild(abilitiesButton.who, abilitiesButton.target);
            if (abilitiesButton.unitType >= 0 && abilitiesButton.unitType < 10)
            {
                gc.GetComponent<GameControl>().players[abilitiesButton.who].SelectedUnit[0].GetComponent<Unit_info>().CleanProcessQueue();
            }
            else {
                ReplaceCursor.GetComponent<CursorReplace>().Replace();
            }
        }
    }

    public void ToCreate(AbilitiesButton abilitiesButton)
    {
        if (gc.GetComponent<GameControl>().CheckResources(abilitiesButton.who, abilitiesButton.target))
        {
            gc.GetComponent<GameControl>().players[abilitiesButton.who].SelectedUnit[0].GetComponent<Unit_info>().processingQueue.Enqueue(abilitiesButton.target);
            gc.GetComponent<GameControl>().ResourceConsume(abilitiesButton.who, abilitiesButton.target);
        }
    }

    public void ToResearch(AbilitiesButton abilitiesButton)
    {
        if (gc.GetComponent<GameControl>().CheckResources(abilitiesButton.who, abilitiesButton.target))
        {
            gc.GetComponent<GameControl>().players[abilitiesButton.who].SelectedUnit[0].GetComponent<Unit_info>().processingQueue.Enqueue(abilitiesButton.target);
            gc.GetComponent<GameControl>().ResourceConsume(abilitiesButton.who, abilitiesButton.target);
            gc.GetComponent<GameControl>().players[abilitiesButton.who].FindUnitData(1).canResearch.Remove(abilitiesButton.target);
            HideAbilities();
        }
    }
}
