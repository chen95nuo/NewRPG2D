using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextClass
{
    public TextClass()
    {
        text = string.Empty;
        color = Color.white;
        playerID = string.Empty;
        playerName = string.Empty;
    }

    public TextClass(string mTxt, Color mColor,string mPlayerID,string mPlayerName)
    {
        this.text = mTxt;
        this.color = mColor;
        this.playerID = mPlayerID;
        this.playerName = mPlayerName;
    }

    public string text;
    public Color color;
    public string playerID;
    public string playerName;
}

public class CharBar : MonoBehaviour {
	
	public YuanInput getInput;
	public UIScrollBar scrollBar;
	public float maxWidth = 350;
	public float Kerning =1;
	public int size=30;
    public Color fontColor;
    public GameObject invMaker;
	public LableLink objLableLink;
	public bool isOnEnable=true;
	public Font font;
	public UIPanel infoBar;
	public int numChat=28;
	
	private List<LableLink> listDisLink=new List<LableLink>();
	
	private ObjectGamepool poolExps;
	private YuanInput[] listTempInput;
	private yuan.YuanNode<YuanInput> nodeInput = new yuan.YuanNode<YuanInput>();
	private yuan.YuanNode<YuanInput> nodeDisInput = new yuan.YuanNode<YuanInput>();
	
	public List<YuanInput> listInput = new List<YuanInput>();
    public SendManager sendManager;
	public BtnSend btnSend;
    public enum CharBarType
    {
        MainAll,
        All,
        Guild,
        Team,
        SomeBody,
        System,
    }

    public CharBarType charBarType;
	
	void Awake()
	{

		sendManager=PanelStatic.StaticSendManager;
		//InsInput ();
		this.gameObject.SetActiveRecursively (true);
		switch (charBarType)
		{

		case CharBarType.All:
		{
			sendManager.eventManagerAll += AddTextPlayer;
		}
			break;
		case CharBarType.Guild:
		{
			sendManager.eventManagerGuild += AddTextPlayer;
		}
			break;
		case CharBarType.Team:
		{
			sendManager.eventManagerTeam += AddTextPlayer;
		}
			break;
		case CharBarType.SomeBody:
		{
			sendManager.eventManagerSomeBody += AddTextPlayer;
		}
			break;
		case CharBarType.System:
		{
			sendManager.eventManagerSystem += AddTextPlayer;
		}
			break;
		}
	}

	public ListTableCharBar listTabel;
	public GameObject objHasNewMessage;



	// Use this for initialization
	void Start () {
		InsInput ();

		if(listTabel!=null)
		{
			listTabel.eventPageChange+=this.OnTableListPageChahge;
			listTabel.SetFrist (listText,RefrshText,10);
//			for(int i=0;i<9;i++)
//			{
//				TextSequence(listInput);
//			}
		}


//		getInput = YuanInput.yuanInput;
		sendManager=PanelStatic.StaticSendManager;

		if(charBarType==CharBarType.MainAll)
		{
			sendManager.eventManagerMainAll += AddTextPlayer;
		}
		this.gameObject.SetActiveRecursively (false);


	}

	void OnDisable()
	{
		if(listTabel!=null)
		{
			listTabel.eventPageChange-=this.OnTableListPageChahge;
		}
	}



    void OnEnable()
    {
		RefrshInputDisPic ();
		if(isOnEnable)
		{
        sendManager.SetBarSwicth();
		}
    }

	public void RefrshInputDisPic()
	{
		if(listTempInput!=null)
		{
			foreach(YuanInput input in listTempInput)
			{
				input.RefrshDisPic ();
			}
		}
	}
	
	void Update()
	{
		GetText ();
		HasNew ();
	}

	private int lastPage=0;
	private float hasNewTime=0;
	void HasNew()
	{
		if(Time.time-hasNewTime>1)
		{
			hasNewTime=Time.time;
			if(listTabel!=null&&objHasNewMessage!=null)
			{
				if(lastPage!=1&&listTabel.NowPage==1)
				{
					objHasNewMessage.SetActive (false);
				}
				if(listTabel.NowPage==1&&objHasNewMessage.activeSelf)
				{
					objHasNewMessage.SetActive (false);
				}
				lastPage=listTabel.NowPage;
			}
		}
	}

