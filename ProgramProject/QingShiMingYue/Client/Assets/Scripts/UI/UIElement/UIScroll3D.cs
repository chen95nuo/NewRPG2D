using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScroll3D : MonoBehaviour
{
	public UIScroller scroller;
	public UIListItemContainer scrollItemContainer;
	public float scrollFactor = 1.0f;
	public Vector2 cheerAvatarFixValue;

	public List<UIButton> actionBtns;

	[HideInInspector]
	public Rect3D clippingRect;

	private Vector2 oldScrollValue = Vector2.zero;
	private bool isCheerAvatarOpened = false;

	public void Init(bool isCheerAvatarOpened)
	{
		clippingRect = GetRect3DForClipItem();
		scrollItemContainer.SetInputDelegate(EZInputDel);

		foreach (var actionBtn in actionBtns)
		{
			actionBtn.SetInputDelegate(EZInputDel);
		}

		this.isCheerAvatarOpened = isCheerAvatarOpened;

		if (isCheerAvatarOpened)
			SetInitPosition((scroller.MaxValue - scroller.Value + scroller.MinValue) / scrollFactor);
		else
			SetInitPosition(scroller.MaxValue * 2 / scrollFactor);
	}

	private void SetInitPosition(Vector2 initPos)
	{
		if (float.IsNaN(initPos.x))
			return;

		Vector3 itemPos = scrollItemContainer.gameObject.transform.localPosition;

		scrollItemContainer.gameObject.transform.localPosition = new Vector3(initPos.x, initPos.y, itemPos.z);

	}

	private Rect3D GetRect3DForClipItem()
	{
		Vector2 scrollerViewArea = scroller.viewableArea;
		Vector2 clipRectTopLeft = Vector2.zero;

		switch (scroller.viewableAreaAhchor)
		{
			case UIScroller.ANCHOR_METHOD.UPPER_LEFT:
				clipRectTopLeft.x = 0f;
				clipRectTopLeft.y = -scrollerViewArea.y;
				break;

			case UIScroller.ANCHOR_METHOD.UPPER_CENTER:
				clipRectTopLeft.x = -scrollerViewArea.x / 2f;
				clipRectTopLeft.y = -scrollerViewArea.y;
				break;

			case UIScroller.ANCHOR_METHOD.UPPER_RIGHT:
				clipRectTopLeft.x = -scrollerViewArea.x;
				clipRectTopLeft.y = -scrollerViewArea.y;
				break;

			case UIScroller.ANCHOR_METHOD.MIDDLE_LEFT:
				clipRectTopLeft.x = 0f;
				clipRectTopLeft.y = -scrollerViewArea.y / 2f;
				break;

			case UIScroller.ANCHOR_METHOD.MIDDLE_CENTER:
				clipRectTopLeft.x = -scrollerViewArea.x / 2f;
				clipRectTopLeft.y = -scrollerViewArea.y / 2f;
				break;

			case UIScroller.ANCHOR_METHOD.MIDDLE_RIGHT:
				clipRectTopLeft.x = -scrollerViewArea.x;
				clipRectTopLeft.y = -scrollerViewArea.y / 2f;
				break;

			case UIScroller.ANCHOR_METHOD.BOTTOM_LEFT:
				clipRectTopLeft.x = 0f;
				clipRectTopLeft.y = 0;
				break;

			case UIScroller.ANCHOR_METHOD.BOTTOM_CENTER:
				clipRectTopLeft.x = -scrollerViewArea.x / 2f;
				clipRectTopLeft.y = 0;
				break;

			case UIScroller.ANCHOR_METHOD.BOTTOM_RIGHT:
				clipRectTopLeft.x = -scrollerViewArea.x;
				clipRectTopLeft.y = 0;
				break;
		}

		//Get view area rect
		Rect viewAreaRect = new Rect(clipRectTopLeft.x, clipRectTopLeft.y, scrollerViewArea.x, scrollerViewArea.y);

		//Get view rect 3D for calculate ListItem clip rect.
		Rect3D viewRect3D = new Rect3D(viewAreaRect);
		viewRect3D.MultFast(scroller.transform.localToWorldMatrix);

		return viewRect3D;
	}

	private void CalculateClippingRect()
	{
		scrollItemContainer.ClippingRect = clippingRect;
		scrollItemContainer.ScanChildren();
		scrollItemContainer.UpdateCamera();
	}

	private void EZInputDel(ref POINTER_INFO ptr)
	{
		scroller.OnInput(ptr);
	}

	private void Update()
	{
		if (scroller == null || !isCheerAvatarOpened)
			return;

		Vector2 scrollValue = scroller.Value;

		SetInitPosition((scroller.MaxValue - scroller.Value) / scrollFactor);

		clippingRect = GetRect3DForClipItem();

		if (oldScrollValue != scrollValue)
			CalculateClippingRect();

		oldScrollValue = scrollValue;


	}
}
