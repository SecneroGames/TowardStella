using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class RoomManager : NetworkBehaviour
{
    [SerializeField] private GameObject P1Panel;
    [SerializeField] private GameObject P2Panel;
    [SerializeField] private GameObject P3Panel;

    [SerializeField] private GameObject mapSelectPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject selectingMap;
    [SerializeField] private GameObject ReadyBtn;

    void Update()
    {
        if (IsServer)
        {
            PlayerPanelsClientRpc(NetworkManager.Singleton.ConnectedClientsList.Count);
            CheckReady(NetworkManager.Singleton.ConnectedClientsList.Count);

            int index = 0;

            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if(index == 0)
                {
                    P1Panel.GetComponent<NetworkObject>().ChangeOwnership(id);
                    P1Panel.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
                    P2Panel.GetComponent<NetworkObject>().RemoveOwnership();
                    P3Panel.GetComponent<NetworkObject>().RemoveOwnership();
                }
                else if(index == 1)
                {
                    P2Panel.GetComponent<NetworkObject>().ChangeOwnership(id);
                    P2Panel.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
                    P3Panel.GetComponent<NetworkObject>().RemoveOwnership();
                }
                else
                {
                    P3Panel.GetComponent<NetworkObject>().ChangeOwnership(id);
                    P3Panel.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
                }

                index++;
            }
        }
    }

    private void CheckReady(int count)
    {
        int readyCount = 0;

        if (P1Panel.GetComponent<PlayerPanel>().isReady.Value)
        {
            readyCount++;
        }
        if (P2Panel.GetComponent<PlayerPanel>().isReady.Value)
        {
            readyCount++;
        }
        if (P3Panel.GetComponent<PlayerPanel>().isReady.Value)
        {
            readyCount++;
        }

        if(readyCount == count)
        {
            ReadyBtn.SetActive(true);
        }
        else
        {
            ReadyBtn.SetActive(false);
        }
    }

    public void StartGame()
    {
        if(IsServer)
        {
            mapSelectPanel.SetActive(true);
            MapSelectingClientRpc();
            roomPanel.SetActive(false);
        }
    }

    public void StartRace(string sceneName)
    {
        if (IsHost)
        {
            NetworkManager.SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
        }
    }

    [ClientRpc]
    private void MapSelectingClientRpc()
    {
        selectingMap.SetActive(true);
        P1Panel.SetActive(false);
        P2Panel.SetActive(false);
        P3Panel.SetActive(false);
    }


    [ClientRpc]
    private void PlayerPanelsClientRpc(int count)
    {
        if (count > 2)
        {
            P1Panel.SetActive(true);
            P2Panel.SetActive(true);
            P3Panel.SetActive(true);
        }
        else if (count > 1)
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
    }
}
