using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ToggleColor : MonoBehaviour
{
    public Toggle toggle;
    public Color OnColor;
    public Color OffColor;
    public Image Targetimage;
    public bool ToggleImageText;
    public Sprite On;
    public Sprite Off;
    public Image ToggleImage;
    public string OnWord;
    public string OffWord;
    public TextMeshProUGUI TargetText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(toggle.isOn)
        {
            Targetimage.color = OnColor;
            if(ToggleImageText)
            {
                ToggleImage.sprite = On;
                TargetText.text = OnWord;
            }
        }
        else
        {
            Targetimage.color = OffColor;
            if (ToggleImageText)
            {
                ToggleImage.sprite = Off;
                TargetText.text = OffWord;
            }
        }
    }
}
