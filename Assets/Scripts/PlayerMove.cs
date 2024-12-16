using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3.0f;
    public Text lifeText;            // Assign the LifeText UI in the Inspector
    public Text timerText;           // Assign a Timer Text UI for power mode countdown
    public Text winText;             // Assign a "You Win" Text UI in the Inspector
    public Text loseText;             // Assign a "You Win" Text UI in the Inspector
    private int lives = 3;           // Pacman starts with 3 lives
    private int enemiesKilled = 0;   // Track the number of enemies destroyed
    private bool isPowerModeActive = false;
    private Vector2 movement = new Vector2();
    private Rigidbody2D rb2D;
    private Animator animator;

    enum CharStates
    {
        MoveRight = 1,
        MoveLeft = 2,
        MoveDown = 3,
        MoveUp = 4,
        Ideal = 5
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateLifeText();
        timerText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false); // Hide "You Win" message initially
        StartCoroutine(StartPowerModeTimer());
    }

    void Update()
    {
        UpdateState();
    }

    void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        rb2D.velocity = movement * speed;
    }

    private void UpdateState()
    {
        if (movement.x > 0) animator.SetInteger("AnimationState", (int)CharStates.MoveRight);
        else if (movement.x < 0) animator.SetInteger("AnimationState", (int)CharStates.MoveLeft);
        else if (movement.y > 0) animator.SetInteger("AnimationState", (int)CharStates.MoveUp);
        else if (movement.y < 0) animator.SetInteger("AnimationState", (int)CharStates.MoveDown);
        else animator.SetInteger("AnimationState", (int)CharStates.Ideal);
    }

    private void UpdateLifeText()
    {
        lifeText.text = "Lives: " + lives;
    }

    public void DecreaseLife()
    {
        lives--;
        UpdateLifeText();

        if (lives <= 0)
        {
            loseText.gameObject.SetActive(true); // Show "Lose Win" message

            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f); // Delay before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator StartPowerModeTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);

            isPowerModeActive = true;
            timerText.gameObject.SetActive(true);

            for (int i = 10; i > 0; i--)
            {
                timerText.text = "Power Mode: " + i;
                yield return new WaitForSeconds(1f);
            }

            isPowerModeActive = false;
            timerText.gameObject.SetActive(false);
        }
    }

    public bool IsPowerModeActive()
    {
        return isPowerModeActive;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;

        // Check if player has killed 4 enemies
        if (enemiesKilled >= 4)
        {
            winText.gameObject.SetActive(true); // Show "You Win" message
            StartCoroutine(RestartGame());
        }
    }
}
