using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    public int movementSpeed;
    public WaypointHandler waypointHandler;
    internal NavMeshAgent navAgent;
    private EnemyStateHandler stateHandler;
    internal bool isLookingAround;
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        waypointHandler.enemyHeight = navAgent.height;
        stateHandler = GetComponent<EnemyStateHandler>();
    }

    /// <summary>
    /// Looks to the left and right for player, gets cancelled if player is spotted. Else returns to patrol
    /// </summary>
    public IEnumerator LookAroundForPlayer()
    {
        isLookingAround = true;
        Vector3 initialForward = transform.forward;
        Vector3 initialRight = transform.right;
        Vector3 rotationTarget = Vector3.Lerp(initialForward, initialRight, 0.5f).normalized;

        yield return StartCoroutine(RotateTowardsDirection(rotationTarget));

        rotationTarget = Vector3.Lerp(initialForward, -initialRight, 0.5f).normalized;

        yield return StartCoroutine(RotateTowardsDirection(rotationTarget));
        isLookingAround = false;
        stateHandler.SwitchToPatrolState();
    }
    /// <summary >
    /// Rotates the enemy towards rotationDir over time.
    /// </summary>
    public IEnumerator RotateTowardsDirection(Vector3 rotationDir)
    {
        float rotateSpeed = 1f;
        float lerpValue = 0;
        Quaternion initialRotation = transform.rotation;
        Quaternion lookAtRotation = Quaternion.LookRotation(rotationDir);
        while (lerpValue < 1)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, lookAtRotation, lerpValue);
            lerpValue += rotateSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// stops all rotation / look around coroutines
    /// </summary>
    public void StopRotating()
    {
        StopCoroutine("RotateTowardsTarget");
        StopCoroutine("LookAroundForPlayer");
    }

    public void MoveToNextWaypoint()
    {
        navAgent.SetDestination(waypointHandler.GetCurrentWaypointPos());
    }
}
