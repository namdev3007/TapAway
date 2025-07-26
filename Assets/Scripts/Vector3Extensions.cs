using UnityEngine;

public static class Vector3Extensions
{
    public static Transform GetTransform(this Vector3 position)
    {
        GameObject temp = new GameObject("TempCameraTarget");
        temp.transform.position = position;
        return temp.transform;
    }
}
