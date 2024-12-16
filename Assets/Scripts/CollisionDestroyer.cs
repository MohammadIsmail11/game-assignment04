using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import to work with UI elements

public class CollisionDestroyer : MonoBehaviour
{
    public Text scoreText; // Reference to the Score Text UI element
    private int score = 0; // Initialize the score

    // This method is called when the collider enters another collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the tag "Destroyable"
        if (other.gameObject.CompareTag("Destroyable"))
        {
            // Destroy the object that was collided with
            Destroy(other.gameObject);

            // Increase the score
            score += 10; // Adjust the score increment as needed
            UpdateScoreUI(); // Update the score display
        }
    }

    // This method updates the score UI text
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString(); // Update the score text
    }
}
