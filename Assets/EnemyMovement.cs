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
    private PlayerDetection playerDetection;
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stateHandler = GetComponent<EnemyStateHandler>();
        playerDetection = GetComponent<PlayerDetection>();
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
    /// Rotates the enemy towards the point where the player was spotted over time.
    /// </summary>
    public IEnumerator RotateTowardsDirection(Vector3 rotationDir)
    {
        float rotateSpeed = 0.01f;
        float distanceThreshold = 0.2f;
        while (Vector3.Distance(transform.forward, rotationDir) >= distanceThreshold)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, rotationDir, rotateSpeed, 0);
            transform.rotation = Quaternion.LookRotation(newDir);
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
        print("Set destination to: " + waypointHandler.GetCurrentWaypointPos());
        navAgent.SetDestination(waypointHandler.GetCurrentWaypointPos());
    }
}
