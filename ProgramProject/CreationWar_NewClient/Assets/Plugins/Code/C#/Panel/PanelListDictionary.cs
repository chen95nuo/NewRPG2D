using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelListDictionary : MonoBehaviour {

    public delegate void DelList(Dictionary<byte,object> mYt);
    private DelList eventList;
    private Dictionary<byte, object> dic;
    private int nowPage = 0;
    private int maxPage = 0;
    public int listNum;
    public BtnEvent pageUp;
    public BtnEvent pageDown;
    public UILabel lblNowPage;

    void Start()
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

    public void SetFrist(Dictionary<byte, object> mDic, DelList mEvent, int mNum)
    {
        this.dic = mDic;
        this.eventList = mEvent;
        this.listNum = mNum;
        this.maxPage = dic.Count / listNum;
        if (dic.Count % listNum > 0)
        {
            maxPage++;
        }
        SetPage(0);
    }



    private int pageNumStart;
    private int pageNumEnd;
    private Dictionary<byte, object> dicPage = new Dictionary<byte, object>();
    public void SetPage(int mNum)
    {
        if (eventList != null && mNum >= 0)
        {
            int tempNum = 0;
            pageNumStart = listNum * mNum;
            pageNumEnd = pageNumStart + listNum - 1;

            if (dic.Count > pageNumStart)
            {
                dicPage.Clear();
                foreach (KeyValuePair<byte,object> item in dic)
                {
                    if (tempNum >= pageNumStart && tempNum <= pageNumEnd)
                    {
                        dicPage.Add(item.Key,item.Value);
                    }
                    if (tempNum >= pageNumEnd)
                    {
                        break;
                    }
                    tempNum++;
                }

                eventList(dicPage);
                nowPage = mNum;
                if (nowPage == 0)
                {
                    pageUp.myButton.isEnabled = false;
                }
                else
                {
                    pageUp.myButton.isEnabled = true;
                }
                Debug.Log(string.Format("++++++++++++++++{0},{1},{2}", dic.Count, pageNumEnd, dicPage.Count));
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

    public void PageUpClick(object sender, object pram)
    {
        SetPage(nowPage - 1);
    }

    public void PageDownClick(object sender, object pram)
    {
        SetPage(nowPage + 1);
    }
}
