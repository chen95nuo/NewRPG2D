using UnityEngine;
using ClientServerCommon;
using System.Collections;
using System.Collections.Generic;
using KodGames.ExternalCall;

public class UIDlgConvertMain : UIModule
{
	//动画播放相关预制品
	public List<SpriteText> numberPrefabs;	//滚动数字
	public List<Transform> numberTransforms;
	public GameObject numberTransform;	//中心点

	public Transform upPosition;	//上消失点
	public Transform downPosition;	//下消失点

	//界面管理
	public SpriteText briefDesc;	//简要描述

	//随机数组管理
	public List<UIElemConvertMainRoot> groups;

	//下方分区显示
	public GameObject downObject;
	public SpriteText otherArea;	//别的区兑换过的

	public SpriteText grodDesc;	//获得描述

	public SpriteText exchangeNumber;	//兑换码
	public SpriteText exchangeDesc;	//分享描述
	public UIBox fuzhiBg;
	public UIButton fuzhi;

	//按钮组
	public UIChildLayoutControl buttonControl;

	//动画控制
	public float actionMaxSpeed;	//最大速度
	public float accelerationSpeed;	//加速度
	public float colorB_B_B;	//颜色变化率

	private float colorB_B = 0.0001f; //透明度增量值
	private float actionStartSpeed = 1;	//当前速度
	private float colorB = 0;	//透明度初始值

	private bool wait = true;

	private int position;
	private int showType;
	private int number;

	private string exchangeCode;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		exchangeCode = string.Empty;

		briefDesc.Text = ConfigDatabase.DefaultCfg.SevenElevenGiftConfig.BridefDesc;

		numberTransform.SetActive(false);

		//设置三个位数显示
		if ((userDatas[3] as KodGames.ClientClass.SevenElevenGift) != null && (userDatas[3] as KodGames.ClientClass.SevenElevenGift).ConvertType != _NumberConvertType.UnKnow)
		{
			groups[0].SetData(_NumberPositionType.UnitsDigit, (int)(userDatas[0]) != -1 ? (int)(userDatas[0]) : -2);
			groups[1].SetData(_NumberPositionType.TensDigit, (int)(userDatas[1]) != -1 ? (int)(userDatas[1]) : -2);
			groups[2].SetData(_NumberPositionType.HundredsDigit, (int)(userDatas[2]) != -1 ? (int)(userDatas[2]) : -2);

			SetLYC(true, true);
		}
		else
		{
			groups[0].SetData(_NumberPositionType.UnitsDigit, (int)(userDatas[0]));
			groups[1].SetData(_NumberPositionType.TensDigit, (int)(userDatas[1]));
			groups[2].SetData(_NumberPositionType.HundredsDigit, (int)(userDatas[2]));

			SetLYC(!((bool)(userDatas[6])), false);
		}

		ShowDwonUI(userDatas[3] as KodGames.ClientClass.SevenElevenGift, (bool)(userDatas[4]), (string)(userDatas[5]));

