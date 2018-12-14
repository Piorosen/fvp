using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 최상위 스킬 데이터 입니다.
public abstract class NetworkSkill {
    public Vector2 CastDirection;
    public Vector2 CastPosition;
    public long NetworkId;

    public long SkillId;
}