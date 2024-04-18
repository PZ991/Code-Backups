using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
    using System;
public class InstanceEntity : MonoBehaviour
{
    // Start is called before the first frame update
    public float val;
    public Type type;
    public PlayerControlV3 cont;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //get method names, number of input, and input type
        var info= typeof(InstanceEntity).GetTypeInfo().DeclaredMethods;
        //var info= Type.GetType("PlayerControlV3").GetTypeInfo().DeclaredMethods;
        foreach (MethodInfo mi in info)
        {
            if (mi.Name == "test")
            {
                mi.Invoke(this, null);
                //mi.Invoke(cont, null);

                /*
                var parameterInfos = mi.GetParameters();
                
                if (parameterInfos.Length == 2)
                {
                    if (parameterInfos[0].ParameterType == typeof(GameObject) &&
                        parameterInfos[1].ParameterType == typeof(Hashtable))
                    {
                        mi.Invoke(this, parameters)
                    }
                }
                */
            }
        }
        object[] parametersArray = new object[] { "Hello" };
        MethodInfo writeLine = typeof(Debug).GetMethod("Log", new Type[] { typeof(string) });
        writeLine.Invoke(null, parametersArray);
        Outest(val,out val);
    }
    public void test()
    {
        Debug.Log("test");
    }
    public void Outest(float val2, out float val )
    {
        val2 += 1;
        val = val2;
    }
    private void OnDrawGizmos()
    {
       // Debug.Log(System.AppDomain.CurrentDomain.GetAssemblies());
        /*
        Assembly asm = Assembly.GetExecutingAssembly();
        Debug.Log("Assembly name=" + asm.FullName);
        Debug.Log("Assembly name = " + asm.FullName); foreach (System.Type type in asm.GetExportedTypes())
        {
            UnityEngine.Debug.Log(type);
        }
        */
        //Type type = typeof(Transform);
        //Debug.Log(Assembly.GetAssembly(type));
        Type type2 = typeof(PlayerControlV3);
       // Debug.Log(Assembly.GetAssembly(type2));

    }
}
