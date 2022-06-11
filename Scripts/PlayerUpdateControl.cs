using GameUtilsFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BoxSouls
{

    public class PlayerUpdateControl : MonoBehaviour
    {
        [Header("Components")]
        public InputControl inputControl;
        public Rigidbody rigid;
        public Transform camTr;
        public Animator anim;


        [Header("Updater")]
        public PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        public PlayerCamera playerCamera = new PlayerCamera();
        public PlayerAnim playerAnim = new PlayerAnim();

        [Header("Attack Target")]
        public Transform attackTarget;

        [Header("Camera Look")]
        public Transform cameraLookTarget;

        [Header("State Vars")]
        public Vector3 moveDir;
        public Vector3 moveDirToAttackTarget;
        public Vector3 MoveDir => moveDir;
        public float moveAmount;
        public Vector3 rootMotionVelocity;

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


            playerLocomotion.Init(this);
            playerCamera.Init(this);
            playerAnim.Init(this);

            updateList.AddRange(new BaseUpdater[] {playerLocomotion,playerAnim });
            fixedUpdateList.AddRange(new BaseUpdater[] { playerLocomotion});
            lateUpdateList.AddRange(new BaseUpdater[] {playerCamera });
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

        public bool IsLockTarget => attackTarget;
        public bool IsInteracting => anim.GetBool(Consts.AnimatorParameters.IS_INTERACTING);
        public bool IsFalling => anim.GetBool(Consts.AnimatorParameters.IS_FALLING);
        public bool IsGrounded => playerLocomotion.IsGrounded;
        public bool IsJumpLaunch => anim.GetBool(Consts.AnimatorParameters.IS_JUMP_LUANCH);



    }
}