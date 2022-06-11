using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BoxSouls
{
    [Serializable]
    public class PlayerCamera : BaseUpdater
    {
        [Header("Camera")]
        public float cameraSensitive = 10;

        float rotateX;
        float rotateY;
        Transform cameraLookTarget => playerControl.cameraLookTarget;

        public override void Init(PlayerUpdateControl playerControl)
        {
            base.Init(playerControl);

            InitCameraInput();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            Assert.IsNotNull(cameraLookTarget, "cameraLookTarget must assign !");

            var cameraInputDir = inputControl.look;
            UpdateCameraLookTarget(cameraInputDir, Time.deltaTime);
        }

        void InitCameraInput()
        {
            rotateX = cameraLookTarget.eulerAngles.y;
            rotateY = cameraLookTarget.eulerAngles.x;
        }

        void UpdateCameraLookTarget(Vector2 lookInputDir, float deltaTime)
        {
            if (playerControl.IsLockTarget)
            {
                var dir = playerLocomotion.moveDirToAttackTarget;
                cameraLookTarget.forward = dir;
                return;
            }

            rotateX -= lookInputDir.y * cameraSensitive * deltaTime;
            rotateY += lookInputDir.x * cameraSensitive * deltaTime;

            rotateX = Mathf.Clamp(rotateX, -45, 45);
            rotateY %= 360;

            cameraLookTarget.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        }

    }
}
