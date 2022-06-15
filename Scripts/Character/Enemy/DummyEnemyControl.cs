using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoxSouls
{
    public class DummyEnemyControl : CharacterControl
    {
        [Header("Refs")]
        public Animator anim;
        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }

        public override void OnDamage(CharacterStats attacker)
        {
            base.OnDamage(attacker);
            anim.CrossFade(Consts.AnimatorStateNames.DAMAGE, 0.2f);
        }
    }
}
