using UnityEngine;
using System.Collections;

public class CkbToPanel : MonoBehaviour {
    public GameObject[] strObjsPanel;
    public UIToggle[] strCkbs;
    public float openPriority = 0;
	public bool isFirst=false;
	public CkbToPanel isFirstCkb;
	public UIToggle ckbFirst;
	
	void Awake()
	{
	     foreach (UIToggle item in strCkbs)
        {
            CkbClick tempCkbClick = item.gameObject.AddComponent<CkbClick>();
            tempCkbClick.ctp = this;
        }
	}


	public int firstNum=0;
    void OnEnable()
    {
//        this.CbkClick(this.gameObject);
        //Invoke("CbkClick", this.openPriority);
		if(isFirst)
		{
			if(isFirstCkb!=null)
			{
			Invoke("EnablePanle", this.openPriority);
			}
			if(ckbFirst!=null)
			{
				Invoke("OnCkbFirst",this.openPriority);
			}
			if(isFirstCkb==null&&ckbFirst==null&& strCkbs.Length>firstNum)
			{
				yuan.YuanClass.SwitchListOnlyOne (strObjsPanel,firstNum,true,true);
				strCkbs[firstNum].value=true;
				//strCkbs[firstNum]
			}
		}
		else
		{
	
			Invoke("CbkClick", this.openPriority);
			
		}	
    }

	private void OnCkbFirst()
	{
		ckbFirst.SendMessage ("OnClick",SendMessageOptions.DontRequireReceiver);
	}

	private void EnablePanle()
	{
					int num=0;
			foreach(UIToggle item in strCkbs)
			{
				if(strObjsPanel[num]==isFirstCkb.gameObject)
				{
                    strObjsPanel[num].SetActiveRecursively(false);
					strObjsPanel[num].SetActiveRecursively (true);
					isFirstCkb.CbkClick ();
					strCkbs[num].value=true;
				}
				else
				{
					strObjsPanel[num].SetActiveRecursively (false);
				}
				num++;
		}
	}

    public void CbkClick()
    {
        int num=0;
        foreach (UIToggle item in strCkbs)
        {
            if (item.value)
            {
				if(!strObjsPanel[num].active)
				{
                	strObjsPanel[num].SetActiveRecursively(true);
				}
            }
            else
            {
				if(strObjsPanel[num].active)
				{
                	strObjsPanel[num].SetActiveRecursively(false);
				}
            }
            num++;
        }
//		if(isFirst&&isFirstCkb!=null)
//		{
//			isFirstCkb.CbkClick ();
//		}
    }



}
