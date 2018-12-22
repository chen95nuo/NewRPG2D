using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopShowBoxDataUI : MonoBehaviour {
	
	
	public UILabel TitleLabel;
	public GameObject  ScrollView;
	public GameObject GridList;
	public GameObject GameBoxItem;
	public UIScrollBar ScrollBar;
	
	private Transform _myTransform;
	private int boxId;
	private List<GameBoxData> curShowList = new List<GameBoxData>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void init()
	{
		if(_myTransform == null)
		{
			_myTransform = transform;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowData()
	{
		CleanScrollData();
		ItemsData item = ItemsData.getData(boxId);
		TitleLabel.text = item.name + "\r\n" + TextsData.getData(593).chinese ;
		for(int i = 0;i < curShowList.Count;i ++)
		{
			GameBoxData gameBox = curShowList[i];
			GameObject box = Instantiate(GameBoxItem) as GameObject ;
			GameObjectUtil.gameObjectAttachToParent(box, GridList);
			
			SimpleCardInfo2 sci = box.transform.FindChild("Card").GetComponent<SimpleCardInfo2>();
			UILabel Name = box.transform.FindChild("Name").GetComponent<UILabel>();
			if(gameBox.goodstpye == 1)			//item//
			{
				sci.setSimpleCardInfo(gameBox.itemid, GameHelper.E_CardType.E_Item);
				ItemsData itemD = ItemsData.getData(gameBox.itemid);
				Name.text = itemD.name;
			}
			else if(gameBox.goodstpye == 2)		//equip//
			{
				sci.setSimpleCardInfo(gameBox.itemid, GameHelper.E_CardType.E_Equip);
				EquipData ed = EquipData.getData(gameBox.itemid);
				Name.text = ed.name;
			}
			else if(gameBox.goodstpye == 3)		//card//
			{
				sci.setSimpleCardInfo(gameBox.itemid, GameHelper.E_CardType.E_Hero);
				CardData cd = CardData.getData(gameBox.itemid);
				Name.text = cd.name;
			}
		}
		
		
	}
	
	public void SetData(List<GameBoxData> showList, int boxId)
	{
		curShowList = showList;
		this.boxId = boxId;
		init();
		_myTransform.gameObject.SetActive(true);
		ShowData();
	}
	
	
	public void hide()
	{
		if(_myTransform!=null)
		{
			_myTransform.gameObject.SetActive(false);
		}
		else 
		{
			gameObject.SetActive(false);
		}
		CleanScrollData();
	}
	
	public void CleanScrollData()
	{
		GridList.GetComponent<UIGrid2>().repositionNow = true;
		ScrollBar.value = 0;
		ScrollBar.barSize = 1;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,370,370);
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
	}
	
	public void gc()
	{
		curShowList.Clear();
	}
	
	public void OnClickCloseBtn()
	{
		hide();
	}
}
