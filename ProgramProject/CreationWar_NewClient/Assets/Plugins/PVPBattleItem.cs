using UnityEngine;
using System.Collections;

public class PVPBattleItem : MonoBehaviour 
{
    public UILabel lblName;
    public UIButtonMessage btnMsg;
	public UILabel LblIsBusy;

    private int itemID = -1;

    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }

	// Use this for initialization
	void Start () 
	{
        if (null == lblName)
        {
            lblName = GetComponentInChildren<UILabel>();
        }

		if (null == LblIsBusy)
		{
			LblIsBusy = GetComponentInChildren<UILabel>();
		}


        if (null == btnMsg)
        {
            btnMsg = GetComponentInChildren<UIButtonMessage>();
        }
	}

    public void SetBattleName(string name)
    {
        if (null == lblName)
        {
            return;
        }

        lblName.text = name;
    }

	public void SetBattleLblbusy(string Busy)
	{
		if (null == LblIsBusy)
		{
			return;
		}
		
		LblIsBusy.text = Busy;
	}
}
