using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Ship_1_Bullet_Explosion : MonoBehaviour
{
    public GameObject ObstacleObject;
    public Explosion_1 OnCollisionFX;

    public float rotationSpeed = .45f; 
    float rotationSpeedBase = 50f; 

    private void Start()
    {
        Initialize();

    }

    public void Initialize()
    {
        ObstacleObject.SetActive(true);
        OnCollisionFX.gameObject.SetActive(false);
        OnCollisionFX.enabled = false;
        rotationSpeedBase = Random.Range(0f, 60f);
    }

    private void OnTriggerEnter(Collider otherobject)
    {
        if (ObstacleObject.activeInHierarchy == false) return;

        Debug.Log("HIITTT");

        if (otherobject.gameObject.CompareTag("Player"))
        {
            ObstacleObject.SetActive(false);
            OnCollisionFX.gameObject.SetActive(true);
            OnCollisionFX.enabled = true;
        }

        if (otherobject.gameObject.CompareTag("Bullet"))
        {
            ObstacleObject.SetActive(false);
            OnCollisionFX.gameObject.SetActive(true);
            OnCollisionFX.enabled = true;
        }
    }

   

    void Update()
    {
        //check if gameobject is active
        if(ObstacleObject.activeInHierarchy)
        {      
            // calculate rotation speed
                     
            transform.Rotate(Vector3.forward, (rotationSpeed*rotationSpeedBase) * Time.deltaTime);
        }
    }
}
