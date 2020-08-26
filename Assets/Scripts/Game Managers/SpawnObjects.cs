using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    public int numObjects = 100;

    void Start()
    {
        SpawnField();
    }

    void SpawnField()
    {

        for (int i = 0; i < numObjects; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-50, 50), 4, Random.Range(-2000, 2000));
            Instantiate(objectPrefab, randPos, Quaternion.identity);
        }
    }
}
