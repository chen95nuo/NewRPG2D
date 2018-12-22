using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopOpenResultUI : BWUIPanel {
	
	
	//非宝箱类物品//
	public GameObject OpenResultGiftBox;
	//宝箱、钥匙类物品//
	public GameObject OpenResultChestBox;
	//非宝箱类的（礼包类的）拖拽界面控制//
	public GameObject GiftDragPanel;
	public GameObject GiftScorllBar;
	public GameObject GiftContent;
	
	//宝箱类的//
	public GameObject ChestDragPanel;
	public GameObject ChestScorllBar;
	public GameObject ChestContent;
	public GameObject ChestMustGetContent;
	
	public GameObject[] Prefabs;
	
	//private Transform _myTransform;
	
	private float startX = 0;
	private float dragStartY = 65f;
	private float chestStartY = 0;
	private float offY1 = -65f;
	//当前材料的类型, 2 非宝箱类（gift）, 5/6宝箱，钥匙类, 7 宝物袋//
	private int curItemType;
	//当前是用的物品的数量//
	private int curItemHaveNum;
	
	private List<GameBoxElement> getItems ;
	

	
	// Use this for initialization
	void Start () {
		//_myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowItems()
	{
		CleanChestScrollData();
		CleanGiftScrollData();
		
		int count = getItems.Count;
		int type = 0;
		int num = 0;
		int itemId = 0;
		int dropType = 0;		//掉落物品的类型，1 固定掉落。 0 随机掉落//
		if(curItemType == 5 || curItemType == 6)
		{
			OpenResultGiftBox.SetActive(false);
			OpenResultChestBox.SetActive(true);
		}
		else if(curItemType == 2 || curItemType == 7)		//宝物袋展示或的物品和消耗品是一样的//
		{
			OpenResultChestBox.SetActive(false);
			OpenResultGiftBox.SetActive(true);
		}
		//显示物品//
		for(int i = 0;i < count;i++)
		{
			GameBoxElement gbe = getItems[i];
			type = gbe.goodsType;
			itemId = gbe.goodsId;
			dropType = gbe.dropType;
			num = gbe.num;
			GameObject item = Instantiate(Prefabs[type - 1])as GameObject;

            UILabel label = item.transform.FindChild("Label").GetComponent<UILabel>();




            
			//显示icon//
			if(type <= 5)
			{
				SimpleCardInfo2 sci2 = item.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
				GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Hero;
				switch(type)
				{
				case 1:			//item//
					cardType = GameHelper.E_CardType.E_Item;
                    label.text = GetName(itemId, GameHelper.E_CardType.E_Item) + "     x" + num;
					break;
				case 2:			//equip//
					cardType = GameHelper.E_CardType.E_Equip;
                    label.text = GetName(itemId, GameHelper.E_CardType.E_Equip) + "     x" + num;
					break;
				case 3:			//card//
					cardType = GameHelper.E_CardType.E_Hero;
                    label.text = GetName(itemId, GameHelper.E_CardType.E_Hero) + "     x" + num;
					break;
				case 4:			//skill//
					cardType = GameHelper.E_CardType.E_Skill;
                    label.text = GetName(itemId, GameHelper.E_CardType.E_Skill) + "     x" + num;
					break;
				case 5:			//passiveSkill//
					cardType = GameHelper.E_CardType.E_PassiveSkill;
                    label.text = GetName(itemId, GameHelper.E_CardType.E_PassiveSkill) + "     x" + num;
					break;
					
				}
				sci2.setSimpleCardInfo(itemId, cardType);
			}
			else 
			{
			}

            switch (type)
            { 
                case 6:
                    label.text = TextsData.getData(658).chinese + "     x" + num;
                    break;
                case 7:
                    label.text = TextsData.getData(662).chinese + "     x" + num;
                    break;
                case 8:
                    label.text = TextsData.getData(659).chinese + "     x" + num;
                    break;
                case 9:
                    label.text = TextsData.getData(660).chinese + "     x" + num;
                    break;
                case 10:
                    label.text = TextsData.getData(661).chinese + "     x" + num;
                    break;
                case 11:
                    label.text = TextsData.getData(657).chinese + "     x" + num;
                    break;
            }

			//修改位置//
			if(curItemType == 5 || curItemType == 6)
			{
				//显示固定获得物品,如果为宝箱类，则第一个是固定获得的物品//
				if(dropType == 1)
				{
					GameObjectUtil.gameObjectAttachToParent(item, ChestMustGetContent);
					item.transform.localPosition = new Vector3(startX, chestStartY, 0);
				}
				else 
				{
					GameObjectUtil.gameObjectAttachToParent(item, ChestContent);
					item.transform.localPosition = new Vector3(startX, dragStartY + i * offY1, 0);
				}
				
			}
			else if(curItemType == 2 || curItemType == 7)
			{
				GameObjectUtil.gameObjectAttachToParent(item,GiftContent);
				item.transform.localPosition = new Vector3(startX, dragStartY + i * offY1, 0);
			}
            item.transform.localPosition = new Vector3(-86f, item.transform.localPosition.y, item.transform.localPosition.z);
		}
	}
    public string GetName(int CardId, GameHelper.E_CardType type)
    {
        string name = "";
        switch (type)
        {
            case GameHelper.E_CardType.E_Hero:
                {
                    CardData cd = CardData.getData(CardId);
                    if (cd == null)
                        return "";
                    name = cd.name;
                } break;
            case GameHelper.E_CardType.E_Equip:
                {
                    EquipData ed = EquipData.getData(CardId);
                    if (ed == null)
                        return "";
                    name = ed.name;
                } break;
            case GameHelper.E_CardType.E_Item:
                {
                    ItemsData itemData = ItemsData.getData(CardId);
                    if (itemData == null)
                        return "";
                    name = itemData.name;
                } break;
            case GameHelper.E_CardType.E_Skill:
                {
                    SkillData sd = SkillData.getData(CardId);
                    if (sd == null)
                        return "";
                    name = sd.name;
                } break;
            case GameHelper.E_CardType.E_PassiveSkill:
                {
                    PassiveSkillData psd = PassiveSkillData.getData(CardId);
                    if (psd == null)
                        return "";
                    name = psd.name;
                } break;
        }
        return name;
    }
	
	public override void show ()
	{
		base.show ();
		ShowItems();
	}
	
	public void SetData(int itemType, List<GameBoxElement> list)
	{
		curItemType = itemType;
		this.getItems = list;
		show();
	}
	
	
	public override void hide ()
	{
		CleanChestScrollData();
		CleanGiftScrollData();
		base.hide ();
	}
	
	
	public void CleanGiftScrollData()
	{
		GiftScorllBar.GetComponent<UIScrollBar>().value = 0;
		GiftDragPanel.transform.localPosition = Vector3.zero;
		GiftDragPanel.GetComponent<UIPanel>().clipRange = new Vector4(0,0,1000,230);
		GameObjectUtil.destroyGameObjectAllChildrens(GiftContent);
	}
	
	public void CleanChestScrollData()
	{
		ChestScorllBar.GetComponent<UIScrollBar>().value = 0;
		ChestDragPanel.transform.localPosition = Vector3.zero;
        ChestDragPanel.GetComponent<UIPanel>().clipRange = new Vector4(0, 0, 1000, 230);
		GameObjectUtil.destroyGameObjectAllChildrens(ChestContent);
		
		GameObjectUtil.destroyGameObjectAllChildrens(ChestMustGetContent);
	}
	
	public void OnClickCloseBtn()
	{
		hide();
		
	}
	
	public void gc()
	{
		getItems.Clear();
		getItems = null;
		GameObject.Destroy(_MyObj);
	}
	
	
}
