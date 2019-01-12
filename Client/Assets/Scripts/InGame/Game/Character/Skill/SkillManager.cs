using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 캐릭터 1명 당 1개씩 달리는 스킬 매니저 입니.
/// 캐릭터가 스킬을 사용하게 될시 스킬의 쿨타임 및 상대방에게 데미지를 받는 것 역시 처리합니다.
/// </summary>
public class SkillManager {
    
    List<Skill> SkillQueue;
    
    public static bool IsActiveSkill(long Skill)
    {
        return GetSkill(Skill) is ActiveSkill;
    }

    public SkillManager()
    {
        SkillQueue = new List<Skill>();
    }

    public bool OnUseSkill(BaseCharacter player, Skill Skill)
    {
        var result = SkillQueue.FirstOrDefault((i) => i.SkillId == Skill.SkillId);
        if (result == null)
        {
            var skill = GetSkill(Skill.SkillId);
            if (skill.OnUseSkill(player))
                SkillQueue.Add(skill);
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

    

    public static Skill GetSkill(long SkillId)
    {
        try
        {
            return SkillInfo.Insatence[SkillId];
        }
        catch (Exception)
        {
            Debug.Log($"ID : {SkillId}");
            return null;
        }
    }
}
