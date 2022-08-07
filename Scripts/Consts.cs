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
                SpeedX = "SpeedX",
                SpeedZ = "SpeedZ",
                IsTwoHands = "IsTwoHands",
                IsInteracting = "IsInteracting",
                IsFalling = "IsFalling",
                IsJumpLaunch = "IsJumpLaunch",
                CanCombo = "CanCombo",
                AttackIndex = "AttackIndex",
                IsLeftHandPutBack = nameof(IsLeftHandPutBack)
        ;

        }

        public class AnimatorStateNameComposition
        {
            public const string 
                LEFT_ATTACK_SUFFIX = "_left",
                OH_ATTACK_PREFIX = "oh_attack_",
                TH_ATTACK_PREFIX = "th_attack_"

                ;
            public static string GetAttackName(bool isTwoHands,bool isLeftAttack,int index)
            {
                if (isTwoHands)
                    isLeftAttack = false;

                var type = isTwoHands ? TH_ATTACK_PREFIX : OH_ATTACK_PREFIX;
                
                return string.Format("{0}{1}{2}", type, index, isLeftAttack?LEFT_ATTACK_SUFFIX : "");
            }
        }

        public class AnimatorStateNames
        {
            public const string
                ROLL_FORWARD = "RollForward",
                ROLL_LEFT = "RollLeft",
                ROLL_RIGHT = "RollRight",
                ROLLING = "Rolling",
                STEP_BACK = "StepBack",
                FALLING = "Falling",
                LAND = "Land",
                JUMP_LAUNCH = "JumpLaunch",
                OH_ATTACK1 = "oh_attack_1",
                OH_ATTACK2 = "oh_attack_2",
                OH_ATTACK3 = "oh_attack_3",
                TH_ATTACK1 = "th_attack_1",
                TH_ATTACK2 = "th_attack_2",
                LEFT_HAND_IDLE="LeftHandIdle",
                RIGHT_HAND_IDLE = "RightHandIdle",
                Locomotion = nameof(Locomotion),

                DAMAGE="Damage"
                ;

        }

        public class AnimatorLayerNames
        {
            public const string BaseLayer = "Base Layer",
                OVERRIDE = "Override"
                ;
        }

        public class Tags
        {
            public const string
                CanHit = nameof(CanHit),
                Player = nameof(Player),
                Enemy = nameof(Enemy)
                
                ;
        }
    }
}
