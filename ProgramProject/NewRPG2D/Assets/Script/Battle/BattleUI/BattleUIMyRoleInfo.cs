using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIMyRoleInfo : MonoBehaviour
    {
        public BattleUIMyRoleInfoItem roleItem;
        public Transform parentRoot;

        private List<BattleUIMyRoleInfoItem> myRoleInfoList;
        private void Awake()
        {
            myRoleInfoList = new List<BattleUIMyRoleInfoItem>();
            Debug.LogError(" BattleUIMyRoleInfo ");
            EventManager.instance.AddListener<MainHeroRole>(EventDefineEnum.CreateRole, OnCreateRole);
            EventManager.instance.AddListener<RoleBase>(EventDefineEnum.HpChange, OnHpChange);
        }

        private void OnDestroy()
        {
            myRoleInfoList.Clear();
            EventManager.instance.RemoveListener<MainHeroRole>(EventDefineEnum.CreateRole, OnCreateRole);
            EventManager.instance.RemoveListener<RoleBase>(EventDefineEnum.HpChange, OnHpChange);
        }

        private void OnCreateRole(MainHeroRole role)
        {
            Debug.LogError("  OnCreateRole " + role.RoleId);
            BattleUIMyRoleInfoItem item = Instantiate(roleItem, parentRoot);
            myRoleInfoList.Add(item);
            item.SetRoleInfo(role.RoleDetailInfo, role.InstanceId);
        }

        private void OnHpChange(RoleBase role)
        {
            for (int i = 0; i < myRoleInfoList.Count; i++)
            {
                if (myRoleInfoList[i].CurrentInstanceId == role.InstanceId)
                {
                    myRoleInfoList[i].SetHpValue(role.RolePropertyValue.RoleHp);
                    break;
                }
            }
        }
    }
}
