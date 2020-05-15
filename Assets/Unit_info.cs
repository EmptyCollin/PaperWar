using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_info : MonoBehaviour
{

    // Use this for initialization
    public double hp;
    public double maxHp ;
    public double moveSpeed;
    public double attSpeed;     // attack times in every second
    public double attDmg;
    public double attRange;
    public double currIntercept;
    public bool isSelect;
    public bool isRanger;
    public int belongTo;
    public int size;        // scaler of object in game

    public double viewRange;

    public enum Type { Character,Building,Resource,Terrain};
    public enum State { Idle,Moving,Attacking,Working };

    public Type type;
    public State state;
    public Animator ani;

    // identify which unit it is 
    public int ID;

    // what this object can create
    public List<int> canBuild;
    public List<int> canCreate;
    public List<int> canResearch;


    //++++++++++++++++Processing+++++++++++++++//
    public Queue<int> processingQueue;
    public float currentProcessing;
    public int currentItem;     // id of which item is in processing
    public Texture itemText;
    //++++++++++++++++Processing+++++++++++++++//

    // only work for constructionArea, that specifiy what it is going to be
    public int goingToBe=-999;


    private GameObject gc;
    private GameObject UISwitch;

    void Start()
    {
        isSelect = false;
        ani = GetComponent<Animator>();
        gc = GameObject.Find("GameController");
        UISwitch = GameObject.Find("Unit(s)");
        processingQueue = new  Queue<int>();
        currentProcessing = 1;
        currentItem = -999;
        itemText = null;
        state = State.Idle;
        SetState(State.Idle);
    }

    void Update() {
        if (currentItem != -999)
        {
            state = State.Working;
        }
        else
        {
            state = State.Idle;
        }
        if (hp <= 0)
        {
            Player P = gc.GetComponent<GameControl>().players[belongTo];
            switch (ID)
            {
                
                case 0:
                    P.BaseDie(gameObject);
                    break;
                case 2:
                    P.Barrier_1_Die(gameObject);
                    break;
                case 3:
                    P.Barrier_2_Die(gameObject);
                    break;
                case 10:
                    P.RemoveFarmerTolist(gameObject);
                    break;
                case 11:
                    P.RemoveFootManTolist(gameObject);
                    break;
                case 12:
                    P.RemoveGunManTolist(gameObject);
                    break;
                case 13:
                    P.RemoveCannonTolist(gameObject);
                    break;
                case 14:
                    P.RemoveFlyTolist(gameObject);
                    break;

                default:
                    break;
            }

            P.removeSelected(gameObject);
            P.removeControlled(gameObject);
            //new
            UISwitch.GetComponent<units_UIswitch>().HideFormation();
            UISwitch.GetComponent<units_UIswitch>().HideAbilities();
            Destroy(gameObject);
            return;
        }

        // research tech or create unit
        // something in working
        
        if (belongTo >= 0) {
            if (currentItem != -999)
            {
                currentProcessing += 0.2f*Time.deltaTime;
                if (currentProcessing >= 1)
                {
                    // Research
                    if (currentItem >= 100)
                    {
                        gc.GetComponent<GameControl>().Research(belongTo, currentItem);
                    }
                    // Create character
                    else if (currentItem == 14) {
                        gc.GetComponent<GameControl>().CreateUnit(belongTo, gc.GetComponent<GameControl>().players[belongTo], transform.position + new Vector3(UnityEngine.Random.value * 20 - 10, 20, UnityEngine.Random.value * 20 - 10), currentItem);
                    }
                    else 
                    {
                        gc.GetComponent<GameControl>().CreateUnit(belongTo, gc.GetComponent<GameControl>().players[belongTo], transform.position + new Vector3(UnityEngine.Random.value * 20 - 10, 0, UnityEngine.Random.value * 20 - 10), currentItem);
                    }

                    // reset
                    currentProcessing = 0;
                    currentItem = -999;
                    itemText = null;


                }
            }
            // nothing in working
            else
            {
                if (processingQueue.Count > 0)
                {
                    currentItem = processingQueue.Dequeue();
                    currentProcessing = 0;
                    itemText = Resources.Load<Texture2D>(gc.GetComponent<GameControl>().players[belongTo].FindUnitData(currentItem).iconPath);
                }
            }
        } 
        
    }

    public void SetState(State s) {
        state = s;
        if (ani != null){
            UpdateAnimation(state);
        }
    }

    private void UpdateAnimation(State state)
    {
        ani.SetInteger("state", (int)state);
    }

    public void SetHp(int sethp)
    {
        hp = sethp;
    }


    private void OnMouseDown()
    {
        //isSelect = true;
        //UI.GetComponent<>().switchTo();
        //GetComponentInParent<>().
    }



    public void CreateFamer(int who)
    {
        hp = 1000;
        maxHp = 1000;
        moveSpeed = 5;
        attSpeed = 2;
        attDmg = 5;
        attRange = 15;
        currIntercept = 0;
        belongTo = who;
        type = Type.Character;
        isRanger = false;

        size = 1;
        ID = 10;
        // need to change, that farmer cant create base
        // use base (0) test here
        canBuild.Add(0);
    }

    public void CreatePlane(int who)
    {
        hp = 1000;
        maxHp = 1000;
        moveSpeed = 5;
        attSpeed = 2;
        attDmg = 5;
        attRange = 40;
        currIntercept = 0;
        belongTo = who;
        type = Type.Character;
        isRanger = true;

        
        ID = 14;
    }


    public void CreateBase(int who)
    {
        hp = 1000;
        maxHp = 1000;
        moveSpeed = 0;
        attSpeed = 0;
        attDmg = 0;
        attRange = 0;
        currIntercept = 10;
        belongTo = who;
        type = Type.Building;

        size = 5;
        ID = 0;
        canBuild.Add(10);
    }

    internal void CreateFootman(int who)
    {
        hp = 1000;
        maxHp = 1000;
        moveSpeed = 5;
        attSpeed = 2;
        attDmg = 15;
        attRange = 15;
        currIntercept = 0;
        belongTo = who;
        type = Type.Character;
        isRanger = false;

        
        ID = 11;
        
    }

    internal void CreateMusketeer(int who)
    {
        hp = 1000;
        maxHp = 1000;
        moveSpeed = 5;
        attSpeed = 2;
        attDmg = 5;
        attRange = 60;
        currIntercept = 0;
        belongTo = who;
        type = Type.Character;
        isRanger = true;

        
        ID = 12;
        
    }

    internal void CreateConstructionArea(int who, int tobe)
    {
        hp = 100;
        maxHp = 1000;
        moveSpeed = 0;
        attSpeed = 0;
        attDmg = 0;
        attRange = 0;
        currIntercept = 0;
        belongTo = who;
        type = Type.Building;
        isRanger = false;

        // ID for constructionArea
        ID = 9;

        goingToBe = tobe;
    }

    internal void CreateNewUnit(int who, Definitions.UnitData unit)
    {
        hp = unit.maxHp;
        maxHp = unit.maxHp;
        moveSpeed = unit.moveSpeed;
        attSpeed = unit.attSpeed;
        attDmg = unit.attDmg;
        attRange = unit.attRange;
        canBuild = unit.canBuild;
        canCreate = unit.canCreate;
        canResearch = unit.canResearch;
        belongTo = who;
        switch (unit.type) {
            case 0:
                type = Type.Building;
                break;
            case 1:
                type = Type.Character;
                break;
            default:
                type = Type.Terrain;
                break;
        };

        isRanger = unit.isRanger;
        name = unit.name;
        size = unit.size;
        viewRange = unit.viewRange;

        ID = unit.ID;
        state = State.Idle;
        SetState(state);
    }

    public void CleanProcessQueue()
    {
        if (currentItem != -999) {
            ReturnResource(currentItem);
        }
        foreach (var i in processingQueue) {
            ReturnResource(i);
        }
    }

    private void ReturnResource(int id)
    {
        Definitions.UnitData u = gc.GetComponent<GameControl>().players[belongTo].FindUnitData(id);
        gc.GetComponent<GameControl>().players[belongTo].PaperNum += u.costPaper;
        gc.GetComponent<GameControl>().players[belongTo].GlueNum += u.costGule;
    }
}
