using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandShooting : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;


    [SerializeField]private float bulletspeed = 15;
    [SerializeField] private float nextTimeToFire = 2;
    [SerializeField]private float fireRate = 0.5f;
    
    void Start()
    {
       
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetButton("Fire1")&& Time.time >= nextTimeToFire)
        {
            Shoot();
        }
    }

   
    void Shoot()
    {
        nextTimeToFire = Time.time + fireRate;
        var bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletspeed;
        Destroy(bullet, 5);

    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }

}
