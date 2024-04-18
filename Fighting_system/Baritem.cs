using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Baritem : MonoBehaviour
{
    public Combo_scriptable combo;
    public TextMeshProUGUI Bar_Key;
    public Image barImage;
    public Sprite defaultBar;
    public Combo_manager combomanager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(combo!=null)
        {
            if(combo.Image!=null)
            {
                barImage.sprite = combo.Image;
            }
            Bar_Key.text = combo.key.ToString();
            if(Input.GetKeyDown(combo.key))
            {
                combomanager.currentcombo = combo;
            }
        }
        else
        {
            barImage.sprite = defaultBar;
            Bar_Key.text = "";
        }
    }
}
