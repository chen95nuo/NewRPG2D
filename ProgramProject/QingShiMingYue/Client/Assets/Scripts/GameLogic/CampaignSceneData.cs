using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class CampaignSceneData : MonoBehaviour
{
	// Scene Data.
	[System.Serializable]
	public class CampaignBaseInfo
	{
		private Transform scrollTrans;
		public Transform ScrollTrans
		{
			get
			{
				if (scrollTrans == null)
					scrollTrans = campaignModelOpenRoot.transform;

				return scrollTrans;
			}
		}

		public List<UIListButton3D> campaignInvokeButtons;
		public GameObject campaignNameRoot;
		public GameObject campaignModelOpenRoot;
		public GameObject campaignModelCloseRoot;

		private GameObject campaignNewParticle;
		public GameObject CampaignNewParticle
		{
			get { return campaignNewParticle; }
			set { campaignNewParticle = value; }
		}

		private object data;
		public object Data
		{
			get { return data; }
			set { data = value; }
		}

		public void SetCampaignInvokeMethond(MonoBehaviour scripte, string invokeMethod, object zone, UIScroller scroller)
		{
			if (campaignInvokeButtons == null || campaignInvokeButtons.Count <= 0)
				return;

			foreach (var button in campaignInvokeButtons)
			{
				button.scriptWithMethodToInvoke = scripte;
				button.methodToInvoke = invokeMethod;
				button.SetList(scroller);
				button.Data = zone;
			}

			SetCampaignData(zone);
		}

		public void SetCampaignData(object zone)
		{
			this.data = zone;
		}
	}

	private class EasingData
	{
		public float timer;
		public float orignal;
		public float tmepData;
	}

	public GameObject campaignLock;
	public UIScroller scroller;
	public GameObject cameraRoot;
	public GameObject skyRoot;
	public GameObject rockRoot;
	public float scrollTimeDeta = 0.005f;
	public float scrollDistanceDelta = 0.005f;
	public float skyMovingSpeed;
	public float rockMovingSpeed;
	public List<CampaignBaseInfo> campaignInfos;
	public List<GameObject> campaignLines;

	private Camera mainCamera;
	public Camera MainCamera
	{
		get
		{
			if (mainCamera == null)
				mainCamera = KodGames.Camera.main;

			return mainCamera;
		}
	}

	private Transform mainTrans;
	public Transform MainTrans
	{
		get
		{
			if (mainTrans == null)
				mainTrans = MainCamera.transform;

			return mainTrans;
		}
	}

	private Transform skyTrans;
	public Transform SkyTrans
	{
		get
		{
			if (skyTrans == null)
				skyTrans = skyRoot.transform;

			return skyTrans;
		}
	}

	private Transform rockTrans;
	public Transform RockTrans
	{
		get
		{
			if (rockTrans == null)
				rockTrans = rockRoot.transform;

			return rockTrans;
		}
	}

	private static CampaignSceneData instance = null;
	public static CampaignSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(CampaignSceneData)) as CampaignSceneData;
			return instance;
		}
	}

	public delegate void OnActionFinishDel();
	private OnActionFinishDel scrollFinishDel;
	private Vector2 oldScrollValue;
	private EasingData easingData;

	private MonoBehaviour buttonScript;
	private string methodToInvoke;

	// Scroller 的原始状态
	private bool scrollState;

	// 解锁时线段的动画时长
	private float lineAnimaTime;

	private int currentIndex = 0;
	public int CurrentIndex { get { return currentIndex; } }

	public void Awake()
	{
		var zoneLineObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, GameDefines.campaignZoneLine));
		var lineAnim = zoneLineObj.GetComponentInChildren<Animation>();
		lineAnimaTime = lineAnim.GetClip("BIGMAP_OBJ_11_02").length;

		GameObject.DestroyImmediate(zoneLineObj);
	}

	public void InitData(MonoBehaviour buttonScript, string methodToInvoke)
	{
		this.buttonScript = buttonScript;
		this.methodToInvoke = methodToInvoke;
	}

	public void SetCampaignLocks(int campaignInfoIndex, bool hasLock, bool showDestoryAnima)
	{
		SetCampaignLocks(campaignInfoIndex, hasLock, showDestoryAnima, null);
	}

	public void SetCampaignLocks(int campaignInfoIndex, bool hasLock, bool showDestoryAnima, OnActionFinishDel showDestoryAnimaDel)
	{
		var campaignInfo = campaignInfos[campaignInfoIndex];

		if (hasLock && campaignInfo.campaignModelCloseRoot.transform.childCount > 0)
			return;

		if (!hasLock && campaignInfo.campaignModelOpenRoot.transform.childCount > 0)
			return;

		var zoneCfg = campaignInfo.Data as CampaignConfig.Zone;
		var modelObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, (hasLock ? zoneCfg.zoneModelClose.modelName : zoneCfg.zoneModelOpen.modelName)));
		var aniamtions = modelObj.GetComponentsInChildren<Animation>();

		// Model Line.
		GameObject zoneLineObj = null;
		Animation lineAnim = null;
		if (!hasLock && campaignInfoIndex > 0 && campaignInfoIndex < campaignInfos.Count && campaignLines.Count > 0)
		{
			zoneLineObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, GameDefines.campaignZoneLine));
			lineAnim = zoneLineObj.GetComponentInChildren<Animation>();
			ObjectUtility.AttachToParentAndResetLocalTrans(campaignLines[campaignInfoIndex - 1], zoneLineObj);
		}

		if (hasLock)
		{
			// Attach Close Model.
			ObjectUtility.AttachToParentAndResetLocalTrans(campaignInfo.campaignModelCloseRoot, modelObj);

			// Play Line Idle Animation.
			if (lineAnim != null)
				lineAnim.Play("BIGMAP_OBJ_11_02", PlayMode.StopAll);

			// Play Model Idle Animation.
			PlayAnimation(aniamtions, zoneCfg.zoneModelClose.modelIdleAnim, true);
		}
		else
		{
			if (showDestoryAnima)
			{
				var closeModel = campaignInfo.campaignModelCloseRoot.transform.GetChild(0);
				var closeModelEvent = closeModel.GetComponent<AnimationEventHandler>();

				var openModelEvent = modelObj.GetComponentInChildren<AnimationEventHandler>();
				openModelEvent.userEventDelegate =
					(eventName, userData) =>
					{
						campaignInfo.CampaignNewParticle = null;

						switch (eventName)
						{
							case "ShowOpenCampaignParticel":
								campaignInfo.CampaignNewParticle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.campaignUnLockParticle));
								ObjectUtility.AttachToParentAndKeepLocalTrans(campaignInfo.campaignModelOpenRoot, campaignInfo.CampaignNewParticle);
								break;

							case "ShowNewCampaignParticel":
								campaignInfo.CampaignNewParticle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.campaignNewPatricle));
								ObjectUtility.AttachToParentAndResetLocalTrans(campaignInfo.campaignModelOpenRoot, campaignInfo.CampaignNewParticle);
								break;

							case "PlayerIdleAnim":
								PlayAnimation(aniamtions, zoneCfg.zoneModelOpen.modelIdleAnim, true);
								if (showDestoryAnimaDel != null)
									showDestoryAnimaDel();
								break;
						}
					};

				closeModelEvent.userEventDelegate =
					(eventName, userData) =>
					{
						switch (eventName)
						{
							case "FinishParticle":
								campaignInfo.CampaignNewParticle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.campaignDispearPatricle));
								ObjectUtility.AttachToParentAndResetLocalTrans(campaignInfo.campaignModelCloseRoot, campaignInfo.CampaignNewParticle);
								break;

							case "FinishAnimation":
								ObjectUtility.AttachToParentAndResetLocalTrans(campaignInfo.campaignModelOpenRoot, modelObj);
								PlayAnimation(aniamtions, zoneCfg.zoneModelOpen.modelEffectAnim);
								GameObject.Destroy(closeModel.gameObject);
								break;

						}
					};

				var lineEvent = zoneLineObj.GetComponentInChildren<AnimationEventHandler>();
				lineEvent.userEventDelegate =
					(eventName, userData) =>
					{
						switch (eventName)
						{
							case "FinishLineAnimation":
								closeModelEvent.gameObject.animation.Play();
								break;
						}
					};

				if (lineAnim != null)
					lineAnim.Play("BIGMAP_OBJ_11", PlayMode.StopAll);
			}
			else
			{
				// Destroy Close Model.
				if (campaignInfo.campaignModelCloseRoot.transform.childCount > 0)
					GameObject.Destroy(campaignInfo.campaignModelCloseRoot.transform.GetChild(0).gameObject);

				// Attach Line.
				if (lineAnim != null)
					lineAnim.Play("BIGMAP_OBJ_11_02", PlayMode.StopAll);

				// Attach Open Model.
				ObjectUtility.AttachToParentAndResetLocalTrans(campaignInfo.campaignModelOpenRoot, modelObj);

				// Play Idle Animation.
				PlayAnimation(aniamtions, zoneCfg.zoneModelOpen.modelIdleAnim, true);
			}
		}

		if (modelObj != null)
		{
			var invokeObjs = modelObj.GetComponentsInChildren<UIListButton3D>();
			campaignInfo.campaignInvokeButtons.Clear();
			for (int i = 0; i < invokeObjs.Length; i++)
			{
				invokeObjs[i].scriptWithMethodToInvoke = this.buttonScript;
				invokeObjs[i].methodToInvoke = this.methodToInvoke;
				invokeObjs[i].Data = campaignInfo.Data;
				invokeObjs[i].SetList(scroller);
				campaignInfo.campaignInvokeButtons.Add(invokeObjs[i]);
			}
		}
	}

	private void PlayAnimation(Animation[] animations, string animationName)
	{
		PlayAnimation(animations, animationName, false);
	}

	private void PlayAnimation(Animation[] animations, string animationName, bool loop)
	{
		if (animations == null || animations.Length <= 0 || string.IsNullOrEmpty(animationName))
			return;

		for (int i = 0; i < animations.Length; i++)
		{
			if (animations[i].name.Equals(animationName))
			{
				animations[i].wrapMode = loop ? WrapMode.Loop : WrapMode.Once;
				animations[i].Play(animationName, PlayMode.StopAll);

				break;
			}

			if (animations[i].GetClip(animationName) != null)
			{
				animations[i].wrapMode = loop ? WrapMode.Loop : WrapMode.Once;
				animations[i].Play(animationName, PlayMode.StopAll);
				break;
			}
		}
	}

	public void SetCampaigNewParticle(int campaignInfoIndex)
	{
		var info = campaignInfos[campaignInfoIndex];
		int zoneStatus = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone((info.Data as ClientServerCommon.CampaignConfig.Zone).zoneId).Status;
		bool isNewCampaign = zoneStatus == ClientServerCommon._ZoneStatus.PlotDialogue;

		if (isNewCampaign)
		{
			if (info.CampaignNewParticle == null)
			{
				info.CampaignNewParticle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.campaignNewPatricle));
				ObjectUtility.AttachToParentAndKeepLocalTrans(info.campaignInvokeButtons[0].gameObject, info.CampaignNewParticle);
			}
		}
		else
		{
			if (info.CampaignNewParticle != null)
			{
				GameObject.Destroy(info.CampaignNewParticle);
			}
		}
	}

	public void LockScroll()
	{
		this.scrollState = this.scroller.TouchScroll;
		this.scroller.TouchScroll = false;
	}

	public void SetScrollState(bool state)
	{
		this.scrollState = state;
	}

	public void UnLockScroll()
	{
		this.scroller.TouchScroll = this.scrollState;
	}

	public void ScrollView(int targetIndex, int startIndex, bool scroll, OnActionFinishDel scrollFinishDel)
	{
		ScrollView(campaignInfos[targetIndex], campaignInfos[startIndex], scroll, -1, scrollFinishDel);
	}

	public void ScrollView(int targetIndex, int startIndex, bool scroll, float duration, OnActionFinishDel scrollFinishDel)
	{
		this.currentIndex = targetIndex;
		ScrollView(campaignInfos[targetIndex], campaignInfos[startIndex], scroll, duration, scrollFinishDel);
	}

	private void ScrollView(CampaignBaseInfo targetInfo, CampaignBaseInfo startInfo, bool scroll, float duration, OnActionFinishDel scrollFinishDel)
	{
		this.scrollFinishDel = scrollFinishDel;

		float startX = campaignInfos[0].ScrollTrans.position.x;
		float distance = campaignInfos[campaignInfos.Count - 1].ScrollTrans.position.x - startX;
		ScrollToPosition((targetInfo.ScrollTrans.position.x - startX) / distance, (startInfo.ScrollTrans.position.x - startX) / distance, duration, scroll);
	}

	private void ScrollToPosition(float distance, float start, float duration, bool scroll)
	{
		if (scroll)
		{
			scrollDistanceDelta = (distance - start) / (duration > 0 ? duration : lineAnimaTime);
			easingData = new EasingData();
			easingData.timer = 0;
			easingData.orignal = distance;
			easingData.tmepData = start;
		}
		else
		{
			UpdateScrollerPos(distance);

			if (scrollFinishDel != null)
				scrollFinishDel();
		}
	}

	public virtual void Update()
	{
		if (scroller == null || !MainCamera.enabled)
			return;

		// Scroll Ball
		float positionX = KodGames.Math.LerpWithoutClamp(scroller.MinValue.x, scroller.MaxValue.x, scroller.ScrollPosition.x);
		MainTrans.position = new Vector3(positionX, MainTrans.position.y, MainTrans.position.z);

		if (skyRoot != null)
			SkyTrans.position = new Vector3((scroller.MaxValue.x - scroller.MinValue.x) * scroller.ScrollPosition.x * skyMovingSpeed, skyTrans.position.y, skyTrans.position.z);

		if (rockRoot != null)
			RockTrans.position = new Vector3((scroller.MaxValue.x - scroller.MinValue.x) * scroller.ScrollPosition.x * rockMovingSpeed, skyTrans.position.y, skyTrans.position.z);

		// Auto Scroll Ball.
		if (easingData == null)
			return;

		if (easingData.tmepData == easingData.orignal)
		{
			easingData = null;

			if (scrollFinishDel != null)
				scrollFinishDel();

			return;
		}

		easingData.timer += Time.deltaTime;

		if (easingData.timer >= scrollTimeDeta)
		{
			easingData.tmepData = Math.Min(easingData.tmepData + scrollDistanceDelta, easingData.orignal);
			UpdateScrollerPos(easingData.tmepData);
			easingData.timer = 0f;
		}
	}

	private void UpdateScrollerPos(float distance)
	{
		float scrollX = KodGames.Math.LerpWithoutClamp(scroller.MinValue.x, scroller.MaxValue.x, distance);
		scroller.ScrollPosition = new Vector2(LerpScrollValueReverse(scroller.MinValue.x, scroller.MaxValue.x, scrollX), scroller.ScrollPosition.y);
	}

	private float LerpScrollValueReverse(float from, float to, float value)
	{
		return (value - from) / (to - from);
	}
}
