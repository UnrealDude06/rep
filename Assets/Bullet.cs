using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float bulletSpeed,radius;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 forwardVector = transform.forward;
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
     for (int i = 0; i < hitColliders.Length; i++)
     {
         GameObject hitCollider = hitColliders[i].gameObject;
         if (hitCollider.CompareTag("Enemy"))
         {
             //add damage
            transform.LookAt(hitCollider.gameObject.transform);
         }
     }
        rb.AddForce(transform.forward * bulletSpeed*Time.deltaTime,ForceMode.Impulse);
        StartCoroutine(dead(2f));



    }
    IEnumerator dead(float time)
    {
        yield return new WaitForSeconds (time);
        Destroy(this.gameObject);
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
          //  Destroy(col.gameObject);
            Destroy(this.gameObject);
        }
    }

     void OnDrawGizmosSelected ()
 {
     Gizmos.color = Color.white;
     Gizmos.DrawWireSphere (transform.position, radius);
 }
}
