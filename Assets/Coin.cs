using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
public Transform player;
public float dist,coinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0,100 * Time.deltaTime,0);
        if(Vector3.Distance(transform.position, player.position) < dist)
{
      transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * coinSpeed);
}

    }
}
