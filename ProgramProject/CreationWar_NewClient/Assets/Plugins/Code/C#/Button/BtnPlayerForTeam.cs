using UnityEngine;
using System.Collections;

public class BtnPlayerForTeam : MonoBehaviour {

    public UILabel lblPlayerName;
    public UILabel lblPlayerLevel;
    public UILabel lblPlayerPro;
    public UISprite picPlayer;
    public PlayerInfo playerInfo;
    public UIToggle myCheckbox;
	public UIButton btnAdd;
	public UIButtonMessage btnAddMessage;
	public UILabel btnAddLable;
    public string playerID = string.Empty;
    public UISlicedSprite picNew;
    public yuan.YuanMemoryDB.YuanRow yr;
	
	public string strPro=string.Empty;

	public UILabel GuildPosition;
	
}
