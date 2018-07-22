using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonBones;
using UnityEngine;
using Animation = DragonBones.Animation;

namespace Assets.Script.Battle
{
    public class RoleAnimationSys
    {
        private UnityArmatureComponent roleAnimator;
        private RoleBase mCurrentRole;
        private Animation roleAnimation;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleAnimator = mCurrentRole.RoleAnimator;
            roleAnimation = roleAnimator.animation;
        }

        public void ChangeAniamtionNameById(int animationId)
        {
        }

 

        #region animation function

        public float SetCurrentAniamtionByName(string animationName)
        {
            roleAnimation.Play(animationName);
            return roleAnimation.GetState(animationName)._duration;
        }

        public bool IsComplete()
        {
            return roleAnimation.isCompleted;
        }

        #endregion
    }
}
