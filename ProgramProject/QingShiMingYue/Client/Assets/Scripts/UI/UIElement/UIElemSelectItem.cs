using System;
using System.Collections.Generic;

public class UIElemSelectItem : MonoBehaviour
{
	public UIButton selectBtn;
	public UIBox stateBox;

	public delegate void ChangeSelectStateDel(bool state, object data);

	private object data;
	public object Data
	{
		get { return data; }
	}

	private ChangeSelectStateDel onStateChangeDel;

	public bool IsSelected { get { return IsActive() && !stateBox.IsHidden(); } }

	public void Awake()
	{
		selectBtn.Data = this;
		selectBtn.scriptWithMethodToInvoke = this;
		selectBtn.methodToInvoke = "OnClickSelect";
	}

	public void InitState(object data, ChangeSelectStateDel del)
	{
		this.data = data;
		this.onStateChangeDel = del;

		stateBox.Hide(true);
	}

	public void InitState(object data)
	{
		InitState(data, null);
	}

	public void AddStateChangeDel(ChangeSelectStateDel del)
	{
		this.onStateChangeDel += del;
	}

	public void SetState(bool IsSelected)
	{
		stateBox.Hide(!IsSelected);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		stateBox.Hide(IsSelected);

		if (onStateChangeDel != null)
			onStateChangeDel(IsSelected, data);
	}

	public void SetActive(bool active)
	{
		this.gameObject.SetActive(active);
	}

	public bool IsActive()
	{
		return this.gameObject.activeSelf;
	}
}