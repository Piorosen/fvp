using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill {
    static long SkillCount = 0;
    public Skill()
    {
        SkillId = SkillCount;
        SkillCount++;
    }

    public abstract bool OnUseSkill(BaseCharacter player);

    public string Name;
    public Image Image;

    public Vector2 Direction;

    public readonly long SkillId;
    public Vector2 Position;
    public float PhysicsDamage;

    public float MaxDelay;
    public float Delay;

    public float Distance;

    public float UseEnergyPoint;
    public float UseHealthPoint;

}