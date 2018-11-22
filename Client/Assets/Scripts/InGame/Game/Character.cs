using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public delegate void ChangeStatus(float now, float max);
public class Character : MonoBehaviour
{ 
    // 이벤트
    public event ChangeStatus ChangeHP;
    public event ChangeStatus ChangeMP;
    
    // 이벤트 발생 시키는 함수
    // 체력이 변화 가 되었을 경우 UI에 변화를 주어야함.
    void OnChangeHP(float now, float max)
    {
        if (ChangeHP != null)
        {
            Debug.Log(now / max);
            HealthObject.value = now / max;
            ChangeHP.Invoke(now, max);
        }
    }
    // 기력이 변화가 되었을 경우에 UI에 변화를 주는 함수.
    void OnChangeMP(float now, float max)
    {
        if (ChangeMP != null)
        {
            ChangeMP.Invoke(now, max);
        }
    }

    public Slider HealthObject;

    // 캐릭터가 이동할 수 있는 최대 속도.
    public float MaxSpeed;
    // 점프하는 힘
    public float Jump;
    // 캐릭터가 이동하는 가속도
    public float Accelerate;
    // 현재 속도
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
            if (value != _HP)
            {
                OnChangeHP(value, MaxHP);
                _HP = value;
            }
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
            if (value != _MP)
            {
                OnChangeMP(value, MaxMP);
                _MP = value;
            }
        }
    }

    // Components
    Animator anime;
    Rigidbody2D rigidBody;
    SpriteRenderer Renderer;
    Text Text;

    public string PlayerName;

    // 고유 정보
    public long? NetworkId = null;
    
    // 현재 스테이터스
    private float _HP = 100.0f;
    private float _MP = 100.0f;

    // 실행이 되면은 각 컴포넌트의 정보를 가져옴.
    void Start()
    {
        anime = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();

        HealthObject = this.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        Debug.Log(HealthObject.value);
        Text = this.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        Text.text = PlayerName;
    }
    

    // 2번째의 RigidBody가 속도에 따라서 Jump 인지 Down인지 체크함.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (rigidBody.velocity.y > 0)
        {
            Debug.Log("공중부양상태");
            HP = HP - 5;
            anime.SetBool("Jump", true);
        }
        else if (rigidBody.velocity.y < 0)
        {
            anime.SetBool("Down", true);
        }
    }

    // RigidBody의 레이어가 무엇이냐에 따라서 상태 전이
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attach" + collision.gameObject.layer);
        if (collision.gameObject.layer == 8)
        {
            anime.SetBool("Jump", false);
            anime.SetBool("Down", false);
        }
    }
    Queue<Vector3> ServerQue = new Queue<Vector3>();
    bool C = false;
    public void ServerMovement(Vector3 data)
    {
        if (!C)
        {
            StartPosition.z = data.z;
            EndPosition.z = data.z;
            C = true;
        }
        rigidBody.gravityScale = 0.0f;
        ServerQue.Enqueue(data);
    }

    // FixedUpdate에서 처리하지 않고
    // 독자적인 Movement에서 처리를 함.
    // PlayerManager의 FixedUpdate에 종속됨.
    Vector3 StartPosition = new Vector3();
    Vector3 EndPosition = new Vector3();

    public void Movement(Vector4 data)
    {
        if (NetworkId != NetworkManager.ClientNetworkId)
        {
            if (ServerQue.Count != 0)
            {
                StartPosition = EndPosition;
                for (; ServerQue.Count != 0;)
                {
                    EndPosition = ServerQue.Dequeue();
                }

                var t = StartPosition;
                t.z = -1;
                transform.position = t;
            }

            Vector3 Distance = EndPosition - StartPosition;

            if (Distance.z != 0)
            {
                transform.Translate(new Vector3(Distance.x * Time.fixedDeltaTime * (1.0f / Distance.z),
                                                Distance.y * Time.fixedDeltaTime * (1.0f / Distance.z),
                                                0));
            }
           
            return;
        }
        
        // InputManager은 인게임내 스마트폰에 있는
        // 가상 스틱의 값을 나타냄
        float x = InputManager.InputVector.x;
        float y = InputManager.InputVector.y;
        // data.z는 키보드의 값을 받을것인지 스마트폰의 입력을 받을것인지 나타냄
        // data.z 값의 편집 방법은 Hierarchy의 PlayerManager의 Is Debug의 값을 수정 요함.
        
        if (x == 0)
        {
            x = data.x;
        }
        if (y == 0)
        {
            y = data.y;
        }

        // 애니메이션 이동 방향의 값 
        anime.SetFloat("DivX", x);
        anime.SetFloat("DivY", y);

        // 캐릭터의 이동 함수
        float Acc = Accelerate * Time.fixedDeltaTime;
        // 이동관련 함수 처리
        #region Move
        if (x < 0)
        {
            // 이동시 MP 10 소모 (초당)
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
            // 이동시 MP 10 소모 (초당)
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
            // x의 값이 0 일 때 멈추는 효과
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

        // 현재 이동하는 속도에 따른 애니메이션 효과 반전
        if (Speed > 0)
        {
            Renderer.flipX = true;
        }
        else if (Speed < 0)
        {
            Renderer.flipX = false;
        }

        // 현재 속도를 체크하여 걷고 있는지 아닌지 설정
        if (Speed != 0.0f)
        {
            anime.SetBool("Walking", true);
        }
        else
        {
            anime.SetBool("Walking", false);
        }

        // 점프 처리 하는 코드
        // 현재 점프중인지 아닌지 체크를 함
        if (!anime.GetBool("Jump"))
        {
            // 키보드의 입력시 절대적으로 1.0f로 고정
            // 가상 스틱을 이용시 50% 가량 위로 이동해야지만 처리
            if (y > 0.5f)
            {
                // MP 30을 소모하므로 30 이상일 경우만
                if (MP >= 30)
                {
                    // MP 30을 낮추고
                    MP -= 30;
                    // 현재 y축의 이동속도를 0로 바꿉니다.
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                    // 점프하는 힘을 줍니다.
                    rigidBody.AddForce(new Vector2(0, Jump));
                    // 점프 애니메이션 설정.
                    anime.SetBool("Jump", true);
                }
            }
        }
        // 초당 MP는 15씩 증가하는 코드.
        MP += Time.fixedDeltaTime * 30;

        // 그외 변화된 좌우 값을 설정합니다.
        rigidBody.transform.Translate(Vector3.right * Speed);
    }
}
