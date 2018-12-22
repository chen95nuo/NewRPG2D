using UnityEngine;
using System.Collections;

public class BattleGameHelperCard : MonoBehaviour {
	
	public static BattleGameHelperCard mInstance = null;
	public GameObject cardObj;
	GameObject card = null;
	
	void Awake()
	{
		mInstance = this;
		hideCard();
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void showCard()
	{
		cardObj.SetActive(true);
		if(card == null)
		{
			GameObject prefab = Resources.Load("Prefabs/Cards/talk") as GameObject;
			if(prefab != null)
			{
				card = GameObject.Instantiate(prefab) as GameObject;
				Vector3 scale = card.transform.localScale;
				GameObjectUtil.setGameObjectLayer(card,STATE.LAYER_ID_NGUI);
				GameObjectUtil.gameObjectAttachToParent(card,cardObj);
				card.transform.localScale = scale;
				//DaoGuangController dgc = card.GetComponent<DaoGuangController>();
				//if(dgc != null)
				//{
				//	if( dgc.trail != null && dgc.trail.gameObject.activeSelf)
				//	{
				//		dgc.trail.gameObject.SetActive(false);
				//	}
				//}
				CardEffectControl cec = card.GetComponent<CardEffectControl>();
				if(cec != null)
				{
					cec.hideEffect();	
				}
			}
		}
		
	}
	
	public void hideCard()
	{
		cardObj.SetActive(false);
	}
	
	public void gc()
	{
		card=null;
		//Resources.UnloadUnusedAssets();
	}
}
