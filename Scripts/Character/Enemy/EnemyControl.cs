using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoxSouls
{
    public class EnemyControl : CharacterControl
    {
        [Header("Refs")]
        public Animator anim;
        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }


    }
}
