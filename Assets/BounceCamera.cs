using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCamera : MonoBehaviour
{
    Vector3[] pointsInSphere =
    {
        new Vector3(0,1,0),
        new Vector3(0,2,0),
        new Vector3(0,-1,0),
        new Vector3(0,-2,0),
        new Vector3(1,0,0),
        new Vector3(2,0,0),
        new Vector3(-1,0,0),
        new Vector3(-2,0,0),
        new Vector3(0,0,1),
        new Vector3(0,0,2),
        new Vector3(0,0,-1),
        new Vector3(0,0,-2),
        new Vector3(1,1,0),
        new Vector3(-1,-1,0),
        new Vector3(-1,1,0),
        new Vector3(0,1,1),
        new Vector3(1,0,1),
        new Vector3(-1,0,1),
        new Vector3(0,-1,1),
        new Vector3(0,1,-1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,-1),
        new Vector3(0,-1,-1),
        new Vector3(1,-1,0),
    };

    public void SetCamera(Vector3 lookFromPos, Vector3 lookAtPos)
    {
        gameObject.SetActive(true);
        Vector3 calculatedPos = CalculatePosition(lookFromPos, lookAtPos);
        if (calculatedPos != Vector3.zero)
        {
            transform.position = calculatedPos;
        }
        transform.LookAt(lookAtPos);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    Vector3 CalculatePosition(Vector3 lookFromPos, Vector3 lookAtPos)
    {
        Vector3 curTestSpot;
        for (int i = 0; i < pointsInSphere.Length; i++)
        {
            curTestSpot = lookFromPos + pointsInSphere[i];
            if(IsPositionViable(curTestSpot,lookFromPos,lookAtPos))
            {
                return curTestSpot;
            }
        }
        //Should never reach
        print("Couldn't find bounce camera position");
        return Vector3.zero;
    }
    bool IsPositionViable(Vector3 testSpot,Vector3 lookFromPos, Vector3 lookAtPos)
    {
        Vector3 testSpotToLookAtDir = (lookAtPos - testSpot);
        Vector3 testSpotToLookFromDir = (lookFromPos - testSpot);
        float testSpotToLookFromDistance = testSpotToLookFromDir.magnitude;
        float testSpotToLookAtDistance = testSpotToLookFromDir.magnitude;
        testSpotToLookFromDir.Normalize();
        testSpotToLookAtDir.Normalize();
        //Check if the path from the current test spot to the lookat position is clear 
        //and that the path from current test spot to the lookFrom position is clear (so the entire ray is visible)
        if (!Physics.Raycast(testSpot, testSpotToLookAtDir, testSpotToLookAtDistance * 0.9f))
        {
            if (!Physics.Raycast(testSpot, testSpotToLookFromDir, testSpotToLookFromDistance * 0.9f))
            {
                return true;
            }
        }
        return false;
    }
}
