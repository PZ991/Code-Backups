using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TimelineResize : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public RectTransform horizontal;
    public RectTransform vertical;
    public RectTransform Resizer;
    public RectTransform minimum;
    public RectTransform BG;

    public RectTransform B1;
    public RectTransform B2;
    public RectTransform Slider;

    public RectTransform HierarchyBG;
    public RectTransform HierarchyViewport;
    Vector2 offsetBG;
     Vector2 offsetM;
     Vector2 offsetH2;
     float offsetV;
     Vector2  offsetV2;
    Vector3 origsize;
    bool dragging;
    Vector2 offsetSlider;
    void Start()
    {
        
        Vector3[] rectsH = new Vector3[4];
        offsetV2 = vertical.position;
        Vector3[] rects = new Vector3[4];
        Vector3[] rectsR = new Vector3[4];
        minimum.GetWorldCorners(rects);
        horizontal.GetWorldCorners(rectsH);
        Resizer.GetWorldCorners(rectsR);
        Vector3[] rectsB1 = new Vector3[4];
        Vector3[] rectsB2 = new Vector3[4];
        B1.GetWorldCorners(rectsB1);
        B2.GetWorldCorners(rectsB2);
        Vector2 pos = (((new Vector2(vertical.position.x / 2, rects[0].y / 2)) + (new Vector2(vertical.position.x / 2, (rectsR[0].y / 2)))));

        offsetV = vertical.position.y-pos.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] rects = new Vector3[4];
        Vector3[] rectsH = new Vector3[4];
        Vector3[] rectsR = new Vector3[4];
        Vector3[] rectsB1 = new Vector3[4];
        Vector3[] rectsB2 = new Vector3[4];
        minimum.GetWorldCorners(rects);
        horizontal.GetWorldCorners(rectsH);
        Resizer.GetWorldCorners(rectsR);
        B1.GetWorldCorners(rectsB1);
        B2.GetWorldCorners(rectsB2);
        
        if (Input.GetMouseButtonDown(0))
        {
            offsetM = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(transform.position.x, transform.position.y);
            offsetBG = new Vector2(BG.position.x, BG.position.y) - new Vector2(BG.position.x, BG.position.y);
            offsetH2 = new Vector2(Resizer.position.x, Resizer.position.y) - new Vector2(horizontal.position.x, horizontal.position.y);
            offsetSlider = new Vector2(rectsB1[3].x, rectsB1[3].y) - new Vector2(Slider.position.x, Slider.position.y);

        }
        if (dragging)
        { 
            transform.position = new Vector2(transform.position.x, Input.mousePosition.y- offsetM.y) ;
            Vector2 dir = ((new Vector2(horizontal.position.x, rects[0].y)) - (new Vector2(horizontal.position.x, rectsR[0].y))).normalized;
            float distance = Vector2.Distance((new Vector2(horizontal.position.x, rectsR[0].y)), (new Vector2(horizontal.position.x, rects[0].y)));
            horizontal.sizeDelta = new Vector2(horizontal.sizeDelta.x, distance/2.4f);
            horizontal.position = (((new Vector2(horizontal.position.x, rects[0].y)) + (new Vector2(horizontal.position.x, rectsR[0].y)))) / 2;

            Vector2 dir1 = ((new Vector2(BG.position.x, rects[0].y)) - (new Vector2(BG.position.x, rectsR[0].y))).normalized;
            float distance1 = Vector2.Distance((new Vector2(BG.position.x, rectsR[0].y)), (new Vector2(BG.position.x, rects[0].y)));
            BG.sizeDelta = new Vector2(BG.sizeDelta.x, distance1/2.4f);
            BG.position = (((new Vector2(BG.position.x, rects[0].y)) + (new Vector2(BG.position.x, rectsR[0].y)))) / 2;

            float distance4 = Vector2.Distance((new Vector2(HierarchyBG.position.x, rectsR[0].y)), (new Vector2(HierarchyBG.position.x, rects[0].y)));
            HierarchyBG.sizeDelta = new Vector2(HierarchyBG.sizeDelta.x, distance4/2.4f);
            HierarchyBG.position = (((new Vector2(HierarchyBG.position.x, rects[0].y)) + (new Vector2(HierarchyBG.position.x, rectsR[0].y)))) / 2;
           
            float distance6 = Vector2.Distance((new Vector2(HierarchyViewport.position.x, rectsR[0].y)), (new Vector2(HierarchyViewport.position.x, rects[0].y)));
            HierarchyViewport.sizeDelta = new Vector2(HierarchyViewport.sizeDelta.x, distance6/2.4f);
            HierarchyViewport.position = (((new Vector2(HierarchyViewport.position.x, rects[0].y)) + (new Vector2(HierarchyViewport.position.x, rectsR[0].y)))) / 2;




            float distance3 = Vector2.Distance((new Vector2(Slider.position.x, rectsB2[0].y)), (new Vector2(Slider.position.x, rectsB1[0].y)));
            Slider.sizeDelta = new Vector2(Slider.sizeDelta.x, distance3/2.4f);
            Vector2 pos2 = (((new Vector2(Slider.position.x, rectsB2[0].y) ) + (new Vector2(Slider.position.x, rectsB1[0].y)))/2) ;
            Slider.position = new Vector2(pos2.x, rectsB1[3].y-offsetSlider.y);




            Vector2 dir2 = ((new Vector2(vertical.position.x, rects[0].y)) - (new Vector2(vertical.position.x, rectsR[0].y))).normalized;
            float distance2 = Vector2.Distance((new Vector2(vertical.position.x, rectsR[0].y)), (new Vector2(vertical.position.x, rects[0].y)));
            vertical.sizeDelta = new Vector2(vertical.sizeDelta.x, distance/2.4f);
            Vector2 pos = (((new Vector2(vertical.position.x / 2, rects[0].y / 2)) + (new Vector2(vertical.position.x / 2, (rectsR[0].y/ 2) )))) ;
            vertical.position = new Vector2(pos.x, pos.y +offsetV );

           
            

        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
    private void OnDrawGizmos()
    {
        Vector3[] rects = new Vector3[4];
        Vector3[] rectsH = new Vector3[4];
        Vector3[] rectsR = new Vector3[4];
        minimum.GetWorldCorners(rects);
        horizontal.GetWorldCorners(rectsH);
        Resizer.GetWorldCorners(rectsR);
        Vector3[] rectsB1 = new Vector3[4];
        Vector3[] rectsB2 = new Vector3[4];
       
        B1.GetWorldCorners(rectsB1);
        B2.GetWorldCorners(rectsB2);
        //Debug.Log(rectsR[0].y+"/"+ rects[0].y+"="+ ((rectsR[0].y + rects[0].y) / 2));
        // Debug.Log(rectsH[0].y+"/"+ rectsH[1].y+"="+(  rectsH[1].y+rectsH[0].y)/2);
        // Debug.Log((((rectsH[1].y + rectsH[0].y) / 2) - 0) / (((rectsR[0].y + rects[0].y) / 2) - 0));
        //Debug.Log(Mathf.InverseLerp(0,((rectsH[1].y + rectsH[0].y) / 2), (((rectsR[0].y + rects[0].y) / 2))));
        /*
        Debug.Log((rectsR[0].y-rects[0].y));
        Debug.Log((rectsH[1].y-rectsH[0].y));
        float on = Mathf.InverseLerp(1, 0, (((rectsH[1].y - rectsH[0].y)) / (rectsR[0].y - rects[0].y)));
        float on2 = Mathf.Lerp(0, 2, on);
        Debug.Log(on2);
        */

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rectsR[0], rects[0]);
        Gizmos.DrawLine(rectsH[1], rectsH[0]);
        Gizmos.color = Color.green;
        //Gizmos.DrawCube(rects[0], Vector3.one * 10);
        //Gizmos.DrawCube(rectsR[0], Vector3.one * 10);
        Gizmos.DrawCube(((new Vector2(Slider.position.x / 2, rectsB1[0].y / 2)) + (new Vector2(Slider.position.x / 2, (rectsB2[0].y / 2)))), Vector3.one * 10);
       // Gizmos.DrawCube(rectsB2[0], Vector3.one * 10);
       // Gizmos.DrawCube(new Vector2(horizontal.position.x, (rectsR[0].y + rects[0].y) / 2), Vector3.one * 20);
    }

    
}
