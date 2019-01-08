using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelEverydayAim : MonoBehaviour {

    public BtnAim btnAim;
    public UIGrid grid;
    public UISlider sliderActivyValue;
    public UILabel lblActivyValue;
    public GameObject invMaker;
    public GameObject invCL;
    public Warnings warnings;

    public UIAtlas UIGround;
    public UIAtlas UISkills;
    public UIAtlas UILiZi;

    public List<BtnAimGet> listAimGet = new List<BtnAimGet>();
    void Start()
    {
		warnings=PanelStatic.StaticWarnings;
        int num = 0;
        //Dictionary<short,object> parm=((Dictionary<object,object>)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.EverydayAims]).DicObjTo<short,object>();
        Dictionary<short, object> parm = ((Dictionary<object, object>)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.EverydayAims]).DicObjTo<short, object>();
//        Debug.Log("------------------------" + parm.Count);
        foreach (KeyValuePair<short,object> item in parm)
        {
            listAimGet[num].panelAim = this;
            listAimGet[num].strInfo = (string)item.Value;
            listAimGet[num].numGet = num;
            listAimGet[num].invMaker = this.invMaker;
            listAimGet[num].invCL = this.invCL;
            listAimGet[num].warnings = this.warnings;
            listAimGet[num].getType = (yuan.YuanPhoton.GetType)item.Key;
            listAimGet[num].SetInfo();
            num++;
        }

    }

    void OnEnable()
    {
        if (BtnGameManager.yt != null)
        {
            RefreshList();
        }

        RefreshBtnState();
    }

    private List<BtnAim> listBtnAim = new List<BtnAim>();
    private void RefreshList()
    {

        foreach(BtnAim item in listBtnAim)
        {
            item.gameObject.SetActiveRecursively(false);
        }

        int mActivyValue = 0;
        BtnAim tempBtn;
        int num = 0;
        int comNum;
        int maxNum;
        string comStr;
        foreach (yuan.YuanMemoryDB.YuanRow yr in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytEverydayAim.Rows)
        {
            if (listBtnAim.Count > num)
            {
                tempBtn = listBtnAim[num];
                tempBtn.gameObject.SetActiveRecursively(true);
            }
            else
            {
                tempBtn = (BtnAim)Instantiate(btnAim);
                tempBtn.transform.parent = grid.transform;
                tempBtn.transform.localScale = Vector3.one;
                tempBtn.transform.position = grid.transform.position;
                tempBtn.AimID = yr["goType"].YuanColumnText.Trim();
                tempBtn.goAheadBtn.target = this.gameObject;
                tempBtn.goAheadBtn.functionName = "GoAheadBtnClick";
                listBtnAim.Add(tempBtn);
            }

            tempBtn.lblName.text = yr["Name"].YuanColumnText;
            switch (yr["Atlas"].YuanColumnText)
            {
                case "0":
                    tempBtn.pic.atlas = UIGround;
                    tempBtn.pic.depth = 3;   
                    break;
                case "1":
                    tempBtn.pic.atlas = UISkills;
                    tempBtn.pic.depth = 7;   
                    break;
                case "2":
                    tempBtn.pic.atlas = UILiZi;
                    break;
            }
            tempBtn.pic.spriteName = yr["SpriteName"].YuanColumnText==""?"UIH_Skill_Button":yr["SpriteName"].YuanColumnText;
            if ((InRoom.isUpdatePlayerLevel?InRoom.playerLevel.Parse (0):int.Parse(BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText) )>= int.Parse(yr["NeedLevel"].YuanColumnText))
            {
                tempBtn.btnDisable.Disable = false;
                comStr = BtnGameManager.yt.Rows[0][((yuan.YuanPhoton.EverydayAimsType)int.Parse(yr["AimType"].YuanColumnText)).ToString()].YuanColumnText;
//                Debug.Log(tempBtn.lblName.text + ":" + comStr);
                comNum = comStr==""?0:int.Parse(comStr);
                maxNum = int.Parse(yr["NeedNum"].YuanColumnText);
                if (comNum >= maxNum)
                {
                    mActivyValue += int.Parse(yr["ActiveValue"].YuanColumnText);
                    tempBtn.lblNum.text = maxNum + "/" + maxNum;
                }
                else
                {
                    tempBtn.lblNum.text = comNum + "/" + maxNum;
                }
                tempBtn.lblAddActiveValue.text = "+" + yr["ActiveValue"].YuanColumnText + StaticLoc.Loc.Get("info332")+"";

                if(comNum < maxNum)
                {
                    tempBtn.goAheadBtn.GetComponent<UIButton>().isEnabled = true;
                }  
                else
                {
                    tempBtn.goAheadBtn.GetComponent<UIButton>().isEnabled = false;
                }
            }
            else
            {
                tempBtn.btnDisable.Disable = true;
                tempBtn.lblAddActiveValue.text = "Lv"+yr["NeedLevel"].YuanColumnText+StaticLoc.Loc.Get("info333")+"";
                tempBtn.lblNum.text = "0/" + yr["NeedNum"].YuanColumnText;
                tempBtn.goAheadBtn.GetComponent<UIButton>().isEnabled = false;
            }
            num++;
        }
        grid.repositionNow = true;

        lblActivyValue.text = mActivyValue <= 100 ? mActivyValue.ToString() : "100";
        sliderActivyValue.sliderValue = ((float)mActivyValue) / 100f;
        
    }

    /// <summary>
    /// 刷新每日目标领取奖励按钮的显示状态
    /// </summary>
    public void RefreshBtnState()
    {
        foreach(BtnAimGet aimGet in listAimGet)
        {
            if (aimGet.numGet == 0)
            {
                if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(0, 1) == "1")
                {
                    listAimGet[0].gameObject.GetComponent<UIButton>().isEnabled = false;
                }
                else
                {
                    listAimGet[0].gameObject.GetComponent<UIButton>().isEnabled = true;
                }
            }
            else if (aimGet.numGet == 1)
            {
                if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(1, 1) == "1")
                {
                    listAimGet[1].gameObject.GetComponent<UIButton>().isEnabled = false;
                }
                else
                {
                    listAimGet[1].gameObject.GetComponent<UIButton>().isEnabled = true;
                }
            }
            else if (aimGet.numGet == 2)
            {
                if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(2, 1) == "1")
                {
                    listAimGet[2].gameObject.GetComponent<UIButton>().isEnabled = false;
                }
                else
                {
                    listAimGet[2].gameObject.GetComponent<UIButton>().isEnabled = true;
                }
            }
            else if (aimGet.numGet == 3)
            {
                if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(3, 1) == "1")
                {
                    listAimGet[3].gameObject.GetComponent<UIButton>().isEnabled = false;
                }
                else
                {
                    listAimGet[3].gameObject.GetComponent<UIButton>().isEnabled = true;
                }
            }
        }
    }

    public void GoAheadBtnClick(GameObject go)
    {
        //BtnAim aim = go.GetComponentInParent<BtnAim>();

        ////Debug.Log("AimID is --------------" + aim.AimID);
        //int id = -1;
        //if(int.TryParse(aim.AimID, out id))
        //{
        //    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("EverydayAnimAction", id, SendMessageOptions.DontRequireReceiver);
        //}
    }
}
