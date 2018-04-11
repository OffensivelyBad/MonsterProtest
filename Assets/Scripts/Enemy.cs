using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    // Serialized fields
    [SerializeField] private Vector3 startPosition = Vector3.zero;
    [SerializeField] private Vector3 destinationPosition = Vector3.zero;

    // Private
    private TextMesh signText;
    private NavMeshAgent agent;
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
        signText = FindObjectOfType<TextMesh>();
        agent = GetComponent<NavMeshAgent>();
        GameManager.Instance.RegisterEnemy(this);
        signText.text = enemyText;
        transform.localPosition = startPosition;
        agent.isStopped = true;
        MoveToDestination(destinationPosition);
	}

    private void MoveToDestination(Vector3 destination) {
        agent.destination = destination;
        agent.isStopped = false;
    }

    public void ProcessAttackText(string text) {
        foreach (char character in text.ToCharArray()) {
            if (remainingText.Length == 0) {
                return;
            }
            char[] remainingCharacters = remainingText.ToCharArray();
            if (remainingCharacters.Length > 0 && character == remainingCharacters[0]) {
                remainingText = remainingText.Substring(1, remainingText.Length - 1);
                signText.text = remainingText;
            }
        }
    }

    public bool IsEnemyDead() {
        return remainingText.Length == 0;
    }
}
