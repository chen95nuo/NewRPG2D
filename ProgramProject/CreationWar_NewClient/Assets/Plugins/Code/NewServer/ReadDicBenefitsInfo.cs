using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
public class ReadDicBenefitsInfo : MonoBehaviour {

    public static float RefreshTime = 0.3f;
	// Use this for initialization
	void Start () {
	
	}

    public static void ReadYuanServerRoom(string url)
    {
        try
        {
            Dictionary<short, object> dicBenefitsInfo = new Dictionary<short,object>();
//            Debug.Log("---------------------------"+url+"----------------------------------");
            XmlDataDocument doc = new XmlDataDocument();
            doc.Load(url);
            XmlNode xn = doc.SelectSingleNode("YuanServerRoom");

            int num = 0;
            string numTitle = string.Empty;




            XmlNode guildNode = xn.SelectSingleNode("GuildLevelUp");
            XmlNodeList listGuild = guildNode.SelectNodes("Level");

            Dictionary<object, object> dicGuildLevel = new Dictionary<object, object>();
            foreach (XmlNode node in listGuild)
            {
                dicGuildLevel.Add((short)int.Parse(node.InnerText.Split(';')[0]), node.InnerText);
            }

            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GuildLevelUp]=dicGuildLevel;


            XmlNode xnLogon = xn.SelectSingleNode("LogonStatus");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.LogonStatus]=int.Parse(xnLogon.InnerText);

            XmlNode xnBenefits = xn.SelectSingleNode("Salaries");

            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.Salaries]= int.Parse(xnBenefits.InnerText);


            xnBenefits = xn.SelectSingleNode("RefreshTime");
            RefreshTime = float.Parse(xnBenefits.InnerText);

            xnBenefits = xn.SelectSingleNode("RankBenefits");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.Rank]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("GuildBenefits");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.Guild]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("TVMessageBlood");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.TVMessageBlood]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("HangUpExp");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.HangUpExp]= float.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("PlayerInvite");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerInvite]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("InviterGold");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.InviterGold]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("InviterBlood");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.InviterBlood]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("InviteesGold");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.InviteesGold]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("InviteesBlood");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.InviteesBlood]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("SystemInfo");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.SystemInfo]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("LegionWaitTime");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.LegionWaitTime]= float.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("LegionTime");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.LegionTime]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("MakeGoldTime");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldTime]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("MakeGoldScale");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldScale]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("MakeGoldAddtion");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldAddtion]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("LegionStartNum");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.LegionStartNum]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("PlayerMaxLevel");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerMaxLevel]= int.Parse(xnBenefits.InnerText);
            //xnBenefits = xn.SelectSingleNode("GameVersion");
            //dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameVersion]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("DataVersion");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.DataVersion]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("PlayerMaxNum");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerMaxNum]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("Yuan91SDKBuy");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.Yuan91SDKBuy]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("GameLanguage");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameLanguage]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("BloodTarn");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.BloodTarn]= int.Parse(xnBenefits.InnerText);
            xnBenefits = xn.SelectSingleNode("OfflinePlayerSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.OfflinePlayerSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("GambleSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GambleSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("EqupmentBuildSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.EqupmentBuildSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("EqupmentHoleSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.EqupmentHoleSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("MailSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MailSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("TransactionSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.TransactionSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("AutoPlaySwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.AutoPlaySwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("PVPSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PVPSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("InviteGoPVESwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.InviteGoPVESwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("InvitePVP1Switch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.InvitePVP1Switch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("HeroPVESwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.HeroPVESwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("MakeSoulSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeSoulSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("PVP4Switch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PVP4Switch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("PVP2Switch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PVP2Switch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("GuildSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GuildSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("PetSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PetSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("PetBuySwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PetBuySwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("RedemptionCodeSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.RedemptionCodeSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("EqupmentUpdateSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.EqupmentUpdateSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("CookSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.CookSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("TrainingSwitch");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.TrainingSwitch]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("KeyStore");
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.KeyStore]= xnBenefits.InnerText;
            xnBenefits = xn.SelectSingleNode("EverydayAims");
            string[] tempStr = xnBenefits.InnerText.Trim().Split(';');
            Dictionary<object, object> dicEverydayAim = new Dictionary<object, object>();
            foreach (string item in tempStr)
            {
                if (item != "")
                {
                    string[] itemStr = item.Split(':');
                    switch (itemStr[0])
                    {
                        case "G":
                            dicEverydayAim.Add((short)yuan.YuanPhoton.GetType.Gold, itemStr[1]);
                            break;
                        case "B":
                            dicEverydayAim.Add((short)yuan.YuanPhoton.GetType.BloodStrone, itemStr[1]);
                            break;
                        case "I":
                            dicEverydayAim.Add((short)yuan.YuanPhoton.GetType.Item, itemStr[1]);
                            break;
                        case "H":
                            dicEverydayAim.Add((short)yuan.YuanPhoton.GetType.HP, itemStr[1]);
                            break;
                    }
                }
            }
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.EverydayAims]=dicEverydayAim;
            XmlNode SevenDayRewardsNode = xn.SelectSingleNode("SevenDayRewards");
            Dictionary<object, object> dicDailyBenefits = new Dictionary<object, object>();
            XmlNode Days = SevenDayRewardsNode.SelectSingleNode("FirstDay");
            dicDailyBenefits[0] = Days.InnerText;
            Days = SevenDayRewardsNode.SelectSingleNode("SecondDay");
            dicDailyBenefits[1] = Days.InnerText;
            Days = SevenDayRewardsNode.SelectSingleNode("ThirdDay");
            dicDailyBenefits[2] = Days.InnerText;
            Days = SevenDayRewardsNode.SelectSingleNode("FourthDay");
            dicDailyBenefits[3] = Days.InnerText;
            Days = SevenDayRewardsNode.SelectSingleNode("FifthDay");
            dicDailyBenefits[4] = Days.InnerText;
            Days = SevenDayRewardsNode.SelectSingleNode("SixthDay");
            dicDailyBenefits[5] = Days.InnerText;
            Days = SevenDayRewardsNode.SelectSingleNode("SeventhDay");
            dicDailyBenefits[6] = Days.InnerText;
            dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.DailyBenefits] = dicDailyBenefits;


            YuanUnityPhoton.dicBenefitsInfo = dicBenefitsInfo;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("--------------------------------------"+ex);
        }
    }
    




	// Update is called once per frame
	void Update () {
	
	}
}
