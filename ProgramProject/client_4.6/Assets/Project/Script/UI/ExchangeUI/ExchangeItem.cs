using UnityEngine;
using System.Collections;

public class ExchangeItem : MonoBehaviour {
	
//	public UISprite Icon;
//	public UISprite Star;
	public UISprite CostIcon;		//消耗的材料的图标//
	public UILabel Name;
	public UILabel Des;				//描述//
	public UILabel CostNum;		//消耗的的数量//
	public GameObject Selected;		//选中效果//
	public int id;					//该兑换物品在兑换表中的id//
	
	
	public SimpleCardInfo2 cardInfo;
}
