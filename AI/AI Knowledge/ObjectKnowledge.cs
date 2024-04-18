using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Object_Info", menuName = "ScriptableObjects/ObjectKnowledge")]

[System.Serializable]
public class ObjectKnowledge : ScriptableObject
{
    //general description of an object that entities must know

    // public ObjectKnowledge collectionofInfo;
    //public string ObjectName;
    public ObjectKnowledge owner;
    public List<ObjectType> type;
    public string typeclassification;
    public string objectname;

    //crafting-value-recipe-grid
    public List<CraftingInfo> crafting = new List<CraftingInfo>();
    public bool liquid;

    public bool stackable;
    public int stacksize;
    public int stacksizemax;
    public bool singleuseitem;
    public int numofuse;
    public int numofusemax;

    public List<GridInfo> Grid = new List<GridInfo>();
    //public int sizemultiplierworld;

    public List<DefaultAnim_Scriptable> EntityAnimationUsage;
    public List<string> EntityAnimationTypesusage;


    public List<CommandValue> commands;
    // Need Connection Ratio Usage

    public List< ObjectValueFloat> floatvalues=new List< ObjectValueFloat>();

    public List< ObjectValueBool> boolvalues = new List< ObjectValueBool>();

    public List<SummonValue> summons = new List<SummonValue>();

    public List<AnimationSelf> SelfAnimation;

    public ProjectileValue projectileval;



    //CollectiveSettings
    public bool isCollection;
    public List<string> objectnames;
    public List<string> objecttargetvalue;
    public List<bool> effect_within_collider_only;
    public List<bool> effect_outside_collider_only;
    public List<bool> effect_touching_collider;
    public List<bool> need_minor_replacement;
    public List<bool> need_major_replacement;
    public List<bool> need_critical_replacement;


    public List<AnimationUser> AnimUsers;


    public ObjectControlType controls;


    #region Food
    //[Header("Food")]
    public bool completelyedible;
    public bool has_bone;
    public bool has_fish_bones;
    public float bone_radius;
    public float percent_of_bone_in_food;
    public float weight;
    public float softness;
    public bool has_handle;
    public Transform Food_handle;
    public float meat_nutrition;
    public float vegetable_nutrition;
    public List<FoodEffects> food_effects;
    public List<float> effect_intensity;
    #endregion

    #region Object
    //[Header("Object")]
    #region Weapon
    public bool weapon;

    public Vector3 OrientationDirection;
    public bool throwable;

    //needs auto feature

    //head

    //need multiple heads - waraxe, hamaxe
    public bool head_weight;
    public Transform head;
    public float head_sharpness;
    public float head_dullness;             //otherside_dullness_unless double_edge
    public float head_general_durability;
    public bool head_double_edge;
    public bool triangulate_handling;
    public Transform head_handle_start;
    public Transform head_handle_end;
    public float head_radius;
    public ObjectKnowledge head_material;
    public List<Transform> head_spikes;
    public float head_number_of_spikes; //for optimization and single collider
    public float head_spike_sharpness;

    //body
    public bool body_weight;
    public Transform body;
    public Transform body_protetion;
    public float body_sharpness;
    public float body_dullness;
    public float body_general_durability;
    public bool body_double_edge;
    public float body_radius;
    public ObjectKnowledge body_material;
    public List<Transform> body_spikes;
    public float body_number_of_spikes; //for optimization and single collider
    public float body_spike_sharpness;
    //handle
    public bool handle_weight;
    public Transform handle;
    public bool is_also_body;
    public float handle_sharpness;
    public float handle_dullness;
    public float handle_general_durability;
    public bool handle_double_edge;
    public Transform handle_handle_start;
    public Transform handle_handle_end;
    public float handle_radius;
    public ObjectKnowledge handle_material;
    public List<Transform> handle_spikes;
    public float handle_number_of_spikes; //for optimization and single collider
    public float handle_spike_sharpness;
    #endregion

    //others
    public bool edible;
    public FoodEffects objecteffects; //healing potions etc
    public General_Shape shape;
    public ObjectKnowledge animalrepresentation;
    public ObjectKnowledge humanrepresentation;

    //message
    public Transform From;
    public Transform Destination;
    public ObjectKnowledge Contacts;
    public string contact_id;
    public List<ObjectKnowledge> multiple_contacts;

    public bool handled_communication;      //mails
    public bool line_communication;         //telephones
    public bool instant_communication;      //phones

    //battery
    public bool needs_energy;
    public float energy_time;

    //Digging Mining
    public float dmining_AOE;
    public float dmining_depth;

    #endregion

