using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System.Threading;
using Foundry;

public class Player : MonoBehaviour
{

    LevelManager levelManager;

    // transform or spawn point

    public int number;

    int playerIndex;   // < should be alligned w Foundry

    public GameObject playerConfigObject;  // 0

    public GameObject super;

    public bool hasAI;

    public Color color;    // should always be joystick colorS
    public int team;
    public bool isOut = false;

   
    [System.NonSerialized] public float speed;
    [System.NonSerialized] public float dodgeSpeed;
    [System.NonSerialized] public float throwPower;


    // acceleration

    public int stamina;
    private float staminaCool;
    public int power;
    private float powerCool;


    private bool dodgeActivated;
    private float fxSpeed = 1f;


    GameObject ball;


    private void Awake()
    {
        GameObject gameManagerObject = GlobalConfiguration.Instance.gameManager.gameObject;

        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        // print("Player Awake");
        //CheckChildStructure();
        //isSet = false;
       // InputUser.listenForUnpairedDeviceActivity = 1;
      //  onChange += DeviceChange();


    }



    void Start()
    {

        // attributes set in GlobalConfig


        // grab from level Manager
        /*
                if (number == 1)       // this should be all in one ui object
                    {
                        staminaBarObject = GameObject.Find("1 Stamina Slider");
                        powerBarObject = GameObject.Find("1 Power Slider");
                    }
                    if (number == 2)
                    {
                        staminaBarObject = GameObject.Find("2 Stamina Slider");
                        powerBarObject = GameObject.Find("2 Power Slider");
                    }
                    if (number == 3)
                    {
                        staminaBarObject = GameObject.Find("3 Stamina Slider");
                        powerBarObject = GameObject.Find("3 Power Slider");
                    }
                    if (number == 4)
                    {
                        staminaBarObject = GameObject.Find("4 Stamina Slider");
                        powerBarObject = GameObject.Find("4 Power Slider");
                    }

                    staminaCool = (int)(stamina-controller3D.staminaCool);    // iffy 
                    staminaBarObject.GetComponent<Slider>().value = stamina;
                    powerCool = (int)(stamina- controller3D.superCoolDown);     // iffy
                    powerBarObject.GetComponent<Slider>().value = power;

                

        playerAudioSource = playerConfigObject.GetComponent<AudioSource>();

        */

    }

    // Update is called once per frame
    void Update()
    {
       

    }


    public void BallReleased()
    {
        // print("Ball Released");

        // Alex code to increase ball speed when thrown
        // Debug.Log($" speed on release is {rigidbody.velocity}");
        float throwSpeedMultiplier = 5f;
        ball.GetComponent<Rigidbody>().velocity *= throwSpeedMultiplier;
        // Debug.Log($" now the speed is {rigidbody.velocity}");
        //




        Ball ballComp =  ball.GetComponent<Ball>();

        ballComp.SetThrown(team);
       

        levelManager.AddThrow(ball, this.gameObject);
    }

    public void BallGrab(SpatialHand hand, SpatialGrabbable grabbable)
    {
        ball = grabbable.gameObject;

        print(ball.name + " grabbed");
        
        Ball ballComp = ball.GetComponent<Ball>();

        ballComp.grounded = false;                         

        if (ballComp.ThrownByOpp(team))
        {


            //playCatch();

            //  playerScript.TriggerCatchFX();

            levelManager.ClearContacts(ball);
            // levelManager.AddCatch(this.gameObject, parent);
            levelManager.LastThrowerOut(ball);
            // levelManager.GetAnotherPlayer(this.gameObject.GetComponentInParent<Player>().team);
            levelManager.RemoveHit(ball);
            //  levelManager.CatchDisplay(playerScript.color, playerConfigObject.transform.position, (Vector3.Magnitude(rigidbody.velocity) + Vector3.Magnitude(velocityCaught)) / 2f);
            ballComp.DeactivateThrow();

            /*
            float hitPauseDuration = Mathf.Clamp(velocityCaught.magnitude / 100f, 0, 3f);
            float hitPausePreDelay = .36f;

            DelayPause(hitPauseDuration, hitPausePreDelay);
            */

            print("~!Caught!~");
        }
    }


    private void enableController()
    {
        /*
        controller3DObject.SetActive(true);
        controller3D.enabled = true;
        PlayerInput playerInput = controller3DObject.GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.enabled = true;   // might be called twice .. (Controller3D.OnEnable())
        }

        playerConfigObject.GetComponent<Rigidbody>().isKinematic = false;
        */
    }

    internal void SetOnStandby(bool v)
    {
        Rigidbody rigidbody = playerConfigObject.GetComponent<Rigidbody>();    // should prolly do this in playerConfig script

        rigidbody.isKinematic = v;


      //  if (hasJoystick)
        {
            if (v)
            {
               // controller3D.DisablePlayerInputControls();
            }
            else
            {            
               // controller3D.EnablePlayerInputControls();
            }
            
        }


        //print("hasAI")
        if (hasAI)


        {
            if (!v)
            {
                NavMeshAgent navMeshAgent = playerConfigObject.GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = true;
            }
            else
            {
                NavMeshAgent navMeshAgent = playerConfigObject.GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = false;
            }
        }
    }

    private void disableAI()
    {
        hasAI = false;

       // playerConfigObject.aiObject.SetActive(false);

        NavMeshAgent navMeshAgent = playerConfigObject.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;

      //  playerConfigObject.playerAura.SetActive(false);
    }
    public void enableAI()
    {


        hasAI = true;
        /*
        aiObject.SetActive(true);
        aiScript.enabled = true;
        aiScript.Init();

        hasJoystick = false;
        // controller3DObject.SetActive(false);
        //     controller3D.enabled = false;
        //  PlayerInput playerInput = controller3DObject.GetComponent<PlayerInput>();
        // playerInput.enabled = false;


        NavMeshAgent navMeshAgent = playerConfigObject.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;

        playerAura.SetActive(false);

        playerConfigObject.GetComponent<Rigidbody>().isKinematic = true;
        */
    }

    private void SetAuraColor(Color color)
    
    {
        /*
        playerAura.SetActive(true);

        GameObject psRoot = playerAura;
        ParticleSystem.MainModule rootPS = psRoot.GetComponent<ParticleSystem>().main;
        rootPS.startColor = new ParticleSystem.MinMaxGradient(color);

        /*
        int childCount = psRoot.transform.childCount;

        for (int i = 0; i < childCount; ++i)
        {
            GameObject childObject = psRoot.transform.GetChild(i).gameObject;
            ParticleSystem.MainModule childPS = childObject.GetComponent<ParticleSystem>().main;

            if (i == 1 || i == 2)
            {
                childPS.startColor = new ParticleSystem.MinMaxGradient(new Color(color.r, color.g, color.b, .1f));
            }
            else
            {
                childPS.startColor = new ParticleSystem.MinMaxGradient(color);
            }

        }
           */

    }


    public void ToggleActivateDodge()
    {
        dodgeActivated = !dodgeActivated;

    }

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
    }
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void SetColor(Color c)

    {
        color = c;

        // if (hasJoystick)
        {
            SetAuraColor(color);
        }

    }
}
