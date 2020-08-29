using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSpawner : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject rampPrefab;
    List<GameObject> rampPool = new List<GameObject>();

    [SerializeField] float floorWidth = 100f;
    float furthestRampPosZ = 0;
    float zSpacing = 100f;

    float currMaxDistance = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CarController.OnGameOver += ResetRamps;

        for (int i = 1; i < 25; i++)
        {
            currMaxDistance = zSpacing * i + Random.Range(-zSpacing / 4, zSpacing / 4);

            var r = Instantiate(rampPrefab, new Vector3(Random.Range(-floorWidth/3, floorWidth/3), -Random.Range(-1f,-.25f), currMaxDistance), Quaternion.identity, gameObject.transform);
            rampPool.Add(r);

            
            //r.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject r in rampPool)
        {
            if (player.transform.position.z > r.transform.position.z + 10)
            {
                r.transform.position += new Vector3(0, 0, currMaxDistance + zSpacing + Random.Range(-zSpacing / 4, zSpacing / 4));
            }
        }

    }

    // if game over/ respawn, reset ramps to beginning
    // ideally called from Respawn Event
    public void ResetRamps()
    {
        for (int i = 0; i < rampPool.Count; i++)
        {
            currMaxDistance = zSpacing * i + Random.Range(-zSpacing / 4, zSpacing / 4);
            rampPool[i].transform.position = new Vector3(Random.Range(-floorWidth / 3, floorWidth / 3), -Random.Range(-1f, -.25f), currMaxDistance);
        }
    }

    void spawnRamps()
    {
        for (int i = 0; i < 8; i++)
        {
            return;
        }
    }



}
