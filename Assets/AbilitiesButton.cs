using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AbilitiesButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int who;
    public int target;
    public int unitType;    // the id of unit who do this operation
    public GameObject gc;
    private GameObject description;
    private GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        description = transform.Find("Description").transform.gameObject;
        text = description.transform.GetChild(0).gameObject;
        description.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Record(int belongTo, int tar,int u)
    {
        gc = GameObject.Find("GameController");
        who = belongTo;
        target = tar;
        unitType = u;
        GetComponent<RawImage>().texture = Resources.Load<Texture2D>(gc.GetComponent<GameControl>().players[belongTo].FindUnitData(tar).iconPath);
        Definitions.UnitData unit = gc.GetComponent<GameControl>().players[belongTo].FindUnitData(tar);
        text = transform.Find("Description").Find("Text").gameObject;
        text.GetComponent<TMP_Text>().text = unit.name + ", P("+unit.costPaper+"), G("+unit.costGule+")";
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);
    }
}
