using UnityEngine;
using System.Collections.Generic;

public class UIScroller : MonoBehaviour, IUIObject
{
	public enum ANCHOR_METHOD
	{
		UPPER_LEFT,
		UPPER_CENTER,
		UPPER_RIGHT,
		MIDDLE_LEFT,
		MIDDLE_CENTER,
		MIDDLE_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_CENTER,
		BOTTOM_RIGHT,
	}

	/// <summary>
	/// Delegate definition for a delegate that receives a notification
	/// when the list begins to snap to an item.
	/// </summary>
	/// <param name="item">The item to which the list is snapping</param>
	public delegate void ValueSnappedDelegate(Vector2 value);

	[SerializeField]
	private Vector2 minValue;
	public Vector2 MinValue
	{
		get { return minValue; }
		set
		{
			minValue = value;
			UpdateContentExtents();
		}
	}

	[SerializeField]
	private Vector2 maxValue;
	public Vector2 MaxValue
	{
		get { return maxValue; }
		set
		{
			maxValue = value;
			UpdateContentExtents();
		}
	}

	[SerializeField]
	public Vector2 defaultValue = Vector2.zero;

	public Vector2 Value
	{
		get
		{
			return new Vector2(UIUtility.LerpWithoutClamp(MinValue.x, MaxValue.x, scrollPos.x),
				UIUtility.LerpWithoutClamp(MinValue.y, MaxValue.y, scrollPos.y));
		}
	}

	[SerializeField]
	private Vector2 valueScrollFactor = Vector2.one;
	public Vector2 ValueScrollFactor
	{
		set { valueScrollFactor = value; }
	}

	/// <summary>
	/// When true, scrolling will operate like a
	/// scrollable list on an iPhone - when a
	/// pointing device (normally a finger) is
	/// dragged across it, it scrolls and coasts
	/// to an eventual stop if not stopped 
	/// manually.
	/// </summary>
	[SerializeField]
	private bool touchScroll = true;
	public bool TouchScroll
	{
		get { return touchScroll; }
		set { touchScroll = value; }
	}

	/// <summary>
	/// The scroll deceleration coefficient. This value
	/// determines how quickly a touch scroll will coast
	/// to a stop.  When using snapping, this also essentially
	/// determines which item will be snapped to when the list
	/// is let go.  If you want to always scroll one item at a
	/// time, for example, set this value high.
	/// </summary>
	[SerializeField]
	private float scrollDecelCoef = 0.04f;

	/// <summary>
	/// The type of easing to use when snapping to an item.
	/// </summary>
	public EZAnimation.EASING_TYPE snapEasing = EZAnimation.EASING_TYPE.ExponentialOut;

	/// <summary>
	/// The alignment of view area
	/// </summary>
	public ANCHOR_METHOD viewableAreaAhchor = ANCHOR_METHOD.MIDDLE_CENTER;

	/// <summary>
	/// The extents of the viewable area, in local
	/// space units.  The contents of the list will
	/// be clipped to this area. Ex: If an area of
	/// 10x10 is specified, the list contents will
	/// be clipped 5 units above the center of the
	/// scroll list's GameObject, 5 units below,
	/// and 5 units on either side.
	/// </summary>
	public Vector2 viewableArea;

	// The one that is actually used for calculations
	// (always in world/local units)
	protected Vector2 viewableAreaActual;
	protected Rect viewableRectActual;

	/// <summary>
	/// When true, the viewableArea itemSpacing, etc, are interpreted as
	/// being specified in screen pixels.
	/// </summary>
	public bool unitsInPixels = false;

	/// <summary>
	/// The camera that will render this list.
	/// This needs to be set when unitsInPixels
	/// is set to true or when you have prefab
	/// items that need to know which camera to
	/// use to size themselves.
	/// </summary>
	public Camera renderCamera;

	public float dragThreshold = float.NaN;

