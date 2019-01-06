using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

//剧情战斗，屏幕上的特效（类似暴走的诗句等），支持播放队列
public class PlotBattleInterludeEffectRound : BattleRound
{
	string curEffectPrefabName = string.Empty;
	UIEffectPlotBattleInterlude currentEffect = null;

	//特效队列，多个特效一个接一个无缝链接播放
	List<string> effectQueue;

	//最后一个特效后不能看到场景，应为要直接切到秘境或主城，此时不删除过场特效，让其遮住战斗场景。
	bool isEndEffect;

	public PlotBattleInterludeEffectRound(RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
		if (roundRecord == null)
		{
			Debug.LogError("[PlotBattleInterludeEffectRound] roundRecord is null");
			return;
		}

		//从配置中解析出特效的prefab名称
		if (roundRecord.configParameterDic != null)
		{
			effectQueue = new List<string>();
			foreach (var tempPair in roundRecord.configParameterDic)
			{
				if (string.IsNullOrEmpty(tempPair.Value))
				{
					Debug.LogError(string.Format("[PlotBattleInterludeEffectRound] roundParameter error. Invalid prefab name. Key={0}", tempPair.Key));
					continue;
				}

				if (tempPair.Key.StartsWith("PlotBattleInterludeEffectPrefab"))
					effectQueue.Insert(0, tempPair.Value);
			}

			isEndEffect = StrParser.ParseBool(roundRecord.Parameter("IsEndEffect"), false);
		}
	}

	//播放下一个特效
	void playNextEffect()
	{
		if (effectQueue == null || effectQueue.Count == 0)
			return;

		//Destroy previous effect.
		if (currentEffect != null)
			MonoBehaviour.Destroy(currentEffect.gameObject);

		curEffectPrefabName = effectQueue[effectQueue.Count - 1];

		var uiPrefab = ResourceManager.Instance.InstantiateAsset<GameObject>(GameDefines.uiModulePath + "/" + curEffectPrefabName);

		UIPnlBattleBar uiBattleBar = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlBattleBar>();

		ObjectUtility.AttachToParentAndResetLocalTrans(uiBattleBar.superSkillPosTop, uiPrefab);

		currentEffect = uiPrefab.GetComponent<UIEffectPlotBattleInterlude>();

		effectQueue.RemoveAt(effectQueue.Count - 1);
	}

	public override bool Start()
	{
		if (!base.Start())
			return false;

		if (effectQueue == null || effectQueue.Count == 0)
			return true;

		playNextEffect();

		//主场景已被遮挡，不必渲染
		KodGames.Camera.main.enabled = false;

		return true;
	}

	public override void Update()
	{
		base.Update();

		if (currentEffect != null && currentEffect.IsOver)
			playNextEffect();
	}

	public override bool CanStartAfterRound()
	{
		if (!base.CanStartAfterRound())
			return false;

		if (effectQueue != null && effectQueue.Count > 0)
			return false;

		if (currentEffect == null)
			return true;

		if (currentEffect.IsOver)
		{
			//如果是最后的结束特效，则用来遮挡场景及UI，不删除，从而在且回战前场景的瞬间看不到战斗场景
			if (!isEndEffect)
				MonoBehaviour.Destroy(currentEffect.gameObject);

			return true;
		}
		else
			return false;
	}

	public override void Finish()
	{
		base.Finish();
		//主场景恢复渲染
		KodGames.Camera.main.enabled = true;

		//如果是最后的结束特效，则用来遮挡场景及UI，不删除，从而在且回战前场景的瞬间看不到战斗场景
		if (currentEffect != null && !isEndEffect)
			GameObject.Destroy(currentEffect.gameObject);

		//跳过战斗时BattleRecordPlayer会直接调用该方法，清空播放队列，否则不能开始下一个Round（CanStartAfterRound），导致无法跳过
		if (effectQueue != null && battleRecordPlayer.IsSkip)
		{
			effectQueue.Clear();
			effectQueue = null;
		}
	}
}
