using System.Collections;
using UnityEngine;

namespace KodGames.Effect
{
	/// <summary>
	/// Camera shake class, to provide shaking camera effect. 
	/// </summary>
	public class CameraShaker : MonoBehaviour
	{
		private bool autoDestroy;
		public bool AutoDestroy
		{
			get { return autoDestroy; }
			set { autoDestroy = value; }
		}

		private bool destroyGameObject;
		public bool DestroyGameObject
		{
			get { return destroyGameObject; }
			set { destroyGameObject = value; }
		}

		// Camera shake.
		protected UnityEngine.Camera cam;
		protected float camShkTime; // Shake time.
		protected float camShkDrt; // Shake duration.
		protected float camShkLastTime;
		protected float camShkInty; // Shake intensity.
		protected float camShkIntv; // Shake interval.
		protected Vector3 camShkOfs = Vector3.zero;
		protected Vector3 camShkStart; // Camera shake start position.

		public static void Shake(GameObject gameObject, float intensity, float duration, float interval)
		{
			var cameraShaker = gameObject.GetComponent<KodGames.Effect.CameraShaker>();
			if (cameraShaker == null)
			{
				cameraShaker = gameObject.gameObject.AddComponent<KodGames.Effect.CameraShaker>();
				cameraShaker.AutoDestroy = true;
			}

			cameraShaker.Shake(intensity, duration, interval);
		}

		// Shake with intensity, duration and interval.
		public void Shake(float intensity, float duration, float interval)
		{
			if (intensity == 0)
				return;

			camShkStart = transform.localPosition;
			camShkDrt = duration;
			camShkInty = intensity;
			camShkIntv = interval;

			camShkTime = camShkDrt;
			camShkLastTime = camShkTime;

			StopShake();

			StartCoroutine("UpdateShake");
		}

		private void StopShake()
		{
			transform.localPosition = camShkStart;

			StopCoroutine("UpdateShake");
		}

		// Adjust camera shake position.
		public void AdjustShkStart(Vector3 delta)
		{
			camShkStart += delta;
		}

		// Shake coroutine function.
		[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
		protected IEnumerator UpdateShake()
		{
			// Adjust camera position during shaking time.
			Transform cachedTrans = transform;
			while (camShkTime > 0)
			{
				camShkTime -= Time.deltaTime;

				if (camShkTime <= 0)
				{
					camShkTime = 0;
					camShkOfs = Vector3.zero;
					continue;
				}

				if (camShkLastTime - camShkTime > camShkIntv)
				{
					float r = camShkInty * (camShkTime / camShkDrt);
					//float quickUp = r * ( UnityEngine.Random.Range( 0, 2 ) * 2 - 1 );
					//float quickLeft = r * ( UnityEngine.Random.Range( 0, 2 ) * 2 - 1 );

					float quickUp = UnityEngine.Random.Range(-r, r);
					float quickLeft = UnityEngine.Random.Range(-r, r);

					camShkOfs = quickUp * transform.up + quickLeft * transform.right;
					camShkLastTime = camShkTime;

					cachedTrans.localPosition = camShkStart + camShkOfs;
				}

				yield return null;
			}

			cachedTrans.localPosition = camShkStart;

			OnShakeFinished();
		}

		public void ForceToFinished()
		{
			transform.localPosition = camShkStart;
			OnShakeFinished();
		}

		private void OnShakeFinished()
		{
			if (autoDestroy)
			{
				if (destroyGameObject)
				{
					foreach (var child in GetComponentsInChildren<Transform>())
						if (child.parent == this.transform)
							child.parent = transform.parent;

					Object.Destroy(this.gameObject);
				}
				else
				{
					Object.Destroy(this);
				}
			}
		}
	}
}
