using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerPowerUps playerPowerUps;
    private const float damage = 50f;

    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    public Transform firePoint;
    public GameObject bulletPrebab;
    public float bulletForce = 20f;

    public void Start()
    {
        playerPowerUps = GetComponent<PlayerPowerUps>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerPowerUps.getRangedAttack() == false)
            {
                Attack();
            }
            else
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
           
        }
    }

    void Attack(){
        animator.SetTrigger("Attack");
        
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        float finalDamage = playerPowerUps.getInflictMoreDamage() ? damage * 2f : damage;
        
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy was hit.");
            enemy.GetComponent<EnemyHealth>().takeDamage(finalDamage);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrebab, firePoint.position, firePoint.rotation);
        animator.SetTrigger("Shoot");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
