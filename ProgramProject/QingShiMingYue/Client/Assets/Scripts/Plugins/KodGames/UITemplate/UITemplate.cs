#if UNITY_EDITOR
﻿using UnityEngine;

public abstract class UITemplate : MonoBehaviour
{
	public abstract System.Type GetTargetType();
	public abstract bool Apply(Component component);
}
#endif
