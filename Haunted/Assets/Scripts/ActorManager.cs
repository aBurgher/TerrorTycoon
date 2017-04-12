using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public GameObject obj;
    List<Wall> Walls;
    struct Wall
    {
        public Transform obj;
        public bool ishit { get; set; }

        public Wall(Transform o, bool hit)
        {
            obj = o;
            ishit = hit;
        }
    }
    // Use this for initialization
    void Start()
    {
        Walls = new List<Wall>();
    }

    // Update is called once per frame
    void Update()
    {
        if (obj != null)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Instantiate(obj);
            }
        for (int i = 0; i < Walls.Count; i++)
        {
            Wall w = Walls[i];
            if (w.obj == null)
                continue;
            if (w.ishit)
            {
                w.ishit = false;
                Walls[i] = w;
            }
            else
            {
                Color c = w.obj.GetComponent<Renderer>().material.color;
                c.a = 1f;
                w.obj.GetComponent<Renderer>().material.color = c;
                Walls.Remove(w);
            }
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Actor"))
        {
            //get screen coordinates
            Vector2 ScreenCO = Camera.main.WorldToScreenPoint(obj.transform.position);
            //cast ray through camera at those coordinates
            Ray ray = Camera.main.ScreenPointToRay(ScreenCO);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100);
            foreach (RaycastHit hit in hits)
            {
                //if it hits a prop and != null
                if(hit.transform.tag == "Prop" || hit.transform.tag == "Wall")
                if (hit.collider != null)
                {
                    Wall w = new Wall(hit.transform, false);
                    if (Walls.Contains(w))
                    {
                        int i = Walls.FindIndex(a => a.obj == w.obj);
                        w.ishit = true;
                        Walls[i] = w;
                    }
                    Color c = hit.transform.GetComponent<Renderer>().material.color;
                    c.a = 0.5f;
                    hit.transform.GetComponent<Renderer>().material.color = c;
                    Wall n = new Wall(hit.transform, true);
                    Walls.Add(n);
                }
            }
        }
    }
}
