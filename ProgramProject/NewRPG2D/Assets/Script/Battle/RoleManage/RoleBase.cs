using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public struct RoleInfo
    {
        public int InstanceId;
        public int TeamId;
        public RoleTypeEnum RoleType;
    }

    public abstract class RoleBase
    {
        public int InstanceId { get; private set; }
        public Animator RoleAnimator { get; private set; }
        public int TeamId { get; private set; }
        public RoleTypeEnum RoleType { get; private set; }
        public Transform RoleTransform { get; private set; }
        public int AnimationId { get; private set; }
        public int CurrentAttackSkillId { get; private set; }
        public bool IsDead { get; private set; }

        public RoleRender MonoRoleRender { get; private set; }
        public RoleAnimationSys RoleAnimation { get; private set; }
        public MoveMoment RoleMoveMoment { get; private set; }
        public ValueProperty RolePropertyValue { get; private set; }
        public DamageMoment RoleDamageMoment { get; private set; }
        public FsmStateMachine<RoleBase> RoleActionMachine;
        public Dictionary<int, FsmState<RoleBase>> RoleActionStateDic; //存放动作状态


        private int[] AttackSkillIdArray;

        public void SetRoleInfo(RoleInfo info, RoleRender roleRender)
        {
            MonoRoleRender = roleRender;
            InitComponent();
            InitFSM();
            InitData();
            SetRoleInfo(info);
        }

        public virtual void SetRoleInfo(RoleInfo info)
        {
            InstanceId = info.InstanceId;
            TeamId = info.TeamId;
            RoleType = info.RoleType;
        }

        public void InitComponent()
        {
            RoleAnimator = MonoRoleRender.GetComponentInChildren<Animator>();
            RoleTransform = MonoRoleRender.transform;
        }

        public void InitData()
        {
            RoleAnimation = new RoleAnimationSys();
            RoleAnimation.SetCurrentRole(this);
            RoleMoveMoment = new MoveMoment();
            RoleMoveMoment.SetCurrentRole(this);
            RolePropertyValue = new ValueProperty();
            RolePropertyValue.SetCurrentRole(this);
            //RoleProperty.InitRoleValue(100f, 100f);
            RolePropertyValue.SetMoveSeed(10f);
            RoleDamageMoment = new DamageMoment();
            RoleDamageMoment.SetCurrentRole(this);
            AttackSkillIdArray = new int[3];
        }

        public virtual void InitFSM()
        {
            RoleActionStateDic = new Dictionary<int, FsmState<RoleBase>>(10);
            RoleActionMachine = new FsmStateMachine<RoleBase>(this);
        }

        public void SetRoleActionState(RoleActionEnum state)
        {
            if (RoleActionStateDic.ContainsKey((int)state))
            {
                RoleActionMachine.ChangeState(RoleActionStateDic[(int)state]);
            }
        }

        public void SetAnimationId(int animationId)
        {
            AnimationId = animationId;
            RoleAnimation.ChangeAniamtionNameById(AnimationId);
        }

        public void SetAttackSkillId(int[] attackSkillId)
        {
            for (int i = 0; i < attackSkillId.Length; i++)
            {
                AttackSkillIdArray[i] = attackSkillId[i];
            }
        }

        public int SetCurrentAttackSkillId(int attackIndex)
        {
            CurrentAttackSkillId = AttackSkillIdArray[attackIndex];
            if (attackIndex++ >= AttackSkillIdArray.Length)
            {
                attackIndex = 0;
            }
            return attackIndex;
        }

        public virtual void UpdateLogic(float deltaTime)
        {
            if (RoleMoveMoment != null) { RoleMoveMoment.Update(deltaTime); }
            if (RoleActionMachine != null) RoleActionMachine.Update(deltaTime);
        }

        public abstract void FixedUpdateLogic(float deltaTime);
        public abstract void Dispose();
    }
}