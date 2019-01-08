using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PanelListTabel : MonoBehaviour
{

    public delegate void DelList(yuan.YuanMemoryDB.YuanTable mYt);
    protected DelList eventList;

    public delegate void DelListDic(Dictionary<short, object> mParms);
	protected DelListDic eventListDic;

    public delegate void DelListBack(yuan.YuanMemoryDB.YuanTable mYt, UIGrid mGrid, List<BtnArena> mListBtns, string btnFunction, BtnGameManager.DegSetBtn degSetBtn, string mName);
	protected DelListBack eventListBack;
	protected UIGrid myGrid;
	protected List<BtnArena> myListBtns;
	protected string myBtnFunction;
	protected BtnGameManager.DegSetBtn myDegSetBtn;
	protected string myName;

	protected yuan.YuanMemoryDB.YuanTable yt;
	protected Dictionary<short, object> dic;
    protected int nowPage = 0;
	protected int maxPage = 0;



	public int NowPage
	{
		get
		{
			return nowPage+1;
		}
	}

	public int MaxPage
	{
		get
		{
			return maxPage;
		}
	}

    public int listNum;
    public BtnEvent pageUp;
    public BtnEvent pageDown;
    public UILabel lblNowPage;

    protected virtual void Start()
    {
        if (pageUp != null)
        {
            pageUp.SetEvent(this.PageUpClick);
            pageUp.myButton.isEnabled = false;
        }
        if (pageDown != null)
        {
            pageDown.SetEvent(this.PageDownClick);
            pageDown.myButton.isEnabled = false;
        }
        if (lblNowPage != null)
        {
            lblNowPage.text = nowPage + "/" + maxPage;
        }
    }

    public void SetFrist(yuan.YuanMemoryDB.YuanTable mYt, DelList mEvent, int mNum)
    {
        this.yt = mYt;
        this.eventList = mEvent;
        this.listNum = mNum;
        this.maxPage = yt.Count / listNum;
        if (yt.Count % listNum > 0)
        {
            maxPage++;
        }
        SetPage(0);
    }

    public void SetZore()
    {
        lblNowPage.text = "0/0";
        pageUp.myButton.isEnabled = false;
        pageDown.myButton.isEnabled = false;
    }

    public void SetFrist(Dictionary<short, object> parms, DelListDic mEvent, int mNum)
    {
        if (pageUp != null)
        {
            pageUp.SetEvent(this.PageUpDicClick);
            pageUp.myButton.isEnabled = false;
        }
        if (pageDown != null)
        {
            pageDown.SetEvent(this.PageDownDicClick);
            pageDown.myButton.isEnabled = false;
        }
        this.dic = parms;
        this.eventListDic = mEvent;
        this.listNum = mNum;
        this.maxPage = dic.Count / listNum;
        if (parms.Count % listNum > 0)
        {
            maxPage++;
        }
        SetPage(0);
    }

    public void SetFrist(yuan.YuanMemoryDB.YuanTable mYt, DelListBack mEvent, int mNum, UIGrid mGrid, List<BtnArena> mListBtns, string btnFunction, BtnGameManager.DegSetBtn degSetBtn, string mName)
    {

        this.myGrid = mGrid;
        this.myListBtns = mListBtns;
        this.myBtnFunction = btnFunction;
        this.myDegSetBtn = degSetBtn;
        this.myName = mName;

        this.yt = mYt;
        this.eventListBack = mEvent;
        this.listNum = mNum;
        this.maxPage = yt.Count / listNum;
        if (yt.Count % listNum > 0)
        {
            maxPage++;
        }
        SetPage(0);
    }

    protected int pageNumStart;
	protected int pageNumEnd;
	protected yuan.YuanMemoryDB.YuanTable ytPage = new yuan.YuanMemoryDB.YuanTable("", "");

    public void SetPage(int mNum)
    {
        //NGUIDebug.Log(string.Format("---------------------Click:{0},ytCount:{1}", mNum,yt==null?0:yt.Rows.Count));
        if ((eventList != null || eventListBack != null) && mNum >= 0)
        {
            //NGUIDebug.Log("++++++++++++++++11111111111111");
            int tempNum = 0;
            pageNumStart = listNum * mNum;
            pageNumEnd = pageNumStart + listNum - 1;
            //NGUIDebug.Log("++++++++++++++++22222222222222");
            if (yt.Rows.Count > pageNumStart)
            {
                //NGUIDebug.Log(string.Format("----------------------------mNum:{0},ytCount:{1}", mNum, yt.Count));
                ytPage.Clear();
                foreach (yuan.YuanMemoryDB.YuanRow yr in yt.Rows)
                {
                    if (tempNum >= pageNumStart && tempNum <= pageNumEnd)
                    {
                        ytPage.Add(yr);
                    }
                    if (tempNum >= pageNumEnd)
                    {
                        break;
                    }
                    tempNum++;
                }
                //NGUIDebug.Log("11111111111111111111111");
                if (eventList != null)
                {
                    //NGUIDebug.Log(string.Format("2222222222222222222222222-------:{0}", eventList.Target == null ? eventList.Method.Name : eventList.Target.ToString()));
                    eventList(ytPage);
                }
                if (eventListBack != null)
                {
                    //NGUIDebug.Log(string.Format("3333333333333333333333333-------:{0}", eventListBack.Target == null ? eventListBack.Method.Name : eventListBack.Target.ToString()));
                    eventListBack(ytPage, myGrid, myListBtns, myBtnFunction, myDegSetBtn, myName);
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
                if (yt.Rows.Count - 1 <= pageNumEnd)
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
    }

    private Dictionary<short, object> dicPage = new Dictionary<short, object>();
    public void SetPageDic(int mNum)
    {
		if ((eventListDic != null || eventListBack != null) && mNum >= 0)
        {
            int tempNum = 0;
            pageNumStart = listNum * mNum;
            pageNumEnd = pageNumStart + listNum - 1;

            if (dic.Count > pageNumStart)
            {
                dicPage.Clear();
                foreach (KeyValuePair<short, object> yr in dic)
                {
                    if (tempNum >= pageNumStart && tempNum <= pageNumEnd)
                    {
                        dicPage.Add(yr.Key, yr.Value);
                    }
                    if (tempNum >= pageNumEnd)
                    {
                        break;
                    }
                    tempNum++;
                }

				if (eventListDic != null)
                {
                    eventListDic(dicPage);
                }
                //                if (eventListBack != null)
                //                {
                //                    eventListBack(dicPage, myGrid, myListBtns, myBtnFunction, myDegSetBtn, myName);
                //                }

                nowPage = mNum;
                if (nowPage == 0)
                {
                    pageUp.myButton.isEnabled = false;
                }
                else
                {
                    pageUp.myButton.isEnabled = true;
                }
                if (dic.Count - 1 <= pageNumEnd)
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
    }



	public void SortGuild()
	{
		int i, j;  // 循环变量
		yuan.YuanMemoryDB.YuanRow temp;  // 临时变量
		for (i = 0; i < yt.Rows.Count - 1; i++)
		{
			for (j = 0; j < yt.Rows.Count - 1 - i; j++)
			{
					if (yt.Rows[j]["GuildLevel"].YuanColumnText.Parse (0)< yt.Rows[j + 1]["GuildLevel"].YuanColumnText.Parse(0))
					{
						// 交换元素
						temp = yt.Rows[j];
						yt.Rows[j] = yt.Rows[j + 1];
						yt.Rows[j + 1] = temp;
					}
			}
			
		}
		SetPage(0);
	}  
	
	public void Sort(SortType mSortType, string mSortRowName, string mKeyName)
	{
		if (mSortType == SortType.desc)
		{
			Dictionary<string, int> dic = new Dictionary<string, int>();
			
			for (int i = 0; i < yt.Rows.Count; i++)
			{
				dic.Add(yt.Rows[i][mKeyName].YuanColumnText, int.Parse(yt.Rows[i][mSortRowName].YuanColumnText));
            }

            var result = from pair in dic orderby pair.Value descending select pair;

            yuan.YuanMemoryDB.YuanTable tempTable = new yuan.YuanMemoryDB.YuanTable("", "");
            tempTable.Rows = yt.Rows;
            yt.Rows = new List<yuan.YuanMemoryDB.YuanRow>();
            foreach (KeyValuePair<string, int> r in result)
            {
                yt.Add(tempTable.SelectRowEqual(mKeyName, r.Key));
            }
        }
        else if (mSortType == SortType.asc)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();

            for (int i = 0; i < yt.Rows.Count; i++)
            {
                dic.Add(yt.Rows[i][mKeyName].YuanColumnText, int.Parse(yt.Rows[i][mSortRowName].YuanColumnText));
            }

            var result = from pair in dic orderby pair.Value ascending select pair;

            yuan.YuanMemoryDB.YuanTable tempTable = new yuan.YuanMemoryDB.YuanTable("", "");
            tempTable.Rows = yt.Rows;
            yt.Rows = new List<yuan.YuanMemoryDB.YuanRow>();
            foreach (KeyValuePair<string, int> r in result)
            {
                yt.Add(tempTable.SelectRowEqual(mKeyName, r.Key));
            }
        }

        SetPage(0);

    }

    public void PageUpClick(object sender, object pram)
    {
        SetPage(nowPage - 1);
    }

    public void PageDownClick(object sender, object pram)
    {
        SetPage(nowPage + 1);
    }

    public void PageUpDicClick(object sender, object pram)
    {
        SetPageDic(nowPage - 1);
    }

    public void PageDownDicClick(object sender, object pram)
    {
        SetPageDic(nowPage + 1);
    }



}
