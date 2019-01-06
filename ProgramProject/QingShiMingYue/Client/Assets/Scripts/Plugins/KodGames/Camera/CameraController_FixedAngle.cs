using UnityEngine;
using System.Collections;

public class CameraController_FixedAngle : CameraController
{
	public override void Update()
	{
		base.Update();

		// Trace player
		if (traceTarget == null)
			return;

		// Update trace position
		transform.position = traceTarget.TracingPosition - transform.forward.normalized * GetTargetDistance();
	}
}