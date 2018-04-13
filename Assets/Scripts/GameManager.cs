using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    // Private
    private GameState state = GameState.playing;
    private readonly List<Enemy> enemyList = new List<Enemy>();
    private Enemy focusedEnemy;
    private int enemiesEliminated = 0;

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
        StartCoroutine(SpawnEnemy());
	}

    IEnumerator SpawnEnemy() {
        if (enemiesEliminated + enemyList.Count < totalEnemies && enemyList.Count < maxEnemiesOnScreen)
        {
            string phrase = PhraseManager.Instance.GetRandomPhrase();
            Enemy newEnemy = Instantiate(GetRandomEnemy());
            newEnemy.transform.position = spawnPoint.transform.position;
            RegisterEnemy(newEnemy);
            newEnemy.EnemyText = phrase;
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
        Destroy(enemy.gameObject);
    }

    public void EliminateEnemy(Enemy enemy) {
        UnregisterEnemy(enemy);
        enemiesEliminated += 1;
        StartEnemies();
        StartCoroutine(SpawnEnemy());
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
}
