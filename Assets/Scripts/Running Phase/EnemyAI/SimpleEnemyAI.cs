using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyAI : MonoBehaviour
{
    public enum EnemyState
    {        
        Idle,
        Chase,
        Roam,
        Attack
    }


    [SerializeField] private NavMeshAgent agent;
    private Transform Player;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private AudioSource audioSource;

    [Header("Setup")]
    [SerializeField] private float chasingRange = 20;
    [SerializeField] private float viewingAngle = 110f;
    [SerializeField] private float walkRadius = 20f;
    [Space]
    float enemyIdleTime = 2f;
    [SerializeField] float enemyRoamTime = 5f;
    [SerializeField] float enemyChaseTime = 10f;

    private EnemyState currentState = EnemyState.Idle;

    private float stateTimer;
    private float defaultSpeed;
    void Start()
    {
        Player = RunningPhaseManager.instance.Player;
        defaultSpeed = agent.speed;
        audioSource = GetComponent<AudioSource>();
       
        //start on idle by default
        stateTimer = enemyIdleTime;
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Roam:
                Roam();
                break;

        }

        UpdateAnimationParameter();
    }


    #region states
    void StateTimerCountDown()
    {
        //timer tic
        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
    }

    void Idle()
    {
        StateTimerCountDown();
        agent.speed = 0;
            
       
        if(stateTimer<=0)
        {
            agent.speed = defaultSpeed;          
            stateTimer = enemyRoamTime;
            currentState = EnemyState.Roam;
        }

    }

    void Chase()
    {
        StateTimerCountDown();

        if (stateTimer <= 0)
        {
            stateTimer = enemyIdleTime;
            currentState = EnemyState.Idle;
        }

        if (IsPlayerVisible())
        {           
            SetDestination(Player.position);
        }

        if(CanAttackPlayer())
        {         
            currentState = EnemyState.Attack;
        }
        

    }
    void Roam()
    {
        //only on enter
        if(stateTimer==enemyRoamTime)
        {
            SetDestination(RandomNavSphere(transform.position, walkRadius, -1));
        }

        StateTimerCountDown();

        if (stateTimer <= 0)
        {
            agent.speed = defaultSpeed;
            stateTimer = enemyIdleTime;
            currentState = EnemyState.Idle;
        }
        if (CanAttackPlayer())
        {
            currentState = EnemyState.Attack;
        }


        if (IsPlayerVisible())
        {
            stateTimer = enemyChaseTime;
            AudioManager.instance.PlaySFX(audioSource, "SpottedSFX");
            currentState = EnemyState.Chase;
        }


    }
    void Attack()
    {       
        agent.speed = 0;
        modelAnimator.Play("Attack");

        if(CanAttackPlayer())
        {
            RunningPhaseManager.instance.OnEnemyHit();
        }
      
        stateTimer = enemyIdleTime;
        currentState = EnemyState.Idle;
    }


    #endregion
    void UpdateAnimationParameter()
    {
        if (agent.speed > 0)
        {
            modelAnimator.SetFloat("MovementFloat", 2f);
        }
        else
        {
            modelAnimator.SetFloat("MovementFloat", -1f);
        }
                 
      
    }

       

  
    bool CanAttackPlayer()
    {
        return Vector3.Distance(RunningPhaseManager.instance.Player.position, transform.position) < 3;
    }

    bool IsPlayerVisible()
    {
        if (Vector3.Distance(RunningPhaseManager.instance.Player.position, transform.position) > chasingRange)
        {
            return false;
        }
             

        //vision check
        Vector3 direction = RunningPhaseManager.instance.Player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
       
        if (angle < viewingAngle / 2)
        {
            return true;
        }

        return false;

    }

    //set destination for NavMeshAgent
    public void SetDestination(Vector3 destination)
    {
        if (agent.enabled)
            agent.SetDestination(destination);
    }

    // for roaming
     Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

}
