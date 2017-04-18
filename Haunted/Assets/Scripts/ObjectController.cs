using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float length = 1, width = 1, Range = 1, Power = 1;
    public bool walkable = false, placed = true, Spooky = true, Scary = false;
    bool triggered = false , ready = true; 
    public Animation Trigger, Reset;
    public Vector2 EndPoint;
    Vector3 previousPosition, Center;
	// Use this for initialization
	void Start () {
       

        previousPosition = transform.position;
        CalculatePoints();
        if (this.gameObject.tag != "Wall")
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
      
        }
    }
	
	// Update is called once per frame
	void Update () {
        //used for storing previous position and end points. 
        if (transform.position != previousPosition && placed)
        {
            CalculatePoints();
            previousPosition = transform.position;
        }
        if (Spooky && !Scary)
        {
            if(!triggered)
                CheckRange();
        }
        else
        {

        }
	}
    void CheckRange()
    {
        GameObject[] Actors = GameObject.FindGameObjectsWithTag("Actor");
        foreach (GameObject actor in Actors)
        {
            float distance = Vector3.Distance(Center, actor.transform.position);
            if (distance < Range)
            {
                bool inRange = false;
                Ray ray = new Ray(actor.transform.position, Center);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                if (hit.collider != null && hit.collider.gameObject.tag == "Prop")
                {
                    inRange = true;
                }
                if (inRange)
                {
                    triggered = true;
                    this.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }
    void CalculatePoints()
    {
        Vector2 d = new Vector2(length, width);
        float rot = transform.rotation.eulerAngles.y;
        EndPoint = new Vector2(transform.position.x + (Mathf.Cos(-rot / 180 * Mathf.PI) * 0.25f * d.y), transform.position.z + (Mathf.Sin(-rot / 180 * Mathf.PI) * d.y * 0.25f));
        EndPoint = new Vector2(Mathf.Round(EndPoint.x*4)/4,Mathf.Round(EndPoint.y*4)/4);
        Center = gameObject.GetComponent<Renderer>().bounds.center;
    }
}
