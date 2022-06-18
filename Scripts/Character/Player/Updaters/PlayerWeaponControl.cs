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

            Equip(defaultLeftWeapon, true);
            Equip(defaultRightWeapon, false);
        }

        public override void Update()
        {
            base.Update();


        }

        public void Equip(WeaponItem item,bool isLeft)
        {
            if (!item)
                return;

            var weaponInst = Object.Instantiate(item.prefab);

            weaponInst.transform.parent = (isLeft ? leftSocket : rightSocket).transform;
            weaponInst.transform.localPosition = Vector3.zero;
            weaponInst.transform.localScale = Vector3.one;
            weaponInst.transform.localRotation = Quaternion.identity;

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

        public int GetMaxCombo(bool isLeftWeapon, bool isRightWeapon)
        {
            if (isRightWeapon && rightWeapon)
                return rightWeapon.comboCount;

            if (leftWeapon)
                return leftWeapon.comboCount;
            return 1;
        }

        public void OpenDamageTrigger() {
            rightWeapon.weaponDamageCollider.OpenTrigger();
        }
        public void CloseDamageTrigger()
        {
            rightWeapon.weaponDamageCollider.CloseTrigger();
        }
    }
}
