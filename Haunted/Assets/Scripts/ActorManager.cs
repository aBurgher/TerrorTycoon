using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {
    public GameObject obj;
    List<GameObject> Walls;
	// Use this for initialization
	void Start () {
        Walls = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if(obj != null)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
            Instantiate(obj);
            }

	}
}
