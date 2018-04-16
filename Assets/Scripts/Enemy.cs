using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    // Serialized fields
    [SerializeField] private GameObject spawn = null;
    [SerializeField] private GameObject destination = null;

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
    public NavMeshAgent Agent {
        get { return agent; }
    }

	// Use this for initialization
	private void Start () {
        transform.position = spawn.transform.position;
        signText = FindObjectOfType<TextMesh>();
        agent = GetComponent<NavMeshAgent>();
        signText.text = enemyText;
        signText.color = Color.black;
        agent.isStopped = true;
        MoveToDestination(destination.transform.position);
	}

	private void Update()
	{
        if (!agent.isActiveAndEnabled) {
            if (transform.position.y < -10) {
                Destroy(gameObject);
            }
            return;
        }
        if (agent.remainingDistance <= agent.stoppingDistance) {
            GameManager.Instance.StopEnemies();
            if (!(transform.rotation.y >= 0.998f && transform.rotation.y <= 1f))
            {                
                transform.Rotate(Vector3.up, 200 * Time.deltaTime);
            }
            else {
                canAttack = true;
                signText.color = Color.red;
                GameManager.Instance.ResumeTimer();
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
                GameManager.Instance.PlaySound(AudioManager.Instance.Hit);
            } else {
                GameManager.Instance.PlaySound(AudioManager.Instance.Miss);
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
