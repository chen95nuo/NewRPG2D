using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;

public class StringUtil
{
	
	//==获取时间戳(毫秒级),str格式:yyyy-MM-dd HH:mm:ss==//
	public static long getTimeStamp(string str)
	{
		DateTime date=DateTime.Parse(str);
		return date.Ticks/10000;
	}
	
	public static string getString(string str) {
		if (str == null)
			return "";
		return str.Trim();
	}
	
	public static float getFloat(string str) {
		if (str == null || ""==str.Trim())
			return 0;
		return float.Parse(str.Trim(),System.Globalization.NumberStyles.Float);
	}
	
	public static double getDouble(string str) {
		if (str == null || ""==str.Trim())
			return 0;
		return double.Parse(str.Trim(),System.Globalization.NumberStyles.Float);
	}
		
	public static int getInt(string str,int row = 0,int col = 0) {
		//string msg = "row : " + row.ToString() + " col : " + col.ToString();
		//try
		{
			if (str == null || ""==str.Trim()) {
				return 0;
			}
			return int.Parse(str.Trim(), System.Globalization.NumberStyles.Integer);
		}
		//catch(Exception e)
		//{
		//	Debug.Log("msg: " + msg);
		//	Debug.Log("str :"+str);
		//}
		//return 0;
	}
	
	public static int getIntFromHexStr(string str) {
		if (str == null || ""==str.Trim()) {
			return 0;
		}
		return int.Parse(str.Trim(), System.Globalization.NumberStyles.HexNumber);
	}
	
	/**
	 * gzip压缩
	 * lt@2013-12-16 下午04:48:05
	 * @param str
	 * @return
	 * @throws IOException
	 */
	public static string compressByGZIP(string str)
	{
		if (string.IsNullOrEmpty(str)) 
		{
			return str;
		}
		MemoryStream ms = new MemoryStream();
        GZipOutputStream gzip = new GZipOutputStream(ms);
        byte[] binary = Encoding.UTF8.GetBytes(str);
        gzip.Write(binary, 0, binary.Length);
		gzip.Finish();
		gzip.Flush();
        gzip.Close();
        byte[] press = ms.ToArray();
		
		return Encoding.GetEncoding("ISO-8859-1").GetString(press);
		
		//return Convert.ToBase64String(press); 
	}
	
	/**
	 * gzip解压缩
	 * lt@2013-12-16 下午04:46:50
	 * @param str
	 * @return
	 * @throws IOException
	 */
	public static string uncompressByGZIP(string str)
	{
		if (string.IsNullOrEmpty(str)) 
		{
			return null;
		}
		byte[] press=Encoding.GetEncoding("ISO-8859-1").GetBytes(str);
		
		//byte[] press = Convert.FromBase64String(str);
		
		GZipInputStream gzi = new GZipInputStream(new MemoryStream(press));
        MemoryStream re = new MemoryStream();
        int count=0;
        byte[] data=new byte[256];
        while ((count = gzi.Read(data, 0, data.Length)) != 0)
        {
            re.Write(data,0,count);
        }
        byte[] depress = re.ToArray();
        return Encoding.UTF8.GetString(depress);
	}
	
}
