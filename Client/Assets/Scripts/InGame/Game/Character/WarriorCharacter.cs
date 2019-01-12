using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

/// <summary>
/// BaseCharacter을 상속을 받고 Warrior캐릭터를 구현합니다. 그리고 스킬 관련 또한 처리를 합니다.
/// </summary>
public class WarriorCharacter : BaseCharacter
{
    SkillManager SkillManage;

    private void Awake()
    {
        SkillManage = new SkillManager();
    }

    private void Update()
    {
        SkillManage.Update();
        HealthPoint += 10 * Time.deltaTime;
        EnergyPoint += 50 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q) == true)
        {
            UseSkill((long)JobType.Warrior.ActBasicSkill);
        }
        if (Input.GetKey(KeyCode.E) == true)
        {
            UseSkill((long)JobType.Warrior.ActFrontDash);
        }

        if (Input.GetKeyUp(KeyCode.R) == true)
        {
            HitSkill((long)JobType.Warrior.ActBasicSkill);
        }

    }

    IEnumerator AttackMotion(float time)
    {
        Debug.Log("공격 애니메이션 시작!");

        Anim.SetBool("Attack", true);
        yield return new WaitForSeconds(time);
        Anim.SetBool("Attack", false);
    }

    public override void UseSkill(long SkillId)
    {
        if (NetworkManager.ClientNetworkId != NetworkId)
        {
            return;
        }
        var skill = SkillManager.GetSkill(SkillId);
        if (skill != null)
        {
            skill.CastDirection = Renderer.flipX ? Vector2.right : Vector2.left;
            skill.CastPosition = this.transform.position;
            if (SkillManage.OnUseSkill(this, skill))
            {
                StartCoroutine(AttackMotion(0.5f));

                this.HealthPoint -= skill.CastHealthPoint;
                this.EnergyPoint -= skill.CastEnergyPoint;
                
                NetworkManager.Instance?.CastSkill(skill);
            }
        }
    }

    public override void HitSkill(long SkillId)
    {
        var skill = SkillManager.GetSkill(SkillId);

        skill.NetworkId = NetworkManager.ClientNetworkId.Value;

        NetworkManager.Instance?.CastSkillHit(skill);

        HealthPoint -= skill.CastHealthPoint;
        EnergyPoint -= skill.CastEnergyPoint;
    }

}