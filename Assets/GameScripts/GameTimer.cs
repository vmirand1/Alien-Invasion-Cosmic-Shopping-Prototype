using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;
    public GameObject player;
    public GameObject confirmationPanel;
    public GameObject gameOverPanel;
    public GameObject congratulationsPanel;
    public PauseMenu pauseMenu;

    private bool timerIsRunning = false;
    private Counter counter; // Reference to Counter script

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        counter = Counter.Instance;
        timerIsRunning = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (congratulationsPanel != null)
        {
            congratulationsPanel.SetActive(false); 
        }

        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
                CheckPlayerPosition();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                CheckGameOverCondition();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = "Time Remaining: " + Mathf.CeilToInt(timeRemaining).ToString();
        }
    }

    private void CheckPlayerPosition()
    {
        if (counter != null && counter.particleSystemParent != null && counter.particleSystemParent.activeSelf)
        {
            float distance = Vector3.Distance(player.transform.position, counter.particleSystemParent.transform.position);

            if (distance <= 1.0f)
            {
                timerIsRunning = false;
                confirmationPanel.SetActive(true); // Display the confirmation panel
            }
        }
    }

    private void CheckGameOverCondition()
    {
        // Check if the player made the target count
        if (counter != null && counter.total >= counter.target)
        {
            GameOver("You didn't make it back to the ship in time!");
        }
        else
        {
            GameOver("You didn't make the target count, you are fired!");
        }
    }

    public void GameOver(string message)
    {
        if (pauseMenu != null)
        {
            pauseMenu.ShowGameOver(message);
        }
    }

    // method to show the congratulations panel
    public void ShowCongratulationsPanel()
    {
        if (congratulationsPanel != null)
        {
            congratulationsPanel.SetActive(true); // Activate the congratulations panel
        }
    }

    // Method to handle level completion after confirmation
    public void OnConfirmationYes()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false); // Hide the confirmation panel
        }

        // Show the congratulations panel
        ShowCongratulationsPanel();
    }

    public void LevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void CancelCompletion()
    {
        confirmationPanel.SetActive(false);
        timerIsRunning = true;
    }
}

