using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class WolfSmokeAvatar : MonoBehaviour
{
	protected Avatar avatar;
	public Avatar Avatar
	{
		get { return avatar; }
	}

	protected BattleRole role;
	public BattleRole Role;

	private bool isMoving = false;
	public bool IsMoving
	{
		get
		{
			return isMoving;
		}
	}

	private bool moveToNextDoor = false;

	public System.Action<object> OnRoleReachPathNode;
	public System.Action<object> OnRoleMoveFinish;

	Transform currentPathNode;
	public Transform CurrentPathNode
	{
		get
		{
			return currentPathNode;
		}
	}
	Transform currentDoorNode;
	public Transform CurrentDoorNode
	{
		get
		{
			return currentDoorNode;
		}
	}

	int targetDoorCount = 0;

	Vector3 moveDirection;

	public static WolfSmokeAvatar Create(int avatarAssetId, Transform position)
	{
		//保证场景中只存在一个角色
		if (WolfSmokeSceneData.Instance.currentPlayerRole != null)
			Destroy(WolfSmokeSceneData.Instance.currentPlayerRole.gameObject);

		Avatar newAvatar = Avatar.CreateAvatar(avatarAssetId);
		WolfSmokeAvatar newRole = newAvatar.gameObject.AddComponent<WolfSmokeAvatar>();
		newRole.avatar = newAvatar;
		newRole.avatar.Load(avatarAssetId, false, true);
		newRole.PlayDefaultAnim(AvatarAction._Type.MelaleucaFloorIdle);
		newRole.avatar.transform.position = position.position;
		newRole.avatar.transform.rotation = position.rotation;

		newRole.role = newRole.GetComponent<BattleRole>();
		if (newRole.role == null)
			newRole.role = newRole.gameObject.AddComponent<BattleRole>();

		newRole.role.AvatarAssetId = avatarAssetId;

		WolfSmokeSceneData.Instance.currentPlayerRole = newRole;

		return newRole;
	}

	public static WolfSmokeAvatar CreateEnemy(int avatarAssetId, Transform position)
	{
		//保证场景中只存在一个敌人
		if (WolfSmokeSceneData.Instance.currentEnemyRole != null)
			Destroy(WolfSmokeSceneData.Instance.currentEnemyRole.gameObject);

		Avatar newAvatar = Avatar.CreateAvatar(avatarAssetId);
		WolfSmokeAvatar newRole = newAvatar.gameObject.AddComponent<WolfSmokeAvatar>();
		newRole.avatar = newAvatar;
		newRole.avatar.Load(avatarAssetId, false, true);
		newRole.PlayDefaultAnim(AvatarAction._Type.MelaleucaFloorIdle);
		newRole.avatar.transform.position = position.position;

		newRole.role = newRole.GetComponent<BattleRole>();
		if (newRole.role == null)
			newRole.role = newRole.gameObject.AddComponent<BattleRole>();

		newRole.role.AvatarAssetId = avatarAssetId;

		WolfSmokeSceneData.Instance.currentEnemyRole = newRole;
		return newRole;
	}

	public void ResetPosition(Transform pathNode, Transform doorNode)
	{
		if (IsMoving)
		{
			Debug.LogError("TowerScene: Avatar is moving ,can't do any action.");
			return;
		}

		currentPathNode = pathNode;
		currentDoorNode = doorNode;

		CachedTransform.position = pathNode.position;
		CachedTransform.forward = TowerSceneData.Instance.RoleForward(doorNode);
		PlayDefaultAnim(AvatarAction._Type.MelaleucaFloorIdle);
	}

	private void MoveToNextPathNode()
	{
		currentPathNode = TowerSceneData.Instance.GetNextPathNode(currentPathNode);

		moveDirection = currentPathNode.position - CachedTransform.position;
	}

	private void MoveToNextDoorNode()
	{
		if (IsMoving)
			return;

		currentDoorNode = TowerSceneData.Instance.GetNextDoorNode(currentDoorNode);

		moveToNextDoor = true;
		isMoving = true;

		PlayDefaultAnim(AvatarAction._Type.Run);

		MoveToNextPathNode();
	}

	public void MoveToNextDoorNode(int doorCount)
	{
		Debug.Log("MoveToNextDoorNode doorCount=" + doorCount);
		if (IsMoving)
			return;

		if (doorCount >= 1)
			targetDoorCount = doorCount;
		else
			return;

		MoveToNextDoorNode();
	}

	void Update()
	{
		if (isMoving)
			UpdateMovement();
	}

	void UpdateMovement()
	{
		CachedTransform.forward = TowerSceneData.Instance.RoleForward(CachedTransform);

		MoveForward(TowerSceneData.Instance.playerRoleSpeed * Time.deltaTime);

		Vector3 newPosi = CachedTransform.position;

		if (Vector3.Dot(moveDirection, currentPathNode.position - newPosi) <= 0)
		{
			//这个处理会将人物“拉回来”导致频闪
			//CachedTransform.position = currentPathNode.position;
			if (moveToNextDoor)
			{
				if (currentPathNode.Equals(currentDoorNode))
				{
					targetDoorCount--;
					if (targetDoorCount == 0)
					{
						CachedTransform.position = currentPathNode.position;

						isMoving = false;
						moveToNextDoor = false;
						PlayDefaultAnim(AvatarAction._Type.MelaleucaFloorIdle);

						if (OnRoleMoveFinish != null)
							OnRoleMoveFinish(null);
					}
					else
					{
						isMoving = false;
						MoveToNextDoorNode();
					}
				}
				else
				{
					MoveToNextPathNode();
				}
			}
			else
			{
				PlayDefaultAnim(AvatarAction._Type.MelaleucaFloorIdle);
				isMoving = false;

				if (OnRoleMoveFinish != null)
					OnRoleMoveFinish(null);
			}
			List<object> parameters = new List<object>();
			parameters.Add(this);
			parameters.Add(currentPathNode);

			if (OnRoleReachPathNode != null)
				OnRoleReachPathNode(parameters);
		}
	}

	protected void MoveForward(float dis)
	{
		CachedTransform.Translate(moveDirection.normalized * dis, Space.World);
	}

	//播放指定Avatar的默认动画
	protected void PlayDefaultAnim(int actionType)
	{
		int actionCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, actionType);
		if (actionCount == 0)
		{
			avatar.AvatarAnimation.PlayDefaultAnim(AvatarAssetConfig.ComponentIdToAvatarTypeId(avatar.AvatarId));
			return;
		}

		AvatarAction animAction = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, actionType, 0);

		if (animAction.GetAnimationName(avatar.AvatarId) == "")
		{
			Debug.LogError(string.Format("Animation name is empty in action({0})", animAction.id.ToString("X")));
			return;
		}

#if TowerPlayerRole_Log
		Debug.Log(string.Format("[TowerPlayerRole] play animation: {0}", animAction.GetAnimationName(avatar.AvatarId)));
#endif

		avatar.PlayAnim(animAction.GetAnimationName(avatar.AvatarId));
	}

	//当avatar移动到一个路径点时执行委托
	public void AddNodeReachDel(System.Action<object> del)
	{
		if (this.OnRoleReachPathNode == null)
			this.OnRoleReachPathNode = new System.Action<object>(del);
		else
			this.OnRoleReachPathNode += del;
	}

	public void DelNodeReachDel(System.Action<object> del)
	{
		if (this.OnRoleReachPathNode == null)
			return;

		this.OnRoleReachPathNode -= del;
	}

	//当avatar停止移动时执行委托
	public void AddMoveFinishDel(System.Action<object> del)
	{
		if (this.OnRoleMoveFinish == null)
			OnRoleMoveFinish = new System.Action<object>(del);
		else
			OnRoleMoveFinish += del;
	}

	public void DelMoveFinishDel(System.Action<object> del)
	{
		if (this.OnRoleMoveFinish == null)
			return;
		else
			this.OnRoleMoveFinish -= del;
	}
}
