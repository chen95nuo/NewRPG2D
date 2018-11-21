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

        protected float magicValue;

        protected RoleBase currentRole;

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

        protected RoleBase FindLoseMaxHpRole()
        {
            float loseMaxHp = 0;
            int roleIndex = -1;

            for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
            {
                RoleBase tempRole = GameRoleMgr.instance.RolesList[i];
                if (tempRole.TeamId == currentRole.TeamId)
                {
                    float loseHp = tempRole.RolePropertyValue.LoseHp;
                    if (loseHp > loseMaxHp)
                    {
                        roleIndex = i;
                        loseMaxHp = loseHp;
                    }
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
