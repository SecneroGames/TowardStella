using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class NetworkCameraManager : NetworkBehaviour
{
    private void Start()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
        }

        transform.parent = null;
    }
}
