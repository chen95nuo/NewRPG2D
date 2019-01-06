#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class BallTest : MonoBehaviour 
{
	public float rotateFactor = 1;
	public UIScroller scroller;
	private Transform cachedTransfrom;
	private Vector2 oldScrollValue;

	// Use this for initialization
	void Start () 
	{
		cachedTransfrom = transform;		
	}
	
	// Update is called once per frame
	void LateUpdate() 
	{
		if(scroller == null)
			return;
		
		// Scroll ball
		Vector2 scrollValue = scroller.Value;
		if (Mathf.Approximately(scrollValue.x, oldScrollValue.x) && Mathf.Approximately(scrollValue.y, oldScrollValue.y))
			return;

		cachedTransfrom.Rotate(Vector3.right, (scrollValue.y - oldScrollValue.y) * rotateFactor);
		cachedTransfrom.Rotate(Vector3.forward, (scrollValue.x - oldScrollValue.x) * rotateFactor);

		Quaternion rotation = cachedTransfrom.rotation;
		rotation.y = 0;
		cachedTransfrom.rotation = rotation;

		oldScrollValue = scrollValue;
	}	
}
#endif