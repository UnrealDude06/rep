using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour
{
  
bool pl = false;

    void Update()
    {

Invoke("nice",1f);
if (GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying == false && pl)
{
//Video Finished.
SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
}
 
    }
    void nice()
    {
        pl = true;
    }
}
