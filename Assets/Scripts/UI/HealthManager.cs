using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    int currentHealth = 100;
    string statusPrefix = "Health: ";
    TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = $"{statusPrefix}{currentHealth}";
        Events.PlayerHP += UpdatePlayerVitality;
    }

    

    private void UpdatePlayerVitality(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        scoreText.text = $"{statusPrefix}{currentHealth}";

        if (currentHealth <= 0)
        {
            Events.GameOver?.Invoke();   
        }
    }


    private void OnDestroy()
    {
        Events.PlayerHP -= UpdatePlayerVitality;
    }
}
