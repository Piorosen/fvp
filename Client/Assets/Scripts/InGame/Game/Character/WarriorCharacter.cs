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
    Dictionary<long, float> CoolTime = new Dictionary<long, float>();

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
        SkillManage = SkillManager.Instance ?? new SkillManager();
    }

    private void Update()
    {
        EnergyPoint += 40 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q) == true)
        {
            UseSkill(0);
        }
        if (Input.GetKey(KeyCode.E) == true)
        {
            UseSkill(1);
        }

        var list = CoolTime.Keys.ToList();
        for (int i = 0; i < CoolTime.Count; i++)
        {
            CoolTime[list[i]] -= Time.deltaTime;
            if (CoolTime[list[i]] < 0.0f)
            {
                CoolTime.Remove(list[i]);
            }
        }
    }

    public override void UseSkill(long SkillId)
    {
        var skill = SkillManage.GetSkill(SkillId);

        if (CoolTime.ContainsKey(SkillId) == false)
        {
            if (EnergyPoint >= skill.UseEnergyPoint && HealthPoint > skill.UseHealthPoint)
            {
                EnergyPoint -= skill.UseEnergyPoint;
                HealthPoint -= skill.UseHealthPoint;

                CoolTime.Add(SkillId, skill.Delay);
                if (skill.SkillId == 0)
                    RigidBody.AddForce(Vector2.left * skill.Knockback);
                else
                    RigidBody.AddForce(Vector2.right * skill.Knockback);
                OnSkillUse(skill);
            }
        }
    }

    public override void HitSkill(long SkillId)
    {
        var skill = SkillManage.GetSkill(SkillId);

        HealthPoint -= skill.UseHealthPoint;
        EnergyPoint -= skill.UseEnergyPoint;


        OnSkillHit(skill);
    }

}