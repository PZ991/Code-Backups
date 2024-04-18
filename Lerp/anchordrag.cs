using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class anchordrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool dragging;
    public Vector2 offset;
    public bool draggable;
    public bool selected;
    public bool started;
    public void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (IsPointerOverUIElement()&&!started)
            {
                offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(transform.position.x, transform.position.y);
            }
            else
            {
                selected = false;
                draggable = false;
                started = true;
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            started = false;
        }
            if (dragging || (draggable && Input.GetMouseButton(1)))
            {
                transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset;
            }
        
        if(selected)
        {
            transform.GetComponent<Image>().color = new Color(1, 0.8f, 0, 1);
        }
        else
        {
            transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        if(draggable)
        {
            transform.GetComponent<Image>().color = new Color(1, 0.8f, 0, 1);

        }

    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        selected = true;

        dragging = true;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        selected = false;

        dragging = false;
    }
    
    public void Draggable()
    {
        selected = true;

        draggable = true;

        //dragging = true;
    }
    public void Undraggable()
    {
        selected = false;

        draggable = false;
        //dragging = false;
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "UILayer")
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

}
