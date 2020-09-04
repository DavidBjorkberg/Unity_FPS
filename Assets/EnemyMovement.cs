using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    public int movementSpeed;
    public Transform[] waypoints;
    public bool circulate;
    NavMeshAgent navAgent;
    int curWaypointTargetIndex;
    float reachedTargetThreshold = 1;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        if(HasReachedWaypoint())
        {
            MoveToNextWaypoint();
        }
    }
    /// <summary>
    /// Increments curWaypointTargetIndex and Sets destination to the next waypoint
    /// </summary>
    void MoveToNextWaypoint()
    {
        curWaypointTargetIndex++;
        navAgent.SetDestination(waypoints[curWaypointTargetIndex].position);
    }
    /// <summary>
    /// Returns true if the enemy is within reachedTargetThreshold of the waypoint
    /// </summary>
    /// <returns></returns>
    bool HasReachedWaypoint()
    {
        return Vector3.Distance(transform.position, waypoints[curWaypointTargetIndex].position) <= reachedTargetThreshold;
    }
}
