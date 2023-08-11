using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtack : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;

    public Animator animator;

    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        damage = 40f;
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();

            playerHealth.takeDamage(damage);
            Debug.Log("BasicEnemy Damage: " + damage);

            Destroy(gameObject);
        }
    }

    void Attack()
    {
        //animator.SetTrigger("Run");
    }
}
