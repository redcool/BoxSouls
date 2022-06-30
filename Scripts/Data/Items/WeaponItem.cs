using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSouls
{
    [CreateAssetMenu(menuName = "BoxSouls/Item/Weapon", order = -100)]
    public class WeaponItem : Item
    {
        public GameObject prefab;
        public Transform instTr;
        public WeaponDamageCollider weaponDamageCollider;

        [Header("Anim Name")]
        public string leftHandAnimPrefix = Consts.AnimatorStateNameComposition.OH_ATTACK_PREFIX;
        public string rightHandAnimPrefix = Consts.AnimatorStateNameComposition.OH_ATTACK_PREFIX;
        public string twoHandsAnimPrefix = Consts.AnimatorStateNameComposition.TH_ATTACK_PREFIX;
        [Header("Sprint Attack Anim")]
        public int sprintAttackLeftHandAnimId = 3;
        public int sprintAttackRightHandAnimId = 3;
        public int sprintAttackTwoHandsAnimId = 2;


        public int comboCount = 3;
    }
}
