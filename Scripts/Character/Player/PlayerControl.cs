using GameUtilsFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cinemachine;
using System;

namespace BoxSouls
{

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

        public bool IsLockedTarget => attackTarget;
        public bool IsInteracting => anim.GetBool(Consts.AnimatorParameters.IS_INTERACTING);
        public bool IsFalling => anim.GetBool(Consts.AnimatorParameters.IS_FALLING);
        public bool IsGrounded => playerLocomotion.IsGrounded;
        public bool IsJumpLaunch => anim.GetBool(Consts.AnimatorParameters.IS_JUMP_LUANCH);
        public bool CanCombo => anim.GetBool(Consts.AnimatorParameters.CAN_COMBO);

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

        public void OnPutBackWeapon(bool isLeftHand)
        {
            playerWeaponControl.PutBackWeapon(isLeftHand);
        }
    }
}