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

        }

        public void RemoveBuff(BuffTypeEnum buffType)
        {
            buffDic.Remove(buffType);
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
