using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//**************************************************************
//Author FernYuan
//Copyright © 2010-2014 Zealm
//Copyright © 2010-2014 FernYuan
//**************************************************************

public class ListTabelGeneric<T,J>  : PanelListTabel  where T:ICollection<J>,new ()
{

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}
	
	public delegate void DelListGeneric(T mParms);
	private DelListGeneric eventListGeneric ;
	private T generic;

	public System.Action<int> eventPageChange;


	public void SetFrist(T parms, DelListGeneric mEvent, int mNum)
	{
		if (pageUp != null)
		{
			pageUp.SetEvent(this.PageUpGenericClick);
			pageUp.myButton.isEnabled = false;
		}
		if (pageDown != null)
		{
			pageDown.SetEvent(this.PageDownGenericClick);
			pageDown.myButton.isEnabled = false;
		}
		this.generic = parms;
		this.eventListGeneric = mEvent;
		this.listNum = mNum;
		this.maxPage = generic.Count / listNum;
		if (parms.Count % listNum > 0)
		{
			maxPage++;
		}
		SetPageGeneric(0);
	}


	private T genericPage = new T();
	public void SetPageGeneric(int mNum)
	{
		if ((eventListGeneric != null || eventListBack != null) && mNum >= 0)
		{
			//Debug.Log (string.Format ("-----------------MaxPage:{0},mNum:{1}",maxPage,mNum));
			//mNum=maxPage-mNum;
			int tempNum = 0;
			pageNumStart = listNum * mNum;
			pageNumEnd = pageNumStart + listNum - 1;
			
			if (generic.Count > pageNumStart)
			{
				genericPage.Clear();
			
				J[] items=new J[generic.Count];

				tempNum=items.Length-1;
				foreach(J yr in generic)
				{
					items[tempNum]=yr;
					tempNum--;
				}

				tempNum=0;
				foreach (J yr in items)
				{
					if (tempNum >= pageNumStart && tempNum <= pageNumEnd)
					{
						genericPage.Add(yr);
					}
					if (tempNum >= pageNumEnd)
					{
						break;
					}
					tempNum++;
				}

				
				if (eventListGeneric != null)
				{
					eventListGeneric(genericPage);
				}
				
				nowPage = mNum;
				if (nowPage == 0)
				{
					pageUp.myButton.isEnabled = false;
				}
				else
				{
					pageUp.myButton.isEnabled = true;
				}
				if (generic.Count - 1 <= pageNumEnd)
				{
					pageDown.myButton.isEnabled = false;
				}
				else
				{
					pageDown.myButton.isEnabled = true;
				}
				if (lblNowPage != null)
				{
					lblNowPage.text = (nowPage + 1) + "/" + maxPage;
				}


			}
		}

		if(eventPageChange!=null)
		{
			eventPageChange(NowPage);
		}
	}
	
	public void PageUpGenericClick(object sender, object pram)
	{
		SetPageGeneric(nowPage - 1);
	}
	
	public void PageDownGenericClick(object sender, object pram)
	{
		SetPageGeneric(nowPage + 1);
	}

}
