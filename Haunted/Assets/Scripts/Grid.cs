using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool update;
 
    public Vector2 Dimensions;
    float gridScale = 4;
    Texture2D currentTex, baseTex;

    public List<Vector2> Path;
    //Strucutre used for storing world data.
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
    void Start()
    {
        //Initializes the grid
        grid = new gTile[100, 80];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                grid[i, j] = new gTile(true, false);
            }
        }
        //initializes the textures
        baseTex = new Texture2D(200, 160, TextureFormat.ARGB32, false);
        currentTex = new Texture2D(200, 160, TextureFormat.ARGB32, false);
        Color[] Colors = baseTex.GetPixels();
        Color C = new Color(0.161f, .407f, 0f);
        for (int i = 0; i < Colors.Length; i++)
            Colors[i] = C;
        baseTex.SetPixels(Colors);
        baseTex.Apply();
        update = true;

        visible(false);
        Dimensions = new Vector2(grid.GetLength(0), grid.GetLength(1));
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (update)
        {
            //clears and updates the grid
            clearGrid();
            updateGrid();
         
            //Updates the sprite that displays the grid. 
            Sprite s = Sprite.Create(currentTex, new Rect(0, 0, currentTex.width, currentTex.height), Vector2.zero, 8, 0, SpriteMeshType.Tight, Vector4.zero);
            GameObject.Find("Tiles").GetComponent<SpriteRenderer>().sprite = s;
            GameObject.Find("Tiles").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            update = false;
        
        }
        //Texture2D t = BuildTexture();

    }

    //converts world position to position on the grid. 
    Vector2 convertCoordinates(Vector3 vec)
    {
        Vector2 gridPos;
        gridPos = new Vector2(Mathf.Round(gridScale * vec.x), Mathf.Round(gridScale * vec.z));
        return gridPos;
    }
    //function ot update teh grid
    void updateGrid()
    {
        //resets the grid texture to be green. 
        currentTex.SetPixels(baseTex.GetPixels());
        currentTex.Apply();
        //Iterates through the grid objects 
        foreach (Transform child in transform)
        {
            //skips the object if it isn't active
            if (!child.gameObject.activeSelf)
                continue;
            //gets the objects dimensions and grid coordinates
            float x, y;
            Vector2 pos = convertCoordinates(child.transform.position);
            x = child.GetComponent<ObjectController>().length;
            y = child.GetComponent<ObjectController>().width;

            //checks the objects rotation and updates the grid and texture pixels based on it. 
            if ((int)child.transform.rotation.eulerAngles.y == 0)
            {
                if (pos.x > Dimensions.x - 1 - (y - 1) || pos.x < 0 || pos.y > Dimensions.y - 1 - (x - 1) || pos.y < 0)
                    continue;
                for (int i = (int)pos.x; i < (int)pos.x + (int)y; i++)
                {
                    for (int j = (int)pos.y; j < (int)pos.y + (int)x; j++)
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
            else if ((int)child.transform.rotation.eulerAngles.y == 90)
            {
                if (pos.x > Dimensions.x - 1 - (x - 1) || pos.x < 0 || pos.y > 80 || pos.y < y)
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
                if (pos.x > Dimensions.x || pos.x < 0 + y || pos.y > Dimensions.y || pos.y < 0 + x)
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
                if (pos.x > Dimensions.x || pos.x < (x) || pos.y > Dimensions.y - y || pos.y < 0)
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
            //applies the texture
            currentTex.Apply();

        }


    }
    //clears the grid back to empty
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

    //checks if an object is placable, returns a 1 for yes, 0 for no, or a -1 for out of bounds. 
    public int canPlace(GameObject obj)
    {
        Vector2 pos = convertCoordinates(obj.transform.position);
        float x = obj.GetComponent<ObjectController>().length;
        float y = obj.GetComponent<ObjectController>().width;
        if ((int)obj.transform.rotation.eulerAngles.y == 0)
        {
            if (pos.x > Dimensions.x - 1 - (y - 1) || pos.x < 0 || pos.y > Dimensions.y - 1 - (x - 1) || pos.y < 0)
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
            if (pos.x > Dimensions.x - 1 - (x - 1) || pos.x < 0 || pos.y > Dimensions.y || pos.y < y)
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
    //sets shows/hides the grid. 
    public void visible(bool visible)
    {
        GameObject.Find("Tiles").GetComponent<SpriteRenderer>().enabled = visible;
    }

  

}
