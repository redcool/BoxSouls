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

        public WeaponItem defaultWeapon;

        public override void Init(PlayerUpdateControl playerControl)
        {
            base.Init(playerControl);

            var sockets = transform.GetComponentsInChildren<WeaponSocket>(); // only 2
            for (int i = 0; i < sockets.Length; i++)
            {
                rightSocket = sockets[i];

                if (sockets[i].isLeftHand)
                    leftSocket = sockets[i];
            }

            Equip(defaultWeapon, false);
        }

        public override void Update()
        {
            base.Update();


        }

        public void Equip(WeaponItem item,bool isLeft)
        {
            if (!item)
                return;

            var w = Object.Instantiate(item.prefab);
            w.transform.parent = (isLeft ? leftSocket : rightSocket).transform;
            w.transform.localPosition = Vector3.zero;
            w.transform.localScale = Vector3.one;
            w.transform.localRotation = Quaternion.identity;
        }

        public void Unequip(WeaponItem item, bool isLeft)
        {

        }
    }
}
