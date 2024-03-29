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
            UpdateTwoHandsHolding();
        }

        public void PlayAnimAndSetInteracting(string stateName,bool isInteracting,float transitionDuration=0.2f)
        {
            anim.PlayAnimSetBool(stateName, Consts.AnimatorParameters.IsInteracting, isInteracting,transitionDuration);
        }

        public void JumpLaunch()
        {
            anim.SetBool(Consts.AnimatorParameters.IsJumpLaunch, true);
            //PlayAnimAndSetInteracting(Consts.AnimatorStateNames.JUMP_LAUNCH, true);
            anim.CrossFade(Consts.AnimatorStateNames.JUMP_LAUNCH, 0.2f);
        }

        private void UpdateMoveAnim()
        {
            if (playerControl.IsInteracting)
                return;

            var speedX = inputControl.movement.x;
            var speedZ = inputControl.movement.y;
            if (!playerControl.IsLockedTarget)
            {
                speedX = 0;
                speedZ = playerLocomotion.moveAmount;
            }

            anim.SetFloat(Consts.AnimatorParameters.SpeedX, speedX, 0.2f, Time.deltaTime);
            anim.SetFloat(Consts.AnimatorParameters.SpeedZ, speedZ, 0.2f, Time.deltaTime);
        }

        void UpdateRollingSprint()
        {
            var canTriggerRolling = inputControl.isRolling && !playerControl.IsInteracting && ! IsInJumpState;
            if (canTriggerRolling)
            {
                var moveDir = playerLocomotion.moveDir;
                moveDir.y = 0;

                anim.SetBool(Consts.AnimatorParameters.IsInteracting, true);

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
                anim.SetBool(Consts.AnimatorParameters.IsFalling, false);
                PlayAnimAndSetInteracting(Consts.AnimatorStateNames.LAND, true);
            }

            // falling trigger
            if (!isGrounded && !isFalling && !isJumpLaunch)
            {

                anim.SetBool(Consts.AnimatorParameters.IsFalling, true);
                PlayAnimAndSetInteracting(Consts.AnimatorStateNames.FALLING, false);
            }
        }

        void UpdateAttack()
        {
            var rightHandAttack = inputControl.IsRightHandAttack();
            var leftHandAttack = inputControl.IsLeftHandAttack();
            var isTwoHands = playerControl.IsTwoHandsHolding;


            //var attackName = Consts.AnimatorStateNames.OH_ATTACK1;
            //if (leftAttack)
            //    attackName += Consts.AnimatorStateNameComposition.LEFT_ATTACK_SUFFIX;

            var isMainAttackTrigger = (leftHandAttack || rightHandAttack); // trigger 
            isMainAttackTrigger = isMainAttackTrigger && !playerControl.IsInteracting;// condition

            var firstAttackIndex = inputControl.isSprint ? playerWeaponControl.GetSprintAttackAnimId(isTwoHands,leftHandAttack, rightHandAttack) : 1; // sprint use WeaponInfo, otherwist use 1

            // 1 main attack or trigger combo
            if (isMainAttackTrigger)
            {
                
                playerControl.isLeftAttacking = leftHandAttack;

                var attackName = Consts.AnimatorStateNameComposition.GetAttackName(isTwoHands, leftHandAttack, firstAttackIndex);
                Debug.Log($"main attack:{attackName}, holding left? { playerControl.IsTwoHandsHoldingLeftWeapon}");
                PlayAnimAndSetInteracting(attackName, true);
                playerStatesControl.ConsumeEnergy(25);
            }

            // 2 cannot trigger main attack, try trigger combo attack
            UpdateComboAttack(leftHandAttack, rightHandAttack, isTwoHands);

            if (leftHandAttack)
                inputControl.ResetLeftHandAttack();
            if (rightHandAttack)
                inputControl.ResetRightHandAttack();
        }

        /// <summary>
        /// 更新连击动作
        /// 
        /// </summary>
        /// <param name="leftHandAttack"></param>
        /// <param name="rightHandAttack"></param>
        /// <param name="isTwoHands"></param>
        void UpdateComboAttack(bool leftHandAttack,bool rightHandAttack,bool isTwoHands)
        {
            var attackIndex = anim.GetInteger(Consts.AnimatorParameters.AttackIndex);
            var maxCombo = playerWeaponControl.GetMaxCombo(leftHandAttack, isTwoHands);

            var canCombo = playerControl.CanCombo && (leftHandAttack || rightHandAttack);
            canCombo = canCombo && (leftHandAttack == playerControl.isLeftAttacking);

            if (canCombo)
            {
                //if (attackIndex >= 1)
                //    Debug.Log("combo 2");

                attackIndex++;
                attackIndex %= maxCombo;

                anim.SetInteger(Consts.AnimatorParameters.AttackIndex, attackIndex);
                anim.SetBool(Consts.AnimatorParameters.CanCombo, false);

                var attackName = Consts.AnimatorStateNameComposition.GetAttackName(isTwoHands, leftHandAttack, attackIndex + 1);
                Debug.Log("Combo attack :" + attackName);

                PlayAnimAndSetInteracting(attackName, true,0.1f);
                playerStatesControl.ConsumeEnergy(25);
            }
        }

        public void UpdateWeaponIdle(bool isLeft)
        {
            var idleName = isLeft ? Consts.AnimatorStateNames.LEFT_HAND_IDLE : Consts.AnimatorStateNames.RIGHT_HAND_IDLE;
            anim.CrossFade(idleName, 0.2f);

        }

        public void UpdateTwoHandsHolding()
        {
            if (playerControl.IsInteracting)
                return;

            /**
             * 1 take back left weapon
             * 2 two hands holding right weapon
             * 
             * */
            if (inputControl.IsHoldRightWeapon())
            {
                inputControl.ResetRightHandAttack();
                playerControl.isLeftHandPutBack = true;

                var isTwoHandsHoldCurrent = playerControl.IsTwoHandsHolding;
                anim.SetBool(Consts.AnimatorParameters.IsTwoHands,!isTwoHandsHoldCurrent);
                anim.SetBool(Consts.AnimatorParameters.IsLeftHandPutBack, true);

                if (isTwoHandsHoldCurrent)
                {
                    // equip left and right weapon
                    playerWeaponControl.EquipWeapon(false);
                    playerWeaponControl.EquipWeapon(true);
                }
                return;
            }

            //3 two hands holding left weapon
            if (inputControl.IsHoldLeftWeapon())
            {
                inputControl.ResetLeftHandAttack();
                playerControl.isLeftHandPutBack = false;

                var isTwoHandsHoldCurrent = playerControl.IsTwoHandsHolding;
                anim.SetBool(Consts.AnimatorParameters.IsTwoHands, !isTwoHandsHoldCurrent);
                anim.SetBool(Consts.AnimatorParameters.IsLeftHandPutBack, false);

                if (isTwoHandsHoldCurrent)
                {
                    // equip left and right weapon
                    playerWeaponControl.EquipWeapon(false);
                    playerWeaponControl.EquipWeapon(true);
                }
                return;
            }
        }
    }
}
