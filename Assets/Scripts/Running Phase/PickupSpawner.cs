using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    // Prefab to spawn
    public GameObject prefabToSpawn;

    // Maximum number of prefabs allowed in the scene
    public int maxPrefabs = 10;

    // Interval between spawns
    public float spawnInterval = 2f;

    // Timer for spawn interval
    private float spawnTimer = 0f;

    // Define the boundary of the spawn area
    public Vector3 minSpawnBounds;
    public Vector3 maxSpawnBounds;

    // Update is called once per frame
    void Update()
    {
        // Increment the spawn timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn and if the number of prefabs is below the limit
        if (spawnTimer >= spawnInterval && CountPrefabs() < maxPrefabs)
        {
            SpawnPrefab();
            spawnTimer = 0f; // Reset the timer
        }
    }

    // Count the number of prefabs in the scene
    private int CountPrefabs()
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag(prefabToSpawn.tag);
        return prefabs.Length;
    }

    // Spawn a new prefab at a random location within the defined area
    private void SpawnPrefab()
    {
        // Generate a random position within the spawn area bounds
        Vector3 randomPosition = new Vector3(
            Random.Range(minSpawnBounds.x, maxSpawnBounds.x),
            Random.Range(minSpawnBounds.y, maxSpawnBounds.y),
            Random.Range(minSpawnBounds.z, maxSpawnBounds.z)
        );

        // Instantiate the prefab at the random position
        Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
    }
}
