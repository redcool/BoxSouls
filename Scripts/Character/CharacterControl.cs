using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoxSouls
{
    public class CharacterControl : MonoBehaviour
    {
        public CharacterStats characterStats = new CharacterStats();

        public virtual void OnDamage(CharacterStats attacker)
        {
            characterStats.hp -= AttackDefenceTools.GetDamage(attacker, characterStats);

            if(characterStats.hp <= 0)
            {
                characterStats.hp = 0;
                OnDie();
            }
        }

        public virtual void OnDie()
        {

        }

        public virtual void OnOpenDamageTrigger()
        {

        }

        public virtual void OnCloseDamageTrigger()
        {

        }
    }
}
