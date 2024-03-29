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
    public class PlayerLocomotion : BaseUpdater
    {
        public RigidbodyCharater rigidCharacter = new RigidbodyCharater();

        [Header("Movement")]
        public float moveSpeed = 4;
        public float sprintSpeed = 6;

        [Header("State Vars")]
        public Vector3 moveDir;
        public Vector3 moveDirToLockedTarget;
        public float moveAmount;
        public Vector3 rootMotionVelocity;

        [Header("Lock Target")]
        public float maxLockDistance = 20;
        public LayerMask lockLayer = 1;
        public string canLockTag = Consts.Tags.CanHit;

        public bool IsGrounded => rigidCharacter.isGrounded;

        public override void Init(PlayerControl playerControl)
        {
            base.Init(playerControl);

            rigidCharacter.rigid = rigid;
        }
        public override void Update()
        {
            var movementInput = inputControl.movement;
            moveDir = CameraTools.CalcMoveDirection(playerControl.camTr, movementInput);
            UpdateMoveAmount(movementInput);

            UpdateTryLock();

            if (playerControl.IsLockedTarget)
                moveDirToLockedTarget = (playerControl.attackTarget.position - transform.position - Vector3.up*2);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            var deltaTime = Time.deltaTime;
            var moveScale = playerControl.IsInteracting ? 0 : 1;
            var scaledMoveDir = moveDir * moveScale;
            //moveDirToAttackTarget *= moveScale;

            // 连击之间,可以改变方向
            var rotatedMoveDir = playerControl.CanCombo ? moveDir : scaledMoveDir;
            var rotatedSpeed = playerControl.CanCombo ? 8 : 10;
            UpdateRotate(rotatedMoveDir, deltaTime, rotatedSpeed);

            UpdateMove(scaledMoveDir);
            UpdateJump(scaledMoveDir, deltaTime);
        }

        void UpdateTryLock()
        {
            if (inputControl.tryLock)
            {
                inputControl.tryLock = false;

                playerControl.attackTarget = CameraTools.RaycastTarget(playerControl.camTr, maxLockDistance, lockLayer,
                    (c) => !c.CompareTag(Consts.Tags.Player) && c.GetComponentInChildren<CharacterControl>()
                    );
            }
        }

        private void UpdateJump(Vector3 moveDir, float fixedDeltaTime)
        {
            if (inputControl.isJump && IsGrounded)
            {
                rigidCharacter.isJump = true;

                playerAnim.JumpLaunch();
            }
            inputControl.isJump = false;
        }

        ///----------------------------- player movement
        void UpdateMoveAmount(Vector2 movementInput)
        {
            moveAmount = Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y));
            if (inputControl.isSprint)
                moveAmount *= 2;
        }

        void UpdateMove(Vector3 moveDir)
        {
            var targetSpeed = inputControl.isSprint ? sprintSpeed : moveSpeed;

            moveDir *= targetSpeed;

            rigidCharacter.rootMotionVelocity = rootMotionVelocity;
            rigidCharacter.MoveRigidbody(ref moveDir);

            Debug.DrawRay(rigid.position, moveDir, Color.blue);
        }

        void UpdateRotate(Vector3 moveDir, float deltaTime,float rotateSpeed=10)
        {
            if (playerControl.IsLockedTarget)
            {
                moveDir = moveDirToLockedTarget;
            }

            moveDir.y = 0;

            if (moveDir.sqrMagnitude < 0.01f)
                moveDir = transform.forward;

            var lookTargetForward = playerControl.cameraLookTarget.forward;

            transform.forward = Vector3.Slerp(transform.forward, moveDir, deltaTime * rotateSpeed);
            playerControl.cameraLookTarget.forward = lookTargetForward;
        }

    }
}
