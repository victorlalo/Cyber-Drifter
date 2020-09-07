using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChunk : MonoBehaviour
{
    [SerializeField] GameObject[] SpawnableObjects;
    List<GameObject> SpawnedObjects = new List<GameObject>();
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
        platformCollider = GetComponent<BoxCollider>();
        maxZPos = platformCollider.bounds.max.z;
        platformWidthMin = platformCollider.bounds.min.x;
        platformWidthMax = platformCollider.bounds.max.x;
    }

    private void OnEnable()
    { 
        PopulatePlatform();
    }

    void Update()
    {
        
    }

    void PopulatePlatform()
    {
        int numObjects = Random.Range(3, 7);
        maxZPos = platformCollider.bounds.max.z;
        float zPos = 0;

        for (int i = 0; i < numObjects; i++)
        {
            zPos = transform.position.z + 0.5f * maxZPos/ numObjects * i; //  + Random.Range(-25, 25);
            GameObject prefab = SpawnableObjects[Random.Range(0, SpawnableObjects.Length)];
            Vector3 spawnPos = new Vector3(Random.Range(platformWidthMin * .8f, platformWidthMax * .8f), 0 , zPos);
            GameObject spawnInstance = Instantiate(prefab, spawnPos, Quaternion.identity); //, gameObject.transform);  <---- KEEP PARENT SCALE AT ONE AND MAKE ALL SPAWNED OBJECTS CHILDREN

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

    public void Repopulate()
    {
        ClearPlatform();
        PopulatePlatform();
    }
}
