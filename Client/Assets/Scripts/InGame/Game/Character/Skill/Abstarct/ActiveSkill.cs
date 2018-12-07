using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : Skill
{
    public float Knockback;
    // 경직시간
    public float RigidTime;

    public float PhysicsDamage;
    public float MaxDelay;
    public float Delay;
    public float Distance;
}
