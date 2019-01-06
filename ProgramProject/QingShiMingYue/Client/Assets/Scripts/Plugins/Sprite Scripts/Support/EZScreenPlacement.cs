//-----------------------------------------------------------------
//  Copyright 2011 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <remarks>
/// A class that eases the process of placing objects
/// on-screen in a screen-relative, or object-relative
/// way, using pixels as units of distance.
/// </remarks>
[ExecuteInEditMode]
[System.Serializable]
[AddComponentMenu("EZ GUI/Utility/EZ Screen Placement")]
public class EZScreenPlacement : MonoBehaviour, IUseCamera
{
	/// <summary>
	/// Specifies what the object will be aligned relative to on the horizontal axis.
	/// </summary>
	public enum HORIZONTAL_ALIGN
	{
		/// <summary>
		/// The object will not be repositioned along the X axis.
		/// </summary>
		NONE = 0,

		/// <summary>
		/// The X coordinate of screenPos will be interpreted as the number of pixels from the left edge of the screen.
		/// </summary>
		SCREEN_LEFT = 1,

		/// <summary>
		/// The X coordinate of screenPos will be interpreted as the number of pixels from the right edge of the screen.
		/// </summary>
		SCREEN_RIGHT = 2,

		/// <summary>
		/// The X coordinate of screenPos will be interpreted as the number of pixels from the center of the screen.
		/// </summary>
		SCREEN_CENTER = 3,

		//SCREEN_RELATIVE = 4,

		/// <summary>
		/// The X coordinate of screenPos will be interpreted as the number of pixels from the object assigned to horizontalObj. i.e. negative values will place this object to the left of horizontalObj, and positive values to the right.
		/// </summary>
		OBJECT = 5,

		SPRITE_LEFT = 6,
		SPRITE_RIGHT = 7,
		SPRITE_CENTER = 8,
	}

	/// <summary>
	/// Specifies what the object will be aligned relative to on the vertical axis.
	/// </summary>
	public enum VERTICAL_ALIGN
	{
		/// <summary>
		/// The object will not be repositioned along the Y axis.
		/// </summary>
		NONE = 0,

		/// <summary>
		/// The Y coordinate of screenPos will be interpreted as the number of pixels from the top edge of the screen.
		/// </summary>
		SCREEN_TOP = 1,

		/// <summary>
		/// The Y coordinate of screenPos will be interpreted as the number of pixels from the bottom edge of the screen.
		/// </summary>
		SCREEN_BOTTOM = 2,

		/// <summary>
		/// The Y coordinate of screenPos will be interpreted as the number of pixels from the center of the screen.
		/// </summary>
		SCREEN_CENTER = 3,

		//SCREEN_RELATIVE = 4,

		/// <summary>
		/// The Y coordinate of screenPos will be interpreted as the number of pixels from the object assigned to verticalObj. i.e. negative values will place this object above verticalObj, and positive values below.
		/// </summary>
		OBJECT = 5,

		SPRITE_TOP = 6,
		SPRITE_BOTTOM = 7,
		SPRITE_CENTER = 8,
	}

	public enum HORIZONTAL_SCALE
	{
		NONE,
		SCREEN_SPRITE_SCALE,
		SCREEN_SPRITE_DOCK,
		SPRITE_SPRITE_SCALE,
		SPRITE_SPRITE_DOCK,
		SCREEN_OBJECT,
	}

	public enum VERTICAL_SCALE
	{
		NONE,
		SCREEN_SPRITE_SCALE,
		SCREEN_SPRITE_DOCK,
		SPRITE_SPRITE_SCALE,
		SPRITE_SPRITE_DOCK,
		SCREEN_OBJECT,
	}

	public enum H_V_SCALE_RATE
	{
		SEPARATE,
		SYNC_MIN,
		SYNC_MAX,
		SYNC_X,
		SYNC_Y
	}

	[System.Serializable]
	public class RelativeTo
	{
		public HORIZONTAL_ALIGN horizontal = HORIZONTAL_ALIGN.NONE;
		public bool calculateRelativeHScale = false;
		public VERTICAL_ALIGN vertical = VERTICAL_ALIGN.NONE;
		public bool calculateRelativeVScale = false;
		public HORIZONTAL_SCALE horizontalScale = HORIZONTAL_SCALE.NONE;
		public VERTICAL_SCALE verticalScale = VERTICAL_SCALE.NONE;
		public H_V_SCALE_RATE scaleRateHV = H_V_SCALE_RATE.SEPARATE;
		public bool clipSprite = false;

		// The script that contains this object
		protected EZScreenPlacement script;

		public EZScreenPlacement Script
		{
			get { return script; }
			set { Script = value; }
		}

		public bool Equals(RelativeTo rt)
		{
			if (rt == null)
				return false;
			return (horizontal == rt.horizontal && calculateRelativeHScale == rt.calculateRelativeHScale &&
				vertical == rt.vertical && calculateRelativeVScale == rt.calculateRelativeVScale &&
				horizontalScale == rt.horizontalScale && verticalScale == rt.verticalScale &&
				scaleRateHV == rt.scaleRateHV && clipSprite == rt.clipSprite);
		}

