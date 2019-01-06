using UnityEngine;
using System.Collections.Generic;

public class UIGameObjectPool : GameObjectPool
{
	bool outerEdgesValid = false;

	Vector2 topLeftEdge;
	public Vector2 TopLeftEdge
	{
		get
		{
			if (!outerEdgesValid)
				FindOuterEdges();
			return topLeftEdge;
		}
	}

	Vector2 bottomRightEdge;
	public Vector2 BottomRightEdge
	{
		get
		{
			if (!outerEdgesValid)
				FindOuterEdges();
			return bottomRightEdge;
		}
	}

	protected override void Awake()
	{
		base.Awake();

		FindOuterEdges();
	}

	private void FindOuterEdges()
	{
		outerEdgesValid = true;

		List<SpriteRoot> uiObjs = new List<SpriteRoot>();

		SpriteRoot obj;
		Component[] objs = sceneObject.transform.GetComponentsInChildren(typeof(SpriteRoot), true);

		for (int i = 0; i < objs.Length; ++i)
		{
			// Don't add ourselves as children:
			if (objs[i].gameObject == sceneObject)
				continue;

			obj = (SpriteRoot)objs[i];
			uiObjs.Add(obj);
		}

		//----------------------------------------------------
		// Again for SpriteText:
		//----------------------------------------------------
		List<SpriteText> textObjs = new List<SpriteText>();

		SpriteText txt;
		Component[] txts = transform.GetComponentsInChildren(typeof(SpriteText), true);

		for (int i = 0; i < txts.Length; ++i)
		{
			// Don't add ourselves as children:
			if (txts[i].gameObject == sceneObject)
				continue;

			// Only process text objects that aren't already associated
			// with controls:
			txt = (SpriteText)txts[i];

			if (txt.Parent != null)
				continue;

			textObjs.Add(txt);
		}

		topLeftEdge = Vector2.zero;
		bottomRightEdge = Vector2.zero;

		Matrix4x4 sm;
		Matrix4x4 lm = transform.worldToLocalMatrix;
		Vector3 tl, br;

		// Search SpriteText objects:
		for (int i = 0; i < textObjs.Count; ++i)
		{
			if (textObjs[i] == null)
				continue;

			textObjs[i].Start();
			sm = textObjs[i].transform.localToWorldMatrix;
			tl = lm.MultiplyPoint3x4(sm.MultiplyPoint3x4(textObjs[i].UnclippedTopLeft));
			br = lm.MultiplyPoint3x4(sm.MultiplyPoint3x4(textObjs[i].UnclippedBottomRight));
			topLeftEdge.x = Mathf.Min(topLeftEdge.x, tl.x);
			topLeftEdge.y = Mathf.Max(topLeftEdge.y, tl.y);
			bottomRightEdge.x = Mathf.Max(bottomRightEdge.x, br.x);
			bottomRightEdge.y = Mathf.Min(bottomRightEdge.y, br.y);
		}

		// Search sprites and controls:
		for (int i = 0; i < uiObjs.Count; ++i)
		{
			if (uiObjs[i] == null)
				continue;

			sm = uiObjs[i].transform.localToWorldMatrix;

			if (uiObjs[i] is AutoSpriteControlBase)
			{
				((AutoSpriteControlBase)uiObjs[i]).FindOuterEdges();
				tl = lm.MultiplyPoint3x4(sm.MultiplyPoint3x4(((AutoSpriteControlBase)uiObjs[i]).TopLeftEdge));
				br = lm.MultiplyPoint3x4(sm.MultiplyPoint3x4(((AutoSpriteControlBase)uiObjs[i]).BottomRightEdge));
			}
			else
			{
				tl = lm.MultiplyPoint3x4(sm.MultiplyPoint3x4(uiObjs[i].UnclippedTopLeft));
				br = lm.MultiplyPoint3x4(sm.MultiplyPoint3x4(uiObjs[i].UnclippedBottomRight));
			}
			topLeftEdge.x = Mathf.Min(topLeftEdge.x, tl.x);
			topLeftEdge.y = Mathf.Max(topLeftEdge.y, tl.y);
			bottomRightEdge.x = Mathf.Max(bottomRightEdge.x, br.x);
			bottomRightEdge.y = Mathf.Min(bottomRightEdge.y, br.y);
		}

		// 计算出边缘数据后，将AutoSpriteControlBase的BoxCollider手动删除
		// 原因是AutoSpriteControlBase 的FindOuterEdges方法会调用Start方法（如果start方法没有执行过）创建BoxCollider组件
		// 在使用GameObjectPool的AllocateItem方法时，得到的item已经是拥有BoxCollider组件，因此再Awake方法时customCollider变量为true。
		// UpdateCollider方法中就会返回不会进行区域裁剪
		for (int i = 0; i < uiObjs.Count; ++i)
		{
			if (uiObjs[i] == null)
				continue;

			if (uiObjs[i] is AutoSpriteControlBase)
			{
				var boxCollider = ((AutoSpriteControlBase)uiObjs[i]).GetComponent<BoxCollider>();
				if (boxCollider != null)
					GameObject.Destroy(boxCollider);
			}
		}
	}
}
