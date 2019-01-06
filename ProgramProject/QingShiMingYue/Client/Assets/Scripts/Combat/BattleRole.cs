using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class BattleRole : CharacterController
{
	// Buffer.
	public class Buff
	{
		public Buff(int pBufID, int pInstID)
		{
			bufID = pBufID;
			instID = pInstID;
		}

		public int bufID; // Buffer id.
		public int instID; // Buffer instance id.
	}

	public enum _ActionState
	{
		Idle,
		Busy,
	}

	private _ActionState actionState = _ActionState.Idle;
	public _ActionState ActionState
	{
		get { return actionState; }
		set { actionState = value; }
	}

	private KodGames.ClientClass.CombatAvatarData avatarData;
	public KodGames.ClientClass.CombatAvatarData AvatarData
	{
		get { return avatarData; }
		set { avatarData = value; }
	}

	protected Vector3 footHoldPosition;  // Foothold position. 
	public Vector3 Foothold
	{
		get { return footHoldPosition; }
		set { footHoldPosition = value; }
	}

	private int avatarIndex = 0;
	public int AvatarIndex
	{
		get { return avatarIndex; }
	}

	private int teamIndex = 0;
	public int TeamIndex
	{
		get { return teamIndex; }
	}

	private int battlePosition;
	public int BattlePosition
	{
		get { return battlePosition; }
		set { battlePosition = value; }
	}

	private double avatarHP;
	public double AvatarHP
	{
		get { return avatarHP; }
		set
		{
			double maxHP = AvatarData.GetAttributeByType(_AvatarAttributeType.MaxHP).Value;
			avatarHP = Math.Max(Math.Min(maxHP, value), 0);
		}
	}

	private BattleRole targetRole;
	public BattleRole TargetRole
	{
		get { return targetRole; }
		set { targetRole = value; }
	}

	private bool pause = false;
	public bool Pause
	{
		get { return pause; }
		set
		{
			if (pause == value)
				return;

			pause = value;
			avatar.Pause = value;
		}
	}

	private Transform fxBone;
	public Vector3 FxPosition
	{
		get
		{
			if (fxBone == null)
			{
				foreach (var subObj in GetComponentsInChildren<Transform>())
				{
					if (subObj.gameObject.name.Equals("Bip01", StringComparison.CurrentCultureIgnoreCase))
					{
						fxBone = subObj.gameObject.transform;
						break;
					}
				}
			}

			if (fxBone != null)
				return fxBone.position;
			else
				return Vector3.zero;
		}
	}

	private UIButton3D click_button;
	public UIButton3D Click_Button
	{
		get
		{
			return click_button;
		}
		set
		{
			if (value != null && click_button != null)
				Destroy(click_button.gameObject);

			click_button = value;
		}
	}

	public override bool Hide
	{
		get { return base.Hide; }
		set
		{
			//Debug.LogWarning(string.Format("Role: {0} Hide:{1}", this.AvatarData.DisplayName, value));
			base.Hide = value;

			//隐藏的角色不能点击，也不应该被点击到，因为其BoxCollider可能会屏蔽掉可见角色的BoxCollider导致可见角色不能单击，这种情况经常出现在剧情战斗中
			if (Click_Button != null)
				Click_Button.gameObject.SetActive(!value);
		}
	}

	private UIAvatarBattleBar battleBar;
	public UIAvatarBattleBar BattleBar
	{
		get { return battleBar; }
		set
		{
			battleBar = value;

			if (battleBar != null)
			{
				battleBar.SetBattleRole(this);
			}
		}
	}

	public void InitializeSkillPower()
	{
		battleBar.hpBar.BarPowerValue = (float)avatarData.GetAttributeByType(_AvatarAttributeType.SP).Value;
	}

	private bool illusionOpenFlag = true;

	private List<int> combatStates = new List<int>(); // Combat states.
	protected List<DelayObject> delayObjects = new List<DelayObject>(); // Delay objects which need to waiting for.

	// When anyone SyngeneticDelayGameObject IsOver , BattleRole will immediate enter idle state and this list will be clear.
	protected List<SyngeneticDelayGameObject> syngeneticDelayObjects = new List<SyngeneticDelayGameObject>();

	protected Dictionary<int, Buff> bufDct = new Dictionary<int, Buff>(); // Buff dictionary.

	public Dictionary<int, Buff> GetBuffs()
	{
		return bufDct;
	}

	public static BattleRole Create(KodGames.ClientClass.CombatAvatarData avatarData, int teamIndex, int avatarIndex)
	{
		Avatar avatar = Avatar.CreateAvatar((teamIndex << 24 | avatarData.BattlePosition));
		BattleRole role = avatar.GetComponent<BattleRole>();
		if (role == null)
			role = avatar.gameObject.AddComponent<BattleRole>();
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		int avatarAssetId = avatarCfg.GetAvatarAssetId(avatarData.BreakThrough);
		role.AvatarAssetId = avatarAssetId;
		role.avatarData = new KodGames.ClientClass.CombatAvatarData();
		role.AvatarData.Copy(avatarData);

		role.teamIndex = teamIndex;
		role.avatarIndex = avatarIndex;
		role.battlePosition = avatarData.BattlePosition;
		role.AvatarHP = avatarData.GetAttributeByType(_AvatarAttributeType.HP).Value;

		role.Awake();

		return role;
	}

	public static BattleRole Create(int avatarResourceId, float maxHP, float hp, float sp, string name, int weaponResourceId)
	{
		Avatar avatar = Avatar.CreateAvatar(avatarResourceId);
		BattleRole role = avatar.GetComponent<BattleRole>();
		if (role == null)
			role = avatar.gameObject.AddComponent<BattleRole>();
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarResourceId);
		int avatarAssetId = avatarCfg.breakThroughs[0].assetId;
		role.AvatarAssetId = avatarAssetId;
		// Generate a CombatAvatarData 
		KodGames.ClientClass.CombatAvatarData bossAvatarData = new KodGames.ClientClass.CombatAvatarData();
		bossAvatarData.ResourceId = avatarResourceId;
		bossAvatarData.DisplayName = name;
		bossAvatarData.Attributes = new List<KodGames.ClientClass.Attribute>();
		KodGames.ClientClass.Attribute maxHPAttr = new KodGames.ClientClass.Attribute();
		maxHPAttr.Type = _AvatarAttributeType.MaxHP;
		maxHPAttr.Value = maxHP;
		bossAvatarData.Attributes.Add(maxHPAttr);
		KodGames.ClientClass.Attribute spAttr = new KodGames.ClientClass.Attribute();
		spAttr.Type = _AvatarAttributeType.SP;
		spAttr.Value = sp;
		bossAvatarData.Attributes.Add(spAttr);

		if (weaponResourceId != IDSeg.InvalidId)
		{
			KodGames.ClientClass.EquipmentData edata = new KodGames.ClientClass.EquipmentData();
			edata.BreakThrough = 0;
			edata.Id = weaponResourceId;
			bossAvatarData.Equipments.Add(edata);
		}

		role.avatarData = bossAvatarData;
		role.AvatarHP = hp;

		role.Awake();

		return role;
	}

	public static int GetBattlePositionRow(int battlePosition)
	{
		return (battlePosition >> 16) & 0xFFFF;
	}

	public static int GetBattlePositionColumn(int battlePosition)
	{
		return battlePosition & 0xFFFF;
	}

	public void CheckAndInitBattleFuncOpenStatus()
	{
#if UNITY_EDITOR //For BattleTest
		if (SysLocalDataBase.Inst != null)
#endif
			illusionOpenFlag = GameUtility.CheckFuncOpened(_OpenFunctionType.Illusion, false, true);
	}

	public int GetBattlePositionRow()
	{
		return GetBattlePositionRow(battlePosition);
	}

	public int GetBattlePositionColumn()
	{
		return GetBattlePositionColumn(battlePosition);
	}

	public int GetWeaponType()
	{
		foreach (var equipment in avatarData.Equipments)
		{
			EquipmentConfig.Equipment equipmentCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.Id);
			if (equipmentCfg != null && equipmentCfg.type == EquipmentConfig._Type.Weapon)
				return equipmentCfg.weaponType;
		}

		return IDSeg.InvalidId;
	}

	//TODO: 这里考虑到幻化时可能会出现问题
	public int GetWeaponAssetID()
	{
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		if (avatarCfg.weaponAssets.Count == 0)
			return IDSeg.InvalidId;
		return avatarCfg.weaponAssets[0].avatarAssetId;
	}

	public string GetWeaponAssetPath()
	{
		//TODO: 这里考虑到幻化时可能会出现问题
		return avatar.GetComponentAssetPath(GetWeaponAssetID());
	}

	// Clone weapon.
	//public GameObject CloneWeapon()
	//{
	//    // Clone the weapon object.
	//    GameObject obj = avatar.CloneComponent(GetWeaponAssetID());

	//    // Keep size when be detached.
	//    obj.transform.localScale = transform.localScale;


	//    return obj;
	//}

	// 替换橘色的默认武器
	public void ReplaceWeapon(int weaponId, string boneName)
	{
		// 删除角色的默认武器
		DeleteDefaultWeapons();

		//幻化武器不删除，如果技能添加的武器不替换掉幻化武器，保留幻化武器
		//使用新的武器
		avatar.UseComponent(weaponId, boneName);
	}

	// 恢复角色的默认武器
	public void RecoverWeapon(int weaponId)
	{
		// 删除覆盖的武器
		avatar.DeleteComponent(weaponId);

		// 使用角色的默认武器
		UseDefaultWeapons();
		UseIllusionWeapons();
	}

	public IllusionConfig.IllusionMode GetIllusionModel()
	{
		if (AvatarData.IllusionID != IDSeg.InvalidId)
		{
			IllusionConfig.Illusion illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(avatarData.IllusionID);
			return ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionModeById(illusionCfg.ModelId);
		}

		return null;
	}

	public void UseDefaultWeapons()
	{
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		foreach (var weaponAsset in avatarCfg.weaponAssets)
			if (weaponAsset.avatarAssetId != IDSeg.InvalidId)
				Avatar.UseComponent(weaponAsset.avatarAssetId, weaponAsset.mountBone);
	}


	//TODO：如果以后角色身上除了武器外可以挂其他Illusion（如肩甲），在IllusionConfig添加部位类型，左右手适配逻辑需修改
	//自动适配左右手
	const string leftHandBoneMarker = "marker_weaponL";
	const string rightHandBoneMarker = "marker_weaponR";
	public void UseIllusionWeapons()
	{
		if (!illusionOpenFlag)
			return;

		bool leftHand = false;
		bool rightHand = false;

		bool illusionLeftHand = false;
		bool illusionRightHand = false;

		//左右手适配。如果角色默认装备在左手（盖聂）而Illusion配置的bone是右手，要装备在左手，这种情况出现在
		//同时适用于盖聂和卫庄的幻化武器，但盖聂左手用剑，卫庄右手用剑。。。。。。
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		foreach (var weaponAsset in avatarCfg.weaponAssets)
		{
			if (weaponAsset.avatarAssetId != IDSeg.InvalidId)
			{
				if (!leftHand)
					leftHand = weaponAsset.mountBone == leftHandBoneMarker;

				if (!rightHand)
					rightHand = weaponAsset.mountBone == rightHandBoneMarker;
			}
		}

		IllusionConfig.IllusionMode illusionMode = GetIllusionModel();
		if (illusionMode != null)
		{

			if (illusionMode.WeaponId1 != IDSeg.InvalidId && !string.IsNullOrEmpty(illusionMode.BoneName1))
				illusionLeftHand = true;

			if (illusionMode.WeaponId2 != IDSeg.InvalidId && !string.IsNullOrEmpty(illusionMode.BoneName2))
				illusionRightHand = true;

			if (!illusionLeftHand && !illusionRightHand)
				return;

			if (illusionLeftHand && illusionRightHand)
			{
				//如果幻化配置了双手，直接使用幻化
				avatar.UseComponent(illusionMode.WeaponId1, illusionMode.BoneName1);
				avatar.UseComponent(illusionMode.WeaponId2, illusionMode.BoneName2);
			}
			//如果幻化配置的左右手与角色默认的左右手不同，挂在默认的左右手上。
			else if (illusionLeftHand)
			{
				if (rightHand)
					avatar.UseComponent(illusionMode.WeaponId1, rightHandBoneMarker);
				else
					avatar.UseComponent(illusionMode.WeaponId1, illusionMode.BoneName1);
			}
			else if (illusionRightHand)
			{
				if (leftHand)
					avatar.UseComponent(illusionMode.WeaponId2, leftHandBoneMarker);
				else
					avatar.UseComponent(illusionMode.WeaponId2, illusionMode.BoneName2);
			}
		}
	}

	/// <summary>
	/// 不包括技能添加的武器
	/// </summary>
	public void DeleteDefaultWeapons()
	{
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		foreach (var weaponAsset in avatarCfg.weaponAssets)
			if (weaponAsset.avatarAssetId != IDSeg.InvalidId)
				avatar.DeleteComponent(weaponAsset.avatarAssetId);
	}

	/// <summary>
	/// 不包括技能添加的武器
	/// </summary>
	public void DeleteIllusionWeapons()
	{
		IllusionConfig.IllusionMode illusionMode = GetIllusionModel();
		if (illusionMode != null)
		{
			if (illusionMode.WeaponId1 != IDSeg.InvalidId)
				avatar.DeleteComponent(illusionMode.WeaponId1);

			if (illusionMode.WeaponId2 != IDSeg.InvalidId)
				avatar.DeleteComponent(illusionMode.WeaponId2);
		}
	}

	public void HideWeapon(bool hide)
	{
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		foreach (var weaponAsset in avatarCfg.weaponAssets)
			avatar.SetActiveComponent(weaponAsset.avatarAssetId, !hide);

		IllusionConfig.IllusionMode illusionMode = GetIllusionModel();
		if (illusionMode != null)
		{
			if (illusionMode.WeaponId1 != IDSeg.InvalidId)
				avatar.SetActiveComponent(illusionMode.WeaponId1, !hide);

			if (illusionMode.WeaponId2 != IDSeg.InvalidId)
				avatar.SetActiveComponent(illusionMode.WeaponId2, !hide);
		}
	}

	public void AddCombatState(int combatState)
	{
		if (combatStates.Contains(combatState))
			return;

		if (LogAction)
			LogMsg("In combat state : " + _CombatStateType.GetNameByType(combatState));

		combatStates.Add(combatState);
		combatStates.Sort();

		// If role is in idle state, replay idle action to validate new state 
		if (actionState == _ActionState.Idle)
			PlayIdleAction();
	}

	public void RemoveCombatState(int combatState)
	{
		if (LogAction)
			LogMsg("Leave combat state : " + _CombatStateType.GetNameByType(combatState));

		combatStates.Remove(combatState);

		// If role is in idle state, replay idle action to validate new state 
		if (actionState == _ActionState.Idle)
			PlayIdleAction();
	}

	public void PlayIdleAction()
	{
		// Play default combat idle.
		PlayActionByType(AvatarAction._Type.CombatIdle);

		ChangeState(_ActionState.Idle);
	}

	public void PlayActionByType(int actionType)
	{
		// Play default combat idle.
		AvatarAction idleAction = GetActionByType(actionType);
		if (idleAction != null)
			base.PlayAction(idleAction.id);
	}

	public override bool PlayAction(int actID)
	{
		if (base.PlayAction(actID) == false)
			return false;

		ChangeState(_ActionState.Busy);

		return true;
	}

	// Stop action.
	public override void StopAction()
	{
		base.StopAction();

		if (LogAction)
			LogMsg("Clear delay object");

		// 防止递归调用造成死循环、栈溢出
		if (delayObjects.Count != 0)
		{
			var tempDelayObjects = delayObjects.ToArray();
			delayObjects.Clear();

			foreach (DelayObject obj in tempDelayObjects)
			{
				obj.Callback();
			}
		}

		ChangeState(_ActionState.Idle);
	}

	public override void Update()
	{
		if (Pause)
			return;

		base.Update();

		// Update delay objects.
		UpdateDelayObjects();
		UpdateSyngeneticDelayGameObjects();
	}

	// Update movement.
	protected override void UpdateMovement()
	{
		base.UpdateMovement();

		// If moving stopped, auto changed to idle state and play idle action.
		if (!isMoving) 	
		{
			if (CanChangeToState(_ActionState.Idle))
				PlayIdleAction();
		}
	}

	// Add delay object.
	public void AddDelayObject(DelayObject delayObj)
	{
		if (LogAction)
			LogMsg("Add delay object : " + delayObj.LogName);

		delayObjects.Add(delayObj);
	}

	// Update delay target which doing passive action.
	protected void UpdateDelayObjects()
	{
		if (delayObjects.Count == 0)
			return;

		for (int i = 0; i < delayObjects.Count; i++)
		{
			var delayObj = delayObjects[i];

			if (delayObj.IsOver())
			{
				if (LogAction)
					LogMsg("Delay object is over : " + delayObj.LogName);

				// Callback.
				delayObj.Callback();

				//尝试解决数组越界错误，delayObjects可能会被外界清空
				if (i >= delayObjects.Count)
					break;

				delayObjects.RemoveAt(i--);
			}
		}

		// If has no delay objects, changed to idle.
		if (delayObjects.Count == 0)
		{
			if (CanChangeToState(_ActionState.Idle))
				PlayIdleAction();
		}
	}

	public void AddSyngeneticDelayGameObject(SyngeneticDelayGameObject obj)
	{
		if (LogAction)
			LogMsg("Add syngenetic delay game object");

		syngeneticDelayObjects.Add(obj);
	}

	protected void UpdateSyngeneticDelayGameObjects()
	{
		if (syngeneticDelayObjects.Count == 0)
			return;

		foreach (var obj in syngeneticDelayObjects)
		{
			if (obj.IsOver())
			{
				PlayIdleAction();
				syngeneticDelayObjects.Clear();
				break;
			}
		}
	}

	public void ChangeState(_ActionState newState)
	{
		actionState = newState;
	}

	private bool CanChangeToState(_ActionState newState)
	{
		if (newState == _ActionState.Idle)
		{
			// Still has delay objects.
			if (delayObjects.Count > 0)
				return false;

			// Waiting for current once animation finished
			if (avatar.IsLoopAnim() == false && avatar.AvatarAnimation.IsEnd() == false)
				return false;

			// Still is moving.
			if (isMoving)
				return false;
		}

		return true;
	}

	protected override void OnAnimationFinished(object userData0, object userData1)
	{
		base.OnAnimationFinished(userData0, userData1);

		if (CanChangeToState(_ActionState.Idle))
			PlayIdleAction();
	}

	public AvatarAction GetActionByType(int actionType)
	{
		return GetActionByType(GetWeaponType(), actionType, UnityEngine.Random.Range(0, 1000));
	}

	private AvatarAction GetActionByType(int weaponType, int actionType, int rand)
	{
		AvatarAction actionCfg = null;

		// Get action by combat stage from the higher priority to lower priority
		for (int i = combatStates.Count - 1; i >= 0; i--)
		{
			actionCfg = GetRandomAction(weaponType, combatStates[i], actionType, rand);

			if (actionCfg == null)
				actionCfg = GetRandomAction(EquipmentConfig._WeaponType.Empty, combatStates[i], actionType, rand);

			if (actionCfg != null)
				return actionCfg;
		}

		actionCfg = GetRandomAction(weaponType, _CombatStateType.Default, actionType, rand);

		if (actionCfg == null)
			actionCfg = GetRandomAction(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, actionType, rand);

		// Can not find the action for the specific state, try to use default state action instead.
		return actionCfg;
	}

	private AvatarAction GetRandomAction(int weaponType, int stateType, int actionType, int randomer)
	{
		int actionCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(weaponType, stateType, actionType);
		if (actionCount == 0)
			return null;

		return ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(weaponType, stateType, actionType, randomer % actionCount);
	}

	// Add buffer by buffer id, and allocate one new instance id.
	public void AddBuff(int instID, int bufID)
	{
		Buff buf = new Buff(bufID, instID);
		bufDct[buf.instID] = buf;

		ProcessBuffs();
	}

	// Delete buffer by instance id.
	public void DelBuff(int instID)
	{
		if (bufDct.ContainsKey(instID))
		{
			bufDct.Remove(instID);

			ProcessBuffs();
		}
	}

	public bool DoesBuffExist(int instId)
	{
		return bufDct.ContainsKey(instId);
	}

	private void ProcessBuffs()
	{
		Color avatarColor = GameDefines.defaultAvatarColor;

		foreach (var kvp in bufDct)
		{
			ClientServerCommon.Buff buffCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetBuffById(kvp.Value.bufID);
			if (buffCfg == null)
				continue;

			for (int modifierSetIdx = 0; modifierSetIdx < buffCfg.modifierSets.Count; ++modifierSetIdx)
			{
				ClientServerCommon.PropertyModifierSet modifierSetCfg = buffCfg.modifierSets[modifierSetIdx];

				for (int idx = 0; idx < modifierSetCfg.modifiers.Count; ++idx)
				{
					PropertyModifier modifier = modifierSetCfg.modifiers[idx];
					if (modifier.type != PropertyModifier._Type.ColorModifier)
					{
						continue;
					}
					avatarColor = new UnityEngine.Color(modifier.color.r, modifier.color.g, modifier.color.b, modifier.color.a);
				}
			}
		}

		avatar.SetColor(avatarColor);
	}

	public bool IsDead()
	{
		return AvatarHP <= 0;
	}
}