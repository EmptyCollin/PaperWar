using System;
using System.Collections;
using geniikw.DataSheetLab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player
{
    public List<GameObject> ControlledUnit;
    public List<GameObject> SelectedUnit;
    public int playerID;
    public Color color;
    private GameObject gc;

    // resources record **********************************
    public double PaperNum;
    public double GlueNum;
    public double PopulationNum;
    // ***************************************************

    // lists
    public List<GameObject> FarmerList;
    public List<GameObject> FootmanList;
    public List<GameObject> GunManList;
    public List<GameObject> CannonList;
    public List<GameObject> FlyList;
    public List<GameObject> BaseList;
    public List<GameObject> Barr1;
    public List<GameObject> Barr2;
    private int nextToCreate;
    private int nextToBuild;

    private List<GameObject> resoursePaper;
    private GameObject Glue;
    

    public List<Definitions.UnitData> unitDatas = new List<Definitions.UnitData>();

    public Player(int id, Color c)
    {
        playerID = id;
        color = c;
        ControlledUnit = new List<GameObject>();
        SelectedUnit = new List<GameObject>();

        PaperNum = 500;
        GlueNum = 100;
        PopulationNum = 0;

        gc = GameObject.Find("GameController");
        FarmerList = new List<GameObject>();
        FootmanList = new List<GameObject>();
        GunManList = new List<GameObject>();
        CannonList = new List<GameObject>();
        FlyList = new List<GameObject>();
        BaseList = new List<GameObject>();
        Barr1 = new List<GameObject>();
        Barr2 = new List<GameObject>();

        resoursePaper = new List<GameObject>();
        resoursePaper.Add(GameObject.Find("Paper1"));
        resoursePaper.Add(GameObject.Find("Paper2"));
        resoursePaper.Add(GameObject.Find("Paper3"));
        resoursePaper.Add(GameObject.Find("Paper4"));
        resoursePaper.Add(GameObject.Find("Paper5"));
        resoursePaper.Add(GameObject.Find("Paper6"));
        Glue = GameObject.Find("Glue");
        nextToCreate = -999;
        nextToBuild = -999;
    }

    public List<GameObject> getSelected() { return SelectedUnit; }
    public List<GameObject> getControlled() { return ControlledUnit; }

    public bool Equals(Player other)
    {
        return playerID == other.playerID;
    }
    public void clearSelect()
    {
        for (int i = 0; i < SelectedUnit.Count; i++)
        {
            SelectedUnit[i].GetComponent<Unit_info>().isSelect = false;
            SelectedUnit[i].GetComponent<Movement>().theGroupList = null;
        }

        SelectedUnit.Clear();
    }

   

    public void addSelected(GameObject g)
    {
        SelectedUnit.Add(g);
        g.GetComponent<Unit_info>().isSelect = true;
        ListInfoCheat();
    }

    public void setSelected(List<GameObject> l)
    {
        SelectedUnit = l;
    }

    public void removeSelected(GameObject removed)
    {
        SelectedUnit.Remove(removed);
    }

    public void addControlled(GameObject g)
    {
        ControlledUnit.Add(g);
    }
    // unit die!
    public void removeControlled(GameObject g)
    {
        ControlledUnit.Remove(g);
    }

    public void ListInfoCheat()
    {
        for (int i = 0; i < SelectedUnit.Count; i++)
        {
            SelectedUnit[i].GetComponent<Movement>().theGroupList = SelectedUnit;
        }
    }

    //==========================================================

    bool isBaseExist = true;
    double CurrentPaperHave = 0;

    int MaxFamer = 7;
    int CurrentFamerNumber = 0;

    bool IsBarrier_1_Build = false;
    bool IsBarrier_2_Build = false;
   
    int CurrentAttackerNumber = 0;
    int MaxAttacker = 10;

    int CurrentAttacker2Number = 0;
    int MaxAttacker2 = 10;

    GameObject TargetHumanPlayer; // need player info;
    int PlayerAttackerNumber = 0;
    int DifferentOfAttack = 5;
    //===========================================================================
    float HumanPlayer_Id = 0;
    public void AiUpdate()
    {
        if (playerID != HumanPlayer_Id)
        {
            AiLogic();
        }
    }

    

    public void AddFarmerToList(GameObject farmer) {
        FarmerList.Add(farmer);
    }

    public void RemoveFarmerTolist(GameObject farmer) {
        FarmerList.Remove(farmer);
    }

    public void AddFootManToList(GameObject FootMan)
    {
        FootmanList.Add(FootMan);
    }

    public void RemoveFootManTolist(GameObject FootMan)
    {
        FootmanList.Remove(FootMan);
    }

    public void AddGunManToList(GameObject GunMan)
    {
        GunManList.Add(GunMan);
    }

    public void RemoveGunManTolist(GameObject GunMan)
    {
        GunManList.Remove(GunMan);
    }

    public void AddCannonToList(GameObject Cannon)
    {
        CannonList.Add(Cannon);
    }

    public void RemoveCannonTolist(GameObject Cannon)
    {
        CannonList.Remove(Cannon);
    }

    public void AddFlyToList(GameObject Fly)
    {
        FlyList.Add(Fly);
    }

    public void RemoveFlyTolist(GameObject Fly)
    {
        FlyList.Remove(Fly);
    }

    public void BuildBase(GameObject Base) {
        isBaseExist = true;
        BaseList.Add(Base);
    }

    public void BuildBarrier1(GameObject B1) {
        IsBarrier_1_Build = true;
        Barr1.Add(B1);
    }

    public void BuildBarrier2(GameObject B2)
    {
        IsBarrier_2_Build = true;
        Barr2.Add(B2);
    }

    public void BaseDie(GameObject Base) {
        // GG
        BaseList.Remove(Base);
        if (BaseList == null || BaseList.Count == 0) {
            isBaseExist = false;
        }
    }

    public void Barrier_1_Die(GameObject B1) {
        
        Barr1.Remove(B1);
        if (Barr1 == null || Barr1.Count == 0)
        {
            IsBarrier_1_Build = false;
        }

    }

    public void Barrier_2_Die(GameObject B1)
    {

        Barr2.Remove(B1);
        if (Barr2 == null || Barr2.Count == 0)
        {
            IsBarrier_1_Build = false;
        }
    }


    //==============================================================================
    public void AiLogic() {
        GameInfoUpdate();
        //==================building logic==========================================
        if (isBaseExist && CurrentFamerNumber+process+1 <= MaxFamer && gc.GetComponent<GameControl>().CheckResources(playerID,10)) {
            CreateFamer();
        }
        if (!IsBarrier_1_Build && gc.GetComponent<GameControl>().CheckResources(playerID, 2) && FarmerList.Count > 0) {
            CreateBarrier_1();
        }
        if (!IsBarrier_2_Build && CurrentAttackerNumber > MaxAttacker && gc.GetComponent<GameControl>().CheckResources(playerID, 3) && FarmerList.Count > 0)
        {
            CreateBarrier_2();
        }
        if (IsBarrier_2_Build && CurrentAttacker2Number <= MaxAttacker2 && CurrentAttackerNumber > MaxAttacker)
        {
            CreateAttackTwo();
        }
        if (IsBarrier_1_Build && CurrentAttackerNumber <= MaxAttacker)
        {
            CreateAttackOne();
        }

        //============================Action logic==============================
        if (CurrentFamerNumber > 0) {
            GoFarming();
        }
        if (CurrentAttackerNumber >= MaxAttacker && CurrentAttacker2Number >= MaxAttacker2) {
            //ALL IN
            AllAttackerAttack();
        }
        
        if (((CurrentAttackerNumber + CurrentAttacker2Number) - PlayerAttackerNumber) >= DifferentOfAttack) {
            AllAttackerAttack();
        }
        
    }

    int process;

    public void GameInfoUpdate() {
        // get numbers
        GameObject p = BaseList[0];
        process = p.GetComponent<Unit_info>().processingQueue.Count + 1;
        //update following:
        //isBaseExist = call code
        CurrentPaperHave = PaperNum;
        CurrentFamerNumber = FarmerList.Count;
        //IsBarrier_1_Build = call code
        //IsBarrier_2_Build = call code
        CurrentAttackerNumber = FootmanList.Count+ GunManList.Count+CannonList.Count;
        CurrentAttacker2Number = FlyList.Count;
        PopulationNum = FarmerList.Count + FootmanList.Count + GunManList.Count + CannonList.Count * 2 + FlyList.Count;


        PlayerAttackerNumber = gc.GetComponent<GameControl>().players[0].FootmanList.Count + gc.GetComponent<GameControl>().players[0].GunManList.Count + gc.GetComponent<GameControl>().players[0].CannonList.Count + gc.GetComponent<GameControl>().players[0].FlyList.Count;


    }

    // Serve AI
    public void CreateFamer()
    {
        GameObject Base = BaseList[0];
        ToCreate(10, Base);
    }

    public void CreateBarrier_1()
    {
        if (nextToBuild == -999) {
            nextToBuild = 2;
        }
        if (nextToBuild != -999 && gc.GetComponent<GameControl>().CheckResources(playerID, nextToBuild))
        {
            ToBuild(nextToBuild, FarmerList[0], BaseList[0].transform.position + new Vector3(-10, 0, -30));
            nextToBuild = -999;
        }
    }

    public void CreateBarrier_2()
    {
        if (nextToBuild == -999)
        {
            nextToBuild = 3;
        }
        if (nextToBuild != -999 && gc.GetComponent<GameControl>().CheckResources(playerID, nextToBuild))
        {
            ToBuild(3, FarmerList[0], BaseList[0].transform.position + new Vector3(-40, 0, 0));
            nextToBuild = -999;
        }
    }

    public void CreateAttackOne()
    {

        if (nextToCreate == -999)
        {
            float r = UnityEngine.Random.value;
            if (r <= 0.6)
            {
                nextToCreate = 11;
            }
            else if (r <= 0.95)
            {
                nextToCreate = 12;
            }
            else 
            {
                nextToCreate = 13;
            }
            
        }
        else
        {
            float g = UnityEngine.Random.value;
            if (g <= 0.0001)
            {
                nextToCreate = -999;
            }
        }

        GameObject Barr = Barr1[0];
        if (nextToCreate != -999 && gc.GetComponent<GameControl>().CheckResources(playerID, nextToCreate)){
            ToCreate(nextToCreate, Barr);
            nextToCreate = -999;
        }
    }

    public void CreateAttackTwo()
    {
        GameObject Barr = Barr2[0];
        if (gc.GetComponent<GameControl>().CheckResources(playerID, 14)){
            ToCreate(14, Barr);
        }
    }

    public void GoFarming()
    {
        foreach (GameObject farmer in FarmerList)
        {
            if(farmer.GetComponent<Movement>().currentTarget == null ||
               farmer.GetComponent<Movement>().currentTarget.GetComponent<Unit_info>().ID != 50 ||
               farmer.GetComponent<Movement>().currentTarget.GetComponent<Unit_info>().ID != 51)
            farmer.GetComponent<Movement>().AttactTo(getCloestPaper(farmer.transform.position));
        }
        if ((nextToCreate != -999 && FindUnitData(nextToCreate).costGule > GlueNum) || ((nextToBuild != -999 && FindUnitData(nextToBuild).costGule > GlueNum)))
        {
            if (FarmerList.Count > 0) {
                FarmerList[FarmerList.Count - 1].GetComponent<Movement>().AttactTo(Glue);
                try
                {
                    FarmerList[FarmerList.Count - 2].GetComponent<Movement>().AttactTo(Glue);
                }
                catch {

                }
            }
        }

    }

    public void AllAttackerAttack()
    {
        //List<GameObject> AllAttacker = FootmanList + GunManList + CannonList + FlyList;

        Vector3 enemyPos = gc.GetComponent<GameControl>().players[0].BaseList[0].transform.position; 
        foreach (GameObject Footman in FootmanList)
        {
            // not fighting then moving
            if (Footman.GetComponent<Unit_info>().state != Unit_info.State.Attacking)
            {
                Footman.GetComponent<Movement>().MoveTo(enemyPos);
            }
            Footman.GetComponent<Movement>().AttackNear();
        }
        foreach (GameObject GunMan in GunManList)
        {
            // not fighting then moving
            if (GunMan.GetComponent<Unit_info>().state != Unit_info.State.Attacking)
            {
                GunMan.GetComponent<Movement>().MoveTo(enemyPos);
            }
            GunMan.GetComponent<Movement>().AttackNear();
        }
        foreach (GameObject Cannon in CannonList)
        {
            // not fighting then moving
            if (Cannon.GetComponent<Unit_info>().state != Unit_info.State.Attacking)
            {
                Cannon.GetComponent<Movement>().MoveTo(enemyPos);
            }
            Cannon.GetComponent<Movement>().AttackNear();
        }
        foreach (GameObject Fly in FlyList)
        {
            // not fighting then moving
            if (Fly.GetComponent<Unit_info>().state != Unit_info.State.Attacking)
            {
                Fly.GetComponent<Movement>().MoveTo(enemyPos);
            }
            Fly.GetComponent<Movement>().AttackNear();
        }
    }

    public void FillUnitDataSet(CSV csv)
    {
        for (int i = 1; i < csv.m_ArrayData.Count; i++)
        {
            Definitions.UnitData unit = new Definitions.UnitData();
            unit.ID = int.Parse(csv.GetString(i, 0));
            unit.name = csv.GetString(i, 1);
            unit.type = int.Parse(csv.GetString(i, 2));
            unit.costPaper = int.Parse(csv.GetString(i, 3));
            unit.costGule = int.Parse(csv.GetString(i, 4));
            unit.maxHp = float.Parse(csv.GetString(i, 5));
            unit.moveSpeed = float.Parse(csv.GetString(i, 6));
            unit.attSpeed = float.Parse(csv.GetString(i, 7));
            unit.attDmg = float.Parse(csv.GetString(i, 8));
            unit.attRange = float.Parse(csv.GetString(i, 9));
            unit.isRanger = int.Parse(csv.GetString(i, 10)) == 0 ? false : true;
            unit.size = int.Parse(csv.GetString(i, 11));
            unit.viewRange = float.Parse(csv.GetString(i, 12));


            unit.canBuild = new List<int>();
            if (csv.GetString(i, 13) != "")
            {
                string[] tile1 = csv.GetString(i, 13).Split(';');
                for (int j = 0; j < tile1.Length; j++)
                {
                    unit.canBuild.Add(int.Parse(tile1[j]));
                }
            }

            unit.canCreate = new List<int>();
            if (csv.GetString(i, 14) != "")
            {
                string[] tile2 = csv.GetString(i, 14).Split(';');
                for (int j = 0; j < tile2.Length; j++)
                {
                    unit.canCreate.Add(int.Parse(tile2[j]));
                }
            }

            unit.canResearch = new List<int>();
            if (csv.GetString(i, 15) != "")
            {
                string[] tile3 = csv.GetString(i, 15).Split(';');
                for (int j = 0; j < tile3.Length; j++)
                {
                    unit.canResearch.Add(int.Parse(tile3[j]));
                }
            }



            unit.prefabPath = csv.GetString(i, 16);
            unit.iconPath = csv.GetString(i, 17);
            unitDatas.Add(unit);

        }
    }



    public Definitions.UnitData FindUnitData(int id)
    {
        for (int i = 0; i < unitDatas.Count; i++) {
            if (unitDatas[i].ID == id) {
                return unitDatas[i];
            }
        }
        return null;
    }


    float Far = 40.0f;
    float Close = 25.0f;
    public bool IsDistanceToObject(GameObject target, float buildX, float buildZ) {
        float DiffX = target.transform.position.x - buildX;
        float DiffZ = target.transform.position.z - buildZ;
        if (Close <= DiffX && DiffX <= Far && -Close >= DiffX && DiffX >= -Far) {
            if (Close <= DiffZ && DiffZ <= Far && -Close >= DiffZ && DiffZ >= -Far) {
                return true;
            }
        }
        return false;
    }


    public float Random() {
       
        float ram = UnityEngine.Random.Range(Close, Far);
        
        return ram;
    }

    public GameObject getCloestPaper(Vector3 pos)
    {
        GameObject currBest = null;
        float Dis = 999;
        foreach (GameObject paper in resoursePaper)
        {
            float dis = Vector3.Distance(pos, paper.transform.position);
            if (dis <= Dis)
            {
                Dis = dis;
                currBest = paper;
            }
        }
        return currBest;
    }


    // AI moves
    public void ToBuild(int id, GameObject Operator, Vector3 pos)
    {
        if (gc.GetComponent<GameControl>().CheckResources(playerID, id))
        {

            Operator.GetComponent<BuildControl>().Tar = pos;
            Operator.GetComponent<BuildControl>().currentBuilding = id;

        }
    }
    

    public void ToCreate(int id, GameObject Operator)
    {
        if (gc.GetComponent<GameControl>().CheckResources(playerID, id))
        {
            Operator.GetComponent<Unit_info>().processingQueue.Enqueue(id);
            gc.GetComponent<GameControl>().ResourceConsume(playerID, id);
        }
    }
    /*
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
    */

}
