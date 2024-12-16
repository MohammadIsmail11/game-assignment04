using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is Pacman
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();

            if (playerMove != null)
            {
                if (playerMove.IsPowerModeActive())
                {
                    playerMove.EnemyKilled(); // Pacman kills the enemy in power mode
                    Destroy(gameObject);       // Destroy this enemy
                }
                else
                {
                    playerMove.DecreaseLife(); // Pacman loses a life when not in power mode
                }
            }
        }
    }
}
