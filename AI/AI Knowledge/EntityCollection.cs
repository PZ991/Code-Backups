using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCollection : MonoBehaviour
{
    public List<GameObject> childs;
    public List<ObjectKnowledge> objects;
    public ObjectKnowledge main;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject item in childs)
        {
            EntityMain mainitem= item.GetComponent<EntityMain>();
        }
    }
}
