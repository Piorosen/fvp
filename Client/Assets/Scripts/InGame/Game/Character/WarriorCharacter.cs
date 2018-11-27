using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public delegate void SkillEvent(Skill skill, long? NetworkId);
public class WarriorCharacter : BaseCharacter
{
    public event SkillEvent SkillUse;
    public event SkillEvent SkillHit;

    protected void OnSkillUse(Skill Skill)
    {
        SkillUse?.Invoke(Skill, NetworkId);
    }

    protected void OnSkillHit(Skill Skill)
    {
        SkillHit?.Invoke(Skill, NetworkId);
    }

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

    public override void HitSkill(long SkillId)
    {
        var skill = SkillManage[SkillId];

        HealthPoint -= skill.UseHealthPoint;
        EnergyPoint -= skill.UseEnergyPoint;


        OnSkillHit(skill);
    }

}