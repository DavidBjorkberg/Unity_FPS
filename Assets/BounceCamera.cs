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
        Physics.Raycast(lookFromPos, Vector3.forward, 1);
        Vector3 curTestSpot;
        Vector3 testSpotToLookAtDir;
        Vector3 testSpotToLookFromDir;
        float testSpotToLookFromDistance;
        float testSpotToLookAtDistance;
        for (int i = 0; i < pointsInSphere.Length; i++)
        {
            curTestSpot = lookFromPos + pointsInSphere[i];
            testSpotToLookAtDir = (lookAtPos - curTestSpot);
            testSpotToLookFromDir = (lookFromPos - curTestSpot);
            testSpotToLookFromDistance = testSpotToLookFromDir.magnitude;
            testSpotToLookAtDistance = testSpotToLookFromDir.magnitude;
            testSpotToLookFromDir.Normalize();
            testSpotToLookAtDir.Normalize();
            //Check if the path from the current test spot to the lookat position is clear 
            //and that the path from current test spot to the lookFrom position is clear (so the entire ray is visible)
            if (!Physics.Raycast(curTestSpot, testSpotToLookAtDir, testSpotToLookAtDistance * 0.9f))
            {
                if (!Physics.Raycast(curTestSpot, testSpotToLookFromDir, testSpotToLookFromDistance * 0.9f))
                {
                        return curTestSpot;
                }
            }
        }
        print("Couldn't find bounce camera position");
        return Vector3.zero;
    }
}
