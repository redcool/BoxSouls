using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BoxSouls
{
    [Serializable]
    public class PlayerUIControl : BaseUpdater
    {
        public Slider hpBarSlider, energyBarSlider;
        public Image leftWeapon, rightWeapon;

        [Header("Icons")]
        public Sprite[] sprites;
        public Sprite GetSprite(string name)
        {
            return Array.Find(sprites, x => x.name == name);
        }

        public override void Init(PlayerControl playerControl)
        {
            base.Init(playerControl);

            //playerWeaponControl.OnWeaponEquip -= PlayerWeaponControl_OnWeaponEquip;
            //playerWeaponControl.OnWeaponEquip += PlayerWeaponControl_OnWeaponEquip;

            PlayerWeaponControl_OnWeaponEquip(true);
            PlayerWeaponControl_OnWeaponEquip(false);
        }

        private void PlayerWeaponControl_OnWeaponEquip(bool isLeft)
        {
            var image = isLeft ? leftWeapon : rightWeapon;
            var spriteName = isLeft ? playerWeaponControl.LeftWeaponItem.spriteName : playerWeaponControl.RightWeaponItem.spriteName;
            image.sprite = GetSprite(spriteName);
        }

        public override void Update()
        {
            base.Update();

            if(hpBarSlider != null)
            {
                hpBarSlider.normalizedValue = playerControl.characterStats.HpRate;
            }

            if(energyBarSlider != null)
            {
                energyBarSlider.normalizedValue = playerControl.characterStats.EnergyRate;
            }
        }
    }
}
