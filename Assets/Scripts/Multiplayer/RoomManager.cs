using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RoomManager : NetworkBehaviour
{
    [SerializeField] GameObject P1Panel;
    [SerializeField] GameObject P2Panel;
    [SerializeField] GameObject P3Panel;

    void Update()
    {
        if (IsServer)
        {
            if (NetworkManager.Singleton.ConnectedClientsList.Count > 2)
            {
                P1Panel.SetActive(true);
                P2Panel.SetActive(true);
                P3Panel.SetActive(true);
            }
            else if (NetworkManager.Singleton.ConnectedClientsList.Count > 1)
            {
                P1Panel.SetActive(true);
                P2Panel.SetActive(true);
                P3Panel.SetActive(false);
            }
            else
            {
                P1Panel.SetActive(true);
                P2Panel.SetActive(false);
                P3Panel.SetActive(false);
            }

            foreach (ulong id  in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Debug.Log(id);
            }
        }
    }
}