	/// <summary>
	/// The percentage of the viewable area that can be "over-scrolled" (scrolled beyond the end/beginning of the list)
	/// </summary>
	[SerializeField]
	private Vector2 overscrollAllowance = new Vector2(0.5f, 0.5f);

	// The scroll position of the list.
	// (0 == beginning, 1 == end)
	private Vector2 scrollPos;

	protected bool m_controlIsEnabled = true;
	protected IUIContainer container;

	protected EZInputDelegate inputDelegate;
	protected EZValueChangedDelegate changeDelegate;
	protected ValueSnappedDelegate valueSnappedDel;

	// Tells us if Start() has already run
	protected bool m_started = false;
	// Tells us if Awake() has already run
	protected bool m_awake = false;

	public bool IsScrolling { get { return isScrolling; } }
	protected bool isScrolling;					// Is true when the list is scrolling.
	protected bool noTouch = true;				// Indicates whether there's currently an active touch/click/drag
	protected const float reboundSpeed = 1f;	// Rate at which the list will rebound from the edges.	
	protected const float lowPassKernelWidthInSeconds = 0.045f; // Inertial sampling width, in seconds
	protected float lowPassFilterFactor;		// = scrollDeltaUpdateInterval / lowPassKernelWidthInSeconds;
	Vector2 scrollInertia;						// Holds onto some of the energy from previous scrolling action for a while
	protected const float backgroundColliderOffset = 0.001f; // How far behind the list items the background collider should be placed.
	Vector2 scrollDelta;
	float lastTime = 0;
	float timeDelta = 0;
	float inertiaLerpInterval = 0.06f;			// Lerp the scroll inertia every 60ms
	float inertiaLerpTime;						// Accumulates the amount of time passed since the last inertia lerp
	Vector2 amtOfPlay;							// The sum total of the extents of our list that are not covered by the viewable area.
	float autoScrollDuration;					// Holds the total number of seconds the current auto scroll operation should take
	Vector2 autoScrollStart;						// The starting point of the auto-scroll.
	Vector2 autoScrollPos;						// Where the auto-scroll operation is taking us.
	Vector2 autoScrollDelta;						// The change in scroll position from where we started when we began auto-scrolling.
	float autoScrollTime;						// The total number of seconds passed since we began auto-scrolling.
	public bool AutoScrolling { get { return autoScrolling; } }
	bool autoScrolling = false;					// Flag to tell us if we're presently auto-scrolling.
	//bool listMoved = false;						// Gets set to true whenever the list is scrolled by hand, so that we can track whether we need to snap or not.
	EZAnimation.Interpolator autoScrollInterpolator; // Easing delegate for snapping/auto-scrolling.
	float localUnitsPerPixel;					// The number of local space units per pixel.

	protected void Awake()
	{
		if (m_awake)
			return;
		m_awake = true;

		autoScrollInterpolator = EZAnimation.GetInterpolator(snapEasing);

		lowPassFilterFactor = inertiaLerpInterval / lowPassKernelWidthInSeconds;
	}

	protected void Start()
	{
		if (m_started)
			return;

		m_started = true;

		// Convert our values to the proper units:
		SetupCameraAndSizes();

		lastTime = Time.realtimeSinceStartup;

		//if (slider != null)
		//{
		//    slider.AddValueChangedDelegate(SliderMoved);
		//    slider.AddInputDelegate(SliderInputDel);
		//}

		// Create a background box collider to catch input events between list items:
		if (Application.isPlaying && collider == null && touchScroll)
		{
			BoxCollider bc = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
			bc.size = new Vector3(viewableRectActual.width, viewableRectActual.height, 0.001f);
			bc.center = new Vector3(viewableRectActual.center.x, viewableRectActual.center.y, 0) + Vector3.forward * backgroundColliderOffset; // Set the collider behind where the list items will be.
			bc.isTrigger = true;
		}

		UpdateContentExtents();

		// Use the default threshold if ours has not been set to anything:
		if (float.IsNaN(dragThreshold) || dragThreshold < 0)
			dragThreshold = UIManager.instance.dragThreshold;

		ScrollToValue(defaultValue, 0, EZAnimation.EASING_TYPE.Default, Vector2.zero);
	}

