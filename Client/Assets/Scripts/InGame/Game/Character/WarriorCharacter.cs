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

                if (NetworkManager.Instance != null)
                    NetworkManager.Instance.CastSkill(skill);
            }
        }
    }

    public override void HitSkill(long SkillId)
    {
        var skill = SkillManager.GetSkill(SkillId);

        HealthPoint -= skill.CastHealthPoint;
        EnergyPoint -= skill.CastEnergyPoint;
    }

}