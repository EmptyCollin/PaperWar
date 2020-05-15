using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definitions
{
    // Start is called before the first frame update
    public enum UnityType { Farmer, Footman };

    public enum GameStatus { Ingame, Meau, EndScreen }

    // change the flag color based on player's color
    public static void ChangeColor(GameObject root, Color c)
    {
        if (root.tag == "FlagColor")
        {
            try
            {
                root.GetComponent<MeshRenderer>().material.color = c*0.9f;
            }
            catch { }
            try
            {
                root.GetComponent<SkinnedMeshRenderer>().material.color = c*0.9f;
            }
            catch { }

        }
        
        for (int i = 0; i < root.transform.childCount; i++)
        {
            ChangeColor(root.transform.GetChild(i).gameObject, c);
        }
        
    }
    // struct for upgrade and unlock items
    public struct WhoAndWhat {
        int who;    // id of who get these update
        int what;   // id of what items are updated
    }

    public class UnitData {
        public int ID           = -999;
        public string name      = "";
        public int type = 0;
        public int costPaper = 0;
        public int costGule = 0;
        public float maxHp     = 0;
        public float moveSpeed = 0;
        public float attSpeed  = 0;     // attack times in every second
        public float attDmg    = 0;
        public float attRange  = 0;
        public bool isRanger    = false;
        public int size         = 0;
        public float viewRange = 0;
        public List<int> canBuild= new List<int>();
        public List<int> canCreate = new List<int>();
        public List<int> canResearch = new List<int>();
        public string prefabPath= "";
        public string iconPath  = "";

    }

}



