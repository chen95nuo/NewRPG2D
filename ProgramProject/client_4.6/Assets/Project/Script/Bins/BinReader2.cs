using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;
using System.Threading;

public class BinReader2  {

	public static string RES_DIR = "Assets/Project/Resources/Bins/";//默认资源所在目录	
	public const char CHAR_VARS_START=';';
	public const char CHAR_COMMENT_START = '/';
	private static BinReader2 _instance;
	
//	public void parseFile(string filename,Type type)
//	{
//		WWW www = new WWW (filename);
//		while(!www.isDone)
//		{
//			Thread.Sleep(5);
//		}
//		if(www.error!= null)
//		{
//			Debug.Log("error ===== " + www.error);
//		}
//		else
//		{
//			byte[] b = www.bytes;
//			//解密
//			byte[] b2 = new byte[b.Length];
//			for(int k=0;k<b2.Length;k++)
//			{
//				b2[k] = (byte)(~(b[k]-8) & 0xFF);
//			}
//			
//			ByteArray ba = new ByteArray(b2);
//			int rows = ba.readInt();
//			int cols = ba.readInt();
//			string[] fieldNames=null;
//		}
//		//Debug.Log("filename:"+filename);
//	}

	
	//============================ios read=========================================//
//	public IEnumerator parseFileForIos(string filename,Type type,LoadRes load)
//	{
//		WWW www = new WWW (filename);
//		yield return www;
//		if(www.error!= null)
//		{
//			Debug.Log("error ===== " + www.error);
//		}
//		else
//		{
//			byte[] b = www.bytes;
//			//解密
//			byte[] b2 = new byte[b.Length];
//			for(int k=0;k<b2.Length;k++)
//			{
//				b2[k] = (byte)(~(b[k]-8) & 0xFF);
//			}
//			
//			ByteArray ba = new ByteArray(b2);
//			int rows = ba.readInt();
//			int cols = ba.readInt();
//			string[] fieldNames=null;
//		}
//		load.loadedNum++;
//		//Debug.Log("filename:"+filename);
//	}

	public static IEnumerator downLoadFile(string remotePath,string localPath,LoadingControl lc,bool setWWW)
	{
		lc.curDownRemotePath=remotePath;
		lc.curDownLocalPath=localPath;
		
		WWW www=new WWW(remotePath);
		if(setWWW)
		{
			lc.setWWW(www);
		}
		yield return www;
		if(www.error!=null)
		{
			Debug.Log(www.error);
			lc.downOver(false);
		}
		else
		{
			byte[] b = www.bytes;
			File.Delete(localPath);
			FileStream fs=new FileStream(localPath,FileMode.Create);
			fs.Seek(0,SeekOrigin.Begin);
			fs.Write(b,0,b.Length);
			fs.Close();
			lc.downOver(true);
		}
	}
	
	public static List<string> readResManager(string filePath)
	{
		List<string> resmanagers=new List<string>();
		if(!File.Exists(filePath))
		{
			return resmanagers;
		}
		FileStream fs=new FileStream(filePath,FileMode.Open);
		byte[] bs=new byte[fs.Length];
		fs.Seek(0,SeekOrigin.Begin);
		fs.Read(bs,0,bs.Length);
		fs.Close();
		ByteArray ba = new ByteArray(bs);
		int rows = ba.readInt();
		ba.readInt();
		for(int i=0;i<rows;i++)
		{
			string fileName=ba.readUTF();
			string fileVersion=ba.readUTF();
			string fileSize=ba.readUTF();
			resmanagers.Add(fileName+"-"+fileVersion+"-"+fileSize);
		}
		return resmanagers;
	}
	
	public static long getFileSize(string filePath)
	{
		if(!File.Exists(filePath))
		{
			return 0;
		}
		long size=0;
		FileInfo fi=new FileInfo(filePath);
		size=fi.Length;
		return size;
	}
	
