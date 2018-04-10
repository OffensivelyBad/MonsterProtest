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
    [SerializeField] private Vector3 frontPosition = Vector3.zero;

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
        SpawnEnemy();
	}

    private void SpawnEnemy() {
        if (enemiesEliminated >= totalEnemies) {
            return;
        }
        string phrase = PhraseManager.Instance.GetRandomPhrase();
        Enemy newEnemy = Instantiate(GetRandomEnemy());
        newEnemy.transform.position = frontPosition;
        newEnemy.EnemyText = phrase;
        SetFocusedEnemy(newEnemy);
    }

    private void SetFocusedEnemy(Enemy enemy) {
        focusedEnemy = enemy;
    }

    private Enemy GetRandomEnemy() {
        int randomIndex = Random.Range(0, enemies.Length - 1);
        return enemies[randomIndex];
    }

    // Manage enemies
    public void RegisterEnemy(Enemy enemy) {
        enemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void EliminateEnemy(Enemy enemy) {
        UnregisterEnemy(enemy);
        enemiesEliminated += 1;
        SpawnEnemy();
    }

    public Enemy GetEnemy() {
        return focusedEnemy;
    }
}
