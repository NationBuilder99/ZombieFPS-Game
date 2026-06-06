using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int enemiesAlive = 0;
    public int round = 0;

    [Header("Spawn Settings")]
    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;

    [Header("UI")]
    public Text roundNumber;
    public Text roundSurvivde;
    public GameObject endScreen;

    public float waveDelay = 3f;

    private bool waveActive = false;

    void Update()
    {
        if (!waveActive && enemiesAlive <= 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        waveActive = true;

        yield return new WaitForSeconds(waveDelay);

        round++;

        roundNumber.text = "Round: " + round;

        for (int x = 0; x < round; x++)
        {
            SpawnEnemy();
        }

        waveActive = false;
    }

    void SpawnEnemy()
    {
        GameObject spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemySpawned = Instantiate(
            enemyPrefab,
            spawnPoint.transform.position,
            Quaternion.identity
        );

        EnemyManager enemy = enemySpawned.GetComponent<EnemyManager>();
        enemy.gameManager = this;

        enemiesAlive++;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

        endScreen.SetActive(true);

        roundSurvivde.text = round.ToString();
    }
}
