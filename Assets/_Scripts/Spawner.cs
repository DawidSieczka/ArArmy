﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> SpawnPoints;

    [SerializeField]
    private GameObject Player;

    private void Awake()
    {
        InvokeSpawning();
    }

    public void InvokeSpawning()
    {
        SpawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
        var anySpawn = TakeRandomSpawnPoint(SpawnPoints.Count);
        SpawnPoints[anySpawn].SpawnPlayer(Player);
    }

    private int TakeRandomSpawnPoint(int amount)
    {
        System.Random rand = new System.Random();
        return rand.Next(amount);
    }
}