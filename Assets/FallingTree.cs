using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : MonoBehaviour
{
public bool triggerDetect;
public GameObject place_to_fall;
public GameObject trigg;
public float rotationWhenFall,roatteSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(triggerDetect)
        {
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,rotationWhenFall),roatteSpeed * Time.deltaTime);

        }
        
    }



}
