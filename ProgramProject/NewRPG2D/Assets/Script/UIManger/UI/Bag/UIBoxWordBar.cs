using Assets.Script.Battle.BattleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoxWordBar : MonoBehaviour
{
    public Image image_Icon;
    public Text txt_Tip;

    public void UpdateInfo(string IconName, string tip)
    {
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(IconName);
        image_Icon.sprite = sp;
        txt_Tip.text = tip;
    }

    public void UpdateInfo(TreasureBoxItemData data)
    {
        PropData propData = PropDataMgr.instance.GetDataByItemId<PropData>(data.ItemId);
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(propData.SpriteName);
        image_Icon.sprite = sp;
        string tip = UpdateStart(data.ItemMinCount, data.ItemMaxCount, propData.quality, propData.des);
        txt_Tip.text = tip;
    }
    public void UpdateInfo(RandomTreasureBoxItemData data)
    {
        PropData propData = PropDataMgr.instance.GetDataByItemId<PropData>(data.ItemData.ItemId);
        UpdateStart(data.ItemData.ItemMinCount, data.ItemData.ItemMaxCount, propData.quality, propData.des);
    }

    public string UpdateStart(float min, float max, QualityTypeEnum quality, string name)
    {
        string tip = "";
        if (min == max)
        {
            string qualitySt = "<color=#{0}>{1}</color> x{2}";
            switch (quality)
            {
                case QualityTypeEnum.White:
                    qualitySt = string.Format(qualitySt, "bbbbbb", name, min);
                    break;
                case QualityTypeEnum.Green:
                    qualitySt = string.Format(qualitySt, "77ff58", name, min);
                    break;
                case QualityTypeEnum.Blue:
                    qualitySt = string.Format(qualitySt, "50b2f0", name, min);
                    break;
                case QualityTypeEnum.Purple:
                    qualitySt = string.Format(qualitySt, "ba27ff", name, min);
                    break;
                case QualityTypeEnum.Orange:
                    qualitySt = string.Format(qualitySt, "ff8d27", name, min);
                    break;
                default:
                    break;
            }
            return qualitySt;
        }
        else
        {
            tip = min.ToString("#0") + "-" + max.ToString("#0");
        }
        return tip;
    }
}
