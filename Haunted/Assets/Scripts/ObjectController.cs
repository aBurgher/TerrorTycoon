using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float length = 1, width = 1;
    public bool walkable = false, placed = true;
    public Vector2 EndPoint;
    Vector3 previousPosition;
	// Use this for initialization
	void Start () {
        this.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Objects");
        previousPosition = transform.position;
        CalculatePoints();
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position != previousPosition && placed)
        {
            CalculatePoints();
            previousPosition = transform.position;
        }
	}
    void CalculatePoints()
    {
        Vector2 d = new Vector2(length, width);
        float rot = transform.rotation.eulerAngles.y;
        EndPoint = new Vector2(transform.position.x + (Mathf.Cos(-rot / 180 * Mathf.PI) * 0.25f * d.y), transform.position.z + (Mathf.Sin(-rot / 180 * Mathf.PI) * d.y * 0.25f));
        EndPoint = new Vector2(Mathf.Round(EndPoint.x*4)/4,Mathf.Round(EndPoint.y*4)/4);
    }
}
