using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour {
    public enum Act {Place, Edit, Delete };
    List<change> Undo = new List<change>();
    List<change> Redo = new List<change>();
    public struct change
    {
        public GameObject obj;
        public Act action;
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
       public change(GameObject o, Act act)
        {
            obj = o;
            action = act;
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }
        public change(GameObject o, Act act, Vector3 vec, Quaternion rot)
        {
            obj = o;
            action = act;
            position = vec;
            rotation = rot;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z) && Undo.Count != 0)
        {
            change c = Undo[Undo.Count - 1];
            if (c.action == Act.Place)
            {
                Undo.Remove(c);
                Destroy(c.obj);
            }
            if (c.action == Act.Edit)
            {
                Undo.Remove(c);
                c.obj.transform.position = c.position;
                c.obj.transform.rotation = c.rotation;
            }
            if (c.action == Act.Delete)
            {
                Instantiate(c.obj, c.position, c.rotation, GameObject.Find("Grid").transform);
                Undo.Remove(c);
            }
            GameObject.Find("Grid").GetComponent<Grid>().update = true;
            GameObject.Find("Grid").GetComponent<Grid>().updatePath = true;
        }
	}
    public void addChange(change c )
    {
        Undo.Add(c);
    }
}
