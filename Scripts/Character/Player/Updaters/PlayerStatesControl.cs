using GameUtilsFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoxSouls
{
    [Serializable]
    public class PlayerStatesControl : BaseUpdater
    {
        float lastStartResumeTime = 0;
        public override void Update()
        {
            base.Update();

            if (CanResume())
                playerControl.characterStats.Resume();
        }

        private bool CanResume()
        {
            var isInteracting = anim.GetBool(Consts.AnimatorParameters.IsInteracting);
            if (isInteracting)
            {
                lastStartResumeTime = Time.time;
                return false;
            }

            if (Time.time - lastStartResumeTime > playerControl.characterStats.energyResumeInterval)
                return true;

            return false;
        }

        public void ConsumeEnergy(int v)
        {
            playerControl.characterStats.ConsumeEnergy(v);
        }
    }
}
