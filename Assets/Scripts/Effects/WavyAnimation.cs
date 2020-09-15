using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WavyAnimation : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    Vector3[] vertexArray;
    int vertexLength;

    // Start is called before the first frame update
    void Start()
    {
        vertexArray = meshFilter.mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < vertexArray.Length; i++)
        {
            Vector3 v = vertexArray[i];
            v.y = Mathf.Sin((v.x + v.z) * Time.time * Time.deltaTime
                );

            vertexArray[i] = v;
        }

        //Vector3 v = vertexArray[1];
        //v.y = Mathf.Sin((v.x + v.z) * Time.time);
        //vertexArray[1] = v;

        meshFilter.mesh.vertices = vertexArray;
    }
}
