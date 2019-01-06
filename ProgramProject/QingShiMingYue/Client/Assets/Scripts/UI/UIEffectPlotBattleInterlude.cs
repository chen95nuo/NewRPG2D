using UnityEngine;
using System.Collections.Generic;

public class UIEffectPlotBattleInterlude : MonoBehaviour
{
	public bool IsOver { get { return timer >= effectDuration; } }
	
	public float effectDuration=0;

	float timer = 0;

	void Update()
	{
		timer += Time.deltaTime;
	}
}