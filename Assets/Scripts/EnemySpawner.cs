using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float spawnInterval = 10f; // Interval between spawns
    public float spawnRadius = 0.5f; // Radius around each spawn point to check for other objects
    public int maxEnemies = 4; // Maximum number of enemies
    private int currentEnemyCount = 0; // Tracks the number of spawned enemies

    void Start()
    {
        // Start spawning enemies after an initial delay
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (currentEnemyCount < maxEnemies) // Check if there are less than max enemies
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Attempt to find an empty space within the game area
            bool foundPosition = false;
            Vector2 spawnPosition = Vector2.zero;
            int maxAttempts = 10; // Limit attempts to find an unoccupied space

            for (int i = 0; i < maxAttempts; i++)
            {
                // Generate a random position within the game area (adjust the range based on your game area size)
                spawnPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));

                // Check if the area is clear by overlapping with other colliders
                if (!Physics2D.OverlapCircle(spawnPosition, spawnRadius))
                {
                    foundPosition = true;
                    break;
                }
            }

            // If a clear position was found, spawn the enemy
            if (foundPosition)
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                currentEnemyCount++; // Increase the enemy count after spawning

                // Optionally, you can print the current number of enemies for debugging
                Debug.Log("Enemies spawned: " + currentEnemyCount);
            }
            else
            {
                // If no position was found, continue to the next attempt (not spawning anything)
                Debug.Log("No available position for enemy spawn.");
            }
        }
    }

    // Reset the enemy count when needed (e.g., when the game restarts)
    public void ResetEnemyCount()
    {
        currentEnemyCount = 0;
    }
}
