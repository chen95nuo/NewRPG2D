using UnityEngine;
using System.Collections;

public class SpriteForBenefits : MonoBehaviour
{

    public UISprite spriteBenefits;
	public UISprite picLevel;
    public UILabel lblNum;
    public string benefitsValue;
    public BenefitsType benefitsType;
	public string itemID;
	
	public GameObject invMaker;

    private bool canClickIcon = false;
	
	/// <summary>
	/// 刷新物品图标
	/// </summary>
	public void RefreshIcon(string _itemID)
	{
        if (string.IsNullOrEmpty(_itemID))
        {
            return;
        }

		this.itemID = _itemID;
		lblNum.text=_itemID.Split (',')[1];

        //--------------任务活动用到这部分-------------
        if (_itemID.Split(',')[0].Equals("1"))// 金币
        {
            spriteBenefits.spriteName = "Gold";
            canClickIcon = false;
            return;
        }
        else if (_itemID.Split(',')[0].Equals("2"))// 荣誉
        {
            spriteBenefits.spriteName = "UIM_Glory_big";
            canClickIcon = false;
            return;
        }
        else if (_itemID.Split(',')[0].Equals("3"))// 英雄徽章
        {
            spriteBenefits.spriteName = "YXhuizhang_big";
            canClickIcon = false;
            return;
        }
        else if (_itemID.Split(',')[0].Equals("4"))// 声望
        {
            spriteBenefits.spriteName = "UIM_Imperial_Crown_big";
            canClickIcon = false;
            return;
        }
        //---------------任务活动用到这部分------------

		object[] parms=new object[2];
        parms[0] = itemID + ",01";
        parms[1]=spriteBenefits;
        PanelStatic.StaticBtnGameManager.InvMake.SendMessage("SpriteName", parms, SendMessageOptions.DontRequireReceiver);
        canClickIcon = true;
	}

    void OnClick()
    {
        if (!canClickIcon) return;

		if(itemID!="")
		{
	        PanelStatic.StaticIteminfo.SetActiveRecursively(true);
	        PanelStatic.StaticIteminfo.transform.position = transform.position;
	        PanelStatic.StaticIteminfo.SendMessage("SetItemID", itemID, SendMessageOptions.DontRequireReceiver);
		}
    }


}