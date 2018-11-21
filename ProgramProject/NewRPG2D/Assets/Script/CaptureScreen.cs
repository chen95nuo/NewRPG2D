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

    private EquipmentRealProperty equipment;

    /// <summary>
    /// 获取人物头像
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="sexType">性别</param>
    /// <param name="equipId">装备id</param>
    /// <param name="bForce">是否强制更新头像</param>
    /// <returns></returns>
    public Texture2D CaptureScreenToTexture2D(int roleId, SexTypeEnum sexType, int equipId, bool bForce = false)
    {
        Texture2D texture = null;
        if (CaptureScreenMgr.instance.IconDic.TryGetValue(roleId, out texture) == false || bForce)
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
                    womanEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
                }
            }
            CaptureScreenMgr.instance.CaptureCamera(mCamera, new Rect(Vector2.zero, new Vector2(121, 140)), roleId);
        }


        return texture;
    }

    public Sprite CaptureScreenToIcon(int roleId, SexTypeEnum sexType, int equipId, bool bForce = false)
    {
        Texture2D texture = CaptureScreenToTexture2D(roleId, sexType, equipId, bForce);

        return SpriteHelper.CreateSprite(texture);
    }
}
