using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPVEScrollMove : MonoBehaviour 
{
	public Transform targetTrans;
	public UIListItemContainer clipItem;
	public UIScroller moveScroller;
	public UIBox chessBg;

	private const float TARGET_SCALE_FACTOR = 1.4f;
	private const float MOVE_FACTOR = 1f;
	private const float Y_DELTA = 10f;
	
	public GameObject effectObjsRoot;
	
	private Vector2 oldScrollValue = Vector2.zero;
	private Rect3D viewRect3D;
	
	void Start()
	{
		EZScreenPlacement moveEz = moveScroller.gameObject.GetComponent<EZScreenPlacement>();
		moveEz.Start();

		float hOffset = (moveEz.orignalSpriteSize.y - moveScroller.viewableArea.y) / 2f;
		float extraScaleWidth = chessBg.width / TARGET_SCALE_FACTOR * (TARGET_SCALE_FACTOR - 1f) / 2f / MOVE_FACTOR;
		float extraScaleHeight = (chessBg.height - moveScroller.viewableArea.y) / 2f;

		moveScroller.MaxValue = new Vector2(extraScaleWidth, extraScaleHeight + hOffset);
		moveScroller.MinValue = new Vector2(-extraScaleWidth, -extraScaleHeight + hOffset);
		moveScroller.ScrollToValue(new Vector2(0, Y_DELTA), 0f, EZAnimation.EASING_TYPE.Linear, Vector2.zero);

		Vector3 originalScale = effectObjsRoot.transform.localScale;
		effectObjsRoot.transform.localScale = new Vector3(TARGET_SCALE_FACTOR, TARGET_SCALE_FACTOR, originalScale.z);
		
		clipItem.SetInputDelegate(EZInputDel);
	}
	
	private void EZInputDel(ref POINTER_INFO ptr)
	{
		moveScroller.OnInput(ptr);
	}
	
	
	void LateUpdate() 
	{
		if (moveScroller == null)
			return;

		Vector2 scrollValue = moveScroller.Value;
	
		targetTrans.Translate(MOVE_FACTOR*(oldScrollValue.x - scrollValue.x), MOVE_FACTOR*(scrollValue.y - oldScrollValue.y), 0f);
		CalculateClippingRect();
		oldScrollValue = scrollValue;
	}
	
	private void CalculateClippingRect()
	{
		clipItem.ClippingRect = viewRect3D;
		clipItem.ScanChildren();
		clipItem.UpdateCamera();
	}
	
	private void Update()
	{
		EZScreenPlacement moveEz = moveScroller.gameObject.GetComponent<EZScreenPlacement>();
		moveEz.Start();

		float hOffset = (moveEz.orignalSpriteSize.y - moveScroller.viewableArea.y) / 2f;
		float extraScaleWidth = chessBg.width / TARGET_SCALE_FACTOR * (TARGET_SCALE_FACTOR - 1f) / 2f / MOVE_FACTOR;
		float extraScaleHeight = (chessBg.height - moveScroller.viewableArea.y) / 2f;

		moveScroller.MaxValue = new Vector2(extraScaleWidth, extraScaleHeight + hOffset);
		moveScroller.MinValue = new Vector2(-extraScaleWidth, -extraScaleHeight + hOffset);
		
		Rect viewAreaRect = new Rect(-moveScroller.viewableArea.x/2f, -moveScroller.viewableArea.y, moveScroller.viewableArea.x, moveScroller.viewableArea.y);
		viewRect3D = new Rect3D(viewAreaRect);
		viewRect3D.MultFast(moveScroller.transform.localToWorldMatrix);
		
	}
}
