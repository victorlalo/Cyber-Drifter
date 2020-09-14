using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{
    public static Dictionary<AirTrick, string> trickDict = new Dictionary<AirTrick, string>()
    {
        { AirTrick.FRONT_FLIP, "Front Flip" },
        { AirTrick.BACK_FLIP, "Back Flip" },
        { AirTrick.BARREL_ROLL, "Barrel Roll" }
    };


    public static float Remap(float val, float minIn, float maxIn, float minOut, float maxOut)
    {
        return minOut + (val - minIn) * ((maxOut - minOut) / (maxIn - minIn));
    }

    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
