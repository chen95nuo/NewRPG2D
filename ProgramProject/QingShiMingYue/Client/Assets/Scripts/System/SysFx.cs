//#define FX_POOL_METRICS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

/// <summary>
/// Hurt effect text type.
/// </summary>
public class HURT_TEXT_TP
{
	public const int MISS = 1; // Miss text.
	public const int CRITICAL = 2; // Critical text.
	public const int HURT = 3; // HURT text.
	public const int BLOCK = 4; // Block text.
	public const int HEAL = 5; // Heal text.
	public const int SP_HURT = 6;//SP Damage
	public const int SP_CRITICAL = 7;//SP Critical Damage

	public const int INV = 0; // Invalid
}

/// <summary>
/// Effect system to provide effect in menu or battle. 
/// </summary>
public class SysFx : SysModule
{
	public static SysFx Instance
	{
		get { return SysModuleManager.Instance.GetSysModule<SysFx>(); }
	}

	// Animation curves.
	private AnimCurve animCurve;
	public AnimCurve AnimCurve
	{
		get { return animCurve; }
	}

	private Dictionary<string, FxPool> name_FxPools = new Dictionary<string, FxPool>();
	private Dictionary<GameObject, FxPool> go_FxPools = new Dictionary<GameObject, FxPool>();

	// Time scale.
	private float tmSclDrt; // Time scale duration.
	private bool tmScl; // Flag of time scale.

	public override bool Initialize()
	{
		// Create animation curve object and not destroy it when change scene.
		var anmCrvObj = ResourceManager.Instance.InstantiateAsset<GameObject>(GameDefines.anmCrvObj);
		anmCrvObj.name = System.IO.Path.GetFileName(GameDefines.anmCrvObj);
		anmCrvObj.SetActive(false);
		GameObject.DontDestroyOnLoad(anmCrvObj);
		animCurve = anmCrvObj.GetComponent<AnimCurve>();
		ObjectUtility.AttachToParentAndKeepWorldTrans(this.transform, animCurve.CachedTransform);

		return true;
	}

	public override void Dispose()
	{
		ReleaseAllFxPool();

		GameObject.Destroy(animCurve.gameObject);
	}

	public override void OnUpdate()
	{
		UpdateTimeScale();
	}

	#region FxPool Management
	public FXController CreateFx(string filePath)
	{
		FxPool fxPool;
		if (name_FxPools.TryGetValue(filePath, out fxPool) == false)
		{
			GameObject obj = ResourceManager.Instance.InstantiateAsset<GameObject>(filePath);
			if (obj == null)
				return null;

			obj.name = System.IO.Path.GetFileName(filePath);
			fxPool = new FxPool(this.CachedTransform, obj);

			name_FxPools.Add(filePath, fxPool);
		}

		return fxPool.Spawm();
	}

	public FXController CreateFxAndBuildParent(string filePath)
	{
		FxPool fxPool;
		if (name_FxPools.TryGetValue(filePath, out fxPool) == false)
		{
			GameObject obj = ResourceManager.Instance.InstantiateAsset<GameObject>(filePath);
			if (obj == null)
				return null;

			obj.name = System.IO.Path.GetFileName(filePath);
			if (obj.GetComponent<FXController>() == null)
			{
				Transform parentTrans = new GameObject(obj.name).transform;
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(parentTrans, obj.transform);

				// Reset root transform
				parentTrans.localPosition = Vector3.zero;
				parentTrans.localRotation = Quaternion.identity;
				parentTrans.localScale = Vector3.one;

				FXController fx = parentTrans.gameObject.AddComponent<FXController>();
				fx.loop = true;
				fx.autoDestroy = true;

				obj = parentTrans.gameObject;
			}

			fxPool = new FxPool(this.CachedTransform, obj);

			name_FxPools.Add(filePath, fxPool);
		}

		return fxPool.Spawm();
	}

	public void FreePooledFx()
	{
		foreach (var kvp in name_FxPools)
			kvp.Value.FreePooledFx();

		foreach (var kvp in go_FxPools)
			kvp.Value.FreePooledFx();
	}

	public void ReleaseAllFxPool()
	{
		foreach (var kvp in name_FxPools)
			kvp.Value.Release();
		name_FxPools.Clear();

		foreach (var kvp in go_FxPools)
			kvp.Value.Release();
		go_FxPools.Clear();
	}
	#endregion

	#region Time scaler
	// Scale game time.
	public void ScaleTime(float scale, float duration)
	{
		if (scale < 0 || duration < 0)
			return;

		tmScl = true;
		Time.timeScale = scale;
		tmSclDrt = Time.realtimeSinceStartup + duration;
	}

	// Resume game time.
	public void ResumeTimeScale()
	{
		tmSclDrt = 0;
	}

	public float GetScaleTime()
	{
		return Time.timeScale;
	}

	private void UpdateTimeScale()
	{
		if (tmScl)
		{
			if (tmSclDrt < Time.realtimeSinceStartup)
			{
				Time.timeScale = 1.0f;
				tmScl = false;
			}
		}
	}
	#endregion

