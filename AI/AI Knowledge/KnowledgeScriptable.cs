using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity_Knowledge", menuName = "ScriptableObjects/EntityKnowledge")]
[System.Serializable]
public class KnowledgeScriptable : ScriptableObject
{
    //objects that are known to an a group of entity
    public List<ObjectKnowledge> Food;
    public List<ObjectKnowledge> Object;
    public List<ObjectKnowledge> Produced_Materials;
    public List<ObjectKnowledge> Plants_n_Trees;
    public List<ObjectKnowledge> Poisonous;
    public List<ObjectKnowledge> Sapient;                       //humans something like that
    public List<ObjectKnowledge> Animals;
    public List<ObjectKnowledge> Transportation;
    public List<ObjectKnowledge> Structure;                     //buildings and roads and structures
    public List<ObjectKnowledge> Decoration;                    //Decals and decoration objects
    public List<ObjectKnowledge> Disease;
    public List<ObjectKnowledge> Medicine;
    public List<ObjectKnowledge> Places_n_Destination;
    public List<ObjectKnowledge> Landforms_n_bodiesofwater;
    public List<ObjectKnowledge> Biomes;
    public List<ObjectKnowledge> Uncategorized;
    public List<ObjectKnowledge> Clans_Groups;
    public List<ObjectKnowledge> Hygiene;
    public List<ObjectKnowledge> Hierarchy;
    
}
