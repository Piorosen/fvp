using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NetworkSkill을 상속을 받고 스킬 코스트 및 스킬 사용 하는 함수를 만듭니다.
/// </summary>
public abstract class Skill : NetworkSkill
{
    public abstract bool OnUseSkill(BaseCharacter player);

    // 블럭 1칸당 크기
    public float Distance = 24;
    public float CastEnergyPoint = 0;
    public float CastHealthPoint = 0;
}