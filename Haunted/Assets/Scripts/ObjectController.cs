using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public float length = 1, width = 1, Range = 1, Power = 1;
    public bool walkable = false, placed = true, Spooky = true, Scary = false;
    public Animator anim;
    public Vector2 EndPoint;
    List<int> triggeredBy;
    Vector3 previousPosition, Center;
    public Renderer rend;
	// Use this for initialization
	void Start () {
        triggeredBy = new List<int>();
        previousPosition = transform.position;
        CalculatePoints();
        if (this.gameObject.tag != "Wall")
        {
          //  this.gameObject.GetComponent<Renderer>().material.color = Color.red;
      
        }
      
    }
	
	// Update is called once per frame
	void Update () {
        //used for storing previous position and end points. 
        if (transform.position != previousPosition && placed)
        {
            CalculatePoints();
            previousPosition = transform.position;
        }
        //if scary
        if (!Spooky && Scary && placed)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            //check range if object is ready
            if (stateInfo.IsName("Idle"))
            {
                CheckRange();
            }
            if(stateInfo.IsName("Trigger"))
            {
                applyEffect();
            }
            
        }
        //if spooky, apply the effect with no change in animation. 
        else if(Spooky && !Scary && placed)
        {
            applyEffect();
        }
    
	}
    public void DisplayRange(bool Show)
    {
        if (!Spooky && !Scary)
            return;
        if (Show)
        {
            GameObject.Instantiate(Resources.Load("RangeDisplay"), GetComponent<BoxCollider>().center, Quaternion.identity, transform);
            Vector3 tmp = transform.FindChild("RangeDisplay(Clone)").gameObject.transform.localScale;
            tmp.x *= Range;
            tmp.z *= Range;
            
            transform.FindChild("RangeDisplay(Clone)").gameObject.transform.localScale = tmp;
            tmp = GetComponent<BoxCollider>().center;
            tmp.y = 0;
            transform.FindChild("RangeDisplay(Clone)").gameObject.transform.localPosition = tmp;
          
      

        }
        else
        {
            Destroy(transform.FindChild("RangeDisplay(Clone)").gameObject);
        
        }
    }
    void CheckRange()
    {
        GameObject[] Actors = GameObject.FindGameObjectsWithTag("Actor");
        foreach (GameObject actor in Actors)
        {
            float distance = Vector3.Distance(Center, actor.transform.position);
            if (distance < Range)
            {
                Ray ray = new Ray(actor.transform.position, Center - actor.transform.position);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                
                if (hit.collider != null && hit.collider.gameObject.tag == "Prop"&& !triggeredBy.Contains(actor.GetInstanceID()))
                {

                    anim.SetTrigger("Trigger");
                    triggeredBy.Add(actor.GetInstanceID());
                }
            }
        }
        //if(triggered)
          //  Trigger.Play();
    }
    void applyEffect()
    {
        GameObject[] Actors = GameObject.FindGameObjectsWithTag("Actor");
        foreach (GameObject actor in Actors)
        {
            float distance = Vector3.Distance(Center, actor.transform.position);
            if (distance < Range)
            {
                Ray ray = new Ray(actor.transform.position, Center-actor.transform.position);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);


                if (hit.collider != null && hit.collider.gameObject.tag == "Prop")
                {
                    actor.GetComponent<ActorController>().ApplyEffect(GetInstanceID(), Power, Spooky);
                    
                }
            }
        }
    }
    void CalculatePoints()
    {
        Vector2 d = new Vector2(length, width);
        float rot = transform.rotation.eulerAngles.y;
        EndPoint = new Vector2(transform.position.x + (Mathf.Cos(-rot / 180 * Mathf.PI) * 0.25f * d.y), transform.position.z + (Mathf.Sin(-rot / 180 * Mathf.PI) * d.y * 0.25f));
        EndPoint = new Vector2(Mathf.Round(EndPoint.x*4)/4,Mathf.Round(EndPoint.y*4)/4);
        Center = transform.TransformPoint(GetComponent<BoxCollider>().center);
    
       
    }
}
