using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoxSouls
{
    [Serializable]
    public class PlayerUIControl : BaseUpdater
    {
        public Slider hpBarSlider, energyBarSlider;

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
