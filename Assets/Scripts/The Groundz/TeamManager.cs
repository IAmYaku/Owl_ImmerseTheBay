﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int number;

    public List<GameObject> players = new List<GameObject>();
    List<GameObject> users = new List<GameObject>();
    List<GameObject> ais = new List<GameObject>();



     int initPlayerCount = 1;  // set in teamSelect

   public int playerCount;

    int aiCount;

    int userCount;

    int outCount;

    int mackCount;
    int kingCount;
    int ninaCount;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }


    public void SetNumber(int x)
    {
        number = x;
    }


    internal void AddObject(GameObject pObject, bool isUser)
    {
        pObject.transform.parent = this.transform;

        if (!players.Contains(pObject))
        {
            players.Add(pObject);
           
        }


        if (isUser)
        {

            if (!users.Contains(pObject))
            {
                userCount++;
                users.Add(pObject);
            }

        }
        else
        {
            if (!ais.Contains(pObject))
            {
                aiCount++;
                ais.Add(pObject);
            }
        }

        playerCount = players.Count;

        // print("team = " + number);
        // print("isUser = " + isUser);
        // print("userCount = " + userCount);
        // print("aiCount = " + aiCount);
    }

    internal void SetInitPlayerCount(int count)
    // parsed at TeamSelect scene only
    {
        initPlayerCount = count;
    }


    internal void SetInitAICount(int count)
    {
        aiCount = count;
    }

    internal void StandByPlayers(bool v)
    {
        foreach (GameObject player in players)
        {
            Player pScript = player.GetComponent<Player>();
            pScript.SetOnStandby(v);
        }
    }



    internal void MoveToSpawnPoints(List<Vector3> spawnPoints)
    {
        //print(" team = " + number);
      //  print("playerCount = " + playerCount);
       // print("spawnPoints.Count " + spawnPoints.Count);
        for (int i = 0; i < playerCount; i++)
        {
            players[i].transform.position = spawnPoints[i];
            //NavMeshAgent.Warp(Vector3).
        }
    }

  

    internal void FaceOpp(int team)
    {
        if (team == 1)
        {
            foreach (GameObject pObject in players)
            {
                pObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        if (team == 2)
        {
            foreach (GameObject pObject in players)
            {
                pObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
        }
    }



    internal void SetRetreatPoints(List<Vector3> spawnPoints)
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (players[i].GetComponent<Player>().hasAI)
            {

                // players[i].transform.GetChild(2).GetComponent<AI>().retreatPoint.position = 
            }

        }
    }

    internal void Clear()
    {
        userCount = 0;
        aiCount = 0;
        initPlayerCount = 1;
        playerCount = 1;
        players.Clear();
    }

    public int GetAICount()
    {
        print("team "+ number + " aiCount = " + aiCount);
        return aiCount;
    }

    public int GetUserCount()
    {
        print("team " + number + " userCount = " + userCount);
        return userCount;
    }

    internal int GetPlayerCount()
    {
        return playerCount;
    }

    internal void ClearAdded()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject player in players)
        {
            if (player.GetComponent<Player>().aiObject.GetComponent<AI>().addedAtStage)
            {
                toRemove.Add(player);
            }
        }

        for (int i=0; i< toRemove.Count; i++)
        {
            ais.Remove(toRemove[i]);
            aiCount--;
            playerCount--;
            players.Remove(toRemove[i]);
        }
    }

}
