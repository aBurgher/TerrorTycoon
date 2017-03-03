using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class PlacementManager : MonoBehaviour
{
    public enum State { Select, Place, Edit };
    public State currentState;
    public GameObject currentObject, gridObj;
    Grid grid;
    Color c, oc;
    UndoManager Undo;
    Vector3 oPos;
    Quaternion oRot;

    // Use this for initialization
    void Start()
    {
        currentState = State.Select;
        gridObj = GameObject.Find("Grid");
        grid = gridObj.GetComponent<Grid>();
        Undo = gridObj.GetComponent<UndoManager>();

    }
    void setTransparent()
    {
        foreach (Transform child in gridObj.transform)
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
                grid.visible(true);
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
                    oPos = currentObject.transform.position;
                    oRot = currentObject.transform.rotation;
                    currentObject.GetComponent<ObjectController>().placed = false;
                    c = currentObject.GetComponent<Renderer>().material.color;
                    oc = c;
                    setTransparent();
                    grid.visible(true);
                }



            }
        }
        else
        {

            snapTo();
            if (Input.GetMouseButtonDown(0) && currentState == State.Edit)
            {
                int canPlace = grid.canPlace(currentObject);
                if (canPlace == 0)
                    return;
                else if (canPlace == -1)
                {
                    Destroy(currentObject);
                    currentState = State.Place;
                }

                currentObject.GetComponent<Renderer>().material.color = oc;
                currentObject.GetComponent<ObjectController>().placed = true;
                Undo.addChange(new UndoManager.change(currentObject, UndoManager.Act.Edit, oPos, oRot));
                currentObject = null;
                currentState = State.Select;
                grid.visible(false);

                grid.update = true;

                foreach (Transform child in gridObj.transform)
                {
                    Color b = child.GetComponent<Renderer>().material.color;
                    b.a = 1.0f;
                    child.GetComponent<Renderer>().material.color = b;
                }

            }
            if (Input.GetMouseButtonDown(0) && currentState == State.Place)
            {
                int canPlace = grid.canPlace(currentObject);
                if (canPlace == 0)
                    return;

                if (canPlace == -1)
                {
                    foreach (Transform child in gridObj.transform)
                    {
                        Color b = child.GetComponent<Renderer>().material.color;
                        b.a = 1.0f;
                        child.GetComponent<Renderer>().material.color = b;
                    }
                    Destroy(currentObject);
                    currentState = State.Select;
                    grid.visible(false);
                    return;
                }
                GameObject obj = Instantiate(currentObject, currentObject.transform.position, currentObject.transform.rotation, gridObj.transform);
                obj.GetComponent<ObjectController>().placed = true;
                obj.GetComponent<Renderer>().material.color = oc;
                setTransparent();

                grid.update = true;

                Undo.addChange(new UndoManager.change(obj, UndoManager.Act.Place));
            }
            else if (Input.GetMouseButtonUp(1))
            {
                currentObject.transform.Rotate(new Vector3(0, 90, 0));
            }


        }

    }

    void snapTo()
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
            if (currentObject.tag == "Prop")
            {
                SnapToWall(vec);
            }
            grid.update = true;
            int canPlace = grid.canPlace(currentObject);
            if (canPlace != 1)
            {
                c = Color.red;
            }
            else
                c = Color.green;
            c.a = 0.5f;
            currentObject.GetComponent<Renderer>().material.color = c;


        }

    }

    void SnapToWall(Vector2 mp)
    {

        if (Input.GetKey(KeyCode.LeftControl))
            return;
        Point point = getClosestPoint();
        if (point.Object != null)
        {
            Transform obj = point.Object.transform;
            Vector2 fP = new Vector2(obj.position.x, obj.position.z);
            Vector2 sP = obj.GetComponent<ObjectController>().EndPoint;
            Vector2 vec = new Vector2(currentObject.transform.position.x, currentObject.transform.position.z);

            
            if (Vector2.Distance(fP, vec) < 1f)
            {

                if (Mathf.RoundToInt((Mathf.Abs(obj.rotation.eulerAngles.y - currentObject.transform.rotation.eulerAngles.y) / 90)) % 2 == 0)
                {
                    currentObject.transform.rotation = obj.rotation;
                    currentObject.transform.Rotate(new Vector3(0, 180, 0));
                    float f = obj.transform.rotation.eulerAngles.y;
                    currentObject.transform.position = new Vector3(fP.x + 0.25f * Mathf.Sin(f / 180 * Mathf.PI), 0, fP.y + 0.25f * Mathf.Cos(f / 180 * Mathf.PI));
                }
                
            }
            else if (Vector2.Distance(sP, vec) < 1f)
            {
                if (Mathf.RoundToInt((Mathf.Abs(obj.rotation.eulerAngles.y - currentObject.transform.rotation.eulerAngles.y) / 90)) % 2 == 0)
                {
                    currentObject.transform.rotation = obj.rotation;
                    currentObject.transform.position = new Vector3(sP.x, 0, sP.y);
                }
            }
            //snap to edges of world. 
            else
                snapEdge();

        }
        else
        {
            snapEdge();
        }

    }
    void snapEdge()
    {
       
        Vector2 vec = new Vector2(currentObject.transform.position.x, currentObject.transform.position.z);
        Vector2 dimensions = grid.Dimensions / 4;

        if (vec.x > dimensions.x - 1f && vec.x < dimensions.x + 1)
        {
            currentObject.transform.position = new Vector3(dimensions.x, 0, vec.y);
            currentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (vec.x < 1f && vec.x > -1)
        {
            currentObject.transform.position = new Vector3(0, 0, vec.y);
            currentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (vec.y > dimensions.y - 1f && vec.y < dimensions.y + 1)
        {
            currentObject.transform.position = new Vector3(vec.x, 0, dimensions.y);
            currentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else if (vec.y < 1f && vec.y > -1)
        {
            currentObject.transform.position = new Vector3(vec.x, 0, 0);
            currentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
        }

    }
    Transform getClosest()
    {
        float f = 1000;
        Transform closestObject = null;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Prop"))
        {

            if (g.transform.GetInstanceID() == currentObject.transform.GetInstanceID())
                continue;

            float tmp = Vector2.Distance(new Vector2(currentObject.transform.position.x, currentObject.transform.position.z), new Vector2(g.transform.position.x, g.transform.position.z));
            if (tmp < f)
            {
                f = tmp;
                closestObject = g.transform;
            }

        }

        return closestObject;
    }
    Point getClosestPoint()
    {
        List<Point> pointList = new List<Point>();
        float f = float.PositiveInfinity;
        Point closestPoint = new Point(null, Vector2.zero);
        Vector2 currentPosition = new Vector2(currentObject.transform.position.x, currentObject.transform.position.z);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Prop"))
        {
            if (g.GetComponent<ObjectController>().placed)
            {
                pointList.Add(new Point(g, new Vector2(g.transform.position.x, g.transform.position.z)));
                pointList.Add(new Point(g, g.GetComponent<ObjectController>().EndPoint));
            }
        }
        foreach (Point p in pointList)
        {
            if (Vector2.Distance(p.point, currentPosition) < f)
            {
                closestPoint = p;
                f = Vector2.Distance(p.point, currentPosition);
            }
        }
        return closestPoint;
    }
    //struct used for wall snapping calculation. 
    struct Point
    {
        public Vector2 point;
        public GameObject Object;

        public Point(GameObject obj, Vector2 p)
        {
            Object = obj;
            point = p;
        }
 
    }
}
