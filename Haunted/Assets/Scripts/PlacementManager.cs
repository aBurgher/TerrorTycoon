using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public enum State { Select, Place, Edit };
    public State currentState;
    public GameObject currentObject;
    Color c, oc;
    // Use this for initialization
    void Start()
    {
        currentState = State.Select;
    }
    void setTransparent()
    {
        foreach (Transform child in GameObject.Find("Grid").transform)
        {
            Color c = child.GetComponent<Renderer>().material.color;
            c.a = 0.5f;
            child.GetComponent<Renderer>().material.color = c;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Select)
        {
            if (currentObject != null)
            {
                currentState = State.Place;
                currentObject.GetComponent<ObjectController>().placed = false;
                c = currentObject.GetComponent<Renderer>().material.color;
                oc = c;
                setTransparent();
                GameObject.Find("Grid").GetComponent<Grid>().visible(true);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 100);
                if (hit.collider != null)
                {
                    currentObject = hit.transform.gameObject;
                    currentState = State.Edit;
                    currentObject.GetComponent<ObjectController>().placed = false;
                    c = currentObject.GetComponent<Renderer>().material.color;
                    oc = c;
                    setTransparent();
                    GameObject.Find("Grid").GetComponent<Grid>().visible(true);
                }



            }
        }
        else
        {

            c.a = 0.5f;
            currentObject.GetComponent<Renderer>().material.color = c;
            Plane plane = new Plane(Vector3.up, 0);
            float dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out dist))
            {
                Vector3 vec = ray.GetPoint(dist);
                vec = new Vector3(Mathf.Round(vec.x * 4) / 4, 0, Mathf.Round(vec.z * 4) / 4);
                currentObject.transform.position = vec;
                GameObject.Find("Grid").GetComponent<Grid>().update = true;
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
            if (Input.GetMouseButtonDown(0) && currentState == State.Edit)
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
                GameObject.Find("Grid").GetComponent<Grid>().visible(false);
                GameObject.Find("Grid").GetComponent<Grid>().updatePath = true;
                GameObject.Find("Grid").GetComponent<Grid>().update = true;
                foreach (Transform child in GameObject.Find("Grid").transform)
                {
                    Color b = child.GetComponent<Renderer>().material.color;
                    b.a = 1.0f;
                    child.GetComponent<Renderer>().material.color = b;
                }
                
            }
            if (Input.GetMouseButtonDown(0) && currentState == State.Place)
            {
                int canPlace = GameObject.Find("Grid").GetComponent<Grid>().canPlace(currentObject);
                if (canPlace == 0)
                    return;

                if (canPlace == -1)
                {
                    foreach (Transform child in GameObject.Find("Grid").transform)
                    {
                        Color b = child.GetComponent<Renderer>().material.color;
                        b.a = 1.0f;
                        child.GetComponent<Renderer>().material.color = b;
                    }
                    Destroy(currentObject);
                    currentState = State.Select;
                    GameObject.Find("Grid").GetComponent<Grid>().visible(false);
                    return;
                }
                GameObject obj = Instantiate(currentObject, currentObject.transform.position, currentObject.transform.rotation, GameObject.Find("Grid").transform);
                obj.GetComponent<ObjectController>().placed = true;
                obj.GetComponent<Renderer>().material.color = oc;
                setTransparent();
                GameObject.Find("Grid").GetComponent<Grid>().updatePath = true;
                GameObject.Find("Grid").GetComponent<Grid>().update = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                currentObject.transform.Rotate(new Vector3(0, 90, 0));
            }

        }

        }

    }
