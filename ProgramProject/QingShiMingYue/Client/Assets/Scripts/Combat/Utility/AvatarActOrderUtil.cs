using System;
using System.Collections.Generic;
public class AvatarActOrderUtil
{
	public static void ShowAvatarActOrderBatch(List<BattleRole> roles)
	{
		try
		{
			roles.Sort((m, n) =>
				{
					double speed1 = m.AvatarData.GetAttributeByType(ClientServerCommon._AvatarAttributeType.Speed).Value;
					double speed2 = n.AvatarData.GetAttributeByType(ClientServerCommon._AvatarAttributeType.Speed).Value;
					//Sort By Speed From the Biggest to Smallest.
					if (UnityEngine.Mathf.Approximately((float)speed1, (float)speed2)) return 0;
					else return speed1 > speed2 ? -1 : 1;
				});
		}
		catch (Exception e)
		{
			Debug.LogError("Sort Avatar By Speed Failed." + e.Message);
			return;
		}

		SysFx sysfx = SysModuleManager.Instance.GetSysModule<SysFx>();
		if (sysfx != null)
		{
			for (int i = 0; i < roles.Count; i++)
				sysfx.ShowAvatarActOrderFx(roles[i], i + 1);
		}
	}
}