using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelHelp : MonoBehaviour {
	
	public List<UIToggle> listCkb=new List<UIToggle>();
	public UIPanel myPanel;
	
	void Awake()
	{
		myPanel=gridTitle.transform.parent.GetComponent<UIPanel>();
	}
	
	void OnEnable()
	{
		OnRootClick (listCkb[0].gameObject);	
	}
	

	
	void OnRootClick(GameObject obj)
	{
		myPanel.transform.localPosition=new Vector3(myPanel.transform.localPosition.x,0,myPanel.transform.localPosition.z);
		myPanel.clipRange=new Vector4(myPanel.clipRange.x,0,myPanel.clipRange.z,myPanel.clipRange.w);
		myPanel.clipOffset = Vector2.zero;
		//gridTitle.transform.parent.transform.localPosition=new Vector3(gridTitle.transform.parent.transform.localPosition.x,0,gridTitle.transform.parent.transform.localPosition.z);
		gridTitle.Reposition ();
		UIToggle tempCkb=obj.GetComponent<UIToggle>();
		int num=0;
		foreach(UIToggle item in listCkb)
		{
			if(tempCkb==item)
			{
				GetTitle (num.ToString ());
				break;
			}
			num++;
		}
	}
	
	public BtnHelp insBtnHelp;
	public UILabel lblInfo;
	private List<BtnHelp> listTitle=new List<BtnHelp>();
	public UIGrid gridTitle;
	void GetTitle(string mType)
	{
		List<yuan.YuanMemoryDB.YuanRow> listRow=YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytHelp.SelectRowsListEqual("HelpType",mType);
		foreach(BtnHelp item in listTitle)
		{
			item.gameObject.SetActiveRecursively (false);
		}
		
		if(listRow!=null)
		{
			int num=0;
			BtnHelp btn;
			foreach(yuan.YuanMemoryDB.YuanRow item in listRow)
			{
				if(listTitle.Count>num)
				{
					listTitle[num].yr=item;
					listTitle[num].title.text=item["HelpName"].YuanColumnText;
					listTitle[num].info=this.lblInfo;
					listTitle[num].gameObject.SetActiveRecursively (true);
					btn=listTitle[num];
				}
				else
				{
					BtnHelp tempBtn=(BtnHelp)Instantiate(insBtnHelp);
					tempBtn.transform.parent=gridTitle.transform;
					tempBtn.transform.localScale=Vector3.one;
					tempBtn.transform.localPosition=Vector3.zero;
					tempBtn.myCkb.group=8;
					tempBtn.yr=item;
					tempBtn.title.text=item["HelpName"].YuanColumnText;
					tempBtn.info=this.lblInfo;
					listTitle.Add (tempBtn);
					btn=tempBtn;
				}
				if(num==0)
				{
					btn.OnClick ();
				}
					num++;
			}
			gridTitle.repositionNow=true;
		}
		
	}
	
}
