using Foundry.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;
    string name;
    int index;

    GameObject p1Icon;
    GameObject p2Icon;
    GameObject p3Icon;
    GameObject p4Icon;

    public List<Vector3> tm1_spawnPoints = new List<Vector3>();
    public List<Vector3> tm2_spawnPoints = new List<Vector3>();

    List<Vector3> ballSpawnPoints = new List<Vector3>();

    [HideInInspector]
    public float farSideLine; // +z
    [HideInInspector]
    public float nearSideLine; // -z
    [HideInInspector]
    public float halfCourtLine; // +x , 0
    [HideInInspector]
    public float baseLineLeft; //-x
    [HideInInspector]
    public float baseLineRight; // +x
    [HideInInspector]
    public float floor;  // -y
    [HideInInspector]
    public float roof; // +y

    public GameObject BottomPlane;
    public GameObject FrontPlane;
    public GameObject BackPlane;
    public GameObject LeftPlane;
    public GameObject RightPlane;
    public GameObject TopPlane;

    [HideInInspector]
    public GameObject[] walls = new GameObject[4];

    public GameObject halfCourtBox;


    public GameObject playingLevelPlane;

    public static bool loadedFromStage;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (!gm)
        {
            gm = GlobalConfiguration.Instance.gameManager.GetComponent<GameManager>();
            

        }

        if (!lm)
        {
            lm  = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            lm.SetStage(this);
        }
       

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GlobalConfiguration.Instance.SetIsAtScene(true, scene.buildIndex, this);
        InitBounds();

    }

    public void CheckStageStart()
    {
        if (GlobalConfiguration.Instance.gameMode == GlobalConfiguration.GameMode.none)
        {

            loadedFromStage = true;

            gm.networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

            GameObject player1 = GetNetworkPlayer();

            GlobalConfiguration.Instance.LoadDefaultGame(player1);

            print("Stage Start");
        }
    }


    GameObject GetNetworkPlayer() { 

        return gm.networkManager.playerObject;
    }

    private void OnEnable()
    {
        /*
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (lm.GetSceneVisited(sceneIndex))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;


            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            if (!gm)
            {
                // gm = GlobalConfig.instaance.InstantiateGM
            }

            lm = gm.levelManager;
            lm.SetStage(this);

            InitBounds();
            // CreateSpawnLocations();
            lm.LoadLevel();
        }
        */
    }

    public List<Vector3> GetSpawnLocations(int team, int playerCount)
    {

        if (team == 1)
        {
            if (tm1_spawnPoints.Count == 0)
            {
                CreateSpawnLocations(team, playerCount);
            }

            
            return tm1_spawnPoints;
        }


        if (team == 2)
        {
            if (tm2_spawnPoints.Count == 0)
            {
         
                CreateSpawnLocations(team, playerCount);
            }

            else     // play local aka arcade
            {
                tm2_spawnPoints.Clear();
                CreateSpawnLocations(team, playerCount);
            }

            return tm2_spawnPoints;
        }

        return null;
    }

    private void CreateSpawnLocations(int team, int playerCount)
    {
        int zOffset = -15 * playerCount;
        int yOffset = 3;                           // takes into account size of players... since floor is @ 0
        float zDistMult = 30f;
        float xDistMult = .8f;

        if (team == 1)
        {
            for (int i=0; i< playerCount; i++)
            {
                //Vector3 location = new Vector3(baseLineLeft * xDistMult, floor + yOffset, zOffset + i * zDistMult);
                Vector3 location = new Vector3(-5 + i * 2f, 0f, 7f);
                print("tm1 location  (" + i + ") = " + location);
                tm1_spawnPoints.Add(location);

            }
        }

        if (team == 2)
        {
            for (int i = 0; i < playerCount; i++)
            {
                // Vector3 location = new Vector3(baseLineRight * xDistMult, floor + yOffset, zOffset + i * zDistMult);
                Vector3 location = new Vector3(-5 + i * 2f, 0f, -10f);
                print("tm2 location  (" + i + ") = " + location);
                tm2_spawnPoints.Add(location);
            }
        }
    }



    internal List<Vector3> GetBallSpawnLocations(int ballCount)
    {
       if (ballSpawnPoints.Count ==0 && ballCount >0 )
        {
            for (int i=0; i<ballCount; i++)
            {
                int x = (int) halfCourtLine;
                 int y = (int) (roof / 2);
                int zOffset = -6;
                int zMult = 6;

                Vector3 ballPos = new Vector3(x, y, zOffset + zMult * i);
                ballSpawnPoints.Add(ballPos);
            }
        }

        return ballSpawnPoints;
    }


    private void InitBounds()
    {
        if (!playingLevelPlane || !BottomPlane || !LeftPlane || !RightPlane || !TopPlane || !FrontPlane || !BackPlane)
        {

            BottomPlane = GameObject.Find("Bottom Plane");
            LeftPlane = GameObject.Find("Left Plane");
            RightPlane = GameObject.Find("Right Plane");
            TopPlane = GameObject.Find("Top Plane");
            FrontPlane = GameObject.Find("Front Plane");
            BackPlane = GameObject.Find("Back Plane");
            playingLevelPlane = GameObject.FindGameObjectWithTag("Playing Level");


        }

        walls[0] = FrontPlane;
        walls[1] = BackPlane;
        walls[2] = LeftPlane;
        walls[3] = RightPlane;


        farSideLine = BackPlane.transform.position.z;     
        halfCourtLine = halfCourtBox.transform.position.x;
        baseLineLeft = LeftPlane.transform.position.x;
        baseLineRight = RightPlane.transform.position.x;
        floor = playingLevelPlane.transform.position.y;
        roof = TopPlane.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal GameObject[] GetWalls()
    {
        return walls;
    }

    internal bool IsInGameBounds(Vector3 pos)
    {

        float padding;

       // print("pos = " + pos);
       // print(" baseLineLeft = " + baseLineLeft);
      //  print(" baseLineRight = " + baseLineRight);
      //  print(" farSideLine = " + farSideLine);
       // print(" nearSideLine = " + nearSideLine);
     //   print(" floor = " + floor);
       // print(" roof = " + roof);


        if (baseLineLeft < pos.x && pos.x < baseLineRight)
        {
            if (nearSideLine < pos.z && pos.z < farSideLine)
            {
                if (floor < pos.y && pos.y < roof)
                {
                    return true;
                }
            }
        }
        
        return false;

    }

    internal void ClearSpawnpoints(int v)
    {
       if (v == 2)
        {
            tm2_spawnPoints.Clear();
        }
       if (v == 1)
        {
            tm1_spawnPoints.Clear();
        }
    }
}