	/// <summary>
	/// Updates any camera-dependent settings, such as the calculated pixel-perfect size.
	/// Use this with BroadcastMessage() to do bulk re-calculation of object sizes whenever your screen-size/resolution changes at runtime.
	/// </summary>
	public void UpdateCamera()
	{
		SetupCameraAndSizes();
	}

	public void SetupCameraAndSizes()
	{
		// Convert our values to the proper units:
		if (renderCamera == null)
		{
			if (UIManager.Exists() && UIManager.instance.uiCameras[0].camera != null)
				renderCamera = UIManager.instance.uiCameras[0].camera;
			else
				renderCamera = Camera.main;
		}

		if (unitsInPixels)
		{
			CalcScreenToWorldUnits();
			viewableAreaActual = new Vector2(viewableArea.x * localUnitsPerPixel, viewableArea.y * localUnitsPerPixel);
		}
		else
		{
			viewableAreaActual = viewableArea;
		}

		// Calculate viewable rect
		viewableRectActual.width = viewableAreaActual.x;
		viewableRectActual.height = viewableAreaActual.y;

		switch (viewableAreaAhchor)
		{
			case ANCHOR_METHOD.UPPER_LEFT:
			case ANCHOR_METHOD.BOTTOM_LEFT:
			case ANCHOR_METHOD.MIDDLE_LEFT:
				viewableRectActual.x = 0;
				break;

			case ANCHOR_METHOD.UPPER_CENTER:
			case ANCHOR_METHOD.MIDDLE_CENTER:
			case ANCHOR_METHOD.BOTTOM_CENTER:
				viewableRectActual.x = -viewableRectActual.width * 0.5f;
				break;

			case ANCHOR_METHOD.UPPER_RIGHT:
			case ANCHOR_METHOD.MIDDLE_RIGHT:
			case ANCHOR_METHOD.BOTTOM_RIGHT:
				viewableRectActual.x = -viewableRectActual.width;
				break;
		}

		switch (viewableAreaAhchor)
		{
			case ANCHOR_METHOD.UPPER_LEFT:
			case ANCHOR_METHOD.UPPER_CENTER:
			case ANCHOR_METHOD.UPPER_RIGHT:
				viewableRectActual.y = -viewableRectActual.height;
				break;

			case ANCHOR_METHOD.MIDDLE_LEFT:
			case ANCHOR_METHOD.MIDDLE_CENTER:
			case ANCHOR_METHOD.MIDDLE_RIGHT:
				viewableRectActual.y = -viewableRectActual.height * 0.5f;
				break;

			case ANCHOR_METHOD.BOTTOM_LEFT:
			case ANCHOR_METHOD.BOTTOM_CENTER:
			case ANCHOR_METHOD.BOTTOM_RIGHT:
				viewableRectActual.y = 0;
				break;
		}

		// Create a background box collider to catch input events between list items:
		if (Application.isPlaying && touchScroll)
		{
			BoxCollider bc;

			if (collider == null)
				bc = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
			else
				bc = (BoxCollider)collider;

			bc.size = new Vector3(viewableRectActual.width, viewableRectActual.height, 0.001f);
			bc.center = new Vector3(viewableRectActual.center.x, viewableRectActual.center.y, 0) + Vector3.forward * backgroundColliderOffset; // Set the collider behind where the list items will be.
			bc.isTrigger = true;
		}
	}

