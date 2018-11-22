using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using Assets.Script.Utility;
using Assets.Script.Utility.Tools;
using UnityEngine;

public class CaptureScreen : MonoBehaviour
{

    [SerializeField] private ChangeRoleEquip manEquip, womanEquip;
    [SerializeField] private Camera mCamera;

    private class IconData
    {
        public int roleId;
        public SexTypeEnum sexType;
        public int equipId;

        public IconData(int roleId, SexTypeEnum sexType, int equipId)
        {
            this.roleId = roleId;
            this.sexType = sexType;
            this.equipId = equipId;
        }

    }

    private EquipmentRealProperty equipment;
    private Dictionary<int, Sprite> roleIcon = new Dictionary<int, Sprite>();
    private Queue<IconData> iconDatas = new Queue<IconData>();
    private IconData tempIconData = null;

    private void Update()
    {
        if (tempIconData != null)
        {
            StartCoroutine(DelayCaptureIcon(tempIconData.roleId, tempIconData.sexType, tempIconData.equipId));
            tempIconData = null;
        }
    }

    /// <summary>
    /// 获取人物头像
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="sexType">性别</param>
    /// <param name="equipId">装备id</param>
    /// <param name="bForce">是否强制更新头像</param>
    /// <returns></returns>

    public Sprite CaptureScreenToIcon(int roleId, SexTypeEnum sexType, int equipId, bool bForce = false)
    {
        Sprite icon = null;
        if (bForce)
        {
            iconDatas.Enqueue(new IconData(roleId, sexType, equipId));
            if (tempIconData == null)
            {
                tempIconData = iconDatas.Dequeue();
            }
            return icon;
        }
        else
        {
            if (roleIcon.TryGetValue(roleId, out icon) == false)
            {
                Texture2D texture = null;
                if (CaptureScreenMgr.instance.IconDic.TryGetValue(roleId, out texture) == false)
                {
                    iconDatas.Enqueue(new IconData(roleId, sexType, equipId));
                    if (tempIconData == null)
                    {
                        tempIconData = iconDatas.Dequeue();
                    }
                    return null;
                }
                icon = SpriteHelper.CreateSprite(texture);
                roleIcon[roleId] = icon;
            }
            return icon;
        }
    }

    public IEnumerator DelayCaptureIcon(int roleId, SexTypeEnum sexType, int equipId)
    {
        manEquip.gameObject.SetActive(sexType == SexTypeEnum.Man);
        womanEquip.gameObject.SetActive(sexType == SexTypeEnum.Woman);
        equipment = EquipmentMgr.instance.GetEquipmentByEquipId(equipId);
        if (sexType == SexTypeEnum.Man)
        {
            if (equipment == null)
            {
                manEquip.ChangeOriginalEquip(EquipTypeEnum.Armor);
            }
            else
            {
                manEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
            }
        }
        else
        {
            if (equipment == null)
            {
                womanEquip.ChangeOriginalEquip(EquipTypeEnum.Armor);
            }
            else
            {
                womanEquip.ChangeEquip(equipment.EquipType, equipment.EquipName, SexTypeEnum.Woman);
            }
        }
        yield return null;
        Texture2D texture =  CaptureScreenMgr.instance.CaptureCamera(mCamera, new Rect(Vector2.zero, new Vector2(121, 140)), roleId);
        Sprite icon = icon = SpriteHelper.CreateSprite(texture);
        roleIcon[roleId] = icon;
        tempIconData = iconDatas.Dequeue();
    }
}
