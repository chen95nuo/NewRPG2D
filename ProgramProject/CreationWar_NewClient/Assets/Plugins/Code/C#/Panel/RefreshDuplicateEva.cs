using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefreshDuplicateEva : MonoBehaviour {


    public GameObject btnSpriteForDungeon;
    public UIGrid grid;
    public UILabel lblPoint;

    public enum DuplicateType
    {
        Normal,
        Elite,
        Dungeon,
    }

    private Dictionary<string, SpriteForDungeon> dicBtns = new Dictionary<string, SpriteForDungeon>();
    private string duplicateString = string.Empty;
    public DuplicateType duplicateType = DuplicateType.Normal;
	// Use this for initialization
	void Start () {
	
	}

    void OnEnable()
    {
        lblPoint.text = BtnGameManager.yt.Rows[0]["DuplicatePoint"].YuanColumnText;
        GetBtns();
    }

    private string[] listInfo = new string[2];
    private string duplicateID = string.Empty;
    private int duplicateStarts = 0;
    private void GetBtns()
    {
        switch(duplicateType)
        {
            case DuplicateType.Normal:
                duplicateString = "DuplicateEvaNormal";
                break;
            case DuplicateType.Elite:
                duplicateString = "DuplicateEvaElite";
                break;
            case DuplicateType.Dungeon:
                duplicateString = "DuplicateEvaDungeon";
                break;
        }
        string[] listDuplicate=BtnGameManager.yt.Rows[0][duplicateString].YuanColumnText.Split(';');
        string strDuplicateName = string.Empty;
        string strDuplicateLevel = string.Empty;

        foreach (string item in listDuplicate)
        {
            if (item != "")
            {
				try
				{
	                listInfo = item.Split(',')	;
	                duplicateID = listInfo[0]	;
	                duplicateStarts =int.Parse( listInfo[1]);

	                strDuplicateName = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID", duplicateID.Substring(0, 3))["MapName"].YuanColumnText;
	                if (duplicateID.Substring(3, 1) == "5")
	                {
						strDuplicateLevel = "精英关卡";
	                }
	                else
	                {
	                    strDuplicateLevel = "难度" + duplicateID.Substring(3, 1);
	                }

	                if (dicBtns.ContainsKey(duplicateID))
	                {
	                    dicBtns[duplicateID].NumStars = duplicateStarts;
	                    //dicBtns[duplicateID].lblName.text = strDuplicateName + strDuplicateLevel;
						dicBtns[duplicateID].lblName.text = strDuplicateName;
	                }
	                else
	                {
	                    SpriteForDungeon btnTemp = ((GameObject)Instantiate(btnSpriteForDungeon)).GetComponent<SpriteForDungeon>();
	                    btnTemp.transform.parent = grid.transform;
	                    btnTemp.transform.localScale = new Vector3(1, 1, 1);
	                    btnTemp.transform.localPosition = Vector3.zero;
	                    btnTemp.NumStars = duplicateStarts;
	                    //btnTemp.lblName.text = strDuplicateName + strDuplicateLevel;
						btnTemp.lblName.text = strDuplicateName ;
	                    dicBtns.Add(duplicateID, btnTemp);
	                }
				}
				catch(System.Exception  e)
				{
					Debug.LogWarning(e.ToString ());
				}
            }
        }
        grid.repositionNow = true;
    }
	

}
