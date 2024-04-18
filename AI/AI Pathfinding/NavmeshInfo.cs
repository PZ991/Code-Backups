using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshInfo : MonoBehaviour
{
    public float Weight;
    public bool isSet;
    public List<string> gridinfo;
    public List<float> gridtemp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Calculate(List<PrefData> weights,int layer)

    {
        foreach (var item in weights)
        {
            if(gridinfo.Contains(item.name))
            {
                gridtemp.Add(item.weight);
            }
        }
        Weight = AddList(gridtemp);
        transform.GetComponent<NavMeshModifier>().area = layer;
       // Debug.Log("test");
        if (layer < 6)
            NavMesh.SetAreaCost(layer, Weight);
        isSet = true;
    }
    public void Clear()
    {
        Weight = 0;
        if(gameObject.GetComponent<NavMeshModifier>()!=null)
        {
            gameObject.GetComponent<NavMeshModifier>().area = 0;
        }
        isSet = false;
    }
    public float AddList(List<float> list)
    {
        float value = 0;
        foreach (var item in list)
        {
            value += item;
        }
        return value;
    }
    private void OnDrawGizmos()
    {
       // Debug.Log(gameObject.GetComponent<NavMeshModifier>().area);
    }
}
