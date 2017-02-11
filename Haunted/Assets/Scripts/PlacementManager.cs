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
                }
            }
        }
        else if (currentState == State.Place)
        {

        }
        else if (currentState == State.Edit)
        {
            Plane plane = new Plane(Vector3.up, 0);
            float dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(plane.Raycast(ray, out dist))
            {
                Vector3 vec = ray.GetPoint(dist);
                vec = new Vector3(Mathf.Round(vec.x * 4) / 4, 0, Mathf.Round(vec.z * 4) / 4);
                //vec = new Vector3(Mathf.Round(vec.x)+0.5f, 0, Mathf.Round(vec.z)+0.5f);
                currentObject.transform.position = vec;
            }
            if (Input.GetMouseButtonUp(0))
            {
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
