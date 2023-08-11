using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health = 100f;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("dead", false);
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        
        Debug.Log("Enemy Health after being hit: " + health);
        
        if (health <= 0)
        {
            animator.SetBool("dead", true);
            
            enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
