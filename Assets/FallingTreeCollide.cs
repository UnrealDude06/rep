using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTreeCollide : MonoBehaviour
{
    public FallingTree tree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            tree.triggerDetect = true;
            Destroy(this.gameObject);
        }
    }
}
