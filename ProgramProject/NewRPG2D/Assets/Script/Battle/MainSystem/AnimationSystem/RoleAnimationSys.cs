using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spine;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Assets.Script.Battle
{
    public class RoleAnimationSys
    {
        private SkeletonAnimation roleAnimator;
        private RoleBase mCurrentRole;
        private AnimationState roleAnimationState;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleAnimator = mCurrentRole.RoleAnimator;
            roleAnimationState = roleAnimator.state;
        }

        public void ChangeAniamtionNameById(int animationId)
        {
        }

 

        #region animation function

        public TrackEntry SetCurrentAniamtionByName(string animationName)
        {
            return roleAnimationState.SetAnimation(0, animationName, false);
        }

        public TrackEntry SetCurrentAniamtionByName(string animationName, bool loop)
        {
            return roleAnimationState.SetAnimation(0, animationName, loop);
        }

        public void AddCompleteListener(AnimationState.TrackEntryDelegate callBack)
        {
            roleAnimationState.Complete += callBack;
        }

        public void RemoveCompleteListener(AnimationState.TrackEntryDelegate callBack)
        {
            roleAnimationState.Complete -= callBack;
        }
        #endregion
    }
}
