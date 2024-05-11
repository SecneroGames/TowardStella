using UnityEngine;

public class Explosion_1 : MonoBehaviour
{
    public float duration = 2f;

    //private void Start()
    //{
    //    // Invoke the DestroyExplosion method after the specified duration.
    //    Invoke("DestroyExplosion", duration);
    //}

    private void OnEnable()
    {
        Invoke("DestroyExplosion", duration);
    }

    private void DestroyExplosion()
    {
        // Destroy the GameObject this script is attached to.
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
