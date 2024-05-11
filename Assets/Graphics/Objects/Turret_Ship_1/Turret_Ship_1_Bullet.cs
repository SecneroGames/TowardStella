using UnityEngine;

public class Turret_Ship_1_Bullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 direction = Vector3.back; // Direction of movement
    public float speed = 30f; // Speed of movement
    void Update()
    {
        // Move the object according to the selected direction and speed
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
