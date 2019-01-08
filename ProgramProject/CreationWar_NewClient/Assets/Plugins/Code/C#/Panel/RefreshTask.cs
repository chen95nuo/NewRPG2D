using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefreshTask : MonoBehaviour {

    public List<GameObject> listLevelBtn = new List<GameObject>();
    public GameObject ckbSelectDuplicate;
    public UIGrid grid;
    public UIGrid gridLevel;
    private yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("mTask", "id");
	void Start () {
        
	}

    void OnEnable()
    {
        GetTable();

    }

    private Dictionary<string, BtnSelectDuplicate> dicBtn = new Dictionary<string, BtnSelectDuplicate>();
    void GetTable()
    {
        string[] tempTaskID = BtnGameManager.yt.Rows[0]["GetPlace"].YuanColumnText.Split(';');
        string taskID=string.Empty;
        string LevelID=string.Empty;
        yuan.YuanMemoryDB.YuanRow yrMap;
        foreach (string item in tempTaskID)
        {
            if (item != ""&&item.Substring (0,1)!="1")
            {
                taskID = item.Substring(0, 3);
                LevelID = item.Substring(3, 1);
               
                yrMap = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID", taskID);
              
                if (dicBtn.ContainsKey(taskID))
                {
                        dicBtn[taskID].strLevel = LevelID;
                }
                else
                {
                    GameObject tempObj = (GameObject)Instantiate(ckbSelectDuplicate);
                    BtnSelectDuplicate tempBtn = tempObj.GetComponent<BtnSelectDuplicate>();
                    tempBtn.lblName.text = yrMap["MapName"].YuanColumnText;
                    tempBtn.transform.parent = grid.transform;
                    tempBtn.transform.localScale = new Vector3(1, 1, 1);
                    tempBtn.transform.localPosition = Vector3.zero;
                    tempBtn.mCheck.group = 4;
                    tempBtn.grid = gridLevel;
                    tempBtn.strLevel = LevelID;
                    tempBtn.listLevelBtn = this.listLevelBtn;
                    dicBtn.Add(taskID, tempBtn);
                }
            }
        }
        grid.repositionNow=true;
        foreach (GameObject item in listLevelBtn)
        {
            item.SetActiveRecursively(false);
        }
    }
	

}
