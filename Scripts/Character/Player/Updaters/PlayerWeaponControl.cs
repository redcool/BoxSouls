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
    public class PlayerWeaponControl : BaseUpdater
    {
        public WeaponSocket leftSocket, rightSocket;

        public WeaponItem defaultRightWeapon,defaultLeftWeapon;
        public WeaponItem leftWeapon, rightWeapon;

        [Header("Put back weapon position")]
        public Transform leftWeaponPosition;
        public Transform rightWeaponPosition;

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

            item.instTr = weaponInst.transform;

            item.weaponDamageCollider = weaponInst.GetComponent<WeaponDamageCollider>();
            item.weaponDamageCollider.Init(playerControl);

            if (isLeft)
                leftWeapon = item;
            else
                rightWeapon = item;

            playerAnim.UpdateWeaponIdle(isLeft);
        }

        public void Unequip(WeaponItem item, bool isLeft)
        {
            leftWeapon = null;
            rightWeapon = null;
        }

        public int GetMaxCombo(bool isLeftAttack, bool isRightAttackOrTwoHands)
        {
            if (isRightAttackOrTwoHands && rightWeapon)
                return rightWeapon.comboCount;

            if (leftWeapon)
                return leftWeapon.comboCount;
            return 1;
        }

        public int GetSprintAttackAnimId(bool isLeftAttack, bool isRightAttack)
        {
            if (isRightAttack && rightWeapon)
                return rightWeapon.sprintAttackRightHandAnimId;

            if (isLeftAttack && leftWeapon)
                return leftWeapon.sprintAttackLeftHandAnimId;
            
            return rightWeapon.sprintAttackTwoHandsAnimId;
        }

        public void OpenDamageTrigger() {
            rightWeapon.weaponDamageCollider.OpenTrigger();
        }
        public void CloseDamageTrigger()
        {
            rightWeapon.weaponDamageCollider.CloseTrigger();
        }

        public void PutBackWeapon(bool isLeftHand)
        {
            if (isLeftHand)
            {
                if(leftWeapon)
                    leftWeapon.instTr.SetParent(leftWeaponPosition,false);
            }
            else
            {
                if(rightWeapon) 
                    rightWeapon.instTr.SetParent(rightWeaponPosition,false);
            }
        }
        public void EquipWeapon(bool isLeftHand)
        {
            if (isLeftHand)
            {
                if (leftWeapon)
                    leftWeapon.instTr.SetParent(leftSocket.transform, false);
            }
            else
            {
                if (rightWeapon)
                    rightWeapon.instTr.SetParent(rightSocket.transform, false);
            }
        }
    }
}