    #region Produced_Materials
    //[Header("Produced_Materials")]
    public ObjectKnowledge material_from;
    public Material base_material_from;
    public float wornout_percent;
    public float material_dirtiness_percent;
    public float prettyness_percent;
    public float luminescence_percent;
    public float reflective_percent;
    public float slippery_percent;
    public float irritation_percent;
    public float flexibility_percent;
    public float stretching_percent;
    public float comfort_percent;
    public float disorientation_percent;
    public float weight_per_gram;
    public float density;
    #endregion

    
    #region Plants and Trees
    public List<ObjectKnowledge> common_places;
    public List<ObjectKnowledge> connected_species;
    public ObjectKnowledge currentland;
    public Transform leaves;
    public bool fix_sway;
    public bool need_sunlight;
    public bool hates_sunlight;
    public bool need_water;
    public bool hates_water;
    public float sunlight_growth_rate;
    public ObjectType seed;
    public Vector3 swaying_direction;
    public float gallons_of_water_need_per_day;
    public float plant_water_drinking_speed;
    public float radius_of_need_free_space;
    public ObjectKnowledge food;
    public float food_production_start_growth_rate;
    public float food_production_speed;
    #endregion
    
    #region Sapients                        
    public List<string> first_name;             //by species
    public List<string> middle_name;             
    public List<string> last_name;             
    public float General_min_health;
    public float General_max_health;
    public float General_min_strength;
    public float General_max_strength;
    public bool Can_Wall_run;
    public bool Knows_How_To_Swim;
    public bool Night_Owl;
    public bool Vampire_Skin;
    public bool Can_Fly;
    public FoodEffects immune;
    public List<float> immunitypercent;
    public FoodEffects poison;
    public List<float> weaknesspercent;

    public float min_muscle_thickness;
    public float max_muscle_thickness;
    public float min_fat_thickness;
    public float max_fat_thickness;
    public float min_skin_durability;
    public float max_skin_durability;
    public float bone_max_velocity_input;


    //public KnowledgeScriptable General_Knowledge;
    #endregion
    
    #region Animals
    public float General_min_health_animal;
    public float General_max_health_animal;
    public float General_min_strength_animal;
    public float General_max_strength_animal;
    public float General_min_stamina_animal;
    public float General_max_stamina_animal;
    public bool Can_Wall_run_animal;
    public bool Knows_How_To_Swim_animal;
    public bool Night_Owl_animal;
    public bool Vampire_Skin_animal;
    public bool Can_Fly_animal;
    public FoodEffects immune_animal;
    public List<float> immunitypercent_animal;
    public FoodEffects poison_animal;
    public List<float> weaknesspercent_animal;

    public float min_muscle_thickness_animal;
    public float max_muscle_thickness_animal;
    public float min_fat_thickness_animal;
    public float max_fat_thickness_animal;
    public float min_skin_durability_animal;
    public float max_skin_durability_animal;
    public float bone_max_velocity_input_animal;
    #endregion
                                                //transportation, weaponized, civilian, etc
    #region Moving or Firing Structures Vechicles
    public bool weaponized;
    public bool needs_magazines;
    public ObjectKnowledge magazine;
    public float ammo_count;
    public ObjectKnowledge material_composition_weapon;
    public float weapon_thickness;
    public ObjectKnowledge needed_puller;
    public bool needs_wheels;
    public List<ObjectKnowledge> wheels;

    #endregion
                                                //buildings

    #region Structures
    public bool defensive_structure;
    public bool civilian_structure;
    public float stuctural_integrity;
    public ObjectKnowledge structure_composition;
    public float thickness;
    public float structure_grid_support;
    public Transform supports;
    public float beauty_structure;
    public float ugliness_structure;
    public Materials materialcomp_struct;
    public float movementbonus_penalty;
    #endregion
                                                //Curtains, Glass,Chairs,Tables,Decals,Carpets,etc
    #region Decoration
    public float beauty_structure_decor;
    public float ugliness_structure_decor;
    public bool triangulate_placements;
    public Transform sitting_placements_start;
    public Transform sitting_placements_end;
    public Transform placement_start;
    public Transform placement_end;
    public Meaning meaning;
    #endregion

    #region Disease
    public string effect_name;
    public FoodEffects effect;
    public float percentage_death;
    public float increasing_multiplier;
    public float increasing_factor_temp;
    public float decreasing_multiplier;
    public float decreasing_factor_temp;
    public float medication_counter;
    public ObjectKnowledge Medicine;
    #endregion

    #region Medicine
    public string effect_name_med;
    public FoodEffects side_effect;
    public float percentage_heal;
    public float increasing_multiplier_med;
    public float increasing_factor_temp_med;
    public float decreasing_multiplier_med;
    public float decreasing_factor_temp_med;
    public ObjectKnowledge Disease;
    #endregion

    #region Hierarchy
    public List<List<string>> Hierarchy;
    //public <string> Societal_Hierarchy;
    //public List<string> Military_Hierarchy;
    //public List<string> Scientific_Hierarchy;
    //public List<string> Governent_Hierarchy;
    #endregion

    #region Places
    public ObjectKnowledge connected_places;
    public List<string> streets;
    public List<ObjectKnowledge> roads;
    //roads
    public List<ObjectKnowledge> connected_road;
    #endregion

    #region Hygiene
    public List<AnimationClip> things_needed_to_complete;
    public List<AnimationClip> things_not_needed_to_complete;
    #endregion

    //[Header("Landforms/Bodies of water")]
    //[Header("Biomes")]
    //[Header("Clans_Groups")]
    //[Header("Uncategorized")]
    //[Header("Cover")


