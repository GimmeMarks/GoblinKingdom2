using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WandShooting : MonoBehaviour
{
    // Variables
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
    public enum SpellType { Basic, Explosion, Ice, Laser }

    [SerializeField] private TMP_Text SpellIndicator;

    [SerializeField] private float nextTimeToFire = 2;
    [SerializeField] private float fireRate = 0.5f;

    // Laser weapon variables
    private bool isCharging = false;
    public float chargeTime = 1f;

    // Explosion weapon variables
    private float explosionRadius = 5.0f;
    private float explosionForce = 300.0F;
    private float timeDelay = 1.5F;

    // Mana and Reloading
    private bool isReloading = false;
    public int shootCost = 10;
    public int currMana = 100;

    // Stats that can be changed by NPCs
    public int maxMana = 100;

    public PlayerController PlayerController;

    public bool Regenerate = true;
    public int regen = 20;
    private float timeleft = 0.0f;
    public float regenUpdateInterval = 0.2f;
    private bool inMenuLocal;
    public bool GodMode;

    // Save for later
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
        inMenuLocal = PlayerController.Instance.inMenu;
        // Check if the current weapon is the laser weapon
        if (currentBulletPrefab == laserBulletPrefab)
        {
            // Use right-click for laser
            if (Input.GetButton("Fire2") && currMana >= shootCost && !isReloading && !inMenuLocal && !PlayerController.inConversation == true)
            {
                if (!isCharging)
                {
                    StartCoroutine(ChargeLaser());
                }
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !inMenuLocal && !PlayerController.inConversation == true)
            {
                if (currentBulletPrefab == baseBulletPrefab)
                    Shoot();
                else
                {
                    if (currMana >= shootCost)
                    {
                        Shoot();
                        currMana -= shootCost;
                    }
                }
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
    }

    void Regen()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0) // Interval ended - update health & mana and start new interval
        {
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

    void RestoreMana(int Mana)
    {
        currMana += Mana;
        if (currMana > maxMana)
            currMana = maxMana;
    }

    IEnumerator ChargeLaser()
    {
        isCharging = true;
        Debug.Log("Charging laser... ");
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;

        if (isCharging == false)
        {
            ShootLaser();
        }
    }

    void ChangeWeapon(int WeaponIndex)
    {
        // Checks to see if the player has bought the wand, if not returns back to 1
        if (WeaponIndex == 2 && !PlayerController.LaserBought)
        {
            WeaponIndex = 1;
        }
        else if (WeaponIndex == 3 && !PlayerController.IceBought)
        {
            WeaponIndex = 1;
        }
        else if (WeaponIndex == 4 && !PlayerController.ExplosiveBought)
        {
            WeaponIndex = 1;
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
                currentBulletPrefab = laserBulletPrefab;
                currentWandPrefab = WandLaser;
                break;
            case 3:
                currentBulletPrefab = iceBulletPrefab;
                currentWandPrefab = WandIce;
                break;
            case 4:
                currentBulletPrefab = explosionBulletPrefab;
                currentWandPrefab = WandExplosion;
                break;
        }
        UpdateGunUI();

        if (currentWandPrefab != null)
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
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        }
        else
        {
            var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        }
    }

    IEnumerator Explode(Vector3 explosionPosition)
    {
        yield return new WaitForSeconds(timeDelay);

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionDirection = hit.transform.position - explosionPosition;
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
            Debug.Log("EXPLOSION!!");
            if (hit is SphereCollider sphereCollider)
            {
                sphereCollider.radius *= 10f;
            }
        }
    }

    void ShootLaser()
    {
        var laser = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
        var bulletSpeed = laser.GetComponent<Bullet>().speed;
        laser.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        currMana -= shootCost;
        Debug.Log("Laser fired with increased damage!");
    }

    void UpdateGunUI()
    {
        SpellIndicator.text = currentBulletPrefab.GetComponent<Bullet>().bulletName;
    }
}
