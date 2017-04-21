using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    float velx, vely, zoom;
    bool flip = false;
    public float speed = 0.5f;
    public float zoomSpeed = 1f;
	// Use this for initialization
	void Start () {
        velx = vely = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            flip = !flip;
        if (flip)
        {
            if (Input.GetKey(KeyCode.W))
            {
                velx += 0.70710678118f;
                vely += 0.70710678118f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velx -= 0.70710678118f;
                vely -= 0.70710678118f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velx -= 0.70710678118f;
                vely += 0.70710678118f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                velx += 0.70710678118f;
                vely -= 0.70710678118f;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                velx += 0.70710678118f;
                vely += 0.70710678118f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velx -= 0.70710678118f;
                vely -= 0.70710678118f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velx -= 0.70710678118f;
                vely += 0.70710678118f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                velx += 0.70710678118f;
                vely -= 0.70710678118f;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.gameObject.transform.GetChild(0).GetComponent<Camera>().orthographicSize += -zoomSpeed*Input.GetAxis("Mouse ScrollWheel");
            this.gameObject.transform.GetChild(0).GetComponent<Camera>().orthographicSize = Mathf.Clamp(this.gameObject.transform.GetChild(0).GetComponent<Camera>().orthographicSize, 0.1f, 10f);
        }
        zoom = this.gameObject.transform.GetChild(0).GetComponent<Camera>().orthographicSize;
        transform.Translate(new Vector3(speed*velx*zoom/5f, 0f, speed*vely*zoom/5f));
        velx = vely = 0;
        if (flip)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
