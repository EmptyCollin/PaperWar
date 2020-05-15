using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notificaiton : MonoBehaviour
{
    private float UI_Alpha = 0;             
    public float alphaSpeed = 0.2f;
    public float delayTime = 1.0f;
    private float lastRecord = 0;
    private CanvasGroup canvasGroup;
    // Use this for initialization
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (canvasGroup == null)
        {
            return;
        }
        if (lastRecord < delayTime) {
            lastRecord += Time.deltaTime;
            return;
        }
        if (UI_Alpha != canvasGroup.alpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, UI_Alpha, alphaSpeed * Time.deltaTime);
            if (Mathf.Abs(UI_Alpha - canvasGroup.alpha) <= 0.01f)
            {
                canvasGroup.alpha = UI_Alpha;
            }
        }
    }

    public void ChangeContent(string content, Color c)
    {
        GetComponent<TMP_Text>().text = content;
        GetComponent<TMP_Text>().color = c;
        canvasGroup.alpha = 1;
        lastRecord = 0;
    }
}