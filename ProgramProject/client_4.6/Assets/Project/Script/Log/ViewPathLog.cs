using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ViewPathLog{
	
	//private string path="D:/unityLog/viewpath.bin";
	private FileStream fs;
	
	private static ViewPathLog instance;
	
	public static ViewPathLog getInstance()
	{
		if(instance==null)
		{
			instance=new ViewPathLog();
		}
		return instance;
	}
	
	private ViewPathLog()
	{
		return;
		////string path=System.Environment.CurrentDirectory;
		//string directory="D:/unityLog";
		//if(!Directory.Exists(directory))
		//{
		//	Directory.CreateDirectory(directory);
		//}
		//if(File.Exists(path))
		//{
		//	File.Delete(path);
		//}
		//if(!File.Exists(path))
		//{
		//	fs=File.Create(path);
		//}
	}
	
	public void writeHead(int colNum,int rowNum)
	{
		return ;
		//writeInt(rowNum);
		//writeInt(colNum);
		//writeUTF(";pathName");
		//writeUTF("size");
		//for(int i=0;i<colNum-2;i++)
		//{
		//	writeUTF("");
		//}
	}
	
	public void writeUTF(string info)
	{
		return;
		//if(fs==null)
		//{
		//	return;
		//}
		//ByteArray ba=new ByteArray(0);
		//ba.writeUTF(info);
		//byte[] bytes=ba.toArray();
		//fs.Write(bytes,0,bytes.Length);
	}
	
	public void writeInt(int info)
	{
		return;
		//if(fs==null)
		//{
		//	return;
		//}
		//ByteArray ba=new ByteArray(0);
		//ba.writeInt(info);
		//byte[] bytes=ba.toArray();
		//fs.Write(bytes,0,bytes.Length);
	}
	
	public void close()
	{
		return;
		//if(fs!=null)
		//{
		//	fs.Close();
		//	fs=null;
		//}
	}
	
	public void reset()
	{
		return;
		//instance=new ViewPathLog();
	}
	
}
