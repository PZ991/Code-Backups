using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(NavmeshAgentAI))]

public class NMAgentEditor : Editor
{
    // Start is called before the first frame update
    public SerializedProperty weights;
    public SerializedProperty manager;
    public SerializedProperty targets;
    public SerializedProperty changedestination;
    public SerializedProperty localdestination;
    public bool fold=true;

    List<float> valueweight;
    List<string> keyweight;
    private void OnEnable()
    {
        //weights = serializedObject.FindProperty("weights");
        manager = serializedObject.FindProperty("manager");
        targets = serializedObject.FindProperty("targets");
        changedestination = serializedObject.FindProperty("changedestination");
        localdestination = serializedObject.FindProperty("localdestination");
    }
    /*
    public override void OnInspectorGUI()
    {        NavmeshAgentAI script = (NavmeshAgentAI)target;


        List<float> valueweight= new List<float>();
        List<string> keyweight= new List<string>();
        if (fold=EditorGUILayout.Foldout(fold, new GUIContent("Dictionary")))
        {
            if (script.weights!=null)
            {
                foreach (var item in script.weights)
                {

                    valueweight.Add(item.Value);
                    keyweight.Add(item.Key);



                }
                
                script.weights.Clear();
                for (int i = 0; i < keyweight.Count; i++)
                {

                    EditorGUILayout.BeginHorizontal();
                    string text =EditorGUILayout.TextField(keyweight[i]);
                    float value =float.Parse(EditorGUILayout.TextField(valueweight[i].ToString()));
                    EditorGUILayout.EndHorizontal();
                    script.weights.Add(text, value);
                    EditorUtility.SetDirty(script);

                }
            }
            if (GUILayout.Button("+"))
            {
                script.weights.Add("", 0);
            }
        }
        EditorGUILayout.PropertyField(manager);
        EditorGUILayout.PropertyField(targets);
        EditorGUILayout.PropertyField(changedestination);
        EditorGUILayout.PropertyField(localdestination);
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();

    }
    */
}
