using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapStructData : PropertyReader {
	
	public string type{get;set;}
	public List<string> elements{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public static float ScreenWidth=960f;
	public static float ScreenNum=8.26f;
	
	public static float nearWidth;
	public static float middleWidth;
	public static float remoteWidth;
	
	public static float nearFactor;
	public static float middleFactor;
	public static float remoteFactor;
	public static float[] moveSteps;
	
	public void addData()
	{
		data.Add(type,this);
		if(data.Count==23)
		{
			nearWidth=getAllWidth(1);
			middleWidth=getAllWidth(2);
			remoteWidth=getAllWidth(3);
			
			ScreenNum=middleWidth/ScreenWidth;
			
			nearWidth=ScreenWidth*ScreenNum;
			middleWidth=ScreenWidth*ScreenNum;
			remoteWidth=ScreenWidth*ScreenNum;
			
			nearFactor=(nearWidth-ScreenWidth)/(ScreenWidth*(ScreenNum-1));
			middleFactor=(middleWidth-ScreenWidth)/(ScreenWidth*(ScreenNum-1));
			remoteFactor=(remoteWidth-ScreenWidth)/(ScreenWidth*(ScreenNum-1));
			
			
			
			moveSteps=new float[3];
			moveSteps[0]=ScreenWidth-(middleWidth-nearWidth)/(ScreenNum-1);
			moveSteps[1]=ScreenWidth;
			moveSteps[2]=ScreenWidth-(middleWidth-remoteWidth)/(ScreenNum-1);
		}
	}
	
	public void resetData()
	{}
	
	public void parse(string[] ss)
	{
		int location=0;
		type=ss[location++];
		int length=ss.Length-1;
		elements=new List<string>();
		for(int i=0;i<length;i++)
		{
			if(string.IsNullOrEmpty(ss[location]))
			{
				continue;
			}
			elements.Add(ss[location++]);
		}
		addData();
	}
	
	public static MapStructData getData(string type)
	{
		return (MapStructData)data[type];
	}
	
	private static int getAllWidth(int type)
	{
		int maxWidth=0;
		MapStructData msd=getData(type+"");
		foreach(string element in msd.elements)
		{
			maxWidth+=StringUtil.getInt(getData(element).elements[0]);
		}
		return maxWidth;
	}
	
	public static int getWidth(string elementName)
	{
		return StringUtil.getInt(((MapStructData)data[elementName]).elements[0]);
	}
	
	public static int getHeight(string elementName)
	{
		return StringUtil.getInt(((MapStructData)data[elementName]).elements[1]);
	}
}