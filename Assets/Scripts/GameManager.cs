using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;
using UnityEngine.UI;

public enum GameState {
    playing, gameOver
}

public class GameManager : Singleton<GameManager> {

    // SerializeFields
    [SerializeField] private Player player = null;
    [SerializeField] private int totalEnemies = 5;
    [SerializeField] private Enemy[] enemies = null;
    [SerializeField] private GameObject spawnPoint = null;
    [SerializeField] private int maxEnemiesOnScreen = 4;
    [SerializeField] private float spawnDelay = 1.5f;
    [SerializeField] private Text scoreText = null;

    // Private
    private GameState state = GameState.playing;
    private readonly List<Enemy> enemyList = new List<Enemy>();
    private Enemy focusedEnemy;
    private int enemiesEliminated = 0;
    private AudioSource audioPlayer;
    private bool timerRunning = false;
    private float totalTime = 0.0f;
    private int numberOfWords = 0;
    private int wordsPerMinute = 0;

    // Public
    public GameState State {
        get { return state; }
    }

	private void Awake()
	{
        Assert.IsNotNull(player);
	}

	// Use this for initialization
	private void Start () {
        audioPlayer = GetComponent<AudioSource>();
        StartCoroutine(SpawnEnemy());
	}

	private void Update()
	{
        if (timerRunning) {
            totalTime += Time.deltaTime;
        }
	}

	IEnumerator SpawnEnemy() {
        if (enemiesEliminated + enemyList.Count < totalEnemies && enemyList.Count < maxEnemiesOnScreen)
        {
            string phrase = PhraseManager.Instance.GetRandomPhrase();
            Enemy newEnemy = Instantiate(GetRandomEnemy());
            newEnemy.transform.position = spawnPoint.transform.position;
            RegisterEnemy(newEnemy);
            newEnemy.EnemyText = phrase;
        } else {
            state = GameState.gameOver;
        }
        SetFocusedEnemy();
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnEnemy());
    }

    private void SetFocusedEnemy() {
        if (enemyList.Count > 0) {
            focusedEnemy = enemyList[0];
        }
    }

    private Enemy GetRandomEnemy() {
        int randomIndex = Random.Range(0, enemies.Length - 1);
        return enemies[randomIndex];
    }

    // Manage enemies
    private void RegisterEnemy(Enemy enemy) {
        enemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) {
        enemyList.Remove(enemy);
        enemy.Agent.enabled = false;
        KillEnemy(enemy);
    }

    private void KillEnemy(Enemy enemy) {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(GetRandomForceVector(), ForceMode.Impulse);
        rb.AddTorque(GetRandomForceVector() * 10);
        PlaySound(AudioManager.Instance.Death);
    }

    private Vector3 GetRandomForceVector() {
        float x = Random.Range(-10, 10);
        float y = Random.Range(5, 15);
        float z = Random.Range(1, 20);
        return new Vector3(x, y, z);
    }

    public void EliminateEnemy(Enemy enemy) {
        timerRunning = false;
        numberOfWords += GetNumberOfWords(enemy.EnemyText);
        SetScore(numberOfWords, totalTime);
        UnregisterEnemy(enemy);
        enemiesEliminated += 1;
        StartEnemies();
        StartCoroutine(SpawnEnemy());
    }

    private int GetNumberOfWords(string text)
    {
        return text.Split(' ').Length;
    }

    public Enemy GetEnemy() {
        return focusedEnemy;
    }

    public void StopEnemies() {
        foreach(Enemy enemy in enemyList) {
            enemy.StopMoving();
        }
    }

    private void StartEnemies() {
        foreach (Enemy enemy in enemyList)
        {
            enemy.ContinueMoving();
        }
    }

    public void PlaySound(AudioClip clip) {
        audioPlayer.PlayOneShot(clip);
    }

    public void ResumeTimer() {
        timerRunning = true;
    }

    private void SetScore(int words, float time) {
        int wpm = (int)(((float)words / time) * 60f);
        scoreText.text = "WPM: " + wpm;
    }
}
