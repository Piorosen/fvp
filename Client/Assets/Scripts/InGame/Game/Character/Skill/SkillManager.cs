﻿using System.Collections;
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

    public void OnUseSkill(BaseCharacter player, long SkillId)
    {
        var result = SkillQueue.FirstOrDefault((i) => i.SkillId == SkillId);
        if (result == null)
        {
            if (this[SkillId].OnUseSkill(player))
                SkillQueue.Add(this[SkillId]);
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
            Distance = 3,
            MasicDamage = 0,
            Name = "Basic",
            PhysicsDamage = 40,
            SpecialDamage = 0,
            UseEnergyPoint = 50,
            UseHealthPoint = 20,
            Image = null,
            Knockback = 8000,
            RigidTime = 0,
            MaxDelay = 0.5f,
            Direction = Vector3.right
        });
        SkillInfo.Add(new ActiveSkill
        {
            Delay = 0,
            Distance = 3,
            MasicDamage = 0,
            Name = "Basic",
            PhysicsDamage = 40,
            SpecialDamage = 0,
            UseEnergyPoint = 60,
            UseHealthPoint = 20,
            Image = null,
            Knockback = 8000,
            RigidTime = 0,
            MaxDelay = 0.5f,
            Direction = Vector3.left
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