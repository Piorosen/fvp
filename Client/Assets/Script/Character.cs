using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float Speed;
    public float Jump;

    Animator anime;
    Rigidbody2D rigidBody;

    SpriteRenderer Renderer;

    BoxCollider2D playerCollider;

	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attach" + collision.gameObject.layer);
        anime.SetBool("Jump", false);
        anime.SetBool("Down", false);
        playerCollider.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.rotation = Quaternion.identity;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 Movement = new Vector3(x, y, this.transform.position.z);

        anime.SetFloat("DivX", x);
        anime.SetFloat("DivY", y);

        if (x != 0)
        {
            if (x > 0)
            {
                Renderer.flipX = true;
            }
            else if (x < 0)
            {
                Renderer.flipX = false;
            }
            anime.SetBool("Walking", true);
        }
        else
        {
            anime.SetBool("Walking", false);
        }


        if (y != 0)
        {
            if (y > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                rigidBody.AddForce(new Vector2(0, Jump));
                anime.SetBool("Jump", true);
            }else  if (y < 0){
                playerCollider.enabled = false;
                anime.SetBool("Down", true);

            }

            anime.SetBool("IsFly", true);
        }
        else if (rigidBody.velocity.y >= 0)
        {
            anime.SetBool("IsFly", false);
        }

        transform.Translate(Movement.x * Speed * Time.deltaTime, 0, 0);
	}
}
