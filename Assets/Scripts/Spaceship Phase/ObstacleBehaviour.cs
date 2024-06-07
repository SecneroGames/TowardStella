using System.Collections;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public GameObject obstacleMesh;
    public Explosion_1 explosionEffect;
    private Collider obstacleCollider;

    public float rotationSpeed = 0.45f;
    private float rotationSpeedBase;

    private void Start()
    {
        Initialize();
        obstacleCollider = GetComponent<Collider>();
    }

    public void Initialize()
    {
        obstacleMesh.SetActive(true);
        explosionEffect.gameObject.SetActive(false);
        explosionEffect.enabled = false;
        rotationSpeedBase = Random.Range(0f, 60f);
        if (obstacleCollider != null)
        {
            obstacleCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!obstacleMesh.activeInHierarchy) return;

        Debug.Log("Hit detected");

        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {
            obstacleMesh.SetActive(false);
            explosionEffect.gameObject.SetActive(true);
            explosionEffect.enabled = true;

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (obstacleMesh.activeInHierarchy)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * rotationSpeedBase * Time.deltaTime);
        }
    }
}
