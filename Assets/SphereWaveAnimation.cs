using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereWaveAnimation : MonoBehaviour
{
    [SerializeField] GameObject sphere;

    [SerializeField] int rows = 10;
    [SerializeField] int cols = 10;

    [SerializeField] float size = 0.1f;
    [SerializeField] float spacing = 1f;
    [SerializeField] float amplitude = 0.3f;
    [SerializeField] float yOffset = -1f;
    [SerializeField] float speed = 500f;

    GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();

    }

    void CreateGrid()
    {
        grid = new GameObject[rows, cols];
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                pos.x = i * spacing;
                pos.z = j * spacing;
                GameObject s = Instantiate(sphere, Vector3.zero, Quaternion.identity, transform);
                s.transform.localPosition = pos;
                s.transform.localScale = Vector3.one * size;

                grid[i, j] = s;

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 pos = grid[i, j].transform.position;
                pos.y = Mathf.Sin(Time.time * Time.fixedDeltaTime * speed + (pos.z)) * amplitude + yOffset;

                grid[i, j].transform.position = pos;
                grid[i, j].transform.localScale = Vector3.one * size;
            }
        }
    }
}
