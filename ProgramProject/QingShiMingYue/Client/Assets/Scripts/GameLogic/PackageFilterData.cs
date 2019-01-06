using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

/// <summary>
/// 解析规则 key(Account Id) "DataType:AvatarTraitType@1|2|3|#AvatarCountryType@1|2|3#;DataType:EquipType;"
/// </summary>

public class PackageFilterData
{
	public class _DataType : TypeNameContainer<_DataType>
	{
		public const int UnKnow = -1;
		public const int PackageAvatar = 0;
		public const int PackageEquip = 1;
		public const int PackageSkill = 2;
		public const int SelectAvatar = 3;
		public const int SelectEquip = 4;
		public const int SelectSkill = 5;

		public static bool Initialize()
		{
			bool result = false;

			result &= RegisterType("PackageAvatar", PackageAvatar);
			result &= RegisterType("PackageEquip", PackageEquip);
			result &= RegisterType("PackageSkill", PackageSkill);
			result &= RegisterType("SelectAvatar", SelectAvatar);
			result &= RegisterType("SelectEquip", SelectEquip);
			result &= RegisterType("SelectSkill", SelectSkill);

			return result;
		}
	}

	public class _FilterType : TypeNameContainer<_FilterType>
	{
		public const int AvatarTraitType = 0;
		public const int AvatarCountryType = 1;
		public const int EquipType = 2;
		public const int QualityLevel = 3;

		public static bool Initialize()
		{
			bool result = false;

			result &= RegisterType("AvatarTraitType", AvatarTraitType);
			result &= RegisterType("AvatarCountryType", AvatarCountryType);
			result &= RegisterType("EquipType", EquipType);
			result &= RegisterType("QualityLevel", QualityLevel);

			return result;
		}
	}

	private const char packageFilter = ';';
	private const char packageValueFilter = ':';
	private const char packageDataFilter = '#';
	private const char packageDataValueFilter = '@';
	private const char dataFilter = '|';

	public class PackageFilter
	{
		private int packageType;
		private Dictionary<int, List<int>> filterDatas = new Dictionary<int, List<int>>();

		public PackageFilter(int packageType)
		{
			this.packageType = packageType;

			string retainValue = PlayerPrefs.GetString(savingKey, string.Empty);

			if (string.IsNullOrEmpty(retainValue))
				return;

			// 分割不同包裹类型（角色，装备）,获得packageType 类型。
			string[] tempValues = retainValue.Split(packageFilter);
			string packageStr = string.Empty;

			for (int i = 0; i < tempValues.Length; i++)
			{
				if (tempValues[i].StartsWith(_DataType.GetNameByType(packageType)))
				{
					packageStr = tempValues[i];
					break;
				}
			}

			if (string.IsNullOrEmpty(packageStr))
				return;

			// 分割得到当前包裹类型字符串。
			tempValues = packageStr.Split(packageValueFilter);
			if (tempValues.Length < 2 || string.IsNullOrEmpty(tempValues[1]))
				return;

			// 分割当前包裹类型下的所有分类字符串。
			tempValues = tempValues[1].Split(packageDataFilter);

			for (int i = 0; i < tempValues.Length; i++)
			{
				var filterTypeStr = tempValues[i].Split(packageDataValueFilter);
				if (filterTypeStr.Length < 2 || string.IsNullOrEmpty(filterTypeStr[1]))
					continue;

				int filterType = _FilterType.GetTypeByName(filterTypeStr[0]);
				filterDatas.Add(filterType, new List<int>());

				var dataStr = filterTypeStr[1].Split(dataFilter);

				for (int j = 0; j < dataStr.Length; j++)
				{
					if (string.IsNullOrEmpty(dataStr[j]))
						continue;

					this.filterDatas[filterType].Add(int.Parse(dataStr[j]));
				}
			}
		}

		public void Put(int filterType, List<int> values)
		{
			if (!filterDatas.ContainsKey(filterType))
				filterDatas.Add(filterType, new List<int>());

			filterDatas[filterType].Clear();
			for (int index = 0; index < values.Count; index++)
				filterDatas[filterType].Add(values[index]);
		}

