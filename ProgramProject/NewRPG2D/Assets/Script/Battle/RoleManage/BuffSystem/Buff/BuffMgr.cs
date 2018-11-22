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
                }
                buffDic[buffType] = buff;
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
