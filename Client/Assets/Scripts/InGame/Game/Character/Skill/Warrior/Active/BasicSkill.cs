using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ActiveSkill을 상속을 받고 OnUseSkill을 구현합니.
/// 모든 스킬 1개 마다 클래스를 구현하여 조금 더 다양한 스킬을 구현하는 방법입니다.
/// </summary>
public class BasicSkill : ActiveSkill
{
    public override bool OnUseSkill(BaseCharacter player)
    {
        if (player.EnergyPoint >= CastEnergyPoint && player.HealthPoint > CastHealthPoint)
        {
            player.HealthPoint -= this.CastHealthPoint;
            player.EnergyPoint -= this.CastEnergyPoint;
            Delay = MaxDelay;

            return true;
        }
        return false;
    }

    public BasicSkill()
    {
        this.SkillId = (long)JobType.Warrior.ActBasicSkill;
        this.MaxDelay = 1.5f;
        this.Distance *= 1.0f;
        this.PhysicsDamage = 65.0f;
        this.CastEnergyPoint = 10.0f;
        this.CastHealthPoint = 0.0f;
        this.Knockback = 3000;
    }
}