	public static void readFile1(string filename,Type type)
	{
		FileStream fs=new FileStream(filename,FileMode.Open);
		byte[] bs=new byte[fs.Length];
		fs.Seek(0,SeekOrigin.Begin);
		fs.Read(bs,0,bs.Length);
		fs.Close();
		//解密
		byte[] b2 = new byte[bs.Length];
		for(int k=0;k<b2.Length;k++)
		{
			b2[k] = (byte)(~(bs[k]-8) & 0xFF);
		}
		ByteArray ba = new ByteArray(b2);
		int rows = ba.readInt();
		int cols = ba.readInt();
		string[] fieldNames=null;
		for(int i=0;i<rows;i++)
		{
			string[] strs = new string[cols];
			for(int j=0;j<cols;j++)
			{
				strs[j] = ba.readUTF();
			}
			if(strs[0].Trim().Length==0)
			{
				break;
			}
			else if(strs[0][0] == CHAR_COMMENT_START)
			{
				continue;
			} 
			else if(strs[0][0] == CHAR_VARS_START)
			{
				strs[0]=strs[0].Substring(1);
				fieldNames=strs;
			}
			else 
			{
			    object	obj = Activator.CreateInstance(type);
				PropertyReader pro=(PropertyReader)obj;
				for(int m=0;m<fieldNames.Length;m++)
				{
					PropertyInfo field=type.GetProperty(fieldNames[m]);
					if(field == null)
					{
						Debug.Log("fieldNames[m]:" + fieldNames[m]);
						Debug.Log("curRow:" + i);
						Debug.Log("m:" + m);
						Debug.Log("-------------------------------:"+filename);
					}
					if(field.PropertyType==typeof(int))
					{
						field.SetValue(pro,StringUtil.getInt(strs[m],i,m),null);
					}
					else if(field.PropertyType==typeof(float))
					{
						field.SetValue(pro,StringUtil.getFloat(strs[m]),null);
					}
					else if(field.PropertyType==typeof(double))
					{
						field.SetValue(pro,StringUtil.getDouble(strs[m]),null);
					}
					else
					{
						field.SetValue(pro,strs[m],null);
					}
				}
				pro.addData();
			}
		}
	}
	
	public static void readFile2(string filename,Type type)
	{
		FileStream fs=new FileStream(filename,FileMode.Open);
		byte[] bs=new byte[fs.Length];
		fs.Seek(0,SeekOrigin.Begin);
		fs.Read(bs,0,bs.Length);
		fs.Close();
		//解密
		byte[] b2 = new byte[bs.Length];
		for(int k=0;k<b2.Length;k++)
		{
			b2[k] = (byte)(~(bs[k]-8) & 0xFF);
		}
		ByteArray ba = new ByteArray(b2);
		int rows = ba.readInt();
		int cols = ba.readInt();	
		for(int i=0;i<rows;i++)
		{
			string[] strs = new string[cols];
			for(int j=0;j<cols;j++)
			{
				strs[j] = ba.readUTF();
			}
			if(strs[0].Trim().Length==0)
			{
				break;
			}
			else if(strs[0][0] == CHAR_COMMENT_START)
			{
				continue;
			} 
			else if(strs[0][0] == CHAR_VARS_START)
			{
				continue;
			} 
			else 
			{
				object obj = Activator.CreateInstance(type); 
				((PropertyReader)obj).parse(strs);
			}
		}
	}
	
	public static void copyFolder(string sourceFolderName,List<string> fileNames,string desFolderName)
	{
		if(!Directory.Exists(desFolderName))
		{
			Directory.CreateDirectory(desFolderName);
		}
		string[] files=Directory.GetFiles(sourceFolderName);
		foreach(string file in files)
		{
			string fileName=Path.GetFileName(file);
			if(fileNames.Contains(fileName))
			{
				File.Copy(file,desFolderName+fileName,true);
				Debug.Log(fileName);
			}
		}
	}
	
	//==请注意在Android中,这些文件被打包成一个.jar形式（也是标准的zip格式文件）的压缩包.这意味着如果你不使用Unity的WWW类来获取文件,就必须要用额外的软件来打开.jar文件并获取资源==//
	public static void copyFolderForAndroid(string sourceFolderName,List<string> fileNames,string desFolderName)
	{
		if(!Directory.Exists(desFolderName))
		{
			Directory.CreateDirectory(desFolderName);
		}
		foreach(string fileName in fileNames)
		{
			WWW www=new WWW(sourceFolderName+fileName);
			while(!www.isDone)
			{
				Thread.Sleep(1);
			}
			if(www.error==null)
			{
				byte[] b = www.bytes;
				File.Delete(desFolderName+fileName);
				FileStream fs=new FileStream(desFolderName+fileName,FileMode.Create);
				fs.Seek(0,SeekOrigin.Begin);
				fs.Write(b,0,b.Length);
				fs.Close();
			}
			else
			{
				Debug.LogError("error ===== " + www.error);
			}
		}
	}
}