using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using Assets.Script.Battle.BattleData.ReadData;
using Assets.Script.Utility;
using DragonBones;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Script.Battle
{
    public struct RoleInfo
    {
        public int InstanceId;
        public TeamTypeEnum TeamId;
        public int RoleId;
        public RoleTypeEnum RoleType;
    }

    public abstract class RoleBase
    {
        public int InstanceId { get; private set; }
        public int RoleId { get; private set; }
        public UnityArmatureComponent RoleAnimator { get; private set; }
        public TeamTypeEnum TeamId { get; private set; }
        public RoleTypeEnum RoleType { get; private set; }
        public Transform RoleTransform { get; private set; }
        public int AnimationId { get; private set; }
        public SkillSlotTypeEnum CurrentSlot { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsCanControl { get; private set; }

        public SearchTarget RoleSearchTarget { get; private set; }
        public RoleRender MonoRoleRender { get; private set; }
        public RoleAnimationSys RoleAnimation { get; private set; }
        public MoveMoment RoleMoveMoment { get; private set; }
        public ValueProperty RolePropertyValue { get; private set; }
        public DamageMoment RoleDamageMoment { get; private set; }
        public SkillCompoment RoleSkill { get; private set; }
        public FsmStateMachine<RoleBase> RoleActionMachine;
        public Dictionary<int, FsmState<RoleBase>> RoleActionStateDic; //存放动作状态

        protected RoleData CurrentRoleData;
        private int[] AttackSkillIdArray;

        public void SetRoleInfo(RoleInfo info, RoleRender roleRender)
        {
            MonoRoleRender = roleRender;
            MonoRoleRender.SetRoleBaseInfo(this);
            SetRoleInfo(info);
            InitComponent();
            InitFSM();
            InitData();
            InitRoleComponentData();
        }

        public virtual void SetRoleInfo(RoleInfo info)
        {
            InstanceId = info.InstanceId;
            TeamId = info.TeamId;
            RoleType = info.RoleType;
            RoleId = info.RoleId;
            CurrentRoleData = RoleDataMgr.instance.GetXmlDataByItemId<RoleData>(RoleId);
        }

        public virtual void InitComponent()
        {
            RoleAnimator = MonoRoleRender.roleAnimation;
            RoleTransform = MonoRoleRender.transform;
        }

        public virtual void InitData()
        {
            RoleAnimation = new RoleAnimationSys();
            RoleAnimation.SetCurrentRole(this);
            RoleMoveMoment = new MoveMoment();
            RoleMoveMoment.SetCurrentRole(this);
            RolePropertyValue = new ValueProperty();
            RolePropertyValue.SetCurrentRole(this);
            InitRoleProperty();
            RolePropertyValue.SetMoveSeed(1f);
            RoleDamageMoment = new DamageMoment();
            RoleDamageMoment.SetCurrentRole(this);
            RoleSkill = new SkillCompoment();
            RoleSkill.SetCurrentRole(this);
            RoleSearchTarget = new SearchTarget();
            RoleSearchTarget.SetCurrentRole(this);
          

            AttackSkillIdArray = new int[3];
        }

        public virtual void InitFSM()
        {
            RoleActionStateDic = new Dictionary<int, FsmState<RoleBase>>(10);
            RoleActionMachine = new FsmStateMachine<RoleBase>(this);
        }

        private void InitSkill()
        {
            RoleSkill.InitSkill(SkillSlotTypeEnum.NormalAttack, CurrentRoleData.NormalAttackId);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill1, CurrentRoleData.SkillDataId01);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill2, CurrentRoleData.SkillDataId02);
            CurrentSlot = SkillSlotTypeEnum.NormalAttack;
        }

        private void InitRoleComponentData()
        {
            InitSkill();
            RoleSearchTarget.InitData();  
        }

        private void InitRoleProperty()
        {
            RolePropertyData data = RolePropertyDataMgr.instance.GetXmlDataByItemId<RolePropertyData>(CurrentRoleData.PropertyId);
            RolePropertyValue.InitRoleValue(data.RoleBaseData);
        }

        public void SetRoleActionState(ActorStateEnum state)
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

        public virtual void UpdateLogic(float deltaTime)
        {
            if (RoleMoveMoment != null) { RoleMoveMoment.Update(deltaTime); }
            if (RoleActionMachine != null) RoleActionMachine.Update(deltaTime);
            if (RoleSearchTarget != null) RoleSearchTarget.Update();

        }

        public abstract void FixedUpdateLogic(float deltaTime);
        public abstract void Dispose();
    }
}