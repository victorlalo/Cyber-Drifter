using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{
    public static float Remap(float val, float minIn, float maxIn, float minOut, float maxOut)
    {
        return minOut + (val - minIn) * ((maxOut - minOut) / (maxIn - minIn));
    } 
}
