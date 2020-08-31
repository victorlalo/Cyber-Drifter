using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSpawner : MonoBehaviour
{
    int initialSpawnNum = 10;
    [SerializeField] GameObject platformPrefab;
    float straightPlatformLength;

    float furthestEdge = 0f;
    int lastPlatformIndex = 0;

    List<GameObject> platformPool = new List<GameObject>();

    private void Awake()
    {
        EndGate.OnGatePassed += SpawnNewPlatform;

        GameObject firstPlatform = Instantiate(platformPrefab, new Vector3(0, transform.position.y, 0), Quaternion.identity, gameObject.transform);
        straightPlatformLength = firstPlatform.GetComponent<RoadChunk>().MaxZPos;
        firstPlatform.GetComponent<RoadChunk>().ClearPlatform();

        platformPool.Add(firstPlatform);

        Initialize();
    }

    void Initialize()
    {
        for (int i = 1; i < initialSpawnNum; i++)
        {
            furthestEdge = i * straightPlatformLength + straightPlatformLength;
            GameObject instance = Instantiate(platformPrefab, new Vector3(0,transform.position.y, furthestEdge), Quaternion.identity, gameObject.transform);
            
            platformPool.Add(instance);
        }
        furthestEdge += straightPlatformLength;
    }

    public void SpawnNewPlatform()
    {
        GameObject lastPlatform = platformPool[lastPlatformIndex];
        lastPlatform.transform.position = new Vector3(0, transform.position.y, furthestEdge);
        lastPlatform.GetComponent<RoadChunk>().Repopulate();

        furthestEdge += straightPlatformLength;
        lastPlatformIndex = (lastPlatformIndex + 1) % initialSpawnNum;

        Debug.Log("MOVED PLATFORM TO END OF LINE");

    }

    public void DespawnPlatform()
    {

    }
}
