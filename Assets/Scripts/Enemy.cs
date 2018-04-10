using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    // Private
    private Text textLabel;
    private string enemyText = "";
    private string remainingText = "";

    // Public
    public string EnemyText {
        get {
            return enemyText;
        }
        set {
            enemyText = value;
            remainingText = value;
        }
    }

	// Use this for initialization
	void Start () {
        textLabel = GetComponent<Text>();
        GameManager.Instance.RegisterEnemy(this);
        textLabel.text = enemyText;
	}

    public void ProcessAttackText(string text) {
        foreach (char character in text.ToCharArray()) {
            if (remainingText.Length == 0) {
                return;
            }
            char[] remainingCharacters = remainingText.ToCharArray();
            if (remainingCharacters.Length > 0 && character == remainingCharacters[0]) {
                remainingText = remainingText.Substring(1, remainingText.Length - 1);
            }
        }
    }

    public bool IsEnemyDead() {
        return remainingText.Length == 0;
    }
}
