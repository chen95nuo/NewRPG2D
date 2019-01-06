//#define TowerPlayerRole_Log

using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class TowerPlayerRole : MonoBehaviour
{
	protected Avatar avatar;
	public Avatar Avatar
	{
		get { return avatar; }
	}

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

	public static TowerPlayerRole Create(int avatarAssetId, bool playDefaultAnim, bool useShadow, Transform oriPosi, Transform InitDoorNode)
	{
		//保证楼梯上只有一个角色
		if (TowerSceneData.Instance.currentPlayerRole != null)
			Destroy(TowerSceneData.Instance.currentPlayerRole.gameObject);

		Avatar newAvatar = Avatar.CreateAvatar(avatarAssetId);
		TowerPlayerRole newRole = newAvatar.gameObject.AddComponent<TowerPlayerRole>();
		newRole.avatar = newAvatar;
		newRole.avatar.Load(avatarAssetId, false, useShadow);

		newRole.currentPathNode = oriPosi;
		newRole.currentDoorNode = InitDoorNode;
		//Set Position and Forward;
		newRole.CachedTransform.position = oriPosi.position;
		newRole.CachedTransform.forward = TowerSceneData.Instance.RoleForward(newRole.CachedTransform);
		newRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.MelaleucaFloorIdle);
		newRole.AddNodeReachDel(TowerSceneData.Instance.OnRoleReachPathNode);
		newRole.AddMoveFinishDel(TowerSceneData.Instance.OnRoleMoveFinish);

		TowerSceneData.Instance.currentPlayerRole = newRole;
		TowerSceneData.Instance.melaCamera.towerPlayerRole = newRole;

		return newRole;
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

		TowerSceneData.Instance.currentPlayerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.Run);

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
		//BugFix 设备上切场景时若Unity卡住则Time.deltaTime会很大，人物会跑出千机楼
		float deltaTime = Time.deltaTime > 0.3f ? 0.033f : Time.deltaTime;

		CachedTransform.forward = TowerSceneData.Instance.RoleForward(CachedTransform);

		MoveForward(TowerSceneData.Instance.playerRoleSpeed * deltaTime);

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
						TowerSceneData.Instance.currentPlayerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.MelaleucaFloorIdle);

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
				TowerSceneData.Instance.currentPlayerRole.Avatar.PlayAnimationByActionType(AvatarAction._Type.MelaleucaFloorIdle);
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
