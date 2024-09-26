using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WandShooting : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;
    public int shootCost = 10;

    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float nextTimeToFire = 0f;
    [SerializeField] private float fireRate = 0.5f;

    private bool isReloading = false;

    void Start()
    {
        // Initialization if needed
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && HealthSystem.Instance.manaPoint >= shootCost && !isReloading)
        {
            Shoot();
            HealthSystem.Instance.UseMana(shootCost); // Use mana from HealthSystem
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(4f);
        HealthSystem.Instance.RestoreMana(100); // Restore full mana
        Debug.Log("Reload Complete! Mana is now: " + HealthSystem.Instance.manaPoint);
        isReloading = false;
    }

    void Shoot()
    {
        nextTimeToFire = Time.time + fireRate;
        var bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        Destroy(bullet, 5f); // Destroy the bullet after 5 seconds
    }
}

