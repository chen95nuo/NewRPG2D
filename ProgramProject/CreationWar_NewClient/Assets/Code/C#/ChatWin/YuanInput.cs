using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//[ExecuteInEditMode]
[AddComponentMenu("YuanClass/YuanGUI/YuanInput")]
public class YuanInput : MonoBehaviour {
    public bool isInput = false;
	public Font font;
	public UILabel.Effect lblEffect=UILabel.Effect.Shadow;
	public UILabel.Overflow lblflow = UILabel.Overflow.ResizeFreely;
	public bool multiLine=false;
	public float maxWidth=10;
	public float Kerning=1;
	public int size=16;
	public string caratChat="|";
    private string text = string.Empty;
	public int chatlength = 25;
	
	
    public string Text
    {
        get { return text; }
        set 
        {
			
			if(!isInput)
			{
//			Debug.Log (string.Format ("#############----{0},{1},{2}",playerID,playerName,this.transform.parent.name));
			}
            text = value;
			if(isInput)
			{
            	textUpdate();
			}
			else
			{
				TextGetUpdate();
			}
        }
    }
	
	public List<char> tempText=new List<char>();
	public List<char> tempTextTemp;
	public List<char> listDisTempText = new List<char>();
	public Transform background;
	public Transform expPoint;
	public UIPanel itemInfoBar;
    public GameObject invMaker;
	private ObjectGamepool poolExpression;
	private List<UISprite> listExpression = new List<UISprite>();



#if UNITY_IPHONE || UNITY_ANDROID
#if UNITY_3_4
	public iPhoneKeyboard keyboard;
#else
	public TouchScreenKeyboard keyboard;
#endif
#else
    string mLastIME = "";
#endif

	private yuan.YuanNode<UILabel> nodeDisLable = new yuan.YuanNode<UILabel>();
	private Dictionary<GameObject,int> listDisExpression =new Dictionary<GameObject, int>();
	private BoxCollider boxCollider;
	
	
	[HideInInspector]
    public bool isSelect = false;

    public bool IsSelect
    {
        get { return isSelect; }
        set { 
            isSelect = value;
            FlashCaratChat(isSelect);
        }
    }
	
	[HideInInspector]
	public bool refresh=false;
	[HideInInspector]
	public float getSize=0;
	[HideInInspector]
	public float mSize=0;
	[HideInInspector]
	public int lines=0;
	[HideInInspector]
	public UISprite exps;
	[HideInInspector]
	public List<UILabel> listLable = new List<UILabel>();	
	
	private int tempLines=0;
	private int linesPlus=0;

    private BoxCollider myCollider;

	void Awake()
	{
		poolExpression = GetComponent<ObjectGamepool>();
		foreach(EffectSP sp in poolExpression.EffectP)
		{
			listExpression.Add (sp.effectprefab.GetComponent<UISprite>());
            
		}	

	}
	
	public void GetExpression()
	{
		if(listExpression.Count==0)
		{
		poolExpression = GetComponent<ObjectGamepool>();
		foreach(EffectSP sp in poolExpression.EffectP)
		{
			listExpression.Add (sp.effectprefab.GetComponent<UISprite>());
		}
		}
	}
	
	void Start()
	{

		
        if(listExpression.Count==0)
		{
			foreach(EffectSP sp in poolExpression.EffectP)
			{
				listExpression.Add (sp.effectprefab.GetComponent<UISprite>());
				
			}
		}
		
		//InsTextLable ();;

		
		if(!isInput)
		{
			
	        myCollider = this.gameObject.AddComponent<BoxCollider>();
	        myCollider.isTrigger = true;
//	        myCollider.size = new Vector3(myCollider.size.x * this.size * 2.5f, myCollider.size.y * this.size, myCollider.size.z);
//	        myCollider.center = new Vector3(myCollider.size.x/2, myCollider.center.y, myCollider.center.z);
			myCollider.size=new Vector3(150,16,1);
			myCollider.center=new Vector3(75,0,0);

		}
		else
		{
			InsCaratChat ();
		}
			
	}
	
//	void FixUpdate()
//	{
//				textUpdate ();
//	}
	
	void Update()
	{
		
		if(isInput&&isSelect)
		{
			if(Application.platform==RuntimePlatform.Android||Application.platform==RuntimePlatform.IPhonePlayer)
			{
				InputChatPhone();
			}
			else
			{
				InputChatPC();
			}
			
		}
	}
	



