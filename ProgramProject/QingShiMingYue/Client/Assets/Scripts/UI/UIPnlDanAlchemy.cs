using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlDanAlchemy : UIModule
{
	/** 炼丹弹板界面*/
	//炼丹蒙板
	public GameObject alchemyBgObj;
	//按钮
	public GameObject btnRoot;
	public SpriteText decomposeLabel;

	public UIButton okBtn;
	public UIButton decomposeBtn;

	//额外获得
	public GameObject extraObj;
	//炼丹
	public UIElemAssetIcon core;

	//炼十次奖励
	public UIElemAssetIcon[] cores;
	public UIScrollList extraRewardList;
	public GameObjectPool extrarRewardPool;
	public UIBox[] danRedBgs;

	//显示分解次数
	public SpriteText freeCountLabel;
	public SpriteText bossCountLabel;
	public SpriteText costCountLabel;

	//单抽无额外奖励时按钮位置
	public Transform alchemyBtnTrans;
	//普通按钮位置
	public Transform batchBtnTrans;

	public Transform uIFxBatchDanTrans;

	public Transform timesRewardBgTrs;
	public Transform alchemyRewardBgTrs;
	public Transform center;

	public GameObject successRoot;
	public GameObject effectRoots;

	private int chatType;
	KodGames.ClientClass.Reward danReward;
	KodGames.ClientClass.Reward extraReward;

	private Vector3 resetZVec3 = new Vector3(0f, 0f, -0.099f);

	private List<GameObject> particleList = new List<GameObject>();//炼丹特效存储
	private bool isAniPlay;

	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;

	private float moveTime = 0;

	private float pI = 3.141592f;
	private int speedDelay = 10;
	private int radius = 120;

	private Animation btnAni;

	private Vector3 oldBtnOk;
	private Vector3 oldBtnDecompose;
	private bool isAniPlayed;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		isAniPlayed = false;

		oldBtnOk = okBtn.gameObject.transform.localPosition;
		oldBtnDecompose = decomposeBtn.transform.localPosition;

		if(userDatas.Length > 0)
		{
			this.danReward = userDatas[0] as KodGames.ClientClass.Reward;
			this.extraReward = userDatas[1] as KodGames.ClientClass.Reward;
			this.chatType = (int)userDatas[2];
			this.decomposeInfo = userDatas[3] as com.kodgames.corgi.protocol.DecomposeInfo;
		}

		StartCoroutine("PlayStarAnimation");

		return true;
	}

	private void Update()
	{
		moveTime += Mathf.Min(0.3f, Time.deltaTime);

		//speedDelay 放慢速度 radius 旋转半径
		//公式根据弧长以及半径求出x，y坐标
		for (int i = 0; i < cores.Length; i++)
		{
			float x = Mathf.Sin(moveTime / speedDelay + i * ((2 * pI) / cores.Length)) * radius;
			float y = Mathf.Cos(moveTime / speedDelay + i * ((2 * pI) / cores.Length)) * radius;
			cores[i].transform.localPosition = new Vector3(x, y, -0.01f);
		}

		if(isAniPlayed && !btnAni.isPlaying)
			isAniPlay = false;
	}


	public override void RemoveOverlay()
	{
		okBtn.gameObject.transform.localPosition = oldBtnOk;
		decomposeBtn.gameObject.transform.localPosition = oldBtnDecompose;

		base.RemoveOverlay();
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
		DestoryRewardParticle();
	}

	private void ClearData()
	{
		StopCoroutine("FillExtraRewardList");
		extraRewardList.ClearList(false);
		extraRewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayStarAnimation()
	{
		isAniPlay = true;
		//练丹
		alchemyBgObj.SetActive(true);
		
		Animation alchAni = alchemyBgObj.GetComponent<Animation>();
		if (alchAni != null)
			alchAni.Play();

		btnRoot.SetActive(false);
		extraObj.SetActive(false);

		for (int i = 0; i < danRedBgs.Length; i++)
		{
			danRedBgs[i].Hide(true);
		}

		core.Hide(true);

		for (int i = 0; i < cores.Length; i++)
			cores[i].Hide(true);

		GameObject furnaceEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.furnaceEffect));

		if (furnaceEffect != null)
		{
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(alchemyBgObj.gameObject, furnaceEffect);
			furnaceEffect.transform.position = center.position;
			particleList.Add(furnaceEffect);
		}
		
		GameObject creatEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danCreateEffect));

		if (creatEffect != null)
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(successRoot.gameObject, creatEffect);
		creatEffect.transform.position = uIFxBatchDanTrans.position;
		
		AudioManager.Instance.PlaySound(GameDefines.danAlchemy, 0.5f);

		yield return new WaitForSeconds(1.5f);

		//单独炼丹
		if (chatType == DanConfig._OperateType.Alchemy)
		{
			if (decomposeInfo.danDecomposeCout > 0)
			{
				freeCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanAlchemy_Free_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, decomposeInfo.danDecomposeCout, decomposeInfo.maxDanDecomposeCout);
				costCountLabel.Text = "";
				bossCountLabel.Text = "";
			}
			else
			{
				freeCountLabel.Text = "";
				costCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanAlchemy_Cost_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, decomposeInfo.danItemDecomposeCount, decomposeInfo.maxDanItemDecomposeCount);

				int bossItemCount = ItemInfoUtility.GetGameItemCount(decomposeInfo.decomposeCost.id);

				bossCountLabel.Text = string.Format(GameUtility.GetUIString("UUIPnlDanAlchemy_Boss_Label"), GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(decomposeInfo.decomposeCost.id), GameDefines.textColorWhite, decomposeInfo.decomposeCost.count, bossItemCount);
			}

			decomposeLabel.Text = GameUtility.GetUIString("UIPnlDanFurnace_Btn_Decompose_OneTimes_Label");
			GameObject danEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danDanEffect));

			if (danEffect != null)
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(successRoot.gameObject, danEffect);
			danEffect.transform.position = uIFxBatchDanTrans.position;

			AudioManager.Instance.PlaySound(GameDefines.menu_Blade2, 0f);

			AnimatePosition.Do(danEffect,
											EZAnimation.ANIM_MODE.FromTo,
											danEffect.transform.localPosition,
											core.transform.localPosition,				
											EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
											0.3f,
											0f,
											null,
											 (data) =>
											 {
												 if (danEffect != null)
													 GameObject.Destroy(danEffect.gameObject);

												 GameObject danGetEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danGetEffect));
												 if (danGetEffect != null)
													 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(core.border.gameObject, danGetEffect);												 

												 //丹
												 core.Hide(false);

												 if (danReward.Dan.Count == 0)
													 Debug.LogError("The Dan Count Is Zero");

												 core.SetData(danReward.Dan[0].ResourceId, danReward.Dan[0].BreakthoughtLevel, danReward.Dan[0].LevelAttrib.Level);
												 core.Data = danReward.Dan[0];

												 PlayQualityAnimation(core.gameObject, danReward.Dan[0].BreakthoughtLevel);

												 for (int i = 0; i < danRedBgs.Length; i++)
												 {
													 danRedBgs[i].Hide(true);

													 if (danReward.Dan[0] != null && danReward.Dan[0].DanAttributeGroups != null && i < danReward.Dan[0].DanAttributeGroups.Count)
													 {
														 danRedBgs[i].Hide(false);

														 string attr = "";
														 //描述
														 attr = attr + danReward.Dan[0].DanAttributeGroups[i].AttributeDesc;
														 //数值
														 attr = attr + string.Format(GameUtility.GetUIString("UIPnlDanInfo_UpAttr_Label"), danReward.Dan[0].DanAttributeGroups[i].DanAttributes[0].PropertyModifierSets[0].Modifiers[0].AttributeValue * 100);

														 danRedBgs[i].spriteText.Text = attr;
													 }
													 else
													 {
														 danRedBgs[i].Hide(false);
														 danRedBgs[i].spriteText.Text = string.Format(GameUtility.GetUIString("UIPnlDanInfo_Attribute1_Label"), GameDefines.textColorDackGray, ItemInfoUtility.GetAssetQualityColor(i + 1), ItemInfoUtility.GetDanTextQuality(i + 1), GameDefines.textColorDackGray);
													 }
												 }

												 StartCoroutine("PlayShowRewardAnimation");
											 });
		}
		//炼十次
		else
		{
			decomposeLabel.Text = GameUtility.GetUIString("UIPnlDanFurnace_Btn_Decompose_Label");

			freeCountLabel.Text = "";
			costCountLabel.Text = "";
			bossCountLabel.Text = "";

			//转轮位置初始化
			for (int i = 0; i < cores.Length; i++)
			{
				float x = Mathf.Sin(i * ((2 * pI) / cores.Length)) * 120;
				float y = Mathf.Cos(i * ((2 * pI) / cores.Length)) * 120;

				cores[i].transform.localPosition = new Vector3(x, y, -0.01f);
			}

			int index = 0;

			List<GameObject> daneffects = new List<GameObject>();
	
			for (int i = 0; i < danReward.Dan.Count; i++)
			{				
				yield return new WaitForSeconds(0.15f);

				GameObject danEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danDanEffect));

				daneffects.Add(danEffect);

				if (danEffect != null)
					ObjectUtility.AttachToParentAndResetLocalPosAndRotation(effectRoots, danEffect);
				danEffect.transform.position = uIFxBatchDanTrans.position;

				for (int j = 0; j < daneffects.Count; j ++)
				{
					GameObject.Destroy(daneffects[j].gameObject, 0.3f);
				}

				AudioManager.Instance.PlaySound(GameDefines.menu_Blade2, 0f);

				bool isplay = false;

				AnimatePosition.Do(danEffect,
												EZAnimation.ANIM_MODE.FromTo,
												danEffect.transform.localPosition,
												cores[i].transform.localPosition,					
												EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
												0.3f,
												0f,
												null,
												 (data) =>
												 {												
													 if(daneffects.Count > 0)
													 {
														 daneffects.Remove(daneffects[0]);
													 }													
													
													 if(index < cores.Length)
													 {
														 GameObject danGetEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danGetEffect));

														 if (danGetEffect != null)
															 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(cores[index].border.gameObject, danGetEffect);

														 cores[index].Hide(false);
														 cores[index].SetData(danReward.Dan[index].ResourceId, danReward.Dan[index].BreakthoughtLevel, danReward.Dan[index].LevelAttrib.Level);
														 cores[index].Data = danReward.Dan[index];

														 PlayQualityAnimation(cores[index].gameObject, danReward.Dan[index].BreakthoughtLevel);
													 }

													 index++;

													 if (index > 9 && !isplay)
													 {
														 StartCoroutine("PlayShowRewardAnimation");
														 isplay = true;
													 }
												 });
			}
		}
	}

	private void DestoryRewardParticle()
	{
		foreach (GameObject particle in particleList)
		{
			GameObject.Destroy(particle);
		}

		particleList.Clear();
	}

	private void PlayQualityAnimation(GameObject asseticon, int quality)
	{
		switch(quality)
		{
			case 4://紫丹已练出特效
				GameObject danShinning4 = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconShinning_S4ZiSe));
				ObjectUtility.AttachToParentAndResetLocalTrans(asseticon, danShinning4);
				danShinning4.transform.localPosition = resetZVec3;
				particleList.Add(danShinning4);
				break;

			case 5://橙丹已练出特效
				GameObject danShinning5 = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconShinning_S5ChengSe));
				ObjectUtility.AttachToParentAndResetLocalTrans(asseticon, danShinning5);
				danShinning5.transform.localPosition = resetZVec3;
				particleList.Add(danShinning5);
				break;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillExtraRewardList(KodGames.ClientClass.Reward extraRewards)
	{
		yield return null;

		var rewards = SysLocalDataBase.CCRewardListToShowReward(extraRewards);
		foreach (var reward in rewards)
		{
			var item = extrarRewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);
			extraRewardList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayShowRewardAnimation()
	{
		bool isExtraReward = SysLocalDataBase.CCRewardListToShowReward(this.extraReward).Count > 0;

		if (isExtraReward)
		{
			yield return new WaitForSeconds(0.5f);
			extraObj.SetActive(true);
			ClearData();
			//新面板操作
			StartCoroutine("FillExtraRewardList", this.extraReward);
			Animation extrAni = extraObj.GetComponent<Animation>();
			if (extrAni != null)
				extrAni.Play();
		}
		else extraObj.SetActive(false);

		yield return new WaitForSeconds(0.8f);
		
		btnRoot.SetActive(true);	
	
		//如果没有额外奖励，并且是单抽
		if (!isExtraReward && chatType == DanConfig._OperateType.Alchemy)
			btnRoot.transform.localPosition = alchemyBtnTrans.localPosition;
		else
			btnRoot.transform.localPosition = batchBtnTrans.localPosition;

		btnAni = btnRoot.GetComponent<Animation>();
		if (btnAni != null)
			btnAni.Play();

		isAniPlayed = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().IsDanGet = true;
		HideSelf();
		GameUtility.JumpUIPanel(_UIType.UIPnlDanFurnace);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDecomposeTimes(UIButton btn)
	{				
		RequestMgr.Inst.Request(new QueryDanDecomposeReq(danReward.Dan));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDecompose(UIButton btn)
	{
		if (chatType == DanConfig._OperateType.Alchemy)
		{
			int bossItemCount = ItemInfoUtility.GetGameItemCount(decomposeInfo.decomposeCost.id);

			if (decomposeInfo.danDecomposeCout <= 0 && decomposeInfo.danItemDecomposeCount <= 0)
			{
				string message = GameUtility.GetUIString("UIDlgDanDecomposeSure_ItemCount_NotEnough");

				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), message);
				return;
			}

			if (decomposeInfo.danDecomposeCout <= 0 && decomposeInfo.decomposeCost.count > bossItemCount)
			{
				string message = string.Format(GameUtility.GetUIString("UIDlgDanDecomposeSure_ItemTips"), ItemInfoUtility.GetAssetName(decomposeInfo.decomposeCost.id), ItemInfoUtility.GetAssetName(decomposeInfo.decomposeCost.id));

				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), message);
				return;
			}

			List<string> guids = new List<string>();
			guids.Add(danReward.Dan[0].Guid);

			KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost();
			cost.Id = decomposeInfo.decomposeCost.id;
			cost.Count = decomposeInfo.decomposeCost.count;

			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanDecomposeSure), decomposeInfo.danDecomposeCout, decomposeInfo.danItemDecomposeCount, danReward.Dan, cost);
		}
		else
			RequestMgr.Inst.Request(new QueryDanDecomposeReq(danReward.Dan));
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardItem(UIButton btn)
	{
		if (isAniPlay)
			return;

		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showItem = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		if (IDSeg.ToAssetType(showItem.id) == IDSeg._AssetType.Dan)
		{
			foreach (var reward in extraReward.Dan)
			{
				if(reward.ResourceId == showItem.id)
				{
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanInfo), reward);
				}
			}						
		}
		else
		{
			GameUtility.ShowAssetInfoUI(showItem, _UILayer.Top);
		}	
	}
	
	//点击内丹
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDanIconClick(UIButton btn)
	{
		if (isAniPlay)
			return;

		UIElemAssetIcon danIcon = btn.Data as UIElemAssetIcon;
		KodGames.ClientClass.Dan dan = danIcon.Data as KodGames.ClientClass.Dan;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanInfo), dan);
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanMenuBot));
	}
}
