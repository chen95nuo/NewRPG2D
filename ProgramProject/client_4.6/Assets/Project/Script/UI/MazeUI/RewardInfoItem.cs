using UnityEngine;
using System.Collections;

public class RewardInfoItem : MonoBehaviour {
	public SimpleCardInfo2 sci2;
	public UILabel nameLabel;
	public UILabel des;
	public UISprite debris;//碎片//
	public UISprite wish;//许愿标记//
	public UISprite BG;//不能许愿的要用黑底盖住//
	private bool isCanWish;
	public UIButtonMessage wishBtn;
	private int id = 0;
	
	public void SetId(int id)
	{
		this.id = id;
	}
	
	public int GetId()
	{
		return this.id;
	}
	
	public void SetIsCanWish(bool isCan)
	{
		isCanWish = isCan;
	}
	
	public bool GetIsCanWish()
	{
		return isCanWish;
	}
}
