using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform pivotPoint; // The point around which the cannon rotates
    public GameObject bulletPrefab; // The cannonball prefab
    public float shootDelay = 1.0f; // Delay between shots
    public float rotationSpeed = 5.0f; // Speed at which the cannon rotates

    public Collider AtackZone;
    public Transform targetEnemy;
    private bool canShoot = true;

    void Start()
    {

    }

    void Update()
    {
        if (targetEnemy != null)
        {
            // Aim at the enemy
            Vector3 direction = targetEnemy.position - pivotPoint.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            pivotPoint.rotation = Quaternion.Slerp(pivotPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Shoot if ready
            if (canShoot)
            {
                Shoot();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Any detected: " + other.name);

        // Check if the object is an enemy
        if (other.CompareTag("enemy"))
        {
            Debug.Log("Enemy detected: " + other.name);
            targetEnemy = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the object is an enemy
        if (other.CompareTag("enemy"))
        {
            Debug.Log("Enemy detected: " + other.name);
            targetEnemy = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset target when the enemy exits
        if (other.CompareTag("enemy"))
        {
            targetEnemy = null;
        }
    }

    private void Shoot()
    {
        Debug.Log("Cannon fired!");
        canShoot = false;

        // Instantiate the bullet and set its position and direction
        GameObject bullet = Instantiate(bulletPrefab, pivotPoint.position, pivotPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(pivotPoint.forward * 1000f); // Adjust force as needed
        }

        // Start the shoot delay coroutine
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
