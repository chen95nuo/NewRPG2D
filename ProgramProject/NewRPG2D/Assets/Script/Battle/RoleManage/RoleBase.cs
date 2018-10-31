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
        public RoleDetailData RoleDetailInfo { get; private set; }
        public WeaponProfessionEnum CurrentProfession { get; private set; }

        public bool FinishMoveToPoint;
        public bool IsCanControl;
        public bool IsCanInterrput;
        public bool CanStartMove;

        public WeaponMoment RoleWeapon { get; private set; }
        public SearchTarget RoleSearchTarget { get; private set; }
        public RoleRender MonoRoleRender { get; private set; }
        public RoleAnimationSys RoleAnimation { get; private set; }
        public MoveMoment RoleMoveMoment { get; private set; }
        public ValueProperty RolePropertyValue { get; private set; }
        public DamageMoment RoleDamageMoment { get; private set; }
        public SkillCompoment RoleSkill { get; private set; }
        public FsmStateMachine<RoleBase> RoleActionMachine;
        public Dictionary<int, FsmState<RoleBase>> RoleActionStateDic; //存放动作状态

      //  protected RoleData CurrentRoleData;
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
           // CurrentRoleData = RoleDataMgr.instance.GetXmlDataByItemId<RoleData>(RoleId);
           // AttackType = RoleWeapon.;
        }

        public virtual void InitComponent()
        {
            RoleAnimator = MonoRoleRender.roleAnimation;
            RoleTransform = MonoRoleRender.transform;
            RoleNewAnimator = MonoRoleRender.roleNewAnimation;
        }

        public virtual void InitData()
        {
            RoleWeapon = new WeaponMoment();
            RoleWeapon.SetCurrentRole(this);
            RoleAnimation = new RoleAnimationSys();
            RoleAnimation.SetCurrentRole(this);
            RoleMoveMoment = new MoveMoment();
            RoleMoveMoment.SetCurrentRole(this, MonoRoleRender.MoveController, MonoRoleRender.MoveSeeker);
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
            RoleActionStateDic.Add((int)ActorStateEnum.Win, new RoleWinState());
        }

        public void InitSkill(int normalAttackId, int skillDataId01, int skillDataId02)
        {
            RoleSkill.InitSkill(SkillSlotTypeEnum.NormalAttack, normalAttackId);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill1, skillDataId01);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill2, skillDataId02);
            CurrentSlot = SkillSlotTypeEnum.NormalAttack;
         
        }

        private void InitRoleComponentData()
        {
            FinishMoveToPoint = true;
            IsCanControl = true;
            IsDead = false;
            IsCanInterrput = true;
            CanStartMove = false;
        }

        private void InitRoleProperty()
        {
            //RolePropertyData data = RolePropertyDataMgr.instance.GetXmlDataByItemId<RolePropertyData>(CurrentRoleData.PropertyId);
          //  RolePropertyValue.InitRoleOtherValue(data.RoleOtherData);
        }

        public void InitRoleBaseProperty(RolePropertyData currentRoleData, RoleDetailData detailInfo)
        {
            RolePropertyValue.InitBaseRoleValue(GetPropertyBaseData(currentRoleData));
            RoleDetailInfo = detailInfo;
            if (detailInfo != null)
            {
                for (int i = 0; i < detailInfo.EquipIdList.Length; i++)
                {
                    RoleWeapon.SetEquipSlot(detailInfo.EquipIdList[i], detailInfo.sexType);
                }
                CurrentProfession = detailInfo.Profession;
            }
            else
            {
                RoleDetailInfo = new RoleDetailData();
                RoleDetailInfo.IconName = currentRoleData.SpriteName;
                RoleDetailInfo.Name = currentRoleData.ItemName;
                for (int i = 0; i < currentRoleData.SpecialPropertyDatas.Length; i++)
                {
                    RoleWeapon.AddSpecialBuffs(currentRoleData.SpecialPropertyDatas[i]);
                }
                CurrentProfession = currentRoleData.Profession;
            }
            RoleSearchTarget.InitData();
            RoleMoveMoment.InitData();
        }

        public bool SetRoleActionState(ActorStateEnum state)
        {
            if (CurrentActorState == state || IsCanControl == false || IsCanInterrput == false)
            {
                return false;
            }
            LastActorState = CurrentActorState;
            CurrentActorState = state;
            if (RoleActionStateDic.ContainsKey((int)state))
            {
                RoleActionMachine.ChangeState(RoleActionStateDic[(int)state]);
            }
            return true;
        }

        public void RestartRoleLastActorState()
        {
           // DebugHelper.Log(" LastActorState   " + LastActorState);
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
                if (RoleMoveMoment != null && CurrentActorState == ActorStateEnum.Run)
                {
                    RoleMoveMoment.Update(deltaTime);
                }
                if (RoleSearchTarget != null && CanStartMove)
                {
                    RoleSearchTarget.Update();
                }
                if(RoleSkill != null) RoleSkill.UpdateLogic(deltaTime);
            }

            if (RoleActionMachine != null) RoleActionMachine.Update(deltaTime);

            if (RoleWeapon != null)
            {
                RoleWeapon.UpdateLogic(deltaTime);
            }
        }

        public virtual void Death()
        { 
            IsDead = true;
            HurtInfo info = default(HurtInfo);
            RoleWeapon.TriggerBuff(TirggerTypeEnum.Death, ref info);
        }

        public virtual void Reborn()
        {
            IsDead = false;
            HurtInfo info = default(HurtInfo);
            RoleWeapon.TriggerBuff(TirggerTypeEnum.Reborn, ref info);
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

        private PropertyData GetPropertyBaseData(RolePropertyData data)
        {
            if (data == null)
            {
                DebugHelper.LogError("dont find role info");
                return default(PropertyData);
            }
            PropertyData propertyBaseData = new PropertyData();
            propertyBaseData.HurtType = data.HurtType;
            propertyBaseData.AttackSpeed = data.AttackSpeed;
            propertyBaseData.AviodHurtPercent = data.AvoidHurt;
            propertyBaseData.CriticalPercent = data.Critial;
            propertyBaseData.MaxDamage = data.DamageMax;
            propertyBaseData.MinDamage = data.DamageMin;
            propertyBaseData.HitPercent = data.HitEnemy;
            propertyBaseData.MagicArmor = data.MagicArmor;
            propertyBaseData.MagicAttack = data.MagicDamage;
            propertyBaseData.PhysicArmor = data.PhysicArmor;
            propertyBaseData.ProfessionNeed = data.Profession;
            propertyBaseData.RoleHp = data.HP;
            return propertyBaseData;
        }
    }
}