using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipInfoView : MonoBehaviour
{

    public Text txt_Name;
    public Text txt_Quality;
    public Text txt_MainNum;
    public Text txt_MainTip;
    public Text txt_OtherNum_1;
    public Text txt_OtherTip_1;
    public Text txt_OtherNum_2;
    public Text txt_OtherTip_2;
    public Text txt_WorkTip;
    public Text txt_NeedLevel;
    public Text txt_Tip_1;

    public Image image_EquipIcon;
    public Image image_EquipQuality;

    private void Awake()
    {
        txt_Tip_1.text = "需求:";
    }

    private void UpdateInfo(EquipmentRealProperty equipData)
    {
        txt_Name.text = equipData.EquipName;
        txt_Quality.text = equipData.QualityType.ToString();
        txt_MainNum.text = equipData.RoleProperty[RoleAttribute.DPS].ToString();
        txt_MainTip.text = equipData.HurtType + "伤害/秒";
        txt_OtherNum_1.text = "equipData.RoleProperty[RoleAttribute]";
        txt_OtherTip_1.text = "点伤害";
        txt_OtherNum_2.text = "攻速:";
        txt_OtherTip_2.text = equipData.AttackSpeed.ToString();

        txt_WorkTip.text = "职业类型: " + equipData.WeaponProfession.ToString();
        txt_NeedLevel.text = equipData.Level.ToString();

        image_EquipIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.ProfessionNeed.ToString());

    }
}
