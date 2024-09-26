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
    private int waveNumber;
    protected Animator anim;
    
    

    //Base Stats
    [SerializeField] protected int eHealth;
    [SerializeField] protected int eSpeed;
    [SerializeField] protected int eDamage;
    [SerializeField] protected string eName;
    [SerializeField] protected float eAttackSpeed;

    //March info
    protected NavMeshAgent agent;
    [SerializeField] protected Transform baseTransform1;
    [SerializeField] protected Transform baseTransform2;

    //Chase info
    [SerializeField] protected float chaseDistance;
    [SerializeField] protected float lingerDistance;
    public Transform playerTransform;

    //Attack info
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float recoverTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = eSpeed;
        enemyState = EnemyState.March;
        ChangeState(enemyState);
        Debug.Log(playerTransform.position + "Player is not null!");
        
    }
    private void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lingerDistance);

    }


    void ChangeState(EnemyState newState)
    {
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

    IEnumerator AI_March()
    {

        print("March Beginning!");
        if (Vector3.Distance(baseTransform1.position, transform.position) < Vector3.Distance(baseTransform2.position, transform.position))
            agent.SetDestination(baseTransform1.position);
        else
            agent.SetDestination(baseTransform2.position);

        while (true)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < chaseDistance)
            {
                ChangeState(EnemyState.Chase);
                yield break;
            }

            // Continuously update the destination while marching
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

    IEnumerator AI_Chase()
    {
        Debug.Log("Now Chasing!");
        agent.isStopped = false;

        while (true)
        {
            agent.SetDestination(playerTransform.position);
            
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
           
            if (elapsedTime >= recoverTime)
            {
                PlayerController.Instance.TakeDamage(eDamage);
                Debug.Log(name + "Attacking for " + eDamage);

                elapsedTime = 0f;
            }
            yield return null;
        }
    }
    void dropGold()
    {

    }
    public void TakeDamage(int damage)
    {
        eHealth -= damage;

        if (eHealth <= 0)
        {
            // Handle enemy death (e.g., destroy the enemy)
            Destroy(gameObject);
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
}
