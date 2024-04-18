using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarScroll : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public bool horizontal;
    public bool increase;
    public Zoom zoom;
    bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            if (horizontal)
            {
                if (increase)
                {
                    IncreaseHorizontal();
                }
                else
                {
                    DecreaseHorizontal();
                }
            }
            else
            {
                if (increase)
                {
                    IncreaseVertical();
                }
                else
                {
                    DecreaseVertical();
                }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }
     public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    public void IncreaseHorizontal()
    {
        zoom.scrollbar.GetComponent<Scrollbar>().value += 0.001f;

    }
    public void DecreaseHorizontal()
    {
        zoom.scrollbar.GetComponent<Scrollbar>().value -= 0.001f;

    }
    public void IncreaseVertical()
    {
        zoom.scrollbarvert.GetComponent<Scrollbar>().value += 0.0001f;

    }
    public void DecreaseVertical()
    {
        zoom.scrollbarvert.GetComponent<Scrollbar>().value -= 0.0001f;

    }
}
