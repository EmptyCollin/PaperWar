using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorReplace : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    void Start() {
        prefab = null;
    }

    void Update() {
        if (prefab != null) {
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            prefab.transform.position = Camera.main.ScreenToWorldPoint(m_MousePos);
        }
    }

    public void Replace() {
        Cursor.visible = false;
        prefab = Instantiate(Resources.Load("UI/ShowConstructionArea")) as GameObject;

    }

    public void Restore() {
        Cursor.visible = true;
        Destroy(prefab);
        prefab = null;
    }
    
}

