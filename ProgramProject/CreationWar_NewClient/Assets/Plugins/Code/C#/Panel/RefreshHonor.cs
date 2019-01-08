using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefreshHonor : MonoBehaviour {

    public CkbToPanel ckbToPanel;
    private string numSpera;
	// Use this for initialization
	void Start () {
        int num = 0;
        foreach (GameObject obj in ckbToPanel.strObjsPanel)
        {
            if (obj == this.gameObject)
            {
                numSpera = ckbToPanel.strCkbs[num].transform.FindChild("Background").GetComponent<UISlicedSprite>().spriteName;
                break;
            }
            num++;
        }
        foreach (yuan.YuanPhoton.HonorType item in honorType)
        {
            List<yuan.YuanMemoryDB.YuanRow> listTemp = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytObjective.SelectRowsListEqual("ObjectiveType", ((int)item).ToString());
            if (listTemp != null)
            {
                yt.Rows.AddRange(listTemp);
            }
        }
       //StartCoroutine( GetTable());
	}


    public UILabel lblPoint;
    void OnEnable()
    {
        lblPoint.text = BtnGameManager.yt.Rows[0]["ObjectivePoint"].YuanColumnText;
       StartCoroutine( GetTable());
        GetPlayerHonor();
    }

    
    public yuan.YuanPhoton.HonorType[] honorType;
    public GameObject SpriteForObjectives;
    public UIGrid grid;
    private yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("", "");
    private Dictionary<string, SpriteForObjectives> dicBtn = new Dictionary<string, SpriteForObjectives>();
    private System.Timers.Timer timer;
    IEnumerator GetTable()
    {

        if (yt.Rows.Count != dicBtn.Count)
        {
            foreach (yuan.YuanMemoryDB.YuanRow item in yt.Rows)
            {
                SpriteForObjectives temp;
                if (!dicBtn.ContainsKey(item["id"].YuanColumnText))
                {
                    temp = ((GameObject)Instantiate(SpriteForObjectives)).GetComponent<SpriteForObjectives>();
                }
                else
                {
                    temp = dicBtn[item["id"].YuanColumnText];
                }
                temp.picHead.spriteName = numSpera;
                temp.lblTitle.text = item["ObjectiveName"].YuanColumnText;
                temp.lblInfo.text = item["ObjectiveInfo"].YuanColumnText;
                temp.lblTime.text = "";

                temp.Disable = true;
                temp.transform.parent = grid.transform;
                temp.transform.localScale = new Vector3(1, 1, 1);
                temp.transform.localPosition = Vector3.zero;
                if (!dicBtn.ContainsKey(item["id"].YuanColumnText))
                {
                    dicBtn.Add(item["id"].YuanColumnText, temp);
                }
                yield return new WaitForEndOfFrame();

            }
            grid.repositionNow = true;
        }
    }

    void GetPlayerHonor()
    {
        string[] ids = BtnGameManager.yt.Rows[0]["ObjectiveIDs"].YuanColumnText.Split(';');
        foreach (string item in ids)
        {
            if (item != ""&&dicBtn.ContainsKey(item))
            {
                dicBtn[item].Disable = false;
            }
        }
		grid.repositionNow = true;
    }

  
    
    

}
