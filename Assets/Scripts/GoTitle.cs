using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoTitle : MonoBehaviour {

	void Start() {
		StartCoroutine(Defeat());
	}

	IEnumerator Defeat() {
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(0);
	}
}