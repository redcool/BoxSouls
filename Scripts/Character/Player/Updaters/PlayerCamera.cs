﻿using Cinemachine;
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
        [Range(0,1)]public float virtualCameraSide = 0.5f;
        public float cameraRotationSmoothSpeed = 20;

        float rotateX;
        float rotateY;
        Transform cameraLookTarget => playerControl.cameraLookTarget;

        Cinemachine3rdPersonFollow tpsFollow;

        public override void Init(PlayerControl playerControl)
        {
            base.Init(playerControl);

            InitCameraInput();

            if(playerControl.virtualCamera)
            tpsFollow = playerControl.virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
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
            if (playerControl.IsLockedTarget)
            {
                var dir = playerLocomotion.moveDirToLockedTarget;
                //cameraLookTarget.forward = Vector3.RotateTowards(cameraLookTarget.forward, dir,Time.deltaTime,0);
                cameraLookTarget.forward = Vector3.Slerp(cameraLookTarget.forward, dir, cameraRotationSmoothSpeed * Time.deltaTime);
                UpdateVirtualCameraSide(virtualCameraSide);
                return;
            }

            rotateX -= lookInputDir.y * cameraSensitive * deltaTime;
            rotateY += lookInputDir.x * cameraSensitive * deltaTime;

            rotateX = Mathf.Clamp(rotateX, -45, 45);
            rotateY %= 360;

            var targetRotation = Quaternion.Euler(rotateX, rotateY, 0);
            cameraLookTarget.rotation = Quaternion.Lerp(cameraLookTarget.rotation, targetRotation, cameraRotationSmoothSpeed * Time.deltaTime);
            //cameraLookTarget.rotation = Quaternion.RotateTowards(cameraLookTarget.rotation, targetRotation, 180*Time.deltaTime);

            UpdateVirtualCameraSide(0.5f);
        }

        void UpdateVirtualCameraSide(float sideValue)
        {
            if (!tpsFollow)
                return;

            tpsFollow.CameraSide = Mathf.Lerp(tpsFollow.CameraSide, sideValue, Time.time * 2);

        }

    }
}
