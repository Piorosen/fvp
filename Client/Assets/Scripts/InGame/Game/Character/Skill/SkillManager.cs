using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class SkillManager {
    static List<Skill> SkillInfo;
    List<Skill> SkillQueue;

    public SkillManager()
    {
        if (SkillInfo == null)
        {
            SkillInfo = new List<Skill>();
            LoadSkill();
        }
        SkillQueue = new List<Skill>();
    }

    public void OnUseSkill(BaseCharacter player, Skill Skill)
    {
        var result = SkillQueue.FirstOrDefault((i) => i.SkillId == Skill.SkillId);
        if (result == null)
        {
            if (this[Skill.SkillId].OnUseSkill(player))
                SkillQueue.Add(this[Skill.SkillId]);
        }
    }

    public void Update()
    {
        for (int i = 0; i < SkillQueue.Count; i++)
        {
            SkillQueue[i].Delay -= Time.deltaTime;
            if (SkillQueue[i].Delay <= 0.0f)
            {
                SkillQueue.RemoveAt(i);
            }
        }
    }

    void LoadSkill()
    {
        SkillInfo.Add(new ActiveSkill
        {
            Delay = 0.0f,
            Distance = 5,
            Name = "Basic",
            PhysicsDamage = 40,
            UseEnergyPoint = 50,
            UseHealthPoint = 20,
            Image = null,
            Knockback = 8000,
            RigidTime = 0,
            MaxDelay = 1.5f,
        });
        SkillInfo.Add(new ActiveSkill
        {
            Delay = 0,
            Distance = 5,
            Name = "Basic",
            PhysicsDamage = 40,
            UseEnergyPoint = 60,
            UseHealthPoint = 20,
            Image = null,
            Knockback = 8000,
            RigidTime = 0,
            MaxDelay = 2.0f,
        });
    }

    public Skill this[long SkillId]
    {
        get
        {
            return SkillInfo[Convert.ToInt32(SkillId)];
        }
    }
}
