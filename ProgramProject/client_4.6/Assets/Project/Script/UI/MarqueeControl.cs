using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarqueeControl : MonoBehaviour,ProcessResponse {
		
	public GameObject marqueeGroup;
	public UILabel marqueeText;
	float mTextLocalScale;
	
	AnnounceResultJson curAnnounceRJ;
	List<Announce> tempAnnounces;
	List<Announce> curAnnounces;
	List<Announce> firstAnnList;
	List<Announce> secondAnnList;
	
	public List<Announce>actAnnounces;
	
	bool isTextMoving;
	
	bool isFirstLvMoving;
	bool isSecondMoving;
	
	int curMsgNum;
	float curFloatingSpeed;
	
	//1请求跑马灯内容//
	int requestType;
	int errorCode;
	bool receiveData;
	
	void Awake()
	{
		marqueeGroup.SetActive(false);
		
		curAnnounces = new List<Announce>();
		
		firstAnnList = new List<Announce>();
		secondAnnList = new List<Announce>();
		
		actAnnounces = new List<Announce>();
	}
	
	// Use this for initialization
	void Start () {
		
		mTextLocalScale = marqueeText.transform.localScale.x;
		
		InvokeRepeating("RequestAnnounceInfo", 0, 60.0f);
//		RequestAnnounceInfo();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
				if(errorCode == 0)
				{
					if(curAnnounces!=null && curAnnounces.Count!= 0)
					{
						marqueeGroup.SetActive(true);
						RefreshList();
						ShowMarqueeLogic();
					}
				}
				break;
			}
		}
	
	}
	
	void RefreshList()
	{
		for(int i=0;i<curAnnounces.Count;i++)
		{
			Announce tAnn = curAnnounces[i];
			if(tAnn.type == 1)
			{
				firstAnnList.Add(tAnn);
			}
			else if(tAnn.type == 2)
			{
				secondAnnList.Add(tAnn);
			}
			else
			{
				Debug.Log("Unknow type!");
			}
		}
	}
	
	void RequestAnnounceInfo()
	{
		requestType = 1;
		PlayerInfo.getInstance().sendRequest(new AnnounceJson(),this);
	}
	
	void ShowMarqueeText(string mText,int type)
	{
		if(type == 1)
		{
			isFirstLvMoving = true;
			Debug.Log("The first lv is moving!");
		}
		else if(type == 2)
		{
			isSecondMoving = true;
			Debug.Log("The second lv is moving!");
		}
		marqueeText.text = mText;
		int mTextLength = NGUIText.StripSymbols(mText).Length;
		Debug.Log("mTextLength:"+mTextLength);
		marqueeText.width = mTextLength * 25;
		marqueeText.transform.localPosition = new Vector3(580,-6,0);
		Vector3 tarLocalPos = new Vector3(-marqueeText.width*mTextLocalScale,-6,0);
		if(curMsgNum>20)
		{
			curFloatingSpeed = 0.006f;
		}
		else if(curMsgNum>10)
		{
			curFloatingSpeed = 0.01f;
		}
		else
		{
			curFloatingSpeed = 0.02f;
		}
		iTween.MoveTo(marqueeText.gameObject,iTween.Hash("position",tarLocalPos,"islocal",true,"time",(float)curFloatingSpeed*marqueeText.width,"easetype",iTween.EaseType.linear,"oncomplete","OnShowThisComplete","oncompleteparams",type,"oncompletetarget",gameObject,"ignoretimescale",true));
	}
	
	void OnShowThisComplete(int curType)
	{	
		if(curType == 1)
		{
			if(firstAnnList.Count>0)
			{
				firstAnnList.RemoveAt(0);
				isFirstLvMoving = false;
			}
		}
		else if(curType == 2)
		{
			if(secondAnnList.Count>0)
			{
				secondAnnList.RemoveAt(0);
				isSecondMoving = false;
			}
		}
		ShowMarqueeLogic();
	}
	
	void ShowMarqueeLogic()
	{
		curMsgNum = firstAnnList.Count+secondAnnList.Count;
		if(isFirstLvMoving || isSecondMoving)
		{
			return;
		}
		if(firstAnnList.Count>0)
		{
			ShowMarqueeText(firstAnnList[0].announce,1);
		}
		else if(secondAnnList.Count>0)
		{
			ShowMarqueeText(secondAnnList[0].announce,2);
		}
		else
		{
			marqueeGroup.SetActive(false);
		}
	}
	
	public void receiveResponse(string json)
	{
		Debug.Log("announce json:"+json);
		if(json!=null)
		{
			switch(requestType)
			{
			case 1:
				AnnounceResultJson arj = JsonMapper.ToObject<AnnounceResultJson>(json);
				errorCode=arj.errorCode;
				if(errorCode==0)
				{
					curAnnounceRJ = arj;
					tempAnnounces = curAnnounceRJ.announces;
					GetCurAnnounces();
					GetActAnnounces();
				}
				receiveData = true;
				break;
			}
		}
	}
	
	void GetCurAnnounces()
	{
		curAnnounces.Clear();
		
		for(int i=0;i<tempAnnounces.Count;i++)
		{
			if(tempAnnounces[i].aType == 1)
			{
				curAnnounces.Add(tempAnnounces[i]);
			}
		}
	}
	
	void GetActAnnounces()
	{
		actAnnounces.Clear();
		
		for(int i=0;i<tempAnnounces.Count;i++)
		{
			if(tempAnnounces[i].aType == 2)
			{
				actAnnounces.Add(tempAnnounces[i]);
			}
		}
	}
}
