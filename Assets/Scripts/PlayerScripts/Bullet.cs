using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]public int damage;
    [SerializeField] public float lifetime;
    [SerializeField]public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy")) 
            Destroy(gameObject);
    }

}
