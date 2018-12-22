using UnityEngine;
using System.Collections;

public class BloodManager : MonoBehaviour {
//	public GameObject bloodBlackBox;
//	public GameObject bloodGreenBox;
//	public GameObject bloodRedBox;
	public UISprite bloodGreen;
	
	public GameObject posObject;
	public GameObject parent;
	private Transform _myTransform;
	public Camera NGUICamera;
	public Camera mainCamera;
//	public CActorManager playerManager;
	//int myPlayerCurHp = 0;
	//float bloodBarWidth = 0;
	public Card myCard;
	public float myCurHp = 0;
//	public UISlider slider;
	
	void Awake(){
		_myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		//bloodBarWidth = _myTransform.localScale.x;
		if(myCard != null){
			
			myCurHp = myCard.getCurHp();
		}
	}
	
	public void init(){
		if(myCard != null){
			myCurHp = myCard.getCurHp();
			bloodGreen.fillAmount = myCurHp/(myCard.getMaxHp());
		}
	}
	
	void LateUpdate()
	{
		if(posObject != null && NGUICamera!= null)
		{
			Vector3 worldPos = posObject.transform.position;
			Vector2 screenPos =  mainCamera.WorldToScreenPoint(worldPos);
			Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
			_myTransform.position = curPos;
		}
	}
	
	public void changeBloodBar(){
		if(myCard != null){
			if(myCurHp != myCard.getCurHp()){
				myCurHp = (float)myCard.getCurHp();
				bloodGreen.fillAmount = myCurHp/(myCard.getMaxHp());
			}
		}
	}
	
	public void BloodBoxControl(int hurtNum){
		GreenBloodBox(hurtNum);
		RedBloodBox(hurtNum);
	}
	
	public void GreenBloodBox(int hurtNum){
//		if(myPlayerCurHp != playerManager.actor.curHp){
//			myPlayerCurHp = playerManager.actor.curHp;
//			float width = myPlayerCurHp/playerManager.actor.maxHp * bloodBarWidth;
//			_myTransform.localScale = new Vector3(width, _myTransform.localScale.y, _myTransform.localScale.z);
//		}
	}
	
	public void RedBloodBox(int hurtNum){
		
	}
	
	public void gc()
	{
		myCard=null;
	}
}