    public enum ObjectType
    {
        Food,
        Object,
        Produced_Material,
        Plants_n_Trees,
        Sapient_Entity,
        Animals,
        Transportation,
        Structure,
        Disease,
        Medicine,
        Places_n_Destination,
        Landforms_n_bodiesofwater,
        Biomes,
        Clans_Groups,
        Uncategorized,
        Hierarchy,
        Hygiene,
        This,
        Numerical,
        Boolean,
        ConnectedChild,
        ConnectedParent
    };
    public enum FoodEffects
    {
        Poison,
        Food_Poisoning,
        Inflammation,
        Burning,
        Irritation,
        Cough,
        Diarrhea,
        Death,
        Nausea
    }
    public enum ObjectUse
    {
        Weapon_Melee,               //Sword,Knife,Axes,Hammers,Etc
        Weapon_Range,               //Bows,Gun,etc
        Artillery,                  //cannons,
        Hidden,                     //landmines,traps,etc
        Nature_Natural_Objects,     //rocks,boulder,bark, wooden logs, leaves
        Digging,                    //shovels etc
        Mining,                     //pickaxe, hammers, etc
    }
    public enum Materials
    {
        Wood,
        Cotton,
        Leather,
        Iron,
        Gold,
        Animal_Scale,
    }
    public enum General_Shape
    {
        Cube,
        Rectangular,
        Circular,
        Oval,
        Triangle,
        Cylinder,
        Animal,
        Humanoid,
        Sword,
        Hammer,
        Pike,
        Pickaxe,
        Axe,
        Shovel,
        Hoe
    }
    public enum Meaning
    {
        Mourning,
        Pleasure,
        Happiness,
        Beauty,
        Empire,
        Prosperity,
        Novelty,
        War
    }

    public enum Functionfloat {Add,Minus,Divide,Multiply,Modulus, Add_Continuous, Minus_Continuous, Divide_Continuous, Multiply_Continuous, Modulus_Continuous,Equals }
    public enum Functionbool {Greater,LessThan,GreaterOrEqual,LessThanOrEqual,Equal,OR,AND,XOR,TimePassed,Inactivity }
    public enum InteractionMode {Touched,Carrying,Consumed,Forced,Pressured,Stab,Slice,EdgeObject,CornerObject,Hit,This,Change,Instant,Inside,ConnectedChild,ConnectedParent}
    public enum FoodEating { Pickup,Spoon,Impaled,Drink,Scooped,Licked,None,Bite}
    public enum ObjectControlType { None, Two_handed_Object, One_Handed_Object, Two_handed_Gun, One_Handed_Gun, Two_handed_Melee, One_Handed_Melee };

}
    public enum Crafting { Instant,Mix,After,Pound,Pressurize,Leave,Process,Heat,Cold,WaitSecond}

//Link Consumption, single consumption
[System.Serializable]
public struct ObjectValueFloat
{
    public string Key;
    public float Value;
    public bool hastarget;
    public bool Wait_boolean;

    public ObjectKnowledge.Functionfloat Mode;
    public ObjectKnowledge.ObjectType targettype;
    public string target;
    public string targetValueKey;
    public ObjectKnowledge.InteractionMode Interaction;

    public List<ObjectKnowledge.ObjectType> booleantype;
    public List<string> booleantarget1;
    public List<string> booleantargetvaluekey1;
    public List<ObjectKnowledge.Functionbool> booleanfunction;
    public List<ObjectKnowledge.ObjectType> booleantype2;
    public List<string> booleantarget2;
    public List<string> booleantargetvaluekey2;


    public List<bool> onlyonce;
    public List<string> targetname;
    public List<string> booltargetname;
    public List<string> booltargetname2;

    public float distance;
    public List<string> ratekey;
    public List<float> rateval;


