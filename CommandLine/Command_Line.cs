using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using TMPro;
using UnityEngine.UI;
using System.Data;
public class Command_Line : MonoBehaviour
{
    public TMP_InputField input;
    public List<Dictionary<string, string>> CommandLogic;
    public List<List<string>> CommandWords;
    public string[] Classes ={"Debug","Command_Line" };
    public string[] arithmeticoperators = { "+", "-", "*", "/", "=", "+=", "-=", "/=", "*=", "++", "--", "%", "%=" };
    public string[] logicaloperators = { "&", "&&", "&=", "^", "^=", "||", "|=" };

    public List<Values> currentclassvar = new List<Values>();

    public List<CLassMethods> assemblynmethod= new List<CLassMethods>();
    public float testval1=20;
    public float testval2=30;
    // Start is called before the first frame update
    void Start()
    {
        List<string> list = new List<string>();
        list.Add("System");
       list.Add("System.Collections");
        //list.Add("System.Collections.Generic");
        list.Add("System.Reflection");
        list.Add("System.Data");
        list.Add("UnityEngine.CoreModule/UnityEngine.");///UnityEngine

        Command_Line foo = new Command_Line();
        var fieldNames = typeof(Command_Line).GetFields();
        object obj = new Vector2(10, 5);
        for (int i = 0; i < fieldNames.Length; i++)
        {
            // Debug.Log(fieldNames[i].FieldType);
            //Debug.Log(fieldNames[i].IsPublic);
            // Debug.Log(fieldNames[i].IsPrivate);
            //Debug.Log(fieldNames[i].GetValue(this));
            //Debug.Log(fieldNames[i].GetType());
            //Debug.Log(fieldNames[i].DeclaringType);
            // Debug.Log(fieldNames[i].SetValue(objval,new obj));

            //Debug.Log((Vector2)obj);
            //Debug.Log(Convert.ChangeType(obj,typeof(Vector2)));
            //Debug.Log(fieldNames[i].FieldType);
            //Debug.Log(fieldNames[i].FieldType);

        }
        //Type[] ty = System.Reflection.Assembly.Load("UnityEngine.CoreModule").GetTypes();
        //Type[] ty = System.Reflection.Assembly.Load("UnityEngine.CoreModule").GetTypes();
        foreach (string item in list)
        {
            string[] split = item.Split('/');
            List<string> Namespace2 = new List<string>();
            if (item.Contains('/'))
            {
                for (int i = 1; i < split.Length; i++)
                {
                    Namespace2.Add(split[i]);
                }
            }
            Type[] ty = System.Reflection.Assembly.Load(split[0]).GetTypes();
            List<string> meths = new List<string>();
            for (int i = 0; i < ty.Length; i++)
            {
                meths.Add(ty[i].Name);
            }
            assemblynmethod.Add(new CLassMethods(split[0], meths,Namespace2));
            // if(item.Name.Equals("Debug"))
            //Debug.Log(item.Name);
            

        }
        int testval3 = 0;
        //Debug.Log(AnalyzeDot("Console.Write()"));
        //Debug.Log(AnalyzeString2("(testval2(testval1))"));
        // Debug.Log(Assembly.GetAssembly(Type.GetType("Command_Line")));

    }
    