	protected void CalcScreenToWorldUnits()
	{
		Plane nearPlane = new Plane(renderCamera.transform.forward, renderCamera.transform.position);

		// Determine the world distance between two vertical screen pixels for this camera:
		float dist = nearPlane.GetDistanceToPoint(transform.position);
		localUnitsPerPixel = Vector3.Distance(renderCamera.ScreenToWorldPoint(new Vector3(0, 1, dist)), renderCamera.ScreenToWorldPoint(new Vector3(0, 0, dist)));
	}

	// Internal version of ScrollListTo that doesn't eliminate scroll coasting inertia, etc.
	protected void ScrollListTo_Internal(Vector2 pos)
	{
		if (float.IsNaN(pos.x) || float.IsNaN(pos.y) /*|| mover == null*/)
			return; // Ignore since the viewing area exactly matches our content, no need to scroll anyway

		scrollPos = pos;

		//if (slider != null && amtOfPlay > 0)
		//    slider.Value = scrollPos;
	}

	/// <summary>
	/// Scrolls the list directly to the position indicated (0-1).
	/// </summary>
	/// <param name="pos">Position of the list - 0-1 (0 == beginning, 1 == end)</param>
	public void ScrollListTo(Vector2 pos)
	{
		if (!m_started)
			Start();

		scrollInertia = Vector2.zero;
		scrollDelta = Vector2.zero;
		isScrolling = false;
		autoScrolling = false;

		ScrollListTo_Internal(pos);
	}

	/// <summary>
	/// Sets or retrieves the scroll position of the list.
	/// The position is given as a value between 0 and 1.
	/// 0 indicates the beginning of the list, 1 the end.
	/// </summary>
	public Vector2 ScrollPosition
	{
		get { return scrollPos; }
		set { ScrollListTo(value); }
	}

	/// <summary>
	/// Scrolls the list to the specified value in the specified number of seconds.
	/// </summary>
	/// <param name="item">The item to scroll to.</param>
	/// <param name="scrollTime">The number of seconds the scroll should last.</param>
	/// <param name="easing">The type of easing to be used for the scroll.</param>
	public void ScrollToValue(Vector2 value, float scrollTime, EZAnimation.EASING_TYPE easing, Vector2 offset)
	{
		autoScrollPos.x = Mathf.Clamp01((value.x + offset.x - minValue.x) / amtOfPlay.x);
		autoScrollPos.y = Mathf.Clamp01((value.y + offset.y - minValue.y) / amtOfPlay.y);

		autoScrollInterpolator = EZAnimation.GetInterpolator(easing);
		autoScrollStart = scrollPos;
		autoScrollDelta = autoScrollPos - scrollPos;
		autoScrollDuration = scrollTime;
		autoScrollTime = 0;
		autoScrolling = true;

		// Do some state cleanup:
		scrollDelta = Vector2.zero;
		isScrolling = false;

		if (valueSnappedDel != null)
			valueSnappedDel(value);
	}

	public void SetViewableArea(float width, float height)
	{
		viewableArea.x = width;
		viewableArea.y = height;
		UpdateCamera();
	}

	// Updates the extents of the content area by the specified amount.
	// change: The amount the content extents have changed (+/-)
	protected void UpdateContentExtents()
	{
		amtOfPlay = new Vector2(Mathf.Abs(maxValue.x - minValue.x), Mathf.Abs(maxValue.y - minValue.y));
	}