    public ObjectValueFloat(string key,float value,bool has_target,bool waitbool,ObjectKnowledge.Functionfloat func,string target2, ObjectKnowledge.ObjectType targettype2, ObjectKnowledge.InteractionMode interaction,List<ObjectKnowledge.Functionbool> boolwait, List<string> target3, List<ObjectKnowledge.ObjectType> targetbool, List<string> target4, List<ObjectKnowledge.ObjectType> targetbool2,float dist,List<string> ratiokey,List<float> ratiovalue,string cons1,List<string> consbool,List<string> consbool2,List<bool> once, List<string> targobj,List<string> booltargobj,List<string>booltargobj2)
            {
        Key = key;
        Value = value;
        hastarget = has_target;
        Wait_boolean = waitbool;

        Mode = func;

        target = target2;
        targettype = targettype2;
        Interaction=interaction;
        booleantarget1 = target3;
        booleantarget2 = target4;
    booleanfunction = boolwait;
        booleantype = targetbool;
        booleantype2 = targetbool2;
        distance = dist;
        ratekey = ratiokey;
        rateval = ratiovalue;
        targetValueKey = cons1;
            booleantargetvaluekey1 = consbool;
            booleantargetvaluekey2 = consbool2;
        onlyonce = once;
        targetname = targobj;
        booltargetname = booltargobj;
        booltargetname2 = booltargobj2;


    }
    public ObjectValueFloat(string key)
            {
        Key = key;
        Value = 0;
        Mode = ObjectKnowledge.Functionfloat.Add;
        hastarget = false;
        target = "";
        targettype = ObjectKnowledge.ObjectType.Animals;
        Wait_boolean = false;
        Interaction= ObjectKnowledge.InteractionMode.Carrying;
        booleantarget1 = new List<string>();
        booleantarget2 = new List<string>();
        booleanfunction = new List<ObjectKnowledge.Functionbool>();
        booleantype = new List<ObjectKnowledge.ObjectType>();
        booleantype2 = new List<ObjectKnowledge.ObjectType>();
        distance = 0;
        ratekey = new List<string>();
        rateval = new List<float>();
        targetValueKey = "";
        booleantargetvaluekey1 = new List<string>();
        booleantargetvaluekey2 = new List<string>();
        onlyonce = new List<bool>();
        targetname = new List<string>();
        booltargetname = new List<string>();
        booltargetname2 = new List<string>();
    }
    public ObjectValueFloat(float value, ObjectValueFloat KeepValues)
            {
        Key = KeepValues.Key+"-Copy";
        Value = value;
        Mode = KeepValues.Mode;
        hastarget = KeepValues.hastarget;
        target = KeepValues.target;
        targettype = KeepValues.targettype;
        Wait_boolean = KeepValues.Wait_boolean;
        Interaction= KeepValues.Interaction;
        booleantarget1 = KeepValues.booleantarget1;
        booleantarget2 = KeepValues.booleantarget2;
        booleanfunction = KeepValues.booleanfunction;
        booleantype = KeepValues.booleantype;
        booleantype2 = KeepValues.booleantype2;
        distance = KeepValues.distance;
        ratekey = KeepValues.ratekey;
        rateval = KeepValues.rateval;
        targetValueKey = KeepValues.targetValueKey;
            booleantargetvaluekey1 = KeepValues.booleantargetvaluekey1;
            booleantargetvaluekey2 = KeepValues.booleantargetvaluekey2;
        onlyonce = KeepValues.onlyonce;
        targetname = KeepValues.targetname;
        booltargetname = KeepValues.booltargetname;
        booltargetname2 = KeepValues.booltargetname2;
        //Per Change, Count change max
    }
    public ObjectValueFloat(string key,float value)
            {
        Key = key;
        Value = value;
        Mode = ObjectKnowledge.Functionfloat.Add;
        hastarget = false;
        target = "";
        targettype = ObjectKnowledge.ObjectType.Animals;
        Wait_boolean = false;
        Interaction= ObjectKnowledge.InteractionMode.Carrying;
        booleantarget1 = new List<string>();
        booleantarget2 = new List<string>();
        booleanfunction = new List<ObjectKnowledge.Functionbool>();
        booleantype = new List<ObjectKnowledge.ObjectType>();
        booleantype2 = new List<ObjectKnowledge.ObjectType>();
        distance = 0;
        ratekey = new List<string>();
        rateval = new List<float>();
        targetValueKey = "";
        booleantargetvaluekey1 = new List<string>();
        booleantargetvaluekey2 = new List<string>();
        onlyonce = new List<bool>();
        targetname = new List<string>();
        booltargetname = new List<string>();
        booltargetname2 = new List<string>();
    }



}
[System.Serializable]
public struct ObjectValueBool
{
    public string Key;
    public bool Value;
    public bool hastarget;
    public bool Wait_boolean;

    public ObjectKnowledge.Functionfloat Mode;
    public ObjectKnowledge.ObjectType targettype;
    public string target;
    public string targetValueKey;
    public ObjectKnowledge.InteractionMode Interaction;

    public List<ObjectKnowledge.ObjectType> booleantype;
    public List<string> booleantarget1;
    public List<string> booleantargetvaluekey1;
    public List<ObjectKnowledge.Functionbool> booleanfunction;
    public List<ObjectKnowledge.ObjectType> booleantype2;
    public List<string> booleantarget2;
    public List<string> booleantargetvaluekey2;


    public List<bool> onlyonce;
    public List<string> targetname;
    public List<string> booltargetname;
    public List<string> booltargetname2;

    public float distance;
    public List<string> ratekey;
    public List<bool> rateval;


