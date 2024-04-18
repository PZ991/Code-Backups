using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character_Info", menuName = "ScriptableObjects/CharacterInfo",order =1)]
[System.Serializable]
public class General_info : ScriptableObject
{
    [Header("General")]
    public ObjectKnowledge species;
    public string name;
    public float General_health;
    public float General_strength;
    public float muscle_thickness;
    public float fat_thickness;
    public float skin_durability;
    public float bone_max_velocity_input;
    //public List<ObjectKnowledge> Individual_knowledge;
    //public List<ObjectKnowledge> almost_forgotten_knowledge;
    //public List<ObjectKnowledge> Individual_Known_sapient_being;        //humans etc

    [Header("AI self Evaluation")]
    public float strength;          //offensive capabilities
    public float Height_fall_limit;
    public float defensive_strength;// defensive_capabilities;
    public List<Traits> real_traits;


    public enum Traits {Masochist,Never_Surrender,Berserker,Defensive,Patient,Sickly,Asthmatic,Calm,Panic,Nervious,
                        Agile_Swift,Coward,Fighter,Smart,Trapper,Bookworm,Forgetful};
}
