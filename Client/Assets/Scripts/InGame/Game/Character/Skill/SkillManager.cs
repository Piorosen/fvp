using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SkillManager {
    public static List<Skill> SkillInfo;
    List<Skill> SkillQueue;
    
    public static bool IsActiveSkill(long Skill)
    {
        return SkillInfo[Convert.ToInt32(Skill)] is ActiveSkill;
    }

    public SkillManager()
    {
        if (SkillInfo == null)
        {
            SkillInfo = new List<Skill>();
            LoadSkill();
        }
        SkillQueue = new List<Skill>();
    }

    public bool OnUseSkill(BaseCharacter player, Skill Skill)
    {
        var result = SkillQueue.FirstOrDefault((i) => i.SkillId == Skill.SkillId);
        if (result == null)
        {
            if (this[Skill.SkillId].OnUseSkill(player))
                SkillQueue.Add(this[Skill.SkillId]);
            return true;
        }
        return false;
    }

    public void Update()
    {
        for (int i = 0; i < SkillQueue.Count; i++)
        {
            if (SkillQueue[i] is ActiveSkill)
            {
                var sk = SkillQueue[i] as ActiveSkill;
                sk.Delay -= Time.deltaTime;
                if (sk.Delay <= 0.0f)
                {
                    SkillQueue.RemoveAt(i);
                }
            }
        }
    }

    void LoadSkill()
    {
        SkillInfo.Add(new ActiveSkill
        {
            SkillId = 0,
            Distance = 5,
            PhysicsDamage = 40,
            CastEnergyPoint = 40,
            HitHealthPoint = 30,
            Knockback = 8000,
            MaxDelay = 1.5f,
        });
        SkillInfo.Add(new ActiveSkill
        {
            SkillId = 1,
            Distance = 5,
            PhysicsDamage = 40,
            CastEnergyPoint = 100,
            HitHealthPoint = 100,
            Knockback = 8000,
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