    public ObjectValueBool(string key, bool value, bool has_target, bool waitbool, ObjectKnowledge.Functionfloat func, string target2, ObjectKnowledge.ObjectType targettype2, ObjectKnowledge.InteractionMode interaction, List<ObjectKnowledge.Functionbool> boolwait, List<string> target3, List<ObjectKnowledge.ObjectType> targetbool, List<string> target4, List<ObjectKnowledge.ObjectType> targetbool2, float dist, List<string> ratiokey, List<bool> ratiovalue, string cons1, List<string> consbool, List<string> consbool2, List<bool> once, List<string> targobj, List<string> booltargobj, List<string> booltargobj2)
    {
        Key = key;
        Value = value;
        hastarget = has_target;
        Wait_boolean = waitbool;

        Mode = func;

        target = target2;
        targettype = targettype2;
        Interaction = interaction;
        booleantarget1 = target3;
        booleantarget2 = target4;
        booleanfunction = boolwait;
        booleantype = targetbool;
        booleantype2 = targetbool2;
        distance = dist;
        ratekey = ratiokey;
        rateval = ratiovalue;
        targetValueKey = cons1;
        booleantargetvaluekey1 = consbool;
        booleantargetvaluekey2 = consbool2;
        onlyonce = once;
        targetname = targobj;
        booltargetname = booltargobj;
        booltargetname2 = booltargobj2;


    }
    public ObjectValueBool(string key)
    {
        Key = key;
        Value = false;
        Mode = ObjectKnowledge.Functionfloat.Add;
        hastarget = false;
        target = "";
        targettype = ObjectKnowledge.ObjectType.Animals;
        Wait_boolean = false;
        Interaction = ObjectKnowledge.InteractionMode.Carrying;
        booleantarget1 = new List<string>();
        booleantarget2 = new List<string>();
        booleanfunction = new List<ObjectKnowledge.Functionbool>();
        booleantype = new List<ObjectKnowledge.ObjectType>();
        booleantype2 = new List<ObjectKnowledge.ObjectType>();
        distance = 0;
        ratekey = new List<string>();
        rateval = new List<bool>();
        targetValueKey = "";
        booleantargetvaluekey1 = new List<string>();
        booleantargetvaluekey2 = new List<string>();
        onlyonce = new List<bool>();
        targetname = new List<string>();
        booltargetname = new List<string>();
        booltargetname2 = new List<string>();
    }
    public ObjectValueBool(bool value, ObjectValueBool KeepValues)
    {
        Key = KeepValues.Key + "-Copy";
        Value = value;
        Mode = KeepValues.Mode;
        hastarget = KeepValues.hastarget;
        target = KeepValues.target;
        targettype = KeepValues.targettype;
        Wait_boolean = KeepValues.Wait_boolean;
        Interaction = KeepValues.Interaction;
        booleantarget1 = KeepValues.booleantarget1;
        booleantarget2 = KeepValues.booleantarget2;
        booleanfunction = KeepValues.booleanfunction;
        booleantype = KeepValues.booleantype;
        booleantype2 = KeepValues.booleantype2;
        distance = KeepValues.distance;
        ratekey = KeepValues.ratekey;
        rateval = KeepValues.rateval;
        targetValueKey = KeepValues.targetValueKey;
        booleantargetvaluekey1 = KeepValues.booleantargetvaluekey1;
        booleantargetvaluekey2 = KeepValues.booleantargetvaluekey2;
        onlyonce = KeepValues.onlyonce;
        targetname = KeepValues.targetname;
        booltargetname = KeepValues.booltargetname;
        booltargetname2 = KeepValues.booltargetname2;
        //Per Change, Count change max
    }
    public ObjectValueBool(string key, bool value)
    {
        Key = key;
        Value = value;
        Mode = ObjectKnowledge.Functionfloat.Add;
        hastarget = false;
        target = "";
        targettype = ObjectKnowledge.ObjectType.Animals;
        Wait_boolean = false;
        Interaction = ObjectKnowledge.InteractionMode.Carrying;
        booleantarget1 = new List<string>();
        booleantarget2 = new List<string>();
        booleanfunction = new List<ObjectKnowledge.Functionbool>();
        booleantype = new List<ObjectKnowledge.ObjectType>();
        booleantype2 = new List<ObjectKnowledge.ObjectType>();
        distance = 0;
        ratekey = new List<string>();
        rateval = new List<bool>();
        targetValueKey = "";
        booleantargetvaluekey1 = new List<string>();
        booleantargetvaluekey2 = new List<string>();
        onlyonce = new List<bool>();
        targetname = new List<string>();
        booltargetname = new List<string>();
        booltargetname2 = new List<string>();
    }

}



[System.Serializable]
public struct CommandValue
{
    public string Activator;
    public string Deactivator;
    public bool Continuous;
    public List<string> TargetClasses;
    public ObjectKnowledge.ObjectType TargetTypes;
    public float radiustarget;
    public ObjectKnowledge.InteractionMode Interaction;

    public string Type;
    public string Method;
    public string Value;
    public List<string> stringparameter;
    public List<float> floatparameter;
    public List<bool> boolparameter;
    public List<Vector3> vectorparameter;
    public List<Quaternion> quaternionparameter;

    public List<string> methodparameters;       //Something like transform.position.x as a parameter or (0) position as replacement to value
    public ObjectKnowledge.Functionfloat methodparameterfunctions;

