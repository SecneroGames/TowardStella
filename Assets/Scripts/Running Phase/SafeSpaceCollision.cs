using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpaceCollision : MonoBehaviour
{
    [SerializeField] private string transitionSceneName;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RunningPhaseManager.instance.EndGame(transitionSceneName);
            Destroy(gameObject);
        }
    }
}
