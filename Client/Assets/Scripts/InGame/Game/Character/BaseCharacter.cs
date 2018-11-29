using System;
using UnityEngine;
using System.Collections.Concurrent;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public delegate void ChangeStatus(float now, float max);

public class BaseCharacter : MonoBehaviour
{
    // 이벤트
    public event ChangeStatus ChangeHP;
    public event ChangeStatus ChangeMP;

    // 이벤트 발생 시키는 함수
    // 체력이 변화 가 되었을 경우 UI에 변화를 주어야함.
    void OnChangeHP(float now, float max)
    {
        ChangeHP?.Invoke(now, max);
    }
    // 기력이 변화가 되었을 경우에 UI에 변화를 주는 함수.
    void OnChangeMP(float now, float max)
    {
        ChangeMP?.Invoke(now, max);
    }

    // Components
    protected Slider HealthObject;
    protected Text Text;
    protected SpriteRenderer Renderer;
    protected Animator Anim;
    protected Rigidbody2D RigidBody;

    protected void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        HealthObject = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        Text = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        HealthPoint = MaxHealth;
        EnergyPoint = MaxEnergy;
    }

    public float MaxHealth = 100.0f;
    public float HealthPoint
    {
        get
        {
            return _HP;
        }
        set
        {
            if (value > MaxHealth)
            {
                value = MaxHealth;
            }
            else if (value < 0)
            {
                value = 0;
            }

            if (value != _HP)
            {
                OnChangeHP(value, MaxHealth);
                _HP = value;
            }
        }
    }
    public float MaxEnergy = 100.0f;
    public float EnergyPoint
    {
        get
        {
            return _MP;
        }
        set
        {
            if (value > MaxEnergy)
            {
                value = MaxEnergy;
            }
            else if (value < 0)
            {
                value = 0;
            }
            if (value != _MP)
            {
                OnChangeMP(value, MaxEnergy);
                _MP = value;
            }
        }
    }

    // 플레이어 이름
    public string PlayerName
    {
        get
        {
            return Text.text;
        }
        set
        {
            Text.text = value;
        }
    }

    // 캐릭터가 이동할 수 있는 최대 속도.
    public float MaxSpeed;
    // 점프하는 힘
    public float JumpPower;
    // 캐릭터가 이동하는 가속도
    public float Accelerate;
    // 현재 속도
    public float Speed;
    // 고유 정보
    public long? NetworkId = null;

    // 현재 스테이터스
    protected float _HP = 100.0f;
    protected float _MP = 100.0f;

    protected float Damage = 9.4f;

    protected bool IsHiding = false;
    protected bool IsGround = false;

    public int MaxJumpCount = 3;
    private int CanJumpCount;

    private ConcurrentQueue<Vector3> ServerQue = new ConcurrentQueue<Vector3>();
    Vector3 StartPosition;
    Vector3 EndPosition;

    protected float JumpDelay = 0.5f;
    protected virtual void Jump()
    {
        if (InputManager.InputVector.y > 0.5f && CanJumpCount > 0 && JumpDelay < 0)
        {
            RigidBody.AddForce(new Vector2(0, JumpPower));
            RigidBody.velocity = new Vector2(RigidBody.velocity.x, 0);
            JumpDelay = 0.3f;
            CanJumpCount--;
        }
        JumpDelay -= Time.fixedDeltaTime;
    }

    public void ServerData(Vector3 data)
    {
        ServerQue.Enqueue(data);
    }
    protected Vector2 ServerMovement()
    {
        if (ServerQue.IsEmpty == false)
        {
            StartPosition = EndPosition;
            while (ServerQue.TryDequeue(out EndPosition))
            {
            }

            var t = StartPosition;
            t.z = transform.position.z;
            transform.position = t;
        }

        Vector3 Distance = EndPosition - StartPosition;

        if (Distance.z != 0)
        {
            return new Vector2(Distance.x * Time.fixedDeltaTime * (1.0f / Distance.z),
                                            Distance.y * Time.fixedDeltaTime * (1.0f / Distance.z)
                                          );
        }
        return new Vector2();
    }
    protected virtual float ClientMove()
    {
        float Acc = Accelerate * Time.fixedDeltaTime;
        // 이동관련 함수 처리
        RigidBody.AddForce(new Vector2(InputManager.InputVector.x * Acc, 0));

        return RigidBody.velocity.x;
    }

    protected void SetAnim(Vector2 animData)
    {
        if (animData.x > 0)
        {
            Renderer.flipX = true;
            Anim.SetBool("Walking", true);
        }
        else if (animData.x < 0)
        {
            Renderer.flipX = false;
            Anim.SetBool("Walking", true);
        }
        else
        {
            Anim.SetBool("Walking", false);
        }

        if (animData.y > 0)
        {
            Anim.SetBool("Jump", true);
        }
        else if (animData.y < 0)
        {
            Anim.SetBool("Down", true);
        }
        else
        {
            Anim.SetBool("Jump", false);
            Anim.SetBool("Down", false);
        }
    }

    // FixedUpdate에서 처리하지 않고
    // 독자적인 Movement에서 처리를 함.
    // PlayerManager의 FixedUpdate에 종속됨.
    public void Movement()
    {
        Vector2 NextMovePosition = new Vector2();

        if (NetworkId == NetworkManager.ClientNetworkId)
        {
            Jump();
            ClientMove();
            NextMovePosition = RigidBody.velocity;
        }
        else
        {
            NextMovePosition = ServerMovement();
        }
        SetAnim(NextMovePosition);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        IsGround = false;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        IsGround = true;
        CanJumpCount = MaxJumpCount;
    }

    public virtual void UseSkill(long SkillId)
    {
        throw new NotImplementedException();
    }
    public virtual void HitSkill(long SkillId)
    {
        throw new NotImplementedException();
    }
}