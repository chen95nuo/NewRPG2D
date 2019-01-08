using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityTools : MonoBehaviour 
{
    public ActivityTools instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    Dictionary<string, object> dynamicActivity = new Dictionary<string, object>();
    public void GetActivityInfo(Dictionary<string, object> activity)
    {
        dynamicActivity = activity;
    }

    //Dictionary<object, object> tmpData = (Dictionary<object, object>)operationResponse.Parameters[(short)GMParams.DynamicAictivity];
    //Dictionary<string, object> dicData = tmpData.DicObjTo<string, object>();

    //WorkClass.pwuserClass.DynamicActivity = new Dictionary<string, object>();
    //foreach (KeyValuePair<string, object> item in dicData)
    //{
    //    WorkClass.pwuserClass.DynamicActivity.Add(item.Key, item.Value);
    //}

    //private void SetList()
    //{
    //    int num=1;
    //    lsvSelectPlayerInfo.Items.Clear();
    //    foreach (object item in WorkClass.pwuserClass.DynamicActivity.Values)
    //    {
    //        Dictionary<string, object> activity = ((Dictionary<object, object>)item).DicObjTo<string,object>();
    //        ListViewItem lvi = new ListViewItem();
    //        lvi.SubItems[0].Text = num.ToString();
    //        lvi.SubItems.Add((string)activity["label"]);
    //        lvi.SubItems.Add((string)activity["onoff"]);
    //        lvi.SubItems.Add((string)activity["name"]);
    //        lvi.SubItems.Add((string)activity["description"]);

    //        Dictionary<string, string> one = ((Dictionary<object, object>)activity["one"]).DicObjTo<string,string>();
    //        SetItemsReword(lvi, one);

    //        Dictionary<string, string> two = ((Dictionary<object, object>)activity["two"]).DicObjTo<string, string>();
    //        SetItemsReword(lvi, two);

    //        Dictionary<string, string> three = ((Dictionary<object, object>)activity["three"]).DicObjTo<string, string>();
    //        SetItemsReword(lvi, three);

    //        lvi.SubItems.Add((string)activity["startime"]);
    //        lvi.SubItems.Add((string)activity["endtime"]);

    //        lsvSelectPlayerInfo.Items.Add(lvi);
    //        num++;
                
    //    }
    //}

    //private void SetItemsReword(ListViewItem mItem, Dictionary<string, string> dicInfo)
    //{
    //    mItem.SubItems.Add(dicInfo["title"]);
    //    mItem.SubItems.Add(dicInfo["icon"]);
    //    mItem.SubItems.Add(dicInfo["info"]);
    //    mItem.SubItems.Add(dicInfo["reword"]);
    //    mItem.SubItems.Add(dicInfo["blood"]);
    //    mItem.SubItems.Add(dicInfo["gold"]);
    //}


}