    //[HideInInspector]
    public string playerID=string.Empty;
    //[HideInInspector]
    public string playerName=string.Empty;
    public BtnSend btnSend;
    private string[] strPlayer = new string[2];
    void OnClick()
    {
//		Debug.Log (string.Format ("---------------------------inputClick,{0},{1},{2},{3},{4}",isInput,playerID,playerName,this.transform.parent.name,this.text));
        if (isInput)
        {
//			Debug.Log ("------------------------OnClickInput");
            isSelect = true;
#if UNITY_IPHONE||UNITY_ANDROID
		keyboard=TouchScreenKeyboard.Open (Text,TouchScreenKeyboardType.Default);
#endif
           
        }
        else
        {
//			Debug.Log (string.Format ("{0},{1}",playerID,BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText));
            if ( playerID != ""&&playerID!=BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText)
            {
                strPlayer[0] = playerID;
                strPlayer[1] = playerName;
//				Debug.Log ("YYYYYYYYYYYYYYYYYYYYYYYY");
				if(btnSend!=null){
              	//btnSend.ShowOne(this,strPlayer);
					PanelStatic.StaticBtnGameManager.RunShowOne((object)strPlayer);
            }
			}
        }

    }
	
	void OnPress(bool isDown)
	{
		if(isDown)
		{
		
		}
	}

	void OnEnable()
	{
		RefrshDisPic ();
	}

	public void RefrshDisPic()
	{
		foreach(LableLink item in listDisLink)
		{
			item.gameObject.SetActiveRecursively (false);
		}
	}
	
  void SetMyLeble(UILabel mLbl)
	{
		this.getLable=mLbl;
	}
	
public List<LableLink> listDisLink;
public List<LableLink> listUseLink=new List<LableLink>();
private List<LableLink> listMyUseLink;
	private UILabel getLable;
	public LableLink objLableLink;
	private StringBuilder strb=new StringBuilder();
	private int widthLable=0;

