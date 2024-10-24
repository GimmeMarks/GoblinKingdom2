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

    //Laser weapon variables
    [SerializeField] private float laserFireRate = 0.1f; // Faster fire rate for laser
    private float laserNextTimeToFire = 0; // Separate next time to fire for laser

    //Explosion weapon variables
    private float explosionRadius = 5.0f;
    private float explosionForce = 300.0F;
    //private float upwardsModifier = 2.0F;
    private float timeDelay = 1.5F;


    //Mana and Reloading
    private bool isReloading = false;
    public int shootCost = 10;

    public int maxMana = 100;
    public int currMana = 100;

    public bool Regenerate = true;
    public int regen = 1;
    private float timeleft = 0.0f;  // Left time for current interval
    public float regenUpdateInterval = 5f;

    public bool GodMode;

    //Charging for laser weapon
    private bool isCharging = false;
    private float chargeTime = 2f;
    private float chargeAmount = 100f;

    
    void Start()
    {
        ChangeWeapon(1);
        UpdateGunUI();
        timeleft = regenUpdateInterval;
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if the current weapon is the laser weapon
        if (currentBulletPrefab == laserBulletPrefab)
        {
            // Use right-click for laser
            if (Input.GetButton("Fire2") && Time.time >= laserNextTimeToFire && currMana >= shootCost && !isReloading)
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
        }
        else
        {
            // Use left-click for other weapons
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currMana >= shootCost && !isReloading)
            {
                Shoot();
                currMana -= shootCost;
            }
        }

        // Reload functionality
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }

        // Weapon switching
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)))
            {
                ChangeWeapon(i);
                break; // Exit the loop once the key is found
            }
        }

        // Mana regeneration
        if (Regenerate)
            Regen();
    }


    private void Regen()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0) // Interval ended - update health & mana and start new interval
        {
            //Debug.Log("Mana Restore");
            if (GodMode)
            {
                RestoreMana(maxMana);
            }
            else
            { 
                RestoreMana(regen);
            }

            timeleft = regenUpdateInterval;
        }
    }

    public void RestoreMana(int Mana)
    {
        currMana += Mana;
        if (currMana > maxMana)
            currMana = maxMana;

      
    }


    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(2f);
        currMana = maxMana;
        isReloading = false;
    }

    IEnumerator ChargeLaser()
    {
        isCharging = true;
        Debug.Log("Charging laser... ");
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;
       

        if (Time.time >= laserNextTimeToFire)
        {
            ShootLaser();
        }
    }

    public void ChangeWeapon(int WeaponIndex)
    {

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
        nextTimeToFire = Time.time + fireRate;
        
        if (currentBulletPrefab == explosionBulletPrefab)
        {
            StartCoroutine(Explode(firepoint.position));
            var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
            //Reset damage for regular shots
            bullet.GetComponent<Bullet>(); // Set to the default damage value
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        }
        else
        {
        
            var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
            //Reset damage for regular shots
            bullet.GetComponent<Bullet>(); // Set to the default damage value
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        }
    }


    private IEnumerator Explode(Vector3 explosionPosition)
    {
        // Wait for the explosion delay before applying damage
        yield return new WaitForSeconds(timeDelay);

        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // Optionally, you can apply a force to the rigidbody if the object has one
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionDirection = hit.transform.position - explosionPosition;
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
            Debug.Log("EXPLOSION!!");
            if (hit is SphereCollider sphereCollider)
            {
                // Modify radius here (for example, double the radius)
                sphereCollider.radius *= 10f; // Adjust this value as needed
            }
        }
    }

        void ShootLaser()
    {
        laserNextTimeToFire = Time.time + laserFireRate; // Set fire rate after charging
        var laser = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);

        laser.GetComponent<Bullet>().damage *= 2; // Double the damage for chared shot

        var bulletSpeed = laser.GetComponent<Bullet>().speed;
        laser.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        currMana -= shootCost; // Deduct mana cost
        Debug.Log("Laser fired with increased damage!");

    }
    void UpdateGunUI()
    {
        SpellIndicator.text = currentBulletPrefab.name;
    }


}