		public void Save()
		{
			string currentPackageStr = string.Format("{0}{1}", _DataType.GetNameByType(packageType), packageValueFilter);
			foreach (var data in filterDatas)
			{
				var filterTypeStr = string.Format("{0}{1}", _FilterType.GetNameByType(data.Key), packageDataValueFilter);

				for (int i = 0; i < data.Value.Count; i++)
					filterTypeStr += string.Format("{0}{1}", data.Value[i], dataFilter);

				currentPackageStr += string.Format("{0}{1}", filterTypeStr, packageDataFilter);
			}

			string saveStr = string.Empty;
			string retainValue = PlayerPrefs.GetString(savingKey, string.Empty);

			string[] tempValues = retainValue.Split(packageFilter);
			for (int i = 0; i < tempValues.Length; i++)
			{
				if (tempValues[i].StartsWith(_DataType.GetNameByType(packageType)))
					saveStr += currentPackageStr;
				else
					saveStr += tempValues[i];

				saveStr += packageFilter;
			}

			if (!saveStr.Contains(_DataType.GetNameByType(packageType)))
				saveStr += currentPackageStr + packageFilter;

			PlayerPrefs.SetString(savingKey, saveStr);
		}

		public List<int> GetFilterDataByType(int filterType)
		{
			if (!filterDatas.ContainsKey(filterType) || filterDatas[filterType] == null)
				filterDatas.Add(filterType, new List<int>());

			return filterDatas[filterType];
		}
	}

	private Dictionary<int, PackageFilter> packgeFilters = new Dictionary<int, PackageFilter>();
	private PackageFilterData() { }

	private static string savingKey;

	private static PackageFilterData instance;
	public static PackageFilterData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new PackageFilterData();
				instance.Initialize();
			}

			return instance;
		}
	}

	private void Initialize()
	{
		if (_DataType.GetRegisterTypeCount() <= 0)
			_DataType.Initialize();

		if (_FilterType.GetRegisterTypeCount() <= 0)
			_FilterType.Initialize();

		InitDefaultValue();
	}

	public static void ClearLocalData()
	{
		instance = null;
	}

	private void InitDefaultValue()
	{
		// Init Data.
		savingKey = SysLocalDataBase.Inst.LoginInfo.AccountId + "PackageFilter" + Platform.Instance.ChannelId + SysLocalDataBase.Inst.LoginInfo.LastAreaId; ;

		packgeFilters.Clear();
		packgeFilters.Add(_DataType.PackageAvatar, new PackageFilter(_DataType.PackageAvatar));
		packgeFilters.Add(_DataType.PackageEquip, new PackageFilter(_DataType.PackageEquip));
		packgeFilters.Add(_DataType.PackageSkill, new PackageFilter(_DataType.PackageSkill));
		packgeFilters.Add(_DataType.SelectAvatar, new PackageFilter(_DataType.SelectAvatar));
		packgeFilters.Add(_DataType.SelectEquip, new PackageFilter(_DataType.SelectEquip));
		packgeFilters.Add(_DataType.SelectSkill, new PackageFilter(_DataType.SelectSkill));

		// Init Local.
		string retainValue = PlayerPrefs.GetString(savingKey, string.Empty);

		if (!string.IsNullOrEmpty(retainValue))
			return;

		foreach (var packageFilter in packgeFilters)
		{
			List<int> filterValues = new List<int>();

			switch (packageFilter.Key)
			{
				case _DataType.PackageAvatar:
				case _DataType.SelectAvatar:

					// Avatar Trait Type.
					filterValues.Clear();
					for (int i = AvatarConfig._AvatarTraitType.UnKnow + 1; i <= AvatarConfig._AvatarTraitType.Heal; i++)
						filterValues.Add(i);
					packageFilter.Value.Put(_FilterType.AvatarTraitType, filterValues);

					// Avatar Country type.
					filterValues.Clear();

					for (int i = 0; i < AvatarConfig._AvatarCountryType.GetRegisterTypeCount(); i++)
						filterValues.Add(AvatarConfig._AvatarCountryType.GetRegisterTypeByIndex(i));

					packageFilter.Value.Put(_FilterType.AvatarCountryType, filterValues);
					break;

				case _DataType.PackageEquip:

					// Equipment Type.
					filterValues.Clear();
					for (int i = EquipmentConfig._Type.Unknown + 1; i <= EquipmentConfig._Type.Treasure; i++)
						filterValues.Add(i);
					packageFilter.Value.Put(_FilterType.EquipType, filterValues);

					break;
			}

			// Quality Type.
			filterValues.Clear();
			for (int quality = 1; quality <= 5; quality++)
				filterValues.Add(quality);
			packageFilter.Value.Put(_FilterType.QualityLevel, filterValues);

			packageFilter.Value.Save();
		}
	}

	public PackageFilter GetPackgetFilterByType(int type)
	{
		if (packgeFilters.ContainsKey(type))
			return packgeFilters[type];

		return null;
	}
}
