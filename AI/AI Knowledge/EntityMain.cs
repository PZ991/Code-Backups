using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMain : MonoBehaviour
{
    //Replace ProjectileTest and SPawnerTest

    // Start is called before the first frame update
    public string name;
    public Transform obj;
    public Transform[] currentseachers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
       obj = FindSibling(transform, name);
    }

    //FindChild
    Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
    //Find Parent
    Transform FindParent(Transform origin, string parentname)
    {
        if (origin.parent.name == parentname)
        {
            return origin.parent;
        }
        else
        {
            if (origin.parent == null)
            {
                return null;
            }
            else
            {
                Transform found = FindParent(origin.parent, parentname);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;

    }
    Transform FindSibling(Transform origin,string siblingname)
    {
        if (origin.parent.Find(siblingname) !=null)
        {
            return origin.parent.Find(siblingname);
        }
       
        return null;
    }
}
