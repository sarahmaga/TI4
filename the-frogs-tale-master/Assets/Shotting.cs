using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrebab;

    public float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        //Input.GetButtonDown("Fire1")
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
        
    }

    void Shoot()
    {
        print("Atirei");
        //Instantiate(bulletPrebab,transform.position, Quaternion.identity);
        GameObject bullet = Instantiate(bulletPrebab,firePoint.position, firePoint.rotation);

    }
}