    public CommandValue(string activator,string deactivator,bool continuous,List<string> classes,ObjectKnowledge.ObjectType types,float radiusList,ObjectKnowledge.InteractionMode interact, string type,string method,string value, List<string> strings, List<float> floats, List<bool> bools, List<Vector3> vectors, List<Quaternion> quaternions,List<string> methodsparam, ObjectKnowledge.Functionfloat func)
    {
        Activator = activator;
        Deactivator = deactivator;
        Continuous = continuous;
        TargetClasses = classes;
        TargetTypes = types;
        radiustarget = radiusList;
        Interaction = interact;
        Type = type;
        Method = method;
        Value = value;
        stringparameter = strings;
        floatparameter = floats;
        boolparameter = bools;
        vectorparameter = vectors;
        quaternionparameter = quaternions;
        methodparameters = methodsparam;
        methodparameterfunctions = func;
    }
     public CommandValue(CommandValue copy)
    {
        Activator = copy.Activator;
        Deactivator = copy.Deactivator;
        Continuous = copy.Continuous;
        TargetClasses = copy.TargetClasses;
        TargetTypes = copy.TargetTypes;
        radiustarget = copy.radiustarget;
        Interaction = copy.Interaction;
        Type = copy.Type;
        Method = copy.Method;
        Value = copy.Value;
        stringparameter = copy.stringparameter;
        floatparameter = copy.floatparameter;
        boolparameter = copy.boolparameter;
        vectorparameter = copy.vectorparameter;
        quaternionparameter = copy.quaternionparameter;
        methodparameters = copy.methodparameters;
        methodparameterfunctions = copy.methodparameterfunctions;
    }

}

[System.Serializable]
public struct SummonValue   
{
    public string Activator;
    public string TargetingDeactivator;
    public ObjectKnowledge.ObjectType TargetType;
    public string TargetClass;
    public ObjectKnowledge.InteractionMode Interact;
    public ObjectKnowledge objectsummon;

    //targeting
    
    public bool usedirection;
    public Vector3 offsettargetingmin;
    public Vector3 offsettargetingmax;
    public bool facetarget;

    //spawining
    public bool allowspawnunderground;
    public bool spawnOnSelf;
    public bool SpawnOnTarget;
    public bool spawninbetween;

    public bool randomspawndistance;

    public bool AdjustingSelfdist;
    public float SpawnMin;
    public float SpawnMax;

    public bool Adjustingdistnormalized;

    public bool isprojectile;
    
    public SummonValue(string key, string targetdeactivator, ObjectKnowledge.ObjectType targettype, string targetclass, ObjectKnowledge.InteractionMode interaction, ObjectKnowledge objectsummoned,bool usedirect,Vector3 offsettarget1,Vector3 offsettarget2,bool facetarget1, bool allowunder,bool spawnself,bool spawnontarget,bool spawnbetween,bool randomspawndist,bool adjust, float adjustmin, float adjustmax,bool projectile,bool Adjustingdistnormalized2)
    {
         Activator=key;
     TargetingDeactivator=targetdeactivator;
    TargetType=targettype;
    TargetClass=targetclass;
   Interact=interaction;
     objectsummon=objectsummoned;
    //targeting
    offsettargetingmin=offsettarget1;
    offsettargetingmax=offsettarget2;
    facetarget=facetarget1;
        usedirection = usedirect;
   //spawining
   allowspawnunderground =allowunder;
    spawnOnSelf=spawnself;
    SpawnOnTarget=spawnontarget;
    spawninbetween=spawnbetween;
 
    randomspawndistance=randomspawndist;

        AdjustingSelfdist = adjust;
        SpawnMin = adjustmin;
        SpawnMax = adjustmax;
        isprojectile = projectile;
        Adjustingdistnormalized = Adjustingdistnormalized2;
}
    public SummonValue(SummonValue copy)
    {
        Activator = copy.Activator;
        TargetingDeactivator = copy.TargetingDeactivator;
        TargetType = copy.TargetType;
        TargetClass = copy.TargetClass;
        Interact = copy.Interact;
        objectsummon = copy.objectsummon;
        //targeting
        offsettargetingmin = copy.offsettargetingmin;
        offsettargetingmax = copy.offsettargetingmax;
        facetarget = copy.facetarget;
        usedirection = copy.usedirection;
        //spawining
        allowspawnunderground = copy.allowspawnunderground;

        spawnOnSelf = copy.spawnOnSelf;
        SpawnOnTarget = copy.SpawnOnTarget;
        spawninbetween = copy.spawninbetween;

        randomspawndistance = copy.randomspawndistance;

        AdjustingSelfdist = copy.AdjustingSelfdist;
        SpawnMin = copy.SpawnMin;
        SpawnMax = copy.SpawnMax;
        isprojectile = copy.isprojectile;
        Adjustingdistnormalized = copy.Adjustingdistnormalized;
    }



}

[System.Serializable]
public struct AnimationSelf
{
    public string Key;
    public string RelativePath;
    public string Type;
    public string Value;
    public AnimationCurve Curve;
    public AnimationClip clip;
    public bool loop;
    public AnimationSelf(string key, string path, string type, string val, AnimationCurve curve, AnimationClip clips, bool looping)
    {
        Key = key;
        RelativePath = path;
        Type = type;
        Value = val;
        Curve = curve;
        clip = clips;
        loop = looping;
    }
    public AnimationSelf(AnimationSelf copy)
    {
        Key = copy.Key;
        RelativePath = copy.RelativePath;
        Type = copy.Type;
        Value = copy.Value;
        Curve = copy.Curve;
        clip = copy.clip;
        loop = copy.loop;
    }
}
[System.Serializable]

