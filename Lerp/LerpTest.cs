using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
public class LerpTest : MonoBehaviour
{
    // Start is called before the first frame update



    [Header("Custom Curve UI")]
    public float min;
    public float max;
    public float mintime;
    public float maxtime;
    public TextMeshProUGUI[] holders;
    public TextMeshProUGUI[] holderstime;
    public Canvas canvas;
    public Scrollbar bar;
    public Scrollbar bartime;
    public RectTransform horizontal;
    public RectTransform vertical;
    public RectTransform timelinehandle;
    public TextMeshProUGUI timelinehandletext;

    [Space(20)]

    [Header("Data Holders")]

    public List<LerpData> lerpdataimport;
    [Space(20)]

    [Header("Instance Holders")]
    public List<List<List<GameObject>>> prefabpos=new List<List<List<GameObject>>>();
    public GameObject prefabcurve;
    public GameObject prefabanchor;
    public List<List<List<RectTransform>>> lines=new List<List<List<RectTransform>>>();
    public List<List<List<RectTransform>>> linesall = new List<List<List<RectTransform>>>();
    public List<List<List<GameObject>>> anchors= new List<List<List<GameObject>>>();
    public List<List<List<RectTransform>>> anchoredlines = new List<List<List<RectTransform>>>();

    //public List<GameObject> allgameobject;
    [Space(20)]
    [Header("Updater")]
    public List<List<bool>> needrenewal=new List<List<bool>>();
    public Zoom zoom;
    public List<List<bool>> movedobject = new List<List<bool>>();
    public List<List<bool>> update_anchorline = new List<List<bool>>();

    private List<List<bool>> smoothen = new List<List<bool>>();
    private List<List<bool>> once = new List<List<bool>>();
    [Space(20)]

    [Header("Export")]
    public List<List<float>> floatval = new List<List<float>>();
    public List<List<float>> timeval = new List<List<float>>();
    public TextMeshProUGUI timetext;
    public TextMeshProUGUI valuetext;
    public List<List<bool>> export = new List<List<bool>>();
    public List<List<string>> property = new List<List<string>>();

    public AnimationCurve output;
    public LerpData lerpdata;

    [Space(20)]
    [Header("Import")]
    public List<List<bool>> import = new List<List<bool>>();
    public List<List<bool>> Add = new List<List<bool>>();
    public List<List<int>> currentobject = new List<List<int>>();
    public List<List<bool>> Delete = new List<List<bool>>();
    public List<List<List<int>>> deleteindexes = new List<List<List<int>>>();

    [Space(20)]
    [Header("Add")]
    public List<Button> CustomButtons;
    public Transform buttonparent;
    public GameObject timelinebutton;
    public GameObject AddUI;
    [Space(20)]
    [Header("Display")]
    public bool switch_FPS;
    public bool usingFPS = true;
    public bool Timeline;
    public RectTransform middle;
    private bool DisplayX;
    private bool DisplayY;
    private bool DisplayZ;
    private bool DisplayPos;
    private bool DisplayRot;
    private bool DisplayIK;
    private List<bool> DisplayCustom=new List<bool>();
    public List<List<bool>> selected= new List<List<bool>>();

    public TMP_Dropdown Dropdown;


    public List<GameObject> mainanimator;
    public List<GameObject> sceneanimated;
    public Toggle usingAllAnims;
    public AnimationClip clip;
    public bool Exportall;

    public bool uses_entity_bones;
    public List<Transform> animatedbones;
    public float exportTime;
    public bool exporting;
    public bool exporttimeset;
    public float lowesttime;
    public float highesttime;
    public TimelineDrag dragV2;

