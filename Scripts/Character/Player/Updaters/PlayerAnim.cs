﻿using GameUtilsFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoxSouls
{
    [Serializable]
    public class PlayerAnim : BaseUpdater
    {

        public bool IsInJumpState =>
            anim.IsInState(Consts.AnimatorLayerNames.OVERRIDE, Consts.AnimatorStateNames.JUMP_LAUNCH)
            || anim.IsInState(Consts.AnimatorLayerNames.OVERRIDE, Consts.AnimatorStateNames.FALLING)
            || anim.IsInState(Consts.AnimatorLayerNames.OVERRIDE, Consts.AnimatorStateNames.LAND);

        public override void Update()
        {
            UpdateMoveAnim();
            UpdateRollingSprint();
            UpdateFallingAndLand();
            UpdateAttack();
        }

        public void PlayAnimAndSetInteracting(string stateName,bool isInteracting)
        {
            anim.PlayAnimSetBool(stateName, Consts.AnimatorParameters.IS_INTERACTING, isInteracting);
        }

        public void JumpLaunch()
        {
            anim.SetBool(Consts.AnimatorParameters.IS_JUMP_LUANCH, true);
            //PlayAnimAndSetInteracting(Consts.AnimatorStateNames.JUMP_LAUNCH, true);
            anim.CrossFade(Consts.AnimatorStateNames.JUMP_LAUNCH, 0.2f);
        }

        private void UpdateMoveAnim()
        {
            if (playerControl.IsInteracting)
                return;

            var speedX = inputControl.movement.x;
            var speedZ = inputControl.movement.y;
            if (!playerControl.IsLockTarget)
            {
                speedX = 0;
                speedZ = playerLocomotion.moveAmount;
            }

            anim.SetFloat(Consts.AnimatorParameters.SPEED_X, speedX, 0.2f, Time.deltaTime);
            anim.SetFloat(Consts.AnimatorParameters.SPEED_Z, speedZ, 0.2f, Time.deltaTime);
        }

        void UpdateRollingSprint()
        {
            var canTriggerRolling = inputControl.isRolling && !playerControl.IsInteracting && ! IsInJumpState;
            if (canTriggerRolling)
            {
                var moveDir = playerLocomotion.moveDir;
                moveDir.y = 0;

                anim.SetBool(Consts.AnimatorParameters.IS_INTERACTING, true);

                if (moveDir.sqrMagnitude == 0)
                    anim.CrossFade(Consts.AnimatorStateNames.STEP_BACK, 0.1f);
                else
                    anim.CrossFade(Consts.AnimatorStateNames.ROLLING, 0.1f);
            }
            inputControl.isRolling = false;
        }

        void UpdateFallingAndLand()
        {
            var isFalling = playerControl.IsFalling;
            var isGrounded = playerControl.IsGrounded;
            var isJumpLaunch = playerControl.IsJumpLaunch;

            // land trigger
            if (isGrounded && isFalling)
            {
                anim.SetBool(Consts.AnimatorParameters.IS_FALLING, false);
                PlayAnimAndSetInteracting(Consts.AnimatorStateNames.LAND, true);
            }

            // falling trigger
            if (!isGrounded && !isFalling && !isJumpLaunch)
            {

                anim.SetBool(Consts.AnimatorParameters.IS_FALLING, true);
                PlayAnimAndSetInteracting(Consts.AnimatorStateNames.FALLING, false);
            }
        }

        void UpdateAttack()
        {
            var rightHandAttack = inputControl.RB;
            var leftAttack = inputControl.RT;
            var isTwoHands = anim.GetBool(Consts.AnimatorParameters.IS_TWO_HANDS);

            //var attackName = Consts.AnimatorStateNames.OH_ATTACK1;
            //if (leftAttack)
            //    attackName += Consts.AnimatorStateNameComposition.LEFT_ATTACK_SUFFIX;

            var canAttack = (leftAttack || rightHandAttack); // trigger 
            canAttack = canAttack && !playerControl.IsInteracting;// condition

            if (canAttack)
            {
                var attackName = Consts.AnimatorStateNameComposition.GetAttackName(isTwoHands, leftAttack, 1);

                PlayAnimAndSetInteracting(attackName, true);
            }

            UpdateComboAttack(leftAttack, rightHandAttack,isTwoHands);

            inputControl.RB = false;
            inputControl.RT = false;
        }

        void UpdateComboAttack(bool leftAttack,bool rightAttack,bool isTwoHands)
        {
            var attackIndex = anim.GetInteger(Consts.AnimatorParameters.ATTACK_INDEX);
            var maxCombo = playerWeaponControl.GetMaxCombo(leftAttack, rightAttack);

            if (playerControl.CanCombo && (leftAttack || rightAttack))
            {
                //if (attackIndex >= 1)
                //    Debug.Log("combo 2");

                attackIndex++;
                attackIndex %= maxCombo;

                anim.SetInteger(Consts.AnimatorParameters.ATTACK_INDEX, attackIndex);
                anim.SetBool(Consts.AnimatorParameters.CAN_COMBO, false);

                var attackName = Consts.AnimatorStateNameComposition.GetAttackName(isTwoHands, leftAttack, attackIndex + 1);
                PlayAnimAndSetInteracting(attackName, true);
            }
        }

        public void UpdateWeaponIdle(bool isLeft)
        {
            var idleName = isLeft ? Consts.AnimatorStateNames.LEFT_HAND_IDLE : Consts.AnimatorStateNames.RIGHT_HAND_IDLE;
            anim.CrossFade(idleName, 0.2f);

        }
    }
}