public struct ProjectileValue
{
    public string Activator;
    public string TargetingDeactivator;

    //targeting
    public bool targetingwhennear;
    public bool stoptargetingwhenoutofdist;
    public float targetingdist;


    public bool smarttargeting;
    public float TargetingRotationSpeed;
    public float starttargetingtime;
    public float startrotationtime;

    //acceleration
    public bool acceleratewhennear;
    public bool stopaccelerationdistance;
    public bool stopspeedoutofdist;
    public float acceleratedist;
    public bool destroyonmaxacceleration;

    public float accelerationtime;
    public float accelerationspeed;
    public Vector2 accelerationminmax;

    //transform life
    public bool destroyifoutofdist;
    public float lifetimedist;

    public bool decay;
    public float projectilelifetime;
    public float endtargetingtime;
    public float endrotationtime;
    public float endaccelaration;
    public float DestroyTime;

    public int repeatnumspawn;
    public float timebetweenspawn;
    //targting max angling
    public bool destroyifoutofangle;
    public float Angle;
    public float AngleXY;
    public float AngleZY;

    //AOE

    public bool explode;
    public float explodedistance;
    public float delayondistanceORcontact;
    public float minpressure;
    public bool armingdistance;
    public float armingdist;
    public float timestartrearm;
    public bool unarmingdistance;
    public float unarmingdist;
    public float timeendrearm;
    public Vector2 AoE;

    public float increasingspeed;
    public bool destroyexplode;
    public float destroytimeaoe;
    public float startaoedamage;
    public float endaoedamage;

    //Others
    public bool Laser;
    public bool projectileAsLaser;

    public float startlaserdamage;
    public float endlaserdamage;
    public bool middledamage;
    public float middledamagemultiplier;

    public bool fade;
    public float fadestart;
    public float fadespeed;
    public bool drain;
    public float drainstart;
    public float drainspeed;
    public Vector3 directiondrain;
    public bool destroyonhit;
    public bool wall;
    public float rangelaser;

    public bool instantRotate;
    public ProjectileValue(string activator, string deactivator, bool targetnear, bool targoutdiststop, float targdist, bool smart, float rotspeed, float targtime, float rottime, bool accnear, bool stopaccout, bool stopspeedout, float accdist, bool destroymaxacc, float acctime, float accspeed, Vector2 accminmax, bool destoutdist, float lifedist, bool decays, float projlife, float endtargtime, float endrottime, float endacctime, bool destoutangle, float angle, float xy, float zy, bool exp, float expdist, Vector2 aoe, float delayaoe, float minpress, bool armdistance, float armdist, float timearm, bool unarmdistance, float unarmdist, float timeendarm,float destroytime,float aoeincreasespeed,bool destroyonaoe,float destroyaoe,float startaoedam,float endaoedam,int repeatnum,float timespawn,bool islaser,bool projectilelaser,float startlaserdam,float endlaserdam,bool lasermiddam,float middledammul,bool fadelaser,float fadestartlaser,float fadespeedlaser,bool drainlaser,float drainstartlaser,Vector3 directiondrainlaser, float drainspeedlaser,bool destroyhit,bool iswall,float laserrange,bool rotatefast)
    {
        instantRotate = rotatefast;
        projectileAsLaser = projectilelaser;
        rangelaser = laserrange;
        fade = fadelaser;
        fadestart = fadestartlaser;
        fadespeed = fadespeedlaser;
        drain = drainlaser;
        drainstart = drainstartlaser;
        drainspeed = drainspeedlaser;
        destroyonhit = destroyhit;
        wall = iswall;
        directiondrain = directiondrainlaser;

        repeatnumspawn = repeatnum;
        timebetweenspawn = timespawn;
        Laser = islaser;
        startlaserdamage = startlaserdam;
        endlaserdamage = endlaserdam;
        middledamage = lasermiddam;
        middledamagemultiplier = middledammul;




        Activator = activator;
        TargetingDeactivator = deactivator;


        //targeting
        targetingwhennear = targetnear;
        stoptargetingwhenoutofdist = targoutdiststop;
        targetingdist = targdist;


        smarttargeting = smart;
        TargetingRotationSpeed = rotspeed;
        starttargetingtime = targtime;
        startrotationtime = rottime;

        //acceleration
        acceleratewhennear = accnear;
        stopaccelerationdistance = stopaccout;
        stopspeedoutofdist = stopspeedout;
        acceleratedist = accdist;
        destroyonmaxacceleration = destroymaxacc;

        accelerationtime = acctime;
        accelerationspeed = accspeed;
        accelerationminmax = accminmax;


     
        //transform life
        destroyifoutofdist=destoutdist;
        lifetimedist=lifedist;
        DestroyTime = destroytime;
        decay=decays;
        projectilelifetime=projlife;
        endtargetingtime=endtargtime;
        endrotationtime=endrottime;
        endaccelaration=endacctime;
        //targting max angling
        destroyifoutofangle=destoutangle;
        Angle=angle;
        AngleXY=xy;
        AngleZY=zy;

        //AOE

        explode=exp;
        explodedistance=expdist;
        AoE=aoe;
        delayondistanceORcontact=delayaoe;
        minpressure=minpress;
       armingdistance=armdistance;
     armingdist=armdist;
     timestartrearm=timearm;
    unarmingdistance=unarmdistance;
     unarmingdist=unarmdist;
     timeendrearm=timeendarm;

        increasingspeed = aoeincreasespeed;
        destroyexplode = destroyonaoe;
        destroytimeaoe = destroyaoe;
        startaoedamage = startaoedam;
        endaoedamage = endaoedam;
    }
   

}