    public AnimatorOverrideController anim;
    public Animator animator;
    public AnimationClip clip2;
    void Start()
    {
        DisplayPos = true;
        DisplayRot = true;
        DisplayX = true;
        DisplayY = true;
        DisplayZ = true;
        DisplayIK = true;
        for (int i = 0; i < DisplayCustom.Count; i++)
        {
            DisplayCustom[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Exportall)
        {
            //clip = new AnimationClip();
            foreach (List<bool> item in export)
            {

                for (int i = 0; i < item.Count; i++)
                {
                    item[i] = true;
                }
            }
            Exportall = false;
        }
        foreach (LerpData item in lerpdataimport)
        {
            if (item.IsMainAnimator)
                if(!mainanimator.Contains(GameObject.Find(item.InstanceID)))
                    mainanimator.Add(GameObject.Find(item.InstanceID));
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (AddUI.activeInHierarchy == true)
            {
                AddUI.SetActive(false);
                AddUI.transform.position = Input.mousePosition;
            }
            else
            {
                AddUI.transform.SetAsLastSibling();

                AddUI.SetActive(true);
                AddUI.transform.position = Input.mousePosition;
            }
        }
        #region Initialization
        for (int j = 0; j < lerpdataimport.Count; j++)
        {
            /*
            if (CustomButtons.Count != lerpdataimport[j].Customdata.Count)
            {
                foreach (Button item in CustomButtons)
                {
                    Destroy(item.gameObject);
                }
                for (int i = 0; i < lerpdataimport[j].Customdata.Count; i++)
                {
                    Button btn = Instantiate(timelinebutton, buttonparent).GetComponent<Button>();
                    CustomButtons.Add(btn);
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = ("Custom Data " + i);
                    btn.GetComponentInChildren<TimelineButton>().dataindex = i;
                }

            }
            if (DisplayCustom.Count != lerpdataimport[j].Customdata.Count)
            {
                DisplayCustom = new List<bool>();
                for (int i = 0; i < lerpdataimport[j].Customdata.Count; i++)
                {
                    DisplayCustom.Add(false);
                }
            }
            */
            if(prefabpos.Count<lerpdataimport.Count)
            {
                prefabpos.Add(new List<List<GameObject>>());
            }
            if(selected.Count<lerpdataimport.Count)
            {
                selected.Add(new List<bool>());
            }

            if(anchors.Count<lerpdataimport.Count)
            {
                anchors.Add(new List<List<GameObject>>());
            }

            if(lines.Count<lerpdataimport.Count)
            {
                lines.Add(new List<List<RectTransform>>());
            }
            if(linesall.Count<lerpdataimport.Count)
            {
                linesall.Add(new List<List<RectTransform>>());
            }
            if(anchoredlines.Count<lerpdataimport.Count)
            {
                anchoredlines.Add(new List<List<RectTransform>>());
            }

            if(needrenewal.Count<lerpdataimport.Count)
            {
                needrenewal.Add(new List<bool>());
            }
            if(movedobject.Count<lerpdataimport.Count)
            {
                movedobject.Add(new List<bool>());
            }
            if(update_anchorline.Count<lerpdataimport.Count)
            {
                update_anchorline.Add(new List<bool>());
            }
            if(smoothen.Count<lerpdataimport.Count)
            {
                smoothen.Add(new List<bool>());
            }
            if(once.Count<lerpdataimport.Count)
            {
                once.Add(new List<bool>());
            }

            if(floatval.Count<lerpdataimport.Count)
            {
                floatval.Add(new List<float>());
            }
            if(timeval.Count<lerpdataimport.Count)
            {
                timeval.Add(new List<float>());
            }
            if(property.Count<lerpdataimport.Count)
            {
                property.Add(new List<string>());
            }

            if(export.Count<lerpdataimport.Count)
            {
                export.Add(new List<bool>());
            }
            if(import.Count<lerpdataimport.Count)
            {
                import.Add(new List<bool>());
            }
            if(Add.Count<lerpdataimport.Count)
            {
                Add.Add(new List<bool>());
            }
            if(Delete.Count<lerpdataimport.Count)
            {
                Delete.Add(new List<bool>());
            }
            if(deleteindexes.Count<lerpdataimport.Count)
            {
                deleteindexes.Add(new List<List<int>>());
            }
            if(deleteindexes[j].Count<7+lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    deleteindexes[j].Add(new List<int>());
                }
            }

            if (prefabpos[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    prefabpos[j].Add(new List<GameObject>());
                }
            }
            if (selected[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    selected[j].Add(new bool());
                }
            }

            if (lines[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    lines[j].Add(new List<RectTransform>());
                }
            }
            if (linesall[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    linesall[j].Add(new List<RectTransform>());
                }
            }
            if (anchors[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    anchors[j].Add(new List<GameObject>());
                }
            }
            if (anchoredlines[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    anchoredlines[j].Add(new List<RectTransform>());
                }
            }

            if (needrenewal[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    needrenewal[j].Add(new bool());
                }
            }
            if (movedobject[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    movedobject[j].Add(new bool());
                }
            }
            if (update_anchorline[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    update_anchorline[j].Add(new bool());
                }
            }
            if (smoothen[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    smoothen[j].Add(new bool());
                }
            }
            if (once[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    once[j].Add(false);
                }
            }

            if (floatval[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    floatval[j].Add(new float());
                }
            }
            if (timeval[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    timeval[j].Add(new float());
                }
            }
            if (property[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    property[j].Add("");
                }
            }

            if (export[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    export[j].Add(new bool());
                }
            }
            if (import[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    import[j].Add(true);
                }
            }
            if (Add[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    Add[j].Add(new bool());
                }
            }
            if (Delete[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    Delete[j].Add(new bool());
                }
            }


            #endregion
            if (!exporting)
            {
                if (DisplayPos)
                {
                    if (DisplayX)
                    {
                        Curving(lerpdataimport[j].PositionX.numbers, lerpdataimport[j].PositionX.numberscurve, lerpdataimport[j].PositionX.curvingpos, lerpdataimport[j].PositionX.curvingposanchor, lerpdataimport[j].PositionX.SmoothArray, false, 0, lerpdataimport[j].Posxcol, j);
                    }
                    else
                    {
                        if (prefabpos[j][0].Count > 0)
                        {
                            foreach (GameObject item in prefabpos[j][0])
                            {
                                Destroy(item);
                            }
                            prefabpos[j][0].Clear();
                        }
                        if (anchors[j][0].Count > 0)
                        {
                            foreach (GameObject item in anchors[j][0])
                            {
                                Destroy(item);
                            }
                            anchors[j][0].Clear();
                        }
                        if (lines[j][0].Count > 0)
                        {

                            foreach (RectTransform item in lines[j][0])
                            {
                                Destroy(item.gameObject);
                            }
                            lines[j][0].Clear();
                        }
                        if (linesall[j][0].Count > 0)
                        {
                            foreach (RectTransform item in linesall[j][0])
                            {
                                Destroy(item.gameObject);
                            }
                            linesall[j][0].Clear();
                        }
                        if (anchoredlines[j][0].Count > 0)
                        {
                            foreach (RectTransform item in anchoredlines[j][0])
                            {
                                Destroy(item.gameObject);
                            }
                            anchoredlines[j][0].Clear();
                        }
                    }
                    if (DisplayY)
                    {
                        Curving(lerpdataimport[j].PositionY.numbers, lerpdataimport[j].PositionY.numberscurve, lerpdataimport[j].PositionY.curvingpos, lerpdataimport[j].PositionY.curvingposanchor, lerpdataimport[j].PositionY.SmoothArray, false, 1, lerpdataimport[j].Posycol, j);
                    }
                    else
                    {
                        if (prefabpos[j][1].Count > 0)
                        {
                            foreach (GameObject item in prefabpos[j][1])
                            {
                                Destroy(item);
                            }
                            prefabpos[j][1] = new List<GameObject>();
                        }
                        if (anchors[j][1].Count > 0)
                        {
                            foreach (GameObject item in anchors[j][1])
                            {
                                Destroy(item);
                            }
                            anchors[j][1] = new List<GameObject>();
                        }
                        if (lines[j][1].Count > 0)
                        {

                            foreach (RectTransform item in lines[j][1])
                            {
                                Destroy(item.gameObject);
                            }
                            lines[j][1] = new List<RectTransform>();
                        }
                        if (linesall[j][1].Count > 0)
                        {
                            foreach (RectTransform item in linesall[j][1])
                            {
                                Destroy(item.gameObject);
                            }
                            linesall[j][1] = new List<RectTransform>();
                        }
                        if (anchoredlines[j][1].Count > 0)
                        {
                            foreach (RectTransform item in anchoredlines[j][1])
                            {
                                Destroy(item.gameObject);
                            }
                            anchoredlines[j][1] = new List<RectTransform>();
                        }
                    }
                    if (DisplayZ)
                    {
                        Curving(lerpdataimport[j].PositionZ.numbers, lerpdataimport[j].PositionZ.numberscurve, lerpdataimport[j].PositionZ.curvingpos, lerpdataimport[j].PositionZ.curvingposanchor, lerpdataimport[j].PositionZ.SmoothArray, false, 2, lerpdataimport[j].Poszcol, j);
                    }
                    else
                    {
                        if (prefabpos[j][2].Count > 0)
                        {
                            foreach (GameObject item in prefabpos[j][2])
                            {
                                Destroy(item);
                            }
                            prefabpos[j][2].Clear();
                        }
                        if (anchors[j][2].Count > 0)
                        {
                            foreach (GameObject item in anchors[j][2])
                            {
                                Destroy(item);
                            }
                            anchors[j][2].Clear();
                        }
                        if (lines[j][2].Count > 0)
                        {

                            foreach (RectTransform item in lines[j][2])
                            {
                                Destroy(item.gameObject);
                            }
                            lines[j][2].Clear();
                        }
                        if (linesall[j][2].Count > 0)
                        {
                            foreach (RectTransform item in linesall[j][2])
                            {
                                Destroy(item.gameObject);
                            }
                            linesall[j][2].Clear();
                        }
                        if (anchoredlines[j][2].Count > 0)
                        {
                            foreach (RectTransform item in anchoredlines[j][2])
                            {
                                Destroy(item.gameObject);
                            }
                            anchoredlines[j][2].Clear();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (prefabpos[j][i].Count > 0)
                        {
                            foreach (GameObject item in prefabpos[j][i])
                            {
                                Destroy(item);
                            }
                            prefabpos[j][i].Clear();
                        }
                        if (anchors[j][i].Count > 0)
                        {
                            foreach (GameObject item in anchors[j][i])
                            {
                                Destroy(item);
                            }
                            anchors[j][i].Clear();
                        }
                        if (lines[j][i].Count > 0)
                        {

                            foreach (RectTransform item in lines[j][i])
                            {
                                Destroy(item.gameObject);
                            }
                            lines[j][i].Clear();
                        }
                        if (linesall[j][i].Count > 0)
                        {
                            foreach (RectTransform item in linesall[j][i])
                            {
                                Destroy(item.gameObject);
                            }
                            linesall[j][i].Clear();
                        }
                        if (anchoredlines[j][i].Count > 0)
                        {
                            foreach (RectTransform item in anchoredlines[j][i])
                            {
                                Destroy(item.gameObject);
                            }
                            anchoredlines[j][i].Clear();
                        }
                    }


                }
                // Curving(lerpdataimport.RotationX.numbers, lerpdataimport.RotationX.numberscurve, lerpdataimport.RotationX.curvingpos, lerpdataimport.RotationX.curvingposanchor, lerpdataimport.RotationX.SmoothArray, false, 3, lerpdataimport.Rotxcol);
                // Curving(lerpdataimport.RotationY.numbers, lerpdataimport.RotationY.numberscurve, lerpdataimport.RotationY.curvingpos, lerpdataimport.RotationY.curvingposanchor, lerpdataimport.RotationY.SmoothArray, false, 4, lerpdataimport.Rotycol);
                // Curving(lerpdataimport.RotationZ.numbers, lerpdataimport.RotationZ.numberscurve, lerpdataimport.RotationZ.curvingpos, lerpdataimport.RotationZ.curvingposanchor, lerpdataimport.RotationZ.SmoothArray, false, 5, lerpdataimport.Rotzcol);
                // Curving(lerpdataimport.IKVal.numbers, lerpdataimport.IKVal.numberscurve, lerpdataimport.IKVal.curvingpos, lerpdataimport.IKVal.curvingposanchor, lerpdataimport.IKVal.SmoothArray, false, 6, lerpdataimport.IKcol);

                for (int i = 0; i < lerpdataimport[j].Customdata.Count; i++)
                {
                    //if(DisplayCustom[i])
                    //Curving(lerpdataimport.Customdata[i].numbers, lerpdataimport.Customdata[i].numberscurve, lerpdataimport.Customdata[i].curvingpos, lerpdataimport.Customdata[i].curvingposanchor, lerpdataimport.Customdata[i].SmoothArray, true, i, lerpdataimport.Customdatacolor[i]);

                }

            }
            else
            {
                Exporting();
                exporting = false;
            }

        }
    }
    
    public void AddKey(int dataindex,bool custom,bool Allpos,bool allrot, bool allcustom,bool all)
    {
        for (int k = 0; k < lerpdataimport.Count; k++)
        {


            if (custom == false)
            {
                if (all && dataindex == 0)
                {
                    for (int i = 0; i < Add.Count; i++)
                    {
                        Add[k][i] = true;
                    }
                }
                else if (all && dataindex == 1)
                {
                    Add[k][0] = true;
                    Add[k][3] = true;
                }
                else if (all && dataindex == 2)
                {
                    Add[k][1] = true;
                    Add[k][4] = true;
                }
                else if (all && dataindex == 3)
                {
                    Add[k][2] = true;
                    Add[k][5] = true;
                }
                else if (Allpos)
                {
                    Add[k][0] = true;
                    Add[k][1] = true;
                    Add[k][2] = true;
                }
                else if (allrot)
                {
                    Add[k][3] = true;
                    Add[k][4] = true;
                    Add[k][5] = true;
                }
                else
                {
                    switch (dataindex)
                    {
                        case 0:
                            Add[k][0] = true;
                            break;
                        case 1:
                            Add[k][1] = true;
                            break;
                        case 2:
                            Add[k][2] = true;
                            break;
                        case 3:
                            Add[k][3] = true;
                            break;
                        case 4:
                            Add[k][4] = true;
                            break;
                        case 5:
                            Add[k][5] = true;
                            break;
                        case 6:
                            Add[k][6] = true;
                            break;

                            break;

                    }
                }
            }
            else
            {
                Add[k][dataindex + 7] = true;
            }
        }
        AddUI.SetActive(false);
    }
    public void Curving(List<Vector2> numbers, List<Vector2> numberscurve, Vector2[] curvingpos, Vector2[] curvingposanchor, SmoothLine[] SmoothArray, bool isList, int o, Color color,int dataindex)
    {
        if (prefabpos[dataindex][o].Count > 0)
        {
            for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
            {
                if (prefabpos[dataindex][o][i].GetComponent<anchordrag>().draggable == true)
                {
                    if (!deleteindexes[dataindex][o].Contains(i))
                    {
                        deleteindexes[dataindex][o].Add(i);
                    }
                }
                else
                {
                    deleteindexes[dataindex][o].Remove(i);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Delete) && deleteindexes[dataindex][o].Count > 0)
        {
            for (int i = 0; i < deleteindexes[dataindex][o].Count; i++)
            {
                Debug.Log(deleteindexes[dataindex][o][i] - (i));
                numbers.RemoveAt(deleteindexes[dataindex][o][i] - (i));
                if (deleteindexes[dataindex][o][i] == 0)
                {
                    numberscurve.RemoveAt(deleteindexes[dataindex][o][i]);
                    numberscurve.RemoveAt(deleteindexes[dataindex][o][i]);
                }
                else if (deleteindexes[dataindex][o][i] - (i) == numbers.Count)
                {
                    numberscurve.RemoveAt(numberscurve.Count - 1);
                    numberscurve.RemoveAt(numberscurve.Count - 1);
                    Debug.Log("end");
                }
                else
                {
                    Debug.Log("middle");

                    numberscurve.RemoveAt((((deleteindexes[dataindex][o][i] - i) * 2) - 1));
                    numberscurve.RemoveAt((((deleteindexes[dataindex][o][i] - i) * 2) - 1));

                }

            }
            import[dataindex][o] = true;
            deleteindexes[dataindex][o].Clear();
        }

        if (Add[dataindex][o])
        {
            int currentiteration = 0;

            for (int i = 0; i < numbers.Count; i++)
            {
                if (timeval[dataindex][o] >= numbers[i].x)
                {
                    if ((currentiteration == numbers.Count-1))
                    {
                        numberscurve.Add(new Vector2(numbers[currentiteration].x + 3.016343f, numbers[currentiteration].y));
                        numberscurve.Add(new Vector2(timeval[dataindex][o] - 3.016343f, numbers[currentiteration].y));
                        numbers.Add(new Vector2(timeval[dataindex][o], numbers[currentiteration].y));
                     //   Debug.Log("end");
                        break;
                    }
                    currentiteration += 1;

                }
                else
                {
                   // Debug.Log("timeval:" + timeval[dataindex][o] + "num:" + numbers[i].x + "/" + currentiteration + "//" + numbers.Count);
                    if (currentiteration == 0)
                    {
                        numbers.Insert(0, new Vector2(timeval[dataindex][o], numbers[currentiteration].y));
                        numberscurve.Insert(currentiteration, new Vector2(numbers[1].x - 3.016343f, numbers[1].y));
                        numberscurve.Insert(currentiteration, new Vector2(timeval[dataindex][o] + 3.016343f, numbers[currentiteration].y));
                        break;
                    }
                    else if ((currentiteration < numbers.Count))
                    {
                        numbers.Insert(currentiteration, new Vector2(timeval[dataindex][o], floatval[dataindex][o]));
                        numberscurve.Insert(((currentiteration) * 2) - 1, new Vector2(timeval[dataindex][o] + 3.016343f, floatval[dataindex][o]));
                        numberscurve.Insert(((currentiteration) * 2) - 1, new Vector2(timeval[dataindex][o] - 3.0163431f, floatval[dataindex][o]));
                      //  Debug.Log("between");
                        break;
                    }
                    

                    
                }


            }


            Add[dataindex][o] = false;
            //prefabpos.Add(Instantiate(prefabcurve, canvas.transform));
            //needrenewal = true;
            import[dataindex][o] = true;
        }
        if (switch_FPS)
        {
            if (usingFPS == true)
            {

                for (int i = 0; i < numbers.Count; i++)
                {
                    numbers[i] = new Vector2(numbers[i].x / zoom.FPS, numbers[i].y);
                }
                for (int i = 0; i < numberscurve.Count; i++)
                {
                    numberscurve[i] = new Vector2(numberscurve[i].x / zoom.FPS, numberscurve[i].y);
                }

                switch_FPS = false;
                usingFPS = false;
            }
            else
            {

                for (int i = 0; i < numbers.Count; i++)
                {
                    numbers[i] = new Vector2(numbers[i].x * zoom.FPS, numbers[i].y);
                }
                for (int i = 0; i < numberscurve.Count; i++)
                {
                    numberscurve[i] = new Vector2(numberscurve[i].x * zoom.FPS, numberscurve[i].y);
                }

                switch_FPS = false;
                usingFPS = true;
            }
        }
        if (import[dataindex][o] == true)
        {
            foreach (GameObject item in prefabpos[dataindex][o])
            {
                Destroy(item);
            }

            foreach (RectTransform item in lines[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            foreach (RectTransform item in linesall[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            foreach (RectTransform item in anchoredlines[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            foreach (GameObject item in anchors[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            if (numberscurve.Count == 0 || numberscurve.Count != ((numbers.Count*2)-2))
            {
                once[dataindex][o] = false;
                    import[dataindex][o] = false;

            }
            else
            {
                once[dataindex][o] = true;
                lines[dataindex][o].Clear();
                linesall[dataindex][o].Clear();
                anchors[dataindex][o].Clear();
                anchoredlines[dataindex][o].Clear();
                //allgameobject.Clear();
                prefabpos[dataindex][o] = new List<GameObject>();
                anchors[dataindex][o] = new List<GameObject>();

                //Debug.Log(lerpdataimport.data[0].numbers.Length);
                for (int i = 0; i < numbers.Count; i++)
                {

                    prefabpos[dataindex][o].Add(Instantiate(prefabcurve, canvas.transform));
                    //anchors.Add(prefabpos[i]);//////

                }
                for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
                {
                    if (i == 0)
                    {
                        //allgameobject.Add(prefabpos[i]);
                        GameObject anchorR = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x + 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                        //allgameobject.Add(anchorR);
                        anchors[dataindex][o].Add(anchorR);
                    }
                    else if (i == prefabpos[dataindex][o].Count - 1)
                    {
                        GameObject anchorL = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x - 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                        //allgameobject.Add(anchorL);
                        anchors[dataindex][o].Add(anchorL);
                        //allgameobject.Add(prefabpos[i]);

                    }
                    else
                    {
                        GameObject anchorL = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x - 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                        //allgameobject.Add(anchorL);
                        //allgameobject.Add(prefabpos[i]);
                        GameObject anchorR = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x + 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                        //allgameobject.Add(anchorR);
                        anchors[dataindex][o].Add(anchorL);
                        anchors[dataindex][o].Add(anchorR);


                    }
                }
                //Debug.Log("success");
                Add[dataindex][o] = false;
                import[dataindex][o] = false;
                //once[o]=true;
                smoothen[dataindex][o] = true;
                //Debug.Log("import");
            }
        }

        #region Moved
        if (movedobject[dataindex][o])
        {



            for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
            {
                if (prefabpos[dataindex][o][i].GetComponent<Image>().enabled == true)
                {
                    int currentval2 = 0;
                    for (int j = 0; j < holders.Length; j++)
                    {


                        if (prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.y > holders[j].GetComponent<RectTransform>().position.y)
                        {


                            currentval2 += 1;
                            continue;
                        }

                        float val = Mathf.InverseLerp(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].GetComponent<RectTransform>().position.y, holders[currentval2].GetComponent<RectTransform>().position.y, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.y);

                        float positionval = Mathf.Lerp(float.Parse(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].text), float.Parse(holders[currentval2].text), val);
                        // Debug.Log("I= " + i + " value= " + val + " position Y=" + positionval);
                        numbers[i] = new Vector2(numbers[i].x, positionval);
                    }
                    int currentval22 = 0;
                    for (int j = 0; j < holderstime.Length; j++)
                    {
                        if (prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.x > holderstime[j].GetComponent<RectTransform>().position.x)
                        {


                            currentval22 += 1;
                            continue;
                        }
                        float val = Mathf.InverseLerp(holderstime[Mathf.Clamp(currentval22 - 1, 0, holders.Length)].GetComponent<RectTransform>().position.x, holderstime[currentval22].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.x);

                        float positionval = Mathf.Lerp(float.Parse(holderstime[Mathf.Clamp(currentval22 - 1, 0, holders.Length)].text), float.Parse(holderstime[currentval22].text), val);
                        //Debug.Log("I= " + i + " value= " + val + " position X=" + positionval);
                        numbers[i] = new Vector2(positionval, numbers[i].y);

                    }
                }
            }

            for (int i = 0; i < anchors[dataindex][o].Count; i++)
            {
                if (anchors[dataindex][o][i].GetComponent<Image>().enabled == true)
                {
                    int currentval2 = 0;
                    for (int j = 0; j < holders.Length; j++)
                    {


                        if (anchors[dataindex][o][i].GetComponent<RectTransform>().position.y > holders[j].GetComponent<RectTransform>().position.y)
                        {


                            currentval2 += 1;
                            continue;
                        }

                        float val = Mathf.InverseLerp(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].GetComponent<RectTransform>().position.y, holders[currentval2].GetComponent<RectTransform>().position.y, anchors[dataindex][o][i].GetComponent<RectTransform>().position.y);

                        float positionval = Mathf.Lerp(float.Parse(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].text), float.Parse(holders[currentval2].text), val);
                        // Debug.Log("I= " + i + " value= " + val + " position Y=" + positionval);
                        numberscurve[i] = new Vector2(numberscurve[i].x, positionval);
                    }
                    int currentval22 = 0;
                    for (int j = 0; j < holderstime.Length; j++)
                    {
                        if (anchors[dataindex][o][i].GetComponent<RectTransform>().position.x > holderstime[j].GetComponent<RectTransform>().position.x)
                        {


                            currentval22 += 1;
                            continue;
                        }
                        float val = Mathf.InverseLerp(holderstime[Mathf.Clamp(currentval22 - 1, 0, holders.Length)].GetComponent<RectTransform>().position.x, holderstime[currentval22].GetComponent<RectTransform>().position.x, anchors[dataindex][o][i].GetComponent<RectTransform>().position.x);

                        float positionval = Mathf.Lerp(float.Parse(holderstime[Mathf.Clamp(currentval22 - 1, 0, holders.Length)].text), float.Parse(holderstime[currentval22].text), val);
                        //Debug.Log("I= " + i + " value= " + val + " position X=" + positionval);
                        numberscurve[i] = new Vector2(positionval, numberscurve[i].y);

                    }

                }
            }
            movedobject[dataindex][o] = false;
        }
        #endregion
        #region Sort

        /*
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i - 1].x > numbers[i].x)
            {
                float firstx = numbers[i - 1].x;
                float firsty = numbers[i - 1].y;
                float secondx = numbers[i].x;
                float secondy = numbers[i].y;
                numbers[i - 1] = new Vector2(secondx, secondy);
                numbers[i] = new Vector2(firstx, firsty);
               

            }
        }
        */

        #endregion
        #region Instances
        if (prefabpos[dataindex][o].Count != numbers.Count)
        {
            //Debug.Log("test");
            //Debug.Log("tes");
            //Debug.Log("error");
            foreach (GameObject item in prefabpos[dataindex][o])
            {
                Destroy(item);
            }
            foreach (GameObject item in anchors[dataindex][o])
            {
                Destroy(item);
            }

            foreach (RectTransform item in lines[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            foreach (RectTransform item in linesall[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            foreach (RectTransform item in anchoredlines[dataindex][o])
            {
                Destroy(item.gameObject);
            }
            once[dataindex][o] = false;
            lines[dataindex][o].Clear();
            linesall[dataindex][o].Clear();
            anchors[dataindex][o].Clear();
            anchoredlines[dataindex][o].Clear();
            //allgameobject.Clear();
            numberscurve = new List<Vector2>();
            curvingpos = new Vector2[0];
            curvingposanchor = new Vector2[0];
            SmoothArray = new SmoothLine[0];
            prefabpos[dataindex][o] = new List<GameObject>();

            for (int i = 0; i < numbers.Count; i++)
            {

                prefabpos[dataindex][o].Add(Instantiate(prefabcurve, canvas.transform));
                //anchors.Add(prefabpos[i]);//////


            }

           // Debug.Log("instance");

        }
        if (once[dataindex][o] == false)
        {
            //Invoke("Renew", 0.05f);
            StartCoroutine(Renew(o, 0.05f, dataindex));
        }
        if (needrenewal[dataindex][o])
        {
            
            foreach (GameObject item in anchors[dataindex][o])
            {
                Destroy(item);

            }
            
            numberscurve = new List<Vector2>();

            for (int i = 0; i < lines[dataindex][o].Count; i++)
            {
                lines[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            anchors[dataindex][o].Clear();
            //allgameobject.Clear();

            for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
            {
                if (i == 0)
                {
                    //allgameobject.Add(prefabpos[i]);
                    GameObject anchorR = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x + 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                    //allgameobject.Add(anchorR);
                    anchors[dataindex][o].Add(anchorR);
                }
                else if (i == prefabpos[dataindex][o].Count - 1)
                {
                    GameObject anchorL = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x - 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                    //allgameobject.Add(anchorL);
                    anchors[dataindex][o].Add(anchorL);
                    //allgameobject.Add(prefabpos[i]);
                }
                else
                {
                    GameObject anchorL = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x - 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                    //allgameobject.Add(anchorL);
                    //allgameobject.Add(prefabpos[i]);
                    GameObject anchorR = Instantiate(prefabanchor, new Vector2(prefabpos[dataindex][o][i].transform.position.x + 50, prefabpos[dataindex][o][i].transform.position.y), Quaternion.identity, prefabpos[dataindex][o][i].transform);
                    //allgameobject.Add(anchorR);
                    anchors[dataindex][o].Add(anchorL);
                    anchors[dataindex][o].Add(anchorR);
                }
               // Debug.Log(prefabpos[dataindex][o].Count);
            }

            for (int i = 0; i < anchors[dataindex][o].Count; i++)
            {
                numberscurve.Add(new Vector2());
            }
            for (int i = 0; i < anchors[dataindex][o].Count; i++)
            {

                int currentval2 = 0;
                for (int j = 0; j < holders.Length; j++)
                {


                    if (anchors[dataindex][o][i].GetComponent<RectTransform>().position.y > holders[j].GetComponent<RectTransform>().position.y)
                    {


                        currentval2 += 1;
                        continue;
                    }

                    float val = Mathf.InverseLerp(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].GetComponent<RectTransform>().position.y, holders[currentval2].GetComponent<RectTransform>().position.y, anchors[dataindex][o][i].GetComponent<RectTransform>().position.y);

                    float positionval = Mathf.Lerp(float.Parse(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].text), float.Parse(holders[currentval2].text), val);
                    // Debug.Log("I= " + i + " value= " + val + " position Y=" + positionval);
                    numberscurve[i] = new Vector2(numberscurve[i].x, positionval);
                }
                int currentval22 = 0;
                for (int j = 0; j < holderstime.Length; j++)
                {
                    if (anchors[dataindex][o][i].GetComponent<RectTransform>().position.x > holderstime[j].GetComponent<RectTransform>().position.x)
                    {


                        currentval22 += 1;
                        continue;
                    }
                    float val = Mathf.InverseLerp(holderstime[Mathf.Clamp(currentval22 - 1, 0, holders.Length)].GetComponent<RectTransform>().position.x, holderstime[currentval22].GetComponent<RectTransform>().position.x, anchors[dataindex][o][i].GetComponent<RectTransform>().position.x);

                    float positionval = Mathf.Lerp(float.Parse(holderstime[Mathf.Clamp(currentval22 - 1, 0, holders.Length)].text), float.Parse(holderstime[currentval22].text), val);
                    //Debug.Log("I= " + i + " value= " + val + " position X=" + positionval);
                    numberscurve[i] = new Vector2(positionval, numberscurve[i].y);

                }

            }

           // Debug.Log("renewal");
            needrenewal[dataindex][o] = false;
            smoothen[dataindex][o] = true;
        }

        #endregion






        min = float.Parse(holders[holders.Length - 1].text);
        max = float.Parse(holders[0].text);
        mintime = float.Parse(holderstime[0].text);
        maxtime = float.Parse(holderstime[holderstime.Length - 1].text);



        #region UI
        #region Curved Coordinate
        //Add weights/ Anchor
        if (linesall[dataindex][o].Count <= 0 && smoothen[dataindex][o] == true && prefabpos[dataindex][o].Count > 0)//&& allgameobject.Count > 0)
        {
            SmoothArray = new SmoothLine[prefabpos[dataindex][o].Count - 1];
            //realSmoothArray = new SmoothLine[allgameobject.Count-1];
            for (int i = 1; i < SmoothArray.Length + 1; i++)
            {
                //Vector2[] point;
                Vector2[] point = { prefabpos[dataindex][o][i - 1].GetComponent<RectTransform>().position, anchors[dataindex][o][(i - 1) * 2].GetComponent<RectTransform>().position, anchors[dataindex][o][((i - 1) * 2) + 1].GetComponent<RectTransform>().position, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position };

                SmoothArray[i - 1] = new SmoothLine(point, MakeSmoothCurveVector2(point, 5));
                //realSmoothArray[i] = new SmoothLine(point,MakeSmoothCurveVector2(point, 2)); 
                //Debug.Log(i);
                for (int j = 1; j < SmoothArray[i - 1].smooths.Length; j++)
                {
                    CreateConnectionall(SmoothArray[i - 1].smooths[j - 1], SmoothArray[i - 1].smooths[j], color, o,dataindex);
                }
            }
            smoothen[dataindex][o] = false;
        }

        for (int i = 1; i < SmoothArray.Length +1; i++)
        {
            //Vector2[] point;
            Vector2[] point = { prefabpos[dataindex][o][i - 1].GetComponent<RectTransform>().position, anchors[dataindex][o][(i - 1) * 2].GetComponent<RectTransform>().position, anchors[dataindex][o][((i - 1) * 2) + 1].GetComponent<RectTransform>().position, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position };

            Vector2[] test = MakeSmoothCurveVector2(point, 5);

            //realSmoothArray[i] = new SmoothLine(point,MakeSmoothCurveVector2(point, 2)); 
            //Debug.Log(i);
            for (int j = 1; j < test.Length; j++)
            {
                //Debug.Log(j - 1 + (SmoothArray[i - 1].smooths.Length * (i - 1)));
                //  if((i-1)>0)
                KeepConnection(test[j - 1], test[j], linesall[dataindex][o][j - 1 + ((SmoothArray[i - 1].smooths.Length - 1) * (i - 1))], linesall[dataindex][o][j - 1 + ((SmoothArray[i - 1].smooths.Length - 1) * (i - 1))].GetComponent<Image>().color,selected[dataindex][o]);
                //  else
                //      KeepConnection(test[j - 1], test[j], linesall[j - 1]);
            }
        }
        #endregion

        #region AnchorLine
        int beyond4 = 0;
        if (update_anchorline[dataindex][o])
        {
            for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
            {
                if (i == 0)
                {
                    CreateConnectionanchor(prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, anchors[dataindex][o][i].GetComponent<RectTransform>().position, o,dataindex);
                }
                else if (i == prefabpos[dataindex][o].Count - 1)
                {
                    CreateConnectionanchor(prefabpos[dataindex][o][prefabpos.Count - 1].GetComponent<RectTransform>().position, anchors[dataindex][o][anchors.Count - 1].GetComponent<RectTransform>().position, o, dataindex);
                    break;
                }
                else
                {
                    CreateConnectionanchor(prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, anchors[dataindex][o][i + beyond4].GetComponent<RectTransform>().position, o, dataindex);
                    CreateConnectionanchor(prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, anchors[dataindex][o][i + beyond4 + 1].GetComponent<RectTransform>().position, o, dataindex);
                    beyond4 += 1;

                }
            }
            update_anchorline[dataindex][o] = false;
        }

        else if (anchoredlines[dataindex][o].Count > 0)
        {
            int beyond5 = 0;

            for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
            {
                if (i == 0)
                {
                    KeepConnection(prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, anchors[dataindex][o][i].GetComponent<RectTransform>().position, anchoredlines[dataindex][o][i], anchoredlines[dataindex][o][i].GetComponent<Image>().color,selected[dataindex][o]);
                }
                else if (i == prefabpos[dataindex][o].Count - 1)
                {
                    KeepConnection(prefabpos[dataindex][o][prefabpos.Count - 1].GetComponent<RectTransform>().position, anchors[dataindex][o][anchors.Count - 1].GetComponent<RectTransform>().position, anchoredlines[dataindex][o][anchoredlines.Count - 1], anchoredlines[dataindex][o][i].GetComponent<Image>().color, selected[dataindex][o]);
                    break;
                }
                else
                {
                    KeepConnection(prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, anchors[dataindex][o][i + beyond5].GetComponent<RectTransform>().position, anchoredlines[dataindex][o][i + beyond5], anchoredlines[dataindex][o][i].GetComponent<Image>().color, selected[dataindex][o]);
                    KeepConnection(prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, anchors[dataindex][o][i + beyond5 + 1].GetComponent<RectTransform>().position, anchoredlines[dataindex][o][i + 1 + beyond5], anchoredlines[dataindex][o][i].GetComponent<Image>().color, selected[dataindex][o]);
                    beyond5 += 1;

                }
            }
        }
        #endregion


        timetext.text = timeval[dataindex].ToString();
        valuetext.text = floatval[dataindex].ToString();

        

        #region Coordinate values
        curvingpos = new Vector2[numbers.Count];
        curvingposanchor = new Vector2[numberscurve.Count];


        for (int i = 0; i < numbers.Count; i++)
        {

            int currentval2 = 0;
            for (int j = 0; j < holders.Length; j++)
            {


                if (numbers[i].y > float.Parse(holders[j].text))
                {

                    if (numbers[i].y > float.Parse(holders[holders.Length - 1].text))
                    {
                        curvingpos[i].y = holders[holders.Length - 1].rectTransform.position.y;
                        prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;

                    }

                    currentval2 += 1;
                    continue;
                }

                float val = Mathf.InverseLerp(float.Parse(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].text), float.Parse(holders[currentval2].text), numbers[i].y);

                float positionval = Mathf.Lerp(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].rectTransform.position.y, holders[currentval2].rectTransform.position.y, val);
                // Debug.Log("I= " + i + " value= " + val + " position Y=" + positionval);
                curvingpos[i].y = positionval;
            }
            int currentval22 = 0;
            for (int j = 0; j < holderstime.Length; j++)
            {
                if (numbers[i].x > float.Parse(holderstime[j].text))
                {

                    if (numbers[i].x > float.Parse(holderstime[holderstime.Length - 1].text))
                    {
                        curvingpos[i].x = holderstime[holderstime.Length - 1].rectTransform.position.x;
                        prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;

                    }

                    currentval22 += 1;
                    continue;
                }
                float val = Mathf.InverseLerp(float.Parse(holderstime[Mathf.Clamp(currentval22 - 1, 0, holderstime.Length)].text), float.Parse(holderstime[currentval22].text), numbers[i].x);

                float positionval = Mathf.Lerp(holderstime[Mathf.Clamp(currentval22 - 1, 0, holderstime.Length)].rectTransform.position.x, holderstime[currentval22].rectTransform.position.x, val);
                //Debug.Log("I= " + i + " value= " + val + " position X=" + positionval);
                curvingpos[i].x = positionval;

            }

        }

        for (int i = 0; i < numberscurve.Count; i++)
        {

            int currentval2 = 0;
            for (int j = 0; j < holders.Length; j++)
            {


                if (numberscurve[i].y > float.Parse(holders[j].text))
                {

                    if (numberscurve[i].y > float.Parse(holders[holders.Length - 1].text))
                    {
                        curvingposanchor[i].y = holders[holders.Length - 1].rectTransform.position.y;
                        //prefabpos[i].GetComponent<Image>().enabled = false;

                    }

                    currentval2 += 1;
                    continue;
                }

                float val = Mathf.InverseLerp(float.Parse(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].text), float.Parse(holders[currentval2].text), numberscurve[i].y);

                float positionval = Mathf.Lerp(holders[Mathf.Clamp(currentval2 - 1, 0, holders.Length)].rectTransform.position.y, holders[currentval2].rectTransform.position.y, val);
                // Debug.Log("I= " + i + " value= " + val + " position Y=" + positionval);
                curvingposanchor[i].y = positionval;
            }
            int currentval22 = 0;
            for (int j = 0; j < holderstime.Length; j++)
            {
                if (numberscurve[i].x > float.Parse(holderstime[j].text))
                {

                    if (numberscurve[i].x > float.Parse(holderstime[holderstime.Length - 1].text))
                    {
                        curvingposanchor[i].x = holderstime[holderstime.Length - 1].rectTransform.position.x;
                        // prefabpos[i].GetComponent<Image>().enabled = false;

                    }

                    currentval22 += 1;
                    continue;
                }
                float val = Mathf.InverseLerp(float.Parse(holderstime[Mathf.Clamp(currentval22 - 1, 0, holderstime.Length)].text), float.Parse(holderstime[currentval22].text), numberscurve[i].x);

                float positionval = Mathf.Lerp(holderstime[Mathf.Clamp(currentval22 - 1, 0, holderstime.Length)].rectTransform.position.x, holderstime[currentval22].rectTransform.position.x, val);
                //Debug.Log("I= " + i + " value= " + val + " position X=" + positionval);
                curvingposanchor[i].x = positionval;

            }

        }



        #endregion



        #region Original Coordinate
        for (int i = 0; i < curvingpos.Length; i++)
        {
            if (!Timeline)
            {
                prefabpos[dataindex][o][i].GetComponent<RectTransform>().position = curvingpos[i];
            }
            else
            {
                prefabpos[dataindex][o][i].GetComponent<RectTransform>().position = new Vector2(curvingpos[i].x, middle.position.y);

            }
        }
        //Debug.Log(curvingposanchor.Length +"/" + anchors.Count);
        for (int i = 0; i < curvingposanchor.Length; i++)
        {
            anchors[dataindex][o][i].GetComponent<RectTransform>().position = curvingposanchor[i];

        }

        if (lines[dataindex][o].Count == 0)
        {
            for (int i = 1; i < prefabpos[dataindex][o].Count; i++)
            {


                Vector2 A = prefabpos[dataindex][o][i - 1].GetComponent<RectTransform>().anchoredPosition;
                Vector2 B = prefabpos[dataindex][o][i].GetComponent<RectTransform>().anchoredPosition;
                //Debug.Log(A);
                CreateConnection(A, B, color, o,dataindex);

            }
        }
        for (int i = 0; i < lines[dataindex][o].Count; i++)
        {
            lines[dataindex][o][i].GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
        {

            if (i >= 1)
            {
                KeepConnection(prefabpos[dataindex][o][i - 1].GetComponent<RectTransform>().position, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position, lines[dataindex][o][i - 1], lines[dataindex][o][i - 1].GetComponent<Image>().color,selected[dataindex][o]);
            }
        }
        #endregion




        if (!exporting)
        {
            int currentval224 = 0;

            for (int j = 0; j < holderstime.Length; j++)
            {
                if (timelinehandle.position.x > holderstime[j].rectTransform.position.x)
                {


                    currentval224 += 1;
                    continue;
                }
                float val = Mathf.InverseLerp(holderstime[Mathf.Clamp(currentval224 - 1, 0, holderstime.Length)].rectTransform.position.x, holderstime[currentval224].rectTransform.position.x, timelinehandle.position.x);

                float positionval = Mathf.Lerp(float.Parse(holderstime[Mathf.Clamp(currentval224 - 1, 0, holderstime.Length)].text), float.Parse(holderstime[currentval224].text), val);
                 
                timeval[dataindex][o] = positionval;
                
                timelinehandletext.text = Mathf.Round(timeval[dataindex][o]).ToString();
            }
        }
        else
        {
            timeval[dataindex][o] = exportTime;
        }
        


        #region Visibility
        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < holderstime.Length; j++)
            {
                if (numbers[i].x > float.Parse(holderstime[j].text) || numbers[i].y > float.Parse(holders[j].text))
                {

                    if (numbers[i].x > float.Parse(holderstime[holderstime.Length - 1].text) || numbers[i].y > float.Parse(holders[holders.Length - 1].text))
                    {

                        prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;

                    }
                    else if (numbers[i].x < float.Parse(holderstime[0].text) || numbers[i].y < float.Parse(holders[0].text))
                    {

                        prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;

                    }
                    else if (numbers[i].x < float.Parse(holderstime[0].text) && numbers[i].y <= float.Parse(holders[0].text))
                    {
                        prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;
                    }
                    else
                    {
                        prefabpos[dataindex][o][i].GetComponent<Image>().enabled = true;
                    }

                    continue;
                }
            }
        }


        int checker = 0;
        for (int i = 0; i < lines[dataindex][o].Count; i++)
        {

            for (int j = 0; j < linesall[dataindex][o].Count; j++)
            {
                checker += 1;
                if (checker > ((i + 1) * 20))
                {
                    break;
                }
                //Debug.Log(i + "/" + Mathf.Clamp(j + (i * 20), 0, linesall.Count - 1));
                if (lines[dataindex][o][i].GetComponent<Image>().enabled == false || Timeline)
                {
                    linesall[dataindex][o][Mathf.Clamp(j + (i * 20), 0, linesall[dataindex].Count - 1)].GetComponent<Image>().enabled = false;
                }
                else
                {
                    linesall[dataindex][o][Mathf.Clamp(j + (i * 20), 0, linesall[dataindex].Count - 1)].GetComponent<Image>().enabled = true;
                }
            }
        }

        for (int i = 0; i < anchors[dataindex][o].Count; i++)
        {

            if (anchors[dataindex][o][i].GetComponent<RectTransform>().position.y >= holderstime[0].GetComponent<RectTransform>().position.y - 10 || Timeline)
            {
                anchors[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            else
            {
                anchors[dataindex][o][i].GetComponent<Image>().enabled = true;

            }


        }
        for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
        {

            if (prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.y >= holderstime[0].GetComponent<RectTransform>().position.y - 10)
            {
                prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            else
            {
                prefabpos[dataindex][o][i].GetComponent<Image>().enabled = true;

            }


        }
        for (int i = 0; i < linesall[dataindex][o].Count; i++)
        {

            if (linesall[dataindex][o][i].GetComponent<RectTransform>().position.y >= holderstime[0].GetComponent<RectTransform>().position.y - 10 || Timeline)
            {
                linesall[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            else
            {
                linesall[dataindex][o][i].GetComponent<Image>().enabled = true;

            }


        }


        #endregion


        #region Float Value V3
        List<Vector2> fixedarray = new List<Vector2>();
        List<Vector2> fixedarrayfix = new List<Vector2>();
        int beyond2 = 0;
        if (numberscurve.Count > 0)
        {
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                fixedarray.Clear();
                if (i == 0)
                {
                    fixedarray.Add(numbers[i]);
                    fixedarray.Add(numberscurve[i]);
                    fixedarray.Add(numberscurve[i + 1]);
                    fixedarray.Add(numbers[i + 1]);

                    Vector2[] smooth = MakeSmoothCurveVector2(fixedarray.ToArray(), 5);
                    for (int j = 0; j < smooth.Length; j++)
                    {
                        fixedarrayfix.Add(smooth[j]);
                    }
                }

                else
                {
                    //Debug.Log(i + beyond2);
                    fixedarray.Add(numbers[i]);
                    fixedarray.Add(numberscurve[i + beyond2 + 1]);
                    fixedarray.Add(numberscurve[i + beyond2 + 2]);
                    fixedarray.Add(numbers[i + 1]);
                    Vector2[] smooth = MakeSmoothCurveVector2(fixedarray.ToArray(), 5);
                    for (int j = 0; j < smooth.Length; j++)
                    {
                        fixedarrayfix.Add(smooth[j]);
                    }

                    beyond2 += 1;
                }

            }
        }

        if (fixedarrayfix.Count > 0 && !(timeval[dataindex][o] < fixedarrayfix[0].x))
        {
            int currentvalreal = 0;
            for (int j = 0; j < fixedarrayfix.Count; j++)
            {
                if (timeval[dataindex][o] >= fixedarrayfix[j].x)
                {



                    currentvalreal += 1;
                    continue;
                }
                float val = Mathf.InverseLerp(fixedarrayfix[currentvalreal - 1].x, fixedarrayfix[currentvalreal].x, timeval[dataindex][o]);
                float positionval = Mathf.Lerp(fixedarrayfix[currentvalreal - 1].y, fixedarrayfix[currentvalreal].y, val);



                //Gizmos.color = Color.green;
                floatval[dataindex][o] = positionval;
                // Gizmos.DrawCube(new Vector3(timeval, positionval, 0), Vector3.one * 5); ;



            }
        }

        #endregion
        if (dragV2.dragging)
        {
            if (!exporting)
            {
                int currentvalreal26 = 1;
                for (int j = 1; j < fixedarrayfix.Count; j++)
                {
                    if (timeval[dataindex][o] >= fixedarrayfix[j].x)
                    {



                        currentvalreal26 += 1;
                        continue;
                    }
                    float val = Mathf.InverseLerp(fixedarrayfix[currentvalreal26 - 1].x, fixedarrayfix[currentvalreal26].x, timeval[dataindex][o]);
                    float positionval = Mathf.Lerp(fixedarrayfix[currentvalreal26 - 1].y, fixedarrayfix[currentvalreal26].y, val);

                    GameObject go = GameObject.Find(lerpdataimport[dataindex].InstanceID);
                    if (!isList)
                    {
                        switch (o)
                        {
                            case 0:
                                {
                                    go.transform.localPosition = new Vector3(positionval, go.transform.position.y, go.transform.position.z);

                                    break;

                                }
                            case 1:
                                {
                                    go.transform.localPosition = new Vector3(go.transform.position.x, positionval, go.transform.position.z);

                                    break;

                                }
                            case 2:
                                {
                                    go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, positionval);

                                    break;

                                }

                        }
                    }


                }
            }
        }


        #endregion

        #region Anchor Clamp
        if (anchors[dataindex][o].Count > 0)
        {
            int beyond3 = 0;
            for (int i = 0; i < prefabpos[dataindex][o].Count; i++)
            {
                if (i == 0)
                {
                    anchors[dataindex][o][i].GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(anchors[dataindex][o][i].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][0].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][1].GetComponent<RectTransform>().position.x), anchors[dataindex][o][i].GetComponent<RectTransform>().position.y, anchors[dataindex][o][i].GetComponent<RectTransform>().position.z);
                }
                else if (i == prefabpos[dataindex][o].Count - 1)
                {
                    anchors[dataindex][o][anchors[dataindex][o].Count - 1].GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(anchors[dataindex][o][anchors[dataindex][o].Count - 1].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][prefabpos[dataindex][o].Count - 2].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][prefabpos[dataindex][o].Count - 1].GetComponent<RectTransform>().position.x), anchors[dataindex][o][anchors[dataindex][o].Count - 1].GetComponent<RectTransform>().position.y, anchors[dataindex][o][anchors[dataindex][o].Count - 1].GetComponent<RectTransform>().position.z);
                    break;
                }
                else
                {
                    anchors[dataindex][o][i + beyond3].GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(anchors[dataindex][o][i + beyond3].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][i - 1].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.x), anchors[dataindex][o][i + beyond3].GetComponent<RectTransform>().position.y, anchors[dataindex][o][i + beyond3].GetComponent<RectTransform>().position.z);
                    anchors[dataindex][o][i + beyond3 + 1].GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(anchors[dataindex][o][i + beyond3 + 1].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][i].GetComponent<RectTransform>().position.x, prefabpos[dataindex][o][i + 1].GetComponent<RectTransform>().position.x), anchors[dataindex][o][i + beyond3 + 1].GetComponent<RectTransform>().position.y, anchors[dataindex][o][i + beyond3 + 1].GetComponent<RectTransform>().position.z);
                    //anchors[i+1+beyond3].GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(anchors[i].GetComponent<RectTransform>().position.x, prefabpos[i].GetComponent<RectTransform>().position.x, prefabpos[i+1].GetComponent<RectTransform>().position.x), anchors[i+beyond3].GetComponent<RectTransform>().position.y, anchors[i+beyond3].GetComponent<RectTransform>().position.z);
                    beyond3 += 1;
                }

            }
        }
        #endregion

