using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private BoxCollider2D spawnArea;

    [Serializable]
    private class Spawnable
    {
        public GameObject prefab;
        public int chance;
        public float cooldown;
        public float cooldownTimer;
    }

    [Serializable]
    private struct EnemyWave
    {
        public List<Spawnable> spawnables;
        public float duration;
        public float frequency;

    }

    [SerializeField] private List<EnemyWave> enemyWaves;
    private float waveTimer;
    private int currentWave = 0;
    private float spawnTimer;



    // Start is called before the first frame update
    void Start()
    {
        StartWave(0);
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            StartWave(currentWave + 1);
        }

        UpdateSpawnableTimers(currentWave);
        spawnTimer = spawnTimer - Time.deltaTime;
        if (spawnTimer <= 0)
        {
            Spawn(PickSpawnable(currentWave));
            spawnTimer = enemyWaves[currentWave].frequency;
        }
    }

    private void StartWave(int wave)
    {
        if (wave >= enemyWaves.Count) wave = enemyWaves.Count - 1;

        currentWave = wave;
        waveTimer = enemyWaves[wave].duration;
        spawnTimer = enemyWaves[wave].frequency;
        ResetSpawnableTimers(wave);
    }

    private Spawnable PickSpawnable(int wave)
    {
        Spawnable result = null;
        int weightsum = 0;
        foreach (Spawnable spawnable in enemyWaves[wave].spawnables)
        {
            if (spawnable.cooldownTimer > 0) continue;
            weightsum += spawnable.chance;
        }
        int selection = UnityEngine.Random.Range(0, weightsum);
        foreach (Spawnable spawnable in enemyWaves[wave].spawnables)
        {
            if (spawnable.cooldownTimer > 0) continue;
            if (selection < spawnable.chance)
            {
                result = spawnable;
                break;
            }
            else
            {
                selection -= spawnable.chance;
            }
        }
        return result;
    }

    private void Spawn(Spawnable spawnable)
    {
        Debug.Log(spawnable);
        if (spawnable == null) return;

        Vector3 position = new Vector3();
        position.x = UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);

        ObjectPoolManager.Instance.RequestObjectAt(spawnable.prefab, position);

        spawnable.cooldownTimer = spawnable.cooldown;
    }

    private void UpdateSpawnableTimers(int wave)
    {
        foreach (Spawnable spawnable in enemyWaves[wave].spawnables)
        {
            spawnable.cooldownTimer = Mathf.Max(0f, spawnable.cooldownTimer - Time.deltaTime);
        }
    }

    private void ResetSpawnableTimers(int wave)
    {
        foreach (Spawnable spawnable in enemyWaves[wave].spawnables)
        {
            spawnable.cooldownTimer = 0f;
        }
    }

}
