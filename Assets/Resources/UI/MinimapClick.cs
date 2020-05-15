using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Button>().onClick.AddListener(MouseClick);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseClick() {
        float offset = -30f;
        // Vector3 OriginScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 pos = Input.mousePosition - transform.position;
        float x, y;
        x = pos.x / GetComponent<RectTransform>().rect.width / GetComponent<RectTransform>().localScale.x;
        y = pos.y / GetComponent<RectTransform>().rect.height / GetComponent<RectTransform>().localScale.y;

        float px, pz;
        px = x* 900 >= 400 ? 400 : x * 900;
        px = x * 900 <= -400 ? -400 : x * 900;
        pz = y * 670 >= 320 ? 320 : y * 670;
        pz = y * 670 <= -320 ? -320 : y * 670;


        Vector3 camPos = new Vector3(px, 70, pz+offset);
        Camera.main.transform.position = camPos;
        
    }
}
