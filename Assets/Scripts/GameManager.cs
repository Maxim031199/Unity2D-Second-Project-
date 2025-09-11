using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private EnemySpawner spawner;
    private int totalEnemies;
    private int killedEnemies;
    private float endScreenDelay = 2f;

    void Start()
    {
        totalEnemies = spawner != null ? spawner.spawnAmount : 0;
        killedEnemies = 0;
    }
    private void OnEnable()
    {
        Events.GameOver += OnGameOver;
        Events.EnemyKilled += OnEnemyKilled;
    }
    private void OnGameOver()
    {
        if (playerAnimator) playerAnimator.SetTrigger("IsDead");
        StartCoroutine(EndGame());
    }
    private void OnEnemyKilled()
    {
        killedEnemies++;
        if (killedEnemies >= totalEnemies)
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
    }
}
