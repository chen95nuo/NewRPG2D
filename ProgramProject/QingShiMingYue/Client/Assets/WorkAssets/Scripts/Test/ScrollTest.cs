#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollTest : MonoBehaviour
{
	public class Pair
	{
		public UIListItemContainer go;
		public bool enable;

		public Pair() { }
		public Pair(UIListItemContainer go, bool enable)
		{
			this.go = go;
			this.enable = enable;
		}
	}

	public int counter = 0;
	public GameObjectPool pool;
	public UIScrollList scrollList;
	public bool listItemAddingToTail = false;
	public bool listItemAddingToHead = false;

	// Use this for initialization
	void Start()
	{

		scrollList.AddInputDelegate(EZInputDelegate);
		scrollList.AddValueChangedDelegate(EZValueChangedDelegate);
		scrollList.AddItemSnappedDelegate(ItemSnappedDelegate);
		scrollList.AddDragDropDelegate(EZDragDropDelegate);
		scrollList.AddDragDropInternalDelegate(DragDrop_InternalDelegate);

		StartCoroutine("Fill");
	}

	[System.Reflection.Obfuscation(Exclude=false, Feature="ExcludeMethodName")]
	IEnumerator Fill()
	{
		yield return null;

		for (int i = 0; i < 3; ++i)
		{
			AddTailItem();
		}		
	}

	// Update is called once per frame
	void Update()
	{
		if (scrollList.AutoScrolling)
			return;

		if (scrollList.IsScrolling)
		{
			if (listItemAddingToHead == false && listItemAddingToTail == false)
			{
				if (scrollList.ScrollPosition < 0 - 0.01)
					listItemAddingToHead = true;
				else if (scrollList.ScrollPosition > 1 + 0.01)
					listItemAddingToTail = true;
			}

			return;
		}

		if (listItemAddingToHead)
		{
			Debug.Log("listItemAddingToHead");
			listItemAddingToHead = false;
			AddFrontItem();
			if (scrollList.Count > 5)
				RemoveTailItem();
		}
		else if (listItemAddingToTail)
		{
			Debug.Log("listItemAddingToTail");
			listItemAddingToTail = false;
			AddTailItem();
			if (scrollList.Count > 5)
				RemoveFrontItem();
		}
	}

	void AddFrontItem()
	{
		UIListItemContainer go = pool.AllocateItem().GetComponent<UIListItemContainer>();
		go.spriteText.Text = (counter++).ToString();
		scrollList.InsertItem(go, 1, false, null, false);
	}

	void RemoveFrontItem()
	{
		if (scrollList.Count == 0)
			return;

		scrollList.RemoveItem(1, false, false, false);
	}

	void AddTailItem()
	{
		UIListItemContainer go = pool.AllocateItem().GetComponent<UIListItemContainer>();
		go.spriteText.Text = (counter++).ToString();
		scrollList.InsertItem(go, scrollList.Count - 1, true, null, false);
	}

	void RemoveTailItem()
	{
		if (scrollList.Count == 0)
			return;

		scrollList.RemoveItem(scrollList.Count - 2, false, true, false);
	}

	void EZInputDelegate(ref POINTER_INFO ptr)
	{
		//Debug.Log("EZInputDelegate");
	}

	void EZValueChangedDelegate(IUIObject obj)
	{
		Debug.Log("EZValueChangedDelegate");
	}

	void ItemSnappedDelegate(IUIListObject item)
	{
		Debug.Log("ItemSnappedDelegate");
	}

	void EZDragDropDelegate(EZDragDropParams parms)
	{
		Debug.Log("EZDragDropDelegate");
	}

	void DragDrop_InternalDelegate(ref POINTER_INFO ptr)
	{
		Debug.Log("DragDrop_InternalDelegate");
	}

	[System.Reflection.Obfuscation(Exclude=false, Feature="ExcludeMethodName")]
	void OnClickButton(UIButton btn)
	{
		AddFrontItem();
		//RemoveItem(btn.transform.parent.GetComponent<UIListItemContainer>());
	}
}
#endif