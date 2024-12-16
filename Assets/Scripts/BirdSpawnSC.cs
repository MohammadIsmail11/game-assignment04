using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawnSC : MonoBehaviour
{
    public GameObject Bird;          // The bird prefab to spawn
    float maxSpawnRate = 1f;         // The initial max spawn rate
    public float spawnRadius = 0.5f; // Radius around each spawn point to check for other objects

    void Start()
    {
        // Start the first spawn and setup the repeating increase in spawn rate
        Invoke("spawnBird", maxSpawnRate);
        InvokeRepeating("increaseInSpawning", 0f, 10f);
    }

    void spawnBird()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));   // Bottom-left corner
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));   // Top-right corner

        // Attempt to find an empty space within the viewport
        bool foundPosition = false;
        Vector2 spawnPosition = Vector2.zero;
        int maxAttempts = 10; // Limit attempts to find an unoccupied space

        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random position
            float randomX = Random.Range(min.x, max.x);
            float randomY = Random.Range(min.y, max.y);
            spawnPosition = new Vector2(randomX, randomY);

            // Check if the area is clear
            if (!Physics2D.OverlapCircle(spawnPosition, spawnRadius))
            {
                foundPosition = true;
                break;
            }
        }

        // If a clear position was found, spawn the bird
        if (foundPosition)
        {
            GameObject aBird = Instantiate(Bird);
            aBird.transform.position = spawnPosition;
        }

        // Schedule the next spawn
        scheduleNextSpawn();
    }

    void scheduleNextSpawn()
    {
        float spawnInSeconds = maxSpawnRate > 1f ? Random.Range(1f, maxSpawnRate) : 1f;
        Invoke("spawnBird", spawnInSeconds);  // Schedule the next bird spawn
    }

    void increaseInSpawning()
    {
        if (maxSpawnRate > 1f)
            maxSpawnRate--;

        if (maxSpawnRate < 1f)
            CancelInvoke("increaseInSpawning");
    }
}
