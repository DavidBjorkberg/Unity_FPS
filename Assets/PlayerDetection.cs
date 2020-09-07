using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Camera FieldOfViewCam;
    public Transform eyePos;
    private EnemyStateHandler stateHandler;
    void Start()
    {
        stateHandler = GetComponent<EnemyStateHandler>();
    }
    //Returns true if the enemy can see the player (defined by the enemy's camera frustum)
    public bool IsPlayerInView()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 viewPos = FieldOfViewCam.WorldToViewportPoint(playerPos);
        bool isInsideFrustum = viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
        if(isInsideFrustum)
        {
            if(Physics.Raycast(eyePos.position,(playerPos - eyePos.position),out RaycastHit hit))
            {
                if(hit.collider.TryGetComponent(out Player player))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Checks after secondsToWait if the player is still in line of sight, then switches to shooting/patrol state
    /// </summary>
    public IEnumerator CheckIfHeldLineOfSight()
    {
        float secondsToWait = 1;
        yield return new WaitForSeconds(secondsToWait);
        if (IsPlayerInView())
        {
            stateHandler.SwitchToShootingState();
        }
        else
        {
            stateHandler.SwitchToPatrolState();
        }
    }
}
