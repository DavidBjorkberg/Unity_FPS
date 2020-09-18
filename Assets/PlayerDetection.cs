using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Camera FieldOfViewCam;
    public Transform eyePos;
    private EnemyStateHandler stateHandler;
    private EnemyColorController colorController;
    private LayerMask ignoreEnemyLayer;
    void Start()
    {
        stateHandler = GetComponent<EnemyStateHandler>();
        colorController = GetComponent<EnemyColorController>();
        ignoreEnemyLayer = ~(1 << 11);
    }
    //Returns true if the enemy can see the player (defined by the enemy's camera frustum)
    public bool IsPlayerInView()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 viewPos = FieldOfViewCam.WorldToViewportPoint(playerPos);
        bool isInsideFrustum = viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
        if (isInsideFrustum)
        {
            if (Physics.Raycast(eyePos.position, (playerPos - eyePos.position), out RaycastHit hit, Mathf.Infinity, ignoreEnemyLayer))
            {
                if (hit.collider.TryGetComponent(out Player player))
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
        float timeWaited = 0;
        Coroutine ColorCoroutine = colorController.SwitchToHostileColour();
        while (timeWaited < secondsToWait)
        {
            timeWaited += Time.deltaTime;
            if (!IsPlayerInView())
            {
                colorController.StopCoroutine(ColorCoroutine);
                stateHandler.SwitchToLookAroundState();
                colorController.SwitchToAwareColour(true);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        stateHandler.SwitchToShootingState();
    }
}
