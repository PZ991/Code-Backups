using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo_manager : MonoBehaviour
{
    [Header("Combo")]
    public Combo_scriptable previouscombo;                      //previouscombo
    public Combo_scriptable currentcombo;                       //current combo
    public List<float> combotimer;                              //List of the timing for interconnected combos
    public List<ComboInfo> interconnected_combo;         //List of all combo interconnected to current combo
    public float currentcombotimer;                             //the timer to compare to combotimer
    public AnimatorEvent animatorhandler;
    [Header("Combo Click")]
    public RectTransform start;                                 //start transform object of the UI
    public RectTransform end;                                   //end transform object of the UI
    public Slider slider;                                       //the Slider of the clicker
    public GameObject Marker;                                   //prefab of the marker to create in slider
    public List<GameObject> Mark;                               //the list of all created marks
    public Canvas canvas;                                       //Canvas of the scene
    public float combomaxtimer;                                 //The largest number value in the combotimer
    [Header("Properties")]
    public float Timer_speed;                                   //speed of the timer
    private bool comboing;                                      //if player is currently comboing
    private int checker;                                        //the time checker between currentcombotimer and combotimer
    [Header("Test")]
    public Combo_scriptable startCombo;                         //The testing start

    [Header("References")]
    public ComboType combotype;                                 //type of ComboUI
    public GameObject ClickParent;                              //Parent of ClickMenu(for disabling)
    public GameObject PiemenuParent;                            //parent of PieMenu(for disabling)
    public GameObject BarMenuParent;                            //parent of BarMenu(for disabling)
    public menuitemscript[] piemenuitem;                        //The chronological order of the pie menu item holder
    public Baritem[] barmenuitem;                               //the chronological order of the bar menu item holder
    public Animator Pieanim;
    public MenuHandler menu;
    //public List<Combo_scriptable> combolist;
    private void Start()
    {
        currentcombo = animatorhandler.previousclips[1];
        //combolist = Resources.LoadAll<Combo_scriptable>("Fighting").ToList();
    }
    private void Update()
    {
        
        if (menu.canvasanim.GetBool("openmenu") == false)
        {
            if (Input.GetMouseButton(0))
            {
                if(combotype == ComboType.Pie)
                Pieanim.SetBool("open", true);
            }

            //if CombotypeUI is to Click
            #region Click_Combo
            if (combotype == ComboType.Click)
            {
                ClickParent.SetActive(true);                //enable the clickparent
                                                            //Only happen if current combo hapens
                #region Previous and CurrentCombo                                           
                if (previouscombo != currentcombo && currentcombo != null)
                {
                    combotimer = currentcombo.combotimer;                                   //set the list of combotiming from object to local
                    combomaxtimer = Mathf.Max(combotimer.ToArray());                        //get the max value of all combotiming
                    interconnected_combo = currentcombo.interconnected_combo;               //set local inter_combo from object inter_combo
                    previouscombo = currentcombo;                                           //set previous combo into the current one

                    if (Mark != null)                                                         //if there are markers in the slider
                    {
                        foreach (GameObject item in Mark)                                   //destroy all markers
                        {
                            Destroy(item);
                        }
                        Mark.Clear();                                                       //remove everything from the tracking list
                    }

                    for (int i = 0; i < interconnected_combo.Count; i++)                                                //set all combo into slider as markers
                    {
                        GameObject mark = Instantiate(Marker, canvas.transform);                                        //instantiate marker
                        float pos2 = Mathf.InverseLerp(0, combomaxtimer, combotimer[i]);                                //get value pos between 0 and maxtimer as percentage
                        mark.GetComponent<RectTransform>().position = Vector2.Lerp(start.position, end.position, pos2); //convert percentage between start and end as position
                        Mark.Add(mark);                                                                                 //add created marker to tracking list
                                                                                                                        //Debug.Log(pos2);
                    }




                }
                else if (currentcombo == null)                                                                          //if there's no new combo or runout of timer
                {
                    previouscombo = null;                                                                               //set previous to null
                    if (Mark != null)                                                                                      //clean list and destroy all markers
                    {
                        foreach (GameObject item in Mark)
                        {
                            Destroy(item);
                        }
                        Mark.Clear();

                    }


                }
                #endregion

                //Only happen if The number of timing and interconnected combo is not the same
                #region Check for Count of List
                if (combotimer.Count != interconnected_combo.Count)
                {
                    Debug.LogWarning("timer or combo must be equal of count");
                }
                for (int i = 0; i < combotimer.Count - 1; i++)
                {
                    if (combotimer[i] > combotimer[i + 1] || combotimer[i] == combotimer[i + 1])
                    {
                        Debug.LogWarning("timer must be increasing ");
                    }

                }
                #endregion

                //Check the timing of each interconnection
                #region Check Timing
                if (currentcombo != null)
                {
                    comboing = true;
                }

                if (comboing == true)
                {
                    //currentcombotimer += Timer_speed;                                                   
                    if (interconnected_combo.Count > 0 && combotimer.Count > 0)                         //if there are interconnections
                    {
                        //Debug.Log(combomaxtimer);
                        currentcombotimer += Timer_speed;                                               //current timing adds ped timer speed

                        if (currentcombotimer > combotimer[checker] && currentcombotimer < combomaxtimer && checker < interconnected_combo.Count)
                        {
                            //current combo timer must be: 
                            //greater than than the current combotimer check
                            //less than the max combotiming
                            //less than the number of interconnected combo
                            checker += 1;

                        }
                        else if (currentcombotimer >= combomaxtimer || checker >= interconnected_combo.Count)
                        {
                            //happens only if current combo is equal or greater than the max timing and the checker is greater than the number of interconnection
                            currentcombo = null;
                            comboing = false;

                        }

                    }
                    if (Input.GetMouseButtonDown(0))        //attack
                    {
                        Attack();

                    }
                }
                else
                {
                    currentcombotimer = 0;                  //reset local combotimer
                    currentcombo = null;                    //remove current combo
                }
                #endregion

                slider.maxValue = combomaxtimer;            //set maxvalue to the max combotime
                slider.value = currentcombotimer;           //set current slider value to the current combo timing


            }
            else
            {
                ClickParent.SetActive(false);                //disable the clickparent
            }
            #endregion

            //if Combotype UI is to be a Pie
            #region Pie Combo
            if (combotype == ComboType.Pie)
            {
                PiemenuParent.SetActive(true);                                              //enable the pie menu parent

                //only happens if previous combo is not the same as the current one
                #region Previous and CurrentCombo
                if (previouscombo != currentcombo && currentcombo != null)
                {
                    combotimer = currentcombo.combotimer;                                   //set local timer list from combo list
                    interconnected_combo = currentcombo.interconnected_combo;               //set local inter_combo from combo list
                    previouscombo = currentcombo;                                           //set previous combo into current one
                    //animatorhandler.clip.Add(currentcombo.animation);                       //add current combo animation to animator queue
                    for (int i = 0; i < piemenuitem.Length; i++)                            //the loop through all the items in the pie
                    {
                        if (i < interconnected_combo.Count)                                  //if there is an interconnected combo in the current one

                        {
                            piemenuitem[i].combo = interconnected_combo[i].combo;                 //set pie menu item into the inter_combo
                        }
                        else
                        {
                            piemenuitem[i].combo = null;                                    //else remove all

                        }
                    }
                    Pieanim.SetBool("open", false);
                }
                else if (currentcombo == null)                                              //if theres no current combo
                {
                    previouscombo = null;                                                   //set previous and current as null
                }
                #endregion

            }
            else
            {
                PiemenuParent.SetActive(false);                                             //disable the pie menu
            }
            #endregion

            //if Combotype UI is to be a Bar Combo
            #region Bar Combo
            if (combotype == ComboType.Bar)
            {
                BarMenuParent.SetActive(true);                                          //enable the Barmenu
                if (previouscombo != currentcombo && currentcombo != null)              //only happens if the previous combo is not the same as the current one
                {

                    interconnected_combo = currentcombo.interconnected_combo;           //set inter_combo list into the current intercombo list
                    previouscombo = currentcombo;                                       //set previous combo as the new one
                    for (int i = 0; i < barmenuitem.Length; i++)                        //number of barmenu length
                    {
                        if (i < interconnected_combo.Count)

                        {
                            barmenuitem[i].combo = interconnected_combo[i].combo;             //set item as the interconnected combo
                        }
                        else
                        {
                            barmenuitem[i].combo = null;                                //else remove all

                        }
                    }
                }
                else if (currentcombo == null)
                {
                    previouscombo = null;
                }
            }
            else
            {
                BarMenuParent.SetActive(false);
            }
            #endregion

        }
    }
  
    public void Attack()
    {
        Debug.Log(interconnected_combo[checker]);
        currentcombotimer = 0;
        currentcombo = interconnected_combo[checker].combo;
       
            //animatorhandler.clip.Add(currentcombo.animation);
        

        
        checker = 0;

    }

    public enum ComboType { Pie,Click,Bar}
}
