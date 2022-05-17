using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;
    public Animator  anim;
    public bool dead;
    [Header("Movement")]
    public float speed;
    public float acc,deacc,maxSpeed,acc_downhill,turnSpeed,groundSlopeAngle,SlopesRotationSpeed;
     public float turnSpeedHigh;
    public float turnSpeedLow;

        float horizontalAxis;
    float verticalAxis,offset_distance;
    
    [Header("Jumping")]
    public bool isGrounded;
    public float jumpSpeed,jumpShortSpeed,gravity;
    bool jump,jumpCancel;
    [Header("Speed stone")]
    public float speed_stone_maxSpeed;
    public bool speed_stone;
    [Header("Lighting Stone")]
    public bool stone3;
    public bool lignting_dash;
    public GameObject lightning_dash_object;
    public float ligtning_speed;
    public bool canDoDash;
    public Camera camera_control;
    

    [Header("Wall Jump")]
    public float WallJumpSpeed;
    public float wallRunGravity,walljumpForce;
    RaycastHit hit,wallL,wallR;
    public bool WallSliding,WallJump,wallLeft,wallRight;
            RaycastHit wallHitRight,wallHitLeft;
        public float cam_tilt,camera_tilt_speed;
        float original_gravity;

    [Header("Shooting")]
    public GameObject bullet;
    public GameObject armShootPoint;
    public bool reload;
    public int ammo;
    float timer = 0.1f;
     float reloadTimer;
    public float reloadTimer_start;
     int start_ammo;
     public ParticleSystem muzzleFlash;

    [Header("Allowing")]
    public bool stopAccelerating;
    public bool canControl,stopJumping,stopRotating;

    [Header("Dash")]
    public bool dash;bool candash;
    public float DashSpeed;
    public ParticleSystem dash_particle;
    [Header("Glide")]
    public bool gliding;
    public float glideSpeed;
    public float glideAcc,glideDeacc,glideMaxSpeed,glideHoriz_speed,glide_rotationSpeed,glide_jumps;
    [Header("Recoil")]
    public float RecoilUp;
    public float recoilSpeed;

    [Header("Environment")]
    public bool GoingUpVine;
    GameObject vine;

    public GameObject Talk_Panel;



    //////useless or temporary floats and vectors
    Vector3 movement,input,wallRunVec,glide_Vector,groundSlopeDir,forward,right;
    Quaternion slope;
    float normal_deacc;
     Vector3 localMove; 
     float normal_maxSpeed;
     public GameObject scoreController; 

    // Start is called before the first frame update
    void Start()
    {
        candash = true;
        normal_maxSpeed=maxSpeed;
         normal_deacc = deacc;
        reloadTimer = reloadTimer_start;
        start_ammo = ammo;   
        original_gravity = gravity;    
        if(scoreController == null)
        {
            scoreController = GameObject.FindGameObjectWithTag("Score");
        }
    }

    // Update is called once per frame
    void Update()
    {
                 RaycastHit hit_ground;

        if (Physics.Raycast(transform.position, -transform.up, out hit_ground, 2f, LayerMask.GetMask("Default")))
        { //1 for the distance orignally
            if (hit_ground.collider != gameObject)
            {
                offset_distance = hit_ground.distance;
                Debug.DrawLine(transform.position, hit_ground.point, Color.cyan);
                if (offset_distance <= 5f)
                {
                    isGrounded = true;
                }

            }
        }
        else
        {
            isGrounded = false;
        }



if(!stopJumping)
{

        if (Input.GetButtonDown("Jump") && isGrounded)   // Player starts pressing the button
        { jump = true; }
        if (Input.GetButtonUp("Jump") && !isGrounded)     // Player stops pressing the button
        { jumpCancel = true; }
}

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        //camera forward and right vectors:
        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();


        //this is the direction in the world space we want to move:
        movement = forward * input.y + right * input.x;

               localMove.x = (Input.GetAxisRaw("Horizontal") * right.x) + (Input.GetAxisRaw("Vertical") * forward.x) * speed;
        //Same with Z except dictates how he will move across the Z plane
        localMove.z = (Input.GetAxisRaw("Horizontal") * right.z) + (Input.GetAxisRaw("Vertical") * forward.z) * speed;



        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("dash",dash);
        anim.SetBool("Glide",gliding);
      anim.SetBool("Wall_Slide",WallSliding);
            anim.SetBool("Wall_Jump",WallJump);
                  anim.SetBool("WallLeft",wallLeft);
      anim.SetBool("WallRight",wallRight);


        anim.SetFloat("Yvelocity", rb.velocity.y);


        if (Physics.Raycast(transform.position, -(transform.up), out hit, 2.5f, LayerMask.GetMask("Default")))
        {
            Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);
            groundSlopeDir = Vector3.Cross(temp, hit.normal);
            groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (isGrounded)
            {
                if(rb.velocity.magnitude >= 0)
                {

                slope = Quaternion.FromToRotation(transform.up, hit.normal);
                transform.rotation =Quaternion.Lerp(transform.rotation, (slope) * transform.rotation,SlopesRotationSpeed * Time.deltaTime);

                }
            }
            else
            {
                Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 0.1f * Time.deltaTime);
            }

        }

        
            if(maxSpeed ==acc_downhill && input.magnitude ==0)
            {
                maxSpeed = normal_maxSpeed;
            }


            if(isGrounded)
            {
                candash = true;
            }

