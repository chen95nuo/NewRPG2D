using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemRunAccumlativeItem : MonoBehaviour
{
	public List<UIElemAssetIcon> assetIcons;
	public List<SpriteText> assetIconNames;
	public SpriteText label;
	public SpriteText numberLabel;
	public UIProgressBar progress;
	public SpriteText progressLabel;
	public UIButton btn;
	public UIBox activityNotify;

	public void SetData(OperationConfig.OperationItem item, bool isEverPurchase, int couldPickCounts, int accumulateMoney)
	{
		InitData();

		// Set Operator Button.
		btn.Data = item.ItemId;

		if (accumulateMoney >= item.CycleMaxCount * item.CompareValue && couldPickCounts <= 0)
			btn.Text = GameUtility.FormatUIString("UIPnlRunAccumulativeTab_Number2");
		else
			btn.Text = GameUtility.FormatUIString("UIPnlRunAccumulativeTab_Number1", couldPickCounts);

		if (accumulateMoney < item.CycleMaxCount * item.CompareValue || couldPickCounts >= 1)
			UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(btn, false);
		else
			UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(btn, true);

		// Set Reward Icon.
		for (int index = 0; index < item.Rewards.Count; index++)
			SetAssetIcon(index, item.Rewards[index]);

		// Set Notify Icon State.
		if (activityNotify != null)
			activityNotify.Hide(!(couldPickCounts > 0));

		// Set NextPick Message and currentPick Message.
		int nextPickCount = Math.Min(accumulateMoney / item.CompareValue + 1, item.CycleMaxCount);
		int nextPickRequireMoney = item.CompareValue - accumulateMoney % item.CompareValue;

		if (accumulateMoney >= item.CycleMaxCount * item.CompareValue)
		{
			progress.Value = 1.0f;
			progressLabel.Text = GameUtility.FormatUIString("UIPnlRunAccumulativeTab_Sign", (item.CompareValue) / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower),
												item.CompareValue / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower));
			label.Text = GameUtility.GetUIString("UIPnlRunAccumulativeTab_Max");
		}
		else
		{
			progress.Value = (accumulateMoney % item.CompareValue) / ((float)item.CompareValue);
			progressLabel.Text = string.Format(GameUtility.GetUIString("UIPnlRunAccumulativeTab_Sign"), (accumulateMoney % item.CompareValue) / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower),
											item.CompareValue / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower));
			label.Text = string.Format(item.ItemDesc, nextPickCount, nextPickRequireMoney / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower));
		}

		// Set Max Pickable Message (static).
		numberLabel.Text = GameUtility.FormatUIString("UIPnlRunAccumulativeTab_Number", item.CycleMaxCount);
	}

	private void InitData()
	{
		// Hide Reward Icon.
		for (int index = 0; index < Math.Min(assetIcons.Count, assetIconNames.Count); index++)
		{
			assetIcons[index].Hide(true);
			assetIconNames[index].Text = string.Empty;
		}

		label.Text = string.Empty;
		numberLabel.Text = string.Empty;
		progressLabel.Text = string.Empty;

		progress.Value = 0;

		if (activityNotify != null)
			activityNotify.Hide(true);
	}

	private void SetAssetIcon(int index, Reward reward)
	{
		assetIcons[index].Hide(false);

		assetIcons[index].Data = reward.id;
		assetIcons[index].SetData(reward.id, reward.count);
		assetIconNames[index].Text = ItemInfoUtility.GetAssetName(reward.id);
	}
}
