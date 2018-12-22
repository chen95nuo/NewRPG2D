using UnityEngine;
using System.Collections;

public class RewardsDatasControl : BWUIPanel {
	
	public static RewardsDatasControl mInstance;
	public UILabel Name;
	public UILabel Level;
	public UILabel Sale;
	public UILabel Des;
	
	public SimpleCardInfo2 cardInfo;
	
	private int rewardsType;
	private int curRewardsId;
	private int curRewardsLevel;
	
	void Awake()
	{
		
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
		
	}
	
	public override void init ()
	{
//		base.init ();
		_MyObj.transform.localPosition = new Vector3(0,0,-720);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetData(int formID,GameHelper.E_CardType cardType,string name, string frameName, string des, int level, int sale, int type)
	{
		show();
		cardInfo.clear();
		cardInfo.setSimpleCardInfo(formID,cardType);
		Name.text = name;
		if(type == 3)			//只有是卡牌时才显示等级//
		{
			Level.gameObject.SetActive(true);
			Level.text = "LV." + level.ToString();
		}
		else 
		{
			Level.gameObject.SetActive(false);
		}
		
		string s1 = TextsData.getData(334).chinese;
		Sale.text = s1 + sale;
		int valueNum = 0;
		if(cardType == GameHelper.E_CardType.E_Skill)
		{
			SkillData sd = SkillData.getData(formID);
			int sdType = sd.type;
			int star = sd.star;
			valueNum = SkillPropertyData.getProperty(sdType, level, star);
		}
		else if(cardType == GameHelper.E_CardType.E_Equip)
		{
			EquipData ed = EquipData.getData(formID);
			int eType = ed.type;
			int star = ed.star;
			valueNum = EquippropertyData.getValue(eType, level, star);
		}
		if(valueNum > 0)
		{
			if(cardType == GameHelper.E_CardType.E_Skill)
			{
				Des.text = des;
			}
			else
			{
				Des.text = des + valueNum;
			}
		}
		else 
		{
			Des.text = des;
		}
		
	}
	
	public override void show ()
	{
		base.show ();
	}
	
	public override void hide ()
	{
		base.hide ();
		cardInfo.clear();
		Resources.UnloadUnusedAssets();
	}
	
	public void gc()
	{
		GameObject.Destroy(_MyObj);		
		mInstance = null;
		_MyObj = null;
	}
	
	public void DrawUI()
	{
		
	}
	
}
