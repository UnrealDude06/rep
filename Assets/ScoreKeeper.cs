using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ScoreKeeper : MonoBehaviour
{
    static public int coins;
   // static public int ammo;
    static public float hp = 100;
    
    public TMP_Text HP_Text;
    public TMP_Text coins_counter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ////set UI Text Hp

             HP_Text.SetText (hp.ToString());

             coins_counter.SetText("$ = " + coins.ToString());


    }
}