	private string tryString="";
private void TextGetUpdate()
{
		GetExpression();
		if(getLable==null)
		{
				getLable=yuan.YuanClass.AddChildToComponent<UILabel>(this.transform,"YuanLable");
				getLable.ambigiousFont=this.font;
				getLable.pivot=UIWidget.Pivot.Left;
				getLable.transform.localScale=new Vector3(1,1,1);
				getLable.transform.localPosition=Vector3.zero;
				getLable.effectStyle=this.lblEffect;
				getLable.overflowMethod =this.lblflow;
				getLable.maxLineCount=1;
				
		}
		getLable.color=this.fontColor;
		//StringBuilder strb=new StringBuilder();
		strb.Length=0;
		for(int i=listUseLink.Count;listUseLink.Count>0;i=listUseLink.Count)
		{
			listUseLink[i-1].gameObject.SetActiveRecursively (false);
			listDisLink.Add (listUseLink[i-1]);
			listUseLink.Remove (listUseLink[i-1]);

		}
		for(int i=0;i<Text.Length;i++)
		{
			strb.Append (text[i]);
		//getLable.text=strb.ToString ();
			if(text[i]==']'&&i-3>=0&&text[i-3]=='['&&int.TryParse (text.Substring (i-2,2),out expressionNum))//表情
			{
				try
				{
					//expressionNum=int.Parse (text.Substring (i-2,2));
					if(expressionNum<=listExpression.Count)
					{
						strb.Replace ("["+text.Substring (i-2,2)+"]","");
						getLable.text=strb.ToString ();
						
						if(listDisLink.Count>0)
						{
							listDisLink[0].id="";
							listDisLink[0].gameObject.SetActiveRecursively (true);
							listDisLink[0].myPic.enabled=true;
							listDisLink[0].myPic.spriteName=listExpression[expressionNum-1].spriteName;
							listDisLink[0].myPic.width=size;
							listDisLink[0].myPic.height=size;
							listDisLink[0].transform.localScale=Vector3.one;
							listDisLink[0].transform.parent=this.transform;
							listDisLink[0].transform.localPosition=Vector3.zero;

//							listDisLink[0].transform.localPosition=new Vector3(getLable.relativeSize.x*size+size/2,0,this.transform.localPosition.z);
							widthLable=getLable.width+10;
							listDisLink[0].transform.localPosition=new Vector3(widthLable,0,this.transform.localPosition.z);


							listDisLink[0].clickCollider.isTrigger=false;
							listDisLink[0].clickCollider.size=Vector3.zero;
							listUseLink.Add (listDisLink[0]);
							listDisLink.Remove (listDisLink[0]);
						}
						else
						{
							LableLink tempLink=(LableLink)Instantiate (objLableLink);
							tempLink.transform.parent=this.transform;
							tempLink.transform.localScale=Vector3.one;
							tempLink.transform.localPosition=Vector3.zero;
					
							listUseLink.Add (tempLink);
							//tempLink.transform.localScale=new Vector3(size,size,1);
							tempLink.transform.localScale=Vector3.one;
//							tempLink.transform.localPosition=new Vector3(getLable.relativeSize.x*size+size/2,0,this.transform.localPosition.z);
							widthLable=getLable.width+10;
							tempLink.transform.localPosition=new Vector3(widthLable,0,this.transform.localPosition.z);

								tempLink.id="";
								tempLink.gameObject.SetActiveRecursively (true);
							tempLink.myPic.enabled=true;
							tempLink.clickCollider.isTrigger=false;
							tempLink.clickCollider.size=Vector3.zero;
							tempLink.infoBar=this.itemInfoBar;
								tempLink.myPic.spriteName=listExpression[expressionNum-1].spriteName;
							tempLink.myPic.width=size;
							tempLink.myPic.height=size;
						}
						strb.Append ("      ");
						getLable.text=strb.ToString ();
					}
				}
				catch(System.Exception ex)
				{
					Debug.Log (ex.ToString ());
				}
			}


			if(text[i]==']'&&i-26>=0&&text[i-26]=='[')//武器
			{

				
				try
				{

					itemIDstr=text.Substring (i-25,25);

					if(itemIDstr.IndexOf("[")==-1&&itemIDstr.IndexOf("]")==-1)
					{
						strb.Replace ("["+itemIDstr+"]","  ");
						if(listDisLink.Count>0)
						{
							listDisLink[0].id=itemIDstr;
							listDisLink[0].gameObject.SetActiveRecursively (true);
												listDisLink[0].transform.parent=this.transform;
							listDisLink[0].transform.localPosition=Vector3.zero;
							//listDisLink[0].myPic.enabled=false;
						listDisLink[0].myPic.width=0;
						listDisLink[0].myPic.height=0;
						listDisLink[0].transform.localPosition=new Vector3(250,0,this.transform.localPosition.z);
							listDisLink[0].clickCollider.size=new Vector3(150,17,1);
						
							listDisLink[0].clickCollider.isTrigger=true;
							listUseLink.Add (listDisLink[0]);
							listDisLink.Remove (listDisLink[0]);
						}
						else
						{
							LableLink tempLink=(LableLink)Instantiate (objLableLink);
							tempLink.transform.parent=this.transform;
							tempLink.transform.localScale=Vector3.one;
							tempLink.transform.localPosition=Vector3.zero;
							listUseLink.Add (tempLink);
							tempLink.infoBar=this.itemInfoBar;
								tempLink.id=itemIDstr;
								tempLink.gameObject.SetActiveRecursively (true);
								//tempLink.myPic.enabled=false;
						tempLink.myPic.height=0;
						tempLink.myPic.width=0;
						tempLink.clickCollider.isTrigger=true;
								tempLink.clickCollider.size=new Vector3(150,17,1);
						tempLink.transform.localPosition=new Vector3(250,0,this.transform.localPosition.z);
						}
					
					getLable.text=strb.ToString ();
                    parms[0] = itemIDstr;
                    parms[1] = getLable;
                     PanelStatic.StaticBtnGameManager.InvMake.SendMessage("LabelName", parms, SendMessageOptions.DontRequireReceiver);
					return;
					}
				}
				catch(System.Exception ex)
				{
					Debug.Log (ex.ToString ());
				}
			}

			if (text[i] == ']' && i - 10 >= 0 && text[i - 10] == '['/*&&long.TryParse(text.Substring(i - 9, 9),out tryLong)*/)//道具
            {

                try
                {
                    itemIDstr = text.Substring(i - 9, 9);
					if(itemIDstr.IndexOf("[")==-1&&itemIDstr.IndexOf("]")==-1)
					{
						strb.Replace ("["+itemIDstr+"]","  ");
						if(listDisLink.Count>0)
						{
							listDisLink[0].id=itemIDstr;
							listDisLink[0].gameObject.SetActiveRecursively (true);
							listDisLink[0].transform.parent=this.transform;
							listDisLink[0].transform.localPosition=Vector3.zero;
							//listDisLink[0].myPic.enabled=false;
						listDisLink[0].myPic.width=0;
						listDisLink[0].myPic.height=0;
							listDisLink[0].clickCollider.isTrigger=true;
						listDisLink[0].transform.localPosition=new Vector3(250,0,this.transform.localPosition.z);
							listDisLink[0].clickCollider.size=new Vector3(150,17,1);
							listUseLink.Add (listDisLink[0]);
							listDisLink.Remove (listDisLink[0]);
						}
						else
						{
							LableLink tempLink=(LableLink)Instantiate (objLableLink);
							tempLink.transform.parent=this.transform;
							tempLink.transform.localScale=Vector3.one;
							tempLink.transform.localPosition=Vector3.zero;
							listUseLink.Add (tempLink);
							tempLink.infoBar=this.itemInfoBar;
								tempLink.id=itemIDstr;
						tempLink.clickCollider.isTrigger=true;
								tempLink.gameObject.SetActiveRecursively (true);
								//tempLink.myPic.enabled=false;
						tempLink.myPic.width=0;
						tempLink.myPic.height=0;
								tempLink.clickCollider.size=new Vector3(150,17,1);
						tempLink.transform.localPosition=new Vector3(250,0,this.transform.localPosition.z);
						}
					
					getLable.text=strb.ToString ();
                    parms[0] = itemIDstr;
                    parms[1] = getLable;
//					Debug.Log ("-------------------"+itemIDstr);
                    PanelStatic.StaticBtnGameManager.InvMake.SendMessage("LabelName", parms, SendMessageOptions.DontRequireReceiver);
					return;
					}
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }

			if (text[i] == ']' && i - 41 >= 0 && text[i - 41] == '['/*&&long.TryParse( text.Substring(i - 40, 40),out tryLong)*/)//配方
            {

                try
                {

                    itemIDstr = text.Substring(i - 40, 40);

					if(itemIDstr.IndexOf("[")==-1&&itemIDstr.IndexOf("]")==-1)
					{
						strb.Replace ("["+itemIDstr+"]","  ");
						if(listDisLink.Count>0)
						{
							listDisLink[0].id=itemIDstr;
							listDisLink[0].gameObject.SetActiveRecursively (true);
						listDisLink[0].clickCollider.isTrigger=true;
														listDisLink[0].transform.parent=this.transform;
							listDisLink[0].transform.localPosition=Vector3.zero;
							//listDisLink[0].myPic.enabled=false;
						listDisLink[0].myPic.width=0;
						listDisLink[0].myPic.height=0;
						listDisLink[0].transform.localPosition=new Vector3(114,0,this.transform.localPosition.z);
							listDisLink[0].clickCollider.size=new Vector3(100,17,1);
							listUseLink.Add (listDisLink[0]);
							listDisLink.Remove (listDisLink[0]);
						}
						else
						{
							LableLink tempLink=(LableLink)Instantiate (objLableLink);
							tempLink.transform.parent=this.transform;
							tempLink.transform.localScale=Vector3.one;
							tempLink.transform.localPosition=Vector3.zero;
							listUseLink.Add (tempLink);
							tempLink.infoBar=this.itemInfoBar;
						tempLink.clickCollider.isTrigger=true;
								tempLink.id=itemIDstr;
								tempLink.gameObject.SetActiveRecursively (true);
								//tempLink.myPic.enabled=false;
						tempLink.myPic.width=0;
						tempLink.myPic.height=0;
								tempLink.clickCollider.size=new Vector3(150,17,1);
						tempLink.transform.localPosition=new Vector3(250,0,this.transform.localPosition.z);
						}
					
					getLable.text=strb.ToString ();
                    parms[0] = itemIDstr;
                    parms[1] = getLable;
                    PanelStatic.StaticBtnGameManager.InvMake.SendMessage("LabelName", parms, SendMessageOptions.DontRequireReceiver);
					return;
					}
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
			}
			
            }
		getLable.text=strb.ToString ();
}
	
	private yuan.YuanString yuanStr=new yuan.YuanString();
	private int offest=0;
	private float fontOffest=0;
	private float fontOffestPlus=0;
	private float caratChatOffset=0;
	private GameObject tempPar;
	private UILabel tempClone;
	private UISprite tempExps;
	private UILabel tempCloneTemp;
	private int expressionNum=0;
	private int itemID=0;
	private float lastFontOffest=0;
	private int fontRows=0;
    private string itemIDstr = string.Empty;
    private object[] parms = new object[2];
	private void textUpdate()
	{
		offest=yuanStr.IsUpdate (text);
		if(offest!=0||refresh)
		{
			
			//if(offest>0)
			//{
			//	for (int i=0;i<Mathf.Abs (offest);i++)
			//	{
			//		if(nodeDisLable.Count>0)
			//		{
			//			tempClone=nodeDisLable.NodeTop ();
			//			tempClone.gameObject.active=true;
			//			nodeDisLable.OutNode ();
			//		}
			//		else
			//		{
			//			tempClone=yuan.YuanClass.AddChildToComponent<UILabel>(transform,"YuanLable");
			//		}
			//		tempClone.transform.localScale=new Vector3(size,size,1);
			//		tempClone.transform.localPosition=Vector3.zero;
			//		
			//		if(font)
			//		{
			//			tempClone.font=this.font;
			//		}
			//		tempClone.text=text[text.Length-offest+i].ToString ();
			//		//clone.transform.Translate (clone.transform.right*(text.Length-offest+i)*Kerning*0.005f*transform.localScale.x*size);
			//		if(listLable.Count>0)
			//		{
			//			fontOffest+=listLable[listLable.Count-1].relativeSize.x/2+Kerning*0.05f+tempClone.relativeSize.x/2;
			//		}
			//		else
			//		{
			//			fontOffest=tempClone.relativeSize.x/2;
			//		}
			//		tempClone.transform.localPosition=new Vector3(fontOffest*transform.localScale.x*size,0,0);
			//		listLable.Add (tempClone);
			//	}
			//}
			//if(offest<0)
			//{
			//	for (int i=0;i<Mathf.Abs (offest);i++)
			//	{
			//		
			//		if(listLable.Count>1)
			//		{
			//			fontOffest-=listLable[listLable.Count-1].relativeSize.x/2+Kerning*0.05f+listLable[listLable.Count-2].relativeSize.x/2;
			//			//Debug.Log (listLable[listLable.Count-1].relativeSize.x/2+"++"+Kerning*0.05f+"++"+listLable[listLable.Count-2].relativeSize.x/2);
			//		}
			//		else
			//		{
			//			fontOffest=0;
			//		}
			//		listLable[listLable.Count-1].text="";
			//		//Destroy (listLable[listLable.Count-1].gameObject);
			//		//listLable.RemoveAt (listLable.Count-1);
			//		nodeDisLable.InNode (listLable[listLable.Count-1]);
			//		listLable[listLable.Count-1].gameObject.active=false;
			//		listLable.RemoveAt (listLable.Count-1);
			//		
			//	}			
			//}
			//Debug.Log ("fontOffest:"+fontOffest);
			
			
			
			
			fontOffest=0;
			lines=0;
			if(listLable.Count>0)
			{
				foreach(UILabel lb in listLable)
				{
					lb.text="";
					nodeDisLable.InNode (lb);
					lb.transform.parent.gameObject.SetActiveRecursively (false);
					lb.expsEnable=false;
				}
				listLable.Clear ();
			}
			for (int i=0;i<text.Length;i++)
			{
                if (!isInput)
                {
                    //yield return new WaitForEndOfFrame();
                }
				if(nodeDisLable.Count>0)
				{
					tempClone=nodeDisLable.NodeTop ();
                    tempClone.color = this.fontColor;
					tempClone.transform.parent.gameObject.SetActiveRecursively (true);
					tempClone.exps.gameObject.active=false;
					nodeDisLable.OutNode ();
				}
				else
				{
					tempPar=yuan.YuanClass.AddChild(transform,"LablePa");
                    //yield return new WaitForEndOfFrame();
					tempPar.transform.localScale=new Vector3(1,1,1);
					tempClone=yuan.YuanClass.AddChildToComponent<UILabel>(tempPar.transform,"YuanLable");
                    //yield return new WaitForEndOfFrame();
					tempClone.color=fontColor;
					tempClone.lableLink=tempClone.gameObject.AddComponent<LableLink>();
                   // yield return new WaitForEndOfFrame();
					tempClone.lableLink.infoBar=itemInfoBar;
					
					tempExps=yuan.YuanClass.AddChildToComponent<UISprite>(tempPar.transform,"Exps");
                   // yield return new WaitForEndOfFrame();
					tempExps.transform.localScale=Vector3.one;
					//tempExps.gameObject.AddComponent<LableLink>();
					tempClone.exps=tempExps;
					tempExps.gameObject.active=false;
				}
				
				tempClone.transform.localScale=new Vector3(1,1,1);
				tempClone.fontSize=this.size;
				tempClone.transform.parent.localPosition=Vector3.zero;
				tempClone.lableLink.clickCollider.size=Vector3.zero;
				
				if(font)
				{
					tempClone.ambigiousFont=this.font;
				}
				tempClone.text=text[i].ToString ();
				
				if(text[i]==']'&&i-3>=0&&text[i-3]=='[')//表情
				{
					try
					{
						expressionNum=int.Parse (text.Substring (i-2,2));
						if(expressionNum<=listExpression.Count)
						{
							for(int j=0;j<3;j++)
							{
								//listLable[listLable.Count-1-j].text="";
								listLable[listLable.Count-1].text="";
								listLable[listLable.Count-1].transform.parent.gameObject.SetActiveRecursively (false);
								
								listLable.RemoveAt (listLable.Count-1);
							}
							tempClone.text="";
							tempClone.GetComponent<LableLink>().id="";
							tempClone.exps.gameObject.active=true;
							tempClone.exps.enabled=true;
							tempClone.exps.atlas=listExpression[expressionNum-1].atlas;
							tempClone.exps.spriteName=listExpression[expressionNum-1].spriteName;
							tempClone.exps.height=20;
							tempClone.exps.width=20;
							tempClone.lableLink.clickCollider.size=new Vector3(1,1,1);
							
							//tempClone.exps.atlas=listExpression[expressionNum-1].atlas;
							//tempClone.exps.spriteName=listExpression[expressionNum-1].spriteName;
						}
					}
					catch(System.Exception ex)
					{
						Debug.Log (ex.ToString ());
					}
				}
				
				if(text[i]==']'&&i-26>=0&&text[i-26]=='[')//武器
				{
					
					try
					{
						itemIDstr=text.Substring (i-25,25);
						if(itemIDstr.IndexOf("[")==-1&&itemIDstr.IndexOf("]")==-1)
						{
							for(int k=0;k<26;k++)
							{
	                            listLable[listLable.Count - 1].text = "";
									listLable[listLable.Count-1].transform.parent.gameObject.SetActiveRecursively (false);
									listLable.RemoveAt (listLable.Count-1);							
							}
							//tempClone.text="[ff0000]["+itemName+"][-]";
	                        tempClone.lableLink.id = itemIDstr;
							tempClone.lableLink.clickCollider.size=new Vector3(4,1,1);
	                        parms[0] = itemIDstr;
	                        parms[1] = tempClone;
	                        PanelStatic.StaticBtnGameManager.InvMake.SendMessage("LabelName", parms, SendMessageOptions.DontRequireReceiver);
							//tempClone.exps.gameObject.active=true;
							//tempClone.exps.atlas=listExpression[expressionNum-1].atlas;
							//tempClone.exps.spriteName=listExpression[expressionNum-1].spriteName;
						}
					}
					catch(System.Exception ex)
					{
						Debug.Log (ex.ToString ());
					}
				}

				if (text[i] == ']' && i - 10 >= 0 && text[i - 10] == '[')//道具
                {

                    try
                    {
                        itemIDstr = text.Substring(i - 9, 9);
						if(itemIDstr.IndexOf("[")==-1&&itemIDstr.IndexOf("]")==-1)
						{
	                        for (int k = 0; k < 10; k++)
	                        {
	                            listLable[listLable.Count - 1].text = "";
	                            listLable[listLable.Count - 1].transform.parent.gameObject.SetActiveRecursively(false);
	                            listLable.RemoveAt(listLable.Count - 1);
	                        }
	                        //tempClone.text = "[ff0000][" + itemName + "][-]";
	                        tempClone.lableLink.id = itemIDstr;
	                        tempClone.lableLink.clickCollider.size = new Vector3(2, 1, 1);
	                        parms[0] = itemIDstr;
	                        parms[1] = tempClone;
	                        PanelStatic.StaticBtnGameManager.InvMake.SendMessage("LabelName", parms, SendMessageOptions.DontRequireReceiver);
	                        //tempClone.exps.gameObject.active=true;
	                        //tempClone.exps.atlas=listExpression[expressionNum-1].atlas;
	                        //tempClone.exps.spriteName=listExpression[expressionNum-1].spriteName;
						}
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                }

				if (text[i] == ']' && i - 41 >= 0 && text[i - 41] == '[')//配方
                {

                    try
                    {
                        itemIDstr = text.Substring(i - 40, 40);
						if(itemIDstr.IndexOf("[")==-1&&itemIDstr.IndexOf("]")==-1)
						{
	                        for (int k = 0; k < 41; k++)
	                        {
	                            listLable[listLable.Count - 1].text = "";
	                            listLable[listLable.Count - 1].transform.parent.gameObject.SetActiveRecursively(false);
	                            listLable.RemoveAt(listLable.Count - 1);
	                        }
	                        //tempClone.text = "[ff0000][" + itemName + "][-]";
	                        tempClone.lableLink.id = itemIDstr;
	                        tempClone.lableLink.clickCollider.size = new Vector3(2, 1, 1);
	                        parms[0] = itemIDstr;
	                        parms[1] = tempClone;
	                        PanelStatic.StaticBtnGameManager.InvMake.SendMessage("LabelName", parms, SendMessageOptions.DontRequireReceiver);
	                        //tempClone.exps.gameObject.active=true;
	                        //tempClone.exps.atlas=listExpression[expressionNum-1].atlas;
	                        //tempClone.exps.spriteName=listExpression[expressionNum-1].spriteName;
						}
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                }
				
				if(listLable.Count>0)
				{
					//fontOffest+=listLable[listLable.Count-1].relativeSize.x/2+Kerning*0.05f+tempClone.relativeSize.x/2;
					fontOffest=listLable[listLable.Count-1].transform.parent.localPosition.x +(listLable[listLable.Count-1].relativeSize.x/2+Kerning*0.05f+ tempClone.relativeSize.x/2)*transform.localScale.x*size;
					if(multiLine&& fontOffest>maxWidth)
					{
						tempClone.transform.parent.localPosition =new Vector3(tempClone.relativeSize.x/2*transform.localScale.x*size,listLable[listLable.Count-1].transform.parent.localPosition.y-size,0);
					}
					else
					{
						tempClone.transform.parent.localPosition =new Vector3(fontOffest,listLable[listLable.Count-1].transform.parent.localPosition.y,0);
					}
				}
				else
				{
					//fontOffest=tempClone.relativeSize.x/2;
					tempClone.transform.parent.localPosition =new Vector3(tempClone.relativeSize.x/2*transform.localScale.x*size,0,0);
					
				}
				//tempClone.transform.localPosition=new Vector3(fontOffest*transform.localScale.x*size,0,0);
                listLable.Add(tempClone);
               //yield return new WaitForEndOfFrame();
			}
			if(multiLine)
			{
				
			}
			else
			{
				SetCaratChat ();
			}
			refresh=false;
		}
		
	}
	[HideInInspector]
	public UILabel lbCaratChat;
	private void InsCaratChat()
	{
		lbCaratChat=yuan.YuanClass.AddChildToComponent<UILabel>(transform,"YuanLable");
		lbCaratChat.transform.localScale=new Vector3(1,1,1);
		lbCaratChat.fontSize=this.size;
		lbCaratChat.transform.localPosition=Vector3.zero;
        lbCaratChat.color = this.fontColor;
		if(font)
		{
			lbCaratChat.ambigiousFont=this.font;
		}
		lbCaratChat.text=caratChat;	
	}
	
	public Color fontColor;
	private void InsTextLable()
	{
		tempClone=yuan.YuanClass.AddChildToComponent<UILabel>(this.transform,"YuanLable");
		tempClone.transform.parent.localPosition=Vector3.zero;
		tempClone.pivot=UIWidget.Pivot.Left;
		tempClone.transform.localScale=new Vector3(1,1,1);
		tempClone.fontSize=this.size;
		tempClone.effectStyle = UILabel.Effect.Shadow;
		tempClone.overflowMethod = UILabel.Overflow.ResizeFreely;
		tempClone.effectColor=fontColor;
		if(font)
		{
			tempClone.ambigiousFont=this.font;
		}
		if(multiLine)
		{
			tempClone.multiLine=true;
			tempClone.pivot=UIWidget.Pivot.BottomLeft;
		}
		else
		{
			tempClone.multiLine=false;
		}
		
		
		if(multiLine)
		{
			tempCloneTemp=yuan.YuanClass.AddChildToComponent<UILabel>(this.transform.parent,"YuanLable");
			tempCloneTemp.transform.localPosition=new Vector3(0,500,0);
			tempCloneTemp.pivot=UIWidget.Pivot.Left;
			tempCloneTemp.transform.localScale=new Vector3(1,1,1);
			tempCloneTemp.fontSize=this.size;
			if(font)
			{
				tempCloneTemp.ambigiousFont=this.font;
			}
			tempCloneTemp.multiLine=false;
			tempTextTemp=new List<char>();
		}
	}
	
	private float tempSize=0;
	private float tempOffest=0;
	private void SetCaratChat()
	{
		//if(listLable.Count>0)
		//{
		//	caratChatOffset=fontOffest+ (listLable[listLable.Count-1].relativeSize.x/2+Kerning*0.05f+lbCaratChat.relativeSize.x/2);
		//}
		//else
		//{
		//	caratChatOffset=lbCaratChat.relativeSize.x/2;
		//}

		//if(tempClone.text.Length>0)
		if(this.text.Length>0)
		{
			lbCaratChat.transform.localPosition=new Vector3(listLable[listLable.Count-1].transform.parent.localPosition.x+((listLable[listLable.Count-1].relativeSize.x/2+Kerning*0.01f+ lbCaratChat.relativeSize.x/2)*transform.localScale.x*size),listLable[listLable.Count-1].transform.parent.localPosition.y,0);
		}
		else
		{
			caratChatOffset=lbCaratChat.relativeSize.x/2;
			tempSize=caratChatOffset*transform.localScale.x*size;
			lbCaratChat.transform.localPosition=new Vector3(tempSize,0,0);
		}
		
		
	 	
		//if(!multiLine)
		//{
		//	tempOffest =tempSize-background.localScale.x+15;
		//	if(tempOffest>0)
		//	{
		//		tempClone.transform.localPosition=new Vector3(-tempOffest,0,0);
		//		foreach(GameObject obj in listDisExpression.Keys)
		//		{
		//			obj.transform.localPosition=new Vector3(obj.transform.localPosition.x-tempOffest,obj.transform.localPosition.y,obj.transform.localPosition.z);
		//		}
		//	}
		//	else
		//	{
		//		tempClone.transform.localPosition=Vector3.zero;
		//	}
		//}
	}
	
	private void FlashCaratChat(bool enlabe)
	{
        
		if(enlabe&&lbCaratChat)
		{
            lbCaratChat.text = caratChat;
            //lbCaratChat.gameObject.SetActive(true);
		}
		else if(lbCaratChat)
		{
            lbCaratChat.text = "";
            //lbCaratChat.gameObject.SetActive(false);
		}
	}
	
	private bool flashCarat=false;
	private IEnumerator aFlashCaratChat()
	{
		while(true)
		{
			if(lbCaratChat&&lbCaratChat.gameObject.active==true)
			{
				if(flashCarat)
				{
					flashCarat=false;
					lbCaratChat.text="";
				}
				else
				{
					flashCarat=true;
					lbCaratChat.text=caratChat;
				}
				yield return new WaitForSeconds(1);
			}
		}
	}
	
	
	private void AddCollider()
	{
		boxCollider = gameObject.AddComponent<BoxCollider>();	
	}
	
	private void InputChatPC()
	{
		foreach(char c in Input.inputString)
		{
			if(c=='\b')
			{
				
				if(text.Length>0)
				{
                    Text = Text.Substring(0, Text.Length - 1);
				}
			}
			else if(c=='\r'||c=='\n')
			{
				//Debug.Log ("QQQQQQQQQQ");
				if(multiLine)
				{
					
				}
				else
				{
					isSelect=false;
				}
			}
			else
			{
				if(Text.Length<=chatlength)
				{
                Text += c;
				}
			}
		}
	}
	
	private void  InputChatPhone()
	{
        #if UNITY_IPHONE||UNITY_ANDROID
		if(!keyboard.done)
		{
			if(keyboard.text.Length<=chatlength)
			{
				Text=keyboard.text;
			}
		}
        #endif
	}
	
	private void InstertExpression(float len,int row,int insNum)
	{
		GameObject clone = poolExpression.SpawnEffect (insNum,Vector3.zero,new Quaternion(0,0,0,0));
		clone.transform.parent=this.transform;
		clone.transform.localScale=new Vector3(size,size,1);
		if(multiLine)
		{
			clone.transform.localPosition=new Vector3((len+clone.GetComponent<UISprite>().relativeSize.x/2-1.1f)*transform.localScale.x*size,size/2+ row*size,0);
		}
		else
		{
			clone.transform.localPosition=new Vector3((len+clone.GetComponent<UISprite>().relativeSize.x/2-1.1f)*transform.localScale.x*size,0,0);
		}
		listDisExpression.Add (clone,insNum);
	}
	
	public void GetText(string str,float size)
	{
		text=str;
		isSelect=false;
		refresh=true;
		getSize=size;
	}
	
	public void AddText(string str,float size)
	{
		
		//if(text!="")
		//{
		//	text=text+"\n"+str;
		//	tempLines++;
		//}
		//else
		//{
		//	text=text+str;
		//	tempLines++;
		//}
		//					
		//while(tempLines>10)
		//{
		//	text=text.Substring (text.IndexOf("\n")+1);
		//	tempLines--;
		//}
		text+=str;

		
		isSelect=false;
		refresh=true;
		getSize=size;
	}
	
	public void AddTextObj(object mText)
	{
		this.Text=(string)mText;
	}
	
	private void GetExpression(ObjectGamepool pool)
	{
		pool.EffectP=poolExpression.EffectP;
	}


    void OnSelect(bool isSelected)
    {
		if(isInput)
		{
	        if (isSelected)
	        {
	            isSelect = true;
	            FlashCaratChat(true);
	            Input.imeCompositionMode = IMECompositionMode.On;
	        }
	        else
	        {
	            isSelect = false;
	            FlashCaratChat(false);
	            Input.imeCompositionMode = IMECompositionMode.Off;
	        }
		}
    }
	
	
	


	


	
	
	
}
