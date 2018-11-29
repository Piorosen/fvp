using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Skill : NetworkSkill
{
    public float CastEnergyPoint = 0;
    public float CastHealthPoint = 0;

    public float HitEnergyPoint = 0;
    public float HitHealthPoint = 0;
}