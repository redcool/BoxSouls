using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BoxSouls
{
    public class ApplyAnimatorRootMove : MonoBehaviour
    {
        public PlayerControl playerControl;
        public Animator anim;

        public Vector3 velocity;
        private void Start()
        {
            playerControl = GetComponentInParent<PlayerControl>();

            Assert.IsNotNull(playerControl);

            anim = playerControl.anim;
        }

        public void OnAnimatorMove()
        {
            var pos = anim.deltaPosition;
            velocity = pos / Time.deltaTime;

            playerControl.playerLocomotion.rootMotionVelocity = velocity;
        }

    }
}
