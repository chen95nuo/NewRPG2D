using UnityEngine;
using System.Collections;
using System.Xml;
public class ReadPlayerInfoXML : MonoBehaviour {


    public static string[] playerinfo;


    private void ReadPlayerInfoXml()
    {
        string data = Resources.Load("Playerinfo").ToString();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(data);

        XmlNode xn = xmlDoc.SelectSingleNode("Playerinfo");
        XmlNodeList xnlist = xn.SelectNodes("property");
        XmlNodeList xnlistChild;

        playerinfo = new string[xnlist.Count];

        for (int i = 0; i < xnlist.Count; i++)
        {
            XmlNode node = xnlist[i];
            playerinfo[i] = node.InnerText;
        }
    }

	// Use this for initialization
	void Start () {
        ReadPlayerInfoXml();
	}
}
