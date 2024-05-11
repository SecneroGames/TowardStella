using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Collider collider;
    public float bulletspeed = 150;
  
    private void Start()
    {
        StartCoroutine(Delay());
    }
     


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.20f);
        collider.enabled = true;
        Destroy(gameObject, 1.25f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletspeed * Time.deltaTime);
    }

   
}


