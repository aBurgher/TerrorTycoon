using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float length = 1, width = 1;
    public bool walkable = false, placed = true;
	// Use this for initialization
	void Start () {
        this.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Objects");
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
