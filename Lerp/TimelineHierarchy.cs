using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TimelineHierarchy : MonoBehaviour
{
    public List<GameObject> display;
    public List<string> displayPath;
    public List<List<GameObject>> hierarchy= new List<List<GameObject>>();

    public GameObject ContentParent;
    public List<GameObject> Instances;
    public GameObject prefab;


    void Start()
    {
        
        displayPath.Clear();
        for (int i = 0; i < display.Count; i++)
        {
            displayPath.Add(GetPath(display[i]));
           // Debug.Log(GetPath(display[i]));
        }
        for (int i = 0; i < displayPath.Count; i++)
        {
            List<List<char>> separation = new List<List<char>>();
            for (int j = 0; j < displayPath[i].Length;j++)
            {
                if(separation.Count==0)
                {
                    separation.Add(new List<char>());
                }
                if(displayPath[i][j]!='/')
                {
                    separation[separation.Count - 1].Add(displayPath[i][j]);
                }
                else if(displayPath[i][j] == '/')
                {
                    separation.Add(new List<char>());
                }
            }
            for (int k = 0; k < separation.Count; k++)
            {
                string number= "instance#" + i + "/" + k + ": ";
                string word = "";
                for (int h = 0; h < separation[k].Count; h++)
                {
                    word = word+separation[k][h].ToString();
                }
                //Debug.Log("instance#" + i + "/" + k + ": "+ word);
                
                for (int l = 0; l < display.Count; l++)
                {
                    if(word==display[l].name)
                    {
                        #region V1
                        #region Child
                        /*
                        for (int o = 0; o < display[l].transform.childCount; o++)
                        {
                            for (int u = 0; u < display.Count; u++)
                            {
                                if (display[l].transform.GetChild(o).name==display[u].name)
                                {
                                    Debug.Log("parent instance = " + display[l].name + " child is: " + display[u].name);
                                    break;
                                }

                            }
                        }
                        */
                        #endregion
                        #region Parent
                        if (!Instances.Find(x => x.name == ("Instance: " + display[l].GetInstanceID())))
                        {

                            GameObject go = Instantiate(prefab, ContentParent.transform);
                            go.GetComponent<HierarchyButton>().connectedobject = display[l].GetComponent<ObjectDataHolder>();
                            go.name = "Instance: " + display[l].GetInstanceID();
                            go.transform.position = new Vector2(go.transform.position.x, go.transform.position.y );
                            go.GetComponentInChildren<TextMeshProUGUI>().text = display[l].name;
                            Instances.Add(go);
                        }
                        #endregion
                        #endregion













                    }
                }

                 

                
            }
        }
        float positiony = 0;
        int count = 0;

        for (int m = 0; m < display.Count; m++)
        {
            
            if (display[m].transform.parent != null)
            {
                List<GameObject> parentlist = new List<GameObject>();
                GameObject currentparentcheck = display[m];
                GameObject outputparentcheck = null;
                bool rootfound = false;

                while (rootfound == false)
                {
                    if (currentparentcheck.transform.parent != null)
                    {
                        if (display.Contains(currentparentcheck.transform.parent.gameObject))
                        {
                            outputparentcheck = display[display.IndexOf(currentparentcheck.transform.parent.gameObject)];
                            rootfound = true;
                            
                        }
                        else
                        {
                            currentparentcheck = currentparentcheck.transform.parent.gameObject;
                        }
                    }
                    else
                    {
                        rootfound = true;
                    }

                }
                if (outputparentcheck != null)
                {
                    if (Instances.Find(x => x.name == ("Instance: " + outputparentcheck.GetInstanceID())) && Instances.Find(x => x.name == ("Instance: " + display[m].GetInstanceID())))
                    {
                        GameObject parent = Instances.Find(x => x.name == ("Instance: " + outputparentcheck.GetInstanceID()));
                        GameObject child= Instances.Find(x => x.name == ("Instance: " + display[m].GetInstanceID()));
                        child.transform.SetParent(  parent.transform,false);
                        child.transform.localPosition = new Vector2(child.transform.localPosition.x+3, child.transform.localPosition.y - 16);
                        
                        count += 1;
                        //Debug.Log("parent instance = " + outputparentcheck.name + " child is: " + display[m].name);

                    }
                }
            }
            else
            {
                GameObject go = Instances.Find(x => x.name == ("Instance: " + display[m].GetInstanceID()));
                go.transform.localPosition = new Vector2(go.transform.localPosition.x, go.transform.localPosition.y - (16*count));
                count += 1;
            }

            display[m].GetComponent<ObjectDataHolder>().data.InstanceID = display[m].GetInstanceID().ToString();
            display[m].name = display[m].GetInstanceID().ToString();
            /*
            for (int p = 0; p < display.Count; p++)
                {
                    if(display[l].transform.parent.gameObject==display[p])
                    {
                        Debug.Log("parent instance = " + display[p].name + " child is: " + display[l].name);
                        break;
                    }
                }
            */
        }


    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CheckParent()
    {
        List<GameObject> checked2 = new List<GameObject>();

        for (int i = 0; i < display.Count; i++)
        {
            if(display[i].transform.childCount==0)
            {
                if(!checked2.Contains(display[i]))
                checked2.Add(display[i]);
            }
        }

        for (int i = 0; i < checked2.Count; i++)
        {
            GameObject obj = checked2[i];
            hierarchy.Add(new List<GameObject>());
            while (obj.transform.childCount != 0)
            {
                hierarchy[i].Add(obj);

                obj = obj.transform.GetChild(0).gameObject;
            }
        }
            
            /*
            while (obj.transform.parent != null)
                {
                    if(obj.transform.childCount!=0&&!checked2.Contains(obj.transform.GetChild(0).gameObject))
                    {
                        
                            if (display.Contains( obj.transform.GetChild(0).gameObject))
                            {
                        checked2.Add(obj.transform.GetChild(0).gameObject);

                        obj = obj.transform.GetChild(0).gameObject;
                                continue;
                            }
                        
                    }

                    // Debug.Log(obj);
                    for (int j = 0; j < display.Count; j++)
                    {
                        if (display[j] == obj.transform.parent.gameObject)
                        {

                            hierarchy[i].Add(obj.transform.parent.gameObject);
                            // Debug.Log(obj.transform.parent.gameObject.name);
                            Debug.Log(display[j]);
                            display.RemoveAt(j);
                            break;
                        }
                    }
                    obj = obj.transform.parent.gameObject;

                }
                hierarchy[i].Add(display[i]);
            */
            //Debug.Log(display[i]);
        
    }

    public static string GetPath(GameObject obj)
    {
        string path =  obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path =  obj.name +"/"+ path;
        }
        return path;
    }
}
