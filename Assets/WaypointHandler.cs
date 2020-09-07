using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointHandler : MonoBehaviour
{
    public bool circulate;
    internal int curWaypointTargetIndex = 1;
    internal List<Waypoint> waypoints = new List<Waypoint>();
    private bool patrollingForward = true;
    private float reachedTargetThreshold = 1;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints.Add(transform.GetChild(i).GetComponent<Waypoint>());
        }
    }
    void Start()
    {
        transform.parent = GameManager.instance.transform.Find("WaypointHandlers");
        transform.position = new Vector3(transform.position.x,0,transform.position.z);
    }
    public void UpdateNextWaypointTargetIndex()
    {
        if (circulate)
        {
            if (curWaypointTargetIndex == waypoints.Count - 1)
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
            if (curWaypointTargetIndex == waypoints.Count - 1 || curWaypointTargetIndex == 0)
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
    public Vector3 GetCurrentWaypointPos()
    {
        return waypoints[curWaypointTargetIndex].transform.position;
    }
    public bool HasReachedWaypoint(Vector3 enemyPos, Waypoint target)
    {
        print("Distance to waypoint: " + Vector3.Distance(enemyPos, target.transform.position));
        return Vector3.Distance(enemyPos, target.transform.position) <= reachedTargetThreshold;
    }
    public Waypoint GetCurrentWaypoint()
    {
        if (curWaypointTargetIndex >= waypoints.Count || curWaypointTargetIndex < 0)
        {
            print("Tried to get waypoint: " + curWaypointTargetIndex + " but max is: " + waypoints.Count);
        }
        return waypoints[curWaypointTargetIndex];
    }
    public void DestroyWaypoint(int index)
    {
        Destroy(waypoints[index].gameObject);
        waypoints.RemoveAt(index);
    }
}
