using System;
using System.Collections;

public class AutoFollowObject : MonoBehaviour
{
	private UnityEngine.GameObject target;
	private UnityEngine.Vector3 offset;

	private UnityEngine.Transform targetTransform;
	public UnityEngine.Transform TargetTransform
	{
		get
		{
			if (target == null)
				return null;

			if (targetTransform == null)
				targetTransform = target.transform;

			return targetTransform;
		}
	}

	private UnityEngine.Vector3 targetPos;

	public void SetTarget(UnityEngine.GameObject target, UnityEngine.Vector3 offset)
	{
		this.target = target;
		this.offset = offset;

		UpdatePosition();
	}

	public void Update()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		if (target == null)
			return;

		if (SysUIEnv.Instance.IsUIObject(target))
		{
			if (CachedTransform.position != TargetTransform.position)
			{
				CachedTransform.position = TargetTransform.position;
				CachedTransform.localPosition = new UnityEngine.Vector3(CachedTransform.localPosition.x, CachedTransform.localPosition.y, -0.001f);
				CachedTransform.localPosition += offset;
			}
		}
		else
		{
			targetPos = UnityEngine.Camera.main.WorldToScreenPoint(TargetTransform.position);
			targetPos = SysUIEnv.Instance.UICam.ScreenToWorldPoint(targetPos) + new UnityEngine.Vector3(0, 0, -1);
			if (targetPos != CachedTransform.position)
			{
				CachedTransform.position = targetPos;
				CachedTransform.localPosition += offset;
			}
		}
	}

	public void OnDisable()
	{
		target = null;
	}
}