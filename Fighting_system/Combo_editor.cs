using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_editor : MonoBehaviour
{
    public bool editing;
    public bool save;
    public Combo_scriptable comboedit;
    public KeyCode key;
    public KeyCode previouskey;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(editing)
        {
            if(key!=null)
            {
                if(comboedit!=null)
                {
                    previouskey = comboedit.key;
                    
                        comboedit.key = key;
                    Debug.Log("Key");
                }
            }
        }
    }
    public void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            //Debug.Log(e.keyCode);
            key = e.keyCode;
        }
    }
}
