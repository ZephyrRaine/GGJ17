using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {
    public float scrollSpeed;
    [HideInInspector]
    public float tileSize = 38.5f;

	private Vector3 startPosition;

	void Start() {
		startPosition = transform.position;
	}

	void Update() {
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSize);
		transform.position = startPosition + Vector3.left * newPosition;
	}
}