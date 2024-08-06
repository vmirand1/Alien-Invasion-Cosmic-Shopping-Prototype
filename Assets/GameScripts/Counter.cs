using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    public static Counter Instance { get; private set; }

    public TextMeshProUGUI totalText;
    public TextMeshProUGUI objectiveText;
    public int target = 3;
    public int total;
    public GameObject particleSystemParent;

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
        total = 0;
        UpdateTotalText();
        SetObjective("Objective: Collect all hotdog carts");

        if (particleSystemParent != null)
        {
            particleSystemParent.SetActive(false);
        }
    }

    public void IncreaseTotal()
    {
        total++;
        UpdateTotalText();
        CheckObjectiveCompletion();
    }

    public void DecreaseTotal()
    {
        total--; // Decrease the item count
        UpdateTotalText();

        if (total < 0)
        {
            GameOver(); // Trigger Game Over logic if total is less than 0
        }
        else
        {
            CheckObjectiveCompletion();
        }
    }

    private void UpdateTotalText()
    {
        if (totalText != null)
        {
            totalText.text = "\n\nHotdog Carts: " + total.ToString() + "/" + target.ToString();
        }
    }

    private void SetObjective(string objective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = objective;
        }
    }

    private void CheckObjectiveCompletion()
    {
        if (total >= target)
        {
            SetObjective("Objective Complete!\nGo to the building with the bowling pins.");
            if (particleSystemParent != null)
            {
                particleSystemParent.SetActive(true); // Activate the ship or particle effect
            }
        }
        else
        {
            SetObjective("Objective: Collect all hotdog carts");
            if (particleSystemParent != null)
            {
                particleSystemParent.SetActive(false); // Deactivate the ship or particle effect
            }
        }
    }

    private void GameOver()
    {
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.GameOver("You have been arrested!");
        }
    }
}
