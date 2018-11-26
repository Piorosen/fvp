using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill {
    static long SkillCount = 0;
    public Skill()
    {
        SkillId = SkillCount;
        SkillCount++;
    }

    public string Name;
    public Image Image;

    public readonly long SkillId;
    public bool IsActive;

    public float PhysicsDamage;
    public float Delay;

    public float SpecialDamage;
    public float MasicDamage;
    public float Distance;

    public float UseEnergyPoint;
    public float UseHealthPoint;

    public Vector2 Direction;
    public float Knockback;
    public float RigidTime;
}