using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemArenaRulesItem : MonoBehaviour
{
	public SpriteText rule;

	public void SetData(string ruleText)
	{
		rule.Text = ruleText;
	}
	
}
