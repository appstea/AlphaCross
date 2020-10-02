using UnityEngine;

public class Letter : MonoBehaviour
{
    public char AlphabetLetter;

    public void Initialize(Vector3 spawnArea, Vector3 spawnCenter, Color color)
    {
        var meshSize = GetComponent<MeshRenderer>().bounds.size;
        var maxSide = Mathf.Max(Mathf.Max(meshSize.x, meshSize.y), meshSize.z);
        var maxBox = new Vector3(maxSide,maxSide,maxSide);
        var correctSpawnArea = spawnArea - new Vector3(maxSide, maxSide, maxSide);
        var newPosition = Helper.RandomPointInBox(spawnCenter, correctSpawnArea);
        while (Physics.BoxCast(newPosition, maxBox, Vector3.zero, out RaycastHit _))
        {
            newPosition = Helper.RandomPointInBox(spawnCenter, correctSpawnArea);
        }
        transform.position = newPosition;
        GetComponent<MeshRenderer>().material.color = color;
    }
}