		public void Copy(RelativeTo rt)
		{
			if (rt == null)
				return;
			horizontal = rt.horizontal;
			calculateRelativeHScale = rt.calculateRelativeHScale;
			vertical = rt.vertical;
			calculateRelativeVScale = rt.calculateRelativeVScale;
			horizontalScale = rt.horizontalScale;
			verticalScale = rt.verticalScale;
			scaleRateHV = rt.scaleRateHV;
			clipSprite = rt.clipSprite;
		}

		// Copy constructor
		public RelativeTo(EZScreenPlacement sp, RelativeTo rt)
		{
			script = sp;
			Copy(rt);
		}

		public RelativeTo(EZScreenPlacement sp)
		{
			script = sp;
		}
	}

	/// <summary>
	/// The camera with which this object should be positioned.
	/// </summary>
	public Camera renderCamera;

	/// <summary>
	/// The position of the object, relative to the screen or other object.
	/// </summary>
	public Vector3 screenPos = Vector3.forward;

	/// <summary>
	/// When set to true, the Z component of Screen Pos will be calculated
	/// based upon the distance of the object to the render camera, rather
	/// than controlling the distance to the render camera.  Enable this
	/// option, for example, when you want to preserve parent-relative
	/// positioning on the Z-axis.
	/// </summary>
	public bool ignoreZ = true;

	public Vector2 relativeSize;
	public Vector2 orignalSpriteSize;

	/// <summary>
	/// Settings object that describes what this object is positioned
	/// relative to.
	/// </summary>
	public RelativeTo relativeTo;

	/// <summary>
	/// The object to which this object is relative.
	/// NOTE: Only used if either the vertical or horizontal elements 
	/// of relativeTo are set to OBJECT (or both).
	/// </summary>
	public Transform relativeObject;

	/// <summary>
	/// When true, positioning of the object is always done in a recursive
	/// fashion.  That is, if this object is relative to any other objects,
	/// those objects, should they also hold an EZScreenPlacement component,
	/// will be positioned before this one.
	/// </summary>
	public bool alwaysRecursive = true;

	/// <summary>
	/// When checked, you can simply use the transform handles in the scene view
	/// to roughly position your object in the scene, and the appropriate
	/// values will be automatically calculated for Screen Pos based on your
	/// other settings.
	/// If you're having problems with slight rounding errors making small
	/// changes to your Screen Pos coordinate, you should probably uncheck
	/// this option.  This is particularly likely to happen if your camera
	/// is rotated at all.
	/// </summary>
	public bool allowTransformDrag = false;

	protected Vector2 screenSize;

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9) && UNITY_EDITOR
	[System.NonSerialized]
	protected bool justEnabled = true;	// Helps us get around a silly issue that, sometimes when an object has OnEnabled() called, it may call SetCamera() on us in edit mode, and this happens in response to an OnGUI() call in-editor, meaning we'll get invalid camera information as a result, thereby positioning the object in the wrong place, so we need to detect this and just ignore any such SetCamera() call.
	[System.NonSerialized]
	protected EZScreenPlacementMirror mirror = new EZScreenPlacementMirror();
#else
	[System.NonSerialized]
	protected bool justEnabled = true;	// Helps us get around a silly issue that, sometimes when an object has OnEnabled() called, it may call SetCamera() on us in edit mode, and this happens in response to an OnGUI() call in-editor, meaning we'll get invalid camera information as a result, thereby positioning the object in the wrong place, so we need to detect this and just ignore any such SetCamera() call.
	[System.NonSerialized]
	protected EZScreenPlacementMirror mirror = new EZScreenPlacementMirror();
#endif

	protected bool m_awake = false;
	protected bool m_started = false;


	void Awake()
	{
		if (m_awake)
			return;
		m_awake = true;

		// IUseCamera may not started, Do not change camera if uc.RenderCamera is null
		IUseCamera uc = (IUseCamera)GetComponent("IUseCamera");
		if (uc != null && uc.RenderCamera != null)
			renderCamera = uc.RenderCamera;

		if (renderCamera == null)
			renderCamera = UIManager.instance.rayCamera;

		if (relativeTo == null)
			relativeTo = new RelativeTo(this);
		else if (relativeTo.Script != this)
		{
			// This appears to be a duplicate object,
			// so create our own copy of the relativeTo:
			RelativeTo newRT = new RelativeTo(this, relativeTo);
			relativeTo = newRT;
		}
	}

	public void Start()
	{
		if (m_started)
			return;
		m_started = true;

		if (renderCamera != null)
		{
			screenSize.x = renderCamera.pixelWidth;
			screenSize.y = renderCamera.pixelHeight;
		}

#if UNITY_EDITOR
		if (relativeObject != null)
		{
			EZScreenPlacement[] components = relativeObject.GetComponents<EZScreenPlacement>();
			foreach (var c in components)
				c.AddDependent(this);
		}

		justEnabled = false;
#endif

		PositionOnScreenRecursively();
	}

#if UNITY_EDITOR
	public void OnEnable()
	{
		justEnabled = true;
	}

	public void OnDestroy()
	{
		if (relativeObject != null)
		{
			EZScreenPlacement[] components = relativeObject.GetComponents<EZScreenPlacement>();
			foreach (var c in components)
				c.RemoveDependent(this);
		}
	}
