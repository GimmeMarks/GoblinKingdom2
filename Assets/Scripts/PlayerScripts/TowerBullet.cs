using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBullet : MonoBehaviour
{
    public double damage;
    public float lifetime;
    public float speed;
    public string bulletName;

    // Start is called before the first frame update
    void Start()
    {

        EventManager.Instance.OnBuyManaUp += UpdateGoldUI; // Subscribe to the regen event
        damage += (damage * (EventManager.Instance.damageBuff * 0.55));
        Destroy(gameObject, lifetime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy")) 
            Destroy(gameObject);
    }
    private void UpdateGoldUI(int amount)
    {

    }
}
