using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour {
    public enum Act {Place, Edit, Delete };
    List<change> Undo;
    List<change> Redo;
    //Defines a change in the gameworld. 
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
        Undo = new List<change>();
        Redo = new List<change>();
      
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z) && Undo.Count != 0)
        {
            change c = Undo[Undo.Count - 1];
            if (c.action == Act.Place)
            {
                Undo.Remove(c);
                c.obj.SetActive(false);
                Redo.Add(c);
            }
            if (c.action == Act.Edit)
            {
                Undo.Remove(c);
                Vector3 tmpPos;
                Quaternion tmpRot;
                tmpPos = c.obj.transform.position;
                tmpRot = c.obj.transform.rotation;
                c.obj.transform.position = c.position;
                c.obj.transform.rotation = c.rotation;
                c.rotation = tmpRot;
                c.position = tmpPos;
                Redo.Add(c);
                
            }
            if (c.action == Act.Delete)
            {
                c.obj.transform.position = c.position;
                c.obj.transform.rotation = c.rotation;
                c.obj.SetActive(true);
                Undo.Remove(c);
                Redo.Add(c);
            }
          
            GameObject.Find("Grid").GetComponent<Grid>().update = true;
        }
        else if (Input.GetKeyDown(KeyCode.Y) && Redo.Count != 0)
        {
            change c = Redo[Redo.Count - 1];
            if (c.action == Act.Place)
            {
                Redo.Remove(c);
                c.obj.SetActive(true);
                Undo.Add(c);
            }
            if (c.action == Act.Edit)
            {
                Redo.Remove(c);
                Vector3 tmpPos;
                Quaternion tmpRot;
                tmpPos = c.obj.transform.position;
                tmpRot = c.obj.transform.rotation;
                c.obj.transform.position = c.position;
                c.obj.transform.rotation = c.rotation;
                c.rotation = tmpRot;
                c.position = tmpPos;
                Undo.Add(c);
            }
            if (c.action == Act.Delete)
            {

                c.obj.SetActive(false);
                Redo.Remove(c);
                Undo.Add(c);
            }
        }
	}
    public void addChange(change c )
    {
        Undo.Add(c);
        Redo.Clear();
    }
}
