using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonOnclick : MonoBehaviour {
    public  GameObject obj = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        PlacementManager manager = GameObject.Find("Grid").GetComponent<PlacementManager>();
        manager.currentObject = Instantiate(obj, Vector3.zero, Quaternion.identity, GameObject.Find("Grid").transform);
    }
}
