using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class RoleEquipSpecialBuff
    {
        public virtual TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Always;
            }
        }

        protected float RandomValue
        {
            get { return Random.Range(0, 1f); }
        }

        private float magicValue;
        protected float MagicValue 
        {
            get { return magicValue; }
        }
        protected HurtInfo mHurtInfo = default(HurtInfo);
        protected RoleBase currentRole;

        protected RoleBase Target
        {
            get
            {
                RoleBase enemy = currentRole.RoleSearchTarget.Target;
                if (enemy == null)
                {
                    enemy = FindRandomTarget(EnemysRoleList);
                }
                return enemy;
            }
        }

        private List<RoleBase> friendsRoleList;
        protected List<RoleBase> FriendsRoleList
        {
            get
            {
                if (friendsRoleList == null)
                {
                    friendsRoleList = new List<RoleBase>();
                    for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
                    {
                        RoleBase tempRole = GameRoleMgr.instance.RolesList[i];
                        if (tempRole.TeamId == currentRole.TeamId)
                        {
                            friendsRoleList.Add(tempRole);
                        }
                    }
                }
                return friendsRoleList;
            }
        }

        private List<RoleBase> enemysRoleList;
        protected List<RoleBase> EnemysRoleList
        {
            get
            {
                if (enemysRoleList == null)
                {
                    enemysRoleList = new List<RoleBase>();
                    for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
                    {
                        RoleBase tempRole = GameRoleMgr.instance.RolesList[i];
                        if (tempRole.TeamId != currentRole.TeamId)
                        {
                            enemysRoleList.Add(tempRole);
                        }
                    }
                }
                return enemysRoleList;
            }
        }

        public virtual void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            currentRole = role;
            magicValue = currentRole.RolePropertyValue.MagicAttack;
        }

        public virtual bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (triggerType == TirggerType)
            {
                return true;
            }
            return false;
        }

        public virtual void UpdateLogic(float deltaTime)
        {

        }

        public virtual void Dispose()
        {

        }

        protected void HurtEnemy(float damage)
        {
            HurtEnemy(Target, damage);
        }

        protected void HurtEnemy(RoleBase target, float damage)
        {
            mHurtInfo.TargeRole = target;
            mHurtInfo.AttackRole = currentRole;
            mHurtInfo.HurtType = currentRole.RolePropertyValue.HurtType;
            mHurtInfo.HurtValue = damage;
            currentRole.RoleDamageMoment.HurtDamage(ref mHurtInfo);
        }

        protected void HurtAllEnemy(float damage)
        {
            HurtEnemy(damage);
            List<RoleBase> enemys = FindEnemys(Target);
            if (enemys != null)
            {
                for (int i = 0; i < enemys.Count; i++)
                {
                    HurtEnemy(enemys[i], damage);
                }
            }
        }

        protected void HurtAllEnemyBuff(float damage, float buffDuration)
        {
            if (Target != null)
            {
                Target.BuffMoment.AddBuff(BuffTypeEnum.ExtraDamage, damage);
            }
            List<RoleBase> enemys = FindEnemys(Target);
            if (enemys != null)
            {
                for (int i = 0; i < enemys.Count; i++)
                {
                    enemys[i].BuffMoment.AddBuff(BuffTypeEnum.ExtraDamage, buffDuration, damage);
                }
            }
        }

        protected void DizzyAllEnemy(float duration)
        {
            for (int i = 0; i < EnemysRoleList.Count; i++)
            {
                RoleBase role = EnemysRoleList[i];
                if (role.IsDead == false)
                {
                    role.BuffMoment.AddBuff(BuffTypeEnum.Dizzy, duration);
                }
            }
        }


        protected List<RoleBase> FindEnemys(RoleBase target)
        {
            if (target == null)
            {
                return null;
            }
            List<RoleBase> enemys = new List<RoleBase>();
            for (int i = 0; i < EnemysRoleList.Count; i++)
            {
                if (EnemysRoleList[i].InstanceId == target.InstanceId)
                {
                    continue;
                }

                if(EnemysRoleList[i].IsDead)
                {
                    continue;
                }

                if (Vector3.Distance(target.RoleTransform.position, EnemysRoleList[i].RoleTransform.position) < 10)
                {
                    enemys.Add(EnemysRoleList[i]);
                }
            }

            return enemys;
        }

        protected RoleBase FindLoseMaxHpRole()
        {
            float loseMaxHp = 0;
            int roleIndex = -1;

            for (int i = 0; i < FriendsRoleList.Count; i++)
            {

                if(FriendsRoleList[i].IsDead)
                {
                    continue;
                }

                float loseHp = FriendsRoleList[i].RolePropertyValue.LoseHp;
                if (loseHp > loseMaxHp)
                {
                    roleIndex = i;
                    loseMaxHp = loseHp;
                }
            }
            if (loseMaxHp > 0)
            {
                return GameRoleMgr.instance.RolesList[roleIndex]; ;
            }
            return null;
        }

        protected bool RebornRole(float healHp)
        {
            for (int i = 0; i < FriendsRoleList.Count; i++)
            {
                RoleBase role = FriendsRoleList[i];
                if (role.IsDead)
                {
                    role.Reborn();
                    role.RolePropertyValue.SetHp(healHp);
                    return true;
                }
            }
            return false;
        }

        protected RoleBase FindRandomTarget(List<RoleBase> roseList)
        {
            RoleBase target = null;
            int i = 0;
            do
            {
                i++;
                target = roseList[Random.Range(0, roseList.Count)];

            } while (target.IsDead && i< roseList.Count * 2);

            return target;
        }
    }
}
