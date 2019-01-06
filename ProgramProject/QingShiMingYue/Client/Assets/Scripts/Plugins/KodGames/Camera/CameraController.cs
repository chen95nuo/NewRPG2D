using UnityEngine;
using System.Collections;

public interface ICameraTraceable
{
	Vector3 TracingPosition { get; }
}

public class CameraController : MonoBehaviour
{
	protected ICameraTraceable traceTarget;
	public ICameraTraceable TraceTarget
	{
		get { return traceTarget; }
	}

	private class EasingData
	{
		public float timer;
		public float duration;
		public EZAnimation.Interpolator interpolator;
	}

	private class FloatEasingData : EasingData
	{
		public float targetDistance;
	}

	private float targetDistance = 0;
	FloatEasingData targetDistanceEasingData;

	public void SetTraceTarget(ICameraTraceable traceTarget, float distance, float duration, EZAnimation.EASING_TYPE easingType)
	{
		this.traceTarget = traceTarget;

		if (traceTarget != null && distance >= 0)
		{
			if (this is CameraController_Touch)
			{
				((CameraController_Touch)this).traceTargetSeted = false;
			}
			
			if (duration != 0)
			{
				FloatEasingData easingData = new FloatEasingData();
				easingData.timer = 0;
				easingData.duration = duration;
				easingData.interpolator = EZAnimation.GetInterpolator(easingType);
				easingData.targetDistance = distance;

				targetDistanceEasingData = easingData;
			}
			else
			{
				targetDistance = distance;
				targetDistanceEasingData = null;
			}
		}
	}

	public float GetTargetDistance()
	{
		if (targetDistanceEasingData != null)
		{
			return targetDistanceEasingData.interpolator(targetDistanceEasingData.timer, targetDistance, targetDistanceEasingData.targetDistance - targetDistance, targetDistanceEasingData.duration);
		}
		else
		{
			return targetDistance;
		}
	}

	//public void SetMinDistance(float distance, float duration, EZAnimation.EASING_TYPE easingType)
	//{
	//}


	//public void SetMaxDistance(float distance, float duration, EZAnimation.EASING_TYPE easingType)
	//{
	//    currDistance = Mathf.Lerp(distance, maxDistance, duration);

	//}

	//public void ResetDistance()
	//{
	//    currDistance = defaultDistance;
	//}

	//public void Zoom(float distanceDelta)
	//{
	//    Vector3 direction = (transform.position - traceTarget.TracingPosition).normalized;
	//    Vector3 translation = distanceDelta*direction;
	//    transform.Translate(translation);
	//}	

	public virtual void Update()
	{
		if (targetDistanceEasingData != null)
		{
			targetDistanceEasingData.timer += Time.deltaTime;

			if (targetDistanceEasingData.timer >= targetDistanceEasingData.duration)
			{
				targetDistance = targetDistanceEasingData.targetDistance;

				targetDistanceEasingData = null;
			}
		}
	}

	public void Shake(float intensity, float duration, float interval)
	{
		if (this.transform.parent.gameObject.GetComponent<KodGames.Effect.CameraShaker>() != null)
		{
			this.transform.parent.gameObject.GetComponent<KodGames.Effect.CameraShaker>().ForceToFinished();
			//return;
		}

		// Add a new game object for shaking
		GameObject shakeObj = new GameObject("shaker");
		shakeObj.transform.parent = this.transform.parent;
		this.transform.parent = shakeObj.transform;

		KodGames.Effect.CameraShaker shaker = shakeObj.AddComponent<KodGames.Effect.CameraShaker>();
		shaker.AutoDestroy = true;
		shaker.DestroyGameObject = true;
		shaker.Shake(intensity, duration, interval);
	}
}