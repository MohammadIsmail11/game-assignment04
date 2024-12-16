using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.0f;                  // Normal movement speed of the enemy
    public float acceleratedSpeed = 4.0f;       // Speed when accelerating towards the player
    public float detectionRange = 1.5f;         // Range for detecting obstacles
    public float accelerationRange = 3.0f;      // Range within which enemy accelerates towards the player
    public float turnRate = 90f;                // Rate at which the enemy can change direction
    private Vector2 direction;                  // Current movement direction
    private LayerMask obstacleLayer;            // Layer to detect obstacles (walls, etc.)
    private Transform player;                   // Reference to the player's transform

    private void Start()
    {
        // Set a random initial direction
        SetRandomDirection();

        // Set the layer mask for obstacle detection (assuming "Wall" is the obstacle layer)
        obstacleLayer = LayerMask.GetMask("Wall");

        // Find the player by tag (assuming the player has a "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Move the enemy in the current direction
        float currentSpeed = GetCurrentSpeed();
        transform.Translate(direction * currentSpeed * Time.deltaTime);

        // Check if there's an obstacle in the way and avoid it
        DetectObstacle();

        // Check if the enemy is outside the viewport and bounce if needed
        CheckBounds();
    }

    private void SetRandomDirection()
    {
        // Set a random direction for the enemy to start moving
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
    }

    private float GetCurrentSpeed()
    {
        // Calculate the distance to the player 
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // If the player is within the acceleration range, return accelerated speed
            if (distanceToPlayer <= accelerationRange)
            {
                // Gradually turn towards the player for smoother following
                Vector2 directionToPlayer = (player.position - transform.position).normalized;
                direction = Vector2.Lerp(direction, directionToPlayer, turnRate * Time.deltaTime).normalized;

                return acceleratedSpeed;
            }
        }

        // If player is out of range, return normal speed
        return speed;
    }

    private void DetectObstacle()
    {
        // Cast a ray to check if there is an obstacle in front of the enemy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, obstacleLayer);

        // If an obstacle is detected, change direction
        if (hit.collider != null)
        {
            // Avoid the obstacle by changing direction
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        // Randomly change the direction by a certain angle (to avoid obstacles)
        float randomTurnAngle = Random.Range(-turnRate, turnRate);
        direction = RotateDirection(direction, randomTurnAngle);
    }

    private Vector2 RotateDirection(Vector2 currentDirection, float angle)
    {
        // Rotate the direction vector by the specified angle
        float radianAngle = angle * Mathf.Deg2Rad;
        float newX = currentDirection.x * Mathf.Cos(radianAngle) - currentDirection.y * Mathf.Sin(radianAngle);
        float newY = currentDirection.x * Mathf.Sin(radianAngle) + currentDirection.y * Mathf.Cos(radianAngle);
        return new Vector2(newX, newY).normalized;
    }

    private void CheckBounds()
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Bounce horizontally if out of bounds
        if (viewportPosition.x < 0 || viewportPosition.x > 1)
        {
            direction.x = -direction.x; // Reverse x direction
            CorrectPosition(viewportPosition);
        }

        // Bounce vertically if out of bounds
        if (viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            direction.y = -direction.y; // Reverse y direction
            CorrectPosition(viewportPosition);
        }
    }

    private void CorrectPosition(Vector2 viewportPosition)
    {
        // Adjust the enemy position to keep it just inside the viewport
        Vector2 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(
            Mathf.Clamp(viewportPosition.x, 0.05f, 0.95f),
            Mathf.Clamp(viewportPosition.y, 0.05f, 0.95f),
            transform.position.z
        ));
        transform.position = worldPosition;
    }

    private void OnDrawGizmos()
    {
        // Draw the detection range and acceleration range in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, accelerationRange);
    }
}
