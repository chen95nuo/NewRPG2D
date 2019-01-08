using UnityEngine;
using System.Collections;

public class ReadSystem : MonoBehaviour
{
    public CharBar barMainAll;
    public CharBar barAll;
    public CharBar barGuild;
    public yuan.YuanMemoryDB.YuanTable ytGuild = new yuan.YuanMemoryDB.YuanTable("mGuildInfo", "id");


    object[] parm = new object[2];
    IEnumerator Start()
    {
        foreach (yuan.YuanMemoryDB.YuanRow yr in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytNotice.Rows)
        {

            parm[0] = (object)yr["NoticeText"].YuanColumnText;
            parm[1] = (object)Color.yellow;
            //barAll.SendMessage("AddText", parm, SendMessageOptions.DontRequireReceiver);
            //barMainAll.SendMessage("AddText", parm, SendMessageOptions.DontRequireReceiver);
			if(barAll!=null){
            barAll.AddText(parm);
			}
			if(barMainAll!=null){
            barMainAll.AddText(parm);
			}
            yield return new WaitForFixedUpdate();
        }
        if (BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
            //InRoom.GetInRoomInstantiate().GetYuanTable("select * from GuildInfo where id=" + BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText, "DarkSword2", ytGuild);
            InRoom.GetInRoomInstantiate ().GetTableForID (BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText,yuan.YuanPhoton.TableType.GuildInfo,ytGuild);
			if (ytGuild.IsUpdate)
            {
                yield return new WaitForSeconds(1);
            }
            if (ytGuild.Rows.Count > 0)
            {
                parm[0] = (object)ytGuild.Rows[0]["GuildNotice"].YuanColumnText;
                parm[1] = (object)Color.green;
                //barAll.SendMessage("AddText", parm, SendMessageOptions.DontRequireReceiver);
                //barMainAll.SendMessage("AddText", parm, SendMessageOptions.DontRequireReceiver);
                //barGuild.SendMessage("AddText", parm, SendMessageOptions.DontRequireReceiver);
                
                barMainAll.AddText(parm);
				if(barAll!=null)
				{
					barAll.AddText(parm);
	                barGuild.AddText(parm);
				}
            }

        }

    }

}
