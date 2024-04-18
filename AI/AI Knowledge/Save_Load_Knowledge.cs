using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleFileBrowser;

public class Save_Load_Knowledge : MonoBehaviour
{
    public string GameSaveFileName;
    public bool testsave;
    public bool testload;
    public bool native;
    public string loadname;
    public string entityname;
    public string type;
    public ObjectKnowledge knowledge;
    public ObjectKnowledge test1;
    public GameType gametype;
    private void Update()
    {
        if (testsave)
        {
            if(native)
            SaveKnowledge(knowledge, entityname,GameSaveFileName,gametype);
            else
            SavePlayerData(knowledge, null);
            testsave = false;

        }
        if (testload)
        {
            if (native)
                test1 = Load_KnowledgeV2(Application.dataPath + "/Resources/" + GameSaveFileName + "/" + entityname + "/" + type + "/" + knowledge.objectname,entityname,GameSaveFileName,gametype);// + ".txt");

            else
                test1 = LoadPlayerData(Application.dataPath + "/Resources/" + GameSaveFileName + "/" + entityname + "/" + type + "/" + knowledge.objectname);// + ".txt");

            //test1 = LoadPlayerData(Application.dataPath + "/Resources/" + GameSaveFileName + "/" + entityname + "/" + type + "/" + knowledge.objectname + ".txt");
            testload = false;

        }

    }
    // Start is called before the first frame update
    #region Jsonutility
    public void SavePlayerData(ObjectKnowledge player, string path)
    {
        string jsonString = JsonUtility.ToJson(player, true);
        File.WriteAllText(Application.dataPath+"/Resources/"  + GameSaveFileName + "/" + entityname + "/" + type +"/"+knowledge.objectname+".txt", jsonString);
    }

    
    public ObjectKnowledge LoadPlayerData(string path)
    {
        string jsonString = File.ReadAllText(path);
        ObjectKnowledge data2 = ObjectKnowledge.CreateInstance<ObjectKnowledge>();
        JsonUtility.FromJsonOverwrite(jsonString,data2);

        return data2;
    }

#endregion


    public static void SaveKnowledge(ObjectKnowledge knowledge, string entity_name, string savefile,GameType gametype)
    {
        string gametypestring = System.Enum.GetName(typeof(GameType), gametype);

        foreach (ObjectKnowledge.ObjectType type in knowledge.type)
        {
            string typestring = System.Enum.GetName(typeof(ObjectKnowledge.ObjectType), type);

            if (!Directory.Exists(Application.dataPath + "/Resources/"+gametypestring+"/" + savefile + "/" + entity_name + "/" + typestring))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/" + gametypestring + "/" + savefile + "/" + entity_name + "/" + typestring);
                //string path = Application.dataPath + "/" + savefile + "/" + entity_name + "/" + typestring + "/" + knowledge.name + ".asset"; //can also use direct directory path

            }
            
                //Debug.Log("Exist1");
                //AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(knowledge), path);
                //string path =  "Assets/" + knowledge.name + ".asset"; //can also use direct directory path
                string path = Application.dataPath + "/Resources/" + gametypestring + "/" + savefile + "/" + entity_name + "/" + typestring + "/" + knowledge.objectname + ".asset";//+ "/" + knowledge.name + ".asset"; //can also use direct directory path
                string default_path = Application.dataPath + "/" + "Resources/Do Not Touch/Object_Default.asset";
                FileBrowserHelpers.CopyFile(default_path, path);
            //ObjectKnowledge asset = ObjectKnowledge.CreateInstance<ObjectKnowledge>();
            //AssetDatabase.CreateAsset(asset, path);

            //create substitute saves
            string jsonString = JsonUtility.ToJson(knowledge, true);
            File.WriteAllText(Application.dataPath + "/Resources/" + gametypestring + "/" + savefile + "/" + entity_name + "/" + typestring + "/" + knowledge.objectname + ".txt", jsonString);
        }




    }

    public static ObjectKnowledge Load_KnowledgeV2(string path,string entityname, string savefile,GameType gametype)
    {
        ObjectKnowledge request = null;

        string str1 = Application.dataPath+"/Resources/";
        string result = path.Replace(str1, "");

        //string gametypestring = System.Enum.GetName(typeof(GameType), gametype);

        Debug.Log(result);
        //if (File.Exists(path+".asset"))
        if (Resources.Load<ObjectKnowledge>(result) != null)
        {
            Debug.Log("Source Found");
             request = Resources.Load<ObjectKnowledge>(result);

        }
        else
        {
            Debug.Log("Restart needed");
            string jsonString = File.ReadAllText( path+".txt");
            request = ObjectKnowledge.CreateInstance<ObjectKnowledge>();
            request.name=Path.GetFileNameWithoutExtension(path + ".txt");
            //JsonUtility.FromJsonOverwrite(jsonString, data);
            JsonUtility.FromJsonOverwrite(jsonString, request);
            if (!File.Exists(path + ".asset"))
            {
                SaveKnowledge(request, entityname, savefile,gametype);
            }
        }

        return request;

        

    }


    /*
    public static ObjectKnowledge LoadKnowledge(string entity_name, string object_name, string savefile, string typestring)
    {
        string path = Application.dataPath + "/" + savefile + "/" + entity_name + "/" + typestring + "/" + object_name + ".asset"; //can also use direct directory path
        if (File.Exists(path))
        {
            Debug.Log("Exist0");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ObjectKnowledge data = formatter.Deserialize(stream) as ObjectKnowledge;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("save not found");
            return null;
        }
    }
    */
}
public enum GameType { Arcade,TopDown,TwoD,Classic};
