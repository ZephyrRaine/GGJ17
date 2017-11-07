using UnityEngine;
using System.Collections;

public class TerrainScroller: MonoBehaviour {
    public float scrollSpeed;


	void Start() {
	}

	void Update() {
			foreach(Transform t in transform)
{
t.position += Vector3.left * Time.deltaTime * scrollSpeed;
}
	}
}