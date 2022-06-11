using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxSouls
{
    public static class Consts
    {
        public class AnimatorParameters
        {
            public const string
                ATTACK_1 = "Attack 1",
                SPEED_X = "SpeedX",
                SPEED_Z = "SpeedZ",
                IS_INTERACTING = "IsInteracting",
                IS_ROLLING = "IsRolling",
                IS_FALLING = "IsFalling",
                IS_JUMP_LUANCH = "IsJumpLaunch"

        ;

        }

        public class AnimatorStateNames
        {
            public const string ROLLING = "Rolling",
                STEP_BACK = "StepBack",
                FALLING="Falling",
                LAND="Land",
                JUMP_LAUNCH ="JumpLaunch"
                ;

        }
    }
}
