using UnityEngine;
using System.Collections;

public class UIEffectSkillStart : MonoBehaviour
{
	public UIElemAvatarCard card;
	public Transform nameRoot;

	public void SetData(int avatarId, int skillId)
	{
		card.SetData(avatarId, false, false, null);
	}

	public void Clear()
	{
		card.Clear();
	}
}
