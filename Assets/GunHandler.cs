using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public GameObject gun;
    public Transform gunPoint;
    public RayGenerator ray;
    private Vector3 pointA; //The three corners of the ray
    private Vector3 pointB;
    private Vector3 pointC;
    private bool hitEnemy;
    void Update()
    {
        pointA = gunPoint.position;
        pointC = Vector3.zero;

        if (Physics.Raycast(gunPoint.position, gun.transform.forward, out RaycastHit hit))
        {
            hitEnemy = hit.collider.transform.root.TryGetComponent(out Enemy enemy);
            pointB = hit.point;

            //If it doesn't hit an enemy, bounce
            if (!hitEnemy)
            {
                Vector3 reflectVector = Vector3.Reflect(gun.transform.forward, hit.normal);

                if (Physics.Raycast(hit.point, reflectVector, out RaycastHit reflectHit))
                {
                    hitEnemy = reflectHit.collider.transform.root.TryGetComponent(out Enemy enemy2);
                    pointC = reflectHit.point;
                }
                else
                {
                    pointC = hit.point + reflectVector * 10;
                }
            }
        }
        else
        {
            hitEnemy = false;
            pointB = gunPoint.position + gun.transform.forward * 50;
        }
        ray.DrawMesh(pointA, pointB, pointC, hitEnemy);
    }
}
