using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BoxSouls
{
    [Serializable]
    public class PlayerWeaponInfo
    {
        public WeaponItem weaponItem;
        public Transform weaponInst;
        public bool isLeftEquiped;
        public WeaponDamageCollider weaponDamageCollider;
    }

    [Serializable]
    public class PlayerWeaponControl : BaseUpdater
    {

        [Header("* Default weaponItem")]
        public WeaponItem defaultRightWeapon;
        public WeaponItem defaultLeftWeapon;


        [Header("* Put back weapon position")]
        public Transform leftWeaponBackTr;
        public Transform rightWeaponBackTr;

        [Header("hands weapon socket")]
        public WeaponSocket leftSocket;
        public WeaponSocket rightSocket;

        [Header("weapon info current equiped")]
        public PlayerWeaponInfo leftHandWeaponInfo;
        public PlayerWeaponInfo rightHandWeaponInfo;

        public WeaponItem LeftWeaponItem => leftHandWeaponInfo.weaponItem;
        public WeaponItem RightWeaponItem => rightHandWeaponInfo.weaponItem;

        public override void Init(PlayerControl playerControl)
        {
            base.Init(playerControl);

            var sockets = transform.GetComponentsInChildren<WeaponSocket>(); // only 2
            for (int i = 0; i < sockets.Length; i++)
            {
                rightSocket = sockets[i];

                if (sockets[i].isLeftHand)
                    leftSocket = sockets[i];
            }

            leftHandWeaponInfo = new PlayerWeaponInfo();
            rightHandWeaponInfo = new PlayerWeaponInfo();

            InitWeaponEquip(defaultLeftWeapon, true);
            InitWeaponEquip(defaultRightWeapon, false);
        }

        public override void Update()
        {
            base.Update();
        }

        public void InitWeaponEquip(WeaponItem item,bool isLeft)
        {
            if (!item)
                return;

            var weaponInst = Object.Instantiate(item.prefab);

            weaponInst.transform.parent = (isLeft ? leftSocket : rightSocket).transform;
            weaponInst.transform.localPosition = Vector3.zero;
            weaponInst.transform.localScale = Vector3.one;
            weaponInst.transform.localRotation = Quaternion.identity;

            UpdateWeaponInfo(item, weaponInst.transform, isLeft);

            playerAnim.UpdateWeaponIdle(isLeft);
        }

        void UpdateWeaponInfo(WeaponItem weaponItem,Transform weaponInst,bool isLeft)
        {
            var info = isLeft ? leftHandWeaponInfo : rightHandWeaponInfo;
            info.weaponInst = weaponInst;
            info.weaponItem = weaponItem;
            info.isLeftEquiped = isLeft;

            info.weaponDamageCollider = weaponInst.GetComponent<WeaponDamageCollider>();
            info.weaponDamageCollider.Init(playerControl);

            // test only
            var mr = weaponInst.GetComponentInChildren<MeshRenderer>();
            mr.material.SetColor("_BaseColor", isLeft ? Color.red : Color.green);
        }

        public void Unequip(bool isLeft)
        {
            
        }

        /// <summary>
        /// 获取weaponItem的连击数
        /// </summary>
        /// <param name="isLeft"></param>
        /// <param name="isTwoHands">two hands need find leftWeaponItem or rightWeaponItem </param>
        /// <returns></returns>
        public int GetMaxCombo(bool isLeft,bool isTwoHands)
        {
            if (isTwoHands && rightHandWeaponInfo.weaponItem)
                return rightHandWeaponInfo.weaponItem.comboCount;

            if (leftHandWeaponInfo.weaponItem)
                return leftHandWeaponInfo.weaponItem.comboCount;
            return 1;
        }

        public int GetSprintAttackAnimId(bool isLeftAttack, bool isRightAttack)
        {
            if (isRightAttack && RightWeaponItem)
                return RightWeaponItem.sprintAttackRightHandAnimId;

            if (isLeftAttack && LeftWeaponItem)
                return LeftWeaponItem.sprintAttackLeftHandAnimId;
            
            return RightWeaponItem.sprintAttackTwoHandsAnimId;
        }

        public void OpenDamageTrigger(bool isLeftWeapon) {
            var weaponInfo = isLeftWeapon ? leftHandWeaponInfo : rightHandWeaponInfo;
            weaponInfo.weaponDamageCollider.OpenTrigger();
        }
        public void CloseDamageTrigger(bool isLeftWeapon)
        {
            var weaponInfo = isLeftWeapon ? leftHandWeaponInfo : rightHandWeaponInfo;
            weaponInfo.weaponDamageCollider.CloseTrigger();
        }

        public void PutBackWeapon(bool isLeftHand)
        {
            var info = isLeftHand ? leftHandWeaponInfo : rightHandWeaponInfo;
            var targetParent = isLeftHand ? leftWeaponBackTr: rightWeaponBackTr;
            info.weaponInst.SetParent(targetParent,false);
        }

        public void EquipWeapon(bool isLeftHand)
        {
            var info = isLeftHand ? leftHandWeaponInfo : rightHandWeaponInfo;
            var targetParent = isLeftHand ? leftSocket.transform : rightSocket.transform;
            info.weaponInst.SetParent(targetParent, false);

        }
    }
}
