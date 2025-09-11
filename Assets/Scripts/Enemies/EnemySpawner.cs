using UnityEngine;
using System.Collections;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject collectablePrefab;
    
    [SerializeField] public int spawnAmount = 12;
    [SerializeField] float spawnDelayMin = 1f;
    [SerializeField] float spawnDelayMax = 2.5f;
    [SerializeField] Transform minBound;
    [SerializeField] Transform maxBound;

    void Start()
    {
        StartCoroutine(SpawnGoblinsOverTime());
    }

    IEnumerator SpawnGoblinsOverTime()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
            var collectable = Instantiate(collectablePrefab, GetRandomSpawnPoint(), Quaternion.identity);
            Events.EnemySpawned?.Invoke();
        }
    }

    private Vector2 GetRandomSpawnPoint()
    {
        return new Vector2(Random.Range(minBound.position.x, maxBound.position.x), minBound.position.y);
    }
}
