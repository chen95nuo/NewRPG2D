using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonBones;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class RoleAnimationSys
    {
        private UnityArmatureComponent roleAnimator;
        private RoleBase mCurrentRole;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleAnimator = mCurrentRole.RoleAnimator;
        }

        public void SetSwitchAnimation(AnimationNameEnum animationName)
        {
            SetCurrentAniamtionByName(animationName);
        }

        public void ChangeAniamtionNameById(int animationId)
        {
        }

        private void SetAnimationClipDic(AnimationNameEnum animationType, AnimationClip clip)
        {
        }

        #region animation function

        public void SetCurrentAniamtionByName(AnimationNameEnum animationName)
        {
            roleAnimator.animation.Play(animationName.ToString());
        }

        #endregion
    }
}
