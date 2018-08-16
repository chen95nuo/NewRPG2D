using UnityEngine;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIRoleHpInfo : MonoBehaviour
    {

        public BattleUIRoleHpInfoItem HpItem;


        private void Awake()
        {
            EventManager.instance.AddListener<RoleBase>(EventDefineEnum.CreateRole, OnCreateRole);
        }

        private void OnDestroy()
        {
            EventManager.instance.RemoveListener<RoleBase>(EventDefineEnum.CreateRole, OnCreateRole);
        }

        private void OnCreateRole(RoleBase role)
        {
            BattleUIRoleHpInfoItem item = Instantiate(HpItem, transform);
            item.SetHpItemInfo(role.TeamId== TeamTypeEnum.Hero, role.RolePropertyValue.RoleHp, role.InstanceId, role.RoleTransform);
        }

    }
}
