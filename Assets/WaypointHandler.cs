using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WaypointHandler : MonoBehaviour
{
    public bool circulate;
    internal int curWaypointTargetIndex = 1;
    internal List<Waypoint> waypoints = new List<Waypoint>();
    internal float enemyHeight;
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
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
        //Check if waypoint and enemy is on the same plane, so Y can be ignored in distance check (Makes it easier to place waypoints)
        if (Mathf.Abs(enemyPos.y - target.transform.position.y) < enemyHeight)
        {
            Vector3 enemyPosZeroY = new Vector3(enemyPos.x, 0, enemyPos.z);
            Vector3 targetPosZeroY = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            return Vector3.Distance(enemyPosZeroY, targetPosZeroY) <= reachedTargetThreshold;
        }
        return false;
    }
    public Waypoint GetCurrentWaypoint()
    {
        return waypoints[curWaypointTargetIndex];
    }
    public void DestroyWaypoint(int index)
    {
        Destroy(waypoints[index].gameObject);
        waypoints.RemoveAt(index);
    }
}
