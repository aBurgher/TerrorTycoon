using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestButtonOnclick : MonoBehaviour {
    public  GameObject obj = null;
    Text infoText;
    // Use this for initialization
    void Start () {
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        PlacementManager manager = GameObject.Find("Grid").GetComponent<PlacementManager>();
        manager.currentObject = Instantiate(obj, Vector3.zero, Quaternion.identity, GameObject.Find("Grid").transform);
        infoText.text = obj.name;
    }
}
