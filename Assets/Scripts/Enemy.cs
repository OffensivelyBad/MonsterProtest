using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    // Serialized fields
    [SerializeField] private Vector3 destinationPosition = Vector3.zero;

    // Private
    private TextMesh signText;
    private NavMeshAgent agent;
    private string enemyText = "";
    private string remainingText = "";
    private bool canAttack = false;

    // Public
    public string EnemyText {
        get { return enemyText; }
        set {
            enemyText = value;
            remainingText = value;
        }
    }

	// Use this for initialization
	private void Start () {
        signText = FindObjectOfType<TextMesh>();
        agent = GetComponent<NavMeshAgent>();
        signText.text = enemyText;
        signText.color = Color.black;
        agent.isStopped = true;
        MoveToDestination(destinationPosition);
	}

	private void Update()
	{
        if (agent.remainingDistance <= agent.stoppingDistance) {
            if (!(transform.rotation.y >= 0.998f && transform.rotation.y <= 1f))
            {
                GameManager.Instance.StopEnemies();
                transform.Rotate(Vector3.up, 100 * Time.deltaTime);
            }
            else {
                canAttack = true;
                signText.color = Color.red;
            }
        }
	}

	private void MoveToDestination(Vector3 destination) {
        agent.destination = destination;
        agent.isStopped = false;
    }

    public void ProcessAttackText(string text) {
        if (!canAttack) {
            return;
        }
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

    public void StopMoving() {
        agent.isStopped = true;
    }

    public void ContinueMoving() {
        agent.isStopped = false;
    }

}
