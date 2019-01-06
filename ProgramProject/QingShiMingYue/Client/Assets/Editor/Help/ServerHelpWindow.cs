using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ServerHelpWindow : EditorWindow
{
	private string[] ips = {
		"127.0.0.1","192.168.199.180"
       };
	private string[] ports = {"26001", "29000" };

    private string xmlModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<ClientConfig>\n<ServerIP>{0}</ServerIP>\n<ServerPort>{1}</ServerPort>\n<AuthServerIP>{2}</AuthServerIP>\n<AuthServerPort>{3}</AuthServerPort>\n<Version>1.2</Version>\n</ClientConfig>";
	//private string xmlModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<ClientConfig>\n<ServerIP>{0}</ServerIP>\n<ServerPort>{1}</ServerPort>\n</ClientConfig>";

	private int ip_index = 0;
	private int port_index = 0;
	private bool isActivion = false;
    private int AuthPort = 26000;
	private string inIpStr = "";
	private static ServerHelpWindow instace;
	[MenuItem("Server Help/Setting/Ip %q")]
	public static void SeverIpSetting()
	{
		instace = GetWindow(typeof(ServerHelpWindow), true, "Service Setting") as ServerHelpWindow;
	}

	void OnGUI()
	{
		ip_index = EditorGUILayout.Popup("选择IP:", ip_index, ips);
		port_index = EditorGUILayout.Popup("选择端口:", port_index, ports);
		isActivion = EditorGUILayout.Toggle("是否使用输入IP：", isActivion);
		if (isActivion)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("请输入Ip地址:");
			inIpStr = EditorGUILayout.TextField(inIpStr);
			EditorGUILayout.EndHorizontal();
		}
		GUILayout.Space(50);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("OK", GUILayout.Height(40)))
		{
			if (isActivion)
				ReadClientConfigXML(inIpStr, ports[port_index]);
			else
				ReadClientConfigXML(ips[ip_index], ports[port_index]);
			instace.Close();
		}
		GUILayout.EndHorizontal();

	}

	void ReadClientConfigXML(string ip, string port)
	{
		string path = "Assets\\Resources\\Texts\\Configs\\ClientConfig.xml";

		if (File.Exists(path))
		{
			File.Delete(path);
		}
		// Create a file to write to.
		using (StreamWriter sw = File.CreateText(path))
		{
            sw.WriteLine(string.Format(xmlModel, ip, port, ip, AuthPort));

		}

		AssetDatabase.Refresh(ImportAssetOptions.Default);


	}



}
