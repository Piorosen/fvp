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
        HealthPoint += 5 * Time.deltaTime;
        EnergyPoint += 10 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q) == true)
        {
            UseSkillReq((long)JobType.Warrior.ActBasicSkill);
        }
        if (Input.GetKey(KeyCode.E) == true)
        {
            UseSkillReq((long)JobType.Warrior.ActFrontDash);
        }

        //if (Input.GetKeyUp(KeyCode.R) == true)
        //{
        //    HitSkillAck((long)JobType.Warrior.ActBasicSkill);
        //}

    }

    IEnumerator AttackMotion(float time)
    {
        Debug.Log("공격 애니메이션 시작!");

        Anim.SetBool("Attack", true);
        yield return new WaitForSeconds(time);
        Anim.SetBool("Attack", false);
    }




    public override void UseSkillAck(long SkillId)
    {
        if (NetworkManager.ClientNetworkId == NetworkId)
        {
            return;
        }
        var skill = SkillInfo.Insatence[SkillId];
        if (skill != null)
        {
            skill.CastDirection = Renderer.flipX ? Vector2.right : Vector2.left;
            skill.CastPosition = this.transform.position;
            if (SkillManage.OnUseSkill(this, skill))
            {
                StartCoroutine(AttackMotion(0.5f));

                this.HealthPoint -= skill.CastHealthPoint;
                this.EnergyPoint -= skill.CastEnergyPoint;
            }
        }
    }

    public override void UseSkillReq(long SkillId)
    {
        if (NetworkManager.ClientNetworkId != NetworkId)
        {
            return;
        }
        var skill = SkillInfo.Insatence[SkillId];
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
                
                
                var result = Physics2D.RaycastAll(transform.position, skill.CastDirection, 4000);
                Debug.DrawRay(transform.position, skill.CastDirection * 40, Color.red, 5);

                Debug.Log(result.Length);
                foreach (var user in result)
                {
                    if (user.collider != null && user.collider.gameObject.layer == 9 && user.collider.GetComponent<Character>().NetworkId.Value != NetworkManager.ClientNetworkId.Value)
                    {
                        Debug.Log($"{user.collider.GetComponent<Character>().NetworkId.Value} {NetworkManager.ClientNetworkId}");
                        HitSkillReq(user.collider.GetComponent<Character>().NetworkId.Value, skill.SkillId);
                    }
                }

            }
        }
    }
    
    
    /// <summary>
    /// 데미지 입었을 때 처리를 합니다.
    /// </summary>
    /// <param name="SkillId"></param>
    public override void HitSkillAck(long SkillId)
    {
        var skill = SkillInfo.Insatence[SkillId];
        if ((skill is ActiveSkill) == true)
        {
            var cast = (skill as ActiveSkill);
            HealthPoint -= cast.PhysicsDamage;
            RigidBody.velocity = new Vector2();
            RigidBody.AddForce(cast.CastDirection * cast.Knockback);
        }
    }

    /// <summary>
    /// 데미지 입을 사랑 요청
    /// </summary>
    /// <param name="SkillId"></param>
    public override void HitSkillReq(long NetworkId, long SkillId)
    {
        var skill = SkillInfo.Insatence[SkillId];
        skill.NetworkId = NetworkId;
        NetworkManager.Instance?.CastSkillHit(skill);
    }

   
}