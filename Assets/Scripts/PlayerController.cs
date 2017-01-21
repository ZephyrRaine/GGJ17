using UnityEngine;
using System.Collections;

public static class Vector2Extension {

	public static Vector2 Rotate(this Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rigidbody2D;
	private int flapNumb;

	public float flapCount;
	public Vector2 speed = new Vector2(0f, 10.0F);
	public float rotationSpeed = 200.0f;

	void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		flapNumb = 0;
	}

    void FixedUpdate()
    {
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.fixedDeltaTime;
        transform.Rotate(0, 0, -rotation);

		if(transform.position.y <= 0) {
			flapNumb = 0;
			rigidbody2D.gravityScale = 0f;
			rigidbody2D.AddRelativeForce(Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
		} else {
			rigidbody2D.gravityScale = 0.5f;
			if(flapCount < flapNumb || Input.GetButton("Fire3")) {
				rigidbody2D.gravityScale = 4f;
			} else if(Input.GetButtonDown("Fire1") && flapNumb < flapCount) {
				flapNumb++;
				Vector2 flap = Vector2.up * 250f;
				rigidbody2D.AddForce(flap * Time.fixedDeltaTime, ForceMode2D.Impulse);
			}
		}
        Camera camera = Camera.main;
    }
}