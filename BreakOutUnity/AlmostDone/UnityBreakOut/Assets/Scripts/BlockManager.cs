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

    private List<GameObject> Bricks;
    private List<GameObject> bricksToRemove;

    void Start()
    {
        this.LoadLevel();   
    }

    public void LoadLevel()
    {
        Bricks = new List<GameObject>();
        bricksToRemove = new List<GameObject>();
        for (float y = starty; y < gridY; y = y + padding)
        {
            for (float x = startX; x < gridX; x = x + padding)
            {
                Vector3 pos = new Vector3(x + padding, y + padding, 0);
                Bricks.Add((GameObject)Instantiate(prefab, pos, Quaternion.identity));
            }
        }
    }

    //Win
    void Win()
    {
        throw new System.Exception("Winner Winner Chicken Dinner!");
    }

    void Update()
    {
        bricksToRemove.Clear();
        foreach (var item in Bricks)
        {
            if (item == null)
            {
                bricksToRemove.Add(item);
            }
        }

        foreach (var item in bricksToRemove)
        {
            Bricks.Remove(item);
            ScoreManager.Score += 1;
        }

        if (Bricks.Count == 0)
        {
            this.Win();
        }
    }
}