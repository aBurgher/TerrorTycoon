using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour {
    public enum State {Select, Place, Edit };
    public State currentState;
    public GameObject currentObject;
	// Use this for initialization
	void Start () {
        currentState = State.Select;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentState == State.Select)
        {
            if (currentObject != null)
                currentState = State.Place;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray,out hit, 100);
                if (hit.collider != null)
                {
                    currentObject = hit.transform.gameObject;
                    currentState = State.Edit;
                    currentObject.GetComponent<ObjectController>().placed = false;
                }
            }
        }
        else if (currentState == State.Place)
        {
            Color c = currentObject.GetComponent<Renderer>().material.color;
            c.a = 0.5f;
            currentObject.GetComponent<Renderer>().material.color = c;
            Plane plane = new Plane(Vector3.up, 0);
            float dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out dist))
            {
                Vector3 vec = ray.GetPoint(dist);
                vec = new Vector3(Mathf.Round(vec.x * 4) / 4, 0, Mathf.Round(vec.z * 4) / 4);
                //vec = new Vector3(Mathf.Round(vec.x)+0.5f, 0, Mathf.Round(vec.z)+0.5f);
                currentObject.transform.position = vec;
            }
            if (Input.GetMouseButtonDown(0))
            {
                c.a = 1f;
                currentObject.GetComponent<Renderer>().material.color = c;
                currentObject = null;
                currentState = State.Select;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                currentObject.transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else if (currentState == State.Edit)
        {
            Color c = currentObject.GetComponent<Renderer>().material.color;
            Color oc = new Color(c.r, c.g, c.b, c.a);
            c.a = 0.5f;
            currentObject.GetComponent<Renderer>().material.color = c;
            Plane plane = new Plane(Vector3.up, 0);
            float dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(plane.Raycast(ray, out dist))
            {
                Vector3 vec = ray.GetPoint(dist);
                vec = new Vector3(Mathf.Round(vec.x * 4) / 4, 0, Mathf.Round(vec.z * 4) / 4);
                currentObject.transform.position = vec;
                int canPlace = GameObject.Find("Grid").GetComponent<Grid>().canPlace(currentObject);
                if (canPlace != 1)
                {
                    c = Color.red;
                }
                else
                    c = Color.green;
                c.a = 0.5f;
                currentObject.GetComponent<Renderer>().material.color = c;

            }
            if (Input.GetMouseButtonDown(0))
            {
                int canPlace = GameObject.Find("Grid").GetComponent<Grid>().canPlace(currentObject);
                if (canPlace == 0)
                    return;
                else if (canPlace == -1)
                {
                    Destroy(currentObject);
                    currentState = State.Place;
                }
                
                currentObject.GetComponent<Renderer>().material.color = oc;
                currentObject.GetComponent<ObjectController>().placed = true;
                currentObject = null;
                currentState = State.Select;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                currentObject.transform.Rotate(new Vector3(0, 90, 0));
            }

        }
        
    }

}
