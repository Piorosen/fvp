using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skill을 상속을 받고 해당 스킬이 액티브 스킬이면 넉백 및 데미지, 쿨타임을 지정합니다.
/// </summary>
public abstract class ActiveSkill : Skill
{
    public float Knockback;
    // 경직시간
    public float RigidTime;

    public float PhysicsDamage;
    public float MaxDelay;
    public float Delay;
}
