using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    Grid wGrid;
	// Use this for initialization
	void Start () {
       
        wGrid = GameObject.Find("Grid").GetComponent<Grid>();
        //Vector2 dimensions = wGrid.getDimensions();
        //for (int i = 0; i < dimensions.x; i++)
        {
          //  for (int j = 0; j < dimensions.y; j++)
            {
            //    Instantiate(tile, new Vector3(0.25f*i, 0.01f, 0.25f*j), Quaternion.AngleAxis(90, Vector3.right), transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        Texture2D t = BuildTexture();
        Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 8, 0, SpriteMeshType.Tight, Vector4.zero);
        
        this.GetComponent<SpriteRenderer>().sprite = s;
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
       
    }
    
    Texture2D BuildTexture()
    {
        Texture2D t = new Texture2D(200, 160, TextureFormat.ARGB32, false);
        Vector2 dimensions = wGrid.getDimensions();
        for (int i = 0; i < 2*dimensions.x; i+= 2)
        {
            for (int j = 0; j<2*dimensions.y;j+=2)
            {
                Grid.gTile tile = wGrid.grid[i/2, j/2];
                if (tile.canWalk)
                {
                    
                    t.SetPixel(i, j, new Color(0.0f, 1.0f, 0.0f, 1f));
                    t.SetPixel(i+1, j, new Color(0.0f, 1.0f, 0.0f, 1f));
                    t.SetPixel(i, j+1, new Color(0.0f, 1.0f, 0.0f, 1f));
                    t.SetPixel(i+1, j+1, new Color(0.0f, 1.0f, 0.0f, 1f));
                }
                else
                {
                    t.SetPixel(i, j, new Color(1.0f, 0.0f, 0.0f, 1f));
                    t.SetPixel(i+1, j, new Color(1.0f, 0.0f, 0.0f, 1f));
                    t.SetPixel(i, j+1, new Color(1.0f, 0.0f, 0.0f, 1f));
                    t.SetPixel(i+1, j+1, new Color(1.0f, 0.0f, 0.0f, 1f));
                }
            }
        }
        t.Apply();
        return t;
    }
}
