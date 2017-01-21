using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	/*private float mapX = 100.0f, mapY = 100.0f;
	private float minX, maxX, minY, maxY;

	function Start() {
		float vertExtent = Camera.orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;

		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2.0;
		maxX = mapX / 2.0 - horzExtent;
		minY = vertExtent - mapY / 2.0;
		maxY = mapY / 2.0 - vertExtent;
	}

	function LateUpdate() {
		var v3 = transform.position;
		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);
		transform.position = v3;
	}*/
}