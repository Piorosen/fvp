using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SkillManager {
    public static SkillManager Instance;

    NetworkManager NetworkManage;

    List<Skill> SkillList;

    public SkillManager()
    {
        if (Instance == null)
        {
            SkillList = new List<Skill>();
            NetworkManage = NetworkManager.Instance ?? new NetworkManager();
            LoadSkill();
            Instance = this;
        }
    }

    void LoadSkill()
    {
        SkillList.Add(new Skill
        {
            Delay = 0.01f,
            Distance = 3,
            IsActive = true,
            MasicDamage = 0,
            Name = "Basic",
            PhysicsDamage = 40,
            SpecialDamage = 0,
            UseEnergyPoint = 0,
            UseHealthPoint = 0,
            Image = null,
            Knockback = 40000,
            RigidTime = 0,

        });
        SkillList.Add(new Skill
        {
            Delay = 0.01f,
            Distance = 3,
            IsActive = true,
            MasicDamage = 0,
            Name = "Basic",
            PhysicsDamage = 40,
            SpecialDamage = 0,
            UseEnergyPoint = 0,
            UseHealthPoint = 0,
            Image = null,
            Knockback = 40000,
            RigidTime = 0
        });
    }

    public Skill GetSkill(long SkillId)
    {
        return SkillList[Convert.ToInt32(SkillId)];
    }
}