RaycastHit stickToWall;


        if(Physics.Raycast(transform.position , transform.forward,out stickToWall,1f,LayerMask.GetMask("Default")))
        {
            Debug.DrawLine(transform.position, stickToWall.point, Color.cyan);
            if(!wallLeft ||!wallRight)
            {
            speed = 5;
            }
            if(!jump)
            {
            rb.velocity.Set(rb.velocity.x,0,0);
            }
        }
    

        if(speed < 0.01 && input.magnitude !=0 )
        {
            speed += 0.01f;

        }



            

            ////turn the glide off when we collide witha  wall
            if((wallLeft ||wallRight )&& gliding)
            {
                gliding = false;
            }

        ////abilities
        if(candash)
        {
        Dash();
        }
        if(!dash)
        {
        MachineGun();
        Glide();
        }
 InteractionWithEnvironment();
 Stones();



    }

    void WallRunDetect()
    {


             wallLeft= Physics.Raycast(transform.position, transform.right, out wallHitRight, LayerMask.GetMask("Default"));
         wallRight = Physics.Raycast(transform.position, -transform.right, out wallHitLeft, LayerMask.GetMask("Default"));
       
                   ////turn the glide off when we collide witha  wall
            if((wallLeft ||wallRight ))
            {
                gliding = false;
            }




       if(!isGrounded)
       {
         if (wallLeft)
        {
        rb.useGravity = false;

        WallRunBegin();

         ////lose all control of player
        canControl = false;
        WallJump = false;
         WallSliding = true;
         stopRotating = true;

         ////stop gliding because it messing around witn the wall jump
         gliding = false;
        }
            else if (wallRight)
            { //1 for the distance orignally
            rb.useGravity = false;
                WallRunBegin();


                ////lose all control of player
                canControl = false;
                WallJump = false;
                WallSliding = true;
                stopRotating = true;
                 ////stop gliding because it messing around witn the wall jump
         gliding = false;
            }
        else
        {
            StopWallRun();
        }
       }
       else{StopWallRun();}

    }


void Stones()
{
    ///speed stone activation
    if(speed_stone )
    {
       
        normal_maxSpeed = speed_stone_maxSpeed;
    }
    if(stone3)
    {
        ///can do lignting dash
        if(isGrounded)
        {
        canDoDash = true;
        }



    }





    if(canDoDash)
    {

        
        if(Input.GetButtonDown("Fire2")  && !isGrounded && !lignting_dash)
        {
            canDoDash = false;
            
            dash = false;
            lignting_dash = true;
            canControl = false;
        camera_control.fieldOfView =Mathf.MoveTowards( camera_control.fieldOfView,87,300 * Time.deltaTime);
           
            
           
            rb.MovePosition(Vector3.MoveTowards(transform.position, lightning_dash_object.transform.position,ligtning_speed* Time.deltaTime) );
            StartCoroutine(DashLigtningCancel(0.6f));
        
        }

        



    }
}


