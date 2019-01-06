using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class TowerSceneData : MonoBehaviour
{
	class BuiltObjUnit
	{
		public GameObject monsterMesh;
		public UIElemTowerSceneDoorPnl3D doorUI;
	}

	//控制门牌的摆放位置
	public Vector3 doorUIOffset = Vector3.zero;
	//控制门口石头模型的摆放位置
	public Vector3 doorMonsterOffset = Vector3.zero;

	private static TowerSceneData instance = null;
	public static TowerSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(TowerSceneData)) as TowerSceneData;

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

	public List<TowerUnit> towerUnits;

	public Transform initPathNode;
	public Transform initDoorNode;

	public Transform axleCentre;

	public float playerRoleSpeed = 12;

	public MelaleucaFloorCamera melaCamera;

	float meshDistance = 0f;

	List<TowerUnit> exchangeQueue = new List<TowerUnit>();
	List<Transform> pathNodeChain;
	List<Transform> doorNodeChain;

	TowerUnit currentUnit;

	public TowerPlayerRole currentPlayerRole = null;

	void Start()
	{
		meshDistance = towerUnits[1].towerMesh.position.y - towerUnits[0].towerMesh.position.y;
		exchangeQueue = towerUnits;

		pathNodeChain = new List<Transform>();
		doorNodeChain = new List<Transform>();
		for (int i = 0; i < exchangeQueue.Count; i++)
		{
			var unit = exchangeQueue[i];
			pathNodeChain.AddRange(unit.pathNodes);
			doorNodeChain.AddRange(unit.doorNodes);
		}
	}

	public void ProcessMeshExchange(Transform currentPathNode)
	{
		if (exchangeQueue[exchangeQueue.Count - 1].meshExchangeForwardTrigger.Equals(currentPathNode))
		{
			var temp = exchangeQueue[0];
			temp.towerMesh.position = new Vector3(temp.towerMesh.position.x, temp.towerMesh.position.y + meshDistance * exchangeQueue.Count, temp.towerMesh.position.z);
			exchangeQueue.RemoveAt(0);
			exchangeQueue.Add(temp);
		}
	}

	public Transform GetNextPathNode(Transform currentPathNode)
	{
		int idx = pathNodeChain.IndexOf(currentPathNode);
		if (idx < pathNodeChain.Count - 1)
			return pathNodeChain[idx + 1];

		return pathNodeChain[0];
	}

	public Transform GetPrevPathNode(Transform currentPathNode)
	{
		int idx = pathNodeChain.IndexOf(currentPathNode);
		if (idx > 1)
			return pathNodeChain[idx - 1];

		return pathNodeChain[pathNodeChain.Count - 1];
	}

	public Transform GetNextDoorNode(Transform currentDoorNode)
	{
		int idx = doorNodeChain.IndexOf(currentDoorNode);
		if (idx < doorNodeChain.Count - 1)
			return doorNodeChain[idx + 1];

		return doorNodeChain[0];
	}

	public Transform GetPrevDoorNode(Transform currentDoorNode)
	{
		int idx = doorNodeChain.IndexOf(currentDoorNode);
		if (idx > 1)
			return doorNodeChain[idx - 1];

		return doorNodeChain[doorNodeChain.Count - 1];
	}

	public Vector3 RoleForward(Transform role)
	{
		Vector3 p0 = tracePosition(role);
		Vector3 v0 = (role.position - p0).normalized;

		return Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(90, Vector3.up), Vector3.one).MultiplyPoint3x4(v0);
	}

	public Vector3 CameraVector(Transform role, float AngleX, float angleY)
	{
		Vector3 p0 = tracePosition(role);
		Vector3 v0 = (role.position - p0).normalized;

		return (Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(AngleX, role.up.normalized), Vector3.one)
		* Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angleY, role.forward.normalized), Vector3.one))
		.MultiplyPoint3x4(v0);
	}

	public Vector3 tracePosition(Transform role)
	{
		return new Vector3(axleCentre.position.x, role.position.y, axleCentre.position.z);
	}

	public Transform AxleCentre
	{
		get
		{
			return axleCentre;
		}
	}

	List<BuiltObjUnit> builtObjs = new List<BuiltObjUnit>();

	public void ClearDoorUI()
	{
		foreach (var obj in builtObjs)
		{
			if (obj.doorUI != null)
				Destroy(obj.doorUI.gameObject);
		}
	}

	public void ClearData()
	{
		foreach (var obj in builtObjs)
		{
			if (obj.monsterMesh != null)
				Destroy(obj.monsterMesh);
			if (obj.doorUI != null)
				Destroy(obj.doorUI.gameObject);
		}

		builtObjs.Clear();
	}

	void SetDoorUI(Transform tempDoor, UIElemTowerSceneDoorPnl3D doorPnlUI)
	{
		ObjectUtility.AttachToParentAndResetLocalTrans(tempDoor.gameObject, doorPnlUI.gameObject);

		//方向矢量
		Vector3 Vec = tracePosition(doorPnlUI.CachedTransform) - doorPnlUI.CachedTransform.position;
		doorPnlUI.CachedTransform.forward = Vec.normalized;
		//偏移矢量
		doorPnlUI.CachedTransform.position += Math.RelativeOffset(doorPnlUI.CachedTransform, doorUIOffset, true);
	}

	//重新初始化每一层的怪物信息。以当前角色所在位置为参考，初始化后面8层（包括当前所站的门的位置，前一个门的位置）
	//TODO 是否可以Cache一些资源？
	public void ReFillFloorData(int DoorIdxOffset)
	{
		Debug.Log("ReFillFloorData DoorIdxOffset=" + DoorIdxOffset);

		if (currentPlayerRole == null)
		{
			Debug.LogError("currentPlayerRole==null");
			return;
		}

		//如果角色正在移动，仍以角色所在路径点为参考，初始化后面8个门的数据，不重置位置
		//currentPlayerRole.ResetPosition(initPathNode, initDoorNode);
		ClearData();

		int currentLayer = SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer;

		//获取当前所站的门
		Transform tempDoor = currentPlayerRole.CurrentDoorNode;
		if (!currentPlayerRole.CurrentDoorNode.Equals(currentPlayerRole.CurrentPathNode))
			tempDoor = GetPrevDoorNode(tempDoor);

		UIElemTowerSceneDoorPnl3D doorPnlUI;
		GameObject monsterMesh = null;

		//获取角色所占位置前一个门（-1）
		tempDoor = GetPrevDoorNode(tempDoor);
		//千层楼配置的最大层数
		int maxConfigedFloorLayer = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.MaxConfigedFloorLayer;

		for (int i = -1; i <= 9; i++, tempDoor = GetNextDoorNode(tempDoor))
		{
			int doorLayer = currentLayer + i - DoorIdxOffset;

			if (doorLayer < 0)
				continue;

			doorPnlUI = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.towerSceneDoor)).GetComponent<UIElemTowerSceneDoorPnl3D>();
			doorPnlUI.SetLayer(doorLayer);

			ObjectUtility.AttachToParentAndResetLocalTrans(tempDoor.gameObject, doorPnlUI.gameObject);

			SetDoorUI(tempDoor, doorPnlUI);

			if (i >= 1)
			{
				MelaleucaFloorConfig.Floor floor = null;

				if (doorLayer > maxConfigedFloorLayer)
					floor = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(maxConfigedFloorLayer);
				else
					floor = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(doorLayer);

				string monsterMeshPath = PathUtility.Combine(GameDefines.otherObjPath, floor.Icon);

				monsterMesh = ResourceManager.Instance.InstantiateAsset<GameObject>(monsterMeshPath);
				if (monsterMesh == null)
				{
					Debug.LogError("monsterMesh missed.");
					monsterMesh = new GameObject("monsterMesh");
				}

				if (i < 9)
				{
					UIListButton3D button3D = monsterMesh.AddComponent<UIListButton3D>();
					button3D.scriptWithMethodToInvoke = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlTowerPlayerInfo>();
					button3D.methodToInvoke = "OnLayerButtonClick";
					button3D.Data = doorLayer;
					button3D.SetList(TowerSceneData.instance.melaCamera.scroller);

					monsterMesh.AddComponent<BoxCollider>().size = new Vector3(1.8f, 4.5f, 1.8f);
					monsterMesh.gameObject.layer = GameDefines.UISceneRaycastLayer;
				}

				ObjectUtility.AttachToParentAndResetLocalTrans(tempDoor.gameObject, monsterMesh);

				Transform monsterTrans = monsterMesh.transform;

				monsterTrans.forward = RoleForward(monsterTrans);
				monsterTrans.position += KodGames.Math.RelativeOffset(monsterTrans, doorMonsterOffset, false);

			}

			BuiltObjUnit unit = new BuiltObjUnit()
			{
				monsterMesh = monsterMesh,
				doorUI = doorPnlUI
			};

			builtObjs.Add(unit);
		}
	}

	//删除位于指定PathNode处的千机楼数据
	public void DestroyDataByPosition(Transform pathNode)
	{
		for (int i = 0; i < builtObjs.Count; i++)
		{
			if (builtObjs[i].monsterMesh == null)
				continue;

			var temp = builtObjs[i].monsterMesh.transform.parent;

			if (temp.Equals(pathNode))
			{
				if (builtObjs[i].monsterMesh != null)
				{

					GameObject moveEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.collisionQianJiLou));
					if (moveEffect != null)
						moveEffect.transform.position = builtObjs[i].monsterMesh.transform.position;

					Destroy(builtObjs[i].monsterMesh, 0.5f);
				}

				//if (builtObjs[i].doorUI.gameObject != null)
				//    Destroy(builtObjs[i].doorUI.gameObject);
				if (builtObjs[i].doorUI == null)
				{
					builtObjs.RemoveAt(i);
					i--;
				}
			}
		}
	}

	public void OnRoleReachPathNode(object parameters)
	{
		Transform roleReachedPathNode = (Transform)(parameters as List<object>)[1];
		TowerPlayerRole tpr = (TowerPlayerRole)(parameters as List<object>)[0];

		Transform forward9thDoorNode = tpr.CurrentDoorNode;
		for (int i = 1; i <= 9; i++)
			forward9thDoorNode = GetNextDoorNode(forward9thDoorNode);

		ProcessMeshExchange(forward9thDoorNode);
		DestroyDataByPosition(roleReachedPathNode);
	}

	public void OnRoleMoveFinish(object parameters)
	{
		//currentPlayerRole.ResetPosition(initPathNode, initDoorNode);
		ReFillFloorData(0);
	}
}