using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class LobbyManager : NetworkBehaviour
{
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Leave()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        else if (IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
}