        #region Visibility Position
        Vector3[] horizontalrect = new Vector3[4];
        horizontal.GetWorldCorners(horizontalrect);
        Vector3[] verticalrect = new Vector3[4];
        vertical.GetWorldCorners(verticalrect);

        Vector3 maxposx = new Vector3(0, 0, 0);
        Vector3 minposx = new Vector3(0, 0, 0);
        Vector3 maxposy = new Vector3(0, 0, 0);
        Vector3 minposy = new Vector3(0, 0, 0);

        int minx = 0;
        int maxx = 0;
        int miny = 0;
        int maxy = 0;
        for (int i = holders.Length - 1; i >= 0; i--)
        {
            if (holders[i].GetComponent<RectTransform>().anchoredPosition.y < horizontalrect[0].y)
            {


                minposy = holders[i].GetComponent<RectTransform>().position;
                miny = i;
            }


        }
        for (int i = 0; i < holders.Length; i++)
        {

            if (holders[i].GetComponent<RectTransform>().anchoredPosition.y < verticalrect[2].y)
            {


                maxposy = holders[i].GetComponent<RectTransform>().position;
                maxy = i;
            }

        }

        for (int i = 0; i < holderstime.Length; i++)
        {
            if (holderstime[i].GetComponent<RectTransform>().anchoredPosition.y < verticalrect[2].y)
            {


                maxposx = holderstime[i].GetComponent<RectTransform>().position;
                maxx = i;
            }


        }
        for (int i = holderstime.Length - 1; i >= 0; i--)
        {

            if (holderstime[i].GetComponent<RectTransform>().anchoredPosition.y < horizontalrect[0].y)
            {


                minposx = holderstime[i].GetComponent<RectTransform>().position;
                minx = i;
            }

        }
        #endregion