	public FXController PlayFX(string fxName, GameObject parent, bool attachedToParent, bool autoDestroy, bool isResetLocalPos)
	{
		string fxPath = PathUtility.Combine(GameDefines.pfxPath, fxName);
		FXController fx = CreateFx(fxPath);
		if (fx == null)
			return null;

		// Attach to parent
		if (attachedToParent)
		{
			if (isResetLocalPos)
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(parent.transform, fx.Root);
			else
				ObjectUtility.AttachToParentAndKeepLocalTrans(parent.transform, fx.Root);
		}
		else
		{
			if (isResetLocalPos)
				ObjectUtility.UnifyWorldTrans(parent.transform, fx.Root);
		}

		// Set layer
		ObjectUtility.SetObjectLayer(fx.gameObject, parent.layer);

		if (autoDestroy)
		{
			// Set auto destroy flag to FX script.
			FXController pfxScp = fx.GetComponentInChildren<FXController>();

			if (pfxScp != null)
			{
				pfxScp.autoDestroy = true;
				pfxScp.loop = false;
			}
		}

		return fx;
	}

	// Pop UI object at specified world or UI position.
	public FXController PopUIObj(string packPath, string prefabName, Vector3 pos, bool inWorldSpace)
	{
		SysUIEnv uiEvn = SysUIEnv.Instance;
		if (uiEvn == null)
			return null;

		var fx = CreateFx(PathUtility.Combine(packPath, prefabName));

		Transform objTrans = fx.Root;
		Vector3 localScale = objTrans.localScale;
		ObjectUtility.AttachToParentAndResetLocalTrans(uiEvn.UIFxRoot, objTrans);
		objTrans.localScale = localScale;

		// Calculate actual position
		if (inWorldSpace)
		{
			// It's in world space, convert to UI space.
			Vector3 tmpPos = uiEvn.UICam.ViewportToWorldPoint(KodGames.Camera.main.WorldToViewportPoint(pos));
			tmpPos.z = uiEvn.UIFxRoot.position.z + uiEvn.DynamicLocalZ;
			objTrans.position = tmpPos;
		}
		else
		{
			objTrans.position = pos;
		}

		return fx;
	}

