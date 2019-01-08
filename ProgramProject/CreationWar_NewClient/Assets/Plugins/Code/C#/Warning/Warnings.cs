using UnityEngine;
using System.Collections;

public class Warnings : MonoBehaviour {
    public WarningAll warningAllEnter;
    public WarningAll warningAllEnterClose;
	public WarningAll warningAllEnterHonor;
    public WarningAll warningAllTime;
	public WarningAll warningAllTimeOut;
	public WarningAll warningAllTime1;
    public GameObject InCL;

    void Start()
    {

        warningAllEnter.gameObject.SetActiveRecursively(false);
        warningAllEnterClose.gameObject.SetActiveRecursively(false);
		warningAllEnterHonor.gameObject.SetActiveRecursively(false);
        warningAllTime.gameObject.SetActiveRecursively(false);
		warningAllTimeOut.gameObject.SetActiveRecursively(false);
		if(warningAllTime1){
			warningAllTime1.gameObject.SetActiveRecursively(false);
		}

    }

    /// <summary>
    /// 弹出开启宝箱面板
    /// </summary>
    /// <param name="mGold">金钱</param>
    /// <param name="mBlood">血石</param>
    /// <param name="mItemID1">物品1</param>
    /// <param name="mItemID2"></param>
    /// <param name="mItemID3"></param>
    /// <param name="mItemID4"></param>
    public void OpenBoxBar(string mGold,string mBlood,params string[] mItemID)
    {
        string[] str = new string[6];
		for(int i=0;i<str.Length;i++)
		{
			str[i]=string.Empty;
		}
        str[0] = mGold;
        str[1] = mBlood;

        for (int i = 0; i < 4; i++)
        {
            if (null != mItemID && mItemID.Length > i)
			{
            	str[2 + i] = mItemID[i];
			}
        }
            InCL.SendMessage("openBox", str, SendMessageOptions.DontRequireReceiver);
    }
}