    // Update is called once per frame
    void Update()
    {
        /*
        DataTable dt = new DataTable();
        int val = 10;
        var answer = dt.Compute("10+(4*3)*4", "");
        Debug.Log(answer);
        */
    }
    private Type StringToType(string typeAsString)
    {
        Type typeAsType = Type.GetType(typeAsString);
        return typeAsType;
    }
    public void test(string msg)
    {
        Debug.Log(msg);
    }
    public void Compile()
    {
        int linecount=input.text.Split('\n').Length;
        int endcount = input.text.Split(';').Length;
        string[] line= input.text.Split('\n');
        Command_Line com = new Command_Line();
        for (int i = 0; i < linecount; i++)
        {

            string[] function = line[i].Split('.','(',')');
            //string parameter = function[1].Split('(', ')');
            //Type type=System.Reflection.Assembly.Load("UnityEngine.CoreModule").GetType("UnityEngine." + function[0]);
            //Type type = System.Reflection.Assembly.Load("Assembly-CSharp").GetType(function[0]);
            //Assembly ass = Assembly.GetAssembly(Type.GetType("System.Console"));
            Type type = Type.GetType(function[0]);
            if (type==null)
            {
                for (int j = 0; j < assemblynmethod.Count; j++)
                {
                    //assemblynmethod[assemblynmethod.Keys[]]
                    if(assemblynmethod[j].Namespace2.Count>0)
                    {
                        for (int k = 0; k < assemblynmethod[j].Namespace2.Count; k++)
                        {
                            type = System.Reflection.Assembly.Load(assemblynmethod[j].Namespace).GetType(assemblynmethod[j].Namespace2[k] + function[0]);
                            
                            if (type != null)
                                break;
                        }
                    }
                    else
                    {
                        type = System.Reflection.Assembly.Load(assemblynmethod[j].Namespace).GetType(assemblynmethod[j].Namespace+"." + function[0]);
                    }
                } 
            }
            if (type.GetField(function[1])!=null)
            {
                Debug.Log(type.GetField(function[1]).GetValue(this.GetComponent(function[0])));
            }
            else
            {
                Debug.Log(type.Name);

                object[] parametersArray = new object[] { function[2] };
                MethodInfo writeLine = type.GetMethod(function[1].Replace('"', ' '), new Type[] { typeof(string) });
                Type type1 = Type.GetType(function[0]);
                writeLine.Invoke(this.GetComponent(type1), parametersArray);

            }
        }


       

    }
    public void Compute()
    {
        DataTable dt = new DataTable();

        int linecount = input.text.Split('\n').Length;
        int endcount = input.text.Split(';').Length;
        string[] line = input.text.Split('\n');
        for (int i = 0; i < linecount; i++)
        {

            string[] function = line[i].Split('.', '(', ')');
            //string parameter = function[1].Split('(', ')');

            Type type = System.Reflection.Assembly.Load("UnityEngine.CoreModule").GetType("UnityEngine." + function[0]);

            object[] parametersArray = new object[] { dt.Compute(function[2], "") };
            MethodInfo writeLine = type.GetMethod(function[1].Replace('"', ' '), new Type[] { typeof(string) });
            writeLine.Invoke(null, parametersArray);
        }
    }

    public void OthersWays()
    {

        var info = typeof(Command_Line).GetTypeInfo().DeclaredMethods;
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
        //Type type = Type.GetType("Namespace.MyClass, MyAssembly");
        //Assembly asm = typeof(Command_Line).Assembly;
        //Type type = asm.GetType("Debug");
        //Type type = typeof("Debug");

        //Type type = Type.GetType("UnityEngine.Debug"); //target type
        // object o = Activator.CreateInstance(type); // an instance of target type
        // YourType your = (YourType)o;






        /*
        DataTable tb = new DataTable();
        List<string> currentclassvalues;
        Dictionary<string, List<string>> accessibleclasses;
        string[] arithmeticoperators = { "+", "-", "*", "/", "=", "+=", "-=", "/=", "*=", "++", "--", "%", "%=" };
        string[] logicaloperators = { "&", "&&", "&=", "^", "^=", "||", "|=" };
        string line = "Console.Write(" + '"' + "Hello World" + '"' + ");";
        


        string[] function = line.Split('.', '(', ')');
        //string parameter = function[1].Split('(', ')');

        //Type type = System.Reflection.Assembly.Load("UnityEngine.CoreModule").GetType("UnityEngine." + function[0]);

        object[] parametersArray = new object[] { function[2] };

        MethodInfo writeLine = typeof(Console).GetMethod(function[1].Replace('"', ' '), new Type[] { typeof(string) });
        writeLine.Invoke(null, parametersArray);
        */
    }

    public void MethodTest(string test)
    {
        Debug.Log(test);
    }
    