    void GetText()
    {
		if(listTabel==null)
		{
	        if (listText.Count > 0&&this.gameObject.active==true)
	        {

	            Add(listText[0].text, listText[0].color, listText[0].playerID, listText[0].playerName,false);
	            listText.RemoveAt(0);
	        }
		}
    }



	
	private void InsInput()
	{
		listTempInput=GetComponentsInChildren<YuanInput>();
		listInput.AddRange (listTempInput);
		if(poolExps==null){
		poolExps = getInput.GetComponent<ObjectGamepool>();
		}
		foreach(YuanInput input in listInput)
		{
			input.multiLine=true;
			input.maxWidth=this.maxWidth;
			input.Kerning=this.Kerning;
			input.size=this.size;
			input.caratChat="";
			input.font=this.font;
            input.invMaker = this.invMaker;
			input.itemInfoBar=getInput.itemInfoBar;
			input.objLableLink=this.objLableLink;
            input.btnSend = getInput.btnSend;
			input.listDisLink=this.listDisLink;
			input.GetComponent<ObjectGamepool>().EffectP=poolExps.EffectP;
			input.btnSend=this.btnSend;
			input.itemInfoBar=this.infoBar;
			nodeInput.InNode (input);
		}
	}
	
	private YuanInput topInput;
	public void AddText(string text)
	{
		if(text!="")
		{
            //listText.Add(new TextClass(text, Color.white,"",""));
			AddList(new TextClass(text, Color.white,"",""));
		}
	}

    public void AddText(object[] parm)
    {
        string text = (string)parm[0];
        Color mColor = (Color)parm[1];
        if (text != "")
        {
            //listText.Add(new TextClass(text, mColor,"",""));
			AddList(new TextClass(text, mColor,"",""));
        }
    }

    public void AddTextPlayer(object[] parm)
    {
        string text = (string)parm[0];
        Color mColor = (Color)parm[1];
        string playerID = (string)parm[2];
        string playerName = (string)parm[3];
        if (text != "")
        {
            //listText.Add(new TextClass(text, mColor, playerID,playerName));
			AddList(new TextClass(text, mColor, playerID,playerName));
        }
    }

	private void AddList(TextClass text)
	{
		listText.Add (text);
		if(listText.Count>50)
		{
			listText.RemoveAt (0);
		}

		if(listTabel!=null)
		{
			if(listTabel.NowPage==1||listTabel.NowPage==0)
			{
				//listTabel.SetPageGeneric (0);
				listTabel.SetFrist (listText,RefrshText,10);
			}
			else
			{
				if(objHasNewMessage!=null)
				{
					objHasNewMessage.SetActive (true);
				}
			}
		}
	}

	void RefrshText(List<TextClass> mListText)
	{
		foreach(YuanInput yi in listInput)
		{
			yi.Text="";
		}


		mListText.Reverse ();
		for(int i=0;i<mListText.Count;i++)
		{
			Add(mListText[i].text, mListText[i].color, mListText[i].playerID, mListText[i].playerName,false);
		}
	}

	void OnTableListPageChahge(int nowPage)
	{
		if(nowPage==0&&objHasNewMessage!=null)
		{
			objHasNewMessage.SetActive (false);
		}
	}

    private List<TextClass> listText = new List<TextClass>();

