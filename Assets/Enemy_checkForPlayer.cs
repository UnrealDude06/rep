using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_checkForPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            GetComponentInParent<Enemy_normal>().tooCloseToPlayer = true;
        }
        else
        { GetComponentInParent<Enemy_normal>().tooCloseToPlayer = false;}
    }
}
