using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIPnlDanMain : UIModule
{
	[System.Serializable]
	public class CardData
	{
		public Vector3 initPos;
		public Vector3 initScale;
		public Color initColor;
		public float initAngel;
		public AnimationCurve positionCurve;

		public float CurvePostionInterplator(float time, float start, float delta, float duration)
		{
			return positionCurve.Evaluate(time / duration) * delta + start;
		}
	}

	public List<CardData> avatarCardDatas;
	public List<UIButton> avatarCards;
	public UIBox selectBg;

	//public UIElemAssetIcon BackBg;
	public UIButton selectAvatarButton;

	public float dragDelta;
	public float roundDuration;

	private bool isScrolling;
	private int currentIndex = 0;
	private List<GameConfig.InitAvatarConfig.InitAvatar> avatarIds;
	private int[] uiTypes = new int[4];

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		selectAvatarButton.SetDragDropDelegate(DragAvatarSelectButtonDel);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		InitView();

		return true;
	}

	private void InitView()
	{
		InitData();

		StartCoroutine("FillData");
	}

	private void InitData()
	{
		currentIndex = 0;
		uiTypes[0] = _UIType.UIPnlDanFurnace;
		uiTypes[1] = _UIType.UIPnlDanAttic;
		uiTypes[2] = _UIType.UIPnlDanDecompose;
		uiTypes[3] = _UIType.UIPnlDanMaterial; 

		avatarCards[0].Data =   0;
		avatarCards[1].Data =   1;
		avatarCards[2].Data =   2;
		avatarCards[3].Data =   3;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		// Load Avatar Card.
		for (int index = 0; index < avatarCards.Count; index++)
		{
			//avatarCards[index].SetData(avatarIds[index].avatarResourceId);
			
			avatarCards[index].CachedTransform.localPosition = avatarCardDatas[index].initPos;
			avatarCards[index].CachedTransform.localScale = avatarCardDatas[index].initScale;
			avatarCards[index].CachedTransform.Rotate(Vector3.forward, avatarCardDatas[index].initAngel);
			avatarCards[index].SetColor(avatarCardDatas[index].initColor);
		}
	}

	public void DragAvatarSelectButtonDel(EZDragDropParams parms)
	{
		switch (parms.evt)
		{
			case EZDragDropEvent.Update:

				if (isScrolling)
					return;

				float dragDelta = parms.dragObj.transform.localPosition.x;
				if (System.Math.Abs(dragDelta) > this.dragDelta)
				{
					DragSelectAvatar(dragDelta < 0);
				}

				break;
		}
	}

	private void DragSelectAvatar(bool pre)
	{
		// Set Scroll State.
		isScrolling = true;
		selectBg.Hide(true);
		selectAvatarButton.controlIsEnabled = false;

		currentIndex = pre ? (currentIndex + 1 == avatarCards.Count ? 0 : currentIndex + 1) : (currentIndex - 1 < 0 ? avatarCards.Count - 1 : currentIndex - 1);

		for (int index = 0; index < avatarCards.Count; index++)
		{
			int nextIndex = -1;

			for (int i = 0; i < avatarCardDatas.Count; i++)
			{
				if (avatarCardDatas[i].initPos.Equals(avatarCards[index].CachedTransform.localPosition))
				{
					nextIndex = pre ? (i - 1 < 0 ? avatarCards.Count - 1 : i - 1) : (i + 1 == avatarCards.Count ? 0 : i + 1);
					break;
				}
			}

			AnimateRotation.Do(avatarCards[index].gameObject,
							   EZAnimation.ANIM_MODE.FromTo,
							   new Vector3(0, 0, avatarCardDatas[nextIndex].initAngel),
							   EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear),
							   roundDuration,
							   0f,
							   null,
							   null);

			FadeSprite.Do(avatarCards[index],
						  EZAnimation.ANIM_MODE.FromTo,
						  avatarCardDatas[nextIndex].initColor,
						  EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear),
						  roundDuration,
						  0f,
						  null,
						  null);

			//AnimateScale.Do(avatarCards[index].gameObject,
			//                EZAnimation.ANIM_MODE.FromTo,
			//                avatarCardDatas[nextIndex].initScale,
			//                EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear),
			//                roundDuration,
			//                0f,
			//                null,
			//                null);

			AnimatePosition.Do(avatarCards[index].gameObject,
							   EZAnimation.ANIM_MODE.FromTo,
							   avatarCardDatas[nextIndex].initPos,
							   avatarCardDatas[nextIndex].CurvePostionInterplator,
							   roundDuration,
							   0f,
							   null,
							   (data) =>
							   {
								   isScrolling = false;
								   selectBg.Hide(false);
								   selectAvatarButton.controlIsEnabled = true;
								   Debug.Log("INDEX : " + currentIndex.ToString());
							   });
		}
	}

	//炼丹
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAlchemyButton(UIButton btn)
	{
		int clickIndex = (int)btn.Data;

		if (clickIndex == currentIndex)
		{
			GameUtility.JumpUIPanel(uiTypes[currentIndex]);
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanMenuBot));
		}
		else if (Mathf.Abs(clickIndex - currentIndex) == 2)
			return;
		else if (Mathf.Abs(clickIndex - currentIndex) == 1)
		{
			if(clickIndex - currentIndex == 1)
				DragSelectAvatar(true);
			else
				DragSelectAvatar(false);
		}
		else if (Mathf.Abs(clickIndex - currentIndex) == 3)
		{
			if (clickIndex - currentIndex == -3)
				DragSelectAvatar(true);
			else
				DragSelectAvatar(false);
		}
	}

	//炼丹
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDragButton(UIButton btn)
	{
		GameUtility.JumpUIPanel(uiTypes[currentIndex]);
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanMenuBot));
	}

	//内丹介绍
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanHelpInfoClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce),DanConfig._IntroduceType.DanIntroduce);
	}
}

