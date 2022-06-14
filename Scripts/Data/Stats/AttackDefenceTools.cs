using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Mathf;

namespace BoxSouls
{
    public static class AttackDefenceTools
    {

        public static int GetDamage(CharacterStats a, CharacterStats b)
        {
            return Min(0, a.attack - b.defence) + a.impale;
        }
    }
}
