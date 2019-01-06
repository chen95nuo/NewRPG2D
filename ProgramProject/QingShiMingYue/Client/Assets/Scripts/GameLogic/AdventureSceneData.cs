using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using KodGames;

public class AdventureSceneData : MonoBehaviour
{
	private static AdventureSceneData instance = null;

	public static AdventureSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(AdventureSceneData)) as AdventureSceneData;

			return instance;
		}
	}

	public List<GameObject> eventGameObjects;
	public GameObject birdObj;
	public GameObject birdRoot;
	public GameObject trialPoint;

	public static bool isClick = true;	//机关鸟飞行过程中不让点击
	public List<com.kodgames.corgi.protocol.DelayReward> getDelayRewards;

	public static com.kodgames.corgi.protocol.MarvellousProto combatMarvellouseProto;
	public static List<com.kodgames.corgi.protocol.DelayReward> delayRewardsConst;
	public static List<Pair<int, int>> fixRewardPackagePars;
	public static List<Pair<int, int>> randRewardPackagePars;

	private GameObject selectEff;
	private Vector3 birdPosi = Vector3.zero;
	private int hadDelayReward = 0;
	public int HadDelayReward
	{
		get { return hadDelayReward; }
	}

	private float delta = 0f;

	//机关鸟高度
	private const int birdHeight = 5;
	//机关鸟初始位置
	public Transform birdTrans;

	void Start()
	{
		isClick = true;
	}

	void Update()
	{
		delta += Time.deltaTime;

		//延时奖励时间更新
		if (delta > 1)
		{
			if (getDelayRewards != null && hadDelayReward == 0)
			{
				for (int i = 0; i < getDelayRewards.Count; i++)
				{
					if (getDelayRewards[i].couldPickTime <= SysLocalDataBase.Inst.LoginInfo.NowTime)
					{
						hadDelayReward++;
						break;
					}
				}
			}
		}
	}

	public void InitPoints()
	{
		for (int i = 0; i < eventGameObjects.Count; i++)
		{
			var points = ObjectUtility.FindChildObject(eventGameObjects[i].gameObject, "point_button");
			if (points != null)
				Object.DestroyImmediate(points.gameObject);
		}
	}

	public void CreateMissionEvent(List<int> points)
	{
		//初始化绑定按钮，防止按钮堆叠
		InitPoints();

		bool isFly = false;

		// 触发点布置
		if (points.Count > eventGameObjects.Count)
		{
			Debug.Log("Points Count Out Of Range");
			return;
		}
		else
		{
			for (int i = 0; i < points.Count; i++)
			{
				if (points[i] == MarvellousAdventureConfig._MarvellousZoneStaus.NotVisit)
				{
					GameObject click_button = new GameObject("point_button");
					UIButton3D button3D = click_button.AddComponent<UIButton3D>();
					//UIPnlAdventureScene nextEvent = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlAdventureScene>();
					button3D.scriptWithMethodToInvoke = this;
					button3D.methodToInvoke = "OnClickNextEvent";
					button3D.Data = i;

					click_button.gameObject.layer = GameDefines.UISceneRaycastLayer;
					ObjectUtility.AttachToParentAndResetLocalPosAndRotation(eventGameObjects[i].gameObject, click_button);
					click_button.AddComponent<BoxCollider>().size = new Vector3(3, 3, 3);
					GameObject missionPreb = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.pfxPath, GameDefines.p_Q_GGYD_Mission));
					ObjectUtility.AttachToParentAndResetLocalPosAndRotation(click_button.gameObject, missionPreb);
				}
				else if (points[i] == MarvellousAdventureConfig._MarvellousZoneStaus.Visiting)
				{
					Vector3 birdPosi = eventGameObjects[i].gameObject.transform.position;
					birdPosi.y += birdHeight;
					birdRoot.gameObject.transform.position = birdPosi;
					isFly = true;
				}
			}
			if (!isFly)
			{
				birdRoot.transform.position = birdTrans.position;
				trialPoint.transform.localPosition = new Vector3(0, 0, 0);
				trialPoint.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			}
		}
	}

	public void BirdFly(int nextZoneId)
	{
		if (selectEff != null)
			DestroyObject(selectEff);
		isClick = false;
		birdPosi = eventGameObjects[nextZoneId].gameObject.transform.position;

		trialPoint.transform.LookAt(birdPosi, Vector3.up);
		AnimateRotation.Do(birdRoot.gameObject, EZAnimation.ANIM_MODE.FromTo, birdRoot.transform.rotation.eulerAngles, new Vector3(0, trialPoint.transform.rotation.eulerAngles.y, 0),
		   AnimateRotation.GetInterpolator(EZAnimation.EASING_TYPE.BackIn), 0.6f, 0f, null, (data) =>
		   {
			   AnimatePosition.Do(birdRoot.gameObject,
				   EZAnimation.ANIM_MODE.FromTo,
				   birdRoot.transform.position, new Vector3(birdPosi.x, birdRoot.transform.position.y, birdPosi.z),
				   AnimatePosition.GetInterpolator(EZAnimation.EASING_TYPE.BackIn),
				   2f,
				   0f,
				   null,
					(enter) =>
					{
						selectEff = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.pfxPath, GameDefines.p_Q_GGYD_Select));
						selectEff.transform.position = birdPosi;
						selectEff.transform.rotation = Quaternion.identity;

						StartCoroutine("SendRequest", nextZoneId);
					});
		   });
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator SendRequest(int nextZoneId)
	{
		yield return new WaitForSeconds(1.6f);//播放完粒子特效执行发布协议，由于粒子特效lift=3，数值太大，停留时间太长，所以写死1.6秒
		var points = ObjectUtility.FindChildObject(eventGameObjects[nextZoneId].gameObject, "point_button");
		if (points != null)
			points.gameObject.SetActive(false);
		//Object.DestroyImmediate(points.gameObject);
		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(0, nextZoneId, null));
	}

	//执行奇遇
	public void GetAdventureTypeByEventId(com.kodgames.corgi.protocol.MarvellousProto marvellousProto)
	{
		if (marvellousProto == null) return;
		//根据事件ID得到对话信息
		int currentEventId = marvellousProto.currentEventId;
		MarvellousAdventureConfig marvellousAdventureConfig = ConfigDatabase.DefaultCfg.MarvellousAdventureConfig;
		int id = marvellousAdventureConfig.GetEventTypeById(currentEventId);
		switch (id)
		{
			case MarvellousAdventureConfig._MarvellousType.DialogTypeEvent:	//消息类型
				var dialogEvent = marvellousAdventureConfig.GetEventById(currentEventId) as MarvellousAdventureConfig.DialogEvent;
				if (dialogEvent.DialogSetId != 0 && ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogEvent.DialogSetId) != null)
				{
					UpdateAdventureMessage(true, dialogEvent.DialogSetId);
					SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
				}
				break;
			case MarvellousAdventureConfig._MarvellousType.BuyTypeEvent:	//购买类型
				var buyEvent = marvellousAdventureConfig.GetEventById(currentEventId) as MarvellousAdventureConfig.BuyEvent;
				if (buyEvent.DialogSetId != 0 && ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(buyEvent.DialogSetId) != null)
					UpdateAdventureMessage(false, buyEvent.DialogSetId);
				SysUIEnv.Instance.ShowUIModule<UIPnlAdventureBuyReward>(buyEvent);
				SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
				break;
			case MarvellousAdventureConfig._MarvellousType.CombatTypeEvent://战斗类型
				var combatEvent = marvellousAdventureConfig.GetEventById(currentEventId) as MarvellousAdventureConfig.CombatEvent;
				if (combatEvent.DialogSetId != 0 && ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(combatEvent.DialogSetId) != null)
					UpdateAdventureMessage(false, combatEvent.DialogSetId);

				SysUIEnv.Instance.ShowUIModule<UIPnlAdventureCombat>(combatEvent);
				SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
				break;
			case MarvellousAdventureConfig._MarvellousType.RewardTypeEvent://奖励类型

				break;
			case MarvellousAdventureConfig._MarvellousType.SelectionTypeEvent://选项类型 ABCD
				var selectEvent = marvellousAdventureConfig.GetEventById(currentEventId) as MarvellousAdventureConfig.SelectionEvent;
				SysUIEnv.Instance.ShowUIModule<UIPnlAdventureQuestionAnswer>(selectEvent);

				if (selectEvent.DialogSetId != 0 && ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(selectEvent.DialogSetId) != null)
					UpdateAdventureMessage(false, selectEvent.DialogSetId);

				SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
				break;
			default:
				UpdateAdventureMessage(false, 0);
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAdventureScene), marvellousProto);
				break;
		}
		CreateMissionEvent(marvellousProto.visitZones);
	}

	private void UpdateAdventureMessage(bool isDialogType, int dialogSetId)
	{
		if (dialogSetId != 0 && ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId) != null)
		{
			if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().IsShown)
				SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().SetData(isDialogType, dialogSetId);
			else
				SysUIEnv.Instance.ShowUIModule<UIPnlAdventureMessage>(isDialogType, dialogSetId);
		}
		else
		{
			if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().IsShown)
				SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().OnHide();
		}

	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickNextEvent(UIButton3D btn)
	{
		if (!isClick) return;
		if (SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value <= 0)
		{
			GameUtility.ShowNotEnoughtAssetUI(IDSeg._SpecialId.Energy, 0);
			return;
		}
		BirdFly((int)btn.Data);
	}
}