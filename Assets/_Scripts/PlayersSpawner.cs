﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersSpawner : MonoBehaviourPunCallbacks
{
    private List<SpawnPoint> SpawnPoints;

    [SerializeField]
    private GameObject Player;

    private const byte Spawns_Event = 5;

    private void Start()
    {
        SpawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                SpawnPlayerInDrawedSpawnPoint(player);
            }
        }
    }

    private void SpawnPlayerInDrawedSpawnPoint(Player player)
    {
        Debug.Log($"player thats going to spawn {player.NickName}");
        var anySpawn = TakeRandomSpawnPoint(SpawnPoints.Count());
        RaiseEventOptions rso = new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } };
        PhotonNetwork.RaiseEvent(Spawns_Event, SpawnPoints[anySpawn].transform.position, rso, SendOptions.SendReliable);
        StartCoroutine(ExcludeSpawnPointTemporarily(SpawnPoints[anySpawn]));
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnSpawn_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnSpawn_EventReceived;
    }

    private void OnSpawn_EventReceived(EventData data)
    {
        if (data.Code == Spawns_Event)
        {
            InvokeSpawning((Vector3)data.CustomData);
        }
    }

    public void InvokeSpawning(Vector3 spawnPointCoordinates)
    {
        MasterManager.NetworkInstantiate(Player, spawnPointCoordinates, Player.transform.rotation);
    }

    public void InvokeSpawning()
    {
        var anySpawn = TakeRandomSpawnPoint(SpawnPoints.Count());

        MasterManager.NetworkInstantiate(Player, SpawnPoints[anySpawn].transform.position, Player.transform.rotation);

        StartCoroutine(ExcludeSpawnPointTemporarily(SpawnPoints[anySpawn]));
    }

    public IEnumerator ExcludeSpawnPointTemporarily(SpawnPoint spawnPoint)
    {
        ExcludeSpawnPoint(spawnPoint);
        yield return new WaitForSeconds(5);
        IncludeSpawnPoint(spawnPoint);
    }

    private void ExcludeSpawnPoint(SpawnPoint spawnPoint)
    {
        Debug.Log($"Excluded spawn point: {spawnPoint.gameObject.name}");
        spawnPoint.gameObject.SetActive(false);
        SpawnPoints.Remove(spawnPoint);
    }

    private void IncludeSpawnPoint(SpawnPoint spawnPoint)
    {
        Debug.Log($"Included spawn point: {spawnPoint.gameObject.name}");
        spawnPoint.gameObject.SetActive(true);
        SpawnPoints.Add(spawnPoint);
    }

    public void MoveObjectToSpawner(GameObject gameObject)
    {
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAa");
            //SpawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
            var anySpawn = TakeRandomSpawnPoint(SpawnPoints.Count());
            gameObject.transform.position = SpawnPoints[anySpawn].transform.position;
        }
    }

    private int TakeRandomSpawnPoint(int amount)
    {
        System.Random rand = new System.Random();
        int anySpawn = 0;
        do
        {
            Debug.Log($"Spawn: {anySpawn}, isOccupied: {SpawnPoints[anySpawn].isOccupied}");

            anySpawn = rand.Next(amount);
        } while (SpawnPoints[anySpawn].isOccupied);
        return anySpawn;
    }   
}