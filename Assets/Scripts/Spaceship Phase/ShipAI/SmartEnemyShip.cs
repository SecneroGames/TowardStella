using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This enemy ship will prioritize for speed boost in order to get ahead of the player
/// </summary>
public class SmartEnemyShip : MonoBehaviour
{
    [System.Serializable]
    enum states
    {
        NORMAL,
        BOOSTER_SPOTTED,
    }

    [SerializeField] states currentState = states.NORMAL;

    [Space]
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField] 
    private AudioSource audioSource;
    [SerializeField] 
    private Transform goal;
    [SerializeField]
    private Vector3 nextTarget;

    [Header("Detection Setup")]
    [SerializeField]
    private float detectionRadius = 250;
    private Vector3 detectionOffset;
    [SerializeField]
    private float observeFrequency = .5f; // observe surroundings every what second
    [SerializeField]
    private LayerMask ObservableLayers;
  
    private float observationTimer = 0;
    private float  defaultSpeed;
    private float  defaultAcceleration;
    private float  boostTimer = 0;



    void Start()
    {
        defaultSpeed = agent.speed;
        defaultAcceleration = agent.acceleration;
        detectionOffset = new Vector3(0, 0, detectionRadius);
        observationTimer = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet")|| other.CompareTag("Obstacle"))
        {
            agent.speed = 0f;
        }
      
    }

  
    void Update()
    {
        HandleBuffTimer();

        switch (currentState)
        {
            case states.NORMAL:
                HandleNormal();
                break;          
            case states.BOOSTER_SPOTTED:
                HandleBoosterSpotted();
                break;

        }
    }

    void HandleNormal()
    {
        observationTimer += Time.deltaTime;

        agent.SetDestination(goal.transform.position);

        if(observationTimer>=observeFrequency)
        {
            observationTimer = 0;
            Observe();
        }    
               

    }

    void HandleBoosterSpotted()
    {
        agent.SetDestination(nextTarget);
        observationTimer = 0;
        //dont do anything but keep going to target
        if (Vector3.Distance(transform.position,nextTarget)<5 )
        {
            agent.SetDestination(goal.transform.position);
            AudioManager.instance.PlaySFX(audioSource, "BoostSFX");
            agent.acceleration = defaultAcceleration * 2.5f;
            agent.speed = defaultSpeed * 1.5f;
            boostTimer = 1.2f; // 1.2 seconds speedbosst
            currentState = states.NORMAL;
        }
    }
    

    void Observe()
    {
        //look for speedboost
        Collider[] observedObjects = Physics.OverlapSphere(transform.position + detectionOffset, detectionRadius, ObservableLayers);
        foreach (var obj in observedObjects)
        {
            if (obj.CompareTag("Speedboost"))
            {
                currentState = states.BOOSTER_SPOTTED;
                nextTarget = obj.transform.position;
                break;//stop loop
            }
        }
    }

    void HandleBuffTimer()
    {
        if (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                agent.acceleration = defaultAcceleration;
                agent.speed = defaultSpeed;
                boostTimer = 0;
                currentState = states.NORMAL;
            }
        }

    }
    // Method to set destination for NavMeshAgent
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void OnDrawGizmos()
    {
        // For detection Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,0,detectionRadius), detectionRadius);
    }
}
