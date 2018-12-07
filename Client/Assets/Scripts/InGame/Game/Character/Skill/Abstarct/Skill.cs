using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Skill : NetworkSkill
{
    public abstract bool OnUseSkill(BaseCharacter player);

    public float CastEnergyPoint = 0;
    public float CastHealthPoint = 0;
}