using UnityEngine;
using System.Collections;

public class UIModelRotation : MonoBehaviour
{
	public float rotateFactor = 1;
	public UIScroller scroller;
	private Transform cachedTransfrom;
	private Vector2 oldScrollValue;
	
	public void ResetScroller()
	{
		scroller.ScrollPosition = new Vector2(0.5f, 0f);
	}
	
	// Use this for initialization
	void Start()
	{
		cachedTransfrom = transform;
		oldScrollValue = scroller.Value;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (scroller == null)
			return;

		// Scroll ball
		Vector2 scrollValue = scroller.Value;
		if (Mathf.Approximately(scrollValue.x, oldScrollValue.x) && Mathf.Approximately(scrollValue.y, oldScrollValue.y))
			return;

		cachedTransfrom.Rotate(Vector3.up, (scrollValue.x - oldScrollValue.x) * rotateFactor);
		oldScrollValue = scrollValue;
	}
}

