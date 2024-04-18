using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HierarchyButton : MonoBehaviour
{
    public ObjectDataHolder connectedobject;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        if (btn == null)
            btn = transform.GetComponent<Button>();
        if (GameObject.Find("Canvas") != null)
        {
            // Debug.Log("fail");
            if (GameObject.Find("Canvas").GetComponent<LerpTest>() != null)
                btn.onClick.AddListener(() => GameObject.Find("Canvas").GetComponent<LerpTest>().SelectObjectData(connectedobject.data));
        }
        
    }
}
