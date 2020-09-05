using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    public int movementSpeed;
    public WaypointHandler waypointHandler;
    private PlayerDetection playerDetection;
    private NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        if(playerDetection.IsPlayerInView())
        {

        }
        else if (waypointHandler.HasReachedWaypoint(transform.position))
        {
            waypointHandler.UpdateNextWaypointTargetIndex();
            MoveToNextWaypoint();
        }
    }
    /// <summary>
    /// Modifies curWaypointTargetIndex and Sets destination to the next waypoint
    /// </summary>
    void MoveToNextWaypoint()
    {
        navAgent.SetDestination(waypointHandler.GetNextWaypoint());
    }
}
