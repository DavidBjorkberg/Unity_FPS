using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public enum EnemyWaypointBehaviour { Continue, LookAround };
    public EnemyWaypointBehaviour wayPointBehaviour;
    public bool destroyAfterUse;

    public void Initialize(EnemyWaypointBehaviour behaviour, Vector3 pos,bool destroyOnUse)
    {
        wayPointBehaviour = behaviour;
        transform.position = pos;
        transform.position = new Vector3(pos.x, 1, pos.z);
        destroyAfterUse = destroyOnUse;
    }
}
