using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeSceneLoadManager : BWUIPanel
{

    public static ChangeSceneLoadManager mInstance;

    string levelName;
    float frameCount;
    bool isCanChangeSene;

    public UILabel description;
    public UILabel Tips;
    public UILabel cardName;
    public GameObject modelNode;
	
	public GameObject loading_sparks;

    void Awake()
    {
		loading_sparks = GameObject.Find("loading_sparks");
        mInstance = this;
        _MyObj = mInstance.gameObject;
        init();
        hide();
		
    }

    public override void init()
    {
        base.init();
        isCanChangeSene = false;
    }

    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanChangeSene)
        {
            if (frameCount > 1)
            {
                Application.LoadLevel(levelName);
                frameCount = 0;
                isCanChangeSene = false;
            }
            frameCount += Time.deltaTime;
        }
    }

    public override void show()
    {
		if(isVisible())
			return;
        base.show();
        ShowLoading();
        if (loading_sparks!=null)
        {
            loading_sparks.SetActive(false);
        }
    }

    public void setData(string levelName)
    {
        show();
        this.levelName = levelName;
        frameCount = 0;
        isCanChangeSene = true;
    }

    public override void hide()
    {
        base.hide();
		
		gc();
    }
	
	public void gc()
	{
//		mInstance = null;
	}

    void OnLevelWasLoaded(int levelId)
    {
        hide();
		mInstance = null; 
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
    }

    void ShowLoading()
    {
        int i = Random.Range(0, CardTipsData.idList.Count);
        int cardTipsId = CardTipsData.idList[i];
        CardTipsData cd = CardTipsData.getData(cardTipsId);
        Show3DCard(cd.id);
        description.text = cd.description;
        cardName.text = cd.name;

        i = Random.Range(0, TipsData.idList.Count);
        int id = TipsData.idList[i];
        TipsData td = TipsData.getData(id);
        Tips.text = td.chinese;


    }
    void Show3DCard(int id)
    {
        CardTipsData ctd = CardTipsData.getData(id);
        if (ctd == null)
        {
            hide();
            return;
        }
        GameObject prefab = Resources.Load("Prefabs/Cards/" + ctd.model) as GameObject;
        GameObject cardModel = GameObject.Instantiate(prefab) as GameObject;
		Vector3 mnP = modelNode.transform.localPosition;
        cardModel.transform.parent = modelNode.transform;
        if (cardModel == null)
        {
            return;
        }
        GameObjectUtil.setGameObjectLayer(cardModel, STATE.LAYER_ID_NGUI);
        CardEffectControl cec = cardModel.GetComponent<CardEffectControl>();
        if (cec != null)
        {
            cec.hideEffect();
        }
		Animator animator = cardModel.GetComponent<Animator>();
		if(animator != null)
		{
			animator.enabled = false;
		}
		
		//Vector3 cdmS = cardModel.transform.localScale;
        cardModel.transform.localScale = new Vector3(ctd.zoom,ctd.zoom,ctd.zoom);
        cardModel.transform.localEulerAngles = new Vector3(0, ctd.modelrotation, 0);
		modelNode.transform.localPosition = mnP;
		cardModel.transform.localPosition = new Vector3(0,ctd.yPos,0);
    }
}