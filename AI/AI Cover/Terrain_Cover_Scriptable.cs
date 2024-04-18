using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TerrainCover", menuName = "ScriptableObjects2/Terrain_Cover_Scriptable")]

[System.Serializable]
public class Terrain_Cover_Scriptable : ScriptableObject
{
    public List<Vector3> terrain_cover;
}
