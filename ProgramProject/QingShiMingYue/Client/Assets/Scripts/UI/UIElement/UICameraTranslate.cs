using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UICameraTranslate : UICameraTranslateBase
{
	public UIScroll3D scroll3DUI;

	private bool clickShowDetail = false;
	private int positionId;
	private bool isCheerAvatarOpened;
	private KodGames.ClientClass.Player player;

	public void Init(int positionId, bool isCheerAvatarOpened)
	{
		Init(SysLocalDataBase.Inst.LocalPlayer, positionId, isCheerAvatarOpened, true);
	}

	public void Init(KodGames.ClientClass.Player initPlayer, int positionId, bool isCheerAvatarOpened, bool showDetail)
	{
		ClearData();

		this.clickShowDetail = showDetail;
		this.player = initPlayer;
		this.positionId = positionId;
		this.isCheerAvatarOpened = isCheerAvatarOpened;

		// Set Input Del.
		scroller.SetInputDelegate(EZInputDel);
		itemBtn.SetInputDelegate(ListBtnEZInputDel);


		if (!gameObject.activeInHierarchy)
			gameObject.SetActive(true);

		LoadAllAvatarModel();

		SetScrollerMaxValue();
	}

	public override void LoadAllAvatarModel()
	{
		var position = player.PositionData.GetPositionById(this.positionId);

		if (position == null)
			return;

		position.AvatarLocations.Sort(DataCompare.CompareLocationByShowPos);

		int openLineUpPosCount = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(positionId).GetAvatarSlotCountByLevel(player.LevelAttrib.Level);
		for (int index = 0; index < openLineUpPosCount; index++)
		{
			KodGames.ClientClass.Location avatarLocation = null;
			for (int i = 0; i < position.AvatarLocations.Count; i++)
			{
				if (index == PlayerDataUtility.GetIndexPosByBattlePos(position.AvatarLocations[i].ShowLocationId))
				{
					avatarLocation = position.AvatarLocations[i];
					break;
				}
			}

			int resourceId = avatarLocation != null ? avatarLocation.ResourceId : IDSeg.InvalidId;
			int breakLevel = avatarLocation != null ? player.SearchAvatar(avatarLocation.Guid).BreakthoughtLevel : 0;

			Avatar avatar = LoadAvatarModel(resourceId, breakLevel, index);
			avatarModels.Add(avatar);
		}
	}

	public void SnapPartnerTranslate()
	{
		Vector2 snapValue = new Vector2(scroller.MinValue.x + avatarScrollerDelta * avatarModels.Count - 1 + scroll3DUI.cheerAvatarFixValue.x, scroller.Value.y + scroll3DUI.cheerAvatarFixValue.y);
		scroller.ScrollToValue(snapValue, 0f, EZAnimation.EASING_TYPE.Linear, Vector2.zero);

		snapIndex = avatarModels.Count;

		if (avatarSnapedDel != null)
			avatarSnapedDel(snapIndex);
	}

	public override void ClearData()
	{
		base.ClearData();

		// Reset PositionId.
		this.positionId = IDSeg.InvalidId;

		// Reset CheerAvatarStatus.
		this.isCheerAvatarOpened = false;
	}

	public override void SetScrollerMaxValue()
	{
		int openLineUpPosCount = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(positionId).GetAvatarSlotCountByLevel(player.LevelAttrib.Level);

		if (isCheerAvatarOpened)
			openLineUpPosCount++;

		Vector2 fixedValue = isCheerAvatarOpened ? scroll3DUI.cheerAvatarFixValue : Vector2.zero;

		if (scroller.MaxValue.x != scroller.MinValue.x + (openLineUpPosCount - 1) * avatarScrollerDelta + fixedValue.x)
			scroller.MaxValue = new Vector2(scroller.MinValue.x + (openLineUpPosCount - 1) * avatarScrollerDelta + fixedValue.x, scroller.MaxValue.y + fixedValue.y);

		if (scroll3DUI != null)
			scroll3DUI.Init(isCheerAvatarOpened);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public override void OnShowAvatarDetailClick(UIButton btn)
	{
		if (!clickShowDetail)
			return;

		var avatarLocations = player.PositionData.GetPositionById(positionId).AvatarLocations;

		for (int i = 0; i < avatarLocations.Count; i++)
		{
			if (avatarLocations[i].ShowLocationId == PlayerDataUtility.GetBattlePosByIndexPos(snapIndex))
			{
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAvatarInfo, avatarLocations[i], true, false, false, true, null);
				break;
			}
		}
	}
}