#endif

	/// <summary>
	/// Positions the object, taking into account any object-relative
	/// dependencies, making sure the objects to which this object is
	/// relative are correctly positioned before positioning this one.
	/// </summary>
	public void PositionOnScreenRecursively()
	{
		if (!m_started)
			Start();

		if (relativeObject != null)
		{
			EZScreenPlacement[] components = relativeObject.GetComponents<EZScreenPlacement>();
			foreach (var sp in components)
				sp.PositionOnScreenRecursively();
		}

		PositionOnScreen();
	}

	/// <summary>
	/// Calculates a local position from a given screen or object-relative position,
	/// according to the current screen-space settings.
	/// </summary>
	/// <param name="screenPos">The screen/object-relative position</param>
	/// <returns>The corresponding position in local space.</returns>
	public Vector3 ScreenPosToLocalPos(Vector3 screenPos)
	{
		return CachedTransform.InverseTransformPoint(ScreenPosToWorldPos(screenPos));
	}

	/// <summary>
	/// Calculates a in the parent's local space from a given screen or 
	/// object-relative position, according to the current screen-space settings.
	/// </summary>
	/// <param name="screenPos">The screen/object-relative position</param>
	/// <returns>The corresponding position in the parent object's local space.</returns>
	public Vector3 ScreenPosToParentPos(Vector3 screenPos)
	{
		return ScreenPosToLocalPos(screenPos) + CachedTransform.localPosition;
	}

	/// <summary>
	/// Calculates the world position from a given screen or object-relative position,
	/// according to the current screen-space settings.
	/// </summary>
	/// <param name="screenPos">The screen/object-relative position</param>
	/// <returns>The corresponding position in world space.</returns>
	public Vector3 ScreenPosToWorldPos(Vector3 screenPos)
	{
		if (!m_started)
			Start();

		if (renderCamera == null)
		{
			Debug.LogError("Render camera not yet assigned to EZScreenPlacement component of \"" + name + "\" when attempting to call PositionOnScreen()");
			return CachedTransform.position;
		}

		Vector3 curPos = renderCamera.WorldToScreenPoint(CachedTransform.position);
		Vector3 pos = screenPos * UIManager.instance.UIScreenScale;
		//pos.Scale(CachedTransform.lossyScale);

		switch (relativeTo.horizontal)
		{
			case HORIZONTAL_ALIGN.SCREEN_LEFT:
				pos.x = pos.x * (relativeTo.calculateRelativeHScale ? screenSize.x / relativeSize.x : 1);
				break;
			case HORIZONTAL_ALIGN.SCREEN_RIGHT:
				pos.x = screenSize.x + pos.x * (relativeTo.calculateRelativeHScale ? screenSize.x / relativeSize.x : 1);
				break;
			case HORIZONTAL_ALIGN.SCREEN_CENTER:
				pos.x = screenSize.x * 0.5f + pos.x * (relativeTo.calculateRelativeHScale ? screenSize.x / relativeSize.x : 1);
				break;
			case HORIZONTAL_ALIGN.OBJECT:
				if (relativeObject != null)
					pos.x = renderCamera.WorldToScreenPoint(relativeObject.position).x + pos.x;
				else
					pos.x = curPos.x;
				break;
			case HORIZONTAL_ALIGN.NONE:
				pos.x = curPos.x;
				break;
			case HORIZONTAL_ALIGN.SPRITE_LEFT:
				{
					SpriteRoot spriteRoot = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (spriteRoot != null)
						pos.x = renderCamera.WorldToScreenPoint(spriteRoot.CachedTransform.localToWorldMatrix.MultiplyPoint3x4(spriteRoot.TopLeft)).x +
							pos.x * (relativeTo.calculateRelativeHScale ? spriteRoot.width / relativeSize.x : 1);
					else
						pos.x = curPos.x;
				}
				break;
			case HORIZONTAL_ALIGN.SPRITE_RIGHT:
				{
					SpriteRoot spriteRoot = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (spriteRoot != null)
						pos.x = renderCamera.WorldToScreenPoint(spriteRoot.CachedTransform.localToWorldMatrix.MultiplyPoint3x4(spriteRoot.BottomRight)).x +
							pos.x * (relativeTo.calculateRelativeHScale ? spriteRoot.width / relativeSize.x : 1);
					else
						pos.x = curPos.x;
				}
				break;
			case HORIZONTAL_ALIGN.SPRITE_CENTER:
				{
					SpriteRoot spriteRoot = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (spriteRoot != null)
						pos.x = renderCamera.WorldToScreenPoint(spriteRoot.CachedTransform.localToWorldMatrix.MultiplyPoint3x4(spriteRoot.Center)).x +
							pos.x * (relativeTo.calculateRelativeHScale ? spriteRoot.width / relativeSize.x : 1);
					else
						pos.x = curPos.x;
				}
				break;
		}

		switch (relativeTo.vertical)
		{
			case VERTICAL_ALIGN.SCREEN_TOP:
				pos.y = screenSize.y + pos.y * (relativeTo.calculateRelativeVScale ? screenSize.y / relativeSize.y : 1);
				break;
			case VERTICAL_ALIGN.SCREEN_BOTTOM:
				pos.y = pos.y * (relativeTo.calculateRelativeVScale ? screenSize.y / relativeSize.y : 1);
				break;
			case VERTICAL_ALIGN.SCREEN_CENTER:
				pos.y = screenSize.y * 0.5f + pos.y * (relativeTo.calculateRelativeVScale ? screenSize.y / relativeSize.y : 1);
				break;
			case VERTICAL_ALIGN.OBJECT:
				if (relativeObject != null)
					pos.y = renderCamera.WorldToScreenPoint(relativeObject.position).y + pos.y;
				else
					pos.y = curPos.y;
				break;
			case VERTICAL_ALIGN.NONE:
				pos.y = curPos.y;
				break;
			case VERTICAL_ALIGN.SPRITE_TOP:
				{
					SpriteRoot spriteRoot = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (spriteRoot != null)
						pos.y = renderCamera.WorldToScreenPoint(spriteRoot.CachedTransform.localToWorldMatrix.MultiplyPoint3x4(spriteRoot.TopLeft)).y +
							pos.y * (relativeTo.calculateRelativeVScale ? spriteRoot.height / relativeSize.y : 1);
					else
						pos.y = curPos.y;
				}
				break;
			case VERTICAL_ALIGN.SPRITE_BOTTOM:
				{
					SpriteRoot spriteRoot = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (spriteRoot != null)
						pos.y = renderCamera.WorldToScreenPoint(spriteRoot.CachedTransform.localToWorldMatrix.MultiplyPoint3x4(spriteRoot.BottomRight)).y +
							pos.y * (relativeTo.calculateRelativeVScale ? spriteRoot.height / relativeSize.y : 1);
					else
						pos.y = curPos.y;
				}
				break;
			case VERTICAL_ALIGN.SPRITE_CENTER:
				{
					SpriteRoot spriteRoot = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (spriteRoot != null)
						pos.y = renderCamera.WorldToScreenPoint(spriteRoot.CachedTransform.localToWorldMatrix.MultiplyPoint3x4(spriteRoot.Center)).y +
							pos.y * (relativeTo.calculateRelativeVScale ? spriteRoot.height / relativeSize.y : 1);
					else
						pos.y = curPos.y;
				}
				break;
		}

		return renderCamera.ScreenToWorldPoint(pos);
	}

	public Vector2 GetCameraScreenSize()
	{
		//Calculate the camera viewport width and height.
		return new Vector2(renderCamera.orthographicSize * 2 * renderCamera.aspect, renderCamera.orthographicSize * 2);
	}

	private void SetSpriteClipingRect(SpriteRoot spriteRoot, float clippingWidth, float clippingHeight)
	{
		if (clippingWidth >= spriteRoot.width && clippingHeight >= spriteRoot.height)
			return;

		clippingWidth = Mathf.Min(spriteRoot.width, clippingWidth);
		clippingHeight = Mathf.Min(spriteRoot.height, clippingHeight);

		Rect viewableRectActual = default(Rect);
		viewableRectActual.width = clippingWidth;
		viewableRectActual.height = clippingHeight;

		switch (spriteRoot.Anchor)
		{
			case SpriteRoot.ANCHOR_METHOD.UPPER_LEFT:
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT:
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT:
				viewableRectActual.x = 0;
				break;

			case SpriteRoot.ANCHOR_METHOD.UPPER_CENTER:
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER:
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_CENTER:
				viewableRectActual.x = -viewableRectActual.width * 0.5f;
				break;

			case SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT:
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT:
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_RIGHT:
				viewableRectActual.x = -viewableRectActual.width;
				break;
		}

		switch (spriteRoot.Anchor)
		{
			case SpriteRoot.ANCHOR_METHOD.UPPER_LEFT:
			case SpriteRoot.ANCHOR_METHOD.UPPER_CENTER:
			case SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT:
				viewableRectActual.y = -viewableRectActual.height;
				break;

			case SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT:
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER:
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT:
				viewableRectActual.y = -viewableRectActual.height * 0.5f;
				break;

			case SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT:
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_CENTER:
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_RIGHT:
				viewableRectActual.y = 0;
				break;
		}

		Rect3D clientClippingRect = default(Rect3D);
		clientClippingRect.FromRect(viewableRectActual);
		clientClippingRect.MultFast(spriteRoot.CachedTransform.localToWorldMatrix);

		spriteRoot.ClippingRect = clientClippingRect;
	}

	private void ScaleOnScreen()
	{
		if (!m_awake)
			return;

		if (!m_started)
			Start();

		if (renderCamera == null)
			return;

		bool scaleSpriteH = false;
		bool scaleSpriteV = false;
		Vector2 spriteScaleRate = Vector2.one;

		bool scaleObjectH = false;
		bool scaleObjectV = false;
		Vector3 objectScaleRate = Vector3.one;

		switch (relativeTo.horizontalScale)
		{
			case HORIZONTAL_SCALE.SCREEN_SPRITE_SCALE:
				scaleSpriteH = true;
				spriteScaleRate.x = GetCameraScreenSize().x / relativeSize.x /*/ CachedTransform.lossyScale.x*/;
				break;
			case HORIZONTAL_SCALE.SCREEN_SPRITE_DOCK:
				scaleSpriteH = true;
				spriteScaleRate.x = (GetCameraScreenSize().x - (relativeSize.x - orignalSpriteSize.x) /** CachedTransform.lossyScale.x*/) / orignalSpriteSize.x /*/ CachedTransform.lossyScale.x*/;
				break;
			case HORIZONTAL_SCALE.SPRITE_SPRITE_SCALE:
				{
					scaleSpriteH = true;
					SpriteRoot relativeSR = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (relativeSR != null)
						spriteScaleRate.x = relativeSR.width / relativeSize.x /*/ CachedTransform.lossyScale.x*/;
				}
				break;
			case HORIZONTAL_SCALE.SPRITE_SPRITE_DOCK:
				{
					scaleSpriteH = true;
					SpriteRoot relativeSR = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (relativeSR != null)
						spriteScaleRate.x = (relativeSR.width - relativeSize.x + orignalSpriteSize.x) / orignalSpriteSize.x;
				}
				break;
			case HORIZONTAL_SCALE.SCREEN_OBJECT:
				{
					scaleObjectH = true;
					objectScaleRate.x = GetCameraScreenSize().x / relativeSize.x;
				}
				break;
		}

		switch (relativeTo.verticalScale)
		{
			case VERTICAL_SCALE.SCREEN_SPRITE_SCALE:
				scaleSpriteV = true;
				spriteScaleRate.y = GetCameraScreenSize().y / relativeSize.y /*/ CachedTransform.lossyScale.y*/;
				break;
			case VERTICAL_SCALE.SCREEN_SPRITE_DOCK:
				scaleSpriteV = true;
				spriteScaleRate.y = (GetCameraScreenSize().y - (relativeSize.y - orignalSpriteSize.y) /** CachedTransform.lossyScale.y*/) / orignalSpriteSize.y /*/ CachedTransform.lossyScale.y*/;
				break;
			case VERTICAL_SCALE.SPRITE_SPRITE_SCALE:
				{
					scaleSpriteV = true;
					SpriteRoot relativeSR = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (relativeSR != null)
						spriteScaleRate.y = relativeSR.height / relativeSize.y /*/ CachedTransform.lossyScale.y*/;
				}
				break;
			case VERTICAL_SCALE.SPRITE_SPRITE_DOCK:
				{
					scaleSpriteV = true;
					SpriteRoot relativeSR = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
					if (relativeSR != null)
						spriteScaleRate.y = (relativeSR.height - relativeSize.y + orignalSpriteSize.y) / orignalSpriteSize.y;
				}
				break;
			case VERTICAL_SCALE.SCREEN_OBJECT:
				{
					scaleObjectV = true;
					objectScaleRate.y = GetCameraScreenSize().y / relativeSize.y;
				}
				break;
		}

		SpriteRoot spriteRoot = CachedTransform.GetComponent<SpriteRoot>();
		UIScrollList scrollList = CachedTransform.GetComponent<UIScrollList>();
		UIScroller scroller = CachedTransform.GetComponent<UIScroller>();

		if (scaleSpriteH && scaleSpriteV)
		{
			switch (relativeTo.scaleRateHV)
			{
				case H_V_SCALE_RATE.SEPARATE:
					break;
				case H_V_SCALE_RATE.SYNC_MIN:
					spriteScaleRate.x = spriteScaleRate.y = Mathf.Min(spriteScaleRate.x, spriteScaleRate.y);
					break;
				case H_V_SCALE_RATE.SYNC_MAX:
					spriteScaleRate.x = spriteScaleRate.y = Mathf.Max(spriteScaleRate.x, spriteScaleRate.y);
					break;
				case H_V_SCALE_RATE.SYNC_X:
					spriteScaleRate.y = spriteScaleRate.x;
					break;
				case H_V_SCALE_RATE.SYNC_Y:
					spriteScaleRate.x = spriteScaleRate.y;
					break;
			}

			if (spriteRoot != null)
			{
				float scaledWidth = orignalSpriteSize.x * spriteScaleRate.x;
				float scaledHeight = orignalSpriteSize.y * spriteScaleRate.y;
				if (relativeTo.clipSprite == false)
					spriteRoot.SetSize(scaledWidth, scaledHeight);
				else
					SetSpriteClipingRect(spriteRoot, scaledWidth, scaledHeight);
			}

			if (scrollList != null)
				scrollList.SetViewableArea(orignalSpriteSize.x * spriteScaleRate.x, orignalSpriteSize.y * spriteScaleRate.y);

			if (scroller != null)
				scroller.SetViewableArea(orignalSpriteSize.x * spriteScaleRate.x, orignalSpriteSize.y * spriteScaleRate.y);
		}
		else if (scaleSpriteH)
		{
			if (spriteRoot != null)
			{
				float scaledWidth = orignalSpriteSize.x * spriteScaleRate.x;
				float scaledHeight = spriteRoot.height;
				if (relativeTo.clipSprite == false)
					spriteRoot.SetSize(scaledWidth, scaledHeight);
				else
					SetSpriteClipingRect(spriteRoot, scaledWidth, scaledHeight);
			}

			if (scrollList != null)
				scrollList.SetViewableArea(orignalSpriteSize.x * spriteScaleRate.x, scrollList.viewableArea.y);

			if (scroller != null)
				scroller.SetViewableArea(orignalSpriteSize.x * spriteScaleRate.x, scrollList.viewableArea.y);
		}
		else if (scaleSpriteV)
		{
			if (spriteRoot != null)
			{
				float scaledWidth = spriteRoot.width;
				float scaledHeight = orignalSpriteSize.y * spriteScaleRate.y;
				if (relativeTo.clipSprite == false)
					spriteRoot.SetSize(scaledWidth, scaledHeight);
				else
					SetSpriteClipingRect(spriteRoot, scaledWidth, scaledHeight);
			}

			if (scrollList != null)
				scrollList.SetViewableArea(scrollList.viewableArea.x, orignalSpriteSize.y * spriteScaleRate.y);

			if (scroller != null)
				scroller.SetViewableArea(scroller.viewableArea.x, orignalSpriteSize.y * spriteScaleRate.y);
		}

		if (scaleObjectH && scaleObjectV)
		{
			switch (relativeTo.scaleRateHV)
			{
				case H_V_SCALE_RATE.SEPARATE:
					break;
				case H_V_SCALE_RATE.SYNC_MIN:
					objectScaleRate.x = objectScaleRate.y = Mathf.Min(objectScaleRate.x, objectScaleRate.y);
					break;
				case H_V_SCALE_RATE.SYNC_MAX:
					objectScaleRate.x = objectScaleRate.y = Mathf.Max(objectScaleRate.x, objectScaleRate.y);
					break;
				case H_V_SCALE_RATE.SYNC_X:
					objectScaleRate.y = objectScaleRate.x;
					break;
				case H_V_SCALE_RATE.SYNC_Y:
					objectScaleRate.x = objectScaleRate.y;
					break;
			}

			// Set scale, if ignoreZ keep z original scale
			//Vector3 parentScale = CachedTransform.parent != null ? CachedTransform.parent.lossyScale : Vector3.one;
			//objectScaleRate.Scale(new Vector3(1 / parentScale.x, 1 / parentScale.y, ignoreZ ? this.CachedTransform.localScale.z : 1 / parentScale.z));
			this.CachedTransform.localScale = objectScaleRate;
		}
	}

	/// <summary>
	/// Repositions the object using the existing screen-space settings.
	/// </summary>
	public void PositionOnScreen()
	{
		if (!m_awake)
			return;

		if (renderCamera == null)
		{
			if (Application.isPlaying)
				Debug.LogWarning("RenderCamera is null, can not place game object : " + gameObject.name);

			return;
		}

		// Keep the 'z' updated in the inspector
		if (ignoreZ)
		{
			Plane plane = new Plane(renderCamera.transform.forward, renderCamera.transform.position);
			screenPos.z = plane.GetDistanceToPoint(CachedTransform.position);
		}

		if (ignoreZ)
		{
			Vector3 pos = ScreenPosToWorldPos(screenPos);
			pos.z = CachedTransform.position.z;
			CachedTransform.position = pos;
		}
		else
			CachedTransform.position = ScreenPosToWorldPos(screenPos);

		ScaleOnScreen();

#if UNITY_EDITOR
		if (!Application.isPlaying)
			UpdateDependents();
#endif

		// Notify the object that it was repositioned:
		SendMessage("OnReposition", SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	/// Positions the object using screen coordinates, according to
	/// the relativity settings stored in relativeToScreen.
	/// </summary>
	/// <param name="x">The number of pixels in the X axis relative to the position specified in relativeToScreen.</param>
	/// <param name="y">The number of pixels in the Y axis relative to the position specified in relativeToScreen.</param>
	/// <param name="depth">The distance the object should be in front of the camera.</param>
	public void PositionOnScreen(int x, int y, float depth)
	{
		PositionOnScreen(new Vector3((float)x, (float)y, depth));
	}

	/// <summary>
	/// Positions the object using screen coordinates, according to
	/// the relativity settings stored in relativeToScreen.
	/// </summary>
	/// <param name="pos">The X and Y screen coordinates where the object should be positioned, as well as the Z coordinate which represents the distance in front of the camera.</param>
	public void PositionOnScreen(Vector3 pos)
	{
		screenPos = pos;
		PositionOnScreen();
	}


	/// <summary>
	/// Accessor for the camera that will be used to render this object.
	/// Use this to ensure the object is properly configured for the
	/// specific camera that will render it.
	/// </summary>
	public Camera RenderCamera
	{
		get { return renderCamera; }
		set { SetCamera(value); }
	}

	/// <summary>
	/// Updates the object's position based on the currently
	/// selected renderCamera.
	/// </summary>
	public void UpdateCamera()
	{
		SetCamera(renderCamera);
	}

	/// <summary>
	/// A no-argument version of SetCamera() that simply
	/// re-assigns the same camera to the object, forcing
	/// it to recalculate all camera-dependent calculations.
	/// </summary>
	public void SetCamera()
	{
		SetCamera(renderCamera);
	}


	/// <summary>
	/// Sets the camera to be used for calculating positions.
	/// </summary>
	/// <param name="c"></param>
	public void SetCamera(Camera c)
	{
		if (c == null || !enabled)
			return;
#if UNITY_EDITOR
		if (!Application.isPlaying && justEnabled)
			return;
#endif

		renderCamera = c;
		screenSize.x = renderCamera.pixelWidth;
		screenSize.y = renderCamera.pixelHeight;

		if (alwaysRecursive || (Application.isEditor && !Application.isPlaying))
			PositionOnScreenRecursively();
		else
			PositionOnScreen();
	}


	public void WorldToScreenPos(Vector3 worldPos)
	{
		if (renderCamera == null)
			return;

		Vector3 newPos = renderCamera.WorldToScreenPoint(worldPos);

		switch (relativeTo.horizontal)
		{
			case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_CENTER:
				screenPos.x = newPos.x - (renderCamera.pixelWidth / 2f);
				break;
			case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_LEFT:
				screenPos.x = newPos.x;
				break;
			case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_RIGHT:
				screenPos.x = newPos.x - renderCamera.pixelWidth;
				break;
			case EZScreenPlacement.HORIZONTAL_ALIGN.OBJECT:
				if (relativeObject != null)
				{
					Vector3 objPos = renderCamera.WorldToScreenPoint(relativeObject.position);
					screenPos.x = newPos.x - objPos.x;
				}
				break;
		}

		switch (relativeTo.vertical)
		{
			case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_CENTER:
				screenPos.y = newPos.y - (renderCamera.pixelHeight / 2f);
				break;
			case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_TOP:
				screenPos.y = newPos.y - renderCamera.pixelHeight;
				break;
			case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_BOTTOM:
				screenPos.y = newPos.y;
				break;
			case EZScreenPlacement.VERTICAL_ALIGN.OBJECT:
				if (relativeObject != null)
				{
					Vector3 objPos = renderCamera.WorldToScreenPoint(relativeObject.position);
					screenPos.y = newPos.y - objPos.y;
				}
				break;
		}

		screenPos.z = newPos.z;

		if (alwaysRecursive)
			PositionOnScreenRecursively();
		else
			PositionOnScreen();
	}

	/// <summary>
	/// Retrieves the screen coordinate of the object's current position.
	/// </summary>
	public Vector3 ScreenCoord
	{
		get { return renderCamera.WorldToScreenPoint(CachedTransform.position); }
	}


	// Tests dependencies for circular dependency.
	// Returns true if safe, false if circular.
	static public bool TestDepenency(EZScreenPlacement sp)
	{
		if (sp.relativeObject == null)
			return true;

		// Table of all objects in the chain of dependency:
		List<EZScreenPlacement> objs = new List<EZScreenPlacement>();
		return DoTestDepenency(sp, objs);
	}

	static public bool DoTestDepenency(EZScreenPlacement sp, List<EZScreenPlacement> objs)
	{
		if (objs.Contains(sp))
			return false; // Circular!

		// Add this one to the list and keep walkin'
		objs.Add(sp);

		// See if we're at the end of the chain:
		if (sp.relativeObject != null)
		{
			EZScreenPlacement[] components = sp.relativeObject.GetComponents<EZScreenPlacement>();

			foreach (var c in components)
				if (DoTestDepenency(c, objs) == false)
					return false;
		}

		// No circular in this brunch, remove form obj list
		objs.Remove(sp);

		return true;
	}

	// List of dependent objects.
	[HideInInspector]
	public EZScreenPlacement[] dependents = new EZScreenPlacement[0];

#if UNITY_EDITOR
	// Notify this object that it has a dependent object.
	public void AddDependent(EZScreenPlacement sp)
	{
		// Ensure the object isn't already on our list:
		foreach (EZScreenPlacement d in dependents)
			if (d == sp)
				return;

		List<EZScreenPlacement> temp = new List<EZScreenPlacement>();
		temp.AddRange(dependents);

		if (!temp.Contains(sp))
		{
			temp.Add(sp);
			dependents = temp.ToArray();
		}
	}

	// Notify the object that it has one fewer dependent.
	public void RemoveDependent(EZScreenPlacement sp)
	{
		List<EZScreenPlacement> temp = new List<EZScreenPlacement>();
		temp.AddRange(dependents);
		temp.Remove(sp);
		dependents = temp.ToArray();
	}

	// Cleans dependents from the list which are no longer valid:
	public void CleanDependents()
	{
		if (dependents == null)
			dependents = new EZScreenPlacement[0];

		List<EZScreenPlacement> temp = new List<EZScreenPlacement>();

		foreach (EZScreenPlacement sp in dependents)
		{
			if (sp != null)
			{
				temp.Add(sp);
			}
		}

		dependents = temp.ToArray();

		if (dependents == null)
			dependents = new EZScreenPlacement[0];
	}

	// Updates the positions of all dependent objects
	public void UpdateDependents()
	{
		CleanDependents();

		foreach (EZScreenPlacement sp in dependents)
			if (sp != null)
				sp.PositionOnScreen();
	}
#endif


	public virtual void DoMirror()
	{
		// Only run if we're not playing:
		if (Application.isPlaying)
			return;

		if (renderCamera != null)
			UIManager.instance.UIScreenScale = renderCamera.pixelHeight / (renderCamera.orthographicSize * 2);

		if (mirror == null)
		{
			mirror = new EZScreenPlacementMirror();
			mirror.Mirror(this);
		}

		mirror.Validate(this);

		// Compare our mirrored settings to the current settings
		// to see if something was changed:
		if (mirror.DidChange(this))
		{
			SetCamera(renderCamera);
			mirror.Mirror(this);	// Update the mirror
		}
	}

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9) && UNITY_EDITOR
	void Update()
	{
		DoMirror();
	}
#else
	// Ensures that the text is updated in the scene view
	// while editing:
	public virtual void OnDrawGizmosSelected()
	{
		DoMirror();
	}

	// Ensures that the text is updated in the scene view
	// while editing:
	public virtual void OnDrawGizmos()
	{
		DoMirror();
	}
#endif

}



