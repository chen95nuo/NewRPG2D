using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

/// <summary>
/// 解析规则 "Type:AngleH|AngleV#"
/// </summary>
public class BattleSceneAngleData
{
	private const char battleFilter = ':';
	private const char dataFilter = '|';
	private const char battleDataFilter = '#';

	public class BattleSceneAngle
	{
		private int combatType;

		private float angleH;
		public float AngleH
		{
			get { return angleH; }
			set { angleH = value; }
		}

		private float angleV;
		public float AngleV
		{
			get { return angleV; }
			set { angleV = value; }
		}

		public BattleSceneAngle(int battleType)
		{
			this.combatType = battleType;
			//默认角度
			this.angleH = ConfigDatabase.DefaultCfg.GameConfig.combatSetting.battleDefault.GetBattleDefaultItemByCombatType(battleType).sceneCamAngleHDefault;
			this.angleV = 0f;
		}

		public override string ToString()
		{
			var toString = string.Format("{0}{1}", _CombatType.GetNameByType(combatType), battleFilter);
			toString += string.Format("{0}{1}", angleH, dataFilter);
			toString += string.Format("{0}{1}", angleV, dataFilter);

			return toString;
		}
	}

	private BattleSceneAngleData() { }

	private static BattleSceneAngleData instance;
	public static BattleSceneAngleData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BattleSceneAngleData();
				instance.Initialize();
			}

			return instance;
		}
	}

	private string saveKey;

	private void Initialize()
	{
		saveKey = SysLocalDataBase.Inst.LoginInfo.AccountId + "BattleSceneAngle" + Platform.Instance.ChannelId + SysLocalDataBase.Inst.LoginInfo.LastAreaId;
		dic_angles.Clear();
	}

	private Dictionary<int, BattleSceneAngle> dic_angles = new Dictionary<int, BattleSceneAngle>();

	public static void ClearLocalData()
	{
		instance = null;
	}

	private void Save(bool saveData)
	{
		string currentSceneStr = "";

		if (saveData)
		{
			string retainValue = PlayerPrefs.GetString(saveKey, string.Empty);
			string[] tempValues = retainValue.Split(battleDataFilter);

			foreach (var tempValue in tempValues)
			{
				bool ignore = false;
				foreach (var data in dic_angles)
				{
					if (tempValue.StartsWith(_CombatType.GetNameByType(data.Key)))
					{
						ignore = true;
						break;
					}
				}

				if (ignore == false && !string.IsNullOrEmpty(tempValue))
					currentSceneStr += string.Format("{0}{1}", tempValue, battleDataFilter);
			}

			foreach (var data in dic_angles)
			{
				if (string.IsNullOrEmpty(data.Value.ToString()))
					continue;

				currentSceneStr += string.Format("{0}{1}", data.Value.ToString(), battleDataFilter);
			}
		}

		PlayerPrefs.SetString(saveKey, currentSceneStr);
	}

	public void Put(int combatType, BattleSceneAngle battleSceneAngle)
	{
		if (!ConfigDatabase.DefaultCfg.GameConfig.GetLocalBattleAngleByCombatType(combatType))
			return;

		if (dic_angles.ContainsKey(combatType))
			dic_angles[combatType] = battleSceneAngle;
		else
			dic_angles.Add(combatType, battleSceneAngle);

		Save(true);
	}

	public BattleSceneAngle GetBattleSceneAngleByBattleType(int combatType)
	{
		if (!ConfigDatabase.DefaultCfg.GameConfig.GetLocalBattleAngleByCombatType(combatType))
			return null;

		if (dic_angles.ContainsKey(combatType))
			return dic_angles[combatType];

		var angleData = new BattleSceneAngle(combatType);
		dic_angles.Add(combatType, angleData);

		string retainValue = PlayerPrefs.GetString(saveKey, string.Empty);
		string[] tempValues = retainValue.Split(battleDataFilter);
		string sceneDataStr = string.Empty;

		for (int i = 0; i < tempValues.Length; i++)
		{
			if (tempValues[i].StartsWith(_CombatType.GetNameByType(combatType)))
			{
				sceneDataStr = tempValues[i];
				break;
			}
		}

		if (string.IsNullOrEmpty(sceneDataStr) == false)
		{
			tempValues = sceneDataStr.Split(battleFilter)[1].Split(dataFilter);
			angleData.AngleH = float.Parse(tempValues[0]);
			angleData.AngleV = float.Parse(tempValues[1]);
		}

		return dic_angles[combatType];
	}


}
