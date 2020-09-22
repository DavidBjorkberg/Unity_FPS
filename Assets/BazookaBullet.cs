using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBullet : MonoBehaviour
{

    public float speed;
    public SphereCollider hitbox;
    public SphereCollider explodeCollider; //Spherecollider so its easier to edit the hitboxes in the editor. For optimized performance a float would instead be used for the radius
    public LayerMask obstacleLayer;
    private LayerMask targetLayer;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 pointC;
    private Collider pointBCollider;
    private int damage;

    public void Initialize(Vector3 pointA, Vector3 pointB, Vector3 pointC, int damage, Collider pointBCollider, bool isPlayerBullet)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        this.pointC = pointC;
        this.damage = damage;
        this.pointBCollider = pointBCollider;
        if (isPlayerBullet)
        {
            targetLayer = 1 << 11;
        }
        else
        {
            targetLayer = 1 << 12;
        }
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        Vector3 dir = (pointB - pointA).normalized;
        transform.forward = dir;
        yield return LerpBetweenPositions(pointA, pointB);
        dir = (pointC - pointB).normalized;
        transform.forward = dir;
        StartCoroutine(MoveInDirection(dir));

    }
    IEnumerator LerpBetweenPositions(Vector3 startPos, Vector3 endPos)
    {
        float lerpValue = 0;
        float distance = Vector3.Distance(startPos, endPos);
        float lerpSpeed = (speed / distance);
        while (lerpValue < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpValue);
            lerpValue += lerpSpeed * Time.deltaTime;
            CheckCollision();
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator MoveInDirection(Vector3 dir)
    {
        Invoke("Explode", 10);
        while (true)
        {
            transform.position += dir * speed * Time.deltaTime;
            CheckCollision();
            yield return new WaitForFixedUpdate();
        }
    }
    void CheckCollision()
    {
        if (Physics.CheckSphere(hitbox.transform.position, hitbox.radius, obstacleLayer | targetLayer))
        {
            Collider[] hits = Physics.OverlapSphere(hitbox.transform.position, hitbox.radius, obstacleLayer | targetLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] != pointBCollider)
                {
                    Explode();
                }
            }
        }
    }
    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(explodeCollider.transform.position, explodeCollider.radius, targetLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].TryGetComponent(out PlayerHealth player))
            {
                player.TakeDamage(damage);
            }
            else if (hits[i].transform.root.TryGetComponent(out Enemy enemy))
            {
                Vector3 shotDir = (enemy.transform.position - transform.position).normalized;
                enemy.TakeDamage(damage,shotDir,2);
            }
        }
        Destroy(gameObject);
    }
}
