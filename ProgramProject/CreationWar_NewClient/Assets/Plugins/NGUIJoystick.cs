using UnityEngine;
using System.Collections;

public class NGUIJoystick : MonoBehaviour
{
	public float radius = 2.0f;
	public Vector3 scale = Vector3.one;
	private Plane mPlane;
	private Vector3 mLastPos;
	private UIPanel mPanel;
	private Vector3 center;
	[HideInInspector]
	public Vector2 position;
	
	private void Start ()
	{
		center = transform.localPosition;
	}
	
	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>
	
	private void OnPress (bool pressed)
	{
		if (enabled && gameObject.active) {
			if (pressed) {
				mLastPos = UICamera.lastHit.point;
				mPlane = new Plane (Vector3.back, mLastPos);
			} else {
				transform.localPosition = center;
				position=Vector2.zero;
			}
		}
	}

	/// <summary>
	/// Drag the object along the plane.
	/// </summary>

	void OnDrag (Vector2 delta)
	{
		if (enabled && gameObject.active) {
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

			Ray ray = UICamera.currentCamera.ScreenPointToRay (UICamera.currentTouch.pos);
			float dist = 0f;

			if (mPlane.Raycast (ray, out dist)) {
				Vector3 currentPos = ray.GetPoint (dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;

				if (offset.x != 0f || offset.y != 0f) {
					offset = transform.InverseTransformDirection (offset);
					offset.Scale (scale);
					offset = transform.TransformDirection (offset);
				}
				
				offset.z = 0;
				transform.position += offset;
				
				float length = transform.localPosition.magnitude;
				 
				if (length > radius) {
					transform.localPosition = Vector3.ClampMagnitude (transform.localPosition, radius);
				}

				position = new Vector2((transform.localPosition.x-center.x)/radius,(transform.localPosition.y-center.y)/radius);
			}
		}
	}
	
}
