using GameUtilsFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoxSouls
{
    public abstract class BaseUpdater
    {
        protected PlayerControl playerControl;
        public Transform transform => playerControl.transform;
        public InputControl inputControl => playerControl.inputControl;
        public Rigidbody rigid => playerControl.rigid;
        public Animator anim => playerControl.anim;
        public PlayerAnim playerAnim => playerControl.playerAnim;
        public PlayerLocomotion playerLocomotion => playerControl.playerLocomotion;
        public PlayerWeaponControl playerWeaponControl => playerControl.playerWeaponControl;
        public PlayerUIControl playerUIControl => playerControl.playerUIControl;
        public PlayerStatesControl playerStatesControl => playerControl.playerStatesControl;

        public virtual void Init(PlayerControl playerControl) {
            this.playerControl = playerControl;
        }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
    }
}
