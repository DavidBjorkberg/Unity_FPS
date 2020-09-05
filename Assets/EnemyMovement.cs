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
    private EnemyStateHandler stateHandler;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stateHandler = GetComponent<EnemyStateHandler>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        switch (stateHandler.state)
        {
            case EnemyStateHandler.States.Patrolling:
                Patrol();
                break;
            case EnemyStateHandler.States.Chasing:
                break;
            case EnemyStateHandler.States.ReturnToPatrol:
                break;
            default:
                break;
        }
    }
    void Patrol()
    {
        if (waypointHandler.HasReachedWaypoint(transform.position))
        {
            waypointHandler.UpdateNextWaypointTargetIndex();
            MoveToNextWaypoint();
        }
    }
    void Chase()
    {

    }
    void ReturnToPatrol()
    {

    }
    /// <summary>
    /// Rotates the enemy towards a point over time.
    /// </summary>
    /// <param name="rotateToPos"></param>
    /// <returns></returns>
    //IEnumerator RotateTowards(Vector3 rotateToPos)
    //{
    //    float rotateSpeed = 5;

    //}
    /// <summary>
    /// Modifies curWaypointTargetIndex and Sets destination to the next waypoint
    /// </summary>
    void MoveToNextWaypoint()
    {
        navAgent.SetDestination(waypointHandler.GetNextWaypoint());
    }
}
