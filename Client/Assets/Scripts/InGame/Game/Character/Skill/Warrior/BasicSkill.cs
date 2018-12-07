using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        this.Distance = 2.0f;
        this.PhysicsDamage = 65.0f;
        this.CastEnergyPoint = 0.0f;
        this.CastHealthPoint = 0.0f;
    }
}
