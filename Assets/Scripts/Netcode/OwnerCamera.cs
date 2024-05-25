using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class OwnerCamera : NetworkBehaviour
{
  

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
        }

        transform.parent = null;
     
    }

   
}
