using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public bool update;
    public struct gTile
    {
        public bool canPlace;
        public bool canWalk;

        public gTile(bool a, bool b)
        {
            canPlace = a;
            canWalk = b;
        }
    }
    public gTile[,] grid;
    // Use this for initialization
    void Start() {
        grid = new gTile[100, 80];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                grid[i, j] = new gTile(true, false);
            }
        }
        update = true;
    }

    // Update is called once per frame
    void Update() {
        if (update)
        {
            clearGrid();
            foreach (Transform child in transform)
            {
                float x, y;
                x = child.GetComponent<ObjectController>().length;
                y = child.GetComponent<ObjectController>().width;
                for (int i = (int)(Mathf.Floor(4*child.position.x)); i < (int)(Mathf.Floor(4 * child.position.x)) + (int)x; i++)
                {
                    for (int j = (int)(Mathf.Floor(4 * child.position.z)); j < (int)(Mathf.Floor(4 * child.position.z)) + (int)y; j++)
                    {
                        grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                    }
                }
            }
        }
    }
    public Vector2 getDimensions()
    {
        return new Vector2(grid.GetLength(0), grid.GetLength(1));
    }
    void clearGrid()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                grid[i, j] = new gTile(true, true);
            }
        }
        
        
    }
}
