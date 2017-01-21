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

[System.Serializable]
public class Boundary {
	public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
    public Vector2 speed = new Vector2(0f, 10.0F);
    public float rotationSpeed = 200.0f;
	private Rigidbody2D rigidbody2D;
	private Vector2 translation;
	private float flapCheck;

	public Boundary boundary;
	public float flapLaps;

    [HideInInspector]
    public bool colliding;

	void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		flapCheck = 0f;
	}

    void FixedUpdate()
    {
        //Deal with rotation first
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.fixedDeltaTime;
        transform.Rotate(0, 0, -rotation);

		if(transform.position.y <= 0) {
			flapCheck = 0f;
			rigidbody2D.gravityScale = 0f;
			translation = Input.GetAxis("Vertical") * speed;
			rigidbody2D.AddRelativeForce(translation * Time.fixedDeltaTime, ForceMode2D.Impulse);
		} else if(Input.GetButton("Fire1") && flapCheck - flapLaps < Time.realtimeSinceStartup) {
			flapCheck = Time.realtimeSinceStartup;
			rigidbody2D.gravityScale = 1f;
			rigidbody2D.AddRelativeForce(translation * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}

        Camera camera = Camera.main;

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        colliding = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }    
}