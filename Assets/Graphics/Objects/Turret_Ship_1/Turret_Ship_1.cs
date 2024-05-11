using UnityEngine;

public class Turret_Ship_1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 direction = Vector3.back; // Direction of movement
    public float speed = 5f; // Speed of movement

    [Header("Turret Settings")]
    public GameObject projectile; // Prefab of the projectile
    public Transform projectileSpawnPoint; // The point where projectiles will spawn
    public float projectileSpeed = 10f; // Speed of the projectile
    public float cooldown = 1f; // Cooldown between shots
    public Transform target; // Target to aim at
    public float distanceThreshold = 1000f; // Distance threshold for shooting

    private float lastFireTime; // Time when the last shot was fired

    void Update()
    {
        // Move the object according to the selected direction and speed
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        // Check if the target is within shooting distance
        if (target != null && Vector3.Distance(transform.position, target.position) <= distanceThreshold)
        {
            // Check if enough time has passed to fire again
            if (Time.time - lastFireTime > cooldown)
            {
                // Aim at the target
                Vector3 directionToTarget = target.position - projectileSpawnPoint.position;
                Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
                projectileSpawnPoint.rotation = rotationToTarget;

                // Fire a projectile
                FireProjectile();
            }
        }
    }

    void FireProjectile()
    {
        // Instantiate a projectile at the spawn point
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Get the rigidbody of the projectile and set its velocity
        Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.velocity = projectileSpawnPoint.forward * projectileSpeed;
        }

        // Update the last fire time
        lastFireTime = Time.time;
    }
}
