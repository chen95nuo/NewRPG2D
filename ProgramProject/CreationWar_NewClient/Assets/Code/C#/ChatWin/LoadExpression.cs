using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadExpression : MonoBehaviour {
	
	public GameObject input;
	public UIButton btn;
    public int depth = 15;
	
	private ObjectGamepool pool;
	private Transform btnPoint;
	private List<UISprite> listExpr=new List<UISprite>();
	private List<UIButton> listBtn = new List<UIButton>();
	
	void Awake()
	{

	}
	
	// Use this for initialization
	void Start () {
		FindObj ();
		GetBtn ();
	}
	
	// Update is called once per frame
	void Update () {
//		if(btnPoint!=null)
//		{
//			NGUIDebug.Log (string.Format ("{0}   P:{1},S:{2},A:{3}",System.DateTime.Now.Millisecond,btnPoint.transform.localPosition,btnPoint.transform.localScale,btnPoint.gameObject.activeSelf));
//		}
	}
	
	void FindObj()
	{
		btnPoint=transform.FindChild ("Btn");
		if(input)
		{
			pool=input.GetComponent<ObjectGamepool>();
		}
		if(pool&&listExpr!=null&&listExpr.Count==0)
		{
			foreach(EffectSP sp in pool.EffectP)
			{
				listExpr.Add (sp.effectprefab.GetComponent<UISprite>());
			}
		}
	}
	
	private GameObject tempBtn;
	private UIPlayTween btntw;
	private BtnExpressionSend btnSend;
	void GetBtn()
	{
		if(listExpr.Count>0&&listBtn.Count==0)
		{ 
			
//			for(int j=0;j<10;j++)
//			{
			int i=0;
			foreach(UISprite sp in listExpr)
			{
				tempBtn=(GameObject)Instantiate (btn.gameObject);

				tempBtn.transform.parent=btnPoint;
				tempBtn.transform.localScale=new Vector3(1.6f,1.6f,1);
				tempBtn.transform.localPosition=Vector3.zero;
				UIButton objBtn=tempBtn.GetComponent<UIButton>();
				listBtn.Add (objBtn);
				objBtn.normalSprite= listExpr[i].spriteName;
                UISprite btSprite = tempBtn.transform.GetComponentInChildren<UISlicedSprite>();
                btSprite.spriteName = listExpr[i].spriteName;
                btSprite.depth = this.depth;
				btntw = tempBtn.GetComponent<UIPlayTween>();
				btntw.tweenTarget=this.gameObject;
				btntw.playDirection=AnimationOrTween.Direction.Reverse;
				btntw.ifDisabledOnPlay=AnimationOrTween.EnableCondition.EnableThenPlay;
				btntw.disableWhenFinished =AnimationOrTween.DisableCondition.DisableAfterReverse;
				btnSend=tempBtn.GetComponent<BtnExpressionSend>();
				btnSend.input=this.input.GetComponent<YuanInput>();
				if((i+1)/10<1)
				{
						btnSend.str="[0"+(i+1).ToString ()+"]";
				}
				else
				{
						btnSend.str="["+(i+1).ToString ()+"]";
				}
				i++;
			}
			SortBtn ();
//			}
		}
	}

	void SortBtn()
	{
		int columMax=14;
		int x=19;
		int y=25;

		int nowColum=0;
		int nowRow=0;
		float tempX=0;
		float tempY=0;

		foreach(UIButton item in listBtn)
		{
			item.transform.localPosition=new Vector3(tempX,tempY,0);
			nowColum++;
			tempX=tempX+x+((BoxCollider)item.gameObject.collider).size.x;
			if(nowColum>=columMax)
			{
				nowRow++;
				tempY=tempY-y-((BoxCollider)item.gameObject.collider).size.y;
				nowColum=0;
				tempX=0;
			}
		}
	}
}