		return true;
	}

	//设置下方显示
	private void ShowDwonUI(KodGames.ClientClass.SevenElevenGift sevenElevenGift, bool isConverArea, string areaName)
	{
		downObject.SetActive(false);

		otherArea.Text = string.Empty;

		grodDesc.Text = string.Empty;

		exchangeNumber.Text = string.Empty;
		exchangeDesc.Text = string.Empty;

		fuzhiBg.Hide(true);
		fuzhi.Hide(true);

		if (sevenElevenGift != null && sevenElevenGift.ConvertType != _NumberPositionType.UnKnow)
		{
			if (!isConverArea)
			{
				otherArea.Text = GameUtility.FormatUIString("UIDlgconvetExplain_Main_Area", areaName);
			}
			else
			{
				if (sevenElevenGift.ConvertType == _NumberConvertType.GoldConVert)
				{
					List<KodGames.Pair<int, int>> rewardList = SysLocalDataBase.ConvertIdCountList(sevenElevenGift.Reward);
					if (rewardList != null && rewardList.Count > 0)
					{
						grodDesc.Text = GameUtility.FormatUIString("UIDlgConvetExplain_Main_Grod_Desc", GameDefines.textColorBtnYellow.ToString(),
																	ItemInfoUtility.GetAssetName(rewardList[0].first),
																	GameDefines.textColorWhite.ToString(),
																	rewardList[0].second);
					}
				}
				else
				{
					downObject.SetActive(true);
					fuzhiBg.Hide(false);
					fuzhi.Hide(false);
					exchangeCode = sevenElevenGift.ExchangeCode;
					exchangeNumber.Text = GameUtility.FormatUIString("UIDlgConvetExplain_Main_Explain_Duihuanma", GameDefines.textColorBtnYellow.ToString(),
																			GameDefines.textColorWhite.ToString(), sevenElevenGift.ExchangeCode);
					exchangeDesc.Text = GameUtility.GetUIString("UIDlgConvetExplain_Main_Explain_Desc");
				}
			}
		}
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	//动画控制
	public void Update()
	{
		if (!wait)
		{
			//先颜色渐变
			if (colorB <= 1)
				UpdataColor();
			else
			{
				//速度控制
				if (actionStartSpeed < actionMaxSpeed)
					UpdatePosition();
				else
				{
					//数字由下移动到上面
					if (groups[position].GetSelf().groupShuzi.gameObject.transform.position.y < groups[position].GetSelf().numberGroup.gameObject.transform.position.y)
						groups[position].GetSelf().groupShuzi.gameObject.transform.position = new Vector3(groups[position].GetSelf().groupShuzi.gameObject.transform.position.x, groups[position].GetSelf().groupShuzi.gameObject.transform.position.y + 1, groups[position].GetSelf().groupShuzi.gameObject.transform.position.z);
					else
					{
						//更新函数控制
						wait = true;
						actionStartSpeed = 1;
						colorB = 0;
						colorB_B = 0.0001f;
					}
				}
			}
		}
	}

	#region 动画控制

	private void UpdataColor()
	{
		for (int index = 0; index < numberPrefabs.Count; index++)
		{
			numberPrefabs[index].SetColor(new Color(1, 1, 1, colorB));
		}

		colorB += colorB_B;
		colorB_B += colorB_B_B;
	}

	private void UpdatePosition()
	{
		for (int index = 0; index < numberPrefabs.Count; index++)
			UpdateNumberTransform(numberPrefabs[index], numberPrefabs[index].gameObject.transform.position.y + actionStartSpeed);

		SetPosition();
		SetGameObjectHide();

		actionStartSpeed += (Time.deltaTime * accelerationSpeed);
		if (actionStartSpeed >= actionMaxSpeed)
		{
			numberTransform.SetActive(false);
			groups[position].SetData(showType, number);
			groups[position].GetSelf().groupShuzi.gameObject.transform.position = new Vector3(groups[position].GetSelf().groupShuzi.gameObject.transform.position.x, downPosition.position.y, groups[position].GetSelf().groupShuzi.gameObject.transform.position.z);
		}
	}

	private void UpdateNumberTransform(SpriteText text, float hight)
	{
		text.gameObject.transform.position = new Vector3(text.gameObject.transform.position.x, hight, text.gameObject.transform.position.z);
	}

	private void SetPosition()
	{
		for (int index = 0; index < numberPrefabs.Count; index++)
		{
			if (numberPrefabs[index].gameObject.transform.position.y >= numberTransform.transform.position.y + 45)
			{
				int temp = GetMixNumber(index);
				numberPrefabs[index].Text = temp.ToString();
				numberPrefabs[index].gameObject.transform.position = new Vector3(numberPrefabs[index].gameObject.transform.position.x, numberTransform.transform.position.y - 135 + actionStartSpeed, numberPrefabs[index].gameObject.transform.position.z);
			}
		}
	}

	private int GetMixNumber(int index)
	{
		int temp = int.Parse(numberPrefabs[index].Text);
		temp += 4;
		temp %= 10;
		return temp;
	}

	private void SetGameObjectHide()
	{
		for (int index = 0; index < numberPrefabs.Count; index++)
		{
			if (numberPrefabs[index].gameObject.transform.position.y > upPosition.position.y || numberPrefabs[index].gameObject.transform.position.y < downPosition.position.y)
				numberPrefabs[index].Hide(true);
			else
				numberPrefabs[index].Hide(false);
		}
	}

	#endregion

	private void SetLYC(params bool[] hideF)
	{
		for (int index = 0; index < Mathf.Min(hideF.Length, buttonControl.childLayoutControls.Length); index++)
			buttonControl.HideChildObj(buttonControl.childLayoutControls[index].gameObject, hideF[index]);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		if (wait)
			HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOtherBtn(UIButton btn)
	{
		if (wait)
			RequestMessage(_NumberConvertType.ExchangeCodeConvert);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGrodBtn(UIButton btn)
	{
		if (wait)
			RequestMessage(_NumberConvertType.GoldConVert);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDesc(UIButton btn)
	{
		if (wait)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgConvertTips), ConfigDatabase.DefaultCfg.SevenElevenGiftConfig.MinuteDesc);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickJihuo(UIButton btn)
	{
		if (wait)
		{
			int showType = (int)btn.Data;
			bool pRet = false;
			switch (showType)
			{
				case _NumberPositionType.UnitsDigit: pRet = true; break;
				case _NumberPositionType.TensDigit:
					if (groups[0].GetSelf().Showtype == 2)
						pRet = true;
					break;
				case _NumberPositionType.HundredsDigit:
					if (groups[0].GetSelf().Showtype == 2 && groups[1].GetSelf().Showtype == 2)
						pRet = true;
					break;
			}

			if (pRet)
				RequestMgr.Inst.Request(new TurnNumberReq(DevicePlugin.GetGUID(), (int)(btn.Data)));
			else
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgConvetExplain_Main_TipsFlow"));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCopy(UIButton btn)
	{
		if (wait)
			Platform.Instance.CopyString(exchangeCode);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFaceBook(UIButton btn)
	{
		if (wait)
		{
			Application.OpenURL(GameUtility.GetUIString("UIDlgConvetExplain_Main_FaceBook"));
		}
	}

	private void RequestMessage(int type)
	{
		string message = string.Empty;

		int count = 0;
		for (int index = 0; index < groups.Count; index++)
		{
			if (groups[index].GetSelf().Showtype == 2)
				count++;
		}

		switch (count)
		{
			case 0: message = GameUtility.GetUIString("UIDlgConvetExplain_Main_Explain_Start_Desc_03"); break;
			case 1:
			case 2: message = GameUtility.GetUIString("UIDlgConvetExplain_Main_Explain_Start_Desc_02"); break;
			case 3: message = GameUtility.GetUIString("UIDlgConvetExplain_Main_Explain_Start_Desc_01"); break;
		}

		if (count == 0)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), message);
		}
		else
		{
			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
			okCallback.Callback = (userData) =>
			{
				RequestMgr.Inst.Request(new NumberConvertReq(type));
				return true;
			};

			MainMenuItem cancelCallback = new MainMenuItem();
			cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(
				GameUtility.GetUIString("UIDlgConvetExplain_Main_Explain_Title"),
				message,
				true,
				cancelCallback,
				okCallback);

			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgMessage), showData);
		}
	}

	public void NumberConvertReqSuccess(KodGames.ClientClass.SevenElevenGift sevenElevenGift)
	{
		if (sevenElevenGift.ConvertType == 1)
			SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(_UIType.UIEffectOpenBox, 1, sevenElevenGift.CostAndRewardAndSync));

		for (int index = 0; index < groups.Count; index++)
		{
			if (groups[index].GetSelf().Showtype != 2)
				groups[index].HideData();
		}

		ShowDwonUI(sevenElevenGift, true, string.Empty);
		SetLYC(true, true);
	}

	public void TurnNumberReqSuccess(int positionX, int number)
	{
		int index = 0;
		switch (positionX)
		{
			case _NumberPositionType.UnitsDigit: index = 0; break;
			case _NumberPositionType.TensDigit: index = 1; break;
			case _NumberPositionType.HundredsDigit: index = 2; break;
		}

		this.position = index;
		this.showType = positionX;
		this.number = number;

		numberTransform.gameObject.transform.position = new Vector3(groups[position].GetSelf().numberGroup.gameObject.transform.position.x, groups[position].GetSelf().numberGroup.gameObject.transform.position.y, numberTransform.gameObject.transform.position.z);
		SetGameObjectHide();

		for (int i = 0; i < Mathf.Min(numberPrefabs.Count, numberTransforms.Count); i++)
		{
			numberPrefabs[i].gameObject.transform.position = new Vector3(numberPrefabs[i].gameObject.transform.position.x, numberTransforms[i].gameObject.transform.position.y, numberPrefabs[i].gameObject.transform.position.y);
			numberPrefabs[i].Text = i.ToString();
		}
		SetGameObjectHide();
		numberTransform.SetActive(true);
		groups[position].GetSelf().groupJihuo.Hide(true);

		wait = false;
	}
}