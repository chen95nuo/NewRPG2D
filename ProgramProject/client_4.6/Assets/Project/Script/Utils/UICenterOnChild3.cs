using UnityEngine;

/// <summary>
/// Ever wanted to be able to auto-center on an object within a draggable panel?
/// Attach this script to the container that has the objects to center on as its children.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Center On Child3(horizontal)")]
public class UICenterOnChild3 : MonoBehaviour
{
	/// <summary>
	/// The strength of the spring.
	/// </summary>

	public float springStrength = 8f;

	/// <summary>
	/// Callback to be triggered when the centering operation completes.
	/// </summary>

	public SpringPanel.OnFinished onFinished;

	UIDraggablePanel mDrag;
	GameObject mCenteredObject;

	/// <summary>
	/// Game object that the draggable panel is currently centered on.
	/// </summary>

	public GameObject centeredObject { get { return mCenteredObject; } }

	void OnEnable () { Recenter(); }
	void OnDragFinished () { if (enabled) Recenter(); }

	/// <summary>
	/// Recenter the draggable list on the center-most child.
	/// </summary>

	public void Recenter ()
	{
		if (mDrag == null)
		{
			mDrag = NGUITools.FindInParents<UIDraggablePanel>(gameObject);

			if (mDrag == null)
			{
				Debug.LogWarning(GetType() + " requires " + typeof(UIDraggablePanel) + " on a parent object in order to work", this);
				enabled = false;
				return;
			}
			else
			{
				mDrag.onDragFinished = OnDragFinished;
				
				if (mDrag.horizontalScrollBar != null)
					mDrag.horizontalScrollBar.onDragFinished = OnDragFinished;

				if (mDrag.verticalScrollBar != null)
					mDrag.verticalScrollBar.onDragFinished = OnDragFinished;
			}
		}
		if (mDrag.panel == null) return;
		
		/**如果水平bar或垂直bar的值为0或1,拖动时不用再停留到center litao@2013.11.26**/
		if (mDrag.horizontalScrollBar != null && (mDrag.horizontalScrollBar.value==0 || mDrag.horizontalScrollBar.value==1))
		{
			return;
		}
		if (mDrag.verticalScrollBar != null && (mDrag.verticalScrollBar.value==0 || mDrag.verticalScrollBar.value==1))
		{
			return;
		}
		
		// Calculate the panel's center in world coordinates
		Vector4 clip = mDrag.panel.clipRange;
		Transform dt = mDrag.panel.cachedTransform;
		Vector3 center = dt.localPosition;
		center.x += clip.x;
		center.y += clip.y;
		center = dt.parent.TransformPoint(center);
		
		// Offset this value by the momentum
		Vector3 offsetCenter = center - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
		mDrag.currentMomentum = Vector3.zero;
		
		float min = float.MaxValue;
		Transform closest = null;
		Transform trans = transform;

		// Determine the closest child
		for (int i = 0, imax = trans.childCount; i < imax; ++i)
		{
			Transform t = trans.GetChild(i);
			float sqrDist = Vector3.SqrMagnitude(t.position - offsetCenter);

			if (sqrDist < min)
			{
				min = sqrDist;
				closest = t;
			}
		}

		if (closest != null)
		{
			mCenteredObject = closest.gameObject;

			// Figure out the difference between the chosen child and the panel's center in local coordinates
			Vector3 cp = dt.InverseTransformPoint(closest.position);
			Vector3 cc = dt.InverseTransformPoint(center);
			Vector3 offset = cp - cc;

			// Offset shouldn't occur if blocked by a zeroed-out scale
			if (mDrag.scale.x == 0f) offset.x = 0f;
			if (mDrag.scale.y == 0f) offset.y = 0f;
			if (mDrag.scale.z == 0f) offset.z = 0f;
			
			/**每次滑动超过行宽的一半就换列 lt@2013-11-27**/
			Vector3 temp=dt.localPosition - offset;
			
			float cellWidth=gameObject.GetComponent<UIGrid>().cellWidth;
			float mul=dt.localPosition.x/cellWidth;

			int intMul=(int)mul;
			float half=mul-intMul;
			
			float x=(intMul-1)*cellWidth;
			if(closest==gameObject.transform.GetChild(0))
			{
				x=intMul*cellWidth;
			}
			if(intMul<0 && Mathf.Abs(intMul-1)>=gameObject.transform.childCount)
			{
				x=intMul*cellWidth;
			}
			if(Mathf.Abs(half)<0.5)
			{
				x=((int)mul)*cellWidth;
			}
			Vector3 offset2=new Vector3(x,temp.y,temp.z);
			
			
			// Spring the panel to this calculated position
			//SpringPanel.Begin(mDrag.gameObject, dt.localPosition - offset, springStrength).onFinished = onFinished;
			SpringPanel.Begin(mDrag.gameObject, offset2, springStrength).onFinished = onFinished;
			
		}
		else mCenteredObject = null;
	}
}