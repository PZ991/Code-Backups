using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimelineDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 offset;
    public bool dragging;
    public RectTransform horizontal;
    public RectTransform vertical;
    public ScrollbarScroll Horizontal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            offset = new Vector2(Input.mousePosition.x, transform.position.y) - new Vector2(transform.position.x, transform.position.y);

        }
        if ( Input.GetMouseButton(0)&&dragging)
        {
            Vector3[] corner1 = new Vector3[4];
            horizontal.GetWorldCorners(corner1);
            Vector3[] corner2 = new Vector3[4];
            vertical.GetWorldCorners(corner2);
            transform.position = new Vector2(Mathf.Clamp((new Vector2(Input.mousePosition.x, transform.position.y) - offset).x, corner1[0].x, corner2[2].x), transform.position.y);
            if (transform.position.x == corner1[0].x && transform.position.x > Input.mousePosition.x)
            {
                Horizontal.DecreaseHorizontal();
            }
            else if (transform.position.x >= corner1[2].x - 25 && transform.position.x < Input.mousePosition.x)
            {
                Horizontal.IncreaseHorizontal();

            }
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
        Vector3[] corner1 = new Vector3[4];
        horizontal.GetWorldCorners(corner1);
        Vector3[] corner2 = new Vector3[4];
        vertical.GetWorldCorners(corner2);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(corner1[2].x-25, corner1[2].y), Vector3.one * 10);
    }
}
