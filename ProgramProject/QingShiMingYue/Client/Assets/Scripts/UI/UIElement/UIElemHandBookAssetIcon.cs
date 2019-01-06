using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemHandBookAssetIcon : MonoBehaviour
{
	public UIElemAssetIcon assetIcon;
	public UIElemAssetIcon costIcon;
	public UIProgressBar progress;
	public UIButton button;
	public SpriteText costText;
	public SpriteText nameText;
	public UIBox maskBox;//阴影	
	public GameObject handBookItemRoot;

	private IllustrationConfig.Illustration illustrationCfg;
	public IllustrationConfig.Illustration IllustrationCfg
	{
		get { return illustrationCfg; }
	}

	// 图鉴中显示数据会有减少的情况，需要clearList的时候清除助手数据
	public void ClearData()
	{
		button.GetComponent<UIElemAssistantBase>().assistantData = IDSeg.InvalidId;
	}

	public void SetData(IllustrationConfig.Illustration illustrationCfg)
	{
		// Set Assistant Data.
		if (illustrationCfg != null)
			button.GetComponent<UIElemAssistantBase>().assistantData = illustrationCfg.Id;
		else
			button.GetComponent<UIElemAssistantBase>().assistantData = IDSeg.InvalidId;

		this.illustrationCfg = illustrationCfg;

		if (illustrationCfg != null)
		{
			Hide(false);

			assetIcon.SetData(illustrationCfg.Id);						
			assetIcon.Data = this;
			assetIcon.border.Data = this;
			button.Data = this;
			nameText.Text = ItemInfoUtility.GetAssetName(illustrationCfg.Id);

			RefreshState();
		}
		else
		{
			Hide(true);
		}
	}

	// 刷新显示状态
	public void RefreshState()
	{
		// 显示消耗品
		costIcon.SetData(illustrationCfg.Cost[0].id);
		costText.Text = illustrationCfg.Cost[0].count.ToString();

		// 获取已有数量
		var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(illustrationCfg.FragmentId);
		var consumableCount = consumable != null ? consumable.Amount : 0;

		// 计算进度条
		progress.Value = (float)consumableCount / (float)illustrationCfg.FragmentCount;
		progress.Text = consumableCount + "/" + illustrationCfg.FragmentCount;

		bool combinable = consumableCount >= illustrationCfg.FragmentCount;
		if (combinable)
		{
			button.spriteText.characterSize = 17f;
			button.Text = GameUtility.GetUIString("UIElemHandBookAssetIcon_HeCheng");//合成
		}
		else
		{
			button.spriteText.characterSize = 14f;
			button.Text = GameUtility.GetUIString("UIElemHandBookAssetIcon_TuJing");//途径
		}

		// 控制阴影
		bool alreadyHave = SysLocalDataBase.Inst.LocalPlayer.CardIds.Contains(IllustrationCfg.Id);
		maskBox.Hide(alreadyHave);
	}

	// hide self
	public void Hide(bool hide)
	{
		handBookItemRoot.SetActive(!hide);
	}
}