    public string AnalyzeString(string text)
    {
       // Stack<int> parenthesisval = new Stack<int>();
        string s = text;
        char[] par= { '(', ')' };

        //Dictionary<int, int> pardict = new Dictionary<int, int>();
        bool opened=false;
        int open = 0;
        int close = 0;
        for (int i =0; i <s.Length; i++)
        {
            // for loop end when i=-1 ('a' not found)
            if (s[i].Equals(')'))
            {
                close = i;
                string final = Filter(s.Substring(open, (i - open) + 1), par);
                //pardict.Add(open, i - 1);
                //Debug.Log(s.Substring(parenthesisval.Peek(), (i - parenthesisval.Peek()) + 1));
                //Debug.Log(s.Substring(parenthesisval.Peek(), (i - parenthesisval.Peek()) + 1));
                //parenthesisval.Pop();
                
               string value =Type.GetType("Command_Line").GetField(final).GetValue(this.GetComponent("Command_Line")).ToString();
                return value;
            }
            else if ( s[i].Equals('('))
            {
                if (opened ==false)
                {
                    open = i;
                    opened = true;
                }
                else
                {
                    //parenthesisval.Push(i); 
                    Debug.Log(text);
                    string newst = AnalyzeString(s.Substring(i+1,s.Length), 0);
                    text.Replace(s.Substring(i, (  i) + 1),newst);
                }
            }

        }
        return text;
        /*
for (int i = s.IndexOf('a'); i > -1; i = s.IndexOf('a', i + 1))
        {
         // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
        }
         */
        // Debug.Log(" Name: "+currentclassvar[i].name + "     || typeof: " + currentclassvar[i].type + "      || value of: " + Convert.ChangeType(currentclassvar[i].value, currentclassvar[i].type));
        // Debug.Log(" Name: " + currentclassvar[i].name + "     || typeof: " + currentclassvar[i].type + "      || value of: " + currentclassvar[i].value);
    }
    public string AnalyzeString(string text,int indexstart)
    {
       // Stack<int> parenthesisval = new Stack<int>();
        string s = text;
        char[] par= { '(', ')' };

        //Dictionary<int, int> pardict = new Dictionary<int, int>();
        bool opened=false;
        int open = 0;
        int close = 0;
        for (int i =0; i <s.Length; i++)
        {
            // for loop end when i=-1 ('a' not found)
            if (s[i].Equals(')'))
            {
                close = i;
                string final = Filter(s.Substring(open, (i - open) + 1), par);
                //pardict.Add(open, i - 1);
                //Debug.Log(s.Substring(parenthesisval.Peek(), (i - parenthesisval.Peek()) + 1));
                //Debug.Log(s.Substring(parenthesisval.Peek(), (i - parenthesisval.Peek()) + 1));
                //parenthesisval.Pop();
                
               string value =Type.GetType("Command_Line").GetField(final).GetValue(this.GetComponent("Command_Line")).ToString();
                return value;
            }
            else if ( s[i].Equals('('))
            {
                if (opened ==false)
                {
                    open = i;
                    opened = true;
                }
                else
                {
                    //parenthesisval.Push(i); 
                    Debug.Log(text);
                    string newst = AnalyzeString(s.Substring(i+1,s.Length), 0);
                    text.Replace(s.Substring(i, ( - i) + 1),newst);
                }
            }

        }
        return text;
        /*
for (int i = s.IndexOf('a'); i > -1; i = s.IndexOf('a', i + 1))
        {
         // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
        }
         */
        // Debug.Log(" Name: "+currentclassvar[i].name + "     || typeof: " + currentclassvar[i].type + "      || value of: " + Convert.ChangeType(currentclassvar[i].value, currentclassvar[i].type));
        // Debug.Log(" Name: " + currentclassvar[i].name + "     || typeof: " + currentclassvar[i].type + "      || value of: " + currentclassvar[i].value);
    }
    public string AnalyzeString2(string text)
    {
        bool opened = false;
        string textsaved = text;
        Debug.Log(text);
        for (int i = 0; i < text.Length; i++)
        {
            if(text[i].Equals('('))
            {
                if(opened)
                {
                    AnalyzeString2(text.Substring(i, text.Length - i));
                }
                else
                {
                    opened = true;
                }
            }
            else if(text[i].Equals(')'))
            {
                string final = Filter(text, new char[] { '(', ')' });
                
                string value = Type.GetType("Command_Line").GetField(final).GetValue(this.GetComponent("Command_Line")).ToString();
                return text.Replace(textsaved, value);
            }
        }
        return text;
    }

