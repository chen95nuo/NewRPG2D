using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using Assets.Script.Battle.BattleData;

public class UILessonItemGrid : MonoBehaviour
{
    public Image Icon;
    public Image BG;
    public Text txt_Num;

    public void UpdateInfo(AwardItemData data)
    {
        PropData propData = PropDataMgr.instance.GetXmlDataByItemId<PropData>(data.ItemId);
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(propData.SpriteName);
        if (data.ItemMinCount == data.ItemMaxCount && data.ItemMaxCount > 0)
        {
            BG.gameObject.SetActive(true);
            txt_Num.text = data.ItemMinCount.ToString("#0") + "~" + data.ItemMaxCount.ToString("#0");
        }
        else if (data.ItemMaxCount > 0)
        {
            BG.gameObject.SetActive(true);
            txt_Num.text = data.ItemMaxCount.ToString("#0");
        }
        else
        {
            BG.gameObject.SetActive(false);
        }
    }

    public void UpdateInfo(TreasureBox data)
    {
        BG.gameObject.SetActive(false);
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.Icon);
    }


}