	int numColor=11*2;
    private void Add(string mTxt,Color mColor,string mPlayerID,string mPlayerName,bool isSelf)
    {
		numColor=11*2;
		if (listInput.Count == 0 && listDisLink.Count == 0)
		{
			return;	
		}

		string nextString=string.Empty;

		int numPlayerNum=0;
		if(isSelf)
		{
			numPlayerNum=0;
		}
		else
		{

			numPlayerNum=mPlayerName.Length+1+4+1+numColor;
		}



		//排除装备，道具和图纸
		if(
			mTxt.Length>0&&mTxt.Length>numPlayerNum&&mTxt.Substring (numPlayerNum,1)=="["&&
		   	(

				mTxt.Length==(9+2+numPlayerNum)||
				mTxt.Length==(25+2+numPlayerNum)||
				mTxt.Length==(40+2+numPlayerNum)
			)
		   )
		{
			
		}
		else
		{
			if(!isSelf)
			{


				if(string.IsNullOrEmpty(mPlayerName))
				{
					numColor=0;
				}
				if(mTxt.Length>numChat+numColor)
				{
					string tempText=mTxt.Substring (0,numChat+numColor);
					int num=0;
					if(tempText[tempText.Length-1]=='[')
					{
						num=4-1;
					}
					else if(tempText[tempText.Length-2]=='[')
					{
						num=4-2;
					}
					else if(tempText[tempText.Length-3]=='[')
					{
						num=4-3;
					}
					nextString=mTxt.Substring (numChat+num+numColor);
					mTxt=mTxt.Substring (0,numChat+num+numColor);
				}
			}
			else
			{
				if(mTxt.Length>numChat)
				{
					string tempText=mTxt.Substring (0,numChat);
					int num=0;
					if(tempText[tempText.Length-1]=='[')
					{
						num=4-1;
					}
					else if(tempText[tempText.Length-2]=='[')
					{
						num=4-2;
					}
					else if(tempText[tempText.Length-3]=='[')
					{
						num=4-3;
					}
					nextString=mTxt.Substring (numChat+num);
					mTxt=mTxt.Substring (0,numChat+num);
				}
			}

		}
		

			//		Debug.Log (string.Format ("+++++++++++++{0},{1},{2}",mPlayerID,mPlayerName,mTxt));
	        if (nodeInput.Count > 0)
	        {
				
	            nodeDisInput.InNode(nodeInput.NodeTop());
				nodeInput.NodeTop().fontColor = mColor;
				nodeInput.NodeTop().playerID = mPlayerID;
				nodeInput.NodeTop().playerName=mPlayerName;
	            nodeInput.NodeTop().Text = mTxt;
	//			Debug.Log (string.Format ("&&&&&&&&&&-------{0},{1},{2}",nodeInput.NodeTop().playerID,nodeInput.NodeTop().playerName,nodeInput.NodeTop().Text));
	            
	            nodeInput.NodeTop().refresh = true;
	            nodeInput.OutNode();
	        }
	        else
	        {
				 topInput = nodeDisInput.NodeTop();
				 topInput.fontColor = mColor;
				
	            topInput.playerID = mPlayerID;
				topInput.playerName=mPlayerName;
	            topInput.Text = mTxt;
	           
	            topInput.refresh = true;
	            nodeDisInput.OutNode();
	            nodeInput.InNode(topInput);
	            nodeDisInput.InNode(nodeInput.NodeTop());
	            nodeInput.OutNode();
	        }

	        TextSequence(nodeDisInput.ToList());
	

		if(!string.IsNullOrEmpty(nextString))
		{
			this.Add (nextString,mColor,mPlayerID,mPlayerName,true);
			//SendMessage (nextString,"","",messageType);
		}
		//scrollBar.scrollValue = 1;
    }
	
    //private List<UILabel> textLable;
    //private float hight;
	private float hightFirst;
	public int listMaxNum=0;
	private void TextSequence(List<YuanInput> list)
	{

		if(list.Count>0)
		{
             List<UILabel> textLable;
             float hight;
			for(int i=0;i<list.Count;i++)
			{
				//textLable=list[list.Count-i-1].listLable;
				//if(i==0)
				//{
				//	list[list.Count-i-1].transform.localPosition=Vector3.zero;
				//	
				//}
				//else if(textLable.Count>0)
				//{
				//	hight = textLable[0].transform.position.y- textLable[textLable.Count-1].transform.position.y;
				//	list[list.Count-i-1].transform.position=new Vector3(transform.position.x,list[list.Count-i].transform.position.y+0.004f*size+hight,transform.position.z);
				//}
				
				if(i==0)
				{
					list[list.Count-i-1].transform.localPosition=Vector3.zero;
					
				}
				else
				{
					list[list.Count-i-1].transform.localPosition=new Vector3(list[list.Count-i-1].transform.localPosition.x,list[list.Count-i].transform.localPosition.y+size,list[list.Count-i-1].transform.localPosition.z);
				}
				if(listMaxNum>0&&list.Count>listMaxNum&&i>=listMaxNum&&list[list.Count-i-1].listUseLink.Count>0)
				{
					list[list.Count-i-1].listUseLink[0].clickCollider.size=Vector3.zero;
					list[list.Count-i-1].playerID="";
				}
			}
		}
	}
}