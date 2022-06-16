using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSouls
{
    [CreateAssetMenu(menuName = "BoxSouls/Item/Weapon",order =-100)]
    public class WeaponItem : Item
    {
        public GameObject prefab;
        public WeaponDamageCollider weaponDamageCollider;

        [Header("Anim Name")]
        public string leftHandAnimName;
        public string rightHandAnimName;
        public string twoHandsAnimName;
    }
}
