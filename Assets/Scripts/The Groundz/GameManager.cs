using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Foundry.Networking;

public class GameManager : MonoBehaviour
{


    GameObject LogFeed;

    public LevelManager levelManager;

    GlobalConfiguration globalConfiguration;

    public GameObject GMFX;

    public static int ageThresh;

    public NetworkManager networkManager;

    public GameObject foundryPersistent;

    public GameObject audioManager;

    private void Awake()
    {
       // if (GlobalConfiguration.Instance.GetIsAtStage())
        {
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

            networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }


    }


}