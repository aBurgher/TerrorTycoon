using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorController : MonoBehaviour
{

    NavMeshAgent agent;
    List<int> AffectedBy;
    public float SpookyLevel = 0, ScaryLevel  = 0;
    ParticleSystem particleSystem;
    ScoreDisplay Score;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        AffectedBy = new List<int>();
        
        particleSystem = GetComponentInChildren<ParticleSystem>();
        Score = GameObject.Find("ScoreText").GetComponent<ScoreDisplay>();
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
    public void ApplyEffect(int ID, float power, bool Spooky)
    {
        if(AffectedBy.Contains(ID))
        {
            return; 
        }
        else
        {
            if (Spooky)
            {

                particleSystem.Emit((int)power);
                SpookyLevel += 5*power;
                SpookyLevel *= 1.0f+(power / 5.0f);
            }
            else
            {
                Score.Score += SpookyLevel * power/5.0f;
                SpookyLevel *= 1.0f - (power / 5.0f);
                particleSystem.Emit(10*(int)power);
            }
            AffectedBy.Add(ID);
        }
    }
}
