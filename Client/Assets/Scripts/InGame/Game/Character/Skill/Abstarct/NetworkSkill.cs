using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NetworkSkill {
    static long SkillCount = 0;
    public NetworkSkill()
    {
        SkillId = SkillCount;
        SkillCount++;
    }

    public abstract bool OnUseSkill(BaseCharacter player);
    
    public Vector2 CastDirection;
    public Vector2 CastPosition;
    public long NetworkId;

    public readonly long SkillId;


}