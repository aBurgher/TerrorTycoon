using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public bool update { get; set; }
    public bool updatePath { get; set; }
    public Vector2 Dimensions;
    float gridScale = 4;
    Texture2D currentTex, baseTex;
    PathFinding PathFinder;
    public List<Vector2> Path;

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
        PathFinder = this.GetComponent<PathFinding>();
        grid = new gTile[100, 80];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                grid[i, j] = new gTile(true, false);
            }
        }
        baseTex = new Texture2D(200, 160, TextureFormat.ARGB32, false);
        currentTex = new Texture2D(200, 160, TextureFormat.ARGB32, false);
        Color[] Colors = baseTex.GetPixels();
        for (int i = 0; i < Colors.Length; i++)
            Colors[i] = Color.green;
        baseTex.SetPixels(Colors);
        baseTex.Apply();
        update = true;
        updatePath = true;
        visible(false);
        Dimensions = new Vector2(grid.GetLength(0), grid.GetLength(1));
    }

    // Update is called once per frame
    void Update() {
        if (update)
        {
            clearGrid();
            updateGrid();
            if (updatePath)
            {
                bool[,] convertedGrid = convertGrid();
                Path = PathFinder.Path(Vector2.zero, Dimensions / 2, new Vector2(convertedGrid.GetLength(0), convertedGrid.GetLength(1)), convertedGrid);
                updatePath = false;
            }
            drawPath();
            Sprite s = Sprite.Create(currentTex, new Rect(0, 0, currentTex.width, currentTex.height), Vector2.zero, 8, 0, SpriteMeshType.Tight, Vector4.zero);
            GameObject.Find("Tiles").GetComponent<SpriteRenderer>().sprite = s;
            GameObject.Find("Tiles").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            update = false;
        }
        //Texture2D t = BuildTexture();
        
    }

  
    Vector2 convertCoordinates(Vector3 vec)
    {
        Vector2 gridPos;
        gridPos = new Vector2(Mathf.Round(gridScale*vec.x), Mathf.Round(gridScale * vec.z));
        return gridPos;
    }

    void updateGrid()
    {
        currentTex.SetPixels(baseTex.GetPixels());
        currentTex.Apply();
        foreach (Transform child in transform)
        {
            float x, y;
            Vector2 pos = convertCoordinates(child.transform.position);
            x = child.GetComponent<ObjectController>().length;
            y = child.GetComponent<ObjectController>().width;
           
            if ((int)child.transform.rotation.eulerAngles.y == 0)
            {
                if (pos.x > Dimensions.x-1 - (y-1) || pos.x < 0 || pos.y > Dimensions.y-1 -(x - 1) || pos.y < 0)
                    continue;
                for (int i = (int)pos.x; i < (int)pos.x + (int)y; i++)
                {
                    for (int j = (int)pos.y; j < (int)pos.y + (int)x; j++)
                    {
                        if(child.GetComponent<ObjectController>().placed)
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        currentTex.SetPixel(2*i, 2*j, Color.red);
                        currentTex.SetPixel(2*i + 1, 2*j, Color.red);
                        currentTex.SetPixel(2*i, 2*j + 1, Color.red);
                        currentTex.SetPixel(2*i + 1, 2*j + 1, Color.red);
                    }
                }
            }
            else if ((int)child.transform.rotation.eulerAngles.y == 90)
            {
                if (pos.x > Dimensions.x-1 - (x - 1) || pos.x < 0 || pos.y > 80 || pos.y < y)
                    continue;
                for (int i = (int)pos.x; i < (int)pos.x + (int)x; i++)
                {
                    for (int j = (int)pos.y - 1; j > (int)pos.y - (int)y - 1; j--)
                    {
                        if (child.GetComponent<ObjectController>().placed)
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        currentTex.SetPixel(2 * i, 2 * j, Color.red);
                        currentTex.SetPixel(2 * i - 1, 2 * j, Color.red);
                        currentTex.SetPixel(2 * i, 2 * j - 1, Color.red);
                        currentTex.SetPixel(2 * i - 1, 2 * j - 1, Color.red);

                    }
                }
            }
            else if ((int)child.transform.rotation.eulerAngles.y == 180)
            {
                if (pos.x > Dimensions.x  || pos.x < 0+y|| pos.y > Dimensions.y || pos.y < 0 +x)
                    continue;
                for (int i = (int)pos.x - 1; i > (int)pos.x - 1 - (int)y; i--)
                {
                    for (int j = (int)pos.y - 1; j > (int)pos.y - 1 - (int)x; j--)
                    {
                        if (child.GetComponent<ObjectController>().placed)
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        currentTex.SetPixel(2 * i, 2 * j, Color.red);
                        currentTex.SetPixel(2 * i + 1, 2 * j, Color.red);
                        currentTex.SetPixel(2 * i, 2 * j + 1, Color.red);
                        currentTex.SetPixel(2 * i + 1, 2 * j + 1, Color.red);
                    }
                }
            }
            else if ((int)child.transform.rotation.eulerAngles.y == 270)
            {
                if (pos.x > Dimensions.x  || pos.x < (x) || pos.y > Dimensions.y-y || pos.y < 0)
                    continue;
                for (int i = (int)pos.x - (int)x; i < pos.x; i++)
                {
                    for (int j = (int)pos.y; j < (int)pos.y + (int)y; j++)
                    {
                        if (child.GetComponent<ObjectController>().placed)
                            grid[i, j] = new gTile(false, child.GetComponent<ObjectController>().walkable);
                        currentTex.SetPixel(2 * i, 2 * j, Color.red);
                        currentTex.SetPixel(2 * i + 1, 2 * j, Color.red);
                        currentTex.SetPixel(2 * i, 2 * j + 1, Color.red);
                        currentTex.SetPixel(2 * i + 1, 2 * j + 1, Color.red);
                    }
                }
            }
            currentTex.Apply();

        }


    }

    void clearGrid()
    {
        for (int i = 0; i < Dimensions.x; i++)
        {
            for (int j = 0; j < Dimensions.y; j++)
            {
                grid[i, j] = new gTile(true, true);
            }
        }
        
        
    }

    public int canPlace(GameObject obj)
    {
        Vector2 pos = convertCoordinates(obj.transform.position);
        float x = obj.GetComponent<ObjectController>().length;
        float y = obj.GetComponent<ObjectController>().width;
        if ((int)obj.transform.rotation.eulerAngles.y == 0)
        {
            if (pos.x > Dimensions.x-1 - (y - 1) || pos.x < 0 || pos.y > Dimensions.y-1 - (x - 1) || pos.y < 0)
                return -1;
            for (int i = (int)pos.x; i < (int)pos.x + (int)y; i++)
            {
                for (int j = (int)pos.y; j < (int)pos.y + (int)x; j++)
                {
                    if (grid[i, j].canPlace == false)
                    {
                        return 0;
                    }
                }
            }
        }
        else if ((int)obj.transform.rotation.eulerAngles.y == 90)
        {
            if (pos.x > Dimensions.x-1 - (x - 1) || pos.x < 0 || pos.y > Dimensions.y || pos.y < y)
                return -1;
            for (int i = (int)pos.x; i < (int)pos.x + (int)x; i++)
            {
                for (int j = (int)pos.y - 1; j > (int)pos.y - (int)y - 1; j--)
                {
                    if (grid[i, j].canPlace == false)
                    {
                        return 0;
                    }
                }
            }
        }
        else if ((int)obj.transform.rotation.eulerAngles.y == 180)
        {
            if (pos.x > Dimensions.x || pos.x < 0 + y || pos.y > Dimensions.y || pos.y < 0 + x)
                return -1;
            for (int i = (int)pos.x - 1; i > (int)pos.x - 1 - (int)y; i--)
            {
                for (int j = (int)pos.y - 1; j > (int)pos.y - 1 - (int)x; j--)
                {
                    if (grid[i, j].canPlace == false)
                    {
                        return 0;
                    }
                }
            }
        }
        else if ((int)obj.transform.rotation.eulerAngles.y == 270)
        {
            if (pos.x > Dimensions.x || pos.x < (x) || pos.y > Dimensions.y - y || pos.y < 0)
                return -1;
            for (int i = (int)pos.x - (int)x; i < pos.x; i++)
            {
                for (int j = (int)pos.y; j < (int)pos.y + (int)y; j++)
                {
                    if (grid[i, j].canPlace == false)
                    {
                        return 0;
                    }
                }
            }
        }

       return 1;
    }
    public void visible(bool visible)
    {
        GameObject.Find("Tiles").GetComponent<SpriteRenderer>().enabled = visible;
    }
    
    void drawPath()
    {
        if (Path == null)
            return;
        foreach (Vector2 vec in Path)
        {
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    currentTex.SetPixel(4 * (int)vec.x+i, 4 * (int)vec.y+j, Color.blue);
                }
            }
        }
        currentTex.Apply();
    }
    bool[,] convertGrid()
    {
        bool[,] newGrid = new bool[(int)Dimensions.x/2, (int)Dimensions.y/2];
        for (int i = 0; i < newGrid.GetLength(0); i++)
        {
            for (int j = 0; j < newGrid.GetLength(1); j++)
            {
                newGrid[i, j] = true;
                if (!grid[2*i, 2*j].canWalk)
                {
                    newGrid[i, j] = false;
                }
                if (!grid[2*i+1, 2*j].canWalk)
                {
                    newGrid[i, j] = false;
                }
                if (!grid[2*i, 2*j+1].canWalk)
                {
                    newGrid[i, j] = false;
                }
                if (!grid[2*i+1, 2*j + 1].canWalk)
                {
                    newGrid[i, j] = false;
                }

            }
        }
        return newGrid;
    }
}