        #region Main Visibility
        if (Timeline)
        {
            vertical.gameObject.SetActive(false);
        }
        else
        {
            vertical.gameObject.SetActive(true);

        }
        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i].x > float.Parse(holderstime[maxx].text) || numbers[i].x < float.Parse(holderstime[minx].text) || numbers[i].y > float.Parse(holders[maxy].text) || numbers[i].y < float.Parse(holders[miny].text))
            {
                prefabpos[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            else
            {
                prefabpos[dataindex][o][i].GetComponent<Image>().enabled = true;

            }
        }
        for (int i = 0; i < numberscurve.Count; i++)
        {
            if ((numberscurve[i].x > float.Parse(holderstime[maxx].text) || numberscurve[i].x < float.Parse(holderstime[minx].text) || numberscurve[i].y > float.Parse(holders[maxy].text) || numberscurve[i].y < float.Parse(holders[miny].text)) || Timeline)
            {
                anchors[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            else
            {
                anchors[dataindex][o][i].GetComponent<Image>().enabled = true;

            }
        }
        for (int i = 0; i < linesall[dataindex][o].Count; i++)
        {

            if ((linesall[dataindex][o][i].GetComponent<RectTransform>().position.x < holderstime[minx].GetComponent<RectTransform>().position.x + 1 || linesall[dataindex][o][i].GetComponent<RectTransform>().position.x > holderstime[maxx].GetComponent<RectTransform>().position.x - 1 || linesall[dataindex][o][i].GetComponent<RectTransform>().position.y < holders[miny].GetComponent<RectTransform>().position.y + 1 || linesall[dataindex][o][i].GetComponent<RectTransform>().position.y > holders[maxy].GetComponent<RectTransform>().position.y - 1) || Timeline)
            {
                linesall[dataindex][o][i].GetComponent<Image>().enabled = false;
            }
            else
            {
                linesall[dataindex][o][i].GetComponent<Image>().enabled = true;

            }


        }
        #endregion

        if (export[dataindex][o])
        {
            
            output = new AnimationCurve();
            
            for (float i = 0; i < numbers[numbers.Count - 1].x; i += 0.1f)
            {
                int currentvalreal2 = 0;
                for (int j = 0; j < fixedarrayfix.Count; j++)
                {
                    if (i >= fixedarrayfix[j].x)
                    {



                        currentvalreal2 += 1;
                        continue;
                    }
                    float val = Mathf.InverseLerp(fixedarrayfix[currentvalreal2 - 1].x, fixedarrayfix[currentvalreal2].x, i);
                    float positionval = Mathf.Lerp(fixedarrayfix[currentvalreal2 - 1].y, fixedarrayfix[currentvalreal2].y, val);
                    int current = 0;
                    if (isList)
                    {
                        current = o + 7;
                    }

                    //Gizmos.color = Color.green;
                    output.AddKey(i, positionval);
                    // Gizmos.DrawCube(new Vector3(timeval, positionval, 0), Vector3.one * 5); ;
                   


                }
            }
            
            if (isList == false)
            {

                clip.legacy = true;
                switch (o)
                {
                    case 0:
                        {
                            clip.SetCurve(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)), typeof(Transform), "localPosition.x", output);
                            Debug.Log(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)));
                            break;
                        }
                    case 1:
                        {
                            clip.SetCurve(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)), typeof(Transform), "localPosition.y", output);
                            Debug.Log(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)));
                            break;
                        }


                    case 2:
                        {
                            clip.SetCurve(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)), typeof(Transform), "localPosition.z", output);
                            Debug.Log(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)));
                            break;
                        }


                }
                clip.legacy = false;
                Debug.Log("success");
            }
            else
            {
                Type textType = null;
                string typeName = lerpdataimport[dataindex].Type[o];
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    textType = assembly.GetType(typeName);
                    if (textType != null)
                        break;
                }
                clip.SetCurve(GetPath(GameObject.Find(lerpdataimport[dataindex].InstanceID)), textType, (lerpdataimport[dataindex].property[o]), output);

            }


            export[dataindex][o] = false;
            lerpdata.Customdata[o] = new CurveData();
            CurveData data = new CurveData(property[dataindex][o], numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);
            if (isList)
            {
                lerpdata.Customdata[o] = data;
            }
            else
            {
                switch (o)
                {
                    case 0:
                        {
                            lerpdataimport[dataindex].PositionX = data;

                            break;
                        }
                    case 1:
                        {
                            lerpdataimport[dataindex].PositionY = data;

                            break;
                        }
                    case 2:
                        {
                            lerpdataimport[dataindex].PositionZ = data;

                            break;
                        }
                    case 3:
                        {
                            lerpdataimport[dataindex].RotationX = data;

                            break;
                        }
                    case 4:
                        {
                            lerpdataimport[dataindex].RotationY = data;

                            break;
                        }
                    case 5:
                        {
                            lerpdataimport[dataindex].RotationZ = data;

                            break;
                        }

                }
            }
            lerpdataimport[dataindex] = lerpdata;
        }
        /*
        if (once[dataindex][o] == false)
        {
            //Invoke("Renew", 0.05f);
            StartCoroutine(Renew(o, 0.05f,dataindex));
        }
        */
        if (isList)
            lerpdataimport[dataindex].Customdata[o] = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);
        else
        {
            switch (o)
            {
                case 0:
                    {
                        lerpdataimport[dataindex].PositionX = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);

                        break;
                    }
                case 1:
                    {
                        lerpdataimport[dataindex].PositionY = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);

                        break;
                    }
                case 2:
                    {
                        lerpdataimport[dataindex].PositionZ = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);

                        break;
                    }
                case 3:
                    {
                        lerpdataimport[dataindex].RotationX = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);

                        break;
                    }
                case 4:
                    {
                        lerpdataimport[dataindex].RotationY = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);

                        break;
                    }
                case 5:
                    {
                        lerpdataimport[dataindex].RotationZ = new CurveData("", numbers, numberscurve, curvingpos, curvingposanchor, SmoothArray);

                        break;
                    }

            }
        }
        for (int k = 0; k < lerpdataimport.Count; k++)
        {


            for (int i = 0; i < prefabpos[k].Count; i++)
            {
                for (int j = 0; j < prefabpos[k][i].Count; j++)
                {

                    if (prefabpos[k][i][j].GetComponent<anchordrag>().selected == true)
                    {

                        selected[k][i] = false;
                        break;
                    }
                    else
                    {
                        selected[k][i] = true;
                    }
                      
                     
                }


            }
            for (int i = 0; i < anchors[k].Count; i++)
            {
                for (int j = 0; j < anchors[k][i].Count; j++)
                {
                    if(selected[k][i]==false)
                    {
                        break;
                    }
                    if (anchors[k][i][j].GetComponent<anchordrag>().selected == true)
                    {

                        selected[k][i] = false;
                        break;
                    }
                    else
                    {
                        selected[k][i] = true;
                    }
                      
                     
                }


            }

        }
    }
    
    public void Exporting()
    {
        lowesttime = 99999;
        highesttime = -99999;
        for (int dataindex = 0; dataindex < lerpdataimport.Count; dataindex++)
        {

            for (int o = 0; o < 7+lerpdataimport[dataindex].Customdata.Count; o++)
            {
                if(o<7)
                {
                    switch (o)
                    {
                        case 0:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].PositionX.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].PositionX.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].PositionX.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].PositionX.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].PositionX.numbers[i].x;
                                    }
                                    
                                }
                                break;

                            }

                        case 1:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].PositionY.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].PositionY.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].PositionY.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].PositionY.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].PositionY.numbers[i].x;
                                    }
                                }
                                break;

                            }

                        case 2:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].PositionZ.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].PositionZ.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].PositionZ.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].PositionZ.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].PositionZ.numbers[i].x;
                                    }
                                }
                                break;

                            }
                            
                        case 3:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].RotationX.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].RotationX.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].RotationX.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].RotationX.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].RotationX.numbers[i].x;
                                    }
                                }
                                break;

                            }
                                 
                        case 4:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].RotationY.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].RotationY.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].RotationY.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].RotationY.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].RotationY.numbers[i].x;
                                    }
                                }
                                break;

                            }

                            case 5:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].RotationZ.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].RotationZ.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].RotationZ.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].RotationZ.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].RotationZ.numbers[i].x;
                                    }
                                }
                                break;

                            }
                            case 6:
                            {
                                for (int i = 0; i < lerpdataimport[dataindex].IKVal.numbers.Count; i++)
                                {
                                    if( lerpdataimport[dataindex].IKVal.numbers[i].x<lowesttime)
                                    {
                                        lowesttime = lerpdataimport[dataindex].IKVal.numbers[i].x;
                                    }
                                    else if(lerpdataimport[dataindex].IKVal.numbers[i].x > highesttime)
                                    {
                                        highesttime = lerpdataimport[dataindex].IKVal.numbers[i].x;
                                    }
                                }
                                break;

                            }

                    }
                }
                else
                {
                    for (int i = 0; i < lerpdataimport[dataindex].Customdata.Count; i++)
                    {
                        for (int j = 0; j < lerpdataimport[dataindex].Customdata[i].numbers.Count; j++)
                        {
                            if (lerpdataimport[dataindex].Customdata[i].numbers[i].x < lowesttime)
                            {
                                lowesttime = lerpdataimport[dataindex].Customdata[i].numbers[i].x;
                            }
                            else if (lerpdataimport[dataindex].Customdata[i].numbers[i].x > highesttime)
                            {
                                highesttime = lerpdataimport[dataindex].Customdata[i].numbers[i].x;
                            }
                        }
                    }
                }
                /*
                int currentvalreal26 = 1;
                for (int j = 1; j < fixedarrayfix.Count; j++)
                {
                    if (timeval[dataindex][o] >= fixedarrayfix[j].x)
                    {



                        currentvalreal26 += 1;
                        continue;
                    }
                    float val = Mathf.InverseLerp(fixedarrayfix[currentvalreal26 - 1].x, fixedarrayfix[currentvalreal26].x, timeval[dataindex][o]);
                    float positionval = Mathf.Lerp(fixedarrayfix[currentvalreal26 - 1].y, fixedarrayfix[currentvalreal26].y, val);

                    GameObject go = GameObject.Find(lerpdataimport[dataindex].InstanceID);
                    if (!isList)
                    {
                        switch (o)
                        {
                            case 0:
                                {
                                    go.transform.localPosition = new Vector3(positionval, go.transform.position.y, go.transform.position.z);

                                    break;

                                }
                            case 1:
                                {
                                    go.transform.localPosition = new Vector3(go.transform.position.x, positionval, go.transform.position.z);

                                    break;

                                }
                            case 2:
                                {
                                    go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, positionval);

                                    break;

                                }

                        }
                    }


                }
                */
            }
        }
        exporttimeset = true;

    }

    public IEnumerator Renew(int o,float delay,int dataindex)
    {
        yield return new WaitForSeconds(delay);
        needrenewal[dataindex][o] = true;
        once[dataindex][o] = true;
    }
    public void Reset()
    {
        bar.value = 0.5f;
        bartime.value = 0.5f;
    }

    public void Initial()
    {
        for (int j = 0; j < lerpdataimport.Count; j++)
        {
            /*
            if (CustomButtons.Count != lerpdataimport[j].Customdata.Count)
            {
                foreach (Button item in CustomButtons)
                {
                    Destroy(item.gameObject);
                }
                for (int i = 0; i < lerpdataimport[j].Customdata.Count; i++)
                {
                    Button btn = Instantiate(timelinebutton, buttonparent).GetComponent<Button>();
                    CustomButtons.Add(btn);
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = ("Custom Data " + i);
                    btn.GetComponentInChildren<TimelineButton>().dataindex = i;
                }

            }
            if (DisplayCustom.Count != lerpdataimport[j].Customdata.Count)
            {
                DisplayCustom = new List<bool>();
                for (int i = 0; i < lerpdataimport[j].Customdata.Count; i++)
                {
                    DisplayCustom.Add(false);
                }
            }
            */
            
            foreach (List<List<GameObject>> obj in prefabpos)
            {
                foreach ( List< GameObject > list in obj)
                {
                    foreach (GameObject mainitem in list)
                    {
                        Destroy(mainitem);
                    }
                }
            }
            foreach (List<List<GameObject>> obj in anchors)
            {
                foreach ( List< GameObject > list in obj)
                {
                    foreach (GameObject mainitem in list)
                    {
                        Destroy(mainitem);
                    }
                }
            }
            foreach (List<List<RectTransform>> obj in linesall)
            {
                foreach ( List<RectTransform> list in obj)
                {
                    foreach (RectTransform mainitem in list)
                    {
                        Destroy(mainitem.gameObject);
                    }
                }
            }
            foreach (List<List<RectTransform>> obj in lines)
            {
                foreach ( List<RectTransform> list in obj)
                {
                    foreach (RectTransform mainitem in list)
                    {
                        Destroy(mainitem.gameObject);
                    }
                }
            }
            foreach (List<List<RectTransform>> obj in anchoredlines)
            {
                foreach ( List<RectTransform> list in obj)
                {
                    foreach (RectTransform mainitem in list)
                    {
                        Destroy(mainitem.gameObject);
                    }
                }
            }

            /*
            foreach (GameObject item in anchors)
            {
                Destroy(item);
            }

            foreach (RectTransform item in lines)
            {
                Destroy(item.gameObject);
            }
            foreach (RectTransform item in linesall)
            {
                Destroy(item.gameObject);
            }
            foreach (RectTransform item in anchoredlines)
            {
                Destroy(item.gameObject);
            }
            */
            prefabpos.Clear();
            selected.Clear();
            anchors.Clear();
            lines.Clear();
            linesall.Clear();
            anchoredlines.Clear();
            needrenewal.Clear();
            movedobject.Clear();
            update_anchorline.Clear();
            smoothen.Clear();
            floatval.Clear();
            once.Clear();
            timeval.Clear();
            property.Clear();
            export.Clear();
            import.Clear();
            Add.Clear();
            Delete.Clear();
            deleteindexes.Clear();
            if (prefabpos.Count < lerpdataimport.Count)
            {
                prefabpos.Add(new List<List<GameObject>>());
            }
            if (selected.Count < lerpdataimport.Count)
            {
                selected.Add(new List<bool>());
            }

            if (anchors.Count < lerpdataimport.Count)
            {
                anchors.Add(new List<List<GameObject>>());
            }

            if (lines.Count < lerpdataimport.Count)
            {
                lines.Add(new List<List<RectTransform>>());
            }
            if (linesall.Count < lerpdataimport.Count)
            {
                linesall.Add(new List<List<RectTransform>>());
            }
            if (anchoredlines.Count < lerpdataimport.Count)
            {
                anchoredlines.Add(new List<List<RectTransform>>());
            }

            if (needrenewal.Count < lerpdataimport.Count)
            {
                needrenewal.Add(new List<bool>());
            }
            if (movedobject.Count < lerpdataimport.Count)
            {
                movedobject.Add(new List<bool>());
            }
            if (update_anchorline.Count < lerpdataimport.Count)
            {
                update_anchorline.Add(new List<bool>());
            }
            if (smoothen.Count < lerpdataimport.Count)
            {
                smoothen.Add(new List<bool>());
            }
            if (once.Count < lerpdataimport.Count)
            {
                once.Add(new List<bool>());
            }

            if (floatval.Count < lerpdataimport.Count)
            {
                floatval.Add(new List<float>());
            }
            if (timeval.Count < lerpdataimport.Count)
            {
                timeval.Add(new List<float>());
            }
            if (property.Count < lerpdataimport.Count)
            {
                property.Add(new List<string>());
            }

            if (export.Count < lerpdataimport.Count)
            {
                export.Add(new List<bool>());
            }
            if (import.Count < lerpdataimport.Count)
            {
                import.Add(new List<bool>());
            }
            if (Add.Count < lerpdataimport.Count)
            {
                Add.Add(new List<bool>());
            }
            if (Delete.Count < lerpdataimport.Count)
            {
                Delete.Add(new List<bool>());
            }
            if (deleteindexes.Count < lerpdataimport.Count)
            {
                deleteindexes.Add(new List<List<int>>());
            }
            if (deleteindexes[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    deleteindexes[j].Add(new List<int>());
                }
            }
            if (prefabpos[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    prefabpos[j].Add(new List<GameObject>());
                }
            }
            if (selected[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    selected[j].Add(new bool());
                }
            }

            if (lines[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    lines[j].Add(new List<RectTransform>());
                }
            }
            if (linesall[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    linesall[j].Add(new List<RectTransform>());
                }
            }
            if (anchors[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    anchors[j].Add(new List<GameObject>());
                }
            }
            if (anchoredlines[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    anchoredlines[j].Add(new List<RectTransform>());
                }
            }

            if (needrenewal[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    needrenewal[j].Add(new bool());
                }
            }
            if (movedobject[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    movedobject[j].Add(new bool());
                }
            }
            if (update_anchorline[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    update_anchorline[j].Add(new bool());
                }
            }
            if (smoothen[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    smoothen[j].Add(new bool());
                }
            }
            if (once[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    once[j].Add(false);
                }
            }

            if (floatval[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    floatval[j].Add(new float());
                }
            }
            if (timeval[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    timeval[j].Add(new float());
                }
            }
            if (property[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    property[j].Add("");
                }
            }

            if (export[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    export[j].Add(new bool());
                }
            }
            if (import[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    import[j].Add(true);
                }
            }
            if (Add[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    Add[j].Add(new bool());
                }
            }
            if (Delete[j].Count < 7 + lerpdataimport[j].Customdata.Count)
            {
                for (int i = 0; i < 7 + lerpdataimport[j].Customdata.Count; i++)
                {
                    Delete[j].Add(new bool());
                }
            }

        }

        }
        public static Vector3[] MakeSmoothCurveVector3(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }

    public static Vector2[] MakeSmoothCurveVector2(Vector2[] arrayToCurve, float smoothness)
    {
        List<Vector2> points;
        List<Vector2> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector2>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector2>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }

    public void ChangeDisplay(int Displaymode)
    {
        
        for (int dataindex = 0; dataindex < lerpdataimport.Count; dataindex++)
        {


            switch (Displaymode)
            {
                case 0:
                    {
                        for (int i = 0; i < import[dataindex].Count; i++)
                        {
                            import[dataindex][i] = true;
                        }
                        DisplayPos = true;
                        DisplayRot = true;
                        DisplayX = true;
                        DisplayY = true;
                        DisplayZ = true;
                        DisplayIK = true;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = true;
                        }
                        break;
                    }
                case 1:
                    {
                        import[dataindex][0] = true;
                        import[dataindex][3] = true;
                        DisplayPos = true;
                        DisplayRot = true;
                        DisplayX = true;
                        DisplayY = false;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 2:
                    {
                        import[dataindex][1] = true;
                        import[dataindex][4] = true;
                        DisplayPos = true;
                        DisplayRot = true;
                        DisplayX = false;
                        DisplayY = true;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 3:
                    {
                        import[dataindex][2] = true;
                        import[dataindex][5] = true;
                        DisplayPos = true;
                        DisplayRot = true;
                        DisplayX = false;
                        DisplayY = false;
                        DisplayZ = true;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 4:
                    {
                        import[dataindex][6] = true;
                        DisplayPos = false;
                        DisplayRot = false;
                        DisplayX = false;
                        DisplayY = false;
                        DisplayZ = false;
                        DisplayIK = true;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 5:
                    {
                        DisplayPos = false;
                        DisplayRot = false;
                        DisplayX = false;
                        DisplayY = false;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = true;
                        }
                        break;
                    }
                case 6:
                    {
                        import[dataindex][0] = true;
                        DisplayPos = true;
                        DisplayRot = false;
                        DisplayX = true;
                        DisplayY = false;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 7:
                    {
                        import[dataindex][1] = true;
                        DisplayPos = true;
                        DisplayRot = false;
                        DisplayX = false;
                        DisplayY = true;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 8:
                    {
                        import[dataindex][2] = true;
                        DisplayPos = true;
                        DisplayRot = false;
                        DisplayX = false;
                        DisplayY = false;
                        DisplayZ = true;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 9:
                    {
                        import[dataindex][3] = true;
                        DisplayPos = false;
                        DisplayRot = true;
                        DisplayX = true;
                        DisplayY = false;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 10:
                    {
                        import[dataindex][4] = true;
                        DisplayPos = false;
                        DisplayRot = true;
                        DisplayX = false;
                        DisplayY = true;
                        DisplayZ = false;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }
                case 11:
                    {
                        import[dataindex][5] = true;
                        DisplayPos = false;
                        DisplayRot = true;
                        DisplayX = false;
                        DisplayY = false;
                        DisplayZ = true;
                        DisplayIK = false;
                        for (int i = 0; i < DisplayCustom.Count; i++)
                        {
                            DisplayCustom[i] = false;
                        }
                        break;
                    }

            }
        }
        
    }

    public void CreateConnection(Vector2 A, Vector2 B, Color color, int o,int dataindex)
    {

        GameObject gameObject = new GameObject("connection", typeof(Image));
        gameObject.transform.SetParent(canvas.transform, false);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (B - A).normalized;
        float distance = Vector2.Distance(A, B);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 2);
        //rectTransform.anchoredPosition = A+dir*distance*.5f;
        rectTransform.anchoredPosition = (A + B) / 2;
        //Debug.Log((A + B) / 2);
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        lines[dataindex][o].Add(rectTransform);
    }

    public void CreateConnectionall(Vector2 A, Vector2 B, Color color, int o,int dataindex)
    {

        GameObject gameObject = new GameObject("connection", typeof(Image));
        gameObject.transform.SetParent(canvas.transform, false);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (B - A).normalized;
        float distance = Vector2.Distance(A, B);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 2);
        //rectTransform.anchoredPosition = A+dir*distance*.5f;
        rectTransform.anchoredPosition = (A + B) / 2;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        linesall[dataindex][o].Add(rectTransform);
    }
    public void CreateConnectionanchor(Vector2 A, Vector2 B, int o,int dataindex)
    {

        GameObject gameObject = new GameObject("connection", typeof(Image));
        gameObject.transform.SetParent(canvas.transform, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (B - A).normalized;
        float distance = Vector2.Distance(A, B) / 2.4f;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1);
        //rectTransform.anchoredPosition = A+dir*distance*.5f;
        rectTransform.position = (A + B) / 2;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        anchoredlines[dataindex][o].Add(rectTransform);
    }

    public void KeepConnection(Vector2 A, Vector2 B, RectTransform line,Color col,bool active)
    {

        RectTransform rectTransform = line;
        Color newcol=col;
        if(active)
        {
            newcol.a = 0.25f;
        }
        else
        {
            newcol.a = 1f;
        }
        rectTransform.GetComponent<Image>().color = newcol;
        Vector2 dir = (B - A).normalized;
        float distance = Vector2.Distance(A, B) / 2.4f;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1);
        //rectTransform.anchoredPosition = A+dir*distance*.5f;
        rectTransform.position = (A + B) / 2;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        //lines.Add(rectTransform);
    }

    public void DisableAnimator()
    {
        animator.Update(0);
        
    }

    public string GetPath(GameObject obj)
    {
        string path = obj.name;

        if (obj.transform.parent != null)
        {
            while (obj.transform.parent != null && !mainanimator.Contains(obj.transform.parent.gameObject))
            {
                obj = obj.transform.parent.gameObject;
                path = obj.name + "/" + path;
            }
        }
        else
        {
            path =  obj.name;
        }
        return path;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SelectObjectData(LerpData data)
    {
       // Debug.Log("test");
        if (usingAllAnims.isOn == false)
        { 
           // Debug.Log("test success");

        lerpdataimport.Clear();
            lerpdataimport.Add(data);
            Initial();
        }
        //make all object glow
    }

    public void ExportAll()
    {
        Exportall = true;
    }
}
[System.Serializable]
public struct SmoothLine
{
    public Vector2[] points;
    public Vector2[] smooths;
    public SmoothLine(Vector2[] point)
    {
        points = point;

        smooths = new Vector2[0];
    }
    public SmoothLine(Vector2[] point, Vector2[] smooth)
    {
        points = point;
        smooths = smooth;
    }

}

[System.Serializable]
public struct CurveData
{
    public string property;
    public List<Vector2> numbers;
    public List<Vector2> numberscurve;
    public Vector2[] curvingpos;
    public Vector2[] curvingposanchor;
    public SmoothLine[] SmoothArray;
    public CurveData(string property2, List<Vector2> num, List<Vector2> numcurve, Vector2[] curvepos, Vector2[] curveanchor, SmoothLine[] smooth)
    {
        property = property2;
        numbers = num;


        numberscurve = numcurve;
        curvingpos = new Vector2[curvepos.Length];
        for (int i = 0; i < curvepos.Length; i++)
        {
            curvingpos[i] = curvepos[i];
        }
        curvingposanchor = new Vector2[curveanchor.Length];
        for (int i = 0; i < curveanchor.Length; i++)
        {
            curvingposanchor[i] = curveanchor[i];
        }
        SmoothArray = new SmoothLine[smooth.Length];
        for (int i = 0; i < smooth.Length; i++)
        {
            SmoothArray[i] = smooth[i];
        }
    }
}
