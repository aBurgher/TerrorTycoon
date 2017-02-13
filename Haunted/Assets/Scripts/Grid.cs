using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public bool update;
    float gridScale = 4;
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
            updateGrid();
        }
        Texture2D t = BuildTexture();
        Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 8, 0, SpriteMeshType.Tight, Vector4.zero);
        GameObject.Find("Tiles").GetComponent<SpriteRenderer>().sprite = s;
        GameObject.Find("Tiles").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public Vector2 getDimensions()
    {
        return new Vector2(grid.GetLength(0), grid.GetLength(1));
    }

    Vector2 convertCoordinates(Vector3 vec)
    {
        Vector2 gridPos;
        gridPos = new Vector2(Mathf.Round(gridScale*vec.x), Mathf.Round(gridScale * vec.z));
        return gridPos;
    }

    void updateGrid()
    {
        foreach (Transform child in transform)
        {
            float x, y;
            Vector2 pos = convertCoordinates(child.transform.position);
            x = child.GetComponent<ObjectController>().length;
            y = child.GetComponent<ObjectController>().width;
            
            if (child.transform.rotation.eulerAngles.y == 0)
            {
                if (pos.x > 99 - (y-1) || pos.x < 0 || pos.y > 79 -(x - 1) || pos.y < 0)
                    continue;
                for (int i = (int)pos.x; i < (int)pos.x + (int)y; i++)
                {
                    for (int j = (int)pos.y; j < (int)pos.y + (int)x; j++)
                    {
                        grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                    }
                }
            }
            else if (child.transform.rotation.eulerAngles.y == 90)
            {
                if (pos.x > 99 - (x - 1) || pos.x < 0 || pos.y > 80 || pos.y < y)
                    continue;
                for (int i = (int)pos.x; i > (int)pos.x - (int)x; i--)
                {
                    for (int j = (int)pos.y - 1; j > (int)pos.y - (int)y - 1; j--)
                    {
                        grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                    }
                }
            }
            else if (child.transform.rotation.eulerAngles.y == 180)
            {
                for (int i = (int)pos.x - 1; i > (int)pos.x - 1 - (int)y; i--)
                {
                    for (int j = (int)pos.y - 1; j > (int)pos.y - 1 - (int)x; j--)
                    {
                        grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                    }
                }
            }
            else if (child.transform.rotation.eulerAngles.y == 270)
            {
                for (int i = (int)pos.x - 1; i < (int)pos.x + (int)x - 1; i++)
                {
                    for (int j = (int)pos.y; j < (int)pos.y + (int)y; j++)
                    {
                        grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                    }
                }
            }


        }


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

    Texture2D BuildTexture()
    {
        Texture2D t = new Texture2D(200, 160, TextureFormat.ARGB32, false);
        Vector2 dimensions = getDimensions();
        Color red = new Color(1.0f, 0.0f, 0.0f, 1f);
        Color green = new Color(0.0f, 1.0f, 0.0f, 1f);
        for (int i = 0; i < 2 * dimensions.x; i += 2)
        {
            for (int j = 0; j < 2 * dimensions.y; j += 2)
            {
                Grid.gTile tile = grid[i / 2, j / 2];
                if (tile.canWalk)
                {

                    t.SetPixel(i, j, green);
                    t.SetPixel(i + 1, j, green);
                    t.SetPixel(i, j + 1, green);
                    t.SetPixel(i + 1, j + 1, green);
                }
                else
                {
                    t.SetPixel(i, j, red);
                    t.SetPixel(i + 1, j, red);
                    t.SetPixel(i, j + 1, red);
                    t.SetPixel(i + 1, j + 1, red);
                }
            }
        }
        t.Apply();
        return t;
    }
}
