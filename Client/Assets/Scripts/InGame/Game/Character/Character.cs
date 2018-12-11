using UnityEngine;

/// <summary>
/// 캐릭터의 체력, 기력, 네트워크 Id등 기본적인 이동 및 UI 등 기초 데이터 틀을 제공합니다.
/// </summary>
public abstract class Character : MonoBehaviour
{
    // 이벤트
    public event ChangeStatus ChangeHP;
    public event ChangeStatus ChangeMP;

    public abstract void Initialize(long NetworkId, string PlayerName);

    // 이벤트 발생 시키는 함수
    // 체력이 변화 가 되었을 경우 UI에 변화를 주어야함.
    protected void OnChangeHP(float now, float max) => ChangeHP?.Invoke(now, max);
    // 기력이 변화가 되었을 경우에 UI에 변화를 주는 함수.
    protected void OnChangeMP(float now, float max) => ChangeMP?.Invoke(now, max);

    #region Property
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

    // 현재 스테이터스
    protected float _HP = 100.0f;
    protected float _MP = 100.0f;
    #endregion

    // 고유 정보
    public long? NetworkId = null;
    // 캐릭터가 이동할 수 있는 최대 속도.
    public float MaxSpeed;
    // 점프하는 힘
    public float JumpPower;
    // 캐릭터가 이동하는 가속도
    public float Accelerate;
    // 현재 속도
    public float Speed;

    protected abstract void SetAnim(Vector2 animData);
    public abstract void Movement();
}