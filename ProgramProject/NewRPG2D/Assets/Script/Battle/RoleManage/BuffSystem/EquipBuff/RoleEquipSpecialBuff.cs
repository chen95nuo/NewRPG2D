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
        protected float MagicValue {
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
                    enemy = EnemysRoleList[Random.Range(0, EnemysRoleList.Count)];
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

        public virtual bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (tirggerType == TirggerType)
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
    }
}