	// Is called by a list item when a drag is detected.
	// For our purposes, it is only called when the drag
	// extends beyond the drag threshold.
	public void ListDragged(POINTER_INFO ptr)
	{
		if (!touchScroll || !controlIsEnabled)
			return;	// Ignore

		autoScrolling = false;

		// Calculate the pointer's motion relative to our control:
		Vector3 inputPoint1;
		Vector3 inputPoint2;
		Vector3 ptrVector;
		float dist;
		Plane ctrlPlane = default(Plane);

		// Early out:
		if (Mathf.Approximately(ptr.inputDelta.sqrMagnitude, 0))
		{
			scrollDelta = Vector2.zero;
			return;
		}

		//listMoved = true;

		ctrlPlane.SetNormalAndPosition(/*mover.*/transform.forward * -1f, /*mover.*/transform.position);

		ctrlPlane.Raycast(ptr.ray, out dist);
		inputPoint1 = ptr.ray.origin + ptr.ray.direction * dist;

		ctrlPlane.Raycast(ptr.prevRay, out dist);
		inputPoint2 = ptr.prevRay.origin + ptr.prevRay.direction * dist;

		// Get the input points into the local space of our list:
		inputPoint1 = transform.InverseTransformPoint(inputPoint1);
		inputPoint2 = transform.InverseTransformPoint(inputPoint2);

		ptrVector = inputPoint1 - inputPoint2;

		// Find what percentage of our content extent this value represents:
		scrollDelta.x = (-ptrVector.x) / amtOfPlay.x * valueScrollFactor.x;
		scrollDelta.y = ptrVector.y / amtOfPlay.y * valueScrollFactor.y;
		//scrollDelta *= transform.localScale.x;

		/*
				ptr.devicePos.z = mover.transform.position.z;
				ptrVector = cam.ScreenToWorldPoint(ptr.devicePos) - cam.ScreenToWorldPoint(ptr.devicePos - ptr.inputDelta);

				if(orientation == ORIENTATION.HORIZONTAL)
				{
					localVector = transform.TransformDirection(Vector3.right);
					scrollDelta = -Vector3.Project(ptrVector, localVector).x;
					scrollDelta *= transform.localScale.x;
					// Find what percentage of our content 
					// extent this value represents:
					scrollDelta /= ( (contentExtents+itemSpacing) - viewableRectActual.width);
				}
				else
				{
					localVector = transform.TransformDirection(Vector3.up);
					scrollDelta = Vector3.Project(ptrVector, localVector).y;
					scrollDelta *= transform.localScale.y;
					// Find what percentage of our content 
					// extent this value represents:
					scrollDelta /= ((contentExtents + itemSpacing) - viewableRectActual.height);
				}
		*/

		Vector2 target = scrollPos + scrollDelta;

		if (target.x > 1f)
		{
			// Scale our delta according to how close we are to reaching our max scroll:
			scrollDelta.x *= Mathf.Clamp01(1f - (target.x - 1f) / overscrollAllowance.x);
		}
		else if (target.x < 0)
		{
			scrollDelta.x *= Mathf.Clamp01(1f + (target.x / overscrollAllowance.x));
		}

		if (target.y > 1f)
		{
			// Scale our delta according to how close we are to reaching our max scroll:
			scrollDelta.y *= Mathf.Clamp01(1f - (target.y - 1f) / overscrollAllowance.y);
		}
		else if (target.y < 0)
		{
			scrollDelta.y *= Mathf.Clamp01(1f + (target.y / overscrollAllowance.y));
		}

		ScrollListTo_Internal(scrollPos + scrollDelta);

		noTouch = false;
		isScrolling = true;
	}

	// Is called by a list item or internally 
	// when the pointing device is released.
	public void PointerReleased()
	{
		noTouch = true;

		if (scrollInertia.x != 0)
			scrollDelta.x = scrollInertia.x;
		if (scrollInertia.y != 0)
			scrollDelta.y = scrollInertia.y;

		scrollInertia = Vector2.zero;
	}

	public void OnInput(POINTER_INFO ptr)
	{
		if (!m_controlIsEnabled)
		{
			if (Container != null)
			{
				ptr.callerIsControl = true;
				Container.OnInput(ptr);
			}

			return;
		}

		// Do our own tap checking with the list's
		// own threshold:
		if (Vector3.SqrMagnitude(ptr.origPos - ptr.devicePos) > (dragThreshold * dragThreshold))
		{
			ptr.isTap = false;
			if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
				ptr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
		}
		else
			ptr.isTap = true;

		if (inputDelegate != null)
			inputDelegate(ref ptr);

		// Change the state if necessary:
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				if (ptr.active)	// If this is a hold
					ListDragged(ptr);
				break;
			case POINTER_INFO.INPUT_EVENT.DRAG:
				if (!ptr.isTap)
					ListDragged(ptr);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			case POINTER_INFO.INPUT_EVENT.TAP:
				PointerReleased();
				break;
		}

