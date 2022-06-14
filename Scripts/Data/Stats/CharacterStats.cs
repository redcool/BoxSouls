using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BoxSouls
{
    [Serializable]
    public class CharacterStats
    {
        [Header("Health")]
        public int maxHp;
        public int hp;

        [Header("Attack Defence")]
        public int attack = 3;
        public int defence = 3;
        public int impale = 1;


    }
}
