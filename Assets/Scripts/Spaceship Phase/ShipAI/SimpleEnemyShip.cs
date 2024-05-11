using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class SimpleEnemyShip : MonoBehaviour
{
   
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] Transform goal; 

    void Start()
    {       
        if (agent.enabled)
        {
            SetDestination(goal.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet")|| other.CompareTag("Obstacle"))
        {
            agent.speed = 0f;
        }
    }


    // Method to set destination for NavMeshAgent
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
