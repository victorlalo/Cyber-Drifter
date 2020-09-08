using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChunk : MonoBehaviour
{
    [SerializeField] GameObject[] SpawnableObjects;
    List<GameObject> SpawnedObjects = new List<GameObject>();
    [SerializeField] GameObject trackModel;

    [SerializeField] GameObject startPos;
    [SerializeField] GameObject endPos;

    float zRange;
    //[SerializeField] GameObject RampPrefab;
    //[SerializeField] GameObject Barrier;
    //[SerializeField] GameObject[] Pickups;

    BoxCollider platformCollider;
    Vector2 platformDims;
    float maxZPos;
    float platformWidthMin, platformWidthMax;

    public float MaxZPos
    {
        get { return maxZPos; }
    }

    void Awake()
    {
        platformCollider = trackModel.GetComponent<BoxCollider>();
        maxZPos = platformCollider.bounds.max.z;
        platformWidthMin = platformCollider.bounds.min.x;
        platformWidthMax = platformCollider.bounds.max.x;

        zRange = endPos.transform.position.z - startPos.transform.position.z;
    }

    //private void OnEnable()
    //{ 
    //    PopulatePlatform();
    //}

    void Update()
    {
        
    }

    public void PopulatePlatform()
    {
        if (SpawnedObjects.Count > 0)
            ClearPlatform();

        int numObjects = Random.Range(2, 5);
        maxZPos = platformCollider.bounds.max.z;
        zRange = endPos.transform.position.z - startPos.transform.position.z;
        float zPos = 0;

        for (int i = 0; i < numObjects; i++)
        {
            zPos = transform.position.z + zRange * (i / numObjects) + Random.Range(-zRange / 10f, zRange / 10f);
            GameObject prefab = SpawnableObjects[Random.Range(0, SpawnableObjects.Length)];
            Vector3 spawnPos = new Vector3(Random.Range(platformWidthMin * .8f, platformWidthMax * .8f), 0 , zPos);
            GameObject spawnInstance = Instantiate(prefab, spawnPos, Quaternion.identity, gameObject.transform); // <---- KEEP PARENT SCALE AT ONE AND MAKE ALL SPAWNED OBJECTS CHILDREN
            spawnInstance.transform.rotation = Quaternion.identity;

            SpawnedObjects.Add(spawnInstance);
        }
    }

    public void ClearPlatform()
    {
        for (int i = 0; i < SpawnedObjects.Count; i++)
        {
            Destroy(SpawnedObjects[i]);
        }

        
    }
}
