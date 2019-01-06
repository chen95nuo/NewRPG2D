using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgOpenBox : UIModule
{
	/// <summary>
	/// Show data.
	/// title,unitPrice,itemIconName,priceIconName,totalCount should not be null
	/// </summary>
	public class ShowData
	{
		public int iconId;
		public int selectCount;
	}

	// Use to layout button
	public SpriteText titleText;
	public SpriteText itemCountField;
	public UIButton increaseBtn;
	public UIButton decreaseBtn;

	public UIElemAssetIcon itemIcon;

	private ShowData showData;

	public EZAnimation.EASING_TYPE easingType;

	public bool useAcc;
	public float minDelta;
	public float deltaDuration;
	public int deltaCount;
	private float maxDelta;

	public int minAcc;
	public int acc;
	public int accCount;
	private int maxAcc;

	private int currentAcc;

	private EZAnimation.Interpolator interpolator;
	private float pressTimer;
	private float lastChangeTime;
	private bool increaseBtnPressed = false;
	private bool decreaseBtnPressed = false;

	private int maxAmount;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		increaseBtn.SetInputDelegate(IncreaseInputDel);
		decreaseBtn.SetInputDelegate(DecreaseInputDel);

		return true;
	}

	public void ShowDialog(ShowData showData)
	{
		if (ShowSelf() == false)
			return;

		this.showData = showData;
		this.increaseBtnPressed = false;
		this.decreaseBtnPressed = false;

		maxDelta = minDelta + ((float)deltaCount) * deltaDuration;
		maxAcc = minAcc + acc * accCount;
		currentAcc = minAcc;

		maxAmount = showData.selectCount;

		// Title
		titleText.Text = ItemInfoUtility.GetAssetName(showData.iconId);

		// Count of the select count
		itemCountField.Text = showData.selectCount.ToString();

		// Icon
		itemIcon.SetData(showData.iconId);

		// Count
		SetSelectCount(showData.selectCount);
	}

	private void Update()
	{
		// Update select count
		if (increaseBtnPressed || decreaseBtnPressed)
		{
			pressTimer += Time.deltaTime;
			float deltaPerSecond = interpolator(Mathf.Min(pressTimer, deltaDuration), minDelta, maxDelta - minDelta, deltaDuration);

			if (deltaPerSecond != 0 && pressTimer - lastChangeTime > (1 / deltaPerSecond))
			{
				if (currentAcc < maxAcc && useAcc)
					currentAcc += acc;

				lastChangeTime += (1 / deltaPerSecond);

				if (increaseBtnPressed)
					SetSelectCount(showData.selectCount + (useAcc ? currentAcc : 1));
				else if (decreaseBtnPressed)
					SetSelectCount(showData.selectCount - (useAcc ? currentAcc : 1));
			}
		}
	}

	private void SetSelectCount(int count)
	{
		showData.selectCount = GetAfordedCount(count);
		itemCountField.Text = showData.selectCount.ToString();
	}

	private int GetAfordedCount(int count)
	{
		if (count <= 0)
			return 1;
		else if (count > 99)
			return 99;
		else if (count > maxAmount)
			return maxAmount;

		return count;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickIncreaseBtn(UIButton btn)
	{
		SetSelectCount(showData.selectCount + 1);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickDecreaseBtn(UIButton btn)
	{
		SetSelectCount(showData.selectCount - 1);
	}

	private void IncreaseInputDel(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				increaseBtnPressed = true;
				decreaseBtnPressed = false;
				currentAcc = minAcc;
				StartPressEasing();
				break;

			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				increaseBtnPressed = false;
				decreaseBtnPressed = false;
				break;
		}
	}

	private void DecreaseInputDel(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				increaseBtnPressed = false;
				decreaseBtnPressed = true;
				currentAcc = minAcc;
				StartPressEasing();
				break;

			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				increaseBtnPressed = false;
				decreaseBtnPressed = false;
				break;
		}
	}

	private void StartPressEasing()
	{
		pressTimer = 0;
		lastChangeTime = 0;
		interpolator = EZAnimation.GetInterpolator(easingType);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCalcleClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnOkClick(UIButton btn)
	{
		RequestMgr.Inst.Request(new ConsumeItemReq(showData.iconId, showData.selectCount));
		HideSelf();
	}
}