// Used to automatically update an EZScreenPlacement object
// when its settings are modified in-editor.
public class EZScreenPlacementMirror
{
	public Vector3 worldPos;
	public Vector3 screenPos;
	public Vector2 relativeSize;
	public EZScreenPlacement.RelativeTo relativeTo;
	public Transform relativeObject;
	public Camera renderCamera;
	public Vector2 screenSize;

	public EZScreenPlacementMirror()
	{
		relativeTo = new EZScreenPlacement.RelativeTo(null);
	}

	public virtual void Mirror(EZScreenPlacement sp)
	{
		worldPos = sp.CachedTransform.position;
		screenPos = sp.screenPos;
		relativeTo.Copy(sp.relativeTo);
		relativeObject = sp.relativeObject;
		renderCamera = sp.renderCamera;
		if (sp.renderCamera != null)
			screenSize = new Vector2(sp.renderCamera.pixelWidth, sp.renderCamera.pixelHeight);
	}

	public virtual bool Validate(EZScreenPlacement sp)
	{
		// Only allow assignment of a relative object IF
		// we intend to use it:
		//		if (sp.relativeTo.horizontal != EZScreenPlacement.HORIZONTAL_ALIGN.OBJECT &&
		//			sp.relativeTo.vertical != EZScreenPlacement.VERTICAL_ALIGN.OBJECT &&
		//			sp.relativeTo.horizontal != EZScreenPlacement.HORIZONTAL_ALIGN.SPRITE_LEFT &&
		//			sp.relativeTo.horizontal != EZScreenPlacement.HORIZONTAL_ALIGN.SPRITE_RIGHT &&
		//			sp.relativeTo.horizontal != EZScreenPlacement.HORIZONTAL_ALIGN.SPRITE_CENTER &&
		//			sp.relativeTo.vertical != EZScreenPlacement.VERTICAL_ALIGN.SPRITE_TOP &&
		//			sp.relativeTo.vertical != EZScreenPlacement.VERTICAL_ALIGN.SPRITE_BOTTOM &&
		//			sp.relativeTo.vertical != EZScreenPlacement.VERTICAL_ALIGN.SPRITE_CENTER )
		//			sp.relativeObject = null;

		// See if our dependency is circular:
		if (sp.relativeObject != null)
		{
			if (!EZScreenPlacement.TestDepenency(sp))
			{
				Debug.LogError("ERROR: The Relative Object you recently assigned on \"" + sp.name + "\" which points to \"" + sp.relativeObject.name + "\" would create a circular dependency.  Please check your placement dependencies to resolve this.");
				sp.relativeObject = null;
			}
		}

		return true;
	}

