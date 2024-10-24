using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public double damage;
    public float lifetime;
    public float speed;
    public double damageBuff;
    public string bulletName;
    [SerializeField] public EventManager EventManger;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnBuyManaUp += UpdateGoldUI; // Subscribe to the regen event
        damage += (damage * (EventManger.damageBuff * 0.55));
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
