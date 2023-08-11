using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private bool receiveMoreHealth;
    private bool doubleMaxHealth;

    private bool inflictMoreDamage;
    private bool receiveHalfDamage;

    private bool areaDamage;
    private bool rangedAttack;
    private const float time = 20f;
    private float timerToReset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        receiveMoreHealth = false;
        doubleMaxHealth = false;

        inflictMoreDamage = false;
        receiveHalfDamage = false;
        rangedAttack = false;
        areaDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerToReset <= 0f)
        {
            receiveMoreHealth = false;

            doubleMaxHealth = false;
            playerHealth.normalMaxHealth();
            
            inflictMoreDamage = false;
            receiveHalfDamage = false;

            areaDamage = false;
            rangedAttack = false;

            // reset timer

            timerToReset = time;
        }
        else
        {
            timerToReset -= Time.deltaTime;
        }
    }
    public bool getReceiveMoreHealth()
    {
        return receiveMoreHealth;
    }

    public void setReceiveMoreHealth(bool receiveMoreHealth)
    {
        this.receiveMoreHealth = receiveMoreHealth;
    }
    public bool getDoubleMaxHealth() { return doubleMaxHealth; }

    public void setDoubleMaxHealth(bool doubleMaxHealth)
    {
        this.doubleMaxHealth = doubleMaxHealth;
        if (doubleMaxHealth)
            playerHealth.doubleMaxHealth();
        else
            playerHealth.normalMaxHealth();
    }

    public bool getInflictMoreDamage() { return inflictMoreDamage; }

    public void setInflictMoreDamage(bool inflictMoreDamage)
    {
        this.inflictMoreDamage = inflictMoreDamage;
    }

    public bool getReceiveHalfDamage() { return receiveHalfDamage; }

    public void setReceiveHalfDamage(bool receiveHalfDamage)
    {
        this.receiveHalfDamage = receiveHalfDamage;
    }

    public bool getAreaDamage() { return areaDamage; }

    public void setAreaDamage(bool areaDamage)
    {
        this.areaDamage = areaDamage;
    }
    public void setRangedAttack(bool rangedAttack)
    {
        this.rangedAttack = rangedAttack;
    }
    public bool getRangedAttack() { return rangedAttack; }
}
