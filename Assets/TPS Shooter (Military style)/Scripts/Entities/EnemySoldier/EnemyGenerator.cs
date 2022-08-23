using UnityEngine;

using LightDev;

namespace TPSShooter
{

  public class EnemyGenerator : MonoBehaviour
  {
    public GenerationWave[] generationWaves;

    private void Start()
    {
      SpawnEnemies();

      Events.EnemyKilled += OnEnemyKilled;
    }

    private void OnDestroy()
    {
      Events.EnemyKilled -= OnEnemyKilled;
    }

    private int _currentWaveIndex;
    private int _aliveEnemies;

    private void SpawnEnemies()
    {
      if (_currentWaveIndex >= generationWaves.Length)
        return;

      foreach (Transform spawnPoint in generationWaves[_currentWaveIndex].spawnPoints)
      {
        Instantiate(generationWaves[_currentWaveIndex].enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        _aliveEnemies++;
      }
    }

    private void OnEnemyKilled(EnemyBehaviour enemy)
    {
      _aliveEnemies--;

      if (_aliveEnemies != 0) return;

      _currentWaveIndex++;
      SpawnEnemies();
    }

    [System.Serializable]
    public class GenerationWave
    {
      public GameObject enemyPrefab;
      public Transform[] spawnPoints;
    }

  }

}