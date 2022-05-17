using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionAlignmentOnSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
         GameObject player = GameObject.FindGameObjectWithTag("Player");
         player.transform.position = this.transform.position;
         Camera.main.GetComponent<Camera_controller>().focus = player.gameObject.transform;
    }
}
