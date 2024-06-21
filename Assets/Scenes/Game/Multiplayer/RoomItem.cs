using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    PhotonLobbyManager manager;

    private void Start()
    {
        manager = FindObjectOfType<PhotonLobbyManager>();
    }
    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void onClickItem()
    {
        manager.JoinRoom(roomName.text);
    }

}

