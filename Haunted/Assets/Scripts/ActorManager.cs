using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {
    public GameObject obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(obj != null)
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
            Instantiate(obj);
            }
	}
}
