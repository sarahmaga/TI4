using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillOrb : MonoBehaviour
{

    private PlayerPowerUps powerUps;

    // Start is called before the first frame update
    void Start()
    {
        powerUps = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerPowerUps>();

        powerUps.setReceiveMoreHealth(false);
        powerUps.setDoubleMaxHealth(false);
        powerUps.setInflictMoreDamage(false);
        powerUps.setReceiveHalfDamage(false);
        powerUps.setAreaDamage(false);
        powerUps.setRangedAttack(false);

        Debug.Log("Every PowerUp is now set to FALSE.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // get PowerUp randomly

            // options:

            // 1 - receiveMoreHealth;
            // 2 - doubleMaxHealth;
            // 3 - inflictMoreDamage;
            // 4 - receiveHalfDamage;
            // 5 - areaDamage;
            // 6 - rangedAttack.

            int option = Random.Range(1, 7);
            //int option = 6;
            switch (option)
            {
                case 1:
                    powerUps.setReceiveMoreHealth(true); Debug.Log("ReceiveMoreHealth = TRUE");
                    break;
                case 2:
                    powerUps.setDoubleMaxHealth(true); Debug.Log("DoubleMaxHealth = TRUE"); 
                    break;
                case 3:
                    powerUps.setInflictMoreDamage(true); Debug.Log("InflictMoreDamage = TRUE");
                    break;
                case 4:
                    powerUps.setReceiveHalfDamage(true); Debug.Log("ReceiveHalfDamage = TRUE");
                    break;
                case 5:
                    powerUps.setAreaDamage(true); Debug.Log("AreaDamage = TRUE");
                    break;
                case 6:
                    powerUps.setRangedAttack(true); Debug.Log("RangedAttack = TRUE");
                    break;
                default: Debug.Log("NONE ARE TRUE."); break;
            }
            powerUps.setInflictMoreDamage(true);

            Destroy(gameObject);
        }
    }
}
