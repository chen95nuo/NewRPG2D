using UnityEngine;
using System.Collections;

// 显示新手指引的卡牌模型//
public class GameHelperCard : MonoBehaviour {
	
	public static GameHelperCard mInstance = null;
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
		gc();
	}
	
	private void gc()
	{
		Resources.UnloadUnusedAssets();
	}
}
