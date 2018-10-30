using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Utility.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleEndPlayerInfo : MonoBehaviour
    {
        public BattleEndPlayerInfoItem[] items;

        public void SetPlayerInfo(List<RoleBase> roles)
        {
            int rolesCount= roles.Count;
            for (int i = 0; i < rolesCount; i++)
            {
                Sprite icon = SpriteHelper.instance.GetIcon(SpriteAtlasTypeEnum.Icon, roles[i].RoleDetailInfo.IconName);
                items[i].SetItemInfo(roles[i].RoleDetailInfo.Name, icon, roles[i].RolePropertyValue.RoleHp / roles[i].RolePropertyValue.MaxHp);
            }
            for (int i = rolesCount; i < items.Length; i++)
            {
                items[i].gameObject.CustomSetActive(false);
            }
        }
    }
}
