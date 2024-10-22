//Zachary Rhodes
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyBaseClass : MonoBehaviour
{
    //For optimization ideas if things go sour
    //https://docs.unity3d.com/Manual/JobSystem.html

    //Initialization
    public enum EnemyState { March, Chase, Attack}
    protected EnemyState enemyState;
    //private int waveNumber;
    protected Animator anim;
    //Gold Prefab
    [SerializeField] private GameObject goldDrop;

    //Base Stats
    protected int eMaxHealth;
    [SerializeField] protected int eHealth;
    [SerializeField] protected int eSpeed;
    [SerializeField] protected int eDamage;
    [SerializeField] protected string eName;
    [SerializeField] protected float eAttackSpeed;

    //March info
    protected NavMeshAgent agent;
    public Transform baseTransform1 { get; set; }
    public Transform baseTransform2 { get; set; }

    //Chase info
    [SerializeField] protected float chaseDistance;
    [SerializeField] protected float lingerDistance;
    public Transform playerTransform;

    //Attack info
    [SerializeField] protected float attackDistance;


    void Start()
    {
        //Intitializing agents calling methods. 
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = eSpeed;
        enemyState = EnemyState.March;
        ChangeState(enemyState);
        eMaxHealth = eHealth;
        
    }
 
    private void OnDrawGizmos()
    {
        //Visuals for different ranges
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lingerDistance);

    }


    void ChangeState(EnemyState newState)
    {
        //State Machine
        StopAllCoroutines();
        enemyState = newState;

        switch (enemyState)
        {
            case EnemyState.March:
                StartCoroutine("AI_March");
                break;
            case EnemyState.Chase:
                StartCoroutine("AI_Chase");
                break;
            case EnemyState.Attack:
                StartCoroutine("AI_Attack");
                break;
        }
    }

    //Default state, march to the base
    IEnumerator AI_March()
    {

       
        print("March Beginning!");
        //Depending on which base is closer, target the closest option
        if (Vector3.Distance(baseTransform1.position, transform.position) < Vector3.Distance(baseTransform2.position, transform.position))
            agent.SetDestination(baseTransform1.position);
        else
            agent.SetDestination(baseTransform2.position);

        while (true)
        {
            //If player is in range, chase!
            if (Vector3.Distance(playerTransform.position, transform.position) < chaseDistance)
            {
                ChangeState(EnemyState.Chase);
                yield break;
            }

            //Continuously update the destination while marching
            if (agent.remainingDistance < 0.5f)
            {
                if (Vector3.Distance(baseTransform1.position, transform.position) < Vector3.Distance(baseTransform2.position, transform.position))
                    agent.SetDestination(baseTransform1.position);
                else
                    agent.SetDestination(baseTransform2.position);
            }

            yield return null;
        }

    }
    //Chase behavior
    IEnumerator AI_Chase()
    {
        Debug.Log("Now Chasing!");
        agent.isStopped = false;

        while (true)
        {
            agent.SetDestination(playerTransform.position);
            //if agent is in attack distance, attack!
            if(Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
            {
                agent.isStopped = true;
                ChangeState(EnemyState.Attack);
                yield break;
            }else if(Vector3.Distance(transform.position, playerTransform.position) > lingerDistance)
            {
                Debug.Log("Whatever, you're not worth it");
                ChangeState(EnemyState.March);
                yield break;
            }
            yield return null;
        }
    }
    //enemy attacking behavior
    IEnumerator AI_Attack()
    {
        Debug.Log("Attacking, RAH!");
        float elapsedTime = 0f;
        while (true)
        {
            

            if(Vector3.Distance(transform.position, playerTransform.position) > attackDistance)
            {
                
                Debug.Log("Get back here!");
                ChangeState(EnemyState.Chase);
                yield break;
            }
            elapsedTime += Time.deltaTime;
           
            //if time passed > attack speed, attack
            if (elapsedTime >= eAttackSpeed)
            {
                PlayerController.Instance.TakeDamage(eDamage);
                Debug.Log(name + "Attacking for " + eDamage);

                elapsedTime = 0f;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);

            }
        }

    }
    //If taking damage and health drops below zero, destroy and drop gold
    void TakeDamage(double damage)
    {

        eHealth -= (int)damage;
        if (eHealth <= 0)
        {
            dropGold(CalcGold());
            Destroy(gameObject);
        }
    }
   
    int CalcGold()
    {
        float rawGold = eMaxHealth * 0.1f;
        int prettyGold = Mathf.RoundToInt(rawGold);
        return prettyGold;
    }
    void dropGold(int gold)
    {
        GameObject goldInstance = Instantiate(goldDrop, transform.position, Quaternion.identity);
        goldScript goldScript = goldInstance.GetComponent<goldScript>();
        goldScript.setGold(gold);
    }
}
