using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static  class ExtensionMethodsPackt 
{
	/**
	 * 写入字典集合
	 * @param mParms
	 */
	public static void putMap(this ZMNetData my, Dictionary<object, object> mParms)
	{
		
		if(mParms!=null)
		{
			my.writeInt(mParms.Count);
			if(mParms.Count>0)
			{
				foreach(KeyValuePair<object,object> item in mParms)
				{
					my.putobject(item.Key);
					my.putobject(item.Value);
				}
			}
		}
		else
		{
			my.writeInt(0);
		}
	}
    //重载+号操作符，数据可以拼接
    public static byte[] Add(this byte[] d1, byte[] d2)
    {
        byte[] bytes = new byte[d1.Length + d2.Length];
        Array.Copy(d1, 0, bytes, 0, d1.Length);
        Array.Copy(d2, 0, bytes, d1.Length, d2.Length);
    //    Debug.Log("------d1.len------"+d1.Length+"------d2.len----------"+d2.Length+"---------retu.len:"+bytes.Length);

        return bytes;
    }
	
	
	/**
	 * 读取字典集合
	 * @return
	 */
	public static Dictionary<object, object> getMap(this ZMNetData my)
	{
		
		Dictionary< object, object> parms=new Dictionary<object, object>();
		int count=my.readInt();
		int num=1;
		if(count>0)
		{
			for(int i=0;i<count;i++)
			{

				object getKey=my.getobject();
				object getValue=my.getobject();
				//Debug.Log (string.Format("------------GetDic:{0},{1}",getKey,getValue));
				num++;
				parms[getKey]=getValue;
			}
		}
		return parms;
		
	}


	/**
	 * 写入字典集合
	 * @param mParms
	 */
	public static void putMapBO(this ZMNetData my,Dictionary<short, object> mParms)
	{
		Dictionary<object, object> changeDicHashMap=new Dictionary<object, object>();
		if(mParms!=null)
		{


			short[] keys=new short[mParms.Count];
			object[] values=new object[mParms.Count];

			mParms.Keys.CopyTo (keys,0);
			mParms.Values.CopyTo (values,0);

			for(int i=0;i<mParms.Count;i++)
			{
				changeDicHashMap[keys[i] as object]=values[i];
			}
//			foreach(KeyValuePair<short,object> item in mParms)
//			{
//				Profiler.BeginSample ("MapBO333 ");
//				changeDicHashMap[item.Key as object]=item.Value;
//				Profiler.EndSample ();
//			}
			my.putMap(changeDicHashMap);

		}
		else
		{
			my.putMap(changeDicHashMap);
		}
	}
	
	/**
	 * 读取字典集合
	 * @return
	 */
	public static Dictionary<short, object> getMapBO(this ZMNetData my)
	{
		Dictionary<object, object> changeDicHashMap=my.getMap();
		Dictionary<short, object> returnDicHashMap=new Dictionary<short, object>();
		int num=1;
		foreach(KeyValuePair<object,object> item in changeDicHashMap)
		{

			object getObj=item.Key;
			//Debug.Log (string.Format("----------{0},{1},{2},{3}",getObj.GetType().ToString(),getObj,num,changeDicHashMap.Count));
			num++;
			try
			{
				if(getObj is short)
				{
					if(!returnDicHashMap.ContainsKey((short)getObj))
					{
						returnDicHashMap[(short)getObj]=item.Value;
					}
				}
				else if(getObj is int)
				{
					if(!returnDicHashMap.ContainsKey((short)((int)getObj)))
					{
						returnDicHashMap[(short)((int)getObj)]=item.Value;
					}
				}

			}
			catch(System.Exception ex)
			{
				Debug.LogWarning (ex.ToString ());
			}
		}
		return returnDicHashMap;
	}

	/**
	 * 写入单精度浮点数组
	 * @param value
	 */
	public static void putFloats(this ZMNetData my,float[] value){

		my.writeInt(value.Length);
		for(int i=0;i<value.Length;i++){
			my.putFloat(value[i]);
		}
	}
	
	/**
	 * 读取单精度浮点数组
	 * @return
	 */
	public static float[] getFloats( this ZMNetData my) {
		int len = my.readInt();
		float[] ret = new float[len];
		for (int i = 0; i < len; i++) {
			ret[i] =my.getFloat();
		}
		return ret;
	}
	
	
	
	/**
	 * 读取object
	 * @return
	 */
	public static object getobject(this ZMNetData my)
	{
		int type=my.readInt();//取出数据类型
		//Debug.Log ("GetObject:"+type);
		object getObj=null;
		switch (type)
		{
		case PramsType.Int:
			getObj=my.readInt();
			break;
		case PramsType.String:
			getObj=my.readString();
			break;
		case PramsType.Float:
			getObj=my.getFloat();
			break;
		case PramsType.Short:
			getObj=my.readShort();
			break;
		case PramsType.Byte:
			getObj=my.readByte();
			break;
		case PramsType.ByteArray:
			getObj=my.readBytes();
			break;
		case PramsType.IntArray:
			getObj=my.readInts();
			break;
		case PramsType.StringArray:
			getObj=my.getStrings();
			break;
		case PramsType.FloatArray:
			getObj=my.getFloats();
			break;
		case PramsType.Dictionary:
			getObj=my.getMap();
			break;
        case PramsType.Bool:
            getObj = my.readBoolean();
            break;
        case PramsType.Long:
            getObj = my.readLong();
            break;
		}
		
		return getObj;
	}
	
	
	public static void putobject(this ZMNetData my, object mObj)
	{
		int type=GetType(mObj);//检测数据类型
		my.writeInt(type);//写入数据类型
		switch (type)
		{
		case PramsType.Int:
			my.writeInt((int)mObj);
			break;
		case PramsType.String:
			my.writeString((string)mObj);
			break;
		case PramsType.Float:
			my.putFloat((float)mObj);
			break;
		case PramsType.Short:
			my.writeShort((short)mObj);
			break;
		case PramsType.Byte:
			my.writeByte((byte)mObj);
			break;
		case PramsType.ByteArray:
			my.writeBytes((byte[])mObj);
			break;
		case PramsType.IntArray:
			my.writeInts((int[])mObj);
			break;
		case PramsType.StringArray:
			my.putStrings((string[])mObj);
			break;
		case PramsType.FloatArray:
			my.putFloats((float[])mObj);
			break;
		case PramsType.Dictionary:
			my.putMap((Dictionary<object,object>)mObj);
			break;
        case PramsType.Bool:
            my.writeBoolean((bool)mObj);
            break;
        case PramsType.Long:
            my.writeLong((long)mObj);
            break;
		}
	}
	
	public static void putStringsAndType(this ZMNetData my, string[] s)
	{
		my.writeByte(PramsType.StringArray);//写入数据类型
		my.putStrings(s);
	}
	
	public static Dictionary<K,V> DicObjTo<K,V>(this Dictionary<object,object> mDic)
	{
		Dictionary<K,V> getDic=new Dictionary<K, V>();
		foreach(KeyValuePair<object,object> item in mDic)
		{
			getDic[(K)item.Key]=(V)item.Value;
		}
		return getDic;
	}
	
	
	//     [Ljava.lang.String String[]
	//		[I int[]
	//		[C char[]
	/**
	 * 获取object拆箱类型
	 * @param mObj
	 * @return
	 */
	public static int GetType(object mObj)
	{
		//String classNameString = mObj.getClass().getName();
		byte mPramsType=PramsType.None;
		if(mObj is int)
		{
			mPramsType=PramsType.Int;
		}
		else if (mObj is string)
		{
			mPramsType=PramsType.String;
		}
		else if (mObj is short)
		{
			mPramsType=PramsType.Short;
		}
		else if (mObj is float)
		{
			mPramsType=PramsType.Float;
		}
		else if (mObj is byte)
		{
			mPramsType=PramsType.Byte;
		}
		else if (mObj is byte[])
		{
			mPramsType=PramsType.ByteArray;
		}
		else if (mObj is int[])
		{
			mPramsType=PramsType.IntArray;
		}
		else if (mObj is string[])
		{
			mPramsType=PramsType.StringArray;
		}
		else if (mObj is float[])
		{
			mPramsType=PramsType.FloatArray;
		}
		else if (mObj is Dictionary<object,object>)
		{
			mPramsType=PramsType.Dictionary;
		}
        else if (mObj is bool)
        {
            mPramsType = PramsType.Bool;
        }
        else if (mObj is long)
        {
            mPramsType = PramsType.Long;
        }
		
		return mPramsType;
	}

	public static Dictionary<byte,object> ToByteObject(this Dictionary<short,object> my)
	{
		Dictionary<byte,object> getParm=new Dictionary<byte, object>();
		foreach(KeyValuePair<short,object> item in my)
		{
			getParm[(byte)item.Key]=item.Value;
		}
		return getParm;
	}

	/// <summary>
	/// 从特定字典集合组合表和字典参数
	/// </summary>
	/// <param name="dic"></param>
	public static void CopyToDictionaryAndParms(this yuan.YuanMemoryDB.YuanTable my,Dictionary<short, object> dic)
	{
		if (dic.Count > 0)
		{
			
			
			my.Refresh();
			my.DeleteRows.Clear();
			my.Clear();
			
			
			
			
			foreach (KeyValuePair<short, object> item in dic)
			{
				
				if (item.Key == 0)
				{
					Dictionary<string, string> tempDic = ((Dictionary<object	, object>)item.Value).DicObjTo<string,string>();
					my.TableName = tempDic["TableName"];
					my.TableKey = tempDic["TableKey"];
				}
				else if (item.Key == 1)
				{
					my.mParms = ((Dictionary<object, object>)item.Value).DicObjTo<short,object>().ToByteObject();
				}
				else
				{
					
					
					string[] listDc = (string[])item.Value;
					int dcNum = 0;
					string dcStr = string.Empty;
					foreach (string dc in listDc)
					{
						if (dcNum == 0)
						{
							dcStr = dc;
						}
						else
						{
							if (my.Count < dcNum)
							{
								yuan.YuanMemoryDB.YuanRow tempRow = new yuan.YuanMemoryDB.YuanRow();
								tempRow.Add(dcStr, dc);
								my.Rows.Add(tempRow);
							}
							else
							{
								my.Rows[dcNum - 1].Add(dcStr, dc);
							}
						}
						dcNum++;
					}
				}
			}
		}
	}

}
