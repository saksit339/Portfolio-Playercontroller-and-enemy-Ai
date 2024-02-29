using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonBehaviour : MonoBehaviour
{

    public bool isDead = false;
    private bool isSeeplayer = false;
    private GameObject player;
    [SerializeField]
    private float seeDistance = 20f;
    private bool isAttack;
    private bool attackReady = true;
    private float distance;
    [SerializeField]
    private bool activeGizmos = false;
    [SerializeField]
    private float dragonCurrentHP = 100;


    [SerializeField]
    private float attackDistance = 5;
    
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private AudioSource attackSound;
    [SerializeField]
    private Animator dragonAnimator;


    private void Start()
    {
        player = GameObject.Find("player");
        
    }

    private void Update()
    {
        if(isDead)
        {
            agent.SetDestination(transform.position);
            Destroy(gameObject,10);
            dragonAnimator.SetBool("isDead",true);
        }
        else
        {
            if (dragonCurrentHP <= 0)
            {
                isDead = true;
            }

            //distance calculate
            
            distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= seeDistance)
            {
                isSeeplayer = true;
            }


            if (isSeeplayer)
            {
                if (distance <= attackDistance)
                {
                    if(attackReady)
                    {
                        StartCoroutine("MeleeAtteck");
                    }
                    
                }
                else
                {
                    if (!isAttack)
                    {
                        agent.SetDestination(player.transform.position);
                        
                    }

                }
            }
        }
        dragonAnimator.SetBool("isWalk", !isAttack);
        dragonAnimator.SetBool("isAttack", isAttack);
        dragonAnimator.SetBool("isSeePlayer", isSeeplayer);
    }

    IEnumerator MeleeAtteck()
    {
        isAttack = true;
        attackReady = false;
        agent.SetDestination(transform.position);
        //play animetion attack
        


        yield return new WaitForSeconds(fireRate);
            
        if(distance <= attackDistance + 2)
        {
            //player take damage
            player.GetComponent<PlayerStatus>().PlayerTakeDamage(20);
            attackSound.Play();

            Debug.Log("attack");
        }

        isAttack = false;
        attackReady = true;
    }

    public void TakeDamage(float damage)
    {
        dragonCurrentHP -= damage;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(15);
            Destroy(other.gameObject);
        }
    }

    





    private void OnDrawGizmos()
    {
        if(activeGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, seeDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackDistance);

            Gizmos.color = Color.red;
            if (player != null)
            {
                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
        
    }



}
