using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGenerator : MonoBehaviour
{
    public GunHandler gunHandler;
    public Material rayMaterial;
    public GameObject gunHolder;
    public Material enemyHitRayMaterial;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] indices;
    private float raySize = 0.05f;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = rayMaterial;
        mesh.RecalculateBounds();
    }
    public void DrawMesh(Vector3 pointA, Vector3 pointB, Vector3 pointC,Vector3 BToCLeftVector, bool hitEnemy)
    {
        mesh.Clear();
        GameObject gun = gunHandler.currentGun.gameObject;
        if (hitEnemy)
        {
            GetComponent<MeshRenderer>().material = enemyHitRayMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = rayMaterial;
        }
        //Move pointB/C away from the wall to prevent it clipping with it.
        Vector3 noClipPointB = pointB + (pointA - pointB).normalized * 0.1f;
        Vector3 noClipPointC = pointC + (pointB - pointC).normalized * 0.1f;

        Vector3 gunBottomLeftCorner = -gun.transform.up * raySize - gun.transform.right * raySize;
        Vector3 gunBottomRightCorner = -gun.transform.up * raySize + gun.transform.right * raySize;
        Vector3 gunTopLeftCorner = gun.transform.up * raySize - gun.transform.right * raySize;
        Vector3 gunTopRightCorner = gun.transform.up * raySize + gun.transform.right * raySize;

        Vector3 BToC = (pointC - pointB).normalized;
        Vector3 BToCDown = Vector3.Cross(BToC, BToCLeftVector);
        Vector3 BToCBottomLeftCorner = BToCLeftVector * raySize + BToCDown * raySize;
        Vector3 BToCBottomRightCorner = -BToCLeftVector * raySize + BToCDown * raySize;
        Vector3 BToCTopLeftCorner = BToCLeftVector * raySize - BToCDown * raySize;
        Vector3 BToCTopRightCorner = -BToCLeftVector * raySize - BToCDown * raySize;

        
            //TODO: Waste to create all vertices and indices when the ray doesn't hit anything
        vertices = new Vector3[]
        {
            gun.transform.InverseTransformPoint(pointA + gunBottomLeftCorner), //0
            gun.transform.InverseTransformPoint(pointA + gunBottomRightCorner), //1
            gun.transform.InverseTransformPoint(pointA + gunTopLeftCorner),    //2
            gun.transform.InverseTransformPoint(pointA + gunTopRightCorner),   //3
            gun.transform.InverseTransformPoint(noClipPointB + gunBottomLeftCorner), //4
            gun.transform.InverseTransformPoint(noClipPointB + gunBottomRightCorner), //5
            gun.transform.InverseTransformPoint(noClipPointB + gunTopLeftCorner),    //6
            gun.transform.InverseTransformPoint(noClipPointB + gunTopRightCorner),   //7
            gun.transform.InverseTransformPoint(noClipPointB + BToCBottomLeftCorner),//8
            gun.transform.InverseTransformPoint(noClipPointB + BToCBottomRightCorner),//9
            gun.transform.InverseTransformPoint(noClipPointB + BToCTopLeftCorner),    //10
            gun.transform.InverseTransformPoint(noClipPointB + BToCTopRightCorner),   //11
            gun.transform.InverseTransformPoint(noClipPointC + BToCBottomLeftCorner),//12
            gun.transform.InverseTransformPoint(noClipPointC + BToCBottomRightCorner),//13
            gun.transform.InverseTransformPoint(noClipPointC + BToCTopLeftCorner),    //14
            gun.transform.InverseTransformPoint(noClipPointC + BToCTopRightCorner),   //15
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
            12,9,8 ,//B->CBottom
            13,9,12,
            10,11,14, //B->CTop
            14,11,15,
            15,9,13, //B->C Left
            11,9,15,
            12,8,10, //B->C right
            14,12,10,
            12,14,13, //PointC
            14,15,13,

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
    public void ResetMesh()
    {
        mesh.Clear();
    }
}
