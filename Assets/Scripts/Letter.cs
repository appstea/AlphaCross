using UnityEngine;

public class Letter : MonoBehaviour
{
    public char AlphabetLetter;
    public Vector3 CenterPoint;

    public void Initialize(Vector3 spawnArea, Vector3 spawnCenter, Color color)
    {
        CenterPoint = GetComponent<BoxCollider>().bounds.extents;
        CenterPoint = new Vector3(CenterPoint.x, 0, CenterPoint.y);
        var maxBox = new Vector3(CenterPoint.y*2,CenterPoint.y*2,CenterPoint.y*2);
        var correctSpawnArea = spawnArea - maxBox;
        var newPosition = Helper.RandomPointInBox(spawnCenter, correctSpawnArea);
        while (Physics.BoxCast(newPosition, maxBox, Vector3.zero, out RaycastHit _))
        {
            newPosition = Helper.RandomPointInBox(spawnCenter, correctSpawnArea);
        }
        transform.position = newPosition;
        GetComponent<MeshRenderer>().material.color = color;
    }
}