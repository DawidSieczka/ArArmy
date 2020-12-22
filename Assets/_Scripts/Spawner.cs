﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> SpawnPoints;

    [SerializeField]
    private GameObject Player;
    public GameObject testobj;

    private void Awake()
    {
        InvokeSpawning();
    }
    
    public void InvokeSpawning()
    {
        SpawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
        var anySpawn = TakeRandomSpawnPoint(SpawnPoints.Count);
        //SpawnPoints[anySpawn].SpawnPlayer(Player);
        MasterManager.NetworkInstantiate(Player, SpawnPoints[anySpawn].transform.position, Player.transform.rotation);

    }

    public void MoveObjectToSpawner(GameObject gameObject)
    {
        SpawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
        var anySpawn = TakeRandomSpawnPoint(SpawnPoints.Count);
        gameObject.transform.position = SpawnPoints[anySpawn].transform.position;
    }
    private int TakeRandomSpawnPoint(int amount)
    {
        System.Random rand = new System.Random();
        return rand.Next(amount);
    }
}