using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointHandler : MonoBehaviour
{
    public bool circulate;
    internal int curWaypointTargetIndex = 1;
    private Waypoint[] waypoints;
    private bool patrollingForward = true;
    private float reachedTargetThreshold = 1;
    private void Start()
    {
        waypoints = new Waypoint[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i).GetComponent<Waypoint>();
        }
        transform.parent = GameManager.instance.transform.Find("WaypointHandlers");
    }
    public void UpdateNextWaypointTargetIndex()
    {
        if (circulate)
        {
            if (curWaypointTargetIndex == waypoints.Length - 1)
            {
                curWaypointTargetIndex = 0;
            }
            else
            {
                curWaypointTargetIndex++;
            }
        }
        else
        {
            if (curWaypointTargetIndex == waypoints.Length - 1 || curWaypointTargetIndex == 0)
            {
                patrollingForward = !patrollingForward;
            }

            if (patrollingForward)
            {
                curWaypointTargetIndex++;
            }
            else
            {
                curWaypointTargetIndex--;
            }
        }
    }
    public Vector3 GetNextWaypoint()
    {
       return waypoints[curWaypointTargetIndex].transform.position;
    }
    public bool HasReachedWaypoint(Vector3 enemyPos)
    {
        return Vector3.Distance(enemyPos, waypoints[curWaypointTargetIndex].transform.position) <= reachedTargetThreshold;
    }
}
