using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager Instance { get; private set; }

    private int wave;
    private int maxWaveDifficultyScore;
    private int currentWaveDifficultyScore;
    private List<EnemySO> enemiesToSpawn;
    private float timeBetweenSpawns = 1f;
    private Vector3 spawnPosition;
    private List<BaseEnemy> enemiesLeft;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one WaveManager object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        spawnPosition = new Vector3(10, 0, 10);
    }

    public void SetupNextWave()
    {
        wave++;
        CalculateWaveDifficultyScore();
        SelectEnemiesToSpawn();
        StartCoroutine(SpawnWave());
    }

    private void CalculateWaveDifficultyScore()
    {
        maxWaveDifficultyScore = 500 + 100 * wave - Random.Range(10 * wave, 50 * wave);
    }

    private void SelectEnemiesToSpawn()
    {
        List<EnemySO> sortedEnemies = new List<EnemySO>(Resources.Load<EnemyHolder>("EnemyHolder").enemies);
        //Sort enemies by their difficulty score in descending order
        sortedEnemies.Sort((a, b) => b.difficultyScore.CompareTo(a.difficultyScore));

        enemiesToSpawn = new List<EnemySO>();

        currentWaveDifficultyScore = 0;
        foreach (EnemySO enemy in sortedEnemies)
        {
            while (currentWaveDifficultyScore + enemy.difficultyScore <= maxWaveDifficultyScore)
            {
                currentWaveDifficultyScore += enemy.difficultyScore;
                enemiesToSpawn.Add(enemy);
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        enemiesLeft = new List<BaseEnemy>();
        while(enemiesToSpawn.Count > 0)
        {
            BaseEnemy enemy = Instantiate(enemiesToSpawn[0].prefab, spawnPosition, Quaternion.identity).GetComponent<BaseEnemy>();
            enemiesLeft.Add(enemy);
            enemy.OnDeath += Enemy_OnDeath;
            enemiesToSpawn.RemoveAt(0);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void Enemy_OnDeath(object sender, System.EventArgs e)
    {
        BaseEnemy enemy = sender as BaseEnemy;
        enemiesLeft.Remove(enemy);
        enemy.OnDeath -= Enemy_OnDeath;

        if(enemiesLeft.Count == 0)
        {
            GameManager.Instance.SetCurrentGamePhase(GameManager.GamePhase.FARMING_PHASE);
        }
    }
}
