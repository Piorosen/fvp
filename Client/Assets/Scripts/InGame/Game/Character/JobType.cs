using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 직업의 스킬과 직업의 종류를 여기에 정리가 되어있습니다.
/// </summary>
public static class JobType
{
    public enum Warrior : long
    {
        ActBasicSkill = 0L,
        ActLesserAttack,
        ActFrontDash,
        ActDaggerDown,
        ActEnergyEffect,
        ActPerfectDefence,

        PsvAllKnockback,
        PsvLesserDagger,
        PsvFiveStack

    }
}