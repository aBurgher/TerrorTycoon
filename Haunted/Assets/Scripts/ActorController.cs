using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    List<Vector2> Path;
    public float moveSpeed;
    int index = 0;
    Vector2 nextPos;
	// Use this for initialization
	void Start () {
 
   }
	
	// Update is called once per frame
	void Update () {
        if (Path == null || Path.Count == 0)
        {
            Path = GameObject.Find("Grid").GetComponent<Grid>().Path;
            transform.position = Path[0];
            nextPos = Path[1];
            index = 1;
        }
        else
        {
            if (Vector3.Distance(transform.position, new Vector3(nextPos.x /2, 0f, nextPos.y /2)) > 0.01)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(nextPos.x /2, 0f, nextPos.y /2), 4*Time.deltaTime);
                transform.forward = new Vector3(nextPos.x, 0f, nextPos.y); 
            }
            else
            {

                if (nextPos != Path[Path.Count - 1])
                {
                    index++;
                    nextPos = Path[index];
                }
                else
                    Destroy(gameObject);
              
            }
        }
	}
}
