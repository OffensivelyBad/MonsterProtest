using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> {
    
    private Enemy enemy = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.inputString != "") {
            AttackEnemy(Input.inputString);
        }
   	}

    private void AttackEnemy(string text) {
        if (!GotEnemy()) {
            return;
        }
        enemy.ProcessAttackText(text);
        CheckEnemyDead(enemy);
    }

    private bool GotEnemy() {
        bool gotEnemy = true;
        if (enemy == null) {
            enemy = GameManager.Instance.GetEnemy();
            gotEnemy |= enemy != null;
        }
        return gotEnemy;
    }

    private void CheckEnemyDead(Enemy attackedEnemy) {
        bool enemyIsDead = attackedEnemy.IsEnemyDead();
        if (enemyIsDead)
        {
            GameManager.Instance.EliminateEnemy(attackedEnemy);
            enemy = null;
        }
    }
}
