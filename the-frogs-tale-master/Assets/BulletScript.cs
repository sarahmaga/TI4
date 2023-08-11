using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float projectileSpeed = 15f;
    private Rigidbody2D rigidbody;
    private const float damage = 50f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * projectileSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<EnemyHealth>().takeDamage(damage);
        Debug.Log("Shoot enemy.");
        Destroy(gameObject);

    }

}
