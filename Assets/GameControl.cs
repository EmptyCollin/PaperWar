using System.Collections;
using geniikw.DataSheetLab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    private Definitions.GameStatus currentStatus;
    string errorFileLocalion;

    public List<Player> players;

    public Player currentPlayer;

    // get resource number in UI 
    Text player_paperNum;
    Text player_glueNum;
    Text player_populationNum;

    private GameObject units;
    private GameObject notification;

    public Player another;

    // starting paper for player
    private GameObject player_start;
    // starting paper for AI
    private GameObject AI_start;
    

    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        currentStatus = (Definitions.GameStatus)1;
        //SwitchStatus(1);
        errorFileLocalion = "Error in GameControl class, ";
        units = GameObject.Find("Unit(s)");
        currentPlayer = new Player(0,Color.red);
        players.Add(currentPlayer);
        //+++++++++++++++++++++++++++++++++++//

        string path = @".\Assets\Resources\Data.csv";
        CSV.GetInstance().LoadFile(path);
        currentPlayer.FillUnitDataSet(CSV.GetInstance());
        print(currentPlayer.FindUnitData(10).canBuild.Count);
        //+++++++++++++++++++++++++++++++++++//

        // Initialize
        player_start = GameObject.Find("Paper2");
        Vector3 Ppos = new Vector3(player_start.transform.position.x+10f, -3.5f, player_start.transform.position .z- 50f);
        
        CreateUnit(currentPlayer.playerID, currentPlayer, Ppos ,0);
        CreateUnit(currentPlayer.playerID, currentPlayer, Ppos + new Vector3(30f, 1f, -40f),10);
        CreateUnit(currentPlayer.playerID, currentPlayer, Ppos + new Vector3(40f, 1f, -40f),10);
        CreateUnit(currentPlayer.playerID, currentPlayer, Ppos + new Vector3(50f, 1f, -40f),10);

        // camera
        Camera.main.transform.position = new Vector3(Ppos.x, Camera.main.transform.position.y, Ppos.z);

        // AI player
        another = new Player(1, Color.blue);
        players.Add(another);
        
        //+++++++++++++++++++++++++++++++++++//
        another.FillUnitDataSet(CSV.GetInstance());
        //+++++++++++++++++++++++++++++++++++//
        AI_start = GameObject.Find("Paper5");
        Vector3 AIpos = new Vector3(AI_start.transform.position.x - 30f, -3.5f, AI_start.transform.position.z - 50f);
        CreateUnit(another.playerID, another, AIpos , 0);
        CreateUnit(another.playerID, another, AIpos + new Vector3(30f, 0, -20f), 10);
        CreateUnit(another.playerID, another, AIpos + new Vector3(40f, 0, -20f), 10);
        CreateUnit(another.playerID, another, AIpos + new Vector3(50f, 0, -20f), 10);
        //InitialPlane(another.playerID, another);



        // Find resourse UI 
        player_paperNum = GameObject.Find("Paper_number").gameObject.GetComponent<Text>();
        player_glueNum = GameObject.Find("Glue_number").gameObject.GetComponent<Text>();
        player_populationNum = GameObject.Find("Population_number").gameObject.GetComponent<Text>();
        notification = GameObject.Find("Notificaiton");
    }

    internal void Research(int who, int id)
    {
        switch (id) {
            case 100:
                AddMaxHP(who);
                break;

            case 101:
                IncreaseAttackDamage(who);
                break;

            case 102:
                IncreaseAttackSpeed(who); 
                break;

            case 103:
                IncreaseMovingSpeed(who);
                break;
        }
    }

    private void AddMaxHP(int who)
    {
        foreach (var unit in players[who].ControlledUnit) {
            unit.GetComponent<Unit_info>().hp *= 1.2;
            unit.GetComponent<Unit_info>().maxHp *= 1.2;
        }

        foreach (var data in players[who].unitDatas) {
            data.maxHp *= 1.2f;
        }
    }

    private void IncreaseAttackDamage(int who)
    {
        foreach (var unit in players[who].ControlledUnit)
        {
            unit.GetComponent<Unit_info>().attDmg *= 1.2;
        }

        foreach (var data in players[who].unitDatas)
        {
            data.attDmg *= 1.2f;
        }
    }

    private void IncreaseMovingSpeed(int who)
    {
        foreach (var unit in players[who].ControlledUnit)
        {
            unit.GetComponent<Unit_info>().moveSpeed *= 1.2;
        }

        foreach (var data in players[who].unitDatas)
        {
            data.moveSpeed *= 1.2f;
        }
    }

    private void IncreaseAttackSpeed(int who)
    {
        foreach (var unit in players[who].ControlledUnit)
        {
            unit.GetComponent<Unit_info>().attSpeed *= 1.2;
        }

        foreach (var data in players[who].unitDatas)
        {
            data.attSpeed *= 1.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Win 
        if (another.BaseList.Count < 1) {
            SceneManager.LoadSceneAsync(2);
        }
        // Lose
        if (currentPlayer.BaseList.Count < 1) {
            SceneManager.LoadSceneAsync(3);
        }

        //============test================
        if (currentStatus == Definitions.GameStatus.Ingame) {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        //======================================

        if (true) {
            //units.SetActive(false);
        }
        
        // update resource
        player_paperNum.text = currentPlayer.PaperNum.ToString();
        player_glueNum.text = currentPlayer.GlueNum.ToString();
        player_populationNum.text = (currentPlayer.FarmerList.Count + currentPlayer.FootmanList.Count + currentPlayer.GunManList.Count + currentPlayer.CannonList.Count * 2 + currentPlayer.FlyList.Count).ToString();


        // Test AI
        AllPlayerUpate();
    }

    public void SwitchStatus(Definitions.GameStatus status) {
        currentStatus = status;
    }

    public void StatusCheck() {
        switch (currentStatus) {
            case Definitions.GameStatus.Ingame:
                if (IngameScreen() <0) {
                    Debug.Log(errorFileLocalion + "IngameScreen method. returning error");
                }
                break;
            case Definitions.GameStatus.Meau:
                if (MeauScreen() < 0)
                {
                    Debug.Log(errorFileLocalion + "MeauScreen method. returning error");
                }
                break;

            case Definitions.GameStatus.EndScreen:
                if (EndScreen() < 0)
                {
                    Debug.Log(errorFileLocalion + "EndScreen method. returning error");
                }
                break;
            default:
                Debug.Log("Error in StatusCheck class, StatusCheck method. did not find status in cases");
                break;
        }

    }

    public int IngameScreen() {
        //1 is running correct, <0 will be catch error
        return 1;
    }

    public int MeauScreen() {
        return 1;
    }

    public void ResourceConsume(int who, int id)
    {
        //return true;    // testing welfare
        // paper
        if (players[who].FindUnitData(id).costPaper > players[who].PaperNum) {
            Notification("There isn't enough paper", who);
            return;
        }
        if (players[who].FindUnitData(id).costGule > players[who].GlueNum)
        {
            Notification("There isn't enough paper", who);
            return;
        }
        players[who].PaperNum -= players[who].FindUnitData(id).costPaper;
        players[who].GlueNum -= players[who].FindUnitData(id).costGule;
    }

    private void Notification(string v, int who)
    {
        if (who == 0)
        {
            notification.GetComponent<Notificaiton>().ChangeContent(v,players[who].color);
        }
    }

    public int EndScreen() {
        return 1;
    }

   
    public void CreateUnit(int who, Player player, Vector3 pos, int id)
    {
        Definitions.UnitData unit = player.FindUnitData(id);
        if (unit == null)
        {
            Debug.Log("No such id");
            return;
        }

        GameObject prefab = Instantiate(Resources.Load(unit.prefabPath)) as GameObject;
        prefab.transform.position = pos+new Vector3(0,1,0);
        prefab.GetComponent<Unit_info>().CreateNewUnit(who, unit);
        Definitions.ChangeColor(prefab, players[who].color);
        player.addControlled(prefab);

        // stop particle when inistialzing
        if (prefab.transform.Find("ParticleSystem") != null) {
            prefab.transform.Find("ParticleSystem").gameObject.GetComponent<ParticleSystem>().Stop();
        }

        // all unit add to their own list
        switch (id)
        {
            case 0:
                player.BuildBase(prefab);
                break;
            case 2:
                player.BuildBarrier1(prefab);
                break;
            case 3:
                player.BuildBarrier2(prefab);
                break;
            case 10:
                player.AddFarmerToList(prefab);
                break;
            case 11:
                player.AddFootManToList(prefab);
                break;
            case 12:
                player.AddGunManToList(prefab);
                break;
            case 13:
                player.AddCannonToList(prefab);
                break;
            case 14:
                player.AddFlyToList(prefab);
                break;

            default:
                break;
        }


        // Minimap icon
        GameObject miniIcon = Instantiate(Resources.Load("Models/MiniMapicons")) as GameObject;
        // larger for building

        if (id >= 0 && id <= 9)
        {
            miniIcon.transform.localScale = new Vector3(35f, 1.0f, 35f);
        }
        else {
            miniIcon.transform.localScale = new Vector3(20f, 1.0f, 20f);
        }
        miniIcon.GetComponent<Renderer>().material.color = players[who].color;
        miniIcon.transform.position = prefab.transform.position + new Vector3(0, 30, 0);
        miniIcon.transform.SetParent(prefab.transform);

    }

    public bool CheckResources(int who, int id)
    {
        if (players[who].FindUnitData(id).costPaper > players[who].PaperNum)
        {
            Notification("There isn't enough paper", who);
            return false;
        }
        if (players[who].FindUnitData(id).costGule > players[who].GlueNum)
        {
            Notification("There isn't enough paper", who);
            return false;
        }
        return true;
    }

    
    void AllPlayerUpate() {
        foreach (Player who in players) {
            who.AiUpdate();
        }
    }
    

}
