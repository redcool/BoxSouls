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
    public class PlayerLocomotion : BaseUpdater
    {
        public RigidbodyCharater rigidCharacter = new RigidbodyCharater();

        [Header("Movement")]
        public float moveSpeed = 4;
        public float sprintSpeed = 6;

        [Header("State Vars")]
        public Vector3 moveDir;
        public Vector3 moveDirToAttackTarget;
        public float moveAmount;
        public Vector3 rootMotionVelocity;


        public bool IsGrounded => rigidCharacter.isGrounded;

        public override void Init(PlayerUpdateControl playerControl)
        {
            base.Init(playerControl);

            rigidCharacter.rigid = rigid;
        }
        public override void Update()
        {
            var movementInput = inputControl.movement;
            moveDir = CameraTools.CalcMoveDirection(playerControl.camTr, movementInput);
            UpdateMoveAmount(movementInput);


            if (playerControl.IsLockTarget)
                moveDirToAttackTarget = (playerControl.attackTarget.position - transform.position - Vector3.up*2);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            var moveScale = playerControl.IsInteracting ? 0 : 1;

            moveDir *= moveScale;
            //moveDirToAttackTarget *= moveScale;

            var deltaTime = Time.deltaTime;

            UpdateMove(moveDir);
            UpdateRotate(moveDir, deltaTime);
            UpdateJump(moveDir, deltaTime);
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

        void UpdateRotate(Vector3 moveDir, float deltaTime)
        {
            if (playerControl.IsLockTarget)
            {
                moveDir = moveDirToAttackTarget;
            }

            moveDir.y = 0;

            if (moveDir.sqrMagnitude < 0.01f)
                moveDir = transform.forward;

            var lookTargetForward = playerControl.cameraLookTarget.forward;

            transform.forward = Vector3.Slerp(transform.forward, moveDir, deltaTime * 10);
            playerControl.cameraLookTarget.forward = lookTargetForward;
        }

    }
}
