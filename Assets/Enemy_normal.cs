using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_normal : MonoBehaviour
{ 
    public enum states {wander,attack};
    public states currentState = states.wander;
    public float wanderRadius;
    public float wanderTimer;
 
    private Transform target;
    private NavMeshAgent agent;
    private float timer;
    public float hp;
    [Header("Finding Player")]
    public bool playerFound;
    public float searchRadius,damageToPlayer,speed;
    GameObject player;
    public bool tooCloseToPlayer;
 
    // Use this for initialization
    void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
    }
 
    // Update is called once per frame
    void Update () {
        if(currentState == states.wander)
        {
            Wander();
            SearchForPlayer();
            if(playerFound)
            {
                currentState = states.attack;
            }
        }   

        if(currentState == states.attack)
        {
            AttackPlayer();
        }
if(hp < 0)
{
    Destroy(this.gameObject);
}
    }

    void Wander()
    {
    timer += Time.deltaTime;
 
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
    
    void SearchForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
     for (int i = 0; i < hitColliders.Length; i++)
     {
         GameObject hitCollider = hitColliders[i].gameObject;
         if (hitCollider.CompareTag("Player"))
         {
             //add damage
           // transform.LookAt(hitCollider.gameObject.transform);
         player =hitCollider.gameObject;
         playerFound=true;
         
         }
     }


    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }


    void AttackPlayer()
    {
        //set a rayacst to know if enemy comes close to player
        RaycastHit hit;
        Physics.Raycast(transform.position,transform.forward,out hit,1f,LayerMask.GetMask("Player"));

        if(hit.collider == player)
        {

            /// do damage
            //attack again after .5 seconds
            Invoke("Do_Damage",0.5f);
            
        }
        else
        {
        
        //face player to attack
      
        }

 if(!tooCloseToPlayer)
        {
              transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0,transform.rotation.y,transform.rotation.z);
        transform.position = Vector3.MoveTowards(this.gameObject.transform.position,player.gameObject.transform.position,speed* Time.deltaTime);
        }

       
    }
    void Do_Damage()
    {
                    ScoreKeeper.hp -= damageToPlayer;

        Invoke("Attack",0.5f);

    }




    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Bullet")
        {
            hp -= 20f;
        }
    }
}