using UnityEngine;
using System.Collections;

public class EnemyBarbarus : MonoBehaviour {

	private GameObject player;
	private Rigidbody2D rigidbody2D;
	public float forceAmount;
	private bool barbarusRange;

	void Start() {
		player = GameObject.FindWithTag("Player");
		rigidbody2D = player.GetComponent<Rigidbody2D>();
		barbarusRange = true;
	}

	void FixedUpdate() {
		if(barbarusRange) {
			rigidbody2D.position += Vector2.left * forceAmount / 60f;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			barbarusRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			barbarusRange = false;
		}
	}
}