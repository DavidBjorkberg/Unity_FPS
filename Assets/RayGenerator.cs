using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] indices;
    float raySize = 0.1f;
    public GameObject gun;
    public Material rayMaterial;
    public Material enemyHitRayMaterial;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = rayMaterial;
        mesh.RecalculateBounds();
    }
    public void DrawMesh(Vector3 pointA, Vector3 pointB, Vector3 pointC, bool hitEnemy)
    {
        mesh.Clear();
        if (hitEnemy)
        {
            GetComponent<MeshRenderer>().material = enemyHitRayMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = rayMaterial;
        }
        //Prevent (mostly) the mesh intersecting with the hit collider
        Vector3 nonIntersectPointB = pointB - (pointB - pointA).normalized * raySize;
        Vector3 nonIntersectPointC = pointC - (pointC - pointB).normalized * raySize;

        Vector3 bottomLeftCorner = -gun.transform.up * raySize - gun.transform.right * raySize;
        Vector3 bottomRightCorner = -gun.transform.up * raySize + gun.transform.right * raySize;
        Vector3 topLeftCorner = gun.transform.up * raySize - gun.transform.right * raySize;
        Vector3 topRightCorner = gun.transform.up * raySize + gun.transform.right * raySize;

        //TODO: Waste to create all vertices and indices when the ray doesn't hit anything
        vertices = new Vector3[]
        {
            gun.transform.InverseTransformPoint(pointA + bottomLeftCorner), //0
            gun.transform.InverseTransformPoint(pointA + bottomRightCorner), //1
            gun.transform.InverseTransformPoint(pointA + topLeftCorner),    //2
            gun.transform.InverseTransformPoint(pointA + topRightCorner),   //3
            gun.transform.InverseTransformPoint(pointB + bottomLeftCorner), //4
            gun.transform.InverseTransformPoint(pointB + bottomRightCorner), //5
            gun.transform.InverseTransformPoint(pointB + topLeftCorner),    //6
            gun.transform.InverseTransformPoint(pointB + topRightCorner),   //7
            gun.transform.InverseTransformPoint(pointC + bottomLeftCorner),//8
            gun.transform.InverseTransformPoint(pointC + bottomRightCorner),//9
            gun.transform.InverseTransformPoint(pointC + topLeftCorner),    //10
            gun.transform.InverseTransformPoint(pointC + topRightCorner),   //11
        };

        indices = new int[]
        {
            0,1,2, //PointA
            2,1,3,
            1,4,0, //A->B Bottom
            1,5,4,
            3,2,6, //A->B Top
            3,6,7,
            1,7,5, //A->B Right
            1,3,7,
            0,4,2, //A->B Left
            4,6,2,
            4,5,6, //PointB
            6,5,7,
            5,8,4, //B->C Bottom
            5,9,8,
            7,6,10,//B->C Top
            7,10,11,
            5,11,9,//B->C Left
            5,7,11,
            4,8,6, //B->C Right
            8,10,6,
            8,10,9, //Point C
            10,11,9
        };

        //Ray didn't hit anything , don't bounce it
        if (pointC == Vector3.zero)
        {
            Vector3[] ABVertices = new Vector3[8];
            int[] ABIndices = new int[30];
            System.Array.Copy(vertices, ABVertices, 8);
            System.Array.Copy(indices, ABIndices, 30);

            mesh.vertices = ABVertices;
            mesh.triangles = ABIndices;

        }
        else
        {
            mesh.vertices = vertices;
            mesh.triangles = indices;
        }
    }
}
