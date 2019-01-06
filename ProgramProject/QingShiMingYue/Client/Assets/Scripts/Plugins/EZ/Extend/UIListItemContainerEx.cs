using UnityEngine;
using System;

public class UIListItemContainerEx : UIListItemContainer
{
	public UIGameObjectPool subItemPool;

	private GameObject subItem;
	public GameObject SubItem { get { return subItem; } }

	override public void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying && subItem == null)
		{
			subItem = subItemPool.AllocateItem();
			ObjectUtility.AttachToParentAndResetLocalTrans(this.gameObject, subItem);
			this.ScanChildren();
			this.FindOuterEdges();
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying && subItem != null)
		{
			subItemPool.ReleaseItem(subItem);
			subItem = null;
			//this.ScanChildren();
			//this.FindOuterEdges();
		}

		base.OnDisabled();
	}

	public override void ReturnToPool()
	{
		if (subItem != null)
		{
			subItemPool.ReleaseItem(this.subItem);
			subItem = null;
			this.ScanChildren();
			this.FindOuterEdges();
		}

		base.ReturnToPool();
	}

	override public Vector2 TopLeftEdge
	{
		get
		{
			if (subItem != null)
				return base.TopLeftEdge;

			return subItemPool != null ? subItemPool.TopLeftEdge : Vector2.zero;
		}
	}

	override public Vector2 BottomRightEdge
	{
		get
		{
			if (subItem != null)
				return base.BottomRightEdge;

			return subItemPool != null ? subItemPool.BottomRightEdge : Vector2.zero;
		}
	}
}
