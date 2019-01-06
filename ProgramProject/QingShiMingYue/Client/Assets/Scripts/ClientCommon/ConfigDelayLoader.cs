using System;
namespace ClientServerCommon
{
	public static class ConfigDelayLoader
	{
		private static ConfigSetting cfgSetting = null;
		public static IFileLoader DelayLoadConfig(Type configType, out string fileName, out int fileFormat)
		{
			if (cfgSetting == null)
			{
#if UNITY_EDITOR
				cfgSetting = new ConfigSetting(Configuration._FileFormat.Xml);
#else
				cfgSetting = new ConfigSetting(Configuration._FileFormat.ProtoBufBinary);
#endif
				GameDefines.SetupConfigSetting(cfgSetting);
			}

			fileFormat = cfgSetting.FileFormat;
			fileName = cfgSetting.GetConfigName(configType);
			//Debug.Log("DelayLoadConfig : " + fileName);
			return new FileLoaderFromTextAsset(ResourceManager.Instance.LoadAsset<UnityEngine.TextAsset>(fileName, true));
		}
	}
}