IEnumerator DashLigtningCancel(float time)
{
  yield return new WaitForSeconds(time);
        canControl = true;
        canDoDash = false;
        lignting_dash =false;
        speed = maxSpeed;
        camera_control.fieldOfView =60;

            
}
    void WallRunBegin()
    {
        // turn off grav
    rb.useGravity = false;
    gravity = 0;
    //stop rotation, jump, and control
    stopJumping = true;
    stopRotating = true;
    canControl = true;
    //trun off glide
    gliding =false;


    wallRunVec = transform.forward * 32 * Time.deltaTime;
    wallRunVec = Vector3.down * wallRunGravity * Time.deltaTime;

    if(Input.GetButtonDown("Jump"))
    {
        if(wallLeft)
            {
                
               
                Vector3 wallJumpDir =   (transform.up * walljumpForce)+(wallHitRight.normal* 32);
                rb.velocity = new Vector3 (rb.velocity.x,0,rb.velocity.z);
                
                rb.AddForce(wallJumpDir * walljumpForce * Time.deltaTime,ForceMode.VelocityChange);
                 StartCoroutine(RegainPlayerControl(1f));
            }
                else if(wallRight)
                    {
                    Vector3 wallJumpDir =  (transform.up * walljumpForce)+(wallHitLeft.normal* 32);
                     rb.velocity = new Vector3 (rb.velocity.x,0,rb.velocity.z);
    	        
                     rb.AddForce(wallJumpDir * walljumpForce * Time.deltaTime,ForceMode.VelocityChange);
                     StartCoroutine(RegainPlayerControl(1f));
                    
                    }


    }
       



    ///cancel wall runs when we are grounded

    if(isGrounded)
    {
        candash = true;
        if(wallLeft || wallRight)
         {
gravity = original_gravity;        
        rb.useGravity = true;
        canControl =true;
        wallLeft =false;
        wallRight = false;
        stopJumping = false;
        stopRotating = false;
        }
    }





 //cam.gameObject.transform.rotation= Quaternion.Euler(transform.rotation.x,transform.rotation.y,cam_tilt);
    }


    void StopWallRun()
    {
        gravity = original_gravity;        
        rb.useGravity = true;
        canControl =true;
        wallLeft =false;
        wallRight = false;
        stopJumping = false;
        stopRotating = false;
        

    }



IEnumerator RegainPlayerControl(float time )
{
     WallSliding = false;
    yield return new WaitForSeconds(time);
    canControl =true;
        wallLeft =false;
        wallRight = false;
        stopJumping = false;
        stopRotating = false;
}

    void Dash()
    {
        if(Input.GetButtonDown("Fire3")  && !isGrounded && !dash)
        {
            candash = false;
            
            dash = true;
            canControl = false;
            gravity = 0;
            rb.useGravity=false;
            Vector3 DashVector = transform.forward * DashSpeed;
            DashVector.y = 0;
            rb.velocity = DashVector;
            StartCoroutine(DashCancel(1.2f));
        dash_particle.Play();
        }
    }
    IEnumerator DashCancel(float time)
    {
        yield return new WaitForSeconds(time);
        canControl = true;
        dash = false;
        speed = maxSpeed;
        dash_particle.Stop();
         gravity = original_gravity;
            rb.useGravity=true;
            candash = false;
            
    }

