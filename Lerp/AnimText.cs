using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
public class AnimText : MonoBehaviour
{
    public AnimationClip clip;
    public Animator anim;
    public string[] import;
    public string[] export;
    public string jsonString;
    public string importjsonString;
    
    // Start is called before the first frame update
    void Start()
    {
        //jsonString = File.ReadAllText(Application.dataPath + "/Resources/Custom/Animation/New Animation.anim");
        //importjsonString = jsonString;
        // import=jsonString.Split("m_Legacy: 0");
        //Debug.Log(Application.dataPath + "/Resources/New Animation");
        File.WriteAllText(Application.streamingAssetsPath + "/Custom/Animation/New Animation.anim", importjsonString);
                
        clip = Resources.Load<AnimationClip>("New Animation");
        Debug.Log(clip.name);
        
        //JsonUtility.FromJsonOverwrite(jsonString, clip);
    }
    

}
