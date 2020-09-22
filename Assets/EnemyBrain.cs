using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public WaypointHandler waypointHandler;
    private EnemyStateHandler stateHandler;
    private PlayerDetection playerDetection;
    private EnemyMovement movement;
    private EnemyGunHandler gunHandler;
    void Awake()
    {
        stateHandler = GetComponent<EnemyStateHandler>();
        playerDetection = GetComponent<PlayerDetection>();
        movement = GetComponent<EnemyMovement>();
        gunHandler = GetComponent<EnemyGunHandler>();
    }
    void Start()
    {
        movement.MoveToNextWaypoint();
        StartCoroutine(DecideAction());
    }
    IEnumerator DecideAction()
    {
        while (true)
        {
            if (stateHandler.IsCurrentState(EnemyStateHandler.States.Patrolling)
                || stateHandler.IsCurrentState(EnemyStateHandler.States.Chasing))
            {
                if (playerDetection.IsPlayerInView())
                {
                    stateHandler.SwitchToDetectedPlayerState();
                    movement.StopRotating();
                    Vector3 rotateDir = (GameManager.instance.player.transform.position - transform.position).normalized;

                    yield return StartCoroutine(movement.RotateTowardsDirection(rotateDir));
                    if (!stateHandler.IsCurrentState(EnemyStateHandler.States.Dead))
                    {
                        yield return StartCoroutine(playerDetection.CheckIfHeldLineOfSight());
                    }
                }
                else
                {
                    HandleWaypoint(waypointHandler.GetCurrentWaypoint());
                }

            }
            else if (stateHandler.IsCurrentState(EnemyStateHandler.States.Shooting))
            {
                gunHandler.Shoot();
                transform.forward = (GameManager.instance.player.transform.position - transform.position).normalized;
                if (!playerDetection.IsPlayerInView())
                {
                    stateHandler.SwitchToChasingState();
                }
            }
            else if (stateHandler.IsCurrentState(EnemyStateHandler.States.LookAround))
            {
                if (!movement.isLookingAround)
                {
                    StartCoroutine(movement.LookAroundForPlayer());
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// Checks distance to next waypoint and handles the behaviour when its reached
    /// </summary>
    public void HandleWaypoint(Waypoint waypoint)
    {
        if (waypointHandler.HasReachedWaypoint(transform.position, waypoint))
        {
            CheckWaypointBehaviour(waypoint);
        }
    }
    void CheckWaypointBehaviour(Waypoint waypoint)
    {
        Waypoint.EnemyWaypointBehaviour behaviour = waypoint.wayPointBehaviour;
        if (waypoint.destroyAfterUse)
        {
            waypointHandler.DestroyWaypoint(waypointHandler.curWaypointTargetIndex);
        }
        else
        {
            waypointHandler.UpdateNextWaypointTargetIndex();
        }
        switch (behaviour)
        {
            case Waypoint.EnemyWaypointBehaviour.Continue:
                movement.MoveToNextWaypoint();
                break;
            case Waypoint.EnemyWaypointBehaviour.LookAround:
                stateHandler.SwitchToLookAroundState();
                break;
            default:
                break;
        }

    }
}
