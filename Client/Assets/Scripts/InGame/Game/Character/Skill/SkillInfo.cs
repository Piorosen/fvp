using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : SingleTon<SkillInfo>
{
    public SkillInfo()
    {
        if (list == null)
        {
            list = new Dictionary<long, Skill>();
            LoadSkill();
        }
    }
    public Dictionary<long, Skill> list;
    void LoadSkill()
    {
        list.Add(key: (long)JobType.Warrior.ActBasicSkill,
                      value: new BasicSkill());
    }

    public Skill this[long index]
    {
        get
        {
            try
            {
                return list[index];
            }
            catch (Exception)
            {
                Debug.Log($"미구현 : ID : {index}");
                return null;
            }
        }
    }
}
