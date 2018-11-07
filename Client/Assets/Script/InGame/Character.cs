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
    
    public int? UID = null;

    void Awake()
    {
        UID = this.GetHashCode();
    }
    
    

    void Start () {
        anime = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (rigidBody.velocity.y > 0)
        {
            Debug.Log("공중부양상태");
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
        if (collision.gameObject.layer == 8)
        {
            anime.SetBool("Jump", false);
            anime.SetBool("Down", false);
        }
    }

    public void Movement(Vector4 data){

        float x = InputManager.DivX;
        float y = InputManager.DivY;
        if (data.z != 0){
            x = data.x;
            y = data.y;
        }

        anime.SetFloat("DivX", x);
       anime.SetFloat("DivY", y);

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
        }
        else
        {
            if (Speed > 0.01)
            {
                Speed -= Acc * 3;
            }
            else if (Speed < -0.01)
            {
                Speed += Acc * 3;
            }
            else
            {
                Speed = 0.0f;
            }
        }

        if (Speed > 0)
        {
            Renderer.flipX = true;
        }
        else if (Speed < 0)
        {
            Renderer.flipX = false;
        }

        if (Speed != 0.0f)
        {
            anime.SetBool("Walking", true);
        }
        else
        {
            anime.SetBool("Walking", false);
        }

        if (!anime.GetBool("Jump"))
        {
            if (y > 0)
            {
                rigidBody.AddForce(new Vector2(0, Jump));
            }
        }

        Vector2 Start = transform.position;
        Vector2 End = new Vector2(Start.x + Speed, Start.y);


        RaycastHit2D hit = Physics2D.Linecast(Start, End);
        if (hit.transform == null)
        {
            Debug.Log("A");
            return;

        }
        rigidBody.transform.Translate(Vector3.right * Speed);


    }

    // Update is called once per frame
    void FixedUpdate () {

    }
}
