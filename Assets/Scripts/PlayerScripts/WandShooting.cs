using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WandShooting : MonoBehaviour
{
    public Transform firepoint;
    public GameObject baseBulletPrefab;
    public GameObject explosionBulletPrefab;
    public GameObject iceBulletPrefab;
    public GameObject laserBulletPrefab;
    public GameObject currentBulletPrefab;
    public enum SpellType { Basic, Explosion, Ice, Laser}

    [SerializeField] private TMP_Text SpellIndicator;
    [SerializeField]private float nextTimeToFire = 2;
    [SerializeField]private float fireRate = 0.5f;

    //Mana and Reloading
    private bool isReloading = false;
    public int shootCost = 10;
    public int maxMana = 100;
    public int currMana = 100;

    
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
            Shoot();
            currMana -= shootCost;
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
      
        switch (WeaponIndex)
        {
            case 1:
                currentBulletPrefab = baseBulletPrefab;
                break;
            case 2:
                currentBulletPrefab = explosionBulletPrefab;
                break;
            case 3:
                currentBulletPrefab = iceBulletPrefab;
                break;
            case 4:
                currentBulletPrefab = laserBulletPrefab;
                break;
        }
        UpdateGunUI();
    }
    void Shoot()
    {
        nextTimeToFire = Time.time + fireRate;
        var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
        var bulletSpeed = bullet.GetComponent<Bullet>().speed;
        bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        

    }
    void UpdateGunUI()
    {
        SpellIndicator.text = currentBulletPrefab.name;
    }


}
