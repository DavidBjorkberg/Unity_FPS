using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHandler : MonoBehaviour
{
    private EnemyMovement movement;
    private EnemyGunHandler gunHandler;
    private PlayerDetection playerDetection;
    internal enum States { Patrolling, Chasing, Shooting, DetectedPlayer, ReturnToPatrol };
    internal States state;

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        gunHandler = GetComponent<EnemyGunHandler>();
        playerDetection = GetComponent<PlayerDetection>();
    }
    void Update()
    {
        if (playerDetection.IsPlayerInView())
        {
            state = States.DetectedPlayer;
        }
    }

}
