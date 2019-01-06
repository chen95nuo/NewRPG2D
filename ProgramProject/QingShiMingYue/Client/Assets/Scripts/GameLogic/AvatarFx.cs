using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

/// <summary>
/// Avatar effect class.
/// </summary>
public class AvatarFx
{
	// Pfx instance.
	protected class FxInst
	{
		public FxInst(FXController fx, int destroyMode, int userData)
		{
			instanceId = idAllcocator.NewID();

			this.fxController = fx;
			this.destroyMode = destroyMode;
			this.userData = userData;
		}

		public void Release()
		{
			// Destroy Pfx object.
			fxController.StopFX();
			fxController = null;
			idAllcocator.ReleaseID(instanceId);
		}

		public int instanceId; // Instance ID.
		public FXController fxController; // Pfx game object.
		public int destroyMode; // Destroy mode.
		public int userData; // User data.

		private static IDAllocator idAllcocator = new IDAllocator(); // ID allocator.
	}

	public AvatarFx(Avatar avatar)
	{
		this.avatar = avatar;
	}

	public void Release()
	{
	}

	public void Update()
	{
	}

	// Play Pfx.
	public void PlayPfx(FXController fx, int destroyMode, int userData, string modelBone, string bone, bool boneFollow, Vector3 offset, Vector3 rotate, bool useSpecificPosition, Vector3 specificPosition)
	{
		// Set Render layer

		/**
		 * Bug 记录。
		 * 
		 * 对于魔化卫庄的组合技，这个技能在主Action中有目标选择，但是每一个伤害事件都有自己的目标选择，最终可能有一种情况，即所有的EventTargetRecord中的目标均不包含Action选择的目标
		 * 在主Action中配置了一些播放特效的事件，这些事件服务器不作处理，也不会发给客户端，而是由客户端自己根据配置文件构建，构建时使用ActionRecord的TargetAvtarIndexs里的AvatarIndex，
		 * 指定的AvatarIndex可能不在任何EventTargetRecord中，所以SkillCombatRound在遮罩屏幕时不会将这个角色点亮，导致“由这个角色”释放的特效也没有被点亮
		 * 
		 * 修改逻辑，如果正处于屏幕遮罩中，那么所有的特效都有应该具有SceneMaskLayer
		 */
		BattleScene bs = BattleScene.GetBattleScene();

		int Layer = 0;
		if (bs != null && bs.IsEnableSceneMask)
			Layer = GameDefines.SceneMaskLayer;
		else Layer = avatar.gameObject.layer;

		// Set Render layer
		fx.Root.gameObject.layer = Layer;
		foreach (var subObj in fx.Root.GetComponentsInChildren<Transform>())
			subObj.gameObject.layer = Layer;

		// If bone follow, attach to this bone.
		if (boneFollow)
		{
			var attachingGO = fx.Root;

			// Try to find specified 
			if (modelBone != "")
			{
				var modelBoneObj = ObjectUtility.FindChildObject(attachingGO, modelBone);
				if (modelBoneObj != null)
					attachingGO = modelBoneObj;
			}

			// If specify bone, to this bone.
			if (bone != "")
			{
				var boneGO = ObjectUtility.FindChildObject(avatar.AvatarObject, bone);
				if (boneGO == null)
					Debug.LogWarning(string.Format("Can not find bone:{0} in avatar", bone));
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(boneGO, attachingGO);
			}
			// Has no specify bone, to the root bone.
			else
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(avatar.CachedTransform, attachingGO);
		}
		// If not bone follow, just play at this bone position.
		else
		{
			// If specify bone, use this bone position.
			if (bone != "")
			{
				var boneGO = ObjectUtility.FindChildObject(avatar.CachedTransform, bone);
				if (boneGO == null)
					Debug.LogWarning(string.Format("Can not find bone:{0} in avatar", bone));
				ObjectUtility.UnifyWorldTrans(boneGO, fx.Root);
			}
			// Has specific transform, use the transform.
			else if (useSpecificPosition)
				fx.Root.position = specificPosition;
			// Has no specify bone, use the root bone position.
			else
				ObjectUtility.UnifyWorldTrans(avatar.CachedTransform, fx.Root);
		}

		fx.Root.Translate(offset);
		fx.Root.Rotate(rotate);

		if (destroyMode != AvatarAction.Effect._DestroyType.Normal)
		{
			// Add to dictionary.
			var fxInst = new FxInst(fx, destroyMode, userData);
			fxMap[fxInst.instanceId] = fxInst;

			// Set auto destroy flag to Pfx script.
			//FXController PfxScript = fx.GetComponentInChildren<FXController>();
			//if (PfxScript != null)
			{
				fx.autoDestroy = true;

				// Pfx will not loop except buff.
				if (destroyMode == AvatarAction.Effect._DestroyType.Buff)
					fx.loop = true;

				if (destroyMode == AvatarAction.Effect._DestroyType.BlockAction)
					fx.AddFinishCallback(PfxFinishedBb, fxInst);
			}
		}
	}

	public void StopPfxByName(string name)
	{
		List<int> removeList = new List<int>();

		// Find Pfx
		foreach (KeyValuePair<int, FxInst> kvp in fxMap)
			if (kvp.Value.fxController.gameObject.name == name)
				removeList.Add(kvp.Key);

		// Stop Pfx
		foreach (var id in removeList)
			StopPfxByInstID(id);

	}

	// Stop Pfx by instance id.
	public void StopPfxByInstID(int instID)
	{
		if (!fxMap.ContainsKey(instID))
			return;

		var fxInst = fxMap[instID];
		fxInst.Release();
		fxMap.Remove(instID);
	}

	// Stop Pfx by user data.
	public void StopPfxByUsd(int usd)
	{
		List<int> removeList = new List<int>();

		// Find Pfx
		foreach (KeyValuePair<int, FxInst> kvp in fxMap)
			if (kvp.Value.userData == usd)
				removeList.Add(kvp.Key);

		// Stop Pfx
		foreach (var id in removeList)
			StopPfxByInstID(id);
	}

	// Stop all Pfx.
	public void StopAllPfx()
	{
		List<int> removeList = new List<int>();

		// Find Pfx
		foreach (KeyValuePair<int, FxInst> kvp in fxMap)
			removeList.Add(kvp.Key);

		// Stop Pfx
		foreach (var id in removeList)
			StopPfxByInstID(id);
	}

	public List<int> GetPfxInstIDListByUsd(int usd)
	{
		List<int> results = new List<int>();

		// Find Pfx
		foreach (KeyValuePair<int, FxInst> kvp in fxMap)
			if (kvp.Value.userData == usd)
				results.Add(kvp.Key);

		return results;
	}

	// Stop all playing Pfx controlled by action.
	public void StopAnimPfx()
	{
		List<int> rmList = new List<int>();

		// Find the action Pfx.
		foreach (KeyValuePair<int, FxInst> kvp in fxMap)
			if (kvp.Value.destroyMode == AvatarAction.Effect._DestroyType.Action)
				rmList.Add(kvp.Key);

		// Stop it.
		foreach (int instID in rmList)
			StopPfxByInstID(instID);
	}

	// Pfx finished callback.
	protected void PfxFinishedBb(object userData)
	{
		var fxInst = userData as FxInst;
		fxInst.Release();
		fxMap.Remove(fxInst.instanceId);
	}

	protected Avatar avatar; // The avatar which this effect belongs to.
	protected Dictionary<int, FxInst> fxMap = new Dictionary<int, FxInst>(); // Pfx dictionary.
}
