using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WandShooting : MonoBehaviour
{
    public Transform wandspawnpoint;
    public Transform firepoint;
    public GameObject baseBulletPrefab;
    public GameObject explosionBulletPrefab;
    public GameObject iceBulletPrefab;
    public GameObject laserBulletPrefab;
    public GameObject currentBulletPrefab;
    public GameObject currentWandPrefab;

    public GameObject WandBasic;
    public GameObject WandExplosion;
    public GameObject WandIce;
    public GameObject WandLaser;

    private GameObject currentWand;
    public enum SpellType { Basic, Explosion, Ice, Laser}

    [SerializeField] private TMP_Text SpellIndicator;
    [SerializeField]private float nextTimeToFire = 2;
    [SerializeField]private float fireRate = 0.5f;

    //Mana and Reloading
    private bool isReloading = false;
    public int shootCost = 10;
    public int currMana = 100;

    //Stats that can be changed buy NPCs
    public int maxMana = 100;

    public PlayerController PlayerController;

    //Charging for laser weapon
    private bool isCharging = false;
    private float chargeTime = 2f;
    private float chargeAmount = 100f;

    
    void Start()
    {
        ChangeWeapon(1);
        UpdateGunUI();

    }

    // Update is called once per frame
    public void Update()
    {

        if (Input.GetButton("Fire1")&& Time.time >= nextTimeToFire && currMana >= shootCost && !isReloading)
        {
            if (currentBulletPrefab == laserBulletPrefab)
            {
                if (!isCharging)
                {
                    StartCoroutine(ChargeLaser());
                }
                else
                {
                    ShootLaser();
                }
            }
            else if (currentBulletPrefab == baseBulletPrefab)
            {
                Shoot();
            }
            else
            {
                Shoot();
                currMana -= shootCost;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
       
        for (int i = 1; i <= 4; i++)
        {
            // Use Enum.Parse to get the corresponding KeyCode dynamically
            if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)))
            {
                ChangeWeapon(i);
                break; // Exit the loop once the key is found
            }
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(2f);
        currMana = maxMana;
        isReloading = false;
    }


    public void ChangeWeapon(int WeaponIndex)
    {

        // Checks to see if the player has bought the wand, if not returns back to 1
        if (WeaponIndex == 2 && !PlayerController.LaserBought)
        {
            WeaponIndex = 1; // Switch back to weapon 1
        }
        else if (WeaponIndex == 3 && !PlayerController.IceBought)
        {
            WeaponIndex = 1; // Switch back to weapon 1
        }
        else if (WeaponIndex == 4 && !PlayerController.ExplosiveBought)
        {
            WeaponIndex = 1; // Switch back to weapon 1
        }


        if (currentWand != null)
        {
            Destroy(currentWand); // Destroy the previous wand
        }

        switch (WeaponIndex)
        {
            case 1:
                currentBulletPrefab = baseBulletPrefab;
                currentWandPrefab = WandBasic;
                break;
            case 2:
                currentBulletPrefab = explosionBulletPrefab;
                currentWandPrefab = WandExplosion;
                break;
            case 3:
                currentBulletPrefab = iceBulletPrefab;
                currentWandPrefab = WandIce;
                break;
            case 4:
                currentBulletPrefab = laserBulletPrefab;
                currentWandPrefab = WandLaser;
                break;
        }
        UpdateGunUI();

        if(currentWandPrefab != null)
        {
            currentWand = Instantiate(currentWandPrefab, wandspawnpoint.position, wandspawnpoint.rotation);
            currentWand.transform.SetParent(Camera.main.transform);
        }
    }
    void Shoot()
    {
        
        var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
        //Reset damage for regular shots
        bullet.GetComponent<Bullet>().damage = 3; // Set to the default damage value

        var bulletSpeed = bullet.GetComponent<Bullet>().speed;
        bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        nextTimeToFire = Time.time + fireRate;
    }

    IEnumerator ChargeLaser()
    {
        isCharging = true;
        Debug.Log("Charging laser... ");
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;
        ShootLaser();
    }

    void ShootLaser()
    {
        nextTimeToFire = Time.time + fireRate; // Set fire rate after charging
        var laser = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);

        laser.GetComponent<Bullet>().damage *= 2; // Double the damage for chared shot

        var bulletSpeed = laser.GetComponent<Bullet>().speed;
        laser.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        currMana -= shootCost; // Deduct mana cost
        Debug.Log("Laser fired with increased damage!");

    }
    void UpdateGunUI()
    {
        SpellIndicator.text = currentBulletPrefab.GetComponent<Bullet>().bulletName;
    }

}
