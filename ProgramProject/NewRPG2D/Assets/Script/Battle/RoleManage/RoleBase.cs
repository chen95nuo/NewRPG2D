using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using Assets.Script.Battle.BattleData.ReadData;
using Assets.Script.Battle.RoleState;
using Assets.Script.Utility;
using Spine.Unity;
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
        public SkeletonAnimation RoleAnimator { get; private set; }
        public Animation RoleNewAnimator { get; private set; }
        public TeamTypeEnum TeamId { get; private set; }
        public RoleTypeEnum RoleType { get; private set; }
        public Transform RoleTransform { get; private set; }
        public int AnimationId { get; private set; }
        public SkillSlotTypeEnum CurrentSlot { get; private set; }
        public ActorStateEnum LastActorState { get; private set; }
        public ActorStateEnum CurrentActorState { get; private set; }
        public bool IsDead { get; private set; }
        public AttackTypeEnum AttackType { get; private set; }
        public CardData RoleDetailInfo { get; private set; }

        public bool FinishMoveToPoint;
        public bool IsCanControl;
        public bool IsCanInterrput;

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
            InitData();
            InitFSM();
            InitRoleComponentData();
        }

        public virtual void SetRoleInfo(RoleInfo info)
        {
            InstanceId = info.InstanceId;
            TeamId = info.TeamId;
            RoleType = info.RoleType;
            RoleId = info.RoleId;
            CurrentRoleData = RoleDataMgr.instance.GetXmlDataByItemId<RoleData>(RoleId);
            AttackType = CurrentRoleData.AttackType;
        }

        public virtual void InitComponent()
        {
            RoleAnimator = MonoRoleRender.roleAnimation;
            RoleTransform = MonoRoleRender.transform;
            RoleNewAnimator = MonoRoleRender.roleNewAnimation;
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
            RoleActionStateDic.Add((int)ActorStateEnum.Death, new RoleDeathState());
        }

        public void InitSkill(int normalAttackId, int skillDataId01, int skillDataId02)
        {
            RoleSkill.InitSkill(SkillSlotTypeEnum.NormalAttack, normalAttackId);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill1, skillDataId01);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill2, skillDataId02);
            CurrentSlot = SkillSlotTypeEnum.NormalAttack;
            RoleSearchTarget.InitData();
        }

        private void InitRoleComponentData()
        {
            FinishMoveToPoint = true;
            IsCanControl = true;
            IsDead = false;
            IsCanInterrput = true;
        }

        private void InitRoleProperty()
        {
            RolePropertyData data = RolePropertyDataMgr.instance.GetXmlDataByItemId<RolePropertyData>(CurrentRoleData.PropertyId);
            RolePropertyValue.InitRoleOtherValue(data.RoleOtherData);
        }

        public void InitRoleBaseProperty(PropertyBaseData data, CardData detailInfo)
        {
            RolePropertyValue.InitBaseRoleValue(data);
            RoleDetailInfo = detailInfo;
        }

        public void SetRoleActionState(ActorStateEnum state)
        {
            if (CurrentActorState == state || IsCanControl == false || IsCanInterrput == false)
            {
                return;
            }
            LastActorState = CurrentActorState;
            CurrentActorState = state;
            if (RoleActionStateDic.ContainsKey((int)state))
            {
                RoleActionMachine.ChangeState(RoleActionStateDic[(int)state]);
            }
        }

        public void RestartRoleLastActorState()
        {
            DebugHelper.Log(" LastActorState   " + LastActorState);
            SetRoleActionState(LastActorState);
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
            if (IsDead)
            {
                return;
            }

            if (IsCanControl)
            {
                if (RoleMoveMoment != null){RoleMoveMoment.Update(deltaTime);}
                if (RoleSearchTarget != null) RoleSearchTarget.Update();
                if(RoleSkill != null) RoleSkill.UpdateLogic(deltaTime);
            }

            if (RoleActionMachine != null) RoleActionMachine.Update(deltaTime);
        }

        public virtual void Death()
        { 
            IsDead = true;
        }

        public void ChangeRoleColor(Color c)
        {
            MonoRoleRender.ChangeColor(c);
        }

        public abstract void FixedUpdateLogic(float deltaTime);
        public abstract void Dispose();


        public override string ToString()
        {
            return RoleTransform.name;
        }
    }
}