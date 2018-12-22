using UnityEngine;
using System.Collections;

public class EnergyManager : BWUIPanel {
	
	public static  EnergyManager mInstance;
//	public UISlider slider;
	public UILabel label;
	public PVESceneControl pve;
	public GameObject energyRed;
	public GameObject energyYellow;
	
	private float maxEnergy;
	private float curEnergy;
	float scaleNum;
	float changeSpeed = 0.003f;
	private Vector3 energyRedScale;
	private UISprite energyRedSpr;
	private UISprite energyYellowSpr;
	
	//bool needInit = false;
	
	//红色怒气条增加//
	bool isAddRed = false;
	//红色怒气槽减少//
	bool isReduceRed = false;
	// Use this for initialization
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		
	}
	
	void Start () {
		init();
	}
	
	public override void init()
	{
//		slider.sliderValue=0;
		scaleNum = 0f;
		//Debug.Log("pve.players[0] =========== " + pve.players.Length);
		if(pve!=null && pve.players[0] == null)
		{
			//needInit = true;
		}
		else {
			init(pve.players[0].getEnergy(), pve.players[0].getMaxEnergy());
		}
		
		if(energyRed != null && energyRedSpr == null){
//			 energyRed.transform.localScale = Vector3.one;
//			energyRedScale = energyRed.transform.localScale;
			energyRedSpr = energyRed.GetComponent<UISprite>();
			
		}
		if(energyYellow!= null && energyYellowSpr == null)
		{
			energyYellowSpr = energyYellow.GetComponent<UISprite>();
		}
		//修改红色怒气//
//		energyRed.transform.localScale = new Vector3(0.0001f,energyRedScale.y, energyRedScale.z);
//		energyYellow.transform.localScale = new Vector3(0.0001f,1, 1);
		if(energyRedSpr != null)
		{
			energyRedSpr.fillAmount = 0;
		}
		if(energyYellowSpr!= null)
		{
			//energyYellowSpr.fillAmount = 0;
		}
		
		//Debug.Log("EneryManager :  energyYellowSpr.fillAmount ========= " + energyYellowSpr.fillAmount);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(energyRed != null && energyRed.activeSelf )
		{
			if(isReduceRed)
			{
				
				if(energyRedSpr.fillAmount > scaleNum)
				{
					energyRedSpr.fillAmount-= changeSpeed;
				}
				else 
				{
					isReduceRed = false;
				}
			}
			
			if(isAddRed)
			{
				if(energyRedSpr.fillAmount < scaleNum)
				{
					energyRedSpr.fillAmount += changeSpeed;
				}
				else 
				{
					isAddRed = false;
				}
			}
		}
		
		if(energyYellowSpr != null)
		{
			//Debug.Log("energyYellowSpr.fillAmount : " + energyYellowSpr.fillAmount );
		}
	}
	
	public void energyChange()
	{
		if(curEnergy != pve.players[0].getEnergy())
		{
			curEnergy = pve.players[0].getEnergy();
			maxEnergy = pve.players[0].getMaxEnergy();
			setValue(curEnergy);
		}
	}
	
	public void init(float curEnery, float maxEnergy)
	{
		this.maxEnergy=maxEnergy;
		this.curEnergy = curEnery;
		float s = curEnery/maxEnergy;
		//Debug.Log("EneryManager : init ()  s ========= " + s);
		if(energyRedSpr == null && energyRed!= null)
		{
			energyRedSpr = energyRed.GetComponent<UISprite>();
		}
		if(energyRedSpr!= null)
		{
			energyRedSpr.fillAmount = s;
		}
		if(energyYellowSpr == null)
		{
			energyYellowSpr = energyYellow.GetComponent<UISprite>();
		}
		
		if(energyYellowSpr != null)
		{
			energyYellowSpr.fillAmount = s;
		}
		label.text=curEnergy+"/"+maxEnergy;
		//Debug.Log("EneryManager : init ()  energyYellowSpr.fillAmoun ========= " + energyYellowSpr.fillAmount);
	}
	
	public void setValue(float curEnergy)
	{
		
		this.curEnergy=curEnergy;
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			//return;
		}
		scaleNum = curEnergy/maxEnergy;
		label.text=curEnergy+"/"+maxEnergy;
		
		if(scaleNum <= 0){
			scaleNum = 0f;
		}
		else if(scaleNum >= 1){
			scaleNum = 1;
		}
		
		//Debug.Log("EneryManager : setValue ()  scaleNum ========= " + scaleNum);
		
		if(energyRed != null && energyRed.activeSelf){
//			float x = scaleNum * energyRedScale.x;
//			Vector3 scaleV3 = new Vector3( x, energyRedScale.y, energyRedScale.z);
////			Debug.Log("eneryRed.Scale ============= " + energyRed.transform.localScale);
//			iTween.ScaleTo(energyRed, scaleV3, 1.0f);
			if(energyRedSpr.fillAmount > scaleNum)		//减少//
			{
				isReduceRed = true;
			}
			
			if(energyRedSpr.fillAmount < scaleNum)		//增加//
			{
				isAddRed = true;
			}
		}
		if(energyYellow != null && energyYellow.activeSelf){
			
//			energyYellow.transform.localScale = new Vector3(scaleNum, 1, 1);
			energyYellowSpr.fillAmount = scaleNum;
		}
		
		//Debug.Log("EneryManager : setValue ()  energyYellowSpr.fillAmount ========= " + energyYellowSpr.fillAmount);
	}
	
	public void gc()
	{
		pve=null;
		_MyObj = null;
		mInstance = null;
	}
	
}
