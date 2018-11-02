using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class Character : MonoBehaviour {

    public float MaxSpeed;
    public float Jump;
    public float Accelerate = 3.0f;
    public float Speed = 0;

    Animator anime;
    Rigidbody2D rigidBody;
    SpriteRenderer Renderer;
    BoxCollider2D playerCollider;

    int? UID = null;

    void Awake()
    {
        UID = this.GetHashCode();
    }

    void Start () {
        anime = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (rigidBody.velocity.y > 0)
        {
            anime.SetBool("Jump", true);
        }
        else if (rigidBody.velocity.y < 0)
        {
            anime.SetBool("Down", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attach" + collision.gameObject.layer);
        anime.SetBool("Jump", false);
        anime.SetBool("Down", false);
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.rotation = Quaternion.identity;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 Movement = new Vector3(x, y, this.transform.position.z);

        float Acc = Accelerate * Time.deltaTime;

        if (x < 0)
        {
            if (Speed < -MaxSpeed)
            {
                Speed = -MaxSpeed;
            }
            else
            {
                Speed -= Acc;
            }
        }
        else if (x > 0)
        {
            if (Speed > MaxSpeed)
            {
                Speed = MaxSpeed;
            }
            else
            {
                Speed += Acc;
            }
        }else{
            if (Speed > 0.01)
            {
                Speed -= Acc * 2;
            }
            else if (Speed < -0.01)
            {
                Speed += Acc * 2;
            }
            else{
                Speed = 0.0f;
            }
        }

        if (Speed > 0){
            Renderer.flipX = true;
        }else if (Speed < 0){
            Renderer.flipX = false;
        }

        if (!anime.GetBool("Jump"))
        {
            if (y > 0)
            {
                rigidBody.AddForce(new Vector2(0, Jump));
            }
        }

        rigidBody.transform.Translate(Vector3.right * Speed);
	}
}
