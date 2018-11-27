using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public override bool OnUseSkill(BaseCharacter player)
    {
        if (player.EnergyPoint >= UseEnergyPoint && player.HealthPoint > UseHealthPoint)
        {
            player.HealthPoint -= this.UseHealthPoint;
            player.EnergyPoint -= this.UseEnergyPoint;
            Delay = MaxDelay;
            player.GetComponent<Rigidbody2D>().AddForce(Direction * Knockback);
            return true;
        }
        return false;
    }


    public Vector2 Direction;
    public float Knockback;
    // 경직시간
    public float RigidTime;
}
