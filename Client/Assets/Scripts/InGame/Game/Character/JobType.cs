using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JobType
{
    public enum Warrior
    {
        ActBasicSkill = 0,
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