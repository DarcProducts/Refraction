using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] ObjectPool pooledEnemies;
    [SerializeField] int amountPerWave;
    [SerializeField] float spawnRadius;
    [SerializeField] GameEvent SpawnedEnemy;
    [Space(10)]
    [SerializeField] int currentWave = 1;
    bool IsFinishedWave;
    public readonly List<GameObject> _currentEnemies = new List<GameObject>();

    void Update()
    {
        IsFinishedWave = _currentEnemies.Count == 0;
        if (IsFinishedWave)
        {
            print($"Finished wave!");
            SpawnWave();
        }
    }

    [ContextMenu("Spawn Wave")]
    void SpawnWave()
    {
        if (pooledEnemies == null) return;
        for (int i = 0; i < amountPerWave * currentWave; i++)
        {
            GameObject e = pooledEnemies.GetObject();
            if (e != null)
            {
                _currentEnemies.Add(e);
                e.transform.position = transform.position + Random.onUnitSphere * spawnRadius;
                SpawnedEnemy?.Invoke(gameObject);
                e.SetActive(true);
            }
        }
        currentWave++;
    }

    public void RemoveEnemyFromList(GameObject obj) => _currentEnemies.Remove(obj);
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
