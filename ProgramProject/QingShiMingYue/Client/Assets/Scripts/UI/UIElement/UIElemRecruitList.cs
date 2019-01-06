using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemRecruitList : MonoBehaviour
{
	public List<UIElemAssetIcon> recruitAvatars;

	public void SetData(List<int> avatarIds)
	{
		for (int i = 0; i < recruitAvatars.Count; i++ )
		{
			recruitAvatars[i].Hide(true);
		}

		for (int i = 0; i < recruitAvatars.Count && i < avatarIds.Count; i++)
		{
			recruitAvatars[i].Hide(false);
			recruitAvatars[i].SetData(avatarIds[i]);
			recruitAvatars[i].border.Data = avatarIds[i];
		}
	}
}