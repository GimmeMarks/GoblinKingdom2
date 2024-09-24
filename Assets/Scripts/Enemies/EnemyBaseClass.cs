//Zachary Rhodes
using System.Collections;
using System.Collections.Generic;
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
            yield return null;
        }
    }

    IEnumerator AI_Chase()
    {

        while (true)
        {
            
            if(Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
            {
                ChangeState(EnemyState.Attack);
                yield break;
            }else if(Vector3.Distance(transform.position, playerTransform.position) > lingerDistance)
            {
                ChangeState(EnemyState.March);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator AI_Attack()
    {
        float elapsedTime = 0f;
        while (true)
        {

            if(Vector3.Distance(transform.position, playerTransform.position) > attackDistance)
            {
                ChangeState(EnemyState.Chase);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= recoverTime)
            {
                
                Debug.Log(name + "Attacking!");
                elapsedTime = 0f;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    void dropGold()
    {

    }
}
