﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Assets.Script.Battle
{
    public class ValueProperty
    {
        public RolePropertyEnum RoleProperty;  //自己的属性
        public RolePropertyEnum AttackProperty; //克制的属性
        public RolePropertyEnum DefenseProperty; // 被克制的属性
        public float RoleHp { get; private set; }
        public float RoleMp { get; private set; }
        public float PhysicAttack { get; private set; }
        public float MagicAttack { get; private set; }
        public float MagicArmor { get; private set; }
        public float PhysicArmor { get; private set; }   
        public float CriticalPercent { get; private set; }  //暴击
        public float AviodHurtPercent { get; private set; } //闪避
        public float HitPercent { get; private set; }       //命中 

        public float MoveSpeed { get; private set; }
        public float AttackSpeed { get; private set; }
        public RoleBase AttackRole { get; private set; }

        private RoleBase currenRole;
        private bl_HUDText HUDRoot;
        private HUDTextInfo HUDTextInfoinfo;

        public void SetCurrentRole(RoleBase mRole)
        {
            currenRole = mRole;
          //  HUDRoot = bl_UHTUtils.GetHUDText;
            HUDTextInfoinfo = new HUDTextInfo(currenRole.RoleTransform,"");
            HUDTextInfoinfo.Color = Color.white;
            HUDTextInfoinfo.Size = 150;
            HUDTextInfoinfo.Speed = 10;
            HUDTextInfoinfo.VerticalAceleration = -2;
            HUDTextInfoinfo.VerticalFactorScale = 0.1f;
            HUDTextInfoinfo.VerticalPositionOffset = 0;
            HUDTextInfoinfo.Side = mRole.TeamId == TeamTypeEnum.Hero ? bl_Guidance.RightDown : bl_Guidance.LeftDown;
        }

        public void InitBaseRoleValue(PropertyData rolePropertyData)
        {
            RoleHp = rolePropertyData.RoleHp;
            PhysicAttack = rolePropertyData.PhysicAttack;
            MagicAttack = rolePropertyData.MagicAttack;
            MagicArmor = rolePropertyData.MagicArmor;
            PhysicArmor = rolePropertyData.PhysicArmor;
            CriticalPercent = rolePropertyData.CriticalPercent;
            AviodHurtPercent = rolePropertyData.AviodHurtPercent;
            HitPercent = rolePropertyData.HitPercent;
            MoveSpeed = 1;
            AttackSpeed = StaticAndConstParamter.AttackSpeed;
        }

        public void SetMoveSeed(float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }

        public bool SetHp(float HpChange, RoleBase attackRole)
        {
            AttackRole = attackRole;
            return SetHp(HpChange);
        }

        public bool SetHp(float HpChange)
        {
            if (BattleStaticAndEnum.isGod == false)
            {
                RoleHp -= HpChange;
            }
            EventManager.instance.SendEvent(EventDefineEnum.HpChange, currenRole);
            //HUDTextInfoinfo.Text = ((int)HpChange).ToString();
            //HUDRoot.NewText(HUDTextInfoinfo);
           // DebugHelper.Log("name=  " + currenRole.RoleTransform.name + " hp " + RoleHp);
            return RoleHp > 0;
        }

        public bool SetMp(float MpChange)
        {
            RoleMp -= MpChange;
            return RoleMp > 0;
        }

        public void SetPhysicAttack(float attackChange)
        {
            PhysicAttack += attackChange;
        }

        public void SetMagicAttack(float attackChange)
        {
            MagicAttack += attackChange;
        }

        public void SetPhysicArmor(float defenseChange)
        {
            PhysicArmor += defenseChange;
        }

        public void SetMagicArmor(float defenseChange)
        {
            MagicArmor += defenseChange;
        }

        public void SetCriticalPercent(float criticalPercentChange)
        {
            CriticalPercent += criticalPercentChange;
        }

        public void SeAviodHurtPercent(float aviodHurtPercentChange)
        {
            AviodHurtPercent += aviodHurtPercentChange;
        }

        public void SetHitPercent(float hitPercentChange)
        {
            HitPercent += hitPercentChange;
        }
    }
}
