using UnityEngine;
using System.Collections;

public class SpreadManager : MonoBehaviour {
	
	public GameObject card_info;
	public GameObject card_info_scrollBar;
	public GameObject unit_info;
	public GameObject unit_info_scrollBar;
    public GameObject break_info;
    public GameObject break_info_scrollBar;
	
	//private Vector3 pos_up=new Vector3(0,160f,0);
	//private Vector3 pos_down=new Vector3(0,-190f,0);
	
	// Use this for initializationr
    void Awake()
    {

        card_info.transform.FindChild("child").gameObject.SetActive(true);
        unit_info.transform.FindChild("child").gameObject.SetActive(false);
        break_info.transform.FindChild("child").gameObject.SetActive(false);


        card_info.transform.FindChild("true").gameObject.SetActive(true);
        unit_info.transform.FindChild("true").gameObject.SetActive(false);
        break_info.transform.FindChild("true").gameObject.SetActive(false);
        initScrollBar();
    }
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void initScrollBar()
	{
		card_info_scrollBar.GetComponent<UIScrollBar>().value=0;
		unit_info_scrollBar.GetComponent<UIScrollBar>().value=0;
        break_info_scrollBar.GetComponent<UIScrollBar>().value = 0;
	}
	public void onClickSpread(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(param == 1)
		{
			
			card_info.transform.FindChild("child").gameObject.SetActive(true);
			unit_info.transform.FindChild("child").gameObject.SetActive(false);
            break_info.transform.FindChild("child").gameObject.SetActive(false);


            card_info.transform.FindChild("true").gameObject.SetActive(true);
            unit_info.transform.FindChild("true").gameObject.SetActive(false);
            break_info.transform.FindChild("true").gameObject.SetActive(false);

			initScrollBar();
		}
		else if(param == 2)
		{
			
			card_info.transform.FindChild("child").gameObject.SetActive(false);
			unit_info.transform.FindChild("child").gameObject.SetActive(true);
            break_info.transform.FindChild("child").gameObject.SetActive(false);

            card_info.transform.FindChild("true").gameObject.SetActive(false);
            unit_info.transform.FindChild("true").gameObject.SetActive(true);
            break_info.transform.FindChild("true").gameObject.SetActive(false);
			initScrollBar();
		}
        else if (param == 3)
        {
            if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
            {
                UISceneDialogPanel.mInstance.showDialogID(54);
                GuideUI22_Break.mInstance.hideAllStep();
                GuideUI22_Break.mInstance.showStep(3);

                
            }
            card_info.transform.FindChild("child").gameObject.SetActive(false);
            unit_info.transform.FindChild("child").gameObject.SetActive(false);
            break_info.transform.FindChild("child").gameObject.SetActive(true);

            card_info.transform.FindChild("true").gameObject.SetActive(false);
            unit_info.transform.FindChild("true").gameObject.SetActive(false);
            break_info.transform.FindChild("true").gameObject.SetActive(true);
            initScrollBar();
        }
	}
}
