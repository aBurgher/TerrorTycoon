using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorController : MonoBehaviour
{

    NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(new Vector3(25,0, 20));
        if (Vector3.Distance(transform.position, agent.destination) < 0.5)
        {
            Destroy(this.gameObject);
        }
    }
}
