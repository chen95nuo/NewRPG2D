using UnityEngine;
using System.Collections;

public class UIElemChooseCardBasic : MonoBehaviour 
{
	public UIElemAssetIcon itemIcon;
	public UIButton itemBg;
	public UIBox itemSelected;
	public UIBox gouBox;
	public SpriteText itemQualityLabel;

	private string _guid = "";
	public bool IsSelected { get { return !gouBox.IsHidden(); } }
	public bool ItemSelected
	{
		get { return itemSelected.StateNum == 0; }
	}

	public string Guid
	{
		get { return _guid; }
	}

	public void ToggleState()
	{
		SetToggleState(!this.ItemSelected);
	}

	protected void SetToggleState(bool selected)
	{
		if (selected)
			itemSelected.SetToggleState(0);
		else
			itemSelected.SetToggleState(1);

		gouBox.Hide(!selected);
	}

	protected void SetBaseData(int resourceId, string guid, bool selected)
	{
		_guid = guid;
		itemQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(resourceId);
		SetToggleState(selected);
	}
}
