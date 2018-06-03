using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class RoleAnimationSys
    {
        private Animator roleAnimator;
        private RoleBase mCurrentRole;
        private AnimatorOverrideController animatorOverrideController;
        private string currentAnimationName = "RoleAnimation";
        private Dictionary<int, AnimationClip> AnimationClipDic;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            animatorOverrideController = new AnimatorOverrideController(roleAnimator.runtimeAnimatorController);
            roleAnimator.runtimeAnimatorController = animatorOverrideController;
            AnimationClipDic = new Dictionary<int, AnimationClip>(10);
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
            AnimationClipDic[(int)animationType] = clip;
        }

        #region animation function

        public void SetCurrentAniamtionByName(AnimationNameEnum animationName)
        {
            animatorOverrideController[currentAnimationName] = AnimationClipDic[(int)animationName];
            roleAnimator.Play(currentAnimationName);
        }

        public void SetTrasnsitionHasExitTime(bool hasExitTime)
        {
        }

        public void SetCurrentAniamtionByName(string animationName)
        {
        }

        public void SetCurrentAniamtionByBool(string triggerName, bool bState)
        {
            roleAnimator.SetBool(triggerName, bState);
        }

        public void SetCurrentAniamtionByTrigger(string triggerName)
        {
            roleAnimator.SetTrigger(triggerName);
        }

        public void SetCurrentAniamtionByInteger(string triggerName, int value)
        {
            roleAnimator.SetInteger(triggerName, value);
        }
        public void SetCurrentAniamtionByFloat(string triggerName, float value)
        {
            roleAnimator.SetFloat(triggerName, value);
        }

        public void SetCurrentAniamtionByFloat(string triggerName, string triggerCurveName)
        {
            roleAnimator.SetFloat(triggerName, GetCurrentAniamtionByFloat(triggerCurveName));
        }

        public float GetCurrentAniamtionByFloat(string triggerName)
        {
            return roleAnimator.GetFloat(triggerName);
        }

        public int GetCurrentAniamtionByInt(string triggerName)
        {
            return roleAnimator.GetInteger(triggerName);
        }
        #endregion
    }
}
