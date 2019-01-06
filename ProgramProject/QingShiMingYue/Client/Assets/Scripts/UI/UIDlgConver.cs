using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgConver : UIModule
{
	//list
	public UIScrollList scrollList;
	public GameObjectPool rewardPool;

	//标题
	public SpriteText titleLable;

	//提示内容
	public SpriteText messageLable;
	public SpriteText converLable;

	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	//private string strConver;
	//private string strGetway;

	//控制每行有多少个Icon
	private const int C_COLUMN_COUNT = 3;

	//开始的时候运行
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.costAndRewardAndSync = userDatas[0] as KodGames.ClientClass.CostAndRewardAndSync;
		//this.strConver = userDatas[1] as string;
		//this.strGetway = userDatas[2] as string;

		//标题
		titleLable.Text = GameUtility.GetUIString("UIPnlConver_Conver_Title");
		//兑换码显示
		messageLable.Text = string.Format("{0}{1}{2}", GameUtility.FormatUIString("UIPnlConver_Message_Label1", GameDefines.textColorBtnYellow.ToString()),
														 GameUtility.FormatUIString("UIPnlConver_Message_Label2", GameDefines.textColorWhite.ToString(), (userDatas[1] as string)),
														 GameUtility.FormatUIString("UIPnlConver_Message_Label3", GameDefines.textColorBtnYellow.ToString()));

		//兑换码的获得途径
		converLable.Hide(true);
		if ((userDatas[2] as string).Length > 0)
		{
			converLable.Hide(false);
			converLable.Text = GameUtility.FormatUIString("UIPnlConver_Message_Label4", (userDatas[2] as string));
		}

		StartCoroutine("FillRewardList");

		return true;
	}

	//关闭时候运行
	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		//清理List
		StopCoroutine("FillRewardList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		List<KodGames.Pair<int, int>> rewardList = SysLocalDataBase.ConvertIdCountList(costAndRewardAndSync.Reward);
		int row = rewardList.Count % C_COLUMN_COUNT == 0 ? rewardList.Count / C_COLUMN_COUNT - 1 : rewardList.Count / C_COLUMN_COUNT;
		List<KodGames.Pair<int, int>> rewards = null;
		for (int index = 0; index <= row; index++)
		{
			if (index < row)
			{
				rewards = rewardList.GetRange(index * C_COLUMN_COUNT, C_COLUMN_COUNT);
			}
			else
			{
				rewards = rewardList.GetRange(index * C_COLUMN_COUNT, Math.Min(rewardList.Count - index * C_COLUMN_COUNT, C_COLUMN_COUNT));
			}

			UIElemConverItem item = rewardPool.AllocateItem().GetComponent<UIElemConverItem>();
			item.SetData(rewards);

			scrollList.AddItem(item.gameObject);
		}
	}


	//关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkOK_Btn(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}

}
