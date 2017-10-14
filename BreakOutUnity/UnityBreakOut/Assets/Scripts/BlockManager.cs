using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{

    public GameObject prefab;
    public float gridX = 5f;
    public float gridY = 5f;
    public float padding = .05f;

    public float startX = -1;
    public float starty = 1;

    void Start()
    {
        for (float y = starty; y < gridY; y = y + padding)
        {
            for (float x = startX; x < gridX; x = x + padding)
            {
                Vector3 pos = new Vector3(x +padding, y + padding, 0 );
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
}