void Glide()
{
    if(wallLeft ==false||wallRight ==false)
{    if(!isGrounded )
    {

        if(Input.GetButtonDown("Jump") && !gliding)
        {
            glideSpeed = speed;
           
            dash =false;
            canControl = false;
            stopAccelerating = true;
            stopJumping = true;
            stopRotating = true;
         gliding = true;


        }

        if(gliding )
        {
            dash =false;
            canControl = false;
            stopAccelerating = true;
            stopJumping = true;
            stopRotating = true;

            rb.useGravity =false;
            gravity = 0;


            Camera.main.GetComponent<Camera_controller>().alignDelay = 0.1f;
            Vector3 roatationAngles = new Vector3(23,Input.GetAxis("Horizontal") * glide_rotationSpeed * Time.deltaTime,0);

            transform.Rotate(0,roatationAngles.y,0);


            
           Vector3 glideVector = transform.forward  * glideSpeed;
           glideVector.y = -3.4f;
             rb.velocity =  (glideVector );

            
           

                ///speed up when aiming down
                if(transform.eulerAngles.x > 20)
                {
                 glideSpeed = Mathf.MoveTowards(glideSpeed,glideMaxSpeed,glideAcc * Time.deltaTime);
                }

                ////rotate up to fly upwards but lose speeed (can only do 3 times)
/*
                if(Input.GetButtonDown("Jump") && glide_jumps >0)
                {
                    roatationAngles.x = -23;
                    speed -= 3f;
                    glide_jumps --;
                }
*/
                //roatationAngles.x = Mathf.MoveTowards(roatationAngles.x,23,0.8f* Time.deltaTime);
                 transform.rotation=(Quaternion.Euler(roatationAngles.x,transform.eulerAngles.y,0));


             /*
            //rotate up and down
           Vector3 glide_rotation =new Vector3(Input.GetAxis("Horizontal") * 5*Time.deltaTime,0,Input.GetAxis("Vertical") * 5 * Time.deltaTime);
            ///accelerate and deaccelerate based on angle
            if(Input.GetAxis("Horizontal") < 1)
            {
                glideSpeed = Mathf.MoveTowards(glideSpeed,glideMaxSpeed,glideAcc);

            }
            if(Input.GetAxis("Horizontal") < -1)
            {
                glideSpeed = Mathf.MoveTowards(glideSpeed,0,glideDeacc);
                
            }
            transform.rotation = Quaternion.Euler (glide_rotation);
           
*/
}
}
            /////cancel glide
            if(isGrounded)
            {
                if(gliding)
                {
                     glideSpeed = speed;
                    gliding = false;
                     
            canControl = true;
            stopAccelerating = false;
            stopRotating = false;
            stopJumping = false;
            rb.useGravity =true;
            gravity = original_gravity;
            Camera.main.GetComponent<Camera_controller>().alignDelay = 05;
                }
            }

        }
    
}

        void MachineGun()
    {
        Vector3 Recoil;
        Recoil = -transform.forward * recoilSpeed;
                        Recoil = transform.up * RecoilUp;
        if(Input.GetButton("Fire1") && !reload && ammo >0)
        {
            timer-=Time.deltaTime;
				if(timer<=0f)
				{
                     if(!isGrounded)
                    {
                        rb.velocity = (Recoil );
                        
                    }
					timer=0.2f;
                    muzzleFlash.Play();
					ShootAtPoint();
                   
				}
               
 anim.SetLayerWeight(anim.GetLayerIndex("ShootingLayer"),1);
        }
        else
        {
             anim.SetLayerWeight(anim.GetLayerIndex("ShootingLayer"),0);
        }


        if(ammo <= 0)
        {
            reload = true;
        
        }
        if(reload)
        {
             anim.SetLayerWeight(anim.GetLayerIndex("ShootingLayer"),0);
             
             reloadTimer -= Time.deltaTime;
             if(reloadTimer < 0)
             {

                 reload =false;
                  reloadTimer = reloadTimer_start;
                  ammo = start_ammo;
             }

        }
    }
    void ShootAtPoint()
    {
        Instantiate(bullet,armShootPoint.gameObject.transform.position,transform.rotation);
        ammo --;
         

    }






    void FixedUpdate()
    {


            rb.AddRelativeForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Force);



        if (rb.velocity.magnitude > maxSpeed)
        {
            Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }


