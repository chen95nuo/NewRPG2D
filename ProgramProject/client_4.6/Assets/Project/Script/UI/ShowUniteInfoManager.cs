using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowUniteInfoManager : BWUIPanel {
	
	public static ShowUniteInfoManager mInstance;
	public UISprite Icon;
	public UILabel Name;
	public UILabel Des;
	public UILabel NeedCardsTip;
	public UILabel NeedNames;
	
	private int curUniteId;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setData(int uniteSkillId)
	{
		this.curUniteId = uniteSkillId;
		show();
		initData();
	}
	
	
	public void initData()
	{
		
		UnitSkillData usd = UnitSkillData.getData(curUniteId);
		
		
		Icon.spriteName = usd.icon;
		
		Name.text = usd.name;
		Des.text = usd.description;
		
		string s1 = TextsData.getData(169).chinese;    	//组成条件//
		string s2 = TextsData.getData(188).chinese;    	//上阵以下任意//
		string s3 = TextsData.getData(189).chinese;		//卡牌//
		NeedCardsTip.gameObject.SetActive(true);
		NeedCardsTip.text =  s1 + "[FFEBB8]" + s2 + usd.cardnum + s3 + "[-]";
		
		
		//修改需要卡牌名字//
		List<int> ids = new List<int>();
		int id = usd.card1;
		if(id > 0)
		{
			ids.Add(id);
		}
		id = usd.card2;
		if(id > 0)
		{
			ids.Add(id);
		}
		id = usd.card3;
		if(id > 0)
		{
			ids.Add(id);
		}
		id = usd.card4;
		if(id > 0)
		{
			ids.Add(id);
		}
		id = usd.card5;
		if(id > 0)
		{
			ids.Add(id);
		}
		id = usd.card6;
		if(id > 0)
		{
			ids.Add(id);
		}
		NeedNames.text = string.Empty;
		if(ids.Count > 0)
		{
			for(int m = 0; m < ids.Count;m++)
			{
				NeedNames.gameObject.SetActive(true);
				
				//判断当前的卡组中是否装备了改卡牌//
				s1 = "、[00FF00]";
				if(m == 0)
				{
					s1 = "[00FF00]";
				}
				else if(m == 3)
				{
					s1 = "\n\r" + "[00FF00]";
				}
				NeedNames.text += s1 + CardData.getData(ids[m]).name +"[-]";
				
			}
			
		}
		else 
		{
			NeedNames.text = "[00FF00]" + TextsData.getData(503).chinese + "[-]";
		}
		
	
	}
	
	
	public override void hide ()
	{
		base.hide ();
	}
}
