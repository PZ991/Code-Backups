using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Eye : MonoBehaviour
{
    [Header("FOV")]
    [SerializeField]
    public float radius;
    [Range(0, 360)]
    public float angle;
}
