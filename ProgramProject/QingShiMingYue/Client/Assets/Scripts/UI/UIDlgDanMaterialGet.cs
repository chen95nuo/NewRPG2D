using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDanMaterialGet : UIModule
{
	public UIScrollList materialList;
	public GameObjectPool rewardIconPool;
	public GameObject windowRoot;
	public GameObject flyRoot;

	private KodGames.ClientClass.CostAndRewardAndSync materialReward;
	public KodGames.ClientClass.CostAndRewardAndSync MaterialReward 
	{
		get { return materialReward; }
		set { materialReward = value; } 
	}

	private List<com.kodgames.corgi.protocol.ShowReward> showRewards;
	private int iconCount = 3;
	private int clickCount = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		showRewards = userDatas[0] as List<com.kodgames.corgi.protocol.ShowReward>;

		FillData();

		return true;
	}

	public override void OnHide()
	{
		materialReward = null;
		windowRoot.transform.localScale = new Vector3(1f, 1f, 1f);		

		base.OnHide();
	}

	private void FillData()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		clickCount = 0;
		materialList.ClearList(false);
		materialList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		for (int i = 0; i < showRewards.Count;)
		{
			List<com.kodgames.corgi.protocol.ShowReward> listRewards = new List<com.kodgames.corgi.protocol.ShowReward>();
			
			while(listRewards.Count < iconCount && i < showRewards.Count)
			{
				listRewards.Add(showRewards[i]);
				i++;
			}

			UIElemDanBoxReward boxItem = rewardIconPool.AllocateItem().GetComponent<UIElemDanBoxReward>();
			boxItem.SetData(listRewards);
			materialList.AddItem(boxItem.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAssetIconClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showReward = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		GameUtility.ShowAssetInfoUI(showReward, _UILayer.Top);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		if (clickCount==0)
			StartCoroutine("PlayStarAnimation");
		clickCount = 1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator PlayStarAnimation()
	{
		Animation alchAni = windowRoot.GetComponent<Animation>();
		if (alchAni != null)
			alchAni.Play();
		
		yield return new WaitForSeconds(0.3f);

		GameObject creatEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danFlyEffect));
		if (creatEffect != null)
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(flyRoot.gameObject, creatEffect);

		yield return new WaitForSeconds(1.0f);
		AudioManager.Instance.PlaySound(GameDefines.menu_Blade2, 0f);

		AnimatePosition.Do(creatEffect,
								EZAnimation.ANIM_MODE.FromTo,
								creatEffect.transform.localPosition,
								SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().GetSceneItemPosition(_UIType.UIPnlDanMaterial),
								EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
								0.8f,
								0f,
								null,
								 (data) =>
								 {
									 GameObject danGetEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danGetEffect));
									 if (danGetEffect != null)
										 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(flyRoot.gameObject, danGetEffect);
									 danGetEffect.transform.localPosition = creatEffect.transform.localPosition;

									 GameObject.Destroy(creatEffect.gameObject);
									 AudioManager.Instance.PlaySound(GameDefines.money, 0f);

									 StartCoroutine("PlayGetItem");						
								 });
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator PlayGetItem()
	{
		yield return new WaitForSeconds(0.8f);
		HideSelf();
	}
}
