using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_hit :MonoBehaviour
{
    /*
    public int combosize;
    public float[] combotimer;
    public Combo_hit[] interconnected_combo;
    */
    public List<float> combotimer;
    public List<Combo_hit> interconnected_combo;
    public float currentcombotimer;


    public float Timer_speed;
    public bool comboing;
    public int checker;
    public Combo_manager manager;


    public void Start()
    {
        
    }
    private void Update()
    {
        if(combotimer.Count!=interconnected_combo.Count)
        {
            Debug.LogWarning("timer or combo must be equal of count");
        }
        for (int i = 0; i < combotimer.Count-1; i++)
        {
            if(combotimer[i]>combotimer[i+1]|| combotimer[i] == combotimer[i + 1])
            {
                Debug.LogWarning("timer must be increasing ");
            }

        }
        if (comboing==true)
        {
            currentcombotimer += Timer_speed;
            if (interconnected_combo!=null && combotimer!=null)
        {
            float combomaxtimer = Mathf.Max(combotimer.ToArray());
            if(currentcombotimer>combotimer[checker]&& currentcombotimer<=combomaxtimer)
            {
                checker += 1;
            }
            else if(currentcombotimer>combomaxtimer)
            {
                //cancel
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
            
        }
        }
        else
        {
            currentcombotimer = 0;
        }
         
    }

    public void Attack()
    {
        Debug.Log(interconnected_combo[checker]);
        checker = 0;
    }

    
}

