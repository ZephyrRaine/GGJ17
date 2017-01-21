using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Vector2 speed = new Vector2(0f, 10.0F);
    public float rotationSpeed = 200.0f;

    [HideInInspector]
    public bool colliding;

    void FixedUpdate()
    {
        //Deal with rotation first
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, -rotation);

        {
			Vector2 translation = Input.GetAxis("Vertical") * speed;

            translation *= Time.deltaTime;
			GetComponent<Rigidbody2D>().AddRelativeForce(translation, ForceMode2D.Impulse);
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