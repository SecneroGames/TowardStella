using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkUI : MonoBehaviour
{
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host Connected");
       
        NetworkManager.Singleton.SceneManager.LoadScene("Running1",LoadSceneMode.Single);
    }

    public void Client()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Connected");
    }
}
