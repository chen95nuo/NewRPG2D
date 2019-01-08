using UnityEngine;
using System.Collections;
using System.Text;

public class GetRandomName : MonoBehaviour {
	
	/// <summary>
	/// 女性名称
	/// </summary>
	public YuanLocalization femaleFirstName;
	/// <summary>
	/// 女性姓氏
	/// </summary>
	public YuanLocalization femaleLastName;
	/// <summary>
	/// 男性名称
	/// </summary>
	public YuanLocalization maleFirstName;
	/// <summary>
	/// 男性姓氏
	/// </summary>
	public YuanLocalization maleLastName;
	/// <summary>
	/// 屏蔽词
	/// </summary>
	public YuanLocalization shieldedWord;
	
	/// <summary>
	/// 游戏所有字符
	/// </summary>
	//public YuanLocalization filterName;
	
	public static string[] shieldedWordStatic;
	void Awake()
	{
		shieldedWordStatic=shieldedWord.myStr;
	}

	/// <summary>
	/// 检查是否有敏感词汇
	/// </summary>
	/// <returns><c>true</c> if has shielded word the specified mText; otherwise, <c>false</c>.</returns>
	/// <param name="mText">M text.</param>
	public static bool HasShieldedWord(string mText)
	{
		for(int i=0;i<shieldedWordStatic.Length;i++)
		{
			if(mText.IndexOf(shieldedWordStatic[i])!=-1)
			{
				return true;
			}
		}
		return false;
	}

	
	
	private System.Random ran=new System.Random((int)System.DateTime.Now.Ticks);
	
	private int numFirstName;
	private int numLastName;
	/// <summary>
	/// 获取女性名字
	/// </summary>
	/// <returns>
	/// The female name.
	/// </returns>
	public string GetFemaleName()
	{
		numFirstName=ran.Next (0,femaleFirstName.myStr.Length);
		numLastName=ran.Next (0,femaleLastName.myStr.Length);
		return string.Format ("{0}{2}{1}",femaleFirstName.myStr[numFirstName],femaleLastName.myStr[numLastName],StaticLoc.Loc.Get("info750"));
	}
	
	
	/// <summary>
	/// 获取男性名字
	/// </summary>
	/// <returns>
	/// The male name.
	/// </returns>
	public string GetMaleName()
	{
		numFirstName=ran.Next (0,maleFirstName.myStr.Length);
		numLastName=ran.Next (0,maleLastName.myStr.Length);
		return string.Format ("{0}{2}{1}",maleFirstName.myStr[numFirstName],maleLastName.myStr[numLastName],StaticLoc.Loc.Get("info750"));
	}
	
	
	StringBuilder sb=new StringBuilder();
	/// <summary>
	/// 过滤字库中没有的字
	/// </summary>
	/// <returns>
	/// The no string.
	/// </returns>
	/// <param name='mStr'>
	/// M string.
	/// </param>
//	public string GetNoString(string mStr)
//	{
//		sb.Length=0;
//		foreach(char item in mStr)
//		{
//			foreach(string strItem in filterName.myStr)
//			{
//				if(strItem==item.ToString ())
//				{
//					sb.Append (item);
//					break;
//				}
//			}
//		}
//		
//		return sb.ToString ();
//	}

}
