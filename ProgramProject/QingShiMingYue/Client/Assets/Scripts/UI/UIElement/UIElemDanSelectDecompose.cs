using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemDanSelectDecompose : MonoBehaviour
{
	public UIButton selectBtn;
	public UIBox activityLight;
	public SpriteText qualityLabel;
	private int quality;
	public int Quality
	{
		get { return quality; }
	}
	
	private bool isSelect;
	public bool IsSelect
	{
		get { return isSelect; }
	}

	public void Init(int quality)
	{
		this.quality = quality;
		selectBtn.Data = this;
		qualityLabel.Text = GameUtility.GetUIString("UI_Dan_Quality_" + quality);
		isSelect = false;
		activityLight.gameObject.SetActive(isSelect);
	}

	public void Select()
	{
		isSelect = !isSelect;
		activityLight.gameObject.SetActive(isSelect);
	}
}