using UnityEngine;

public static class Helper
{
    public static Vector3 RandomPointInBox(Vector3 center, Vector3 size) {
 
        return center + new Vector3(
            (Random.value - 0.5f) * size.x,
            (Random.value - 0.5f) * size.y,
            (Random.value - 0.5f) * size.z
        );
    }
}