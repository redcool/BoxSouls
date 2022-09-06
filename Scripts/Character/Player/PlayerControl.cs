using GameUtilsFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cinemachine;
using System;

namespace BoxSouls
{
    /// <summary>
    /// Update Player States
    /// </summary>
    public class PlayerControl : CharacterControl
    {
        [Header("Components")]
        public InputControl inputControl;
        public Rigidbody rigid;
        public Transform camTr;
        public Animator anim;
        public CinemachineVirtualCamera virtualCamera;

        [Header("Updater")]
        public PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        public PlayerCamera playerCamera = new PlayerCamera();
        public PlayerAnim playerAnim = new PlayerAnim();
        public PlayerWeaponControl playerWeaponControl = new PlayerWeaponControl();
        public PlayerUIControl playerUIControl = new PlayerUIControl();
        public PlayerStatesControl playerStatesControl = new PlayerStatesControl();

        [Header("Attack Target")]
        public Transform attackTarget;

        [Header("Camera Look")]
        public Transform cameraLookTarget;

        [Header("Screen Cursor ")]
        public bool isLockCursor;

        public List<BaseUpdater> updateList = new List<BaseUpdater>();
        public List<BaseUpdater> fixedUpdateList = new List<BaseUpdater>();
        public List<BaseUpdater> lateUpdateList = new List<BaseUpdater>();

        // Start is called before the first frame update
        void Awake()
        {
            inputControl = GetComponent<InputControl>();
            rigid = GetComponent<Rigidbody>();
            camTr = Camera.main.transform;
            anim = GetComponentInChildren<Animator>();

            InitUpdaters();

            if (isLockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void InitUpdaters()
        {
            playerLocomotion.Init(this);
            playerCamera.Init(this);
            playerAnim.Init(this);
            playerWeaponControl.Init(this);
            playerUIControl.Init(this);
            playerStatesControl.Init(this);

            updateList.AddRange(new BaseUpdater[] { 
                playerLocomotion,
                playerAnim,
                playerUIControl ,
                playerStatesControl
            });

            fixedUpdateList.AddRange(new BaseUpdater[] { 
                playerLocomotion
            });

            lateUpdateList.AddRange(new BaseUpdater[] { 
                playerCamera
            });
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < updateList.Count; i++)
            {
                updateList[i].Update();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < fixedUpdateList.Count; i++)
            {
                fixedUpdateList[i].FixedUpdate();
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < lateUpdateList.Count; i++)
            {
                lateUpdateList[i].LateUpdate();
            }
        }

        private void OnDestroy()
        {
            playerLocomotion.Destroy();
            playerCamera.Destroy();
            playerAnim.Destroy();
            playerWeaponControl.Destroy();
            playerUIControl.Destroy();
            playerStatesControl.Destroy();
        }

        public bool IsLockedTarget => attackTarget;
        public bool IsInteracting => anim.GetBool(Consts.AnimatorParameters.IsInteracting);
        public bool IsFalling => anim.GetBool(Consts.AnimatorParameters.IsFalling);
        public bool IsGrounded => playerLocomotion.IsGrounded;
        public bool IsJumpLaunch => anim.GetBool(Consts.AnimatorParameters.IsJumpLaunch);
        public bool CanCombo => anim.GetBool(Consts.AnimatorParameters.CanCombo);
        public bool IsTwoHandsHolding => anim.GetBool(Consts.AnimatorParameters.IsTwoHands);
        public bool IsTwoHandsHoldingLeftWeapon => IsTwoHandsHolding && !isLeftHandPutBack;

        [Header("Attacking info")]
        public bool isLeftAttacking; // attacking use left hand, driving by PlayerAnim
        public bool isLeftHandPutBack; // driving by PlayerAnim

        #region Animation Event Receive
        public void OnAnimatorMoved(Vector3 velocity)
        {
            playerLocomotion.rootMotionVelocity = velocity;
        }

        public override void OnOpenDamageTrigger()
        {
            playerWeaponControl.OpenDamageTrigger();
        }

        public override void OnCloseDamageTrigger()
        {
            playerWeaponControl.CloseDamageTrigger();
        }

        public void OnPutBackWeapon()
        {
            playerWeaponControl.PutBackWeapon(isLeftHandPutBack);
        }
        #endregion
    }
}