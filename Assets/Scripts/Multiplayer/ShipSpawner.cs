using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShipSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerShip;

    void Start()
    {
        int index = 0;

        foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject shipObj = Instantiate(playerShip, spawnPoints[index].position, Quaternion.identity);
            NetworkObject shipNetObj = shipObj.GetComponent<NetworkObject>();
            shipNetObj.SpawnWithOwnership(id);

            index++;
        }
    }

    void Update()
    {
        
    }
}
