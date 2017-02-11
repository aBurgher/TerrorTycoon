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

                if (child.transform.rotation.eulerAngles.y == 270)
                {
                    if (child.position.x > 25 - (0.25 * (y-1)) || child.position.x < 0 || child.position.z > 20 - (0.25*(x - 1)) || child.position.z < 0)
                        continue;
                    for (int i = (int)(Mathf.Floor(4 * child.position.x)); i < (int)(Mathf.Floor(4 * child.position.x)) + (int)y; i++)
                    {
                        for (int j = (int)(Mathf.Floor(4 * child.position.z)); j < (int)(Mathf.Floor(4 * child.position.z)) + (int)x; j++)
                        {
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        }
                    }
                }
                else if (child.transform.rotation.eulerAngles.y == 0)
                {
                    for (int i = (int)(Mathf.Floor(4 * child.position.x)); i > (int)(Mathf.Floor(4 * child.position.x)) - (int)x; i--)
                    {
                        for (int j = (int)(Mathf.Floor(4 * child.position.z))-1; j > (int)(Mathf.Floor(4 * child.position.z)) - (int)y-1; j--)
                        {
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        }
                    }
                }
                else if (child.transform.rotation.eulerAngles.y == 90)
                {
                    for (int i = (int)(Mathf.Floor(4 * child.position.x))-1; i > (int)(Mathf.Floor(4 * child.position.x))-1 - (int)y; i--)
                    {
                        for (int j = (int)(Mathf.Floor(4 * child.position.z))-1; j > (int)(Mathf.Floor(4 * child.position.z))-1 - (int)x; j--)
                        {
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        }
                    }
                }
                else if (child.transform.rotation.eulerAngles.y == 180)
                {
                    for (int i = (int)(Mathf.Floor(4 * child.position.x))-1; i < (int)(Mathf.Floor(4 * child.position.x)) + (int)x-1; i++)
                    {
                        for (int j = (int)(Mathf.Floor(4 * child.position.z)); j < (int)(Mathf.Floor(4 * child.position.z)) + (int)y; j++)
                        {
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        }
                    }
                }


            }
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
        for (int i = 0; i < 2 * dimensions.x; i += 2)
        {
            for (int j = 0; j < 2 * dimensions.y; j += 2)
            {
                Grid.gTile tile = grid[i / 2, j / 2];
                if (tile.canWalk)
                {

                    t.SetPixel(i, j, new Color(0.0f, 1.0f, 0.0f, 1f));
                    t.SetPixel(i + 1, j, new Color(0.0f, 1.0f, 0.0f, 1f));
                    t.SetPixel(i, j + 1, new Color(0.0f, 1.0f, 0.0f, 1f));
                    t.SetPixel(i + 1, j + 1, new Color(0.0f, 1.0f, 0.0f, 1f));
                }
                else
                {
                    t.SetPixel(i, j, new Color(1.0f, 0.0f, 0.0f, 1f));
                    t.SetPixel(i + 1, j, new Color(1.0f, 0.0f, 0.0f, 1f));
                    t.SetPixel(i, j + 1, new Color(1.0f, 0.0f, 0.0f, 1f));
                    t.SetPixel(i + 1, j + 1, new Color(1.0f, 0.0f, 0.0f, 1f));
                }
            }
        }
        t.Apply();
        return t;
    }
}
