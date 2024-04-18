using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MainAI : MonoBehaviour
{

    public AIFighterAI fighting;
    public AIEyesHead head;
    public AIMovementAI moveming;
    public ObjectKnowledge knowledge;
    public bool once;
    public List<float> val1;
    public List<bool> val2;
    public List<bool> actual;
    public Transform mainobj;
    public Transform boneobj;
}
