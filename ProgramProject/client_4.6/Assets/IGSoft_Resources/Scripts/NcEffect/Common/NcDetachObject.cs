// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class NcDetachObject : NcEffectBehaviour
{
	// Attribute ------------------------------------------------------------------------
	public		GameObject			m_LinkGameObject;

	// Property -------------------------------------------------------------------------
	public static NcDetachObject Create(GameObject parentObj, GameObject linkObject)
	{
		NcDetachObject deObj = parentObj.AddComponent<NcDetachObject>();
		deObj.m_LinkGameObject = linkObject;
		return deObj;
	}

	// Function --------------------------------------------------------------------
	// Loop Function --------------------------------------------------------------------
	// Control Function -----------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		if (bRuntime)
 			AdjustSpeedRuntime(m_LinkGameObject, fSpeedRate);
	}

	// utility fonction ----------------------------------------------------------------
}
