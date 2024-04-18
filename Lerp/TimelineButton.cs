using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineButton : MonoBehaviour
{
    public bool All;
    public bool AllRot;
    public bool AllPos;
    public bool AllCustom;
    public int dataindex;
    public Button btn;
    public bool Custom;
    public void Start()
    {
        if (GameObject.Find("Canvas") != null)
        {
            // Debug.Log("fail");
            if (GameObject.Find("Canvas").GetComponent<LerpTest>() != null)
                btn.onClick.AddListener(() => GameObject.Find("Canvas").GetComponent<LerpTest>().AddKey(dataindex, Custom, AllPos, AllRot, AllCustom, All));
        }
    }
}
