using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public enum ROUNDING { UP, DOWN, CLOSEST }

    public static float ToNearestMultiple(this float f, int multiple, ROUNDING roundTowards = ROUNDING.CLOSEST)
    {
        f /= multiple;

        return (roundTowards == ROUNDING.UP ? Mathf.Ceil(f) : (roundTowards == ROUNDING.DOWN ? Mathf.Floor(f) : Mathf.Round(f))) * multiple;
    }

    /// <summary>
    /// Using a multiple with a maximum of two decimal places, will round to this value based on the ROUNDING method chosen
    /// </summary>
    public static float ToNearestMultiple(this float f, float multiple, ROUNDING roundTowards = ROUNDING.CLOSEST)
    {
        f = float.Parse((f * 100).ToString("f0"));
        multiple = float.Parse((multiple * 100).ToString("f0"));

        f /= multiple;

        f = (roundTowards == ROUNDING.UP ? Mathf.Ceil(f) : (roundTowards == ROUNDING.DOWN ? Mathf.Floor(f) : Mathf.Round(f))) * multiple;

        return f / 100;
    }
}
