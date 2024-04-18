using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selectionbox : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 startPos;
    public List<List<List<anchordrag>>> Unit= new List<List<List<anchordrag>>>();
    public LerpTest line;
    public Canvas canvas;
    public bool moving;
    bool started;
    void Start()
    {
        ReleaseSelectionBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPointerOverUIElement()&&!started)
        {
            if (line.lerpdataimport.Count != Unit.Count)
            {
                Unit = new List<List<List<anchordrag>>>();
                for (int i = 0; i < line.lerpdataimport.Count; i++)
                {
                    Unit.Add(new List<List<anchordrag>>());

                }
            }
            for (int k = 0; k < line.lerpdataimport.Count; k++)
            {
                if (line.prefabpos[k].Count != Unit[k].Count)
                {
                    Unit[k] = new List<List<anchordrag>>();

                }

                for (int i = 0; i < line.prefabpos[k].Count; i++)
                {
                    List<anchordrag> listdrag = new List<anchordrag>();
                    Unit[k].Add(listdrag);

                    for (int j = 0; j < line.prefabpos[k][i].Count; j++)
                    {
                        var unit = line.prefabpos[k][i][j].GetComponent<anchordrag>();



                        if (!Unit[k][i].Contains(unit))
                        {
                            listdrag.Add(unit);
                        }
                    }
                }
                for (int i = 0; i < line.anchors[k].Count; i++)
                {

                    for (int j = 0; j < line.anchors[k][i].Count; j++)
                    {
                        //Debug.Log(line.anchors[i][j]);
                        var unit = line.anchors[k][i][j].GetComponent<anchordrag>();
                        if (!Unit[k][i].Contains(unit))
                        {
                            Unit[k][i].Add(unit);
                        }
                    }

                }
            }
            /*
            if (line.prefabpos.Count != Unit.Count)
            {
                Unit = new List<List<List<anchordrag>>>();

            }
            for (int i = 0; i < line.prefabpos.Count; i++)
            {
                List<anchordrag> listdrag = new List<anchordrag>();
                Unit.Add(listdrag);

                for (int j = 0; j < line.prefabpos[i].Count; j++)
                {
                    var unit = line.prefabpos[i][j].GetComponent<anchordrag>();



                    if (!Unit[i].Contains(unit))
                    {
                        listdrag.Add(unit);
                    }
                }
            }
            for (int i = 0; i < line.anchors.Count; i++)
            {

                for (int j = 0; j < line.anchors[i].Count; j++)
                {
                    //Debug.Log(line.anchors[i][j]);
                    var unit = line.anchors[i][j].GetComponent<anchordrag>();
                    if (!Unit[i].Contains(unit))
                    {
                        Unit[i].Add(unit);
                    }
                }

            }
            */


            if (Input.GetMouseButtonDown(1))
            {
                startPos = Input.mousePosition;

            }
            for (int k = 0; k < line.lerpdataimport.Count; k++)
            {


                // mouse held down
                for (int i = 0; i < Unit[k].Count; i++)
                {
                    for (int j = 0; j < Unit[k][i].Count; j++)
                    {
                        if (moving == false)
                        {
                            if (Unit[k][i][j].dragging == true || Unit[k][i][j].draggable && Input.GetMouseButton(1))
                            {
                                moving = true;
                                break;
                            }
                            else
                            {
                                moving = false;
                            }
                        }
                    }

                }

                if (Input.GetMouseButton(1))
                {

                    Vector3 mousePos = Input.mousePosition;

                    //Debug.DrawLine(mousePos + Vector3.one, mousePos, Color.black, 100);

                    //now create a new vector3
                    UpdateSelectionBox(mousePos);
                }



                if (Input.GetMouseButtonUp(1))
                {
                    ReleaseSelectionBox();
                    for (int i = 0; i < line.movedobject[k].Count; i++)
                    {
                        line.movedobject[k][i] = true;

                    }
                }

            }
        }
        else if(!IsPointerOverUIElement())
        {
            started = true;
        }
        if (Input.GetMouseButtonUp(1))
            started = false;
    }
    void UpdateSelectionBox(Vector2 curMousePos)
    {
        float width = curMousePos.x - startPos.x;
            float height = curMousePos.y - startPos.y;
        //   selectionBox.gameObject.SetActive(true);
        if (moving == false)
        {
            selectionBox.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

            selectionBox.position = startPos + new Vector2(width / 2, height / 2);
        } 
        else
        {
            selectionBox.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            selectionBox.sizeDelta = new Vector2(0,0);

            selectionBox.position =new Vector2(0,0);

        }
        
    }
    void ReleaseSelectionBox()
    {
        //selectionBox.gameObject.SetActive(false);
        selectionBox.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        Vector2 min = new Vector2(selectionBox.position.x, selectionBox.position.y) - (selectionBox.sizeDelta / 2);
        Vector2 max = new Vector2(selectionBox.position.x, selectionBox.position.y) + (selectionBox.sizeDelta / 2);
        for (int k = 0; k < line.lerpdataimport.Count; k++)
        {


            for (int i = 0; i < Unit[k].Count; i++)
            {
                foreach (anchordrag unit in Unit[k][i])
                {
                    Vector3 screenPos = unit.transform.position;

                    if ((screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y) && moving == false && unit.GetComponent<Image>().enabled == true)
                    {
                        //selectionUnits.Add(unit);
                        unit.Draggable();
                    }
                    else
                    {

                        unit.Undraggable();
                    }
                }
            }
        }
        /*
        for (int k = 0; k < line.lerpdataimport.Count; k++)
        {


            for (int i = 0; i < Unit[k].Count; i++)
            {
                for (int j = 0; j < Unit[k][i].Count; j++)
                {
                    if (Unit[k][i][j].selected == true && j!= Unit[k][i].Count)
                    {
                        line.selected[k][i] = true;
                        continue;
                    }
                    else if(j== Unit[k][i].Count)
                    {
                        line.selected[k][i] = false;
                    }
                }
                    
                
            }
        }
        */
                    moving = false;
    }





    //Returns 'true' if we touched or hovering on Unity UI element.
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
