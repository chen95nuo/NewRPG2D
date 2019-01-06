using UnityEngine;
using System;
using System.Collections.Generic;

public class UIChildLayoutControl : MonoBehaviour
{
	public enum ORIENTATION
	{
		HORIZONTAL_LEFT,
		HORIZONTAL_CENTER,
		HORIZONTAL_RIGHT,
		VERTICAL_TOP,
		VERTICAL_CENTER,
		VERTICAL_BOTTOM,
	};

	[System.Serializable]
	public class LayoutChildControl
	{
		public enum ANCHOR
		{
			LEFT_TOP,
			CENTER,
			BOTTOM_RIGHT,
		};

		public void Initialize(GameObject gameObject, bool useSpriteSize, float widthHeight, float topLeftOffset, float bottomRightOffset)
		{
			this.gameObject = gameObject;
			this.useSpriteSize = useSpriteSize;
			this.widthHeight = widthHeight;
			this.topLeftOffset = topLeftOffset;
			this.bottomRightOffset = bottomRightOffset;
		}

		public void Hide(bool hide)
		{
			gameObject.SetActive(!hide);
		}

		public GameObject gameObject;
		public ANCHOR anchor = ANCHOR.CENTER;
		public bool useSpriteSize;
		public float widthHeight;
		public float topLeftOffset;
		public float bottomRightOffset;

		public float GetWidthHeight(bool horizontal)
		{
			if (!useSpriteSize)
				return widthHeight;

			SpriteRoot sr = gameObject.GetComponent<SpriteRoot>();
			if (sr != null)
				return horizontal ? sr.width : sr.height;
			else
				return widthHeight;
		}

		public bool hide;
	}

	public ORIENTATION orientation = ORIENTATION.VERTICAL_CENTER;
	public LayoutChildControl[] childLayoutControls = new LayoutChildControl[0];
	public bool resizeSprite = true;

	[ContextMenu("Layout")]
	public void Layout()
	{
		foreach (var obj in childLayoutControls)
		{
			HideChildObj(obj.gameObject, obj.hide);
		}
	}

	public bool HideChildObj(GameObject gameObject, bool hide)
	{
		LayoutChildControl item = GetLayoutChildControl(gameObject);
		item.hide = hide;

		switch (orientation)
		{
			case ORIENTATION.HORIZONTAL_LEFT:
			case ORIENTATION.HORIZONTAL_CENTER:
			case ORIENTATION.HORIZONTAL_RIGHT:
				LayoutHorizontal(orientation);
				break;
			case ORIENTATION.VERTICAL_TOP:
			case ORIENTATION.VERTICAL_CENTER:
			case ORIENTATION.VERTICAL_BOTTOM:
				LayoutVertical(orientation);
				break;
		}

		return true;
	}

	public bool HideChildObj(GameObject gameObject, bool hide, bool hideAndChangeSize)
	{
		if (hideAndChangeSize)
		{
			HideChildObj(gameObject, hideAndChangeSize);
		}
		else
		{
			LayoutChildControl item = GetLayoutChildControl(gameObject);
			item.hide = hide;
			item.gameObject.SetActive(!item.hide);
		}

		return true;
	}

	public void HideAllChildObjWithoutChangingSize(float width, float height)
	{
		foreach (var item in childLayoutControls)
		{
			item.gameObject.SetActive(false);
		}

		// Resize root sprite
		SpriteRoot border = GetComponent<SpriteRoot>();
		if (border != null)
			border.SetSize(width, height);
	}


	public LayoutChildControl GetLayoutChildControl(GameObject go)
	{
		foreach (var item in childLayoutControls)
			if (item.gameObject == go)
				return item;

		return null;
	}

	private void LayoutHorizontal(ORIENTATION orientation)
	{
		// Get total width
		float totalWidth = 0;
		foreach (var item in childLayoutControls)
		{
			item.gameObject.SetActive(!item.hide);

			if (item.hide == false)
				totalWidth += item.topLeftOffset + item.GetWidthHeight(true) + item.bottomRightOffset;
		}

		// Resize root sprite
		if (resizeSprite)
		{
			SpriteRoot border = GetComponent<SpriteRoot>();
			if (border != null)
				border.SetSize(totalWidth, border.height);
		}

		// Get child start position
		float startX = 0;
		switch (orientation)
		{
			case ORIENTATION.HORIZONTAL_LEFT:
				startX = 0;
				break;
			case ORIENTATION.HORIZONTAL_CENTER:
				startX = -totalWidth / 2;
				break;
			case ORIENTATION.HORIZONTAL_RIGHT:
				startX = -totalWidth;
				break;
		}

		// Layout children
		foreach (var item in childLayoutControls)
		{
			item.gameObject.SetActive(!item.hide);

			if (item.hide == false)
			{
				Vector3 position = item.gameObject.transform.localPosition;
				position.x = startX + item.topLeftOffset + item.GetWidthHeight(true) / 2;
				item.gameObject.transform.localPosition = position;

				startX += item.topLeftOffset + item.GetWidthHeight(true) + item.bottomRightOffset;
			}
		}
	}

	private void LayoutVertical(ORIENTATION orientation)
	{
		float totalHeight = 0;
		foreach (var item in childLayoutControls)
		{
			item.gameObject.SetActive(!item.hide);

			if (item.hide == false)
				totalHeight += item.topLeftOffset + item.GetWidthHeight(false) + item.bottomRightOffset;
		}

		AutoSpriteControlBase border = GetComponent<AutoSpriteControlBase>();
		if (border != null)
			border.SetSize(border.width, totalHeight);

		float startY = 0;
		switch (orientation)
		{
			case ORIENTATION.VERTICAL_TOP:
				startY = 0;
				break;
			case ORIENTATION.VERTICAL_CENTER:
				startY = totalHeight / 2;
				break;
			case ORIENTATION.VERTICAL_BOTTOM:
				startY = totalHeight;
				break;
		}

		foreach (var item in childLayoutControls)
		{
			if (item.hide == false)
			{
				Vector3 position = item.gameObject.transform.localPosition;
				switch (item.anchor)
				{
					case LayoutChildControl.ANCHOR.LEFT_TOP:
						position.y = startY - item.topLeftOffset;
						break;
					case LayoutChildControl.ANCHOR.CENTER:
						position.y = startY - (item.topLeftOffset + item.GetWidthHeight(false) / 2);
						break;
					case LayoutChildControl.ANCHOR.BOTTOM_RIGHT:
						position.y = startY - (item.topLeftOffset + item.GetWidthHeight(false));
						break;
				}

				item.gameObject.transform.localPosition = position;

				startY -= item.topLeftOffset + item.GetWidthHeight(false) + item.bottomRightOffset;
			}
		}
	}
}