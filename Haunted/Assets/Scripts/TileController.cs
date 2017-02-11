using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    Grid wGrid;
    Renderer rend;
    Grid.gTile t;
	// Use this for initialization
	void Start () {
        wGrid = GameObject.Find("Grid").GetComponent<Grid>();
        rend = this.GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
        t = wGrid.grid[(int)Mathf.Round(transform.position.x * 4), (int)Mathf.Round(transform.position.z * 4)];
        if (rend != null)
        {
            if (t.canPlace)
            {
                //if (rend.material.color != null)
                    rend.material.color = new Color(0f, 1f, 0f, 0.5f);
            }
            else
            {
                //if (rend.material.color != null)
                    rend.material.color = new Color(1f, 0f, 0f, 0.5f);
            }
        }
        else
        {
            rend = GetComponent<SpriteRenderer>();
        }
        
	}
}
