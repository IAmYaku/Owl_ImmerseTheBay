using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfiguration : MonoBehaviour
{
    GameObject parent;
    LevelManager levelManager;

    public GameObject playerAura;
    public GameObject aiObject;
    Player player;
    //Controller3D controller3D;
    AI ai;

    GameObject hitObject;
    GameObject catchObject;
    GameObject winObject;

    public GameObject staminaBarObject;
    public GameObject powerBarObject;

    public AudioSource audioSource;
    public SpriteRenderer spriteRenderer;
    public Rigidbody rigidbody;
    public SphereCollider headCollider;   // extra points +?, sfx, vfx, etc
    public CapsuleCollider bodyCollider;

    public AudioClip catchSound;
    public AudioClip[] throwSounds;
    public AudioClip dodgeSound;

    public AudioClip outSound;
    public AudioClip footsteps;

    bool onGround;
    bool isJumping;

    float pushVal = .2f;   // rename and pull to gr

    float t_contact0;

    public bool ballContact;     // what if multiple balls
    GameObject ballHit;


    private bool isDeRendering;
    private float dR_Cool;
    private float drC_t0;
    private float drC_tF;


    void Start()
    {
       // print("PlayerConfig start");

        parent = this.transform.parent.gameObject;

        if (!player)
        {
            player = parent.GetComponent<Player>();
        }

        if (!playerAura)
        {

        }
        /*
        if (!controller3D)
        {
            controller3D = player.controller3DObject.GetComponent<Controller3D>();
        }

        if (!ai)
        {
            ai = player.aiObject.GetComponent<AI>();
        }

        */

        if (!levelManager)
        {
             levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
      if (levelManager.isPlaying)
      {

          if (hasJoystick)
          {
              staminaCool = (stamina - controller3D.staminaCool);
              staminaSR.material.SetFloat("_Rotation", staminaCool / 300f);
              staminaBarObject.GetComponent<Slider>().value = staminaCool;
              powerCool = (power - controller3D.superCoolDown);
              powerSR.material.SetFloat("_Rotation", powerCool / 60f);
              powerBarObject.GetComponent<Slider>().value = powerCool;
          }
          if (hasAI)
          {
              staminaCool = (int)(stamina - aiScript.staminaCool);
              staminaBarObject.GetComponent<Slider>().value = staminaCool;
              powerCool = (int)(power - aiScript.superCoolDown);
              powerBarObject.GetComponent<Slider>().value = powerCool;
          }          
      }

      if (isDeRendering)
      {
          drC_tF = Time.realtimeSinceStartup;
          float t = drC_tF - drC_t0;
          DeRender(t);
      }
      */
    }

    internal void SwitchToWinAnimation()
    {
        throw new NotImplementedException(); 
    }

    public void RemoveContact()
    {
        ballContact = false;     // what if multiple balls?
        ballHit = null;

    }

    void OnCollisionEnter(Collision collision)
    {
      //  if (levelManager)
        {

         //   if (!player.isOut && levelManager.isPlaying)
            {
                if (collision.gameObject.tag == "Ball")
                {
                    ballHit = collision.gameObject;
                    print("ballHit = " + ballHit);

                    if (ballHit.GetComponent<Ball>().CheckPlayerHit(player.team))                                                                           // make more module
                    {
                        print("~Contact~");

                        ballHit.GetComponent<Ball>().contact = true;

                        float ballHitVelocity = ballHit.GetComponent<Rigidbody>().velocity.magnitude;
                        print("ballHitVelocity = " + ballHitVelocity);
                  

                        ballContact = true;         // what if multiple balls

                        
                    //    TriggerHitAnimation();

                        float stallTime = .2f;
                        float hitDelay = .0005f;
         
                     //   TriggerKnockBack(ballHit.GetComponent<Rigidbody>().velocity, ballHitISupered);
                     //   SlowDownPlayer(hitDelay, stallTime);



                        levelManager.AddHit(ballHit, parent);

                      //  levelManager.TriggerHitFX(gameObject, ballHit);
                       // player.SetHitFX(true);

                        float ballHitPauseWeight = 50f;
                        float hitPauseDuration = Mathf.Clamp( ballHitVelocity / ballHitPauseWeight, FXManager.min_HitPauseDuration, FXManager.max_HitPauseDuration);
                        float hitPausePreDelay = .0125f;

                      //  DelayPause(hitPauseDuration, hitPausePreDelay);

                       // levelManager.CamShake(ballHit.GetComponent<Rigidbody>().velocity.magnitude, transform);

                        //if gameMode = local
                       // levelManager.PostFX("Player1Hit");
                    }

   
                }


                // TODO doesnt make smoothe as intended 
                if (collision.gameObject.tag == "Wall")
                {
                   // CorrectPosition(collision.gameObject)
                }




            }

        }   
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
           
        }
        else
        {
          //  inBallCollision = false;
        }

    }





    private void DelayPause(float hitPauseDuration, float hitPausePreDelay)
    {
        levelManager.SetHitPauseDuration(hitPauseDuration);
        //Invoke("DoHitPause", hitPausePreDelay);
        levelManager.HitPause();

    }

    private void DoHitPause()
    {
        levelManager.HitPause();
    }

    public void DisablePlayer()
    {
        /*
        isOut = true;
       
        if (hasJoystick)
        {
            if (controller3D.ballGrabbed)
            {
                controller3D.DropBall();
            }

            controller3D.isKnockedOut = false;
            controller3D.playerConfigObject.GetComponent<PlayerConfiguration>().ballContact = false;
            hitObject.SetActive(false);
            playerAura.gameObject.SetActive(false);

        }

        else
        {
            if (aiScript.enabled)
            {
                if (aiScript.ballGrabbed)
                {
                    aiScript.DropBall();
                }
                aiScript.isKnockedOut = false;
                aiScript.playerConfigObject.GetComponent<PlayerConfiguration>().ballContact = false;
                aiScript.enabled = false;
                hitObject.SetActive(false);

                NavMeshAgent navMeshAgent = playerConfigObject.GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = false;

            }
        }

        DeRender();
        */
    }

    public void DeRender()
    {
        //playerConfigObject.GetComponent<PlayerConfiguration>().SetOutAnimation(true);
        isDeRendering = true;
        dR_Cool = 1f;                                       // careful here, things mess up
        drC_t0 = Time.realtimeSinceStartup;
       // GetComponent<CapsuleCollider>().enabled = false;
        //GetComponent<SphereCollider>().enabled = false;
        //GetComponent<Rigidbody>().isKinematic = true;

    }

    internal void playFootsteps()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = .85f;
            audioSource.clip = footsteps;
            audioSource.Play();
        }

    }

    private AudioClip GetThrowSound()
    {
        /*
                if (throwSounds.Length > 0)
                {
                    return throwSounds[0];
                }
                else
                {
                    return null;
                }
        */
        return null;
    }


    internal void PlayOutSound()
    {
        /*
        playerAudioSource.clip = outSound;
        playerAudioSource.pitch += UnityEngine.Random.Range(-3f, 3f);
        playerAudioSource.volume = .25f;
        playerAudioSource.Play();

        NormalAudioSource();
        */
    }

    internal void PlayDodgeSound()
    {
        /*

        playerAudioSource.clip = dodgeSound;
        playerAudioSource.pitch += UnityEngine.Random.Range(1f, 1.5f);
        playerAudioSource.volume = .85f;
        playerAudioSource.Play();

        NormalAudioSource();
        */

    }
    internal void playThrowSound()
    {
        /*
        playerAudioSource.volume = 1f;
        playerAudioSource.pitch += UnityEngine.Random.Range(1f, 1.5f);
        playerAudioSource.clip = GetThrowSound();
        playerAudioSource.Play();

        NormalAudioSource();
        */
    }

    private void NormalAudioSource()
    {
        /*
        playerAudioSource.pitch = 1;
        playerAudioSource.volume = .5f;
        */
    }



    internal void SetHitFX(bool x)
    {
        hitObject.SetActive(x);
    }

    internal void TriggerCatchFX()
    {
        catchObject.SetActive(true);
        Invoke("DisableCatchFX", 2f);
    }

    internal void DisableCatchFX()
    {
        catchObject.SetActive(false);

    }

    internal void TriggerWinFX()
    {
        winObject.SetActive(true);
        Invoke("DisableWinFX", 2f);
    }
    internal void DisableWinFX()
    {
        winObject.SetActive(false);

    }

}
