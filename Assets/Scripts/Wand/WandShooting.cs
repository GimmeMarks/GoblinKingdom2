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

    public AudioClip basicWandSound;
    public AudioClip explosionWandSound;
    public AudioClip laserWandSound;
    public AudioClip iceWandSound;
    public AudioClip ManaRegen;

    public AudioSource audioSource;


    private GameObject currentWand;
    private int currentWeaponIndex = 0;  // Start with an invalid index (0 means no weapon selected)
    public enum SpellType { Basic, Explosion, Ice, Laser }

    public TMP_Text SpellIndicator;

    private float nextTimeToFire = 0;

    // Laser weapon variables
    private bool isCharging = false;
    public float chargeTime;

    // Explosion weapon variables
    private float explosionRadius = 5.0f;
    private float explosionForce = 300.0F;
    private float timeDelay = 1.5F;

    // Mana and Reloading
    public int currMana;
    // Stats that can be changed by NPCs
    public int maxMana = 100;

    // Weapon Costs
    public int BasicShootCost;
    public int LaserShootCost;
    public int IceShootCost;
    public int ExplosionShootCost;

    public float BasicFireRate;
    public float LaserFireRate; // + chargeTime Delay
    public float IceFireRate;
    public float ExplosionFireRate;

    //Controls if player is in tutorial
    public bool canShoot = true;

    public PlayerController PlayerController;

    public int regen;
    public float regenDelay;
    public float regenUpdateInterval;
    private bool inMenuLocal;
    public bool GodMode;

    // Track which weapons have been bought
    public bool BasicBought = true;
    public bool LaserBought = false;
    public bool IceBought = false;
    public bool ExplosionBought = false;

    void Start()
    {

        // Initially set the weapon based on what the player has bought
        UpdateWeaponList();
        if (currentWeaponIndex > 0)
        {
            ChangeWeapon(currentWeaponIndex); // Set to the first available weapon
        }
        UpdateGunUI();
        regenDelay = regenUpdateInterval;
    }

    // Update is called once per frame
    public void Update()
    {
        inMenuLocal = PlayerController.Instance.inMenu;

        Regen();

        // Check for weapon switch inputs (scroll with 1 and 2)
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchWeapon(-1);  // Scroll backward through available weapons
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchWeapon(1);  // Scroll forward through available weapons
        }

        // Continue the rest of your shooting logic as is
        if (canShoot == true)
        {
            HandleShooting();
        }
    }

    // Controls if the player can shoot or naw
    public void ToggleShootOn()
    {
        canShoot = true;
    }
    public void ToggleShootOff()
    {
        canShoot = false;
    }

    void Regen()
    {
        regenDelay -= Time.deltaTime;

        if (regenDelay <= 0.0) // Interval ended - update health & mana and start new interval
        {
            if (GodMode)
            {
                RestoreMana(maxMana);
            }
            else
            {
                RestoreMana(regen);
                if (currMana < maxMana)
                PlaySound(ManaRegen);
            }
            regenDelay = regenUpdateInterval;
        }
    }

    void RestoreMana(int Mana)
    {
        currMana += Mana;
        if (currMana > maxMana)
            currMana = maxMana;
    }

    void HandleShooting()
    {
        //Handles Laser Shoot
        if (currentBulletPrefab == laserBulletPrefab)
        {
            if (Input.GetButton("Fire1") && currMana >= LaserShootCost && !inMenuLocal && !PlayerController.inConversation == true)
            {
                if (!isCharging)
                {
                    currMana -= LaserShootCost;
                    StartCoroutine(ChargeLaser());
                }
            }
        }
        //Handles Explosion Shoot
        else if (currentBulletPrefab == explosionBulletPrefab)
        {
            if (Input.GetButton("Fire1") && currMana >= ExplosionShootCost && !inMenuLocal && !PlayerController.inConversation == true)
            {
                if (!isCharging)
                {
                    currMana -= ExplosionShootCost;
                    Shoot();
                    PlaySound(explosionWandSound);
                }
            }
        }
        else if (currentBulletPrefab == iceBulletPrefab)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !inMenuLocal && !PlayerController.inConversation == true)
            {
                if (currMana >= IceShootCost)
                {
                    Shoot();
                    currMana -= IceShootCost;
                    PlaySound(iceWandSound);
                }

            }
        }
        else if (currentBulletPrefab == baseBulletPrefab)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !inMenuLocal && !PlayerController.inConversation == true)
            {
                Shoot();
                currMana -= BasicShootCost;
                PlaySound(basicWandSound);
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator ChargeLaser()
    {
        isCharging = true;
        Debug.Log("Charging laser... ");
        PlaySound(laserWandSound);
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;

        if (isCharging == false)
        {
            ShootLaser();
        }
    }

    void ChangeWeapon(int WeaponIndex)
    {
        // Only allow changing to a weapon if it's been bought
        if (WeaponIndex == 1 && BasicBought)
        {
            currentBulletPrefab = baseBulletPrefab;
            currentWandPrefab = WandBasic;
        }
        else if (WeaponIndex == 2 && LaserBought)
        {
            currentBulletPrefab = laserBulletPrefab;
            currentWandPrefab = WandLaser;
        }
        else if (WeaponIndex == 3 && IceBought)
        {
            currentBulletPrefab = iceBulletPrefab;
            currentWandPrefab = WandIce;
        }
        else if (WeaponIndex == 4 && ExplosionBought)
        {
            currentBulletPrefab = explosionBulletPrefab;
            currentWandPrefab = WandExplosion;
        }

        UpdateGunUI();

        if (currentWandPrefab != null)
        {
            if (currentWand != null) Destroy(currentWand); // Destroy previous wand if any
            currentWand = Instantiate(currentWandPrefab, wandspawnpoint.position, wandspawnpoint.rotation);
            currentWand.transform.SetParent(Camera.main.transform);
        }
    }

    void SwitchWeapon(int direction)
    {
        // Move to the next or previous available weapon in the list
        int weaponCount = 0;
        List<int> availableWeapons = new List<int>();

        // Create a list of all available weapons
        if (BasicBought) availableWeapons.Add(1);
        if (LaserBought) availableWeapons.Add(2);
        if (IceBought) availableWeapons.Add(3);
        if (ExplosionBought) availableWeapons.Add(4);

        // Get the current index in the list
        weaponCount = availableWeapons.Count;

        if (weaponCount == 0) return; // If no weapons are available, do nothing

        // Get the index of the currently selected weapon
        int currentIndex = availableWeapons.IndexOf(currentWeaponIndex);

        // Calculate the new index by adding the direction
        currentIndex = (currentIndex + direction + weaponCount) % weaponCount; // Ensures wrapping

        // Set the currentWeaponIndex to the new weapon
        currentWeaponIndex = availableWeapons[currentIndex];

        // Change the weapon
        ChangeWeapon(currentWeaponIndex);
    }

    void Shoot()
    {
        if (currentBulletPrefab == explosionBulletPrefab)
        {
            StartCoroutine(Explode(firepoint.position));

            var bullet = Instantiate(explosionBulletPrefab, firepoint.position, firepoint.rotation);
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;

            nextTimeToFire = Time.time + ExplosionFireRate;
        }
        else if (currentBulletPrefab == iceBulletPrefab)
        {
            var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;

            nextTimeToFire = Time.time + IceFireRate;
        }
        else
        {
            var bullet = Instantiate(currentBulletPrefab, firepoint.position, firepoint.rotation);
            var bulletSpeed = bullet.GetComponent<Bullet>().speed;
            bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;

            nextTimeToFire = Time.time + BasicFireRate;
        }
    }

    IEnumerator Explode(Vector3 explosionBulletPrefab)
    {
        yield return new WaitForSeconds(timeDelay);

        Collider[] colliders = Physics.OverlapSphere(explosionBulletPrefab, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionDirection = hit.transform.position - explosionBulletPrefab;
                rb.AddExplosionForce(explosionForce, explosionBulletPrefab, explosionRadius);
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
        Debug.Log("Laser Fired!");
    }

    void UpdateGunUI()
    {

        SpellIndicator.text = currentBulletPrefab.GetComponent<Bullet>().bulletName;

    }

    void UpdateWeaponList()
    {
        // Set the available weapons based on what the player has bought
        if (BasicBought) currentWeaponIndex = 1;
        else if (LaserBought) currentWeaponIndex = 2;
        else if (IceBought) currentWeaponIndex = 3;
        else if (ExplosionBought) currentWeaponIndex = 4;
        else currentWeaponIndex = 0; // No weapons bought
    }
}
