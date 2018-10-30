using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIRoleHpInfo : MonoBehaviour
    {

        public BattleUIRoleHpInfoItem HpItem;

        List<RoleBase> HpInfoItemList=new List<RoleBase>();
        private void Awake()
        {
            EventManager.instance.AddListener<RoleBase>(EventDefineEnum.CreateRole, OnCreateRole);
            EventManager.instance.AddListener<HpChangeParam>(EventDefineEnum.HpChange, OnHpChange);
        }

        private void OnDestroy()
        {
            EventManager.instance.RemoveListener<RoleBase>(EventDefineEnum.CreateRole, OnCreateRole);
            EventManager.instance.RemoveListener<HpChangeParam>(EventDefineEnum.HpChange, OnHpChange);
        }

        private void OnHpChange(HpChangeParam param)
        {
            if (HpInfoItemList.Contains(param.role) == false)
            {
                HpInfoItemList.Add(param.role);
                BattleUIRoleHpInfoItem item = Instantiate(HpItem, transform);
                item.SetHpItemInfo(param.role.TeamId == TeamTypeEnum.Hero, param.role.RolePropertyValue.RoleHp,
                    param.role.InstanceId, param.role.RoleTransform);
            }
        }

        private void OnCreateRole(RoleBase role)
        {
          
        }

    }
}
