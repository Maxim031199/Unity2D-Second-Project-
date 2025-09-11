using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private EnemySpawner spawner;
    private float endScreenDelay = 1.3f;
    private int activeEnemies = 0;

    private void OnEnable()
    {
        Events.GameOver += OnGameOver;
        Events.EnemyKilled += OnEnemyKilled;
        Events.EnemySpawned += OnEnemySpawned; 
    
}
    private void OnGameOver()
    {
        if (playerAnimator) playerAnimator.SetTrigger("IsDead");
        StartCoroutine(EndGame());
    }

    private void OnEnemySpawned()
    {
        activeEnemies++;
    }

    private void OnEnemyKilled()
    {
        activeEnemies--;
        if (activeEnemies <= 0)
        {
            StartCoroutine(GameWon());
        }
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(endScreenDelay);
        Time.timeScale = 0;
        Debug.Log("Player Died! Game Over!");
    }
    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(endScreenDelay);
        Time.timeScale = 0;
        Debug.Log("All enemies defeated! You win!");
    }
   private void OnDisable()
    {
        Events.GameOver -= OnGameOver;
        Events.EnemyKilled -= OnEnemyKilled;
        Events.EnemySpawned -= OnEnemySpawned; 
    }
}
