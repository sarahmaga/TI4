using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 2.5f;
    [SerializeField] float lifeTime = 3f;

    private Transform target;
    private Vector2 targetPos;

    private PlayerHealth targetHealth;

    private const float damage = 50f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetHealth = target.GetComponent<PlayerHealth>();

        targetPos = new Vector2(target.position.x, target.position.y);

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetHealth.takeDamage(damage);

            Debug.Log("RangedEnemy Base Damage: " + damage);
            Destroy(gameObject);
        }
    }
}