	// Pop hurt text according hurt type.
	public void PopHurtText(Vector3 pos, float pDltVal, int hurtType)
	{
		// Select hurt template game object.
		FXController hurtObj = null;

		if (hurtType == HURT_TEXT_TP.MISS)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtMiss, pos, true);
		else if (hurtType == HURT_TEXT_TP.CRITICAL)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtCrtc, pos, true);
		else if (hurtType == HURT_TEXT_TP.BLOCK)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtBlck, pos, true);
		else if (hurtType == HURT_TEXT_TP.HURT)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtHurt, pos, true);
		else if (hurtType == HURT_TEXT_TP.HEAL)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtHeal, pos, true);
		else if (hurtType == HURT_TEXT_TP.SP_HURT)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtSP_Hurt, pos, true);
		else if (hurtType == HURT_TEXT_TP.SP_CRITICAL)
			hurtObj = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiTxtSP_CrtcHurt, pos, true);

		if (hurtObj == null)
			return;

		var st = hurtObj.GetComponentInChildren<SpriteText>();
		if (st != null)
		{
			st.parseColorTags = true;
			st.Color = Color.white;

			if (hurtType == HURT_TEXT_TP.HURT || hurtType == HURT_TEXT_TP.CRITICAL)
				st.Text = string.Format("-{0:F0}", pDltVal);
			else if (hurtType == HURT_TEXT_TP.HEAL)
				st.Text = string.Format("+{0:F0}", pDltVal);
			else if (hurtType == HURT_TEXT_TP.SP_HURT||hurtType == HURT_TEXT_TP.SP_CRITICAL)
				st.Text = string.Format("{0:F0}", pDltVal);
		}
	}

	// Pop SKill UI
	public FXController PopSkill(Vector3 pos, BattleRole caster, int skillID)
	{
		AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(skillID);
		if (assetDescCfg == null)
			return null;

		var skillFx = PopUIObj(GameDefines.uiEffectPath, GameDefines.uiFxSkillStart, pos, false);
		SpriteText txSkillName = skillFx.GetComponentInChildren<SpriteText>();
		txSkillName.Text = assetDescCfg.name;

		caster.AddDelayObject(new DelayFX(skillFx, null, null, null));

		return skillFx;
	}

	//3、4星角色在角色头上弹技能名称，每个技能有独立的特效
	public FXController PopSuperSkillForNormalAvatar(Vector3 worldPos, BattleRole caster, int skillID)
	{
		AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(skillID);
		if (assetDescCfg == null)
			return null;

		SkillConfig.Skill skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillID);
		if (skillCfg == null)
			return null;

		//未配置
		if (string.IsNullOrEmpty(skillCfg.uiPfxName))
			return null;

		var skillFx = PopUIObj(GameDefines.pfxPath, skillCfg.uiPfxName, worldPos, false);
		//var skillFx = PopUIObj(GameDefines.pfxPath, "p_UI_SkillName_4_MengHuXiaShan", worldPos, false);

		//TODO:是否需要delay？
		//caster.AddDelayObject(new DelayFX(skillFx, null, null, null));

		return skillFx;
	}

	// Pop SKill UI
	public FXController PopSuperSkill(BattleRole caster, int skillID, bool sponsor)
	{
		AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(skillID);
		if (assetDescCfg == null)
			return null;

		SkillConfig.Skill skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillID);
		if (skillCfg == null)
			return null;

		// Get position
		Vector3 pos = Vector3.zero;
		UIPnlBattleBar uiBattleBar = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlBattleBar>();
		if (sponsor)
			pos = uiBattleBar.superSkillPosTop.transform.position;
		else
			pos = uiBattleBar.superSkillPosBottom.transform.position;

		// Create border FX
		string fxName = sponsor ? GameDefines.uiFxSuperSkillStart_Sponsor : GameDefines.uiFxSuperSkillStart_Opponent;
		var skillFx = PopUIObj(GameDefines.uiEffectPath, fxName, pos, false);

		// Create name FX
		PlayFX(skillCfg.uiPfxName, skillFx.gameObject, true, true, false);

		// Set target
		var skillStartFx = skillFx.GetComponent<UIEffectSkillStart>();
		skillStartFx.SetData(caster.AvatarData.ResourceId, skillID);

		var delayObj = new DelayFX(skillFx, OnPopSuperSkillFXFinishDelegate, skillFx, null);
		caster.AddDelayObject(delayObj);
		caster.AddSyngeneticDelayGameObject(new SyngeneticDelayGameObject(delayObj));

		return skillFx;
	}

	public void OnPopSuperSkillFXFinishDelegate(object data0, object data1)
	{
		FXController skillFx = data0 as FXController;
		if (skillFx == null)
			return;

		var skillStartFx = skillFx.GetComponent<UIEffectSkillStart>();
		skillStartFx.Clear();
	}

	public void ShowAvatarActOrderFx(BattleRole role, int num)
	{
		string fx = "";
		switch (num)
		{
			case 1: fx = GameDefines.fxAvatarBattleNum_1; break;
			case 2: fx = GameDefines.fxAvatarBattleNum_2; break;
			case 3: fx = GameDefines.fxAvatarBattleNum_3; break;
			case 4: fx = GameDefines.fxAvatarBattleNum_4; break;
			case 5: fx = GameDefines.fxAvatarBattleNum_5; break;
			case 6: fx = GameDefines.fxAvatarBattleNum_6; break;
		}
		if (fx == "") return;

		//Use SysFx
		//FXController fc = PlayFX(fx, role.Avatar.gameObject, false, true, true);
		//if (fc == null || fc.particleSystemDataArray.Length < 1)
		//{
		//	Debug.LogError("Create Avatar ActionNumFx Failed");
		//	return;
		//}
		//fc.particleSystemDataArray[0].particleSystem.startRotation = Mathf.PI;

		//Use AvatarFx
		string path = System.IO.Path.Combine(GameDefines.pfxPath, fx);
		FXController fc = CreateFx(path);
		if (fc == null || fc.particleSystemDataArray.Length < 1)
		{
			GameObject.Destroy(fc.gameObject);
			return;
		}
		fc.particleSystemDataArray[0].particleSystem.startRotation = Mathf.PI;
		role.Avatar.PlayPfx(fc, AvatarAction.Effect._DestroyType.Normal, int.MaxValue - 1, "", "", true, Vector3.zero, Vector3.zero, false, Vector3.zero);

	}

#if FX_POOL_METRICS
	public override void OnGUIUpdate()
	{
		base.OnGUIUpdate();

		GUILayout.BeginVertical();

		GUILayout.Label("------ SysFx ------");

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Free Pooled Fx"))
			FreePooledFx();

		if (GUILayout.Button("Clear all FX pool"))
			ReleaseAllFxPool();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Total FxPool Count : ");
		GUILayout.Label(name_FxPools.Count.ToString());
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("AllocCount : ");
		int allocCount = 0;
		foreach (var kvp in name_FxPools)
			allocCount += kvp.Value.AllocCount;
		GUILayout.Label(allocCount.ToString());
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("SpawmFromPoolCount : ");
		int spawmFromPoolCount = 0;
		foreach (var kvp in name_FxPools)
			spawmFromPoolCount += kvp.Value.SpawmFromPoolCount;
		GUILayout.Label(spawmFromPoolCount.ToString());
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("DespawmToPoolCount : ");
		int despawmToPoolCount = 0;
		foreach (var kvp in name_FxPools)
			despawmToPoolCount += kvp.Value.DespawmToPoolCount;
		GUILayout.Label(despawmToPoolCount.ToString());
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}
#endif
}