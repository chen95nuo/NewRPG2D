using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class WolfSmokeSceneData : MonoBehaviour
{
	private ActionFxPlayer actionFxPlayer = new ActionFxPlayer();
	public ActionFxPlayer ActionFxPlayer
	{
		get { return actionFxPlayer; }
	}

	private static WolfSmokeSceneData instance = null;
	public static WolfSmokeSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(WolfSmokeSceneData)) as WolfSmokeSceneData;

			return instance;
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

	public GameObject cameraRoot;
	public GameObject cameraMoveRoot;
	public GameObject stageRoot;

	public List<GameObject> pathNodes;
	public WolfSmokeAvatar currentPlayerRole = null;
	public WolfSmokeAvatar currentEnemyRole = null;

	private BattleRole playerRole;
	private BattleRole enemyRole;

	public List<GameObject> doorNodes;
	public List<GameObject> doorPreb;

	private bool isMoving = false;
	public bool IsMoving { get { return isMoving; } }

	private List<Vector3> eggVectors = new List<Vector3>();
	private List<GameObject> eggs = new List<GameObject>();
	//标记跑到第几个彩蛋
	private int eggIndex = 0;

	//下一扇门
	private GameObject nextDoor = null;
	public float playerRoleSpeed = 12;

	//下一关的位置记录点（己方位置，敌方）
	private Vector3 moveDirection;
	private Vector3 flyDirection;
	private Vector3 nextAvatarNode;
	private Vector3 nextEnemyNode;

	//烽火狼烟关卡记录点
	private Transform wolfStageTrans;

	//第几关卡
	private int stageIndex;

	//最大关卡数量
	private int stageMaxCount;

	//定值距离
	private readonly int distance = 60;

	//下一关敌人
	private int nextEnemyAssetId = 0;
	private string enemyName;
	private int power;

	//原始相机位置
	private Vector3 oldCameraPosition;
	private Quaternion oldCameraRotation;
	private Transform oldCameraTrans;

	private bool battleDialogue;

	//相机移动标志
	//0 静止态 1 普通 - 抬高 2 抬高 - 普通
	private int isCameraMoving = 0;
	private float moveTime = 0;

	//跑动需要数据
	private UIPnlWolfBattleResult.WolfBattleResultData wolfBattleData;

	//关卡数量

	public void CreateWolfRun(int playerAssetId, int enemyAssetId, int stageIndex, UIPnlWolfBattleResult.WolfBattleResultData wolfBattleData, bool isShow, string enemyName, int power)
	{
		this.battleDialogue = isShow;
		this.enemyName = enemyName;
		this.power = power;

		//创建跑动前的关卡跟敌人			
		AvatarConfig.Avatar enemyCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(wolfBattleData.ShowAvatar.resourceId);
		int enemyId = enemyCfg.GetAvatarAssetId(wolfBattleData.ShowAvatar.breakthoughtLevel);

		CreateAvatar(playerAssetId, enemyId, stageIndex - 1, false, true, enemyName, power);
		this.nextEnemyAssetId = enemyAssetId;
		this.wolfBattleData = wolfBattleData;
		//创建彩蛋		
		CreateEggs(pathNodes[3].transform.position, pathNodes[2].transform.position.z - pathNodes[3].transform.position.z);
	}

	public void CreateWolfStage(int stageIndex, int stageCount)
	{
		this.stageMaxCount = stageCount;

		//超出界限判定
		if (stageIndex > stageMaxCount)
			stageIndex = stageMaxCount;

		this.stageIndex = stageIndex;
		//生成光卡节点
		ReSetTransform(stageRoot, stageRoot);

		//中间关卡
		GameObject stageBefore = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex - 1].ModelObjName));
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageRoot, stageBefore);

		//关卡名字
		UIElemWolfNumberPlate doorBeforeUI = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.wolfDoorPlate)).GetComponent<UIElemWolfNumberPlate>();
		doorBeforeUI.SetNumberPlate(ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex - 1].StageName);
		ObjectUtility.AttachToParentAndResetLocalTrans(stageBefore.GetComponentInChildren<UIElemWolfDoorNumberObj>().doorNumberObj, doorBeforeUI.gameObject);

		//后期关卡
		GameObject stageNext = null;
		if (stageIndex < stageMaxCount)
			stageNext = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex].ModelObjName));
		else
			stageNext = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex - 1].ModelObjName)); ;
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageRoot, stageNext);

		Vector3 nextPosi = stageBefore.transform.position;
		nextPosi.z = stageBefore.transform.position.z - distance;
		stageNext.transform.position = nextPosi;

		//关卡名字
		if (stageIndex < stageMaxCount)
		{
			UIElemWolfNumberPlate doorUI = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.wolfDoorPlate)).GetComponent<UIElemWolfNumberPlate>();
			doorUI.SetNumberPlate(ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex].StageName);
			ObjectUtility.AttachToParentAndResetLocalTrans(stageNext.GetComponentInChildren<UIElemWolfDoorNumberObj>().doorNumberObj, doorUI.gameObject);
		}

		//前期关卡
		GameObject beforeStage = null;
		if (stageIndex > 1)
			beforeStage = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex - 2].ModelObjName));
		else
			beforeStage = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex - 1].ModelObjName)); ;

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(stageRoot, beforeStage);

		Vector3 beforePosi = beforeStage.transform.position;
		beforePosi.z = stageBefore.transform.position.z + distance;
		beforeStage.transform.position = beforePosi;

		//关卡名字
		if (stageIndex > 1)
		{
			UIElemWolfNumberPlate doorNextUI = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.wolfDoorPlate)).GetComponent<UIElemWolfNumberPlate>();
			doorNextUI.SetNumberPlate(ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stageIndex - 2].StageName);
			ObjectUtility.AttachToParentAndResetLocalTrans(beforeStage.GetComponentInChildren<UIElemWolfDoorNumberObj>().doorNumberObj, doorNextUI.gameObject);
		}
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

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	IEnumerator AvatarEnterSceneThread(List<object> param)
	{
		yield return new WaitForEndOfFrame();

		if (playerRole != null)
			Destroy(playerRole.gameObject);

		int playerAssetId = (int)param[0];
		int enemAssetId = (int)param[1];
		int stageIndex = (int)param[2];
		bool isShowDialogue = (bool)param[3];
		bool isBattleWin = (bool)param[4];

		this.stageIndex = stageIndex;
		CreateEnemyAvatar(enemAssetId, isBattleWin);
		CreatePlayerAvatar(playerAssetId);
		playerRole.Hide = true;

		ActionFxPlayer.PlayActionFx(playerRole,
		 ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.EnterScene, 0).id,
		  true);

		while (playerRole.ActionState == BattleRole._ActionState.Busy)
			yield return null;

		PlayDefaultAnim(playerRole.Avatar, _CombatStateType.Default, AvatarAction._Type.MelaleucaFloorIdle);

		ScenePrepare(isShowDialogue, isBattleWin);
	}

	public void AvatarEnterScene(int playerAssetId, int enemyAssetId, int stageIndex, bool isShowDialogue, bool isBattleWin, string enemyName, int enemyPower)
	{
		this.stageIndex = stageIndex;
		this.enemyName = enemyName;
		this.power = enemyPower;

		CreateDoor();
		ReSetTransform(cameraRoot, cameraRoot);
		List<object> param = new List<object>();
		param.Add(playerAssetId);
		param.Add(enemyAssetId);
		param.Add(stageIndex);
		param.Add(isShowDialogue);
		param.Add(isBattleWin);

		StartCoroutine("AvatarEnterSceneThread", param);
	}

	private void ScenePrepare(bool isShowDialogue, bool isBattleWin)
	{
		//下一关镜头返还位置
		oldCameraTrans = cameraRoot.transform;

		oldCameraRotation = cameraRoot.transform.rotation;
		oldCameraPosition = cameraRoot.transform.position;
		oldCameraPosition.z -= distance;

		ReSetTransform(cameraMoveRoot, cameraMoveRoot);

		//初始化下一关的基准点
		nextAvatarNode = playerRole.gameObject.transform.position;
		nextEnemyNode = enemyRole.gameObject.transform.position;

		nextAvatarNode.z -= distance;
		nextEnemyNode.z -= distance;

		CreatePlayerButton();

		if (isShowDialogue)
			SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().ShowWolfDialogue();
	}

	void CreatePlayerAvatar(int playerAssetId)
	{
		//场景中只存在一个角色跟敌人
		if (playerRole != null)
			Destroy(playerRole.gameObject);

		playerRole = CreateRole(playerAssetId, false, true);
		PlayDefaultAnim(playerRole.Avatar, _CombatStateType.Default, AvatarAction._Type.MelaleucaFloorIdle);
		ReSetTransform(playerRole.gameObject, pathNodes[0]);
	}

	void CreateEnemyAvatar(int enemyAssetId, bool isBattleWin)
	{
		//场景中只存在一个角色跟敌人
		if (enemyRole != null)
			Destroy(enemyRole.gameObject);

		enemyRole = CreateRole(enemyAssetId, false, true);
		PlayDefaultAnim(enemyRole.Avatar, _CombatStateType.Default, AvatarAction._Type.MelaleucaFloorIdle);
		ReSetTransform(enemyRole.gameObject, pathNodes[1]);

		//是否为第一关		
		if (stageIndex == 1)
			CreateEnemyButton(true, isBattleWin);
		else
			CreateEnemyButton(false, isBattleWin);
	}

	public void CreateAvatar(int playerAssetId, int enemyAssetId, int stageIndex, bool isShowDialogue, bool isBattleWin, string enemyName, int power)
	{
		//先设置stageIndex ReSetTransform中有用到

		this.stageIndex = stageIndex;
		this.enemyName = enemyName;
		this.power = power;

		CreateDoor();
		ReSetTransform(cameraRoot, cameraRoot);
		CreatePlayerAvatar(playerAssetId);
		CreateEnemyAvatar(enemyAssetId, isBattleWin);
		ScenePrepare(isShowDialogue, isBattleWin);
	}

	private void CreateEnemyButton(bool isFirst, bool isBattleWin)
	{
		//如果不是从战斗胜利返回，挂载点击特效并创建姓名板
		if (!isBattleWin)
		{
			GameObject clickEffect = null;
			if (isFirst)
				clickEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.wolfClickMe_Font));
			else
				clickEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.wolfClickMe));

			if (clickEffect != null)
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(enemyRole.gameObject, clickEffect);

			//创建敌方姓名板			

			UIElemWolfNumberPlate playerNameUI = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.wolfDoorName)).GetComponent<UIElemWolfNumberPlate>();
			playerNameUI.SetNumberPlate(this.enemyName, this.power);
			ObjectUtility.AttachToParentAndResetLocalTrans(enemyRole.gameObject, playerNameUI.gameObject);
		}

		GameObject click_button = new GameObject("enemy_button");
		UIButton3D button3D = click_button.AddComponent<UIButton3D>();
		UIPnlWolfInfo wolfInfo = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlWolfInfo>();
		button3D.scriptWithMethodToInvoke = wolfInfo;
		button3D.methodToInvoke = "OnClickEnemy";

		click_button.AddComponent<BoxCollider>().size = new Vector3(2f, 3f, 2f);
		click_button.gameObject.layer = GameDefines.UISceneRaycastLayer;
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(enemyRole.gameObject, click_button);
		click_button.gameObject.transform.localPosition = new Vector3(0, 1, 0);
	}

	//创建己方3D按钮
	private void CreatePlayerButton()
	{
		GameObject click_button = new GameObject("palyer_button");
		UIButton3D button3D = click_button.AddComponent<UIButton3D>();
		UIPnlWolfInfo wolfInfo = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlWolfInfo>();
		button3D.scriptWithMethodToInvoke = wolfInfo;
		button3D.methodToInvoke = "OnClickAvatar";

		click_button.AddComponent<BoxCollider>().size = new Vector3(1.5f, 2.5f, 1.5f);
		click_button.gameObject.layer = GameDefines.UISceneRaycastLayer;
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(playerRole.gameObject, click_button);
		click_button.gameObject.transform.localPosition = new Vector3(0, 1, 0);
	}

	public void ReSetCamerTransform(int stageNum)
	{
		this.stageIndex = stageNum;
		ReSetTransform(cameraRoot, cameraRoot);
	}

	public void ReSetTransform(GameObject source, GameObject target)
	{
		Vector3 newPosi = target.transform.position;
		newPosi.z -= (stageIndex - 1) * distance;

		source.transform.position = newPosi;
		source.transform.rotation = target.transform.rotation;
		source.transform.localScale = target.transform.localScale;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	IEnumerator PassPointThread()
	{
		if (playerRole == null || enemyRole == null)
		{
			Debug.Log("敌人或者己方角色缺失");
			yield break;
		}

		ActionFxPlayer.PlayActionFx(enemyRole,
		 ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Die, 0).id,
		  true);

		while (enemyRole.ActionState == BattleRole._ActionState.Busy)
			yield return null;

		enemyRole.Avatar.Destroy();
		GameObject.Destroy(enemyRole.gameObject);

		//创建新敌方角色
		enemyRole = CreateRole(nextEnemyAssetId, false, true);
		isCameraMoving = 1;
		moveTime = 0;

		PlayDefaultAnim(enemyRole.Avatar, _CombatStateType.Default, AvatarAction._Type.MelaleucaFloorIdle);

		ReSetTransform(enemyRole.gameObject, pathNodes[1]);
		enemyRole.gameObject.transform.position = nextEnemyNode;
		CreateEnemyButton(false, false);
	}

	//战斗胜利之后播放的动画（彩蛋系统）
	public void PassPoint()
	{
		StartCoroutine("PassPointThread");
	}

	//播放指定Avatar的默认动画
	private void PlayDefaultAnim(Avatar avatar, int stateType, int actionType)
	{
		int actionCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(EquipmentConfig._WeaponType.Empty, stateType, actionType);
		if (actionCount == 0)
		{
			avatar.AvatarAnimation.PlayDefaultAnim(AvatarAssetConfig.ComponentIdToAvatarTypeId(avatar.AvatarId));
			return;
		}

		AvatarAction animAction = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, stateType, actionType, 0);

		if (animAction.GetAnimationName(avatar.AvatarId) == "")
		{
			Debug.LogError(string.Format("Animation name is empty in action({0})", animAction.id.ToString("X")));
			return;
		}

		avatar.PlayAnim(animAction.GetAnimationName(avatar.AvatarId));
	}

	public void OpenDoor()
	{
		//开门
		var doorAnima = doorPreb[(stageIndex - 1) / 4].GetComponentInChildren<Animation>();
		doorAnima.Play();
		//角色跑动
		PlayDefaultAnim(playerRole.Avatar, _CombatStateType.Default, AvatarAction._Type.Run);
		//移动方向
		moveDirection = pathNodes[2].transform.position - pathNodes[0].transform.position;
		//彩蛋起飞方向  
		flyDirection = pathNodes[4].transform.position - pathNodes[3].transform.position;

		isMoving = true;
	}

	//创建门
	private void CreateDoor()
	{
		int doorIndex = stageIndex - 1;
		doorPreb[doorIndex / 4].transform.position = doorNodes[doorIndex].transform.position;

		//创建门时停止门的动画
		doorPreb[doorIndex / 4].gameObject.GetComponentInChildren<Animation>().Stop();

		//创建需要跑到的门
		if (stageIndex < stageMaxCount)
		{
			nextDoor = (GameObject)GameObject.Instantiate(doorPreb[(stageIndex) / 4].gameObject);
			nextDoor.transform.position = doorNodes[stageIndex].transform.position;
			nextDoor.gameObject.GetComponentInChildren<Animation>().Stop();
		}
	}

	//创建彩蛋
	private void CreateEggs(Vector3 eggPath, float lengh)
	{
		//在哪关之后创建彩蛋
		eggPath.z -= distance * (this.stageIndex - 1);
		eggVectors = new List<Vector3>();
		List<ClientServerCommon.WolfSmokeConfig.EggsEffect> eggEffs = ConfigDatabase.DefaultCfg.WolfSmokeConfig.EggsEffects;

		//+1预留最后角色站位
		int eggCount = this.wolfBattleData.WolfEggs.Count + 1;
		//均分彩蛋位置 
		for (int i = 0; i < eggCount; i++)
		{
			Vector3 eggPosi = eggPath;
			eggPosi.z += (lengh / eggCount) * i;
			eggVectors.Add(eggPosi);
		}

		//在各个位置上创建彩蛋
		for (int i = 0; i < eggCount - 1; i++)
		{
			string eggModel = "";
			string getEff = "";
			for (int j = 0; j < eggEffs.Count; j++)
			{
				if (eggEffs[j].RewardId == this.wolfBattleData.WolfEggs[i].RewardId)
				{
					eggModel = eggEffs[j].Model;
					getEff = eggEffs[j].GetEffect;
					break;
				}
			}
			GameObject egg = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.otherObjPath, eggModel));
			egg.transform.position = eggVectors[i];

			//箱子特效			
			if (!getEff.Equals(string.Empty))
			{
				GameObject boxEff = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, getEff));
				if (boxEff != null)
					ObjectUtility.AttachToParentAndResetLocalPosAndRotation(egg, boxEff);
			}
			eggs.Add(egg);
		}
	}

	private void MoveCamera(Transform startPosi, Transform endPosi)
	{
		float deltaTime = Time.deltaTime > 0.3f ? 0.033f : Time.deltaTime;

		moveTime += deltaTime;

		cameraRoot.transform.position = Vector3.Lerp(startPosi.position, endPosi.position, moveTime / 2f);
		cameraRoot.transform.rotation = Quaternion.Lerp(startPosi.rotation, endPosi.rotation, moveTime / 2f);
	}

	void Update()
	{
		//当相机移动
		if (isCameraMoving != 0)
		{
			//正向停止
			if (isCameraMoving > 0 && cameraRoot.transform.position.Equals(cameraMoveRoot.transform.position))
			{
				isCameraMoving = 0;
				moveTime = 0;
				OpenDoor();
			}
			else if (isCameraMoving > 0)
			{
				MoveCamera(oldCameraTrans, cameraMoveRoot.transform);
			}

			//反向停止
			if (isCameraMoving < 0 && cameraRoot.transform.position.Equals(oldCameraPosition))
			{
				isCameraMoving = 0;
				moveTime = 0;
			}
			else if (isCameraMoving < 0)
			{
				cameraMoveRoot.transform.rotation = oldCameraRotation;
				cameraMoveRoot.transform.position = oldCameraPosition;
				MoveCamera(cameraRoot.transform, cameraMoveRoot.transform);
			}
		}

		if (isMoving)
			UpdateMovement();

		//TODO 优化 时间空间 log 2N
		for (int index = 0; index < eggIndex; index++)
		{
			if (eggs[index] != null)
			{
				var boxEffect = ObjectUtility.FindChildObject(eggs[index].gameObject, "p_Q_Chest");
				if (boxEffect != null)
					GameObject.Destroy(boxEffect.gameObject);

				MoveFly(eggs[index].gameObject, 30f * Mathf.Min(0.3f, Time.deltaTime));
				if (eggs[index].gameObject.transform.position.y >= pathNodes[4].gameObject.transform.position.y)
				{
					GameObject.Destroy(eggs[index].gameObject);
				}
			}
			else if (eggs[eggs.Count - 1] == null)
			{
				eggIndex = 0;
				isCameraMoving = -1;
			}
		}
	}

	// TODO 优化不需要在更新中函数做的赋值操作
	void UpdateMovement()
	{
		float deltaTime = Time.deltaTime > 0.3f ? 0.033f : Time.deltaTime;

		MoveForward(playerRoleSpeed * deltaTime);
		//基准点位置
		Vector3 newPosi = playerRole.transform.position;

		if (eggIndex < eggs.Count && newPosi.z <= eggVectors[eggIndex].z)
		{

			GameObject eggEff = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.getBoxParticle));
			if (eggEff != null)
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(eggs[eggIndex], eggEff);

			eggIndex++;
			SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().UpDateEggsReward(wolfBattleData.WolfEggs[eggIndex - 1], eggIndex - 1);
		}

		if (newPosi.z <= nextAvatarNode.z)
		{
			playerRole.transform.position = nextAvatarNode;
			isMoving = false;
			PlayDefaultAnim(playerRole.Avatar, _CombatStateType.Default, AvatarAction._Type.MelaleucaFloorIdle);
			if (this.battleDialogue)
				SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().ShowWolfDialogue();

			//角色停止移动，按钮开放点击
			SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().ResetPlayerMoving();
		}
	}

	private void MoveForward(float speed)
	{
		playerRole.transform.Translate(moveDirection.normalized * speed, Space.World);
		//TODO相机移动逻辑
		cameraRoot.transform.Translate(moveDirection.normalized * speed, Space.World);
	}

	private void MoveFly(GameObject eggObj, float speed)
	{
		//彩蛋起飞逻辑
		eggObj.transform.Translate(flyDirection.normalized * speed, Space.World);
	}
}
