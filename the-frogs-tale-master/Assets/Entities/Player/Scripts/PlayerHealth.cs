using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private PlayerPowerUps playerPowerUps;
    public Animator animator;

    public Slider healthBar;
    private float maxHealth = 100f;
    private float health;

    public GameObject gameOver;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        playerPowerUps = GetComponent<PlayerPowerUps>();

        health = maxHealth;
        healthBar.value = health/100;

        Debug.Log("Initial MaxHealth: " + maxHealth);
        Debug.Log("Initial Health: " + health);

        animator.SetFloat("Health", Mathf.Abs(health));
    }

    /*void Update()
    {
        animator.SetFloat("Health", Mathf.Abs(health));

        if (health <= 0f)
        {
            timer += Time.deltaTime;
            if(timer > 2) {
                animator.SetFloat("Health", Mathf.Abs(health));
                Destroy(gameObject);
            }
        }
    }*/

    public void updateBar()
    {
        healthBar.value = health/100;
    }

    public void takeDamage(float damage)
    {
        health -= playerPowerUps.getReceiveHalfDamage() ? damage / 2f : damage;
        animator.SetFloat("Health", Mathf.Abs(health));
        
        // healthBar.value = health/100;
        updateBar();
        Debug.Log("Health after taking damage: " + health);

        if (health <= 0f)
        {
            animator.SetFloat("Health", Mathf.Abs(health));
            // Destroy(gameObject, 2f);

            //while (timer < 1f) {
            //    timer += Time.deltaTime;
            //}
            Destroy(gameObject, 2f);
            
            Time.timeScale = 0f;
            gameOver.SetActive(true);
            
        }
    }

    public void regenerate(float healthPoints)
    {
        // if power-up for extra regen is active, multiply healthPoints by a constant

        Debug.Log("playerPowerUps.getReceiveMoreHealth(): " + playerPowerUps.getReceiveMoreHealth());

        float healthToRegen = playerPowerUps.getReceiveMoreHealth() ? healthPoints * 2f : healthPoints;

        Debug.Log("Health BEFORE regenerating: " + health);

        health = (health + healthToRegen >= maxHealth) ? maxHealth : health + healthToRegen;
        
        // healthBar.value = health/100;
        updateBar();
        Debug.Log("Health AFTER regenerating: " + health);
    }

    public void doubleMaxHealth()
    {
        maxHealth = 200f;
        Debug.Log("UPDATED MAX HEALTH TO DOUBLE");
    }

    public void normalMaxHealth()
    {
        maxHealth = 100f;
        if (health > 100f) health = 100f;

        Debug.Log("UPDATED MAX HEALTH TO NORMAL");
        Debug.Log("Health is now " + health);
    }
}
