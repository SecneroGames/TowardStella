using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpaceCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RunningPhaseManager.instance.EndGame();
            Destroy(gameObject);
        }
    }
}
