using UnityEngine;
using System.Collections;

public class BoundToBackground : MonoBehaviour {
	private GameObject player;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;

	void Start() {
		player = GameObject.FindWithTag("Player");
		rb = player.GetComponent<Rigidbody2D>();
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Player")) {
			rb.transform.position *= -2;
			player.GetComponent<Rigidbody2D>().AddRelativeForce(rb.transform.position, ForceMode2D.Impulse);
			/*boxCollider.offset = new Vector2(boxCollider.offset.x, -boxCollider.offset.y);
			background.transform.position = new Vector3(
				background.transform.position.x,
				-Mathf.Sign(background.transform.position.y),
				background.transform.position.z);*/
		} else Destroy(other.gameObject);
	}
}