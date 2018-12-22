using UnityEngine;
using System.Collections;

public class ResultCardManager : MonoBehaviour {
	
	private Transform _myTransform;
	
	public SimpleCardInfo2 cardInfo;
	public UILabel cardLevel;
	public GameObject LevelUpEff;
	public ExpManager CardExp;  
	
	TweenAlpha levelUpAlpha;
	TweenPosition levelUpPosition;
	
	void Awake()
	{
		_myTransform=gameObject.transform;
	}
	
	// Use this for initialization
	void Start () {
		if(levelUpAlpha == null)
		{
			levelUpAlpha = LevelUpEff.GetComponent<TweenAlpha>();
		}
		
		if(levelUpPosition == null)
		{
			levelUpPosition = LevelUpEff.GetComponent<TweenPosition>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void ChangeLvLabel(int lv)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_LEVELUP);
		cardLevel.text = "LV." + lv;
	}
	
	public void ShowCardLevelUpEff()
	{
		LevelUpEff.SetActive(true);
		levelUpPosition.from = new Vector3(0, 0, 0);
		levelUpAlpha.from = 1;
		GameObjectUtil.playForwardUITweener(levelUpPosition);
		GameObjectUtil.playForwardUITweener(levelUpAlpha);
		
		//==播个特效==//
		GameObject effect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/LeveL_icon",1)) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(effect,_myTransform.gameObject);
		GameObjectUtil.setGameObjectLayer(effect,_myTransform.gameObject.layer);
		Destroy(effect,2f);
	}
}