if(!stopRotating)
{

        if (input.magnitude > 0)
        {
            Quaternion rot = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);


        }

        float tS = speed / 5;

        turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, tS);
}
       
        movement = transform.forward * speed;
        if (canControl)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        //rb.AddForce ( movement * Time.deltaTime,ForceMode.VelocityChange);
        if (stopAccelerating == false)
        {
            if (input.magnitude != 0)
            {
speed = Mathf.MoveTowards(speed, maxSpeed * input.magnitude, acc * Time.deltaTime);

                   if(transform.localEulerAngles.x <0f && groundSlopeAngle > 9)
                {maxSpeed = acc_downhill;}
                if( groundSlopeAngle < 9)
                {
                    if(input.magnitude == 0)
                   { maxSpeed = normal_maxSpeed;}
                }
            }
            //////////////////////////////////////////////////
            else
            {
                // apply deceleration unless slope is too steep and player is going downhill
                if (Vector3.Angle(hit.normal, Vector3.up) < 45f )
                {
                    speed *= 1f - (true ? deacc : deacc);
                }
            }
        }

        if(!isGrounded)
        {
            deacc = 0.01f;
            turnSpeed = 20f;
        }
        else
        {
            deacc = normal_deacc;
        }



        ///////////////////////////////////////////////////////////

        if (isGrounded)
        {
           
      
            //If the angle between the stick and the player's direction is larger than 150 degrees, incur a skid penalty.
            if (Vector3.Angle(transform.forward, localMove) >= 120 && speed != 0 && isGrounded)
            { SkidTurnaround(); }


        }



        // Normal jump (full speed)
        if (jump && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            jump = false;
        }
        // Cancel the jump when the button is no longer pressed
        if (jumpCancel)
        {
            if (rb.velocity.y > jumpShortSpeed)
                rb.velocity = new Vector3(rb.velocity.x, jumpShortSpeed, rb.velocity.z);
            jumpCancel = false;
        }


        if(ScoreKeeper.hp<0 )
        {
            dead= true;
            canControl =false;
            stopJumping =false;
            StartCoroutine(RestartLevel(1f));
        }


    }
IEnumerator RestartLevel(float time)
{
    yield return new WaitForSeconds(time);
    SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    ScoreKeeper.hp =100;
}
    void InteractionWithEnvironment()
    {
        if(GoingUpVine)
        {
            canControl = false;
            stopRotating = true;
            stopJumping = true;
            stopAccelerating = true;
            gliding =false;
            dash = false;
            StartCoroutine(StopVine(vine.GetComponent<Vine>().timeToStop));
               rb.MovePosition (Vector3.MoveTowards(transform.position,vine.GetComponent<Vine>().playerNextPos.transform.position,vine.GetComponent<Vine>().bouceSpeed * Time.deltaTime));

        vine.GetComponent<Vine>().anim.Play();
        }
            
        

     


            }
          
             
    

    IEnumerator StopVine(float time)
    {
        yield return new WaitForSeconds ( time);
       
            //transform.position = Vector3.forward * 5 * Time.deltaTime;
            GoingUpVine = false;
            canControl = true;
            stopRotating = false;
            stopJumping = false;
            stopAccelerating = false;
            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
            gravity = original_gravity;
            vine.GetComponent<Vine>().anim.Stop();
    }

    

    void SkidTurnaround()
    {
        if (isGrounded)
        {
            transform.forward *= -1;
            float skidVelocity = (speed * -1);

            speed = skidVelocity / 2;

                        if(maxSpeed ==acc_downhill )
            {
                maxSpeed = normal_maxSpeed;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
       if(col.gameObject.tag == "Enemy")
        {
            TakeDamage(10,false);
            
        } 
               if(col.gameObject.tag == "FinishRunning")
        {
            ///goto next scene with player and change players spawn point to an empty gamobject in the next level
			        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);

            DontDestroyOnLoad( scoreController );
            GameObject startPos = GameObject.FindGameObjectWithTag("StartPosition");
            transform.position = startPos.gameObject.transform.position;
        } 
       
    }

    IEnumerator OffPanel(float time)
    {
        yield return new WaitForSeconds(time);
         Talk_Panel.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
       if(col.gameObject.tag == "FallingTree")
        {
             TakeDamage(05,false) ;
            
        } 
         if(col.gameObject.tag == "VineTrigger")
        {
            GoingUpVine = true;
            vine = col.gameObject;
            
        } 
               if(col.gameObject.tag == "Spikes")
        {
            TakeDamage(20 ,false);
            
        } 


        if(col.gameObject.tag == "Coin")
        {
            Destroy(col.gameObject);
            ScoreKeeper.coins ++;
        }
        if(col.gameObject.tag == "Talk")
        {
            Talk_Panel.SetActive(true);
            Talk_Panel.GetComponentInChildren<TMP_Text>().SetText( col.gameObject.GetComponent<TextToSay>().thingToSay);
            StartCoroutine(OffPanel(GetComponentInChildren<TextToSay>().textTime));
            



           // Talking();
            
        } 
    }


    void TakeDamage(float damage,bool done)
    {
        if(done == false)
        {
        ScoreKeeper.hp -= damage;
        done =true;
        }

        return;
    }

}
