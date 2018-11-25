using System;
using System.Collections.Generic;

namespace Assets.Script.Battle
{

    public class BuffMgr
    {
        private Dictionary<BuffTypeEnum, BuffBase> buffDic;
        private RoleBase currentRole;
        private List<BuffTypeEnum> cleanList;

        public void SetCurrentRole(RoleBase role)
        {
            currentRole = role;
            buffDic = new Dictionary<BuffTypeEnum, BuffBase>();
            cleanList = new List<BuffTypeEnum>();
        }

        public void AddBuff(BuffTypeEnum buffType, params Object[] param)
        {
            BuffBase buff = null;
            if (buffDic.TryGetValue(buffType, out buff) == false)
            {
                switch (buffType)
                {
                    case BuffTypeEnum.HealHp:
                        buff = new BuffHealHp();
                        break;
                    case BuffTypeEnum.ExtraDamage:
                        buff = new BuffExtraDamage();
                        break;
                    case BuffTypeEnum.ChangeArmor:
                        buff = new BuffChangeArmor();
                        break;
                    case BuffTypeEnum.ChangeMaxHp:
                        buff = new BuffChangeMaxHp();
                        break;
                    case BuffTypeEnum.Dizzy:
                        buff = new BuffDizzy();
                        break;
                    case BuffTypeEnum.IncreaseDamage:
                        buff = new BuffIncreaseDamage();
                        break;
                    case BuffTypeEnum.ChangeCritial:
                        buff = new BuffChangeCritial();
                        break;
                    case BuffTypeEnum.ReduceArmor:
                        buff = new BuffReduceArmor();
                        break;
                    case BuffTypeEnum.IncreaseAvoid:
                        buff = new BuffIncreaseAvoid();
                        break;
                    case BuffTypeEnum.IncreaseMagicArmor:
                        buff = new BuffIncreaseMagicArmor();
                        break;
                    case BuffTypeEnum.ReduceDamage:
                        buff = new BuffReduceDamage();
                        break;
                }
                buffDic[buffType] = buff;
            }
            else
            {
                buff.RmoveBuff();
            }
            buff.AddBuff(currentRole, buffType, param);
        }

        public void RemoveBuff(BuffTypeEnum buffType)
        {
            buffDic[buffType].RmoveBuff();
            buffDic.Remove(buffType);
        }

        public void RemoveBuff(BuffBase buff)
        {
            buff.RmoveBuff();
            foreach (var buffTemp in buffDic)
            {
                if (buffTemp.Value == buff)
                {
                    buffDic.Remove(buffTemp.Key);
                    return;
                }
            }
        }

        public void Update(float deltaTime)
        {
            cleanList.Clear();
            foreach (var buffItem in buffDic)
            {
                if (buffItem.Value.Update(deltaTime) == false)
                {
                    cleanList.Add(buffItem.Key);
                }
            }

            for (int i = 0; i < cleanList.Count; i++)
            {
                buffDic.Remove(cleanList[i]);
            }
        }
    }
}
