using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{

    [SerializeField] private GameObject Player;
    private void OnClientConnectedCallback(ulong clientId)
    {
        SpawnPlayerServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership =false)]
    private void SpawnPlayerServerRpc(ulong clientId)
    {
       
        GameObject PlayerObject = Instantiate(Player, transform.position, Quaternion.identity);
        NetworkObject PlayerNetworkObject = PlayerObject.GetComponent<NetworkObject>();
        PlayerNetworkObject.SpawnWithOwnership(clientId);
    }

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        if (IsServer)
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }  
}
