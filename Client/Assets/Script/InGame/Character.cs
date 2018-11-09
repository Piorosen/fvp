using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;


public delegate void ChangeStatus(float now, float max);
public class Character : MonoBehaviour
{
    public event ChangeStatus ChangeHP;
    public event ChangeStatus ChangeMP;

    void OnChangeHP(float old, float now)
    {
        if (ChangeHP != null)
            ChangeHP.Invoke(old, now);
    }
    void OnChangeMP(float old, float now)
    {
        if (ChangeMP != null)
            ChangeMP.Invoke(old, now);
    }

    public float MaxSpeed;
    public float Jump;
    public float Accelerate = 3.0f;
    public float Speed;

    public float MaxHP = 100.0f;
    public float HP
    {
        get
        {
            return _HP;
        }
        set
        {
            if (value > MaxHP)
            {
                value = MaxHP;
            }
            else if (value < 0)
            {
                value = 0;
            }
            OnChangeHP(value, MaxHP);
            _HP = value;
        }
    }

    public float MaxMP = 100.0f;
    public float MP
    {
        get
        {
            return _MP;
        }
        set
        {
            if (value > MaxMP)
            {
                value = MaxMP;
            }
            else if (value < 0)
            {
                value = 0;
            }
            OnChangeMP(value, MaxMP);
            _MP = value;
        }
    }


    Animator anime;
    Rigidbody2D rigidBody;
    SpriteRenderer Renderer;

    public int? UID = null;
    private float _HP = 100.0f;
    private float _MP = 100.0f;

    void Awake()
    {
        UID = this.GetHashCode();
    }




    void Start()
    {
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

    public void Movement(Vector4 data)
    {

        float x = InputManager.InputVector.x;
        float y = InputManager.InputVector.y;
        if (data.z != 0)
        {
            x = data.x;
            y = data.y;
        }

        anime.SetFloat("DivX", x);
        anime.SetFloat("DivY", y);

        float Acc = Accelerate * Time.deltaTime;
        #region Move
        if (x < 0)
        {
            MP -= 10 * Time.deltaTime;
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
            MP -= 10 * Time.deltaTime;
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
            if (Speed > 0.05)
            {
                Speed -= Acc * 1.3f;
            }
            else if (Speed < -0.05)
            {
                Speed += Acc * 1.3f;
            }
            else
            {
                Speed = 0.0f;
            }
        }
        #endregion

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

        if (rigidBody.velocity.y == 0){
            anime.SetBool("Jump", false);
        }

        if (!anime.GetBool("Jump"))
        {
            if (y > 0.5f)
            {
                if (MP >= 30)
                {
                    MP -= 30;
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                    rigidBody.AddForce(new Vector2(0, Jump));
                    anime.SetBool("Jump", true);
                }
            }
        }
        MP += Time.deltaTime * 15;

        rigidBody.transform.Translate(Vector3.right * Speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
