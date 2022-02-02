using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] ObjectPool pooledEnemies;
    [SerializeField] float spawnDelay;
    [SerializeField] int amountPerWave;
    [SerializeField] float spawnRadius;
    [SerializeField] GameEvent SpawnedEnemy;
    [SerializeField] GameEvent WaveCompleted;
    [SerializeField] bool exponential;
    [SerializeField] Material enemiesLeft;
    [Space(10)]
    [SerializeField] int currentWave = 1;
    bool IsFinishedWave;
    bool startedInvoke;
    bool newGame = true;
    public readonly List<GameObject> _currentEnemies = new List<GameObject>();

    void LateUpdate()
    {
        IsFinishedWave = _currentEnemies.Count == 0;
        if (IsFinishedWave && !startedInvoke)
        {
            if (!newGame)
                WaveCompleted?.Invoke(gameObject);
            newGame = false;
            startedInvoke = true;
            Invoke(nameof(SpawnWave), spawnDelay);
        }
    }

    [ContextMenu("Spawn Wave")]
    void SpawnWave()
    {
        startedInvoke = false;
        if (pooledEnemies == null) return;
        int num;
        if (exponential)
            num = amountPerWave * currentWave;
        else
            num = amountPerWave + currentWave;
        for (int i = 0; i < num; i++)
        {
            GameObject e = pooledEnemies.GetObject();
            if (e != null)
            {
                _currentEnemies.Add(e);
                e.transform.position = transform.position + Random.onUnitSphere * spawnRadius;
                SpawnedEnemy?.Invoke(e);
                e.SetActive(true);
            }
        }
        UpdateEnemiesLeft();
        ShiftState.S.ShiftToState(WavelengthControler.CurrentWaveType);
        currentWave++;
    }

    public void RemoveEnemyFromList(GameObject obj)
    {
        _currentEnemies.Remove(obj);
        UpdateEnemiesLeft();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void UpdateEnemiesLeft()
    {
        if (enemiesLeft == null) return;
        int num;
        if (exponential)
            num = amountPerWave * currentWave;
        else
            num = amountPerWave + currentWave;
        float clampVal = Utils.RemapClamped(_currentEnemies.Count, 0, num, 0, 1);
        enemiesLeft.SetFloat("_XFill", clampVal);
    }
}
