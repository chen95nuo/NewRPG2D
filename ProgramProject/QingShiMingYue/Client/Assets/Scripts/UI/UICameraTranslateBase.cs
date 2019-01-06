using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UICameraTranslateBase : MonoBehaviour
{
	public delegate void AvatarSnapedDelegate(int avatarBattleIndex);

	public UIScroller scroller;
	public UIListButton itemBtn;
	public Transform cameraRoot;
	public float translateFactor = 1;

	//Scroll delta to snap
	public float snapScrollOffset;

	//Avatar model distance between two models.
	public float avatarOffset;
	public float avatarScrollerDelta = 250f;

	//Avatar model rotation
	public Vector3 avatarModelRotaion;
	public Vector3 avatarPostion;
	public Vector3 avatarScale = new Vector3(1f, 1f, 1f);
	public Vector2 oldScrollValue;

	protected int snapIndex = 0;
	public int SnapIndex { get { return snapIndex; } }

	protected List<KodGames.ClientClass.Avatar> avatarDatas = new List<KodGames.ClientClass.Avatar>();
	protected List<Avatar> avatarModels = new List<Avatar>();

	protected AvatarSnapedDelegate avatarSnapedDel;

	public void Init(List<KodGames.ClientClass.Avatar> avatarDatas)
	{
		ClearData();

		snapIndex = 0;
		this.avatarDatas = avatarDatas;

		// Set Input Del.
		scroller.SetInputDelegate(EZInputDel);
		itemBtn.SetInputDelegate(ListBtnEZInputDel);

		LoadAllAvatarModel();
		SetScrollerMaxValue();
	}

	public void SetAvatarSnapedDelegate(AvatarSnapedDelegate snapedDel)
	{
		avatarSnapedDel = snapedDel;
	}

	public void RemoveOverlay()
	{
		foreach (Avatar avatar in avatarModels)
			if (avatar.AvatarId != IDSeg.InvalidId)
				avatar.PlayAnim(ConfigDatabase.DefaultCfg.AnimationConfig.GetDefaultAnimation(avatar.AvatarTypeId));
	}

	public virtual void ClearData()
	{
		// Delete Input Del.
		itemBtn.RemoveInputDelegate(ListBtnEZInputDel);

		// Destroy Avatar Model.
		foreach (Avatar avatar in avatarModels)
			avatar.Destroy();

		// Clear List.
		avatarModels.Clear();
		avatarDatas.Clear();
		scroller.ScrollPosition = scroller.defaultValue;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public virtual void OnShowAvatarDetailClick(UIButton btn)
	{
		if (snapIndex < 0 || snapIndex >= avatarDatas.Count)
			return;
		
		GameUtility.ShowAssetInfoUI(avatarDatas[snapIndex].ResourceId);
	}

	public virtual void LoadAllAvatarModel()
	{
		for (int i = 0; i < avatarDatas.Count; i++)
		{
			Avatar avatar = LoadAvatarModel(avatarDatas[i].ResourceId, avatarDatas[i].BreakthoughtLevel, i);
			avatarModels.Add(avatar);
		}
	}

	protected Avatar LoadAvatarModel(int avatarId, int breakLevel, int index)
	{
		Avatar avatar = Avatar.CreateAvatar(avatarId);

		int avatarAssetId = IDSeg.InvalidId;

		if (avatarId != IDSeg.InvalidId)
		{
			avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId).GetAvatarAssetId(breakLevel);
			// Load avatar.
			if (avatar.Load(avatarAssetId, false, true) == false)
				return null;
		}

		// Set to current layer.
		avatar.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);
		avatar.gameObject.transform.localPosition = new Vector3(transform.localPosition.x + avatarOffset * index, avatarPostion.y, avatarPostion.z);

		// Put to mount bone.
		ObjectUtility.AttachToParentAndKeepLocalTrans(gameObject, avatar.gameObject);

		avatar.gameObject.transform.localRotation *= Quaternion.AngleAxis(avatarModelRotaion.x, Vector3.right);
		avatar.gameObject.transform.localRotation *= Quaternion.AngleAxis(avatarModelRotaion.y, Vector3.up);

		avatar.gameObject.transform.localScale = avatarScale;

		//Play Idle animation.
		if (avatar.AvatarId != IDSeg.InvalidId)
			avatar.PlayAnim(ConfigDatabase.DefaultCfg.AnimationConfig.GetDefaultAnimation(avatar.AvatarTypeId));

		return avatar;
	}

	public void UpdateModels(int avatarIndex, int avatarId, int breakLevel)
	{
		Avatar avatar = LoadAvatarModel(avatarId, breakLevel, avatarIndex);

		if (avatarModels.Count <= avatarIndex)
		{
			avatarModels.Add(avatar);
		}
		else
		{
			var avatarToDelete = avatarModels[avatarIndex];
			avatarModels.RemoveAt(avatarIndex);

			if (avatarToDelete != null)
				avatarToDelete.Destroy();

			avatarModels.Insert(avatarIndex, avatar);
		}

		SetScrollerMaxValue();
	}

	public void SnapTranslate(int avatarIndex)
	{
		snapIndex = avatarIndex;

		Vector2 snapValue = new Vector2(scroller.MinValue.x + avatarScrollerDelta * avatarIndex, scroller.Value.y);
		scroller.ScrollToValue(snapValue, 0f, EZAnimation.EASING_TYPE.Linear, Vector2.zero);
	}

	public virtual void SetScrollerMaxValue()
	{
		if (scroller.MaxValue.x != scroller.MinValue.x + (avatarDatas.Count - 1) * avatarScrollerDelta)
			scroller.MaxValue = new Vector2(scroller.MinValue.x + (avatarDatas.Count - 1) * avatarScrollerDelta, scroller.MaxValue.y);
	}

	protected void SnapToAvatarModel()
	{
		if (scroller.AutoScrolling)
			return;

		int lastSnapIndex = snapIndex;

		if (GetScrollDelta() > snapScrollOffset)
		{
			scroller.ScrollToValue(GetSnapValue(snapIndex + 1), 0.1f, EZAnimation.EASING_TYPE.Linear, Vector2.zero);

			if (avatarSnapedDel != null && lastSnapIndex != snapIndex)
				avatarSnapedDel(snapIndex);
		}
		else if (GetScrollDelta() < -snapScrollOffset)
		{
			scroller.ScrollToValue(GetSnapValue(snapIndex - 1), 0.1f, EZAnimation.EASING_TYPE.Linear, Vector2.zero);
			if (avatarSnapedDel != null && lastSnapIndex != snapIndex)
				avatarSnapedDel(snapIndex);
		}
		else
		{
			scroller.ScrollToValue(GetSnapValue(snapIndex), 0.1f, EZAnimation.EASING_TYPE.Linear, Vector2.zero);
		}
	}

	protected void Update()
	{
		Vector2 scrollValue = scroller.Value;
		if (Mathf.Approximately(scrollValue.x, oldScrollValue.x))
			return;

		if (float.IsNaN(scrollValue.x))
			return;

		cameraRoot.Translate(translateFactor * (scrollValue.x - oldScrollValue.x), 0f, 0f);
		oldScrollValue = scrollValue;
	}

	protected Vector2 GetSnapValue(int index)
	{
		snapIndex = Mathf.Max(0, index);

		snapIndex = Mathf.Min(snapIndex, avatarModels.Count);

		return new Vector2(scroller.MinValue.x + avatarScrollerDelta * snapIndex, scroller.Value.y);
	}

	protected float GetScrollDelta()
	{
		return scroller.Value.x - (scroller.MinValue.x + snapIndex * avatarScrollerDelta);
	}

	protected void EZInputDel(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				SnapToAvatarModel();
				break;

			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				scroller.PointerReleased();
				SnapToAvatarModel();
				break;
		}
	}

	protected void ListBtnEZInputDel(ref POINTER_INFO ptr)
	{
		scroller.OnInput(ptr);
	}
}
