using UnityEngine;
using System.Collections;

public class BoundToBackground : MonoBehaviour {
	private GameObject player;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;

	[HideInInspector]
	public bool onScreen;

	void Start() {
		player = GameObject.FindWithTag("Player");
		rb = player.GetComponent<Rigidbody2D>();
		onScreen = true;
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			onScreen = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			onScreen = false;
		} else Destroy(other.gameObject);
	}
}