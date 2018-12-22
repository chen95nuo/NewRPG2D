using UnityEngine;
using System.Collections.Generic;

public class MailUIResultJson : ErrorJson 
{
	//==未读邮件个数==//
	public int ur;
	//==邮件标题,发件人,发件时间(yyyy-MM-dd HH:ss),是否新邮件(2未读邮件,0已读邮件)==//
	public List<MailUIResultElement> ms;
	
	public void sort()
	{
		if(ms==null || ms.Count==0)
		{
			return;
		}
		List<MailUIResultElement> result1=new List<MailUIResultElement>();
		List<MailUIResultElement> result2=new List<MailUIResultElement>();
		for(int i=0;i<ms.Count;i++)
		{
			MailUIResultElement s=ms[i];
			s.index=i;
			if(s.mark==2)
			{
				result1.Add(s);
			}
			else
			{
				result2.Add(s);
			}
		}
		ms.Clear();
		ms.AddRange(result1);
		ms.AddRange(result2);
	}
}
