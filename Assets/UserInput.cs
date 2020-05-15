using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    public GameObject hoveredObject;

    public GameObject selectedObject;
    private GameObject UI_Switch;

    private GameObject gc;
    private Player P;

    // selection Box
    private Camera cam;
    public RectTransform selectionBox;
    private Vector2 startPos;

    RaycastHit hit = new RaycastHit();
    // Start is called before the first frame update

    public void resetStartPos()
    {
        startPos = Input.mousePosition;
    }

    void Start()
    {
        gc = GameObject.Find("GameController");
        P = gc.GetComponent<GameControl>().currentPlayer;

        cam = Camera.main;

        UI_Switch = GameObject.Find("Unit(s)");
    }


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo;
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.root.gameObject;

            HoveredObject(hitObject);
        }
        else
        {
            ClearHover();
        }

        // control all the selected unit
        for (int i=0; i<P.getSelected().Count; i++) {
            GameObject currObject = P.getSelected()[i];
            if (Input.GetMouseButtonDown(1) && hoveredObject != null)
            {

                //var mray = Camera.main.ScreenPointToRay(Input.mousePosition); Will be deled
                // Move !!!
                if (hoveredObject.GetComponent<Unit_info>().type == Unit_info.Type.Terrain)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit))
                    {
                        if (currObject.GetComponent<Unit_info>().moveSpeed > 0)
                        {
                            currObject.GetComponent<Movement>().MoveTo(hit.point);
                        }

                    }
                }

                // Hovered not player's units
                else if (hoveredObject.GetComponent<Unit_info>().belongTo != P.playerID)
                {

                    // ID == 10 mean the selection unit is a farmer, ID >=50 means that is a kind of resource
                    if (hoveredObject.GetComponent<Unit_info>().ID >= 50 && currObject.GetComponent<Unit_info>().ID != 10)
                    {
                    }

                    // Attack
                    else
                    {
                        if (currObject.GetComponent<Unit_info>().ID >=10 && currObject.GetComponent<Unit_info>().ID <= 14)
                            currObject.GetComponent<Movement>().AttactTo(hoveredObject);
                    }

                }

            }
        }

        if (!IsMouseOverUI())
        {

            // left mouse down
            if (Input.GetMouseButtonDown(0))
            {
                if (hoveredObject != null && hoveredObject.GetComponent<Unit_info>().belongTo == gc.GetComponent<GameControl>().currentPlayer.playerID)
                {
                    selectedObject = hoveredObject;
                }

                else
                {

                }
                // selection Box
                startPos = Input.mousePosition;

            }

            // left mouse up
            else if (Input.GetMouseButtonUp(0) && !IsMouseOverUI())
            {
                // clearn up the current ui
                UI_Switch.GetComponent<units_UIswitch>().HideAbilities();
                UI_Switch.GetComponent<units_UIswitch>().HideFormation();

                if (selectedObject == hoveredObject && selectedObject != null)
                {
                    // the unit is really selected

                    if (P.getSelected().Contains(selectedObject)) { }
                    else
                    {
                        P.clearSelect();
                        P.addSelected(selectedObject);
                    }
                }

                else
                {
                    gc.GetComponent<GameControl>().currentPlayer.clearSelect();
                }

                // selection Box
                releaseSelectionBox();
            }

            // left mouse held down
            else if (Input.GetMouseButton(0))
            {
                // selection Box
                updateSelectionBox(Input.mousePosition);

            }
        }
    }

    void updateSelectionBox (Vector2 curMousePos)
    {
        // check if selectionBox is active
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            // active it
            selectionBox.gameObject.SetActive(true);
        }

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }



    // called when we release the selection box
    public void releaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (GameObject unit in P.getControlled())
        {
            if (unit.GetComponent<Unit_info>().moveSpeed != 0)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    if (P.getSelected().Count < 16) { P.addSelected(unit); } 
                }
            }
        }
    }

    private bool IsMouseOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
        {
            return true;
        }

        return false;
    }


    void HoveredObject(GameObject obj)
    {
        if (hoveredObject != null)
        {
            if(obj == hoveredObject)
                return;
        }

        hoveredObject = obj;
    }

    void ClearHover()
    {
        hoveredObject = null;
    }

}
