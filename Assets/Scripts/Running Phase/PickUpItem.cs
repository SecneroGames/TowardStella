using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PickupType
{
    CREDIT,
    TIME
}
public class PickUpItem : MonoBehaviour
{
    [Header("Setup")]
    public float maxRotationSpeed = 40f;
    public float speedFrequency = 1f;
    [Header("Respawn setting")]
    public float respawnTime;
    public int respawnLimit;
    int collectedCounter;

    [Space]
    public GameObject PickupObject;
    public PickupType pickupType;

    bool PickedUp = false;
    private void Start()
    {
        PickedUp = true;
        collectedCounter = 0;
        PickupObject.SetActive(false);
        //randomize spawn time
        StartCoroutine(Spawn(Random.Range(0,1.2f)));
    }
    void Update()
    {
        if (PickedUp) return;
 
        float rotationSpeed = maxRotationSpeed * Mathf.Cos(Time.time * speedFrequency);
        PickupObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PickedUp) return;
        //ignore if not player
        if (!other.CompareTag("Player")) return;

        AudioManager.instance.PlaySFX("PickupSFX");
        RunningPhaseManager.instance.OnPickup(pickupType);

        PickedUp = true;
        PickupObject.SetActive(false);
        collectedCounter++;
       
        if(collectedCounter<respawnLimit)
            StartCoroutine(Respawn());
    }


    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        PickupObject.SetActive(true);
        PickedUp = false;
    }


    IEnumerator Spawn(float initialWaitTime)
    {
        yield return new WaitForSeconds(initialWaitTime);
        PickupObject.SetActive(true);
        PickedUp = false;
    }
}
