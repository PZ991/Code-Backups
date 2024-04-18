using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DisctionaryTest : MonoBehaviour
{

    public Dictionary<string, float> floatdict=new Dictionary<string, float>();
    public bool subtract_health;
    // Start is called before the first frame update
    void Start()
    {
        floatdict.TryAdd("health", 100);
        
        var myKey = floatdict.FirstOrDefault(x => x.Value == 100).Key;
        var myvalue = floatdict.FirstOrDefault(x => x.Key == "health").Value;
       // Debug.Log(myKey);
       // Debug.Log(myvalue);
    }

    // Update is called once per frame
    void Update()
    {
        if(subtract_health)
        {

            floatdict["health"] -= 10;
            subtract_health = false;
            Debug.Log(floatdict["health"]);
        }
        
    }
}
