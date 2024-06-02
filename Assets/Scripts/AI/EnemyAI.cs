using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 2f; // Speed of the enemy movement
    [SerializeField] private float closeEnoughDistance = 0.1f; // Distance to switch to the next waypoint
    [SerializeField] private float rotationSpeed = 5f; // Speed of the enemy rotation

    private Transform target;
    private int waypointIndex = 0;
    public int laps = 0;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            target = waypoints[waypointIndex];
        }
        else
        {
            Debug.LogError("No waypoints set in the waypoints array.");
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return; // Exit if there are no waypoints

        MoveTowardsTarget();
        RotateTowardsTarget();

        if (Vector3.Distance(transform.position, target.position) < closeEnoughDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            target = waypoints[waypointIndex];
        }

        if(laps >= 3)
        {
            RaceTrackManager.Instance.shipsDone = 1;
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Goal")
        {
            laps++;
        }
    }
}
