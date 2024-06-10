using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class PlayerPanel : NetworkBehaviour
{
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    [SerializeField] private TextMeshProUGUI isReadyTxt;
    [SerializeField] private TextMeshProUGUI readyTxt;
    [SerializeField] private GameObject readyBtn;

    void Update()
    {
        if (IsClient)
        {
            if (isReady.Value)
            {
                isReadyTxt.text = "READY";
                readyTxt.text = "UNREADY";
            }
            else
            {
                isReadyTxt.text = "NOT READY";
                readyTxt.text = "READY";
            }
        }

        if (IsOwner)
        {
            readyBtn.SetActive(true);
        }
        else
        {
            readyBtn.SetActive(false);
        }
    }

    public void Ready()
    {
        if (IsOwner)
        {
            if (!isReady.Value)
            {
                isReady.Value = true;
            }
            else
            {
                isReady.Value = false;
            }
        }
    }
}
