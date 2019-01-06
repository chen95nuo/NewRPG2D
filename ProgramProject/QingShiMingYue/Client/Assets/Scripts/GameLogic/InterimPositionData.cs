using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;

/// <summary>
/// 解析规则 key(hiuegherst) "Type:PositionId@employLocationId%employShowLocationId$ShowLocationId,LocationID|ShowLocationId,LocationID|ShowLocationId,LocationID|#"
/// </summary>

public class InterimPositionData
{
	private const char positionFilter = ';';
	private const char positionValueFilter = ':';
	private const char positionIndexFilter = '%';
	private const char positionSubIndexFilter = '$';
	private const char positionDataValueFilter = '@';
	private const char dataFilter = '|';
	private const char locationFilter = ',';
	private const char positionDataFilter = '#';

	public class LocationData
	{
		public int showLocationId;
		public int locationId;
	}

	public class LocalPositionData
	{
		private int combatType;
		public int employLocationId;
		public int employShowLocationId;
		public int positionId;
		public List<LocationData> locationDatas = new List<LocationData>();

		public LocalPositionData(int combatType)
		{
			this.combatType = combatType;
		}

		public LocationData GetLocationDataByLocationId(int locationId)
		{
			foreach (var location in locationDatas)
				if (location.locationId == locationId)
					return location;

			return null;
		}

		public LocationData GetLocationDataByShowLocationId(int showLocationId)
		{
			foreach (var location in locationDatas)
				if (location.showLocationId == showLocationId)
					return location;

			return null;
		}

		public void Add(LocalPositionData positionData)
		{
			this.employLocationId = positionData.employLocationId;
			this.employShowLocationId = positionData.employShowLocationId;
			this.positionId = positionData.positionId;
			this.locationDatas = positionData.locationDatas;
		}

		public void Clear()
		{
			this.employLocationId = -1;
			this.positionId = -1;
			this.locationDatas.Clear();
		}

		public override string ToString()
		{
			var positionTypeStr = string.Format("{0}{1}", _CombatType.GetNameByType(combatType), positionValueFilter);
			positionTypeStr += string.Format("{0}{1}", positionId, positionDataValueFilter);
			positionTypeStr += string.Format("{0}{1}", employLocationId, positionIndexFilter);
			positionTypeStr += string.Format("{0}{1}", employShowLocationId, positionSubIndexFilter);

			foreach (var location in locationDatas)
			{
				positionTypeStr += string.Format("{0}{1}", location.locationId, locationFilter);
				positionTypeStr += string.Format("{0}{1}", location.showLocationId, locationFilter);
				positionTypeStr = string.Format("{0}{1}", positionTypeStr, dataFilter);
			}

			return positionTypeStr;
		}
	}

	private InterimPositionData() { }

	private Dictionary<int, LocalPositionData> positionDatas = new Dictionary<int, LocalPositionData>();

	private static InterimPositionData instance;
	public static InterimPositionData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new InterimPositionData();
				instance.Initialize();
			}

			return instance;
		}
	}

	private string saveKey;

	private void Initialize()
	{
		saveKey = SysLocalDataBase.Inst.LoginInfo.AccountId + "PositionData" + Platform.Instance.ChannelId + SysLocalDataBase.Inst.LoginInfo.LastAreaId;
		positionDatas.Clear();
	}

	public static void ClearLocalData()
	{
		instance = null;
	}

	public void Put(int combatType, LocalPositionData value)
	{
		if (!ConfigDatabase.DefaultCfg.GameConfig.GetLocalRecordStateByCombatType(combatType))
			return;

		if (!positionDatas.ContainsKey(combatType))
			positionDatas.Add(combatType, new LocalPositionData(combatType));

		positionDatas[combatType].Clear();
		positionDatas[combatType].Add(value);
		Save(true);
	}

	private void Save(bool saveData)
	{
		string currentPackageStr = "";

		if (saveData)
		{
			string retainValue = PlayerPrefs.GetString(saveKey, string.Empty);
			string[] tempValues = retainValue.Split(positionDataFilter);

			foreach (var tempValue in tempValues)
			{
				bool ignore = false;
				foreach (var data in positionDatas)
				{
					if (tempValue.StartsWith(_CombatType.GetNameByType(data.Key)))
					{
						ignore = true;
						break;
					}
				}

				if (ignore == false && !string.IsNullOrEmpty(tempValue))
					currentPackageStr += string.Format("{0}{1}", tempValue, positionDataFilter);
			}

			foreach (var data in positionDatas)
			{
				if (string.IsNullOrEmpty(data.Value.ToString()))
					continue;

				currentPackageStr += string.Format("{0}{1}", data.Value.ToString(), positionDataFilter);
			}
		}

		PlayerPrefs.SetString(saveKey, currentPackageStr);
	}

	public LocalPositionData GetPositionDataByType(int combatType)
	{
		if (!ConfigDatabase.DefaultCfg.GameConfig.GetLocalRecordStateByCombatType(combatType))
			return null;

		if (positionDatas.ContainsKey(combatType))
			return positionDatas[combatType];

		var positionData = new LocalPositionData(combatType);
		positionDatas.Add(combatType, positionData);

		string retainValue = PlayerPrefs.GetString(saveKey, string.Empty);

		string[] tempValues = retainValue.Split(positionDataFilter);
		string packageStr = string.Empty;

		for (int i = 0; i < tempValues.Length; i++)
		{
			if (tempValues[i].StartsWith(_CombatType.GetNameByType(combatType)))
			{
				packageStr = tempValues[i];
				break;
			}
		}

		if (string.IsNullOrEmpty(packageStr))
		{
			var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

			positionData.positionId = position.PositionId;
			positionData.employLocationId = position.EmployLocationId;
			positionData.employShowLocationId = position.EmployShowLocationId;
			positionData.locationDatas.Clear();

			foreach (var locationPair in position.Pairs)
			{
				var locationData = new LocationData();
				locationData.locationId = locationPair.LocationId;
				locationData.showLocationId = locationPair.ShowLocationId;
				positionData.locationDatas.Add(locationData);
			}
		}
		else
		{
			// TempValues : PositionId@employLocationId%employShowLocationId$ShowLocationId,LocationID
			tempValues = packageStr.Split(positionValueFilter)[1].Split(positionDataValueFilter);
			positionData.positionId = int.Parse(tempValues[0]);

			// TempValues : employLocationId%employShowLocationId$ShowLocationId,LocationID
			tempValues = tempValues[1].Split(positionIndexFilter);
			positionData.employLocationId = int.Parse(tempValues[0]);

			// TempValues : employShowLocationId$ShowLocationId,LocationID|ShowLocationId,locationId|
			tempValues = tempValues[1].Split(positionSubIndexFilter);
			positionData.employShowLocationId = int.Parse(tempValues[0]);

			// TempValues : ShowLocationId,LocationID|ShowLocationId,locationId|
			tempValues = tempValues[1].Split(dataFilter);

			positionData.locationDatas.Clear();

			for (int i = 0; i < tempValues.Length; i++)
			{
				var locationStr = tempValues[i].Split(locationFilter);

				if (string.IsNullOrEmpty(tempValues[i]) || locationStr.Length < 2)
					continue;

				var location = new LocationData();
				location.locationId = int.Parse(locationStr[0]);
				location.showLocationId = int.Parse(locationStr[1]);
				positionData.locationDatas.Add(location);
			}
		}

		return positionDatas[combatType];
	}
}