	public virtual bool DidChange(EZScreenPlacement sp)
	{
		if (worldPos != sp.CachedTransform.position)
		{
			if (sp.allowTransformDrag)
			{
				// Calculate new screen position:
				sp.WorldToScreenPos(sp.CachedTransform.position);
			}
			else
				sp.PositionOnScreen();
			return true;
		}

		if (sp.allowTransformDrag)
		{
			SpriteRoot sr = relativeObject != null ? relativeObject.GetComponent<SpriteRoot>() : null;
			Vector2 relativeSize = Vector2.zero;
			if (sr != null)
				relativeSize = new Vector2(sr.width, sr.height);
			else if (sp.renderCamera != null)
				relativeSize = sp.GetCameraScreenSize();
			if (relativeSize.x != sp.relativeSize.x || relativeSize.y != sp.relativeSize.y)
			{
				sp.relativeSize = relativeSize;
				sp.PositionOnScreen();

				return true;
			}

			sr = sp.GetComponent<SpriteRoot>();
			UIScrollList sl = sp.GetComponent<UIScrollList>();
			if (sr != null)
				sp.orignalSpriteSize = new Vector2(sr.width, sr.height);
			else if (sl != null)
				sp.orignalSpriteSize = sl.viewableArea;
			else
				sp.orignalSpriteSize = Vector2.zero;
		}

		if (screenPos != sp.screenPos)
			return true;
		if (!relativeTo.Equals(sp.relativeTo))
			return true;
		if (renderCamera != sp.renderCamera)
			return true;
		if (renderCamera != null && (screenSize.x != sp.renderCamera.pixelWidth || screenSize.y != sp.renderCamera.pixelHeight))
			return true;
		if (relativeObject != sp.relativeObject)
		{
#if UNITY_EDITOR
			// Remove ourselves as a dependent on the previous object:
			if (relativeObject != null)
			{
				EZScreenPlacement c = relativeObject.GetComponent<EZScreenPlacement>();
				if (c != null)
					c.RemoveDependent(sp);
			}

			// Add ourselves as a dependent to the new object:
			if (sp.relativeObject != null)
			{
				EZScreenPlacement c = sp.relativeObject.GetComponent<EZScreenPlacement>();
				if (c != null)
					c.AddDependent(sp);
			}
#endif
			return true;
		}

		return false;
	}
}