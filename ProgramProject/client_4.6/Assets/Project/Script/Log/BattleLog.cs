using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class BattleLog
{
	private static BattleLog instance;
	/**传给服务器供校验**/
	private List<string> battlelogs;
	
	public static BattleLog getInstance()
	{
		if(instance==null)
		{
			instance=new BattleLog();
		}
		return instance;
	}
	
	private BattleLog()
	{
		battlelogs=new List<string>();
	}
	
	/**开始记录战斗日志**/
	public void logStart()
	{
		battlelogs.Clear();
	}
	
	/**站位%血量改变值%暴击闪避标识%死亡标识**/
	public void log(int sequence,int[] hurt)
	{
		battlelogs.Add(sequence+"%"+hurt[0]+"%"+hurt[1]+"%"+hurt[2]);
	}
	
	public string getLog()
	{
		string s="";
		for(int i=0;i<battlelogs.Count;i++)
		{
			s+=(string)battlelogs[i]+"&";
		}
		if(!"".Equals(s))
		{
			s=s.Substring(0,s.Length-1);
		}
		battlelogs.Clear();
		if("".Equals(s))
		{
			s=" ";
		}
		return s;
	}
	
//	private void writeToFile2()
//	{
//		//string path=System.Environment.CurrentDirectory;
//		string directory="D:/unityLog";
//		string path="D:/unityLog/battle_"+System.DateTime.Now.ToString("yyyy-MM-dd")+".log";
//		if(!Directory.Exists(directory))
//		{
//			Directory.CreateDirectory(directory);
//		}
//		if(!File.Exists(path))
//		{
//			using(StreamWriter sw=File.CreateText(path))
//			{
//				//sw.Encoding=System.Text.Encoding.UTF8;
//				sw.WriteLine("Battle Log:");
//			}
//		}
//		using(StreamWriter sw=File.AppendText(path))
//		{
//			for(int i=0;i<battleInfo.Count;i++)
//			{
//				sw.WriteLine(System.DateTime.Now.ToString("HH:mm:ss")+" "+((string)battleInfo[i]),System.Text.Encoding.UTF8);
//			}
//		}
//		clear();
//	}
	
}
