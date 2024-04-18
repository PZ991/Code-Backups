using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CurveData", menuName = "ScriptableObjects2/CurveData")]
[System.Serializable]
public class LerpData : ScriptableObject
{
    public string path;
    public string InstanceID;

    public CurveData PositionX = new CurveData();
    public CurveData PositionY = new CurveData();
    public CurveData PositionZ = new CurveData();
    public CurveData RotationX = new CurveData();
    public CurveData RotationY = new CurveData();
    public CurveData RotationZ=new CurveData();
    private bool IK;
    public CurveData IKVal = new CurveData();
    public List<CurveData> Customdata = new List<CurveData>();
    public List<string> Type = new List<string>();
    public List<string> property = new List<string>();
    public Color Posxcol;
    public Color Posycol;
    public Color Poszcol;
    public Color Rotxcol;
    public Color Rotycol;
    public Color Rotzcol;
    public Color IKcol;
    public List<Color> Customdatacolor=new List<Color>();
    public bool IsMainAnimator;
    public bool UseSceneAnimator;
    
}
