using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class EncryptAndDecipher{

	/**
	 * encrypt
	 * lt@2013-8-16 11:36:32
	 * @param userId
	 * @param msg 明文
	 * @return
	 */
	public static string encrypt(string key,string msg)
	{
		if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(msg))
		{
			return msg;
		}
		try
		{
			byte[] keyBytes=Encoding.UTF8.GetBytes(key);
			byte[] msgBytes=Encoding.UTF8.GetBytes(msg);
			byte[] ciperBytes=new byte[msgBytes.Length];
			for(int i=0;i<msgBytes.Length;i++)
			{
				byte temp=keyBytes[i%keyBytes.Length];
				if(i%2==0)
				{
					ciperBytes[i]=(byte)(msgBytes[i]+temp);
				}
				else
				{
					ciperBytes[i]=(byte)(msgBytes[i]-temp);
				}
			}
			return byte2hex(ciperBytes);
		}
		catch (Exception e)
		{
			Debug.LogError(e);
		}
		return null;
	}
	
	/**
	 * 解密
	 * lt@2013-8-15 下午08:38:07
	 * @param userId
	 * @param decipherMsg 密文
	 * @return
	 */
	public static string decipher(string key,string decipherMsg)
	{
		if(key==null || "".Equals(key) || decipherMsg==null)
		{
			return decipherMsg;
		}
		try
		{
			byte[] keyBytes=Encoding.UTF8.GetBytes(key);
			byte[] decipherBytes=hex2byte(decipherMsg);
			byte[] msgBytes=new byte[decipherBytes.Length];
			for(int i=0;i<decipherBytes.Length;i++)
			{
				byte temp=keyBytes[i%keyBytes.Length];
				if(i%2==0)
				{
					msgBytes[i]=(byte)(decipherBytes[i]-temp);
				}
				else
				{
					msgBytes[i]=(byte)(decipherBytes[i]+temp);
				}
			}
			return Encoding.UTF8.GetString(msgBytes);
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
		return null;
	}
	
	/**
	 * 将二进制字节数组转化为十六进制字符串
	 * @param b 二进制字节数组
	 * @return String
	 */
	private static string byte2hex(byte[] b) 
	{
		string result="";
		for (int i=0;i<b.Length;i++) 
		{
			string temp=(b[i]&0xff).ToString("X");
			if(temp.Length==1) 
			{
				temp="0"+temp;
			} 
			result=result+temp;
		}
		return result;
	}
	
	/**
	 * 十六进制字符串转化为二进制字节数组
	 * @param hex
	 * @return
	 */
	private static byte[] hex2byte(string hex)
	{
		byte[] result=new byte[hex.Length/2];
		for (int i=0;i<result.Length;i++) 
		{
			string temp=hex[i*2]+""+hex[i*2+1];
			result[i]=(byte)StringUtil.getIntFromHexStr(temp);
		}
		return result;
	}
	
	/**
	 * encrypt
	 * lt@2013-8-16 11:36:32
	 * @param userId
	 * @param msg 明文
	 * @return
	 */
	public static string encrypt2(string key,string msg)
	{
		if(key==null || "".Equals(key) || msg==null)
		{
			return msg;
		}
		if(key==null)
		{
			key="";
		}
		string ciper="";
		for(int i=0;i<msg.Length;i++)
		{
			char temp=key[i%key.Length];
			if(i%2==0)
			{
				ciper+=(char)(msg[i]+temp);
			}
			else
			{
				ciper+=(char)(msg[i]-temp);
			}
		}
		return ciper;
	}
	
	/**
	 * 解密
	 * lt@2013-8-15 下午08:38:07
	 * @param userId
	 * @param decipherMsg 密文
	 * @return
	 */
	public static string decipher2(string key,string ciper)
	{
		if(key==null || "".Equals(key) || ciper==null)
		{
			return ciper;
		}
		string msg="";
		for(int i=0;i<ciper.Length;i++)
		{
			char temp=key[i%key.Length];
			if(i%2==0)
			{
				msg+=(char)(ciper[i]-temp);
			}
			else
			{
				msg+=(char)(ciper[i]+temp);
			}
		}
		return msg;
	}
}
