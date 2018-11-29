using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public override bool OnUseSkill(BaseCharacter player)
    {
        if (player.EnergyPoint >= CastEnergyPoint && player.HealthPoint > CastHealthPoint)
        {
            player.HealthPoint -= this.CastHealthPoint;
            player.EnergyPoint -= this.CastEnergyPoint;
            Delay = MaxDelay;
            NetworkManager.Instance.CastSkill(player.transform.position, this);
            return true;
        }
        return false;
    }
    public float Knockback;
    // 경직시간
    public float RigidTime;

    public float PhysicsDamage;
    public float MaxDelay;
    public float Delay;
    public float Distance;
        
}
