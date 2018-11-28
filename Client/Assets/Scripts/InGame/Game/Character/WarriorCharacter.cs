using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class WarriorCharacter : BaseCharacter
{

    SkillManager SkillManage;

    private new void Awake()
    {
        base.Awake();
        SkillManage = new SkillManager();
    }

    private void Update()
    {
        SkillManage.Update();
        HealthPoint += 10 * Time.deltaTime;
        EnergyPoint += 50 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q) == true)
        {
            SkillManage.OnUseSkill(this, 0);
        }
        if (Input.GetKey(KeyCode.E) == true)
        {
            SkillManage.OnUseSkill(this, 1);
        }
    }

    public override void UseSkill(long SkillId)
    {
        var skill = SkillManage[SkillId];
        skill.OnUseSkill(this);
        skill.Position = this.transform.position;
        skill.Direction = Renderer.flipX ? Vector2.left : Vector2.right;
        OnSkillUse(skill);
    }

    public override void HitSkill(long SkillId)
    {
        var skill = SkillManage[SkillId];

        HealthPoint -= skill.UseHealthPoint;
        EnergyPoint -= skill.UseEnergyPoint;
    }

}