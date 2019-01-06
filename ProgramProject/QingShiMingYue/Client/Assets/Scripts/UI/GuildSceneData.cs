using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class GuildSceneData : MonoBehaviour
{
	public GameObject roadRoot;
	public GameObject positionRoot;
	public GameObject cameraRoot;
	public UIScroller scroller;
	public int playerSpeed;
	public float distiance;
	public float scrollerPerDelta;
	public Vector2 scrollerFactor;

	private static GuildSceneData instance = null;
	public static GuildSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(GuildSceneData)) as GuildSceneData;

			return instance;
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

	private UnityEngine.Camera mainCamera;
	public UnityEngine.Camera MainCamera
	{
		get
		{
			if (mainCamera == null)
				mainCamera = KodGames.Camera.main;

			return mainCamera;
		}
	}

	private bool isMoving = false;
	public bool IsMoving { get { return isMoving; } }

	private bool isCameraMoving = false;
	public bool IsCameraMoving { get { return isCameraMoving; } }

	private bool isCameraFar = false;
	public bool IsCameraFar { get { return isCameraFar; } }

	private Vector3 zeroPointCamPos;

	private List<com.kodgames.corgi.protocol.Stage> roads = new List<com.kodgames.corgi.protocol.Stage>();
	private List<com.kodgames.corgi.protocol.Stage> points = new List<com.kodgames.corgi.protocol.Stage>();
	private int targetIndex;

	private int playerIndex;
	public int PlayerIndex
	{
		get { return playerIndex; }
	}

	private BattleRole playerRole;
	private Vector3 moveDirection = new Vector3(1f, 0f, 0f);
	private List<int> pathIndexs = new List<int>();
	private int playerMoveNode;

	private Dictionary<int, GameObject> pointDics = new Dictionary<int, GameObject>();
	private Dictionary<int, GameObject> roadDics = new Dictionary<int, GameObject>();

	//用于记录可通过和不可通过的点
	private Dictionary<int, com.kodgames.corgi.protocol.Stage> unTrafficDics = new Dictionary<int, com.kodgames.corgi.protocol.Stage>();

	//超过这个格子数量的移动方式为传送
	private int moveCount = 5;

	private string preName;
	private string roadPreName;
	private int maxRows = 100;

	//根号系数
	private float radicalNum2 = 1.4f;

	//未探索点哈希表
	private HashSet<int> unKnowPoints;

	// Scroll Value.
	private Vector2 oldScrollValue;
	private Vector2 scrollDelta;

	//探索返回数据
	private int mapNum;
	private KodGames.ClientClass.StageInfo stageInfo;
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private int operateType;

	private bool isNotCompleteBoss = false;

	private void Awake()
	{
		this.zeroPointCamPos = cameraRoot.transform.position;
	}

	#region Update

	private void Update()
	{
		// Scroll Map.
		var scrollValue = scroller.Value;
		if (Mathf.Approximately(scrollValue.x, oldScrollValue.x) == false || Mathf.Approximately(scrollValue.y, oldScrollValue.y) == false)
		{
			var y = KodGames.Math.LerpWithoutClamp(scroller.MinValue.y, scroller.MaxValue.y, scroller.ScrollPosition.x);
			var x = KodGames.Math.LerpWithoutClamp(scroller.MinValue.x, scroller.MaxValue.x, scroller.ScrollPosition.y);
			var xDelta = (scroller.MaxValue - scroller.MinValue).x / (scrollDelta.x * distiance);
			var yDelta = (scroller.MaxValue - scroller.MinValue).y / (scrollDelta.y * distiance);

			MainTrans.position = new Vector3(zeroPointCamPos.x + (x / xDelta + y / yDelta), MainTrans.position.y, zeroPointCamPos.z + (-x / xDelta + y / yDelta));

			oldScrollValue = scrollValue;
		}

		if (isMoving)
			UpdateMovement();
	}

	private void UpdateMovement()
	{
		//BugFix 设备上切场景时若Unity卡住则Time.deltaTime会很大，人物会跑出地图
		float deltaTime = Mathf.Min(0.033f, Time.deltaTime);

		moveDirection = GetPositionByIndex(pathIndexs[playerMoveNode]) - GetPositionByIndex(pathIndexs[playerMoveNode - 1]);
		playerRole.gameObject.transform.forward = moveDirection;
		SysLocalDataBase.Inst.LocalPlayer.DirectionX = moveDirection.x;
		SysLocalDataBase.Inst.LocalPlayer.DirectionZ = moveDirection.z;

		playerRole.gameObject.transform.Translate(moveDirection.normalized * deltaTime * playerSpeed, Space.World);
		cameraRoot.transform.Translate(moveDirection.normalized * deltaTime * playerSpeed, Space.World);

		Vector3 newPosi = playerRole.gameObject.transform.position;

		if (Vector3.Dot(moveDirection, GetPositionByIndex(pathIndexs[playerMoveNode]) - newPosi) <= 0)
		{
			//到达处理			
			playerMoveNode++;
			if (isNotCompleteBoss && playerMoveNode >= pathIndexs.Count - 1)
			{
				isMoving = false;

				if (pathIndexs.Count > 2)
					moveDirection = GetPositionByIndex(pathIndexs[playerMoveNode]) - GetPositionByIndex(pathIndexs[playerMoveNode - 1]);

				playerRole.gameObject.transform.forward = moveDirection;
				SysLocalDataBase.Inst.LocalPlayer.DirectionX = moveDirection.x;
				SysLocalDataBase.Inst.LocalPlayer.DirectionZ = moveDirection.z;
				playerIndex = pathIndexs[playerMoveNode - 1];
				int changeRoadIndex = (pathIndexs[pathIndexs.Count - 1] + pathIndexs[pathIndexs.Count - 2]) / 2;

				cameraRoot.transform.position = GetCameraPositionByIndex(playerIndex);
				playerRole.gameObject.transform.position = new Vector3(GetPositionByIndex(playerIndex).x, 3f, GetPositionByIndex(playerIndex).z);
				playerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.MelaleucaFloorIdle);

				ArrivePointChangeLight(pathIndexs[pathIndexs.Count - 1], changeRoadIndex);
				ArriveEvent();
			}
			else if (playerMoveNode >= pathIndexs.Count)
			{
				isMoving = false;
				playerIndex = pathIndexs[playerMoveNode - 1];
				int changeRoadIndex = (playerIndex + pathIndexs[playerMoveNode - 2]) / 2;

				cameraRoot.transform.position = GetCameraPositionByIndex(playerIndex);
				playerRole.gameObject.transform.position = new Vector3(GetPositionByIndex(playerIndex).x, 3f, GetPositionByIndex(playerIndex).z);
				playerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.MelaleucaFloorIdle);
				pathIndexs.Clear();

				ArrivePointChangeLight(playerIndex, changeRoadIndex);
				ArriveEvent();
			}
		}
	}

	#endregion

	#region CreatePoint

	public void CreatePoint(List<com.kodgames.corgi.protocol.Stage> stages, int playerIndex, string preName, string roadPreName, HashSet<int> unKnowPoints, int mapNum)
	{
		this.playerIndex = playerIndex;
		this.preName = preName;
		this.roadPreName = roadPreName;
		this.unKnowPoints = unKnowPoints;
		this.oldScrollValue = Vector2.zero;
		this.mapNum = mapNum;
		CreatePlayerAvatar();

		SetScrollerPosArea(stages);

		SetScrollPosition(playerIndex);

		for (int i = 0; i < stages.Count; i++)
		{
			int x = stages[i].index % maxRows;
			int z = stages[i].index / maxRows;

			GameObject stageObj = null;

			if (stages[i].type == GuildStageConfig._StageType.Road)
			{
				roads.Add(stages[i]);

				//未探索的点周围路应都为暗色，只遍历未探索点即可				
				if (unKnowPoints.Contains(stages[i].index + 1) || unKnowPoints.Contains(stages[i].index - 1) || unKnowPoints.Contains(stages[i].index + maxRows) || unKnowPoints.Contains(stages[i].index - maxRows))
					stageObj = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, roadPreName + GameDefines.ghmg_shadow));
				else
					stageObj = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, roadPreName));

				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(roadRoot, stageObj);
				stageObj.transform.localPosition = new Vector3(x * distiance, 0f, z * distiance);

				roadDics.Add(stages[i].index, stageObj);
			}
			else
			{
				points.Add(stages[i]);

				if (stages[i].status == GuildStageConfig._StageStatus.UnSearch)
					stageObj = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, preName + GameDefines.ghmg_shadow));
				else
					stageObj = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, preName));

				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(positionRoot, stageObj);
				stageObj.transform.localPosition = new Vector3(x * distiance, 0f, z * distiance);

				GameObject click_button = new GameObject("point_button");
				UIListButton3D button3D = click_button.AddComponent<UIListButton3D>();				
				button3D.scriptWithMethodToInvoke = this;
				button3D.methodToInvoke = "OnClickPointEvent";
				button3D.SetList(scroller);
				button3D.Data = stages[i].index;

				click_button.gameObject.layer = GameDefines.UISceneRaycastLayer;
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageObj, click_button);
				click_button.AddComponent<BoxCollider>().size = new Vector3(2f, 6.5f, 2f);

				pointDics.Add(stages[i].index, stageObj);

				if (stages[i].status != GuildStageConfig._StageStatus.Complete)
					unTrafficDics.Add(stages[i].index, stages[i]);

				if (stages[i].iconId != IDSeg.InvalidId)
				{
					UIElemGuildPointMonsterIcon icon = (ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiModulePath, GameDefines.monsterIcon))).GetComponent<UIElemGuildPointMonsterIcon>();
					icon.SetNumberPlate(stages[i].iconId);
					ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageObj, icon.gameObject);
					//BOSS死亡
					if (stages[i].type == GuildStageConfig._StageType.ChallengeBoss || stages[i].type == GuildStageConfig._StageType.PassBoss)
					{
						if (stages[i].status == GuildStageConfig._StageStatus.Complete)
							icon.SetDeadBoss();
						else if (stages[i].showCombatIcon)
							icon.SetBossBattle();
					}
					//小怪死亡
					if(stages[i].eventType == GuildStageConfig._EventType.HiddenEnemy)
					{
						if (stages[i].status == GuildStageConfig._StageStatus.Complete)
							icon.SetDeadBoss();
					}
				}
				else
				{
					if (stages[i].status == GuildStageConfig._StageStatus.UnSearch)
					{
						if (stages[i].type == GuildStageConfig._StageType.ChallengeBoss)
						{
							UIElemGuildPointMonsterIcon icon = (ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiModulePath, GameDefines.monsterIcon))).GetComponent<UIElemGuildPointMonsterIcon>();
							icon.SetNumberPlate(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.UnSearchChallengeBossIconId, stages[i].costMove);
							ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageObj, icon.gameObject);
						}
						else if (stages[i].type == GuildStageConfig._StageType.PassBoss)
						{
							UIElemGuildPointMonsterIcon icon = (ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiModulePath, GameDefines.monsterIcon))).GetComponent<UIElemGuildPointMonsterIcon>();
							icon.SetNumberPlate(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.UnSearchPassBossIconId, stages[i].costMove);
							ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageObj, icon.gameObject);
						}
						else
						{
							UIElemGuildPointMonsterIcon icon = (ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiModulePath, GameDefines.monsterIcon))).GetComponent<UIElemGuildPointMonsterIcon>();
							icon.SetNumberCost(stages[i].costMove);
							ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageObj, icon.gameObject);
						}
					}
				}
			}
		}
	}

	#endregion

	#region CreateMovePath
	//广域搜索路径（可能存在环路无法使用深度搜索）
	public List<List<int>> CreateMovePath(List<List<int>> moveRoads)
	{
		List<List<int>> nextMoveRoads = new List<List<int>>();

		//TODO不在遍历已经到头的节点
		foreach (var roadPaths in moveRoads)
		{
			for (int i = 0; i < roads.Count; i++)
			{
				int upon = roadPaths[roadPaths.Count - 1] - roads[i].index;

				if (Mathf.Abs(upon) == 1 || Mathf.Abs(upon) == maxRows)
				{
					int nextIndex = roadPaths[roadPaths.Count - 1] - 2 * upon;

					if (!roadPaths.Contains(nextIndex))
					{
						List<int> newRoadPaths = new List<int>();
						newRoadPaths.AddRange(roadPaths);
						newRoadPaths.Add(nextIndex);
						nextMoveRoads.Add(newRoadPaths);

						if (targetIndex == nextIndex)
						{
							//寻找到通向目标的通路之后删除之前无用的路径
							nextMoveRoads.Clear();
							nextMoveRoads.Add(newRoadPaths);

							return nextMoveRoads;
						}
					}
				}
			}
		}

		if (nextMoveRoads.Count > 0)
		{
			foreach (var list in moveRoads)
			{
				list.Clear();
			}

			moveRoads.Clear();

			return CreateMovePath(nextMoveRoads);
		}
		else return moveRoads;
	}

	private void CreatePlayerAvatar()
	{
		//场景中只存在一个角色跟敌人
		if (playerRole != null)
			Destroy(playerRole.gameObject);

		Vector3 avatarPos = GetPositionByIndex(playerIndex);
		avatarPos.y = 3f;

		//设置角色模型
		int positionId = SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId;
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(positionId);
		position.AvatarLocations.Sort((a1, a2) =>
		{
			return a1.ShowLocationId - a2.ShowLocationId;
		});

		if (position.AvatarLocations.Count <= 0)
		{
			Debug.LogError("No Avatar On Position.");
			return;
		}

		var avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(position.AvatarLocations[0].Guid);
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

		playerRole = CreateRole(avatarCfg.GetAvatarAssetId(avatar.BreakthoughtLevel), true, true);
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(positionRoot, playerRole.gameObject);
		playerRole.gameObject.transform.localPosition = avatarPos;
		moveDirection = new Vector3(SysLocalDataBase.Inst.LocalPlayer.DirectionX, 0f, SysLocalDataBase.Inst.LocalPlayer.DirectionZ);
		playerRole.gameObject.transform.forward = moveDirection;
	}

	private BattleRole CreateRole(int assetId, bool playDefaultAnim, bool useShadow)
	{
		Avatar avatar = Avatar.CreateAvatar(assetId);

		BattleRole newRole = avatar.GetComponent<BattleRole>();
		if (newRole == null)
			newRole = avatar.gameObject.AddComponent<BattleRole>();

		KodGames.ClientClass.CombatAvatarData avatarData = new KodGames.ClientClass.CombatAvatarData();
		avatarData.ResourceId = assetId;
		avatarData.DisplayName = name;
		avatarData.Attributes = new List<KodGames.ClientClass.Attribute>();

		KodGames.ClientClass.EquipmentData edata = new KodGames.ClientClass.EquipmentData();
		edata.BreakThrough = 0;
		edata.Id = IDSeg.InvalidId;

		newRole.AvatarData = avatarData;
		newRole.Awake();
		newRole.AvatarAssetId = assetId;
		newRole.Avatar.Load(assetId, playDefaultAnim, useShadow);

		return newRole;
	}

	#endregion

	#region UIScroller

	private float LerpScrollValueReverse(float from, float to, float value)
	{
		return (value - from) / (to - from);
	}

	private Vector3 GetCameraPositionByIndex(int index)
	{
		return new Vector3(zeroPointCamPos.x + (index % maxRows) * distiance, zeroPointCamPos.y, zeroPointCamPos.z + (index / maxRows) * distiance);
	}

	private void SetScrollPosition(int index)
	{
		var position = GetCameraPositionByIndex(index);

		var xDelta = (scroller.MaxValue - scroller.MinValue).x / (scrollDelta.x * distiance);
		var yDelta = (scroller.MaxValue - scroller.MinValue).y / (scrollDelta.y * distiance);

		scroller.ScrollPosition = new Vector2(
			LerpScrollValueReverse(scroller.MinValue.y, scroller.MaxValue.y, ((position - zeroPointCamPos).z + (position - zeroPointCamPos).x) * yDelta / 2),
			LerpScrollValueReverse(scroller.MinValue.x, scroller.MaxValue.x, (position.x - position.z - zeroPointCamPos.x + zeroPointCamPos.z) * xDelta / 2));
	}

	private void SetScrollerPosArea(List<com.kodgames.corgi.protocol.Stage> stages)
	{
		if (stages == null)
			return;

		foreach (var stage in stages)
		{
			if (stage.index % maxRows > scrollDelta.x)
				scrollDelta.x = stage.index % maxRows;

			if (stage.index / maxRows > scrollDelta.y)
				scrollDelta.y = stage.index / maxRows;
		}

		scroller.MinValue = new Vector2(-(scrollerPerDelta * scrollDelta.y) / radicalNum2, 0f);
		scroller.MaxValue = new Vector2(((scrollerPerDelta * scrollDelta.x) / radicalNum2), (scrollerPerDelta * scrollDelta.x) / radicalNum2 + (scrollerPerDelta * scrollDelta.y) / radicalNum2);
		scroller.ValueScrollFactor = new Vector2(scrollerFactor.x * scrollDelta.x, scrollerFactor.y * scrollDelta.y);
	}

	#endregion

	#region ChangeLight
	private void ArrivePointChangeLight(int playerIndex, int changeRoadIndex)
	{
		//柱子变亮
		if (pointDics.ContainsKey(playerIndex) && unKnowPoints.Contains(playerIndex))
		{
			Vector3 trans = new Vector3();

			if (pointDics[playerIndex] != null)
			{
				trans = pointDics[playerIndex].transform.localPosition;
				GameObject.Destroy(pointDics[playerIndex]);
			}

			pointDics[playerIndex] = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, preName));

			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(positionRoot, pointDics[playerIndex]);
			pointDics[playerIndex].transform.localPosition = trans;

			GameObject click_button = new GameObject("point_button");
			UIButton3D button3D = click_button.AddComponent<UIButton3D>();
			button3D.scriptWithMethodToInvoke = this;
			button3D.methodToInvoke = "OnClickPointEvent";
			button3D.Data = playerIndex;

			click_button.gameObject.layer = GameDefines.UISceneRaycastLayer;
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(pointDics[playerIndex], click_button);
			click_button.AddComponent<BoxCollider>().size = new Vector3(2f, 6.5f, 2f);

			//柱子上事件贴图
			if (stageInfo.IconId != IDSeg.InvalidId && stageInfo.EventType != GuildStageConfig._EventType.Enemy)
			{
				UIElemGuildPointMonsterIcon icon = (ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiModulePath, GameDefines.monsterIcon))).GetComponent<UIElemGuildPointMonsterIcon>();
				icon.SetNumberPlate(stageInfo.IconId);
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(pointDics[playerIndex], icon.gameObject);

				if (stageInfo.EventType == GuildStageConfig._EventType.ChallengeBoss || stageInfo.EventType == GuildStageConfig._EventType.PassBoss)
					if (stageInfo.Status == GuildStageConfig._StageStatus.Complete)
						icon.SetDeadBoss();

				if (stageInfo.EventType == GuildStageConfig._EventType.HiddenEnemy)
					if (stageInfo.Status == GuildStageConfig._StageStatus.Complete)
						icon.SetDeadBoss();
			}
		}
		//节点变亮，节点位置为相邻点，和除以2

		if (roadDics.ContainsKey(changeRoadIndex))
		{
			Vector3 trans = new Vector3();

			if (roadDics[changeRoadIndex] != null)
			{
				trans = roadDics[changeRoadIndex].transform.localPosition;
				GameObject.Destroy(roadDics[changeRoadIndex]);
			}

			roadDics[changeRoadIndex] = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, roadPreName));

			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(roadRoot, roadDics[changeRoadIndex]);
			roadDics[changeRoadIndex].transform.localPosition = trans;
		}
	}
	#endregion


	private void CameraMoveLine(Vector3 targetPosition)
	{
		isCameraMoving = true;
		scroller.TouchScroll = false;
		//先移动相机在传送
		if (cameraRoot.transform.position != GetCameraPositionByIndex(playerIndex))
		{
			Vector3 movePosition = GetCameraPositionByIndex(playerIndex);
			AnimatePosition.Do(cameraRoot,
					EZAnimation.ANIM_MODE.FromTo,
					cameraRoot.transform.position,
					movePosition,
					EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
					0.8f,
					0f,
					null,
					data =>
					{
						CameraMoveTransfer(targetPosition);
					});

		}
		else CameraMoveTransfer(targetPosition);
	}

	private void CameraMoveTransfer(Vector3 targetPosition)
	{
		if (playerRole != null)
		{
			GameObject moveEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.collisionQianJiLou));
			if (moveEffect != null)
				moveEffect.transform.position = playerRole.gameObject.transform.position;

			Destroy(playerRole.gameObject, 0.5f);
		}

		AnimatePosition.Do(cameraRoot,
		EZAnimation.ANIM_MODE.FromTo,
		cameraRoot.transform.position,
		targetPosition,
		EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
		0.8f,
		0f,
		null,
		data =>
		{
			isCameraMoving = false;
			scroller.TouchScroll = true;

			playerIndex = targetIndex;
			int changeRoadIndex = (playerIndex + pathIndexs[pathIndexs.Count - 2]) / 2;

			if (playerRole != null)
				Destroy(playerRole.gameObject);

			CreatePlayerAvatar();

			if (!isNotCompleteBoss)
				ArrivePointChangeLight(playerIndex, changeRoadIndex);
			else
			{
				changeRoadIndex = (pathIndexs[pathIndexs.Count - 1] + pathIndexs[pathIndexs.Count - 2]) / 2;
				ArrivePointChangeLight(pathIndexs[pathIndexs.Count - 1], changeRoadIndex);
			}

			ArriveEvent();
		});
	}

	private void CameraMoveFar(Vector3 targetPosition, bool isMove)
	{
		isCameraMoving = true;
		scroller.TouchScroll = false;

		AnimatePosition.Do(cameraRoot,
						EZAnimation.ANIM_MODE.FromTo,
						cameraRoot.transform.position,
						targetPosition,
						EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
						0.8f,
						0f,
						null,
						data =>
						{
							if (!isCameraFar)
								scroller.TouchScroll = true;

							isCameraMoving = false;
							isMoving = isMove;
							if (isMove)
								playerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.Run);
						});
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickPointEvent(UIButton3D btn)
	{
		if (isMoving || isCameraMoving)
			return;

		targetIndex = (int)btn.Data;

		if (isCameraFar)
		{
			scroller.TouchScroll = true;
			isCameraFar = false;
			Vector3 movePosition = GetCameraPositionByIndex(targetIndex);
			CameraMoveFar(movePosition, false);
			return;
		}

		if (playerIndex == targetIndex)
			return;

		playerMoveNode = 1;

		List<int> startPaths = new List<int>();
		List<List<int>> startPathRoots = new List<List<int>>();
		startPaths.Add(playerIndex);
		startPathRoots.Add(startPaths);

		foreach (var roadList in GuildSceneData.Instance.CreateMovePath(startPathRoots))
		{
			string msg = "";
			pathIndexs.Clear();
			foreach (var road in roadList)
			{
				msg = msg + road.ToString() + ",";
				pathIndexs.Add(road);
			}
		}

		//判断是否可以
		if (unTrafficDics.ContainsKey(pathIndexs[pathIndexs.Count - 2]))
		{
			CantMoveTips();
			return;
		}

		//探索协议
		RequestMgr.Inst.Request(new GuildStageExploreReq(pathIndexs[pathIndexs.Count - 2], targetIndex));
	}

	public void CantMoveTips()
	{
		// Show message dlg and reconnect
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");

		string tipsMessage = GameUtility.GetUIString("UIPnlGuildPointMain_MessageTips");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIPnlGuildPointMain_MessageTitle"),
			tipsMessage,
			false,
			null,
			okCallback);

		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}

	public void QueryStageExploreSuccess(KodGames.ClientClass.StageInfo stageInfo, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, int operateType, bool isCost)
	{
		this.stageInfo = stageInfo;
		this.combatResultAndReward = combatResultAndReward;
		this.operateType = operateType;

		isNotCompleteBoss = false;

		if (!isCost && unTrafficDics.ContainsKey(stageInfo.Index))
			if (unTrafficDics[stageInfo.Index].status == GuildStageConfig._StageStatus.UnSearch)
			{				
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildPointMain_AlreadyExplore"));
				unTrafficDics[stageInfo.Index].status = GuildStageConfig._StageStatus.Searching;
			}

		if (unTrafficDics.ContainsKey(stageInfo.Index))
			unTrafficDics[stageInfo.Index].status = GuildStageConfig._StageStatus.Searching;

		if (stageInfo.Status == GuildStageConfig._StageStatus.Complete)
		{
			if (unTrafficDics.ContainsKey(stageInfo.Index))
			{
				unTrafficDics.Remove(stageInfo.Index);
			}
		}

		if (stageInfo.EventType == GuildStageConfig._EventType.ChallengeBoss || stageInfo.EventType == GuildStageConfig._EventType.PassBoss)
		{
			if (stageInfo.Status != GuildStageConfig._StageStatus.Complete)
			{
				isNotCompleteBoss = true;
				AvatarRun();
			}
			else
				AvatarRun();
		}
		else
			AvatarRun();
	}

	public void AvatarRun()
	{
		if (pathIndexs.Count > moveCount)
		{
			if (isNotCompleteBoss)
				targetIndex = pathIndexs[pathIndexs.Count - 2];

			Vector3 movePosition = GetCameraPositionByIndex(targetIndex);
			//传送相机移动方式
			CameraMoveLine(movePosition);
		}
		else
		{
			if (pathIndexs.Count == 2 && isNotCompleteBoss)
			{
				if (playerRole != null)
				{
					moveDirection = GetPositionByIndex(pathIndexs[1]) - GetPositionByIndex(pathIndexs[0]);
					playerRole.gameObject.transform.forward = moveDirection;
					SysLocalDataBase.Inst.LocalPlayer.DirectionX = moveDirection.x;
					SysLocalDataBase.Inst.LocalPlayer.DirectionZ = moveDirection.z;
				}
				int changeRoadIndex = (pathIndexs[0] + pathIndexs[1]) / 2;
				ArrivePointChangeLight(pathIndexs[1], changeRoadIndex);
				ArriveEvent();
			}
			else if (cameraRoot.transform.position != GetCameraPositionByIndex(playerIndex))
			{
				Vector3 movePosition = GetCameraPositionByIndex(playerIndex);
				CameraMoveFar(movePosition, true);
			}
			else
			{
				isMoving = true;
				playerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.Run);
			}
		}
	}

	private void ArriveEvent()
	{
		switch (stageInfo.EventType)
		{
			//判断类型是否是赠送奖励类型
			case GuildStageConfig._EventType.Box:
				SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().UIInit();
				SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().UpdateMapExplore();
				if (operateType == GuildStageConfig._ExploreOperateType.Explore)
					SysUIEnv.Instance.ShowUIModule<UIEffectSchoolOpenBox>(stageInfo, true, true, false, false);
				else
					SysUIEnv.Instance.ShowUIModule<UIEffectSchoolOpenBox>(stageInfo, false, false, true, true);
				break;

			case GuildStageConfig._EventType.ChallengeBoss:
			case GuildStageConfig._EventType.PassBoss:
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGuildPointBossView), stageInfo, mapNum);
				break;

			case GuildStageConfig._EventType.Enemy:
			case GuildStageConfig._EventType.HiddenEnemy:
				if (stageInfo.Status != GuildStageConfig._StageStatus.Complete || combatResultAndReward.BattleRecords.Count != 0)
				{
					UIPnlGuildPointMonsterBattleResult.GuildPointBattleResultData battleResultData = new UIPnlGuildPointMonsterBattleResult.GuildPointBattleResultData(stageInfo, combatResultAndReward);

					// Go to combat state
					List<object> paramsters = new List<object>();
					paramsters.Add(combatResultAndReward);
					paramsters.Add(battleResultData);

					SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);
				}
				else if (stageInfo.EventType == GuildStageConfig._EventType.HiddenEnemy && stageInfo.Status == GuildStageConfig._StageStatus.Complete)
				{
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGuildPointMonsterView), stageInfo.ShowRewards, stageInfo);					
				}

				break;
		}
	}

	public void CameraAway(int initPoint, float depth)
	{
		/*相机向量 - 原点向量 获得相机在哪个向量上进行远近拉伸
			用原点向量 + 拉伸系数 * 拉伸向量 获得相机关于世界坐标原点的向量*/
		isCameraFar = true;
		//scroller.TouchScroll = false;
		SetScrollPosition(initPoint);
		Vector3 targetCameraPos = GetPositionByIndex(initPoint) + zeroPointCamPos * depth;
		CameraMoveFar(targetCameraPos, false);
	}

	public void CamereRrestore()
	{
		if (isCameraFar)
			isCameraFar = false;

		Vector3 movePosition = GetCameraPositionByIndex(playerIndex);
		CameraMoveFar(movePosition, false);
		SetScrollPosition(playerIndex);
	}

	private Vector3 GetPositionByIndex(int index)
	{
		int x = index % maxRows;
		int z = index / maxRows;

		return new Vector3(x * distiance, 0f, z * distiance);
	}
}
