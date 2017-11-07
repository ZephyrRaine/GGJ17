using UnityEngine;
using UnityEngine.SceneManagement;
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
	private Rigidbody2D myRigidbody2D;
    private Animator anim;
	private Vector2 lastKnown;
	private int flapNumb, frameCooldown;
	private float xprev = 0f;

	public float flapCount, diveSpeed;
	public Vector2 speed = new Vector2(0f, 10f);
	public float rotationSpeed = 200f, dashCooldown = 2f, dashSpeed = 1000f, gravityWater = 0f, gravityAir = 0.5f, inertia = 300f;

	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		flapNumb = 0;
		dashCooldown *= 60f;
		frameCooldown = 0;
		myRigidbody2D.inertia = inertia;
        anim = GetComponentInChildren<Animator>();
	}

    void FixedUpdate()
    {
		if(transform.position.x <= GameObject.FindWithTag("Barbarus").GetComponent<Rigidbody2D>().transform.position.x + 0.2f) {
			SceneManager.LoadScene(0);
		}

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.fixedDeltaTime;
        transform.Rotate(0, 0, -rotation);

		if(transform.position.y <= 0) {
			if(transform.position.x <= GameObject.FindWithTag("Barbarus").transform.position.x + 0.2f) {
                     SceneManager.LoadScene(2);
            }
            flapNumb = 0;
			myRigidbody2D.gravityScale = gravityWater;
			if(Input.GetButtonDown("Jump") && frameCooldown == 0) {
                anim.SetTrigger("Dash");
				Vector2 direction = lastKnown - (Vector2)(transform.position);
				direction.Normalize();
				myRigidbody2D.AddForce((Vector2)transform.up * dashSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
				frameCooldown++;
			} else {
				lastKnown = (Vector2)transform.position;
				myRigidbody2D.AddRelativeForce(Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
			}
			if(frameCooldown > dashCooldown)
				frameCooldown = 0;
			else if(frameCooldown > 0)
				frameCooldown++;
		} else {
			myRigidbody2D.gravityScale = gravityAir;
			if(Input.GetButtonDown("Jump") && flapNumb < flapCount) {
				flapNumb++;
                anim.SetTrigger("Flap");
				Vector2 flap = Vector2.up * 250f;
				myRigidbody2D.AddForce(flap * Time.fixedDeltaTime, ForceMode2D.Impulse);
			} else if(xprev - transform.position.y > 0) {
				myRigidbody2D.AddForce(Vector2.down * diveSpeed * (xprev - transform.position.y) * Time.fixedDeltaTime, ForceMode2D.Impulse);
			}
			xprev = transform.position.y;
		}
        anim.SetFloat("Yposition", transform.position.y);
		Camera camera = Camera.main;
    }
}