[System.Serializable]
public struct AnimationUser
{
    public string HolderParent;
    public string BoneParent;
    public string RootBone;
    public List<int> Layer;
    public List<Vector3> holdingpoint;
    public List<Quaternion> rotationorientation;

    public DefaultAnim_Scriptable EntityAnimationUsage;
    public string EntityAnimationTypesusage;

    public AnimationUser(int zero)
    {
        HolderParent = "";
        BoneParent = "";
        RootBone = "";

        Layer = new List<int>();
        holdingpoint = new List<Vector3>();
        rotationorientation = new List<Quaternion>();
        EntityAnimationUsage = new DefaultAnim_Scriptable();
        EntityAnimationTypesusage = "";

    }
    public AnimationUser(string holder, string bone, string root, List<int> layer, List<Vector3> holding, List<Quaternion> orientation, DefaultAnim_Scriptable entity, string EntityType)
    {
        HolderParent = holder;
        BoneParent = bone;
        RootBone = root;

        Layer = layer;
        holdingpoint = holding;
        rotationorientation = orientation;
        EntityAnimationUsage = entity;
        EntityAnimationTypesusage = EntityType;

    }
    public AnimationUser(AnimationUser copy)
    {
        HolderParent = copy.HolderParent;
        BoneParent = copy.BoneParent;
        RootBone = copy.RootBone;

        Layer = copy.Layer;
        holdingpoint = copy.holdingpoint;
        rotationorientation = copy.rotationorientation;
        EntityAnimationUsage = copy.EntityAnimationUsage;
        EntityAnimationTypesusage = copy.EntityAnimationTypesusage;

    }
    

}

[System.Serializable]
public struct GridInfo
{
    public string Grid_Name;
    public bool ThirdDimensinal;
    public List<Vector3Int> Grid_pos ;
    public int sizemultiplierinv;
    public GridInfo(string name,bool threed,List<Vector3Int> gridpos,int multip)
    {
        this.Grid_Name = name;
        this.ThirdDimensinal = threed;
        this.Grid_pos = gridpos;
        this.sizemultiplierinv = multip;

    }
    public GridInfo(GridInfo copy)
    {
        this.Grid_Name = copy.Grid_Name;
        this.ThirdDimensinal = copy.ThirdDimensinal;
        this.Grid_pos = copy.Grid_pos;
        this.sizemultiplierinv = copy.sizemultiplierinv;
    }
}

[System.Serializable]
public struct CraftingInfo
{
    public List<ObjectKnowledge> Recipes;
    public Crafting craft;
    public List<ObjectValueBool> craftvalue ;
    public List<ObjectValueFloat> recipeamount ;

    public ObjectKnowledge craftingstation ;
    public List<ObjectKnowledge> output;

    public bool charactermovingcrafting;
    public bool charactercanleavecraftingwindow;
    public float normalizedcraftingleft;
    public bool animationbasedtimecrafting;

    public float stationdistancemin;
    public bool stayinstation;
    public bool movetoinventory;
    public CraftingInfo(List<ObjectKnowledge> recipes, Crafting crft, List<ObjectValueBool> value, List<ObjectValueFloat> amount, ObjectKnowledge station, List<ObjectKnowledge> outputs, bool movecraft, bool leave, float craftingleft, bool timecraft, float distmin, bool stay, bool moveinv)
    {
        Recipes = recipes;
        craft = crft;
        craftvalue = value;
        recipeamount = amount;
        craftingstation = station;
        output = outputs;

        charactermovingcrafting = movecraft;
        charactercanleavecraftingwindow = leave;
        normalizedcraftingleft = craftingleft;
        animationbasedtimecrafting = timecraft;

        stationdistancemin = distmin;
        stayinstation = stay;
        movetoinventory = moveinv;
    }
    public CraftingInfo(int newnum)
    {
        Recipes = new List<ObjectKnowledge>();
        craft = Crafting.Instant;
        craftvalue = new List<ObjectValueBool>();
        recipeamount = new List<ObjectValueFloat>();
        craftingstation = null;
        output = new List<ObjectKnowledge>();

        charactermovingcrafting = false;
        charactercanleavecraftingwindow = false;
        normalizedcraftingleft = 0f;
        animationbasedtimecrafting = false;

        stationdistancemin = 0f;
        stayinstation = false;
        movetoinventory = false;
    }
    

}


