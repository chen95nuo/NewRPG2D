using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization; 
using System.Xml.XPath;
using System.IO;
using System.Text;

//public enum MainTaskType {
//	xiaoguai = 1,
//	props = 2,
//	boss = 3
//}
//
//public enum MainItemType {
//	equipment = 0,
//	garbage = 1,
//	taskItem = 2,
//	consumable = 3
//}
//public enum MainTaskNeedType {
//	level = 1,
//	taskBefore = 2,
//	time = 3
//}  
//
//public class MainTaskReward{
//	public MainItemType type = 0;
//	public string id  = "000";
//	public int gold = 0;
//	public int  exp = 0;
//	public int rank = 0;
//}
//public class MainTaskNPC {
//	public string id  = "001";
//	public string point  = "001001";
//	public string map  = "01";
//}
//
//public class MainTask	{
//	public MainTaskType taskType ; 
//	public string id  ;
//	public MainTaskNeedType needType;
//	public string needId ; 
//	public string doneType ; 
//	public string donNum ;
//	public MainTaskNPC mainNPC;	
//	public MainTaskReward reward = null;
//	public int jindu;	
//} 
//
//public class MainPlayerStatus{
//	public int level = 1;
//	public string name;
//	public string[] taskId ;
//}
public class TransactionParameters{
	public string playerID;
	public string playerName;	
	public yuan.YuanPhoton.RequstType requstType ;
	public string equepmentID;
	public string gold;
	public string blood;
	public bool isReady;
	public bool isTransaction;
}
public class XmlC : Photon.MonoBehaviour {
//public class XmlC : MonoBehaviour {
//	XmlDocument mainXmlDoc;
//	public static string _FileLocation;
//	public static string _FileName = "test.xml";
//	public static string _PlayerFileName = "PlayerStatusData.xml";
//	
//	private MainTaskNPC useNPC;
//	public MainTaskNPC LoadXmlNPC(string NPCid) {
//		useNPC = new MainTaskNPC();
//		mainXmlDoc = new XmlDocument();
//		mainXmlDoc.Load(_FileLocation+"/test.xml");
//		XmlNode root = mainXmlDoc.SelectSingleNode("datas");
//		if(root != null){		
//			XmlNode node = root.ChildNodes[0];
//			XmlNodeList nodelist = node.ChildNodes;
//            foreach (XmlNode NPCnode in nodelist)
//            {
//                XmlElement xmlelement = (XmlElement)NPCnode;
//                if (xmlelement.GetAttribute("id") == NPCid)
//                {
//					xmlelement.SetAttribute("id","002");
//					useNPC.id = xmlelement.GetAttribute("id");
//					useNPC.point = xmlelement.GetAttribute("point");
//					useNPC.map = xmlelement.GetAttribute("map");
//                    break;
//                }
//            }			
//		}
//		return useNPC;
//	}
	public XmlElement SetAttribute(string s1 , string s2,XmlElement xe){
		return xe;
	}
	
	public XmlElement SetToElement(XmlElement xe , XmlNode xn){
		xe = (XmlElement)xn;
		return xe;
	}
}
