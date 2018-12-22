using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TickerUI : BWUIPanel {
	
	public static TickerUI mInstance;
	
	public GameObject[] nums;
	public GameObject rise;
	
	private float width;
	private	float height;
	private float during;
	private List<TweenPosition> list;
	private int index;
	
	void Awake()
	{
		_MyObj=gameObject;
		mInstance=this;
	}
	
	// Use this for initialization
	void Start () {
		base.init();
		_MyObj.transform.localPosition=new Vector3(-100f,-255,0);
		//==设置位置==//
		int middle=nums.Length/2;
		for(int i=0;i<nums.Length;i++)
		{
			GameObject go1=nums[i].transform.FindChild("1").gameObject;
			//GameObject go2=nums[i].transform.FindChild("2").gameObject;
			if(width==0)
			{
				width=go1.GetComponent<UILabel>().width*go1.transform.localScale.x;
			}
			if(height==0)
			{
				height=go1.GetComponent<UILabel>().height*go1.transform.localScale.y;
			}
			nums[i].transform.localPosition=new Vector3((i-middle)*width,0,0);
			nums[i].GetComponent<TweenPosition>().enabled=false;
		}
		
		for(int i=0;i<rise.transform.childCount;i++)
		{
			rise.transform.FindChild((i+1)+"").localPosition=new Vector3((i-middle-1)*width,0,0);
		}
		hide();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void show(int firstNum,int secondNum,float time)
	{
		if((firstNum+"").Length>7 || (secondNum+"").Length>7)
		{
			return;
		}
			
		base.show();
		//==设置rise==//
		int k=0;
		string result=(secondNum-firstNum)+"";
		if(secondNum-firstNum>0)
		{
			result="+"+result;
		}
		for(int i=0;i<rise.transform.childCount;i++)
		{
			GameObject riseCell=rise.transform.FindChild((i+1)+"").gameObject;
			riseCell.SetActive(true);
			if(i<rise.transform.childCount-result.Length)
			{
				riseCell.SetActive(false);
			}
			else
			{
				riseCell.GetComponent<UILabel>().text=result[k++]+"";
			}
		}
		rise.GetComponent<TweenPosition>().enabled=false;
		rise.SetActive(false);
		if(list==null)
		{
			list=new List<TweenPosition>();
		}
		list.Clear();
		//==显示数字长度==//
		string firstString=firstNum+"";
		string secondString=secondNum+"";
		int maxLength=firstString.Length>secondString.Length?firstString.Length:secondString.Length;
		during=time/maxLength;
		
		int k1=0;
		int k2=0;
		for(int i=0;i<nums.Length;i++)
		{
			GameObject go1=nums[i].transform.FindChild("1").gameObject;
			GameObject go2=nums[i].transform.FindChild("2").gameObject;
			go1.SetActive(true);
			go2.SetActive(true);
			go1.transform.localPosition=new Vector3(0,0,0);
			go2.transform.localPosition=new Vector3(0,height,0);
			
			Vector3 curPos=nums[i].transform.localPosition;
			TweenPosition tw=nums[i].GetComponent<TweenPosition>();
			tw.from=new Vector3(curPos.x,0,curPos.z);
			tw.to=new Vector3(curPos.x,-height,curPos.z);
			tw.duration=during;
			tw.tweenFactor=0;
			
			if(i<nums.Length-firstString.Length)
			{
				go1.SetActive(false);
			}
			else
			{
				go1.GetComponent<UILabel>().text=firstString[k1++]+"";
			}
			if(i<nums.Length-secondString.Length)
			{
				go2.SetActive(false);
			}
			else
			{
				go2.GetComponent<UILabel>().text=secondString[k2++]+"";
			}
			if(go1.activeSelf || go2.activeSelf)
			{
				list.Add(tw);
			}
		}
		index=0;
		list[index].enabled=true;
	}
	
	public override void hide()
	{
		base.hide();
	}
	
	public void tweenOver()
	{
		index++;
		if(index<list.Count)
		{
			list[index].enabled=true;
		}
		else
		{
			rise.SetActive(true);
			Vector3 oldPos=rise.transform.localPosition;
			rise.transform.localPosition=new Vector3(oldPos.x,0,oldPos.z);
			TweenPosition tw=rise.GetComponent<TweenPosition>();
			tw.from=new Vector3(oldPos.x,0,oldPos.z);
			tw.to=new Vector3(oldPos.x,height,oldPos.z);
			tw.duration=1f;
			tw.tweenFactor=0;
			tw.enabled=true;
		}
	}
	
	public void riseOver()
	{
		hide();
	}
	
	public void onClickBtn()
	{
		show(654321,12345,1f);
	}
	
}