    public string AnalyzeDot(string text)
    {
        bool scanning = false;
        bool function = false;
        List<char> val = new List<char>();
        List<char> caller = new List<char>();
        for (int i = 0; i < text.Length; i++)
        
        {
            if(text[i].Equals('.'))
            {
                scanning = true;
                for (int k = i-1; k >= 0; k--)
                {
                    if (caller.Count == 0)
                        if (text[k].Equals(' '))
                            continue;
                        else
                        caller.Add(text[k]);
                    else
                        if (text[k].Equals(' '))
                            break;
                        else
                        caller.Insert(0, text[k]);
                }
            }
            if (scanning == true)
            {
                if (val.Count <= 0)
                {
                    if (text[i].Equals(' '))
                        continue;
                    else
                        val.Add(text[i]);
                }
                else
                {
                    if (text[i].Equals('('))
                    {
                        function = true;
                        break;
                    }
                    if (!text[i].Equals(' '))
                    {
                        val.Add(text[i]);
                    }
                    else
                        break;
                    
                   
                }
            }
        }
        Debug.Log(new string(caller.ToArray()));
        Debug.Log(function);
        return new string(val.ToArray());
    }
    public  string Filter( string str, char[] charsToRemove)
    {
        foreach (char c in charsToRemove) {
            str = str.Replace(c.ToString(), String.Empty);
        }
 
        return str;
    }

    public void CheckVariables(string val)
    {
        Command_Line foo = new Command_Line();
        var fieldNames = typeof(Command_Line).GetFields();
        Debug.Log("Class Name: Command_Line\nValues:");
        for (int i = 0; i < fieldNames.Length; i++)
        {
            //currentclassvar.Add(fieldNames[i].Name, fieldNames[i].GetValue(foo));
            currentclassvar.Add(new Values(fieldNames[i].Name, fieldNames[i].FieldType, fieldNames[i].GetValue(this)));
            Debug.Log(" Name: "+currentclassvar[i].name + "     || typeof: " + currentclassvar[i].type + "      || value of: " + Convert.ChangeType(currentclassvar[i].value, currentclassvar[i].type));
           // Debug.Log(" Name: "+currentclassvar[i].name + "     || typeof: " + currentclassvar[i].type + "      || value of: " + currentclassvar[i].value);
            //Console.WriteLine(fieldNames[i].GetType().GetProperty());
            //Debug.Log(fieldNames[i].FieldType);
            //Type type = System.Reflection.Assembly.GetExecutingAssembly().GetType(currentclassvar[i].type.ToString());
            //Console.WriteLine()
        }

    }

    public Boolean Formethod(object obj1, object obj2, string foroperator)
    {

        switch (foroperator)
        {
            case ">=":
                {
                    float fl1 = float.Parse(obj1.ToString());
                    float fl2 = float.Parse(obj2.ToString());
                    return fl1 >= fl2;
                    break;
                }
            case ">":
                {
                    float fl1 = float.Parse(obj1.ToString());
                    float fl2 = float.Parse(obj2.ToString());
                    return fl1 > fl2;
                    break;
                }
            case "<=":
                {
                    float fl1 = float.Parse(obj1.ToString());
                    float fl2 = float.Parse(obj2.ToString());
                    return fl1 <= fl2;
                    break;
                }
            case "<":
                {
                    float fl1 = float.Parse(obj1.ToString());
                    float fl2 = float.Parse(obj2.ToString());
                    return fl1 < fl2;
                    break;
                }
            case "==":
                {
                    return obj1.Equals(obj2);
                    break;
                }
            case "&&":
                {
                    return (bool)obj1 && (bool)obj2;
                    break;
                }
            case "||":
                {
                    return (bool)obj1 || (bool)obj2;
                    break;
                }
            default:
                {
                    return false;
                    break;
                }
        }
    }
}
public struct Values
{
    public string name;
    public Type type;
    public object value;
    public Values(string name2, Type type2, object value2)
    {
        name = name2;
        type = type2;
        value = value2;

    }
}
public struct CLassMethods
{
    public string Namespace;
    public List<string> Types;
    public List<string> Namespace2;
    public CLassMethods(string name, List<string> type2,List<string> name2 )
    {
        Namespace = name;
        Namespace2 = name2;
        Types = type2;
        
    }
}
