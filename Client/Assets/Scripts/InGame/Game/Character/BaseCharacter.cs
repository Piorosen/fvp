using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ChangeStatus(float now, float max);

/// <summary>
/// 유니티의 컴포넌트와 통신을 위한 공간 입니다. 그 외 애니메이션, 서버처리, 이동처리가 여기서 진행이 됩니다.
/// </summary>
public class BaseCharacter : Character
{
    // Components
    protected Slider HealthObject;
    protected Text Text;
    protected SpriteRenderer Renderer;
    protected Animator Anim;
    protected Rigidbody2D RigidBody;
    public SoundManager Sound;

    public override void Initialize(long NetworkId, string PlayerName)
    {
        Anim = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        HealthObject = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        Text = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        RigidBody = GetComponent<Rigidbody2D>();
        Sound = GetComponent<SoundManager>();

        this.NetworkId = NetworkId;
        this.PlayerName = PlayerName;

        if (NetworkId != NetworkManager.ClientNetworkId)
        {
            RigidBody.gravityScale = 0.0f;
        }
        HealthPoint = MaxHealth;
        EnergyPoint = MaxEnergy;
        ChangeHP += ChangeHPEvent;
    }

    protected void ChangeHPEvent(float now, float max)
    {
        Debug.Log($"{now}");
        HealthObject.value = now / max;
    }

    public void OnUpdateUiInfo()
    {
        OnChangeHP(HealthPoint, MaxHealth);
        OnChangeMP(EnergyPoint, MaxEnergy);
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

    protected float Damage = 9.4f;

    protected bool IsHiding = false;
    protected bool IsGround = false;

    protected override void SetAnim(Vector2 animData)
    {
        if (animData.x > 0)
        {
            Renderer.flipX = true;
            Anim.SetBool("Walking", true);
            Sound.PlaySound(SoundName.Warrior.Walk);
        }
        else if (animData.x < 0)
        {
            Renderer.flipX = false;
            Anim.SetBool("Walking", true);
            Sound.PlaySound(SoundName.Warrior.Walk);
        }
        else
        {
            Anim.SetBool("Walking", false);
        }

        if (animData.y > 0)
        {
            Anim.SetBool("Jump", true);
        }
        else
        {
            if (animData.y < 0)
            {
                Anim.SetBool("Down", true);
            }
            else
            {
                Anim.SetBool("Jump", false);
                Anim.SetBool("Down", false);
            }
        }
        // Debug.Log(animData);

        if (animData.x == 0 || animData.y != 0)
        {
           
            Sound.StopSound(SoundName.Warrior.Walk);
        }
    }
    
    #region Server 캐릭터 관련 이동 처리
    private Queue<Vector3> ServerQue = new Queue<Vector3>();
    Vector3 StartPosition;
    Vector3 EndPosition;

    float time;
    bool Check = false;
    public void ServerData(Vector3 data)
    {
        if (Check == false)
        {
            StartPosition = data;
            EndPosition = data;
            RigidBody.gravityScale = 0.0f;
            Check = true;
        }
        ServerQue.Enqueue(data);
    }
    protected Vector2 ServerMovement()
    {
        //Debug.Log($"{NetworkId}  {NetworkManager.ClientNetworkId}");
        time += Time.fixedDeltaTime;
        //Debug.Log($"{StartPosition.z} + {time} >= {EndPosition.z}");
        if (StartPosition.z + time >= EndPosition.z)
        {
            time = Time.fixedDeltaTime;
            StartPosition = EndPosition;
            while (ServerQue.Count != 0)
            {
                EndPosition = ServerQue.Dequeue();
                //Debug.Log(EndPosition);
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
    #endregion
    #region Client 캐릭터 이동 관련
    public int MaxJumpCount = 3;
    private int CanJumpCount;
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

    protected float ClientMove()
    {
        float Acc = Accelerate * Time.fixedDeltaTime;
        // 이동관련 함수 처리
        RigidBody.AddForce(new Vector2(InputManager.InputVector.x * Acc, 0));

        return RigidBody.velocity.x;
    }
    #endregion

    // FixedUpdate에서 처리하지 않고
    // 독자적인 Movement에서 처리를 함.
    // PlayerManager의 FixedUpdate에 종속됨.
    public override void Movement()
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
            transform.Translate(NextMovePosition);
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

    public virtual void UseSkillAck(long SkillId)
    {
        throw new NotImplementedException();
    }
    public virtual void UseSkillReq(long SkillId)
    {
        throw new NotImplementedException();
    }

    public virtual void HitSkillAck(long SkillId)
    {
        throw new NotImplementedException();
    }

    public virtual void HitSkillReq(long NetworkId, long SkillId)
    {
        throw new NotImplementedException();

    }

}