		if (Container != null)
		{
			ptr.callerIsControl = true;
			Container.OnInput(ptr);
		}
	}


	public void LateUpdate()
	{
		timeDelta = Time.realtimeSinceStartup - lastTime;
		lastTime = Time.realtimeSinceStartup;
		inertiaLerpTime += timeDelta;

		// Smooth our scroll inertia:
		if (!noTouch)
		{
			if (inertiaLerpTime >= inertiaLerpInterval)
			{
				// Accumulate inertia if we aren't coasting or auto-scrolling:
				scrollInertia.x = Mathf.Lerp(scrollInertia.x, scrollDelta.x, lowPassFilterFactor);
				scrollInertia.y = Mathf.Lerp(scrollInertia.y, scrollDelta.y, lowPassFilterFactor);
				scrollDelta = Vector2.zero;
				inertiaLerpTime = inertiaLerpTime % inertiaLerpInterval;
			}
		}

		if (isScrolling && noTouch && !autoScrolling)
		{
			scrollDelta -= (scrollDelta * scrollDecelCoef);

			// See if we need to rebound from the edge:
			if (scrollPos.x < 0)
			{
				scrollPos.x -= scrollPos.x * reboundSpeed * (timeDelta / 0.166f);
				// Compute resistance:
				scrollDelta.x *= Mathf.Clamp01(1f + (scrollPos.x / overscrollAllowance.x));
			}
			else if (scrollPos.x > 1f)
			{
				scrollPos.x -= (scrollPos.x - 1f) * reboundSpeed * (timeDelta / 0.166f);
				// Compute resistance:
				scrollDelta.x *= Mathf.Clamp01(1f - (scrollPos.x - 1f) / overscrollAllowance.x);
			}

			if (Mathf.Abs(scrollDelta.x) < 0.0001f)
			{
				scrollDelta.x = 0;
				if (scrollPos.x > -0.0001f && scrollPos.x < 0.0001f)
					scrollPos.x = Mathf.Clamp01(scrollPos.x);
			}

			// See if we need to rebound from the edge:
			if (scrollPos.y < 0)
			{
				scrollPos.y -= scrollPos.y * reboundSpeed * (timeDelta / 0.166f);
				// Compute resistance:
				scrollDelta.y *= Mathf.Clamp01(1f + (scrollPos.y / overscrollAllowance.y));
			}
			else if (scrollPos.y > 1f)
			{
				scrollPos.y -= (scrollPos.y - 1f) * reboundSpeed * (timeDelta / 0.166f);
				// Compute resistance:
				scrollDelta.y *= Mathf.Clamp01(1f - (scrollPos.y - 1f) / overscrollAllowance.y);
			}

			if (Mathf.Abs(scrollDelta.y) < 0.0001f)
			{
				scrollDelta.y = 0;
				if (scrollPos.y > -0.0001f && scrollPos.y < 0.0001f)
					scrollPos.y = Mathf.Clamp01(scrollPos.y);
			}

			ScrollListTo_Internal(scrollPos + scrollDelta);

			if ((scrollPos.x >= 0 && scrollPos.x <= 1.001f && scrollDelta.x == 0) &&
				(scrollPos.y >= 0 && scrollPos.y <= 1.001f && scrollDelta.y == 0))
				isScrolling = false;
		}
		else if (autoScrolling)
		{
			// Auto-scroll:
			autoScrollTime += timeDelta;

			if (autoScrollTime >= autoScrollDuration)
			{
				autoScrolling = false;
				scrollPos = autoScrollPos;
			}
			else
			{
				scrollPos.x = autoScrollInterpolator(autoScrollTime, autoScrollStart.x, autoScrollDelta.x, autoScrollDuration);
				scrollPos.y = autoScrollInterpolator(autoScrollTime, autoScrollStart.y, autoScrollDelta.y, autoScrollDuration);
			}

			ScrollListTo_Internal(scrollPos);
		}
	}

	public bool controlIsEnabled
	{
		get { return m_controlIsEnabled; }
		set { m_controlIsEnabled = value; }
	}

	/// <summary>
	/// When set to true, the control will instruct any
	/// pointers which have it as their target to
	/// de-target them.  Use this if you are deactivating
	/// a control and want no input to go to it.
	/// It is strongly recommended NOT to use this feature
	/// on any control that appears in a scroll list, or
	/// else you may be unable to scroll past the edge of
	/// the list's viewable area.
	/// </summary>
	public virtual bool DetargetOnDisable
	{
		get { return DetargetOnDisable; }
		set { DetargetOnDisable = value; }
	}

	// Allows an object to act as a proxy for other
	// controls - i.e. a UIVirtualScreen
	// But in our case, just return ourselves since
	// we're not acting as a proxy
	public IUIObject GetControl(ref POINTER_INFO ptr)
	{
		return this;
	}

	public virtual IUIContainer Container
	{
		get { return container; }
		set { container = value; }
	}

	public bool RequestContainership(IUIContainer cont)
	{
		Transform t = transform.parent;
		Transform c = ((Component)cont).transform;

		while (t != null)
		{
			if (t == c)
			{
				container = cont;
				return true;
			}
			else if (t.gameObject.GetComponent("IUIContainer") != null)
				return false;

			t = t.parent;
		}

		// Never found *any* containers:
		return false;
	}

	public bool GotFocus() { return false; }

	public void SetInputDelegate(EZInputDelegate del)
	{
		inputDelegate = del;
	}

	public void AddInputDelegate(EZInputDelegate del)
	{
		inputDelegate += del;
	}

	public void RemoveInputDelegate(EZInputDelegate del)
	{
		inputDelegate -= del;
	}


	public void SetValueChangedDelegate(EZValueChangedDelegate del)
	{
		changeDelegate = del;
	}

	public void AddValueChangedDelegate(EZValueChangedDelegate del)
	{
		changeDelegate += del;
	}

	public void RemoveValueChangedDelegate(EZValueChangedDelegate del)
	{
		changeDelegate -= del;
	}

	/// <summary>
	/// Adds a delegate to be called when the list snaps to an item.
	/// </summary>
	/// <param name="del">Delegate to be called.</param>
	public void AddValueSnappedDelegate(ValueSnappedDelegate del)
	{
		valueSnappedDel += del;
	}

	/// <summary>
	/// Removes a delegate from the list of those to be called when
	/// the list snaps to an item.
	/// </summary>
	/// <param name="del">The delegate to remove.</param>
	public void RemoveValueSnappedDelegate(ValueSnappedDelegate del)
	{
		valueSnappedDel -= del;
	}

	#region Drag&Drop

	//---------------------------------------------------
	// Drag & Drop stuff
	//---------------------------------------------------
	public object Data
	{
		get { return null; }
		set { }
	}

	public bool IsDraggable
	{
		get { return false; }
		set { }
	}

	public LayerMask DropMask
	{
		get { return -1; }
		set { }
	}

	public float DragOffset
	{
		get { return 0; }
		set { }
	}

	public EZAnimation.EASING_TYPE CancelDragEasing
	{
		get { return EZAnimation.EASING_TYPE.Default; }
		set { }
	}

	public float CancelDragDuration
	{
		get { return 0; }
		set { }
	}

	public bool IsDragging
	{
		get { return false; }
		set { }
	}

	public GameObject DropTarget
	{
		get { return null; }
		set { }
	}

	public bool DropHandled
	{
		get { return false; }
		set { }
	}

	public void DragUpdatePosition(POINTER_INFO ptr) { }

	public void CancelDrag() { }

	// <summary>
	// Receives regular notification of drag & drop events
	// pertaining to this object when an object is being
	// dragged.  This is called on potential drop targets
	// when an object is dragged over them.  It is also
	// called on the object(s) being dragged/dropped.
	// </summary>
	// <param name="parms">The EZDragDropParams structure which holds information about the event.</param>
	public void OnEZDragDrop_Internal(EZDragDropParams parms)
	{
		if (dragDropDelegate != null)
			dragDropDelegate(parms);
	}

	// Delegate to be called on drag and drop notifications.
	protected EZDragDropDelegate dragDropDelegate = null;

	/// <summary>
	/// Adds a delegate to be called with drag and drop notifications.
	/// </summary>
	/// <param name="del">The delegate to add.</param>
	public void AddDragDropDelegate(EZDragDropDelegate del)
	{
		dragDropDelegate += del;
	}

	/// <summary>
	/// Removes a delegate from the list of those to be called 
	/// with drag and drop notifications.
	/// </summary>
	/// <param name="del">The delegate to add.</param>
	public void RemoveDragDropDelegate(EZDragDropDelegate del)
	{
		dragDropDelegate -= del;
	}

	/// <summary>
	/// Sets the delegate to be called with drag and drop notifications.
	/// NOTE: This will replace any previously registered delegates.
	/// </summary>
	/// <param name="del">The delegate to add.</param>
	public void SetDragDropDelegate(EZDragDropDelegate del)
	{
		dragDropDelegate = del;
	}

	// Setters for the internal drag drop handler delegate:
	public void SetDragDropInternalDelegate(EZDragDropHelper.DragDrop_InternalDelegate del) { }
	public void AddDragDropInternalDelegate(EZDragDropHelper.DragDrop_InternalDelegate del) { }
	public void RemoveDragDropInternalDelegate(EZDragDropHelper.DragDrop_InternalDelegate del) { }
	public EZDragDropHelper.DragDrop_InternalDelegate GetDragDropInternalDelegate() { return null; }


	#endregion

	void OnDrawGizmosSelected()
	{
		Vector3 ul, ll, lr, ur;

		SetupCameraAndSizes();

		ul = (transform.position + transform.TransformDirection(Vector3.right * viewableRectActual.x * transform.lossyScale.x) + transform.TransformDirection(Vector3.up * viewableRectActual.yMax * transform.lossyScale.y));
		ll = (transform.position + transform.TransformDirection(Vector3.right * viewableRectActual.x * transform.lossyScale.x) + transform.TransformDirection(Vector3.up * viewableRectActual.y * transform.lossyScale.y));
		lr = (transform.position + transform.TransformDirection(Vector3.right * viewableRectActual.xMax * transform.lossyScale.x) + transform.TransformDirection(Vector3.up * viewableRectActual.y * transform.lossyScale.y));
		ur = (transform.position + transform.TransformDirection(Vector3.right * viewableRectActual.xMax * transform.lossyScale.x) + transform.TransformDirection(Vector3.up * viewableRectActual.yMax * transform.lossyScale.y));

		Gizmos.color = new Color(1f, 0, 0.5f, 1f);
		Gizmos.DrawLine(ul, ll);	// Left
		Gizmos.DrawLine(ll, lr);	// Bottom
		Gizmos.DrawLine(lr, ur);	// Right
		Gizmos.DrawLine(ur, ul);	// Top
	}

	//private Vector2 oldPos = Vector2.zero;
	//void FixedUpdate()
	//{
	//    //if (Mathf.Approximately(Value.x, oldPos.x) && Mathf.Approximately(Value.y, oldPos.y))
	//    //    return;

	//    //oldPos = Value;
	//    //Debug.Log(oldPos);
